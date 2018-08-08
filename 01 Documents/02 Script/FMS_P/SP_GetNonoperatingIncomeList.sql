USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_GetNonoperatingIncomeList]    Script Date: 03/14/2017 17:55:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





-- =============================================
-- Author:		<胡东杨>
-- Create date: <2017/03/14>
-- Description:	<查询所有营业外收入>
-- =============================================
ALTER PROCEDURE [dbo].[SP_GetNonoperatingIncomeList]
@PageSize INT = -1,
	@PageIndex INT = 1,
	@Count INT = 0 OUT,
	@C_GUID NVARCHAR(50),
	@DateBegin DATETIME = NULL,
	@DateEnd DATETIME = NULL
AS
BEGIN

	
	DECLARE @temp TABLE(
		rownumber INT,
		IE_GUID NVARCHAR(40),
		RPerName NVARCHAR(40),
		AffirmDate DATETIME,
		Date DATETIME,
		Amount DECIMAL(18,4),
		Currency NVARCHAR(40),
		TaxationType VARCHAR(40),
		TaxationAmount DECIMAL(18,4),
		SumAmount DECIMAL(18,4),
		Remark VARCHAR(200),
		InvNo VARCHAR(20),		
		InvType NVARCHAR(40),
		CreateDate DATETIME
	 )
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
		INSERT INTO @temp
		SELECT ROW_NUMBER()OVER(ORDER BY IE.IE_GUID DESC) rownumber,
		IE.IE_GUID,BP.Name AS RPerName,CONVERT(VARCHAR(10),IE.AffirmDate,111) AS AffirmDate,
		CONVERT(VARCHAR(10),IE.Date,111) AS Date,IE.Amount,IE.Currency,IE.TaxationType,
		IE.TaxationAmount,IE.SumAmount,IE.Remark,IE.InvNo,IE.InvType,
		CONVERT(VARCHAR(10),IE.CreateDate,111) AS CreateDate
		FROM dbo.T_IERecord IE
		 LEFT JOIN dbo.T_BusinessPartner BP ON BP.BP_GUID = IE.RPer
		WHERE IE.IE_Flag='I'
		AND IE.InvType='营业外收入'
		AND IE.C_GUID=@C_GUID
	
	    --汇总查询数目--
		SELECT @Count = COUNT(*) FROM @temp;
		SELECT T.IE_GUID,T.RPerName,T.AffirmDate,T.Date,T.Amount,T.Currency,
		T.TaxationType,T.TaxationAmount,T.SumAmount,T.Remark,T.InvNo,
		T.InvType,T.CreateDate
		FROM @temp T
		WHERE (T.rownumber>= ((@PageIndex - 1)*@PageSize) + 1
		AND T.rownumber <= @PageIndex*@PageSize)
		OR (@PageSize = -1)
	
END


GO


