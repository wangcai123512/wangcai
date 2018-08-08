USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_AddAttachment]    Script Date: 03/21/2017 08:29:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<魏礼明>
-- Create date: <2015/05/20>
-- Description:	<新增附件>
-- =============================================
ALTER PROCEDURE [dbo].[SP_AddAttachment]
	-- Add the parameters for the stored procedure here
	@A_GUID NVARCHAR(50),
	@FileName NVARCHAR(200),
	@FileType NVARCHAR(500),
	@FR_GUID NVARCHAR(50),
	@FlieData VARBINARY(MAX),
	@FileRemark NVARCHAR(500) = NULL,
	@Number NVARCHAR(100)= NULL
AS
BEGIN
		-- SET NOCOUNT ON added to prevent extra result sets from
		-- interfering with SELECT statements.
		DECLARE @AttachmentCount VARCHAR(40)
		
		SET NOCOUNT ON;
		SELECT @AttachmentCount = COUNT(*) FROM dbo.T_Attachment 
		WHERE FR_GUID=@A_GUID 
		AND FileName=@FileName
		
		-- Insert statements for procedure here
		IF @AttachmentCount = 0
			BEGIN
				INSERT INTO dbo.T_Attachment
				(A_GUID,FileName,FileType,FR_GUID,FlieData,FileRemark,Number)
				VALUES(@A_GUID,@FileName,@FileType,@FR_GUID,@FlieData,@FileRemark,@Number)
			END
    END

GO


