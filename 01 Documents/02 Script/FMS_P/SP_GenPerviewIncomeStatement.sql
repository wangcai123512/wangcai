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
-- Description:	��������Ԥ����
-- =============================================
CREATE PROCEDURE [dbo].[SP_GenPerviewIncomeStatement]
	@C_ID           VARCHAR(40),
	@report_date    VARCHAR(10),
	@period_type    VARCHAR(50),        --������������(�¶�=month,����=quarter,���=year)
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
	DECLARE     @quarter INT ;  --����
	
	DECLARE @report_fix   VARCHAR(10)
	SET @report_fix='PL'
    
    --���ú��� ��������
    SELECT @CurrentRepBeginDate= begin_date,
           @CurrentRepEndDate= end_date,
           @report_fix=report_fix
    FROM [F_ReportDateCalculate](@report_date,@period_type,@report_fix) 
	 
	SET @Year = DATEPART(YEAR,@CurrentRepBeginDate);
	
	--��ʼ��ģ���
    UPDATE dbo.T_IncomeStatementTemplate
    SET amount=NULL,amount_sum=NULL
    	
	--�������룬�ɱ���
	SELECT a.Profit_GUID,SUM(a.SumAmount)AS amount,MIN(b.AccCode)AS item_code
	INTO #temp_amount
	FROM dbo.T_IERecord AS a
	INNER JOIN dbo.T_GeneralLedgerAccount AS b ON a.Profit_GUID=b.LA_GUID
	WHERE a.C_GUID=@C_ID
	    AND a.AffirmDate BETWEEN @CurrentRepBeginDate AND @CurrentRepEndDate
	GROUP BY a.Profit_GUID
 
	--����ģ���е����룬�ɱ����
	UPDATE  a 
	SET a.amount=b.amount
	FROM  T_IncomeStatementTemplate AS a
	INNER JOIN #temp_amount AS b ON a.item_code=b.item_code
	
	--Ӫҵ��֧��
	DECLARE @temp   DECIMAL(18,4)
	
	SELECT @temp=SUM(Amount)
    FROM dbo.T_DeclareCostSpending
    WHERE InvType IN('Ԥ����Ӧ��','֧��Ѻ�����֧���','Ͷ��֧��')
        AND Record='δ��¼'
        AND Date BETWEEN @CurrentRepBeginDate AND @CurrentRepEndDate       
        AND  C_GUID=@C_ID
    

	UPDATE dbo.T_IncomeStatementTemplate
	SET amount =@temp
	WHERE item_code='7019'      --7019 Ӫҵ��֧��
	
	
	--������Ӫҵ������6610��=6001 ��Ӫҵ������-7004 ��Ӫҵ��ɱ�-7006 ��Ӫҵ��˰�𼰸���
    DECLARE @6001   DECIMAL(18,4)       --��Ӫҵ������
    DECLARE @7004   DECIMAL(18,4)       --��Ӫҵ��ɱ�
    DECLARE @7006   DECIMAL(18,4)       --��Ӫҵ��˰�𼰸���
    DECLARE @6610   DECIMAL(18,4)       --��Ӫҵ������

    SELECT @6001= ISNULL(amount,0)
    FROM dbo.T_IncomeStatementTemplate
    WHERE item_code='6001'
    
    SELECT @7004= ISNULL(amount,0)
    FROM dbo.T_IncomeStatementTemplate
    WHERE item_code='7004'
	
	SELECT @7006= ISNULL(amount,0)
    FROM dbo.T_IncomeStatementTemplate
    WHERE item_code='7006'
    
    --��Ӫҵ������
    SET @6610 = @6001-@7004-@7006
    
    UPDATE dbo.T_IncomeStatementTemplate
    SET amount= @6610
    WHERE item_code='6610'
    
    
    --6611 ����Ӫҵ����= @6610 ��Ӫҵ������+ 6600 ����ҵ������- 7017 Ӫҵ���� -7013 �������- 7014 �������- 7012 ���۷���
    DECLARE @6611   DECIMAL(18,4)       --Ӫҵ����
    DECLARE @6600   DECIMAL(18,4)       --����ҵ������
    DECLARE @7017   DECIMAL(18,4)       --Ӫҵ����
    DECLARE @7013   DECIMAL(18,4)       --�������
    DECLARE @7014   DECIMAL(18,4)       --�������
    DECLARE @7012   DECIMAL(18,4)       --���۷���
    
    --����ҵ������
    SELECT @6600= ISNULL(amount,0)
    FROM dbo.T_IncomeStatementTemplate
    WHERE item_code='6600'
    
    --Ӫҵ����
    SELECT @7017= ISNULL(amount,0)
    FROM dbo.T_IncomeStatementTemplate
    WHERE item_code='7017'
    
	--�������
	SELECT @7013= ISNULL(amount,0)
    FROM dbo.T_IncomeStatementTemplate
    WHERE item_code='7013'
    
    --�������
    SELECT @7014= ISNULL(amount,0)
    FROM dbo.T_IncomeStatementTemplate
    WHERE item_code='7014'
    
    --���۷���
    SELECT @7012= ISNULL(amount,0)
    FROM dbo.T_IncomeStatementTemplate
    WHERE item_code='7012'
    
    --@6611 Ӫҵ����= @6610 ��Ӫҵ������+ 6600 ����ҵ������- 7017 Ӫҵ���� -7013 �������- 7014 �������- 7012 ���۷���
    SET @6611=@6610+@6600-@7017-@7013-@7014-@7012
    
    UPDATE dbo.T_IncomeStatementTemplate
    SET amount= @6611
    WHERE item_code='6611' 
    
    -- ���� 6613 �����ܶ� =@6611 Ӫҵ���� + 6111 Ͷ������ +6051 ��������+  6301 Ӫҵ������- 7019 Ӫҵ��֧��  
    
    DECLARE @6613   DECIMAL(18,4)       --�����ܶ�
    DECLARE @6111   DECIMAL(18,4)       --Ͷ������
    DECLARE @6051   DECIMAL(18,4)       --��������
    DECLARE @6301   DECIMAL(18,4)       --Ӫҵ������
    DECLARE @7019   DECIMAL(18,4)       --Ӫҵ��֧�� 
    
     --Ͷ������
    SELECT @6111= ISNULL(amount,0)
    FROM dbo.T_IncomeStatementTemplate
    WHERE item_code='6111'
    
    --��������
    SELECT @6051= ISNULL(amount,0)
    FROM dbo.T_IncomeStatementTemplate
    WHERE item_code='6051'
    
	--Ӫҵ������
	SELECT @6301= ISNULL(amount,0)
    FROM dbo.T_IncomeStatementTemplate
    WHERE item_code='6301'
    
    --Ӫҵ��֧��
    SELECT @7019= ISNULL(amount,0)
    FROM dbo.T_IncomeStatementTemplate
    WHERE item_code='7019'
    
    --�����ܶ� =@6611 Ӫҵ���� + 6111 Ͷ������ +6051 ��������+  6301 Ӫҵ������- 7019 Ӫҵ��֧��  
    SET @6613 = @6611+ @6111 +@6051 +@6301 -@7019
    
    UPDATE dbo.T_IncomeStatementTemplate
    SET amount= @6613
    WHERE item_code='6613' 
    
    -- ���� ������ = @6613 �����ܶ� - @7020 ����˰- @8001 �����ɶ�Ȩ��
    DECLARE @7020   DECIMAL(18,4)       --����˰
    DECLARE @8001   DECIMAL(18,4)       --�����ɶ�Ȩ��
    
      --����˰
    SELECT @7020= ISNULL(amount,0)
    FROM dbo.T_IncomeStatementTemplate
    WHERE item_code='7020'
    
    --�����ɶ�Ȩ��
    SELECT @8001= ISNULL(amount,0)
    FROM dbo.T_IncomeStatementTemplate
    WHERE item_code='8001'
    
    -- ���� ������ 
    UPDATE dbo.T_IncomeStatementTemplate
    SET amount= @6613-@7020-@8001
    WHERE item_code='6614' 
    
    --��������ۼƽ��
    
    --��ѯ�������ʷ,���ܸ�����
    
    SELECT b.row_no,SUM(b.beginning_amount) AS his_sum_amount
    INTO #his_data
    FROM dbo.T_Report AS a
    INNER JOIN dbo.T_ReportDetails AS b ON a.Rep_GUID=b.rep_guid
    WHERE a.Type=@report_fix       --�����       
        AND a.period_type =period_type 
        AND a.rep_date LIKE CAST(@Year AS VARCHAR(4) )+'%'
    GROUP BY b.row_no    
   
    IF @@ROWCOUNT=0
    BEGIN
        --û����ʷ���� ���=����
        UPDATE a 
        SET a.amount_sum=a.amount
        FROM dbo.T_IncomeStatementTemplate AS a
        
    END
    ELSE
    BEGIN
        --���� ����ۼ�=��ʷ+����
         
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


