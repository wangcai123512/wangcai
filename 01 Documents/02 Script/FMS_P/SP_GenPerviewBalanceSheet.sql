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
-- Description:	�����ʲ���ծԤ����
-- =============================================
CREATE PROCEDURE [dbo].[SP_GenPerviewBalanceSheet]
    @C_ID NVarChar(40),
    @report_date    VARCHAR(10),
	@period_type    VARCHAR(50),        --������������(�¶�=month,����=quarter,���=year)
    @RepNo	NVARCHAR(40) OUT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE     @Year       INT ;
    DECLARE     @Month      INT ;
	DECLARE     @CurrentRepBeginDate DATE ;
	DECLARE     @CurrentRepEndDate DATE ;
	DECLARE     @quarter INT ;  --����
	
	DECLARE @report_fix   VARCHAR(10)
	SET @report_fix='BS'
    
    --���ú��� ��������
    SELECT @CurrentRepBeginDate= begin_date,
           @CurrentRepEndDate= end_date,
           @report_fix=report_fix
    FROM [F_ReportDateCalculate](@report_date,@period_type,@report_fix) 
	 
	SET @Year = DATEPART(YEAR,@CurrentRepBeginDate);
	
	 
	
	--��ʼ���ʲ���ծ��ģ��
	UPDATE dbo.T_BalanceSheetTemplate
	SET asset_start_amount=NULL,asset_end_amount=NULL,debt_start_amount=NULL,debt_end_amount=NULL
 
	
	---1. ͳ��IERecord��Ӧ�գ�Ӧ��(Ӧ��:IE_Flag=I,Ӧ����IE_Flag=E)
	---2. ����Ӧ�ղ�ѯ����Ӧ�����ա�
	---3. Ӧ��-����=����Ӧ��	
	
	--Ӧ��
	DECLARE @Receivables    DECIMAL(18,4)       --����Ӧ������
	DECLARE @rec_sum        DECIMAL(18,4)       --������������
	
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
	WHERE IE_Flag='I'        --I=����
	    AND State='Ӧ��'
	    AND a.C_GUID=@C_ID
	    AND a.AffirmDate BETWEEN @CurrentRepBeginDate AND @CurrentRepEndDate
	    
	  
	--Ӧ���ܽ��    
	SELECT @Receivables =SUM(a.SumAmount)
	FROM #IE_Temp AS a 
	
	-- ����	
	SELECT @rec_sum= SUM(ISNULL(a.SumAmount,0))
	FROM dbo.T_RecPayRecord AS a
	INNER JOIN #IE_Temp AS b ON a.IE_GUID=b.IE_GUID 
	WHERE a.RP_Flag='R'
 
	-- ����Ӧ��=Ӧ������-��������
	UPDATE T_BalanceSheetTemplate
	SET asset_end_amount = @Receivables-ISNULL(@rec_sum,0)
	WHERE asset_row_no=6   
	
	--����Ӧ���˿�
	TRUNCATE TABLE #IE_Temp
	
	INSERT INTO #IE_Temp
	        ( IE_GUID, Profit_GUID, SumAmount ) 
	SELECT a.IE_GUID,a.Profit_GUID,a.SumAmount	
	FROM dbo.T_IERecord AS a
	WHERE IE_Flag='E'        --I=Ӧ��
	    AND IEGroup <>'����'
	    AND State='Ӧ��'
	    AND a.C_GUID=@C_ID
	    AND a.AffirmDate BETWEEN @CurrentRepBeginDate AND @CurrentRepEndDate
	    
	--Ӧ���ܽ��    
	SET @Receivables = 0;
	SET @rec_sum = 0;
	
	SELECT @Receivables =SUM(a.SumAmount)
	FROM #IE_Temp AS a 
	
	-- �Ѹ�	
	SELECT @rec_sum= SUM(ISNULL( a.SumAmount,0)) 	 
	FROM dbo.T_RecPayRecord AS a
	INNER JOIN #IE_Temp AS b ON a.IE_GUID=b.IE_GUID 
	WHERE a.RP_Flag='P'
 
	-- ����Ӧ��=Ӧ������-��������
	UPDATE T_BalanceSheetTemplate
	SET debt_end_amount = @Receivables-ISNULL(@rec_sum,0)
	WHERE debt_row_no=70       -- ����Ӧ�� 
	
	--����Ӧ������
	TRUNCATE TABLE #IE_Temp
	
	INSERT INTO #IE_Temp
	        ( IE_GUID, Profit_GUID, SumAmount ) 
	SELECT a.IE_GUID,a.Profit_GUID,a.SumAmount	
	FROM dbo.T_IERecord AS a
	WHERE IE_Flag='E'        --I=Ӧ��
	    AND IEGroup ='����'
	    AND State='Ӧ��'
	    AND a.C_GUID=@C_ID
	    AND a.AffirmDate BETWEEN @CurrentRepBeginDate AND @CurrentRepEndDate
	    
	--Ӧ���ܽ��    
	SET @Receivables = 0;
	SET @rec_sum = 0;
	
	SELECT @Receivables =SUM(a.SumAmount)
	FROM #IE_Temp AS a 
	
	-- �Ѹ�	
	SELECT @rec_sum= SUM(ISNULL( a.SumAmount,0)) 	 
	FROM dbo.T_RecPayRecord AS a
	INNER JOIN #IE_Temp AS b ON a.IE_GUID=b.IE_GUID 
	WHERE a.RP_Flag='P'
 
	-- ����Ӧ��=Ӧ������-��������
	UPDATE T_BalanceSheetTemplate
	SET debt_end_amount = @Receivables-ISNULL(@rec_sum,0)
	WHERE debt_row_no=72       -- ����Ӧ��  
	 
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
	
	
	
	DECLARE @current_asst_sum               DECIMAL(18,4)          --�����ʲ��ϼ�    
	DECLARE @long_terminvestments_sum       DECIMAL(18,4)          --����Ͷ�ʺϼ�    
	
	SET @current_asst_sum = 0
	SET @long_terminvestments_sum = 0
	
	--�����ʲ���ծ����� ��Ŀ�Ľ��
	SELECT Profit_GUID,SUM( amount) amount,MIN(b.Name) name
	INTO #BalanceSheet
	FROM #IE_List  AS a
	INNER JOIN dbo.T_GeneralLedgerAccount AS b ON a.Profit_GUID=b.LA_GUID
	GROUP BY  Profit_GUID    
	

	
	 --�̶��ʲ�ԭ��
	DECLARE @asst_amount        DECIMAL(18,4)
	DECLARE @dis_amount         DECIMAL(18,4)   --�۾ɽ�� 
	DECLARE @dis_amount_1       DECIMAL(18,4)   --�̶��ʲ���ֵ׼��
	DECLARE @fix_asst_sum       DECIMAL(18,4)  
 
	SET @dis_amount=0    
	SET @dis_amount_1=0     --�̶��ʲ���ֵ׼��
		
	SELECT @asst_amount=SUM(a.Amount) 
    FROM dbo.T_AIDRecord AS a
    INNER JOIN T_AIDTypeRecord AS b ON a.InvType=b.AT_GUID 
    WHERE 
     b.Asset_class='�̶��ʲ�'
    AND b.AID_FLAG='A'          --A=�ʲ�
    AND a.C_GUID= @C_ID
    AND a.Date BETWEEN @CurrentRepBeginDate AND @CurrentRepEndDate
    
    
    
    UPDATE T_BalanceSheetTemplate
	SET asset_end_amount=@asst_amount
	WHERE asset_row_no=39       --�̶��ʲ�ԭ��
	
	UPDATE T_BalanceSheetTemplate
	SET asset_end_amount= @asst_amount - @dis_amount
	WHERE asset_row_no=41       --�̶��ʲ���ֵ
	
	UPDATE T_BalanceSheetTemplate
	SET asset_end_amount=@asst_amount - @dis_amount - @dis_amount_1
	WHERE asset_row_no=43       --�̶��ʲ�����     
	       
	
	-- �̶��ʲ��ϼ� =�̶��ʲ����� +��������+�ڽ�����+�̶��ʲ����� ����3���ޣ�	
	 
	SET @fix_asst_sum =@asst_amount -@dis_amount -@dis_amount_1
	
	UPDATE T_BalanceSheetTemplate
	SET asset_end_amount= @fix_asst_sum
	WHERE asset_row_no=50       --�̶��ʲ��ϼ�
	
	--�����ʲ�
	SET @asst_amount = 0
	
	SELECT @asst_amount=SUM(a.Amount) 
    FROM dbo.T_AIDRecord AS a
    INNER JOIN T_AIDTypeRecord AS b ON a.InvType=b.AT_GUID 
    WHERE 
     b.Asset_class='�����ʲ�'
    AND b.AID_FLAG='A'
    AND a.C_GUID= @C_ID
    AND a.Date BETWEEN @CurrentRepBeginDate AND @CurrentRepEndDate
    
    DECLARE @long_cost                  DECIMAL(18,4)       --���ڴ�̯����
	DECLARE @other_long_asst            DECIMAL(18,4)       --���������ʲ�  
	DECLARE @temp_asst_sum               DECIMAL(18,4)       --�����ʲ������������ʲ��ϼ�
	
	SET @long_cost=0
	SET @other_long_asst=0
	
    UPDATE T_BalanceSheetTemplate
    SET asset_end_amount=@asst_amount
    WHERE asset_row_no=51   --�����ʲ�
    
    --�����ʲ������������ʲ��ϼ�
    SET @temp_asst_sum = @asst_amount + @long_cost + @other_long_asst
    
    UPDATE T_BalanceSheetTemplate
    SET asset_end_amount = @temp_asst_sum
    WHERE asset_row_no=60   --�����ʲ������������ʲ��ϼ�
    
    --�ʲ��ϼ�= �����ʲ��ϼ�+����Ͷ�ʺϼ�+�̶��ʲ��ϼ�+���ʲ������������ʲ��ϼ�
    UPDATE T_BalanceSheetTemplate
    SET asset_end_amount = @current_asst_sum + @long_terminvestments_sum + @fix_asst_sum + @temp_asst_sum
    WHERE asset_row_no= 67   --�ʲ��ϼ�
	
	
	--��ծ
 
	     
	----�����ʲ���ĩ��
	--UPDATE a
	--SET a.asset_end_amount=b.amount
	--FROM T_BalanceSheetTemplate AS a
	--INNER JOIN #BalanceSheet AS b ON LTRIM(a.asset_item_name)=LTRIM(b.name)
	
	--���¸�ծ��ĩ��
	--UPDATE a
	--SET a.debt_end_amount=b.amount
	--FROM T_BalanceSheetTemplate AS a
	--INNER JOIN #BalanceSheet AS b ON LTRIM(a.debt_item_name)=LTRIM(b.name)
	
	
	--��ȡ�ڳ���
    SELECT a.Money,b.Name
    INTO #InitAmount
    FROM dbo.T_BeginningBalance AS a
    INNER JOIN dbo.T_GeneralLedgerAccount AS b ON a.Acc_GUID=b.LA_GUID
	WHERE a.C_GUID=@C_ID
	
	--���������˻����ڳ��ֽ�
	DECLARE @init_cash  DECIMAL(18,4)
	SET @init_cash=0
	
	SELECT @init_cash = SUM( CAST(Amount AS DECIMAL(18,4)))
	FROM dbo.T_BankAccount
	WHERE C_GUID=@C_ID
	
	UPDATE T_BalanceSheetTemplate
	SET asset_start_amount=@init_cash
	WHERE RTRIM(asset_item_name)='�����ʽ�'
	
	
	--�����ʲ��ڳ���
	--SELECT *
	UPDATE a
	SET a.asset_start_amount=b.money
	FROM T_BalanceSheetTemplate AS a
	INNER JOIN #InitAmount AS b ON RTRIM(a.asset_item_name)=b.Name
	
	--���¸�ծ�ڳ���+������Ȩ��
	--SELECT *
	UPDATE a
	SET a.debt_start_amount=b.money
	FROM T_BalanceSheetTemplate AS a
	INNER JOIN #InitAmount AS b ON RTRIM(a.debt_item_name)=b.Name

	--��ѯ����
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


