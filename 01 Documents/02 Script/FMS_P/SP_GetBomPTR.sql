USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_GetBomPTR]    Script Date: 02/14/2017 14:32:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
	-- =============================================
	-- Author:		<Author,,Name>
	-- Create date: <Create Date>
	-- Description:	
	-- =============================================
	ALTER PROCEDURE [dbo].[SP_GetBomPTR]
	@nodes NVARCHAR(50),
	@nodesid NVARCHAR(50),
	@subnodes NVARCHAR(50),
	@C_GUID NVARCHAR(50),
	@nodelevel INT,
	@Count INT=0 OUT
	AS
	BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	
			 DECLARE  @ProductBomNode NVARCHAR(50)		
			 SELECT	@ProductBomNode=product_subtype_id
			 FROM dbo.T_ProductBom PBD
			 WHERE GUID=@nodesid
			 		 
	BEGIN TRAN; 
		BEGIN
		IF @nodelevel = '1'
			BEGIN	
				SELECT * 
				FROM dbo.T_ProductBom PB
				WHERE PB.product_subtype_id= @ProductBomNode
				AND PB.node =@subnodes
				AND PB.node_level='1' 
							
				SELECT @Count = COUNT(*)
				FROM dbo.T_ProductBom PB
				WHERE PB.product_subtype_id= @ProductBomNode 
				AND PB.node =@subnodes
				AND PB.node_level='1'
			END
		ELSE 
			BEGIN
				--需要返回一个结果，满足count（*）=0就好--
				SELECT * 
				FROM dbo.T_ProductBom PB
				WHERE PB.parent_node='123' 
				AND PB.node ='123' 
							
				SELECT @Count = COUNT(*)
				FROM dbo.T_ProductBom PB
				WHERE PB.parent_node='123' 
				AND PB.node ='123' 

			END
		END
    COMMIT TRAN;	
END

GO


