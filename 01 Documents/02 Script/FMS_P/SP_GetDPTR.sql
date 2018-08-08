USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_GetDPTR]    Script Date: 02/09/2017 14:45:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	删除物料类别前查询
-- =============================================
ALTER PROCEDURE [dbo].[SP_GetDPTR]
@AST_GUID NVARCHAR(50),
@AST_ParentAidType NVARCHAR(50)=NULL,
@C_GUID NVARCHAR(50)=NULL,
@AID_Flag NVARCHAR(50)=NULL,
@AidTypeName NVARCHAR(50)=NULL,
@Count INT = 0 OUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	IF @AID_Flag='P'
		 BEGIN
			IF @AST_ParentAidType IS NULL
				BEGIN
				SELECT *
					FROM dbo.T_Product P
					WHERE P.TypeId=@AST_ParentAidType
					AND P.C_GUID=@C_GUID
					
					SELECT @Count = COUNT(*)
					FROM dbo.T_Product p
					WHERE P.TypeId=@AST_ParentAidType
					AND P.C_GUID=@C_GUID		
				END
				ELSE
				BEGIN
				SELECT * 
					FROM dbo.T_Product P
					WHERE P.SubTypeId=@AST_GUID 
					AND P.C_GUID=@C_GUID
					
					SELECT @Count = COUNT(*)
					FROM dbo.T_Product P
					WHERE P.SubTypeId=@AST_GUID
					AND P.C_GUID=@C_GUID 
				END
	   END
	ELSE
		BEGIN
			IF @AST_ParentAidType IS NULL
				BEGIN
				SELECT *
					FROM dbo.T_AIDRecord AR
					WHERE AR.InvType=@AST_ParentAidType
					AND AR.C_GUID=@C_GUID	 
					
					SELECT @Count = COUNT(*)
					FROM dbo.T_AIDRecord AR
					WHERE AR.InvType=@AST_ParentAidType	
					AND AR.C_GUID=@C_GUID		
				END
				ELSE
				BEGIN
				SELECT * 
					FROM dbo.T_AIDRecord AR
					WHERE AR.SubType=@AST_GUID 
					AND AR.C_GUID=@C_GUID	
					
				   SELECT @Count = COUNT(*)
					FROM dbo.T_AIDRecord AR
					WHERE AR.SubType=@AST_GUID 
					AND AR.C_GUID=@C_GUID	
				END
		END
END

GO


