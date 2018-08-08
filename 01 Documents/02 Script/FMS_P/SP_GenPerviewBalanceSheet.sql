USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_GenPerviewBalanceSheet]    Script Date: 12/22/2016 09:57:14 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_GenPerviewBalanceSheet]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_GenPerviewBalanceSheet]
GO

USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_GenPerviewBalanceSheet]    Script Date: 12/22/2016 09:57:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	生成资产负债预览表
-- =============================================
CREATE PROCEDURE [dbo].[SP_GenPerviewBalanceSheet]
    @C_ID NVarChar(40),
    @report_date    VARCHAR(10),
	@period_type    VARCHAR(50),        --报表周期类型(月度=month,季度=quarter,年度=year)
    @RepNo	NVARCHAR(40) OUT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE     @Year       INT ;
    DECLARE     @Month      INT ;
	DECLARE     @CurrentRepBeginDate DATE ;
	DECLARE     @CurrentRepEndDate DATE ;
	DECLARE     @quarter INT ;  --季度
	
	DECLARE @report_fix   VARCHAR(10)
	SET @report_fix='BS'
    
    --调用函数 计算日期
    SELECT @CurrentRepBeginDate= begin_date,
           @CurrentRepEndDate= end_date,
           @report_fix=report_fix
    FROM [F_ReportDateCalculate](@report_date,@period_type,@report_fix) 
	 
	SET @Year = DATEPART(YEAR,@CurrentRepBeginDate);
	
	 
	
	--初始化资产负债表模板
	UPDATE dbo.T_BalanceSheetTemplate
	SET asset_start_amount=NULL,asset_end_amount=NULL,debt_start_amount=NULL,debt_end_amount=NULL
 
	
	---1. 统计IERecord的应收，应付(应收:IE_Flag=I,应付：IE_Flag=E)
	---2. 根据应收查询到对应的已收。
	---3. 应收-已收=本期应收	
	
	--应收
	DECLARE @Receivables    DECIMAL(18,4)       --本期应收总数
	DECLARE @rec_sum        DECIMAL(18,4)       --本期已收总数
	
	CREATE TABLE #IE_Temp
	(
	     IE_GUID            VARCHAR(50),
	     Profit_GUID        VARCHAR(50),
	     SumAmount          DECIMAL(18,4)
	)
	
	SET @Receivables=0  
	
	INSERT INTO #IE_Temp
	        ( IE_GUID, Profit_GUID, SumAmount ) 
	SELECT a.IE_GUID,a.Profit_GUID,a.SumAmount  
	FROM dbo.T_IERecord AS a
	WHERE IE_Flag='I'        --I=收入
	    AND State='应收'
	    AND a.C_GUID=@C_ID
	    AND a.AffirmDate BETWEEN @CurrentRepBeginDate AND @CurrentRepEndDate
	    
	  
	--应收总金额    
	SELECT @Receivables =SUM(a.SumAmount)
	FROM #IE_Temp AS a 
	
	-- 已收	
	SELECT @rec_sum= SUM(ISNULL(a.SumAmount,0))
	FROM dbo.T_RecPayRecord AS a
	INNER JOIN #IE_Temp AS b ON a.IE_GUID=b.IE_GUID 
	WHERE a.RP_Flag='R'
 
	-- 本期应收=应收总数-已收总数
	UPDATE T_BalanceSheetTemplate
	SET asset_end_amount = @Receivables-ISNULL(@rec_sum,0)
	WHERE asset_row_no=6   
	
	--计算应付账款
	TRUNCATE TABLE #IE_Temp
	
	INSERT INTO #IE_Temp
	        ( IE_GUID, Profit_GUID, SumAmount ) 
	SELECT a.IE_GUID,a.Profit_GUID,a.SumAmount	
	FROM dbo.T_IERecord AS a
	WHERE IE_Flag='E'        --I=应付
	    AND IEGroup <>'工资'
	    AND State='应付'
	    AND a.C_GUID=@C_ID
	    AND a.AffirmDate BETWEEN @CurrentRepBeginDate AND @CurrentRepEndDate
	    
	--应付总金额    
	SET @Receivables = 0;
	SET @rec_sum = 0;
	
	SELECT @Receivables =SUM(a.SumAmount)
	FROM #IE_Temp AS a 
	
	-- 已付	
	SELECT @rec_sum= SUM(ISNULL( a.SumAmount,0)) 	 
	FROM dbo.T_RecPayRecord AS a
	INNER JOIN #IE_Temp AS b ON a.IE_GUID=b.IE_GUID 
	WHERE a.RP_Flag='P'
 
	-- 本期应收=应收总数-已收总数
	UPDATE T_BalanceSheetTemplate
	SET debt_end_amount = @Receivables-ISNULL(@rec_sum,0)
	WHERE debt_row_no=70       -- 本期应付 
	
	--计算应付工资
	TRUNCATE TABLE #IE_Temp
	
	INSERT INTO #IE_Temp
	        ( IE_GUID, Profit_GUID, SumAmount ) 
	SELECT a.IE_GUID,a.Profit_GUID,a.SumAmount	
	FROM dbo.T_IERecord AS a
	WHERE IE_Flag='E'        --I=应付
	    AND IEGroup ='工资'
	    AND State='应付'
	    AND a.C_GUID=@C_ID
	    AND a.AffirmDate BETWEEN @CurrentRepBeginDate AND @CurrentRepEndDate
	    
	--应付总金额    
	SET @Receivables = 0;
	SET @rec_sum = 0;
	
	SELECT @Receivables =SUM(a.SumAmount)
	FROM #IE_Temp AS a 
	
	-- 已付	
	SELECT @rec_sum= SUM(ISNULL( a.SumAmount,0)) 	 
	FROM dbo.T_RecPayRecord AS a
	INNER JOIN #IE_Temp AS b ON a.IE_GUID=b.IE_GUID 
	WHERE a.RP_Flag='P'
 
	-- 本期应收=应收总数-已收总数
	UPDATE T_BalanceSheetTemplate
	SET debt_end_amount = @Receivables-ISNULL(@rec_sum,0)
	WHERE debt_row_no=72       -- 本期应付  
	 
	-- 
	TRUNCATE TABLE #IE_Temp
	
	INSERT INTO #IE_Temp
	(IE_GUID,Profit_GUID,SumAmount)
	SELECT a.IE_GUID,a.Profit_GUID,a.SumAmount  
	FROM dbo.T_IERecord AS a
	WHERE a.C_GUID=@C_ID
	    AND a.Date BETWEEN @CurrentRepBeginDate AND @CurrentRepEndDate
	 
	
	SELECT a.IE_GUID,SUM(a.SumAmount) AS paid_amount
	INTO #RecPay_Temp
	FROM dbo.T_RecPayRecord AS a
	INNER JOIN #IE_Temp AS b ON a.IE_GUID=b.IE_GUID 
	GROUP BY a.IE_GUID   
	
	
	SELECT a.IE_GUID,a.Profit_GUID,a.SumAmount-b.paid_amount AS amount 
	INTO #IE_List
	FROM #IE_Temp AS a
	LEFT JOIN   #RecPay_Temp AS b ON a.IE_GUID=b.IE_GUID
	
	
	
	DECLARE @current_asst_sum               DECIMAL(18,4)          --流动资产合计    
	DECLARE @long_terminvestments_sum       DECIMAL(18,4)          --长期投资合计    
	
	SET @current_asst_sum = 0
	SET @long_terminvestments_sum = 0
	
	--汇总资产负债表各个 项目的金额
	SELECT Profit_GUID,SUM( amount) amount,MIN(b.Name) name
	INTO #BalanceSheet
	FROM #IE_List  AS a
	INNER JOIN dbo.T_GeneralLedgerAccount AS b ON a.Profit_GUID=b.LA_GUID
	GROUP BY  Profit_GUID    
	

	
	 --固定资产原价
	DECLARE @asst_amount        DECIMAL(18,4)
	DECLARE @dis_amount         DECIMAL(18,4)   --折旧金额 
	DECLARE @dis_amount_1       DECIMAL(18,4)   --固定资产减值准备
	DECLARE @fix_asst_sum       DECIMAL(18,4)  
 
	SET @dis_amount=0    
	SET @dis_amount_1=0     --固定资产减值准备
		
	SELECT @asst_amount=SUM(a.Amount) 
    FROM dbo.T_AIDRecord AS a
    INNER JOIN T_AIDTypeRecord AS b ON a.InvType=b.AT_GUID 
    WHERE 
     b.Asset_class='固定资产'
    AND b.AID_FLAG='A'          --A=资产
    AND a.C_GUID= @C_ID
    AND a.Date BETWEEN @CurrentRepBeginDate AND @CurrentRepEndDate
    
    
    
    UPDATE T_BalanceSheetTemplate
	SET asset_end_amount=@asst_amount
	WHERE asset_row_no=39       --固定资产原价
	
	UPDATE T_BalanceSheetTemplate
	SET asset_end_amount= @asst_amount - @dis_amount
	WHERE asset_row_no=41       --固定资产净值
	
	UPDATE T_BalanceSheetTemplate
	SET asset_end_amount=@asst_amount - @dis_amount - @dis_amount_1
	WHERE asset_row_no=43       --固定资产净额     
	       
	
	-- 固定资产合计 =固定资产净额 +工程物资+在建工程+固定资产清理 （后3项无）	
	 
	SET @fix_asst_sum =@asst_amount -@dis_amount -@dis_amount_1
	
	UPDATE T_BalanceSheetTemplate
	SET asset_end_amount= @fix_asst_sum
	WHERE asset_row_no=50       --固定资产合计
	
	--无形资产
	SET @asst_amount = 0
	
	SELECT @asst_amount=SUM(a.Amount) 
    FROM dbo.T_AIDRecord AS a
    INNER JOIN T_AIDTypeRecord AS b ON a.InvType=b.AT_GUID 
    WHERE 
     b.Asset_class='无形资产'
    AND b.AID_FLAG='A'
    AND a.C_GUID= @C_ID
    AND a.Date BETWEEN @CurrentRepBeginDate AND @CurrentRepEndDate
    
    DECLARE @long_cost                  DECIMAL(18,4)       --长期待摊费用
	DECLARE @other_long_asst            DECIMAL(18,4)       --其他长期资产  
	DECLARE @temp_asst_sum               DECIMAL(18,4)       --无形资产及其他长期资产合计
	
	SET @long_cost=0
	SET @other_long_asst=0
	
    UPDATE T_BalanceSheetTemplate
    SET asset_end_amount=@asst_amount
    WHERE asset_row_no=51   --无形资产
    
    --无形资产及其他长期资产合计
    SET @temp_asst_sum = @asst_amount + @long_cost + @other_long_asst
    
    UPDATE T_BalanceSheetTemplate
    SET asset_end_amount = @temp_asst_sum
    WHERE asset_row_no=60   --无形资产及其他长期资产合计
    
    --资产合计= 流动资产合计+长期投资合计+固定资产合计+形资产及其他长期资产合计
    UPDATE T_BalanceSheetTemplate
    SET asset_end_amount = @current_asst_sum + @long_terminvestments_sum + @fix_asst_sum + @temp_asst_sum
    WHERE asset_row_no= 67   --资产合计
	
	
	--负债
 
	     
	----更新资产期末数
	--UPDATE a
	--SET a.asset_end_amount=b.amount
	--FROM T_BalanceSheetTemplate AS a
	--INNER JOIN #BalanceSheet AS b ON LTRIM(a.asset_item_name)=LTRIM(b.name)
	
	--更新负债期末数
	--UPDATE a
	--SET a.debt_end_amount=b.amount
	--FROM T_BalanceSheetTemplate AS a
	--INNER JOIN #BalanceSheet AS b ON LTRIM(a.debt_item_name)=LTRIM(b.name)
	
	
	--获取期初数
    SELECT a.Money,b.Name
    INTO #InitAmount
    FROM dbo.T_BeginningBalance AS a
    INNER JOIN dbo.T_GeneralLedgerAccount AS b ON a.Acc_GUID=b.LA_GUID
	WHERE a.C_GUID=@C_ID
	
	--计算银行账户的期初现金
	DECLARE @init_cash  DECIMAL(18,4)
	SET @init_cash=0
	
	SELECT @init_cash = SUM( CAST(Amount AS DECIMAL(18,4)))
	FROM dbo.T_BankAccount
	WHERE C_GUID=@C_ID
	
	UPDATE T_BalanceSheetTemplate
	SET asset_start_amount=@init_cash
	WHERE RTRIM(asset_item_name)='货币资金'
	
	
	--更新资产期初数
	--SELECT *
	UPDATE a
	SET a.asset_start_amount=b.money
	FROM T_BalanceSheetTemplate AS a
	INNER JOIN #InitAmount AS b ON RTRIM(a.asset_item_name)=b.Name
	
	--更新负债期初数+所有者权益
	--SELECT *
	UPDATE a
	SET a.debt_start_amount=b.money
	FROM T_BalanceSheetTemplate AS a
	INNER JOIN #InitAmount AS b ON RTRIM(a.debt_item_name)=b.Name

	--查询报表
	SELECT  id,asset_item_name ,asset_row_no , asset_start_amount ,asset_end_amount ,
	         debt_item_name ,[debt_row_no] ,debt_start_amount ,debt_end_amount 
	FROM T_BalanceSheetTemplate
	ORDER BY id
	 
    	
	DROP TABLE #InitAmount
    DROP TABLE #BalanceSheet
    DROP TABLE #IE_List
    DROP TABLE #IE_Temp
    DROP TABLE #RecPay_Temp  

	
	SET @RepNo =  @report_fix+ @report_date ;
	 
  
END

GO


