USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_GetAIDRecordUp]    Script Date: 01/09/2017 16:29:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,chenxiang>
-- Create date: <Create Date,,07/16/2015>
-- Description:	获取采购记录
-- =============================================
ALTER PROCEDURE [dbo].[SP_GetAIDRecordUp]
	@PageSize INT = -1,
	@PageIndex INT = 1,
	@Count INT = 0 OUT,
	@Flag NVARCHAR(1) = NULL,
	@C_GUID NVARCHAR(50),
	@DateBegin DATETIME = NULL,
	@DateEnd DATETIME = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @temp TABLE(
		rownumber INT,
		GUID NVARCHAR(40),
		Date DATE,
		Amount DECIMAL(18,4),
		Currency NVARCHAR(40),
		RPer NVARCHAR(40),
		RPerName NVARCHAR(40),
		InvType NVARCHAR(40),
		Description NVARCHAR(100),
		DepreciationPeriod INT,
		SurplusValue DECIMAL(18,4),
		State NVARCHAR(40),
		Remark NVARCHAR(200),
		A_GUID NVARCHAR(40)
	 )
	 
    -- Insert statements for procedure here

    BEGIN
		INSERT INTO @temp
		SELECT ROW_NUMBER()OVER(ORDER BY AID.Date DESC) rownumber,AID.GUID,AID.Date,AID.Amount,AID.Currency,AID.RPer,BP.Name AS RPerName,AID.InvType,AID.Description,AID.DepreciationPeriod,AID.SurplusValue,AID.State,AID.Remark,TA.A_GUID AS A_GUID
		FROM dbo.T_AIDRecord AID
		LEFT JOIN dbo.T_BusinessPartner BP ON AID.RPer = BP.BP_GUID
		LEFT JOIN dbo.T_Attachment TA ON TA.FR_GUID = AID.GUID
		LEFT JOIN dbo.T_DeclareCostSpending TDCS ON TDCS.GUID = AID.GUID
		WHERE AID.C_GUID=@C_GUID
		AND (TDCS.State='未付')
		AND (AID.AID_Flag = @Flag OR @Flag IS NULL OR LEN(@Flag) = 0)
		AND(AID.Date >= @DateBegin OR @DateBegin IS NULL)
		AND(AID.Date < DATEADD(DAY,1,@DateEnd) OR @DateEnd IS NULL)

		SELECT @Count = COUNT(*) FROM @temp;
		SELECT T.GUID,T.Date,T.Amount,T.Currency,T.RPer,T.RPerName,T.InvType,T.Description,T.DepreciationPeriod,T.SurplusValue,T.State,T.Remark,T.A_GUID
		FROM @temp T
		WHERE (T.rownumber>= ((@PageIndex - 1)*@PageSize) + 1
		AND T.rownumber <= @PageIndex*@PageSize )
		OR (@PageSize = -1)
	END
END
GO


