USE [FMS_Develop]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetUPTR]    Script Date: 11/29/2016 16:37:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	更新父类别前查询
-- =============================================
ALTER PROCEDURE [dbo].[SP_GetUPTR]
@AidTypeName NVARCHAR(50),
@AID_FLAG NVARCHAR(50),
@C_GUID NVARCHAR(50),
@Count int = 0 out
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
		SELECT *
		from dbo.T_AIDTypeRecord AR
		where AR.AidTypeName =@AidTypeName AND AR.AID_FLAG =@AID_FLAG AND  AR.C_GUID =@C_GUID
		
		SELECT @Count = COUNT(*)
		from dbo.T_AIDTypeRecord AR
		where AR.AidTypeName =@AidTypeName AND AR.AID_FLAG =@AID_FLAG AND  AR.C_GUID =@C_GUID
END
