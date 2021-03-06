USE [FMS_Develop]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetAttachmentList]    Script Date: 03/21/2017 16:40:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,chenxiang>
-- Create date: <Create Date,,07/16/2015>
-- Description:	获取@FR_GUID下面所有附件
-- =============================================
ALTER PROCEDURE [dbo].[SP_GetAttachmentList]
	@PageSize INT = -1,
	@PageIndex INT = 1,
	@Count INT = 0 OUT,	
	@FR_GUID NVARCHAR(40),	
	@DateBegin DATETIME = NULL,
	@DateEnd DATETIME = NULL
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   DECLARE @temp TABLE(
		rownumber INT,
		A_GUID NVARCHAR(40),
		FileName NVARCHAR(200),
		FileType NVARCHAR(500),
		FlieData VARBINARY(MAX),
		FR_GUID NVARCHAR(40),
		FileRemark NVARCHAR(500),
		Number NVARCHAR(100)
	 )	 
    -- Insert statements for procedure here
--TA.A_GUID AS A_GUID
    BEGIN
		INSERT INTO @temp
		SELECT ROW_NUMBER()OVER(ORDER BY A_GUID DESC) rownumber,A_GUID,
		FileName,FileType,FlieData,FR_GUID,FileRemark,Number
		FROM dbo.T_Attachment TA
		WHERE TA.FR_GUID=@FR_GUID	
		
        --T.A_GUID
		SELECT @Count = COUNT(*) FROM @temp;
		SELECT T.A_GUID,T.FileName,T.FileType,T.FlieData,
		T.FR_GUID,T.FileRemark,T.Number
		FROM @temp T
		WHERE (T.rownumber>= ((@PageIndex - 1)*@PageSize) + 1
		AND T.rownumber <= @PageIndex*@PageSize )
		OR (@PageSize = -1)
		
		
	END
END






