USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_GetAIDPersonRecord]    Script Date: 01/09/2017 16:22:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,chenxiang>
-- Create date: <Create Date,,07/16/2015>
-- Description:	获取采购记录的“使用”记录
-- =============================================
ALTER PROCEDURE [dbo].[SP_GetAIDPersonRecord]
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
		GUID NVARCHAR(40),
		Use_AID_Amount NVARCHAR(40),
		Create_Date VARCHAR(20),
		AidTypeName VARCHAR(40),
		ASTTypeName VARCHAR(40)
	 )
	 
    -- Insert statements for procedure here
--TA.A_GUID AS A_GUID
    BEGIN
		INSERT INTO @temp
		SELECT ROW_NUMBER()OVER(ORDER BY AID_PR.GUID DESC) rownumber,AID_PR.GUID,
		Use_AID_Amount,CONVERT(VARCHAR(10),Create_Date,111) AS Create_Date,c.AidTypeName,d.ASTTypeName
		
		FROM dbo.T_AID_Product AID_PR
		LEFT JOIN dbo.T_Product TP ON TP.GUID = AID_PR.Product_Guid
	    INNER JOIN dbo.T_AIDTypeRecord c ON TP.TypeId = c.AT_GUID
		INNER JOIN dbo.T_AIDSubTypeRecord d ON TP.SubTypeId = d.AST_GUID	
		WHERE AID_PR.AID_Guid=@ID
		
        --,T.A_GUID
		SELECT @Count = COUNT(*) FROM @temp;
		SELECT T.GUID,T.Use_AID_Amount,T.Create_Date,T.AidTypeName,T.ASTTypeName 
		FROM @temp T
		WHERE (T.rownumber>= ((@PageIndex - 1)*@PageSize) + 1
		AND T.rownumber <= @PageIndex*@PageSize )
		OR (@PageSize = -1)
	END
END


GO


