USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_DelProductBomDetail]    Script Date: 02/16/2017 18:57:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	É¾³ýProductBom
-- =============================================
ALTER PROCEDURE [dbo].[SP_DelProductBomDetail]
	@ID NVARCHAR(40),
	@nodelevel INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
			DECLARE @product_subtype_id VARCHAR(50)
			DECLARE @node VARCHAR(50)
			DECLARE @CountSonSum VARCHAR(50)
			
			SELECT @product_subtype_id=product_subtype_id,@node=node 
			FROM dbo.T_ProductBom
			WHERE GUID = @ID
			
			SELECT  @CountSonSum=COUNT(*)
			FROM dbo.T_ProductBom
			WHERE product_subtype_id=@product_subtype_id
			AND  parent_node=@node
			AND  node_level='2'
						
	BEGIN TRAN;
	  IF @nodelevel='1'
		BEGIN
			
			DELETE dbo.T_ProductBom  WHERE GUID = @ID;
			IF @CountSonSum>0
			   BEGIN
					DELETE dbo.T_ProductBom
					WHERE product_subtype_id=@product_subtype_id
					AND  parent_node=@node
					AND  node_level='2'
			   END			
		END
	  ELSE
	  	BEGIN
	  		DELETE dbo.T_ProductBom  WHERE GUID = @ID;
	  	END
    COMMIT TRAN;
END


GO


