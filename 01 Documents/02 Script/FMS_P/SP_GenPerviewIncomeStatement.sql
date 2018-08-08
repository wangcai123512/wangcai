USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_GenPerviewIncomeStatement]    Script Date: 01/04/2017 10:33:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_GenPerviewIncomeStatement]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_GenPerviewIncomeStatement]
GO

USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_GenPerviewIncomeStatement]    Script Date: 01/04/2017 10:33:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	生成利润预览表
-- =============================================
CREATE PROCEDURE [dbo].[SP_GenPerviewIncomeStatement]
	@C_ID           VARCHAR(40),
	@report_date    VARCHAR(10),
	@period_type    VARCHAR(50),        --报表周期类型(月度=month,季度=quarter,年度=year)
    @RepNo	    VARCHAR(40) OUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    DECLARE     @Year       INT ;
    DECLARE     @Month      INT ;
	DECLARE     @CurrentRepBeginDate DATE ;
	DECLARE     @CurrentRepEndDate DATE ;
	DECLARE     @quarter INT ;  --季度
	
	DECLARE @report_fix   VARCHAR(10)
	SET @report_fix='PL'
    
    --调用函数 计算日期
    SELECT @CurrentRepBeginDate= begin_date,
           @CurrentRepEndDate= end_date,
           @report_fix=report_fix
    FROM [F_ReportDateCalculate](@report_date,@period_type,@report_fix) 
	 
	SET @Year = DATEPART(YEAR,@CurrentRepBeginDate);
	
	--初始化模板表
    UPDATE dbo.T_IncomeStatementTemplate
    SET amount=NULL,amount_sum=NULL
    	
	--汇总收入，成本表
	SELECT a.Profit_GUID,SUM(a.SumAmount)AS amount,MIN(b.AccCode)AS item_code
	INTO #temp_amount
	FROM dbo.T_IERecord AS a
	INNER JOIN dbo.T_GeneralLedgerAccount AS b ON a.Profit_GUID=b.LA_GUID
	WHERE a.C_GUID=@C_ID
	    AND a.AffirmDate BETWEEN @CurrentRepBeginDate AND @CurrentRepEndDate
	GROUP BY a.Profit_GUID
 
	--更新模板中的收入，成本金额
	UPDATE  a 
	SET a.amount=b.amount
	FROM  T_IncomeStatementTemplate AS a
	INNER JOIN #temp_amount AS b ON a.item_code=b.item_code
	
	--营业外支出
	DECLARE @temp   DECIMAL(18,4)
	
	SELECT @temp=SUM(Amount)
    FROM dbo.T_DeclareCostSpending
    WHERE InvType IN('预付供应商','支付押金和暂支借款','投资支出')
        AND Record='未记录'
        AND Date BETWEEN @CurrentRepBeginDate AND @CurrentRepEndDate       
        AND  C_GUID=@C_ID
    

	UPDATE dbo.T_IncomeStatementTemplate
	SET amount =@temp
	WHERE item_code='7019'      --7019 营业外支出
	
	
	--计算主营业务利润（6610）=6001 主营业务收入-7004 主营业务成本-7006 主营业务税金及附加
    DECLARE @6001   DECIMAL(18,4)       --主营业务收入
    DECLARE @7004   DECIMAL(18,4)       --主营业务成本
    DECLARE @7006   DECIMAL(18,4)       --主营业务税金及附加
    DECLARE @6610   DECIMAL(18,4)       --主营业务利润

    SELECT @6001= ISNULL(amount,0)
    FROM dbo.T_IncomeStatementTemplate
    WHERE item_code='6001'
    
    SELECT @7004= ISNULL(amount,0)
    FROM dbo.T_IncomeStatementTemplate
    WHERE item_code='7004'
	
	SELECT @7006= ISNULL(amount,0)
    FROM dbo.T_IncomeStatementTemplate
    WHERE item_code='7006'
    
    --主营业务利润
    SET @6610 = @6001-@7004-@7006
    
    UPDATE dbo.T_IncomeStatementTemplate
    SET amount= @6610
    WHERE item_code='6610'
    
    
    --6611 计算营业利润= @6610 主营业务利润+ 6600 其他业务利润- 7017 营业费用 -7013 管理费用- 7014 财务费用- 7012 销售费用
    DECLARE @6611   DECIMAL(18,4)       --营业利润
    DECLARE @6600   DECIMAL(18,4)       --其他业务利润
    DECLARE @7017   DECIMAL(18,4)       --营业费用
    DECLARE @7013   DECIMAL(18,4)       --管理费用
    DECLARE @7014   DECIMAL(18,4)       --财务费用
    DECLARE @7012   DECIMAL(18,4)       --销售费用
    
    --其他业务利润
    SELECT @6600= ISNULL(amount,0)
    FROM dbo.T_IncomeStatementTemplate
    WHERE item_code='6600'
    
    --营业费用
    SELECT @7017= ISNULL(amount,0)
    FROM dbo.T_IncomeStatementTemplate
    WHERE item_code='7017'
    
	--管理费用
	SELECT @7013= ISNULL(amount,0)
    FROM dbo.T_IncomeStatementTemplate
    WHERE item_code='7013'
    
    --财务费用
    SELECT @7014= ISNULL(amount,0)
    FROM dbo.T_IncomeStatementTemplate
    WHERE item_code='7014'
    
    --销售费用
    SELECT @7012= ISNULL(amount,0)
    FROM dbo.T_IncomeStatementTemplate
    WHERE item_code='7012'
    
    --@6611 营业利润= @6610 主营业务利润+ 6600 其他业务利润- 7017 营业费用 -7013 管理费用- 7014 财务费用- 7012 销售费用
    SET @6611=@6610+@6600-@7017-@7013-@7014-@7012
    
    UPDATE dbo.T_IncomeStatementTemplate
    SET amount= @6611
    WHERE item_code='6611' 
    
    -- 计算 6613 利润总额 =@6611 营业利润 + 6111 投资收益 +6051 补贴收入+  6301 营业外收入- 7019 营业外支出  
    
    DECLARE @6613   DECIMAL(18,4)       --利润总额
    DECLARE @6111   DECIMAL(18,4)       --投资收益
    DECLARE @6051   DECIMAL(18,4)       --补贴收入
    DECLARE @6301   DECIMAL(18,4)       --营业外收入
    DECLARE @7019   DECIMAL(18,4)       --营业外支出 
    
     --投资收益
    SELECT @6111= ISNULL(amount,0)
    FROM dbo.T_IncomeStatementTemplate
    WHERE item_code='6111'
    
    --补贴收入
    SELECT @6051= ISNULL(amount,0)
    FROM dbo.T_IncomeStatementTemplate
    WHERE item_code='6051'
    
	--营业外收入
	SELECT @6301= ISNULL(amount,0)
    FROM dbo.T_IncomeStatementTemplate
    WHERE item_code='6301'
    
    --营业外支出
    SELECT @7019= ISNULL(amount,0)
    FROM dbo.T_IncomeStatementTemplate
    WHERE item_code='7019'
    
    --利润总额 =@6611 营业利润 + 6111 投资收益 +6051 补贴收入+  6301 营业外收入- 7019 营业外支出  
    SET @6613 = @6611+ @6111 +@6051 +@6301 -@7019
    
    UPDATE dbo.T_IncomeStatementTemplate
    SET amount= @6613
    WHERE item_code='6613' 
    
    -- 计算 净利润 = @6613 利润总额 - @7020 所得税- @8001 少数股东权益
    DECLARE @7020   DECIMAL(18,4)       --所得税
    DECLARE @8001   DECIMAL(18,4)       --少数股东权益
    
      --所得税
    SELECT @7020= ISNULL(amount,0)
    FROM dbo.T_IncomeStatementTemplate
    WHERE item_code='7020'
    
    --少数股东权益
    SELECT @8001= ISNULL(amount,0)
    FROM dbo.T_IncomeStatementTemplate
    WHERE item_code='8001'
    
    -- 计算 净利润 
    UPDATE dbo.T_IncomeStatementTemplate
    SET amount= @6613-@7020-@8001
    WHERE item_code='6614' 
    
    --汇总年度累计金额
    
    --查询当年的历史,汇总各项金额
    
    SELECT b.row_no,SUM(b.beginning_amount) AS his_sum_amount
    INTO #his_data
    FROM dbo.T_Report AS a
    INNER JOIN dbo.T_ReportDetails AS b ON a.Rep_GUID=b.rep_guid
    WHERE a.Type=@report_fix       --利润表       
        AND a.period_type =period_type 
        AND a.rep_date LIKE CAST(@Year AS VARCHAR(4) )+'%'
    GROUP BY b.row_no    
   
    IF @@ROWCOUNT=0
    BEGIN
        --没有历史数据 年度=当期
        UPDATE a 
        SET a.amount_sum=a.amount
        FROM dbo.T_IncomeStatementTemplate AS a
        
    END
    ELSE
    BEGIN
        --更新 年度累计=历史+当期
         
        UPDATE a 
        SET a.amount_sum=a.amount+b.his_sum_amount
        FROM dbo.T_IncomeStatementTemplate AS a
        INNER JOIN #his_data AS b ON a.row_no=b.row_no
    END
    
    
    
	SELECT id ,item_name , amount ,amount_sum ,row_no
	FROM dbo.T_IncomeStatementTemplate 
	
	SET @RepNo = @report_fix+ @report_date ;
	
	 
END

GO


