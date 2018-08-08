USE [FMS_Develop]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetUserState]    Script Date: 06/13/2017 14:49:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,hdy>
-- Create date: <Create Date,6.13,>
-- Description:	<Description,获取用户状态,>
-- =============================================
ALTER PROCEDURE[dbo].[SP_GetUserState] 
	@U_GUID NVARCHAR(50)=null,
	@C_GUID NVARCHAR(50)=null
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT State FROM [FMS_Develop].[dbo].[R_UserCompany] where U_GUID=@U_GUID and C_GUID=@C_GUID
END
