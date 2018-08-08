USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_DelPTR]    Script Date: 02/16/2017 18:59:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	删除物料类别
-- =============================================
ALTER PROCEDURE [dbo].[SP_DelPTR]
	@AT_GUID NVARCHAR(40),
	@Flag NVARCHAR(40),
	@C_GUID NVARCHAR(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

		IF @Flag='P'
		   BEGIN
				DELETE dbo.T_AIDSubTypeRecord  WHERE  AST_GUID = @AT_GUID;	
				DELETE dbo.T_AIDTypeRecord  WHERE C_GUID = @C_GUID AND AID_FLAG = @Flag AND AT_GUID = @AT_GUID;
				
				DELETE dbo.T_ProductBom
				WHERE  product_subtype_id=@AT_GUID 									
		   END
		ELSE
			BEGIN
				DELETE dbo.T_AIDSubTypeRecord  WHERE  AST_GUID = @AT_GUID;	
				DELETE dbo.T_AIDTypeRecord  WHERE C_GUID = @C_GUID AND AID_FLAG = @Flag AND AT_GUID = @AT_GUID;
			END
	
END


GO


