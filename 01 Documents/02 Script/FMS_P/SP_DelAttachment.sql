USE [FMS_Develop]
GO
/****** Object:  StoredProcedure [dbo].[SP_DelAttachment]    Script Date: 01/04/2017 09:22:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	删除财务记录下的所有附件
-- =============================================
ALTER PROCEDURE [dbo].[SP_DelAttachment]
	@FR_ID NVARCHAR(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
     IF @FR_ID IS NOT NULL
     	BEGIN
	    BEGIN TRAN;
          DELETE dbo.T_Attachment WHERE FR_GUID = @FR_ID;
        COMMIT TRAN;
        END
END
