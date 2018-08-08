USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_GenPerviewIncomeStatements]    Script Date: 12/20/2016 17:36:21 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_GenPerviewIncomeStatements]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_GenPerviewIncomeStatements]
GO

USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_GenPerviewIncomeStatements]    Script Date: 12/20/2016 17:36:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	生成利润预览表
-- =============================================
CREATE PROCEDURE [dbo].[SP_GenPerviewIncomeStatements]
	@C_ID NVARCHAR(40),
	@Year INT OUT,
    @Month INT OUT,
    @RepNo	NVARCHAR(40) OUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @CurrentRepBeginDate DATE ;
	DECLARE @CurrentRepEndDate DATE ;
	
	--生成报表时间区间
	
	IF EXISTS (SELECT * FROM dbo.T_Report Rep WHERE  Rep.C_GUID = @C_ID AND Rep.Type='PL') 
	BEGIN 
		DECLARE @LastRepYear INT;
		DECLARE @LastRepMonth INT;
		
		SELECT TOP 1 @LastRepYear =Rep.[Year],@LastRepMonth = Rep.[Month]
		FROM dbo.T_Report Rep 
		WHERE  Rep.C_GUID = @C_ID AND Rep.Type='PL' 
		ORDER BY RepNo DESC;
		
		SET @CurrentRepBeginDate = DATEADD(year,@LastRepYear-1,CONVERT(DATE,'0001-1-1'));
		SET @CurrentRepBeginDate= DATEADD(month,@LastRepMonth,@CurrentRepBeginDate);	
	END
	ELSE
	BEGIN
		SELECT @CurrentRepBeginDate = ReportStartDate
		FROM dbo.T_CompanySetting
		WHERE C_GUID = @C_ID;
	END
	
	SET @Year = DATEPART(YEAR,@CurrentRepBeginDate);
	SET @Month = DATEPART(MONTH,@CurrentRepBeginDate);	
	SET @CurrentRepEndDate= DATEADD(month,1 ,@CurrentRepBeginDate);
	SET @RepNo = 'PL'+ CONVERT(NVARCHAR(8),@CurrentRepBeginDate,112);
    --生成报表数据
    DECLARE @TmpV_PR TABLE
    (
		Profit_GUID NVARCHAR(40),
		AccGroup INT,
		Flag NVARCHAR(40),
		Money DECIMAL(18,2)
    );
    
    INSERT INTO @TmpV_PR(Profit_GUID,AccGroup,Money,Flag)
    SELECT FR.Profit_GUID,GLA.AccGroup,FR.Amount as Money,FR.IE_Flag as Flag
    FROM dbo.T_IERecord FR
    LEFT JOIN dbo.T_GeneralLedgerAccount AS GLA ON FR.Profit_GUID = GLA.LA_GUID
    WHERE FR.C_GUID = @C_ID AND DATE BETWEEN @CurrentRepBeginDate AND @CurrentRepEndDate;
    
    DECLARE  @TmpFR TABLE
    (
		ACC_GUID NVARCHAR(40),
        Value DECIMAL(18,2),
        AccGrp INT
    )
    --(收入)
    INSERT INTO @TmpFR(ACC_GUID,Value,AccGrp)
    SELECT FR.Profit_GUID AS ACC_GUID,(FR.Money) AS Value,FR.AccGroup AS AccGrp
    FROM @TmpV_PR FR
    WHERE Flag='I'  
    --AND   FR.AccGroup = 6 OR FR.AccGroup = 7 OR FR.AccGroup = 5;
    
    --(营业税金)
    INSERT INTO @TmpFR(ACC_GUID,Value,AccGrp)
    SELECT '132B92DE-1469-411F-A75F-C04B61E507D1',(FR.TaxationAmount) AS Value,GLA.AccGroup AS AccGrp
    FROM T_IERecord FR
    LEFT JOIN dbo.T_GeneralLedgerAccount AS GLA ON FR.Profit_GUID = GLA.LA_GUID
    WHERE FR.IE_Flag='I' 
    AND GLA.AccGroup = 6 OR GLA.AccGroup = 7 OR GLA.AccGroup = 5;
    
    --(费用)
    INSERT INTO @TmpFR(ACC_GUID,Value,AccGrp)
    SELECT FR.Profit_GUID AS ACC_GUID,(0-FR.Money) AS Value,FR.AccGroup AS AccGrp
    FROM @TmpV_PR FR
    WHERE Flag='E'
    --AND FR.AccGroup = 6 OR FR.AccGroup = 7 OR FR.AccGroup = 5;
    
  
  SELECT LA.AccGroup AS AccGrp,LA.Name,LA.AccCode AS Code ,ISNULL(T.CurrentValue,0) AS EndingValue
  FROM dbo.R_CompanyAccount R_CA
  LEFT JOIN dbo.T_GeneralLedgerAccount LA ON R_CA.LA_GUID = LA.LA_GUID
  LEFT JOIN (
		SELECT AccGrp,ACC_GUID,SUM(Value) AS CurrentValue
		FROM @TmpFR
		GROUP BY AccGrp,ACC_GUID
		HAVING SUM(Value)<> 0
	) AS T ON T.ACC_GUID = R_CA.LA_GUID
	WHERE R_CA.C_GUID = @C_ID AND (LA.AccGroup = 6 OR LA.AccGroup = 7 )
END


--ROLLBACK

GO


