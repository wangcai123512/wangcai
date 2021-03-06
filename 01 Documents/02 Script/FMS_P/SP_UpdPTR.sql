USE [FMS_Develop]
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdPTR]    Script Date: 11/25/2016 14:20:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	
-- =============================================
ALTER PROCEDURE [dbo].[SP_UpdPTR]
	@AT_GUID NVARCHAR(40),
	@Flag NVARCHAR(1),
	@AidTypeName NVARCHAR(40),
	@C_GUID NVARCHAR(40),
	@Asset_class NVARCHAR(40)=NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	
	BEGIN TRAN;
	DELETE FROM dbo.T_AIDTypeRecord WHERE AT_GUID = @AT_GUID;
	INSERT INTO dbo.T_AIDTypeRecord
	VALUES (@AT_GUID,@AidTypeName,@Flag,@C_GUID,@Asset_class);
	COMMIT TRAN;
END

