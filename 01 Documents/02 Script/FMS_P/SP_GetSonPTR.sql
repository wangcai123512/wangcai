USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_GetSonPTR]    Script Date: 02/13/2017 14:50:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	更新子类别前查询
-- =============================================
ALTER PROCEDURE [dbo].[SP_GetSonPTR]
@AT_GUID NVARCHAR(50),
@ASTTypeName NVARCHAR(50),
@AID_FLAG NVARCHAR(50),
@C_GUID NVARCHAR(50),
@Count INT = 0 OUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
			--DECLARE @AT_GUID  VARCHAR(50)
			--SELECT AT_GUID=@AT_GUID
			--FROM dbo.T_AIDTypeRecord AR
			--WHERE AR.AidTypeName =@AidTypeName AND AR.AID_FLAG =@AID_FLAG AND  AR.C_GUID =@C_GUID				
	BEGIN TRAN; 
		BEGIN 
			SELECT * 
			FROM dbo.T_AIDSubTypeRecord AST
			LEFT JOIN dbo.T_AIDTypeRecord ATR ON AST.AST_ParentAidType=ATR.AT_GUID
			WHERE AST.ASTTypeName=@ASTTypeName 
				AND ATR.AID_FLAG =@AID_FLAG 
				AND ATR.C_GUID =@C_GUID
				AND AST.AST_ParentAidType=@AT_GUID
						
			SELECT @Count = COUNT(*)
			FROM dbo.T_AIDSubTypeRecord AST
			LEFT JOIN dbo.T_AIDTypeRecord ATR ON AST.AST_ParentAidType=ATR.AT_GUID
			WHERE AST.ASTTypeName=@ASTTypeName 
				AND ATR.AID_FLAG =@AID_FLAG 
				AND ATR.C_GUID =@C_GUID
				AND AST.AST_ParentAidType=@AT_GUID
		END
    COMMIT TRAN;	
END

GO


