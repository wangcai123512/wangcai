USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_UpdAttachmentName]    Script Date: 03/09/2017 16:29:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





ALTER PROCEDURE [dbo].[SP_UpdAttachmentName]
	@A_GUID NVARCHAR(40),
	@FileName NVARCHAR(200)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

    -- Insert statements for procedure here
    BEGIN TRAN;
	UPDATE T_Attachment
	SET  FileName=@FileName
	WHERE A_GUID=@A_GUID;
	COMMIT TRAN;
END






GO


