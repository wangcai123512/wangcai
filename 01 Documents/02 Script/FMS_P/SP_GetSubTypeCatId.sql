USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_GetSubTypeCatId]    Script Date: 12/22/2016 13:30:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	获取子类别id
-- =============================================
ALTER PROCEDURE [dbo].[SP_GetSubTypeCatId]
@C_GUID NVARCHAR(50),
@InvType NVARCHAR(100),
@SonInvType NVARCHAR(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
    DECLARE @InvTypeCateId NVARCHAR(100)	
			
    SELECT @InvTypeCateId=P.AT_GUID
	FROM dbo.T_AIDTypeRecord p 
	WHERE  C_GUID=@C_GUID AND AidTypeName=@InvType	
	
    -- Insert statements for procedure here
	SELECT P.AST_GUID
	FROM dbo.T_AIDSubTypeRecord p 
	WHERE ASTTypeName=@SonInvType AND AST_ParentAidType=@InvTypeCateId
	
END


GO


