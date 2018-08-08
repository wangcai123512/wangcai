USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_GetIEPersonRecord]    Script Date: 02/08/2017 09:33:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,chenxiang>
-- Create date: <Create Date,,07/16/2015>
-- Description:	获取采购记录的“转售”记录
-- =============================================
ALTER PROCEDURE [dbo].[SP_GetIEPersonRecord]
	@PageSize INT = -1,
	@PageIndex INT = 1,
	@Count INT = 0 OUT,
	@Flag NVARCHAR(1) = NULL,
	@ID NVARCHAR(40),
	@C_GUID NVARCHAR(50),
	@DateBegin DATETIME = NULL,
	@DateEnd DATETIME = NULL,
	@Customer NVARCHAR(40) = NULL,
	@Grp NVARCHAR(40)=NULL,
	@State NVARCHAR(40)=NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   DECLARE @temp TABLE(
		rownumber INT,
		IE_GUID NVARCHAR(40),
		Date VARCHAR(20),
		Amount DECIMAL(18,4),
		Currency NVARCHAR(40),
		RPer NVARCHAR(40),
		RPerName NVARCHAR(40),
		InvType NVARCHAR(40),
		Description NVARCHAR(100),
		State NVARCHAR(40),
		Remark NVARCHAR(200),
	    Resale_Amount DECIMAL(18, 4)
		--A_GUID NVARCHAR(40)
	 )	 
    -- Insert statements for procedure here
--TA.A_GUID AS A_GUID
    BEGIN
		INSERT INTO @temp
		SELECT ROW_NUMBER()OVER(ORDER BY IE.Date DESC) rownumber,IE_GUID,
		CONVERT(VARCHAR(20),Date,111) AS Date,Amount,Currency,RPer,BP.Name AS RPerName,
		InvType,IE.IEDescription,State,IE.Remark,IE.Resale_Amount	
		FROM dbo.T_IERecord IE
		LEFT JOIN dbo.T_BusinessPartner BP ON IE.RPer = BP.BP_GUID
		--LEFT JOIN dbo.T_Attachment TA ON TA.FR_GUID = AID.GUID
		WHERE IE.C_GUID=@C_GUID
		AND(IE.GUID_Parent=@ID)
		AND(IE.IE_Flag='I')
		
        --,T.A_GUID
		SELECT @Count = COUNT(*) FROM @temp;
		SELECT T.IE_GUID,T.Date,T.Amount,T.Currency,T.RPer,T.RPerName,T.InvType,
		T.Description,T.State,T.Remark,T.Resale_Amount
		FROM @temp T
		WHERE (T.rownumber>= ((@PageIndex - 1)*@PageSize) + 1
		AND T.rownumber <= @PageIndex*@PageSize )
		OR (@PageSize = -1)
	END
END





GO


