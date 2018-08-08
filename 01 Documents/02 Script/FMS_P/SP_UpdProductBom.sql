USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_UpdProductBom]    Script Date: 02/15/2017 16:26:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






-- =============================================
-- Author:		<Author,,hdy>
-- Create date: <Create Date,01/16/2017>
-- Description:	更新节点数量
-- =============================================
ALTER PROCEDURE [dbo].[SP_UpdProductBom]
		@nodesid     VARCHAR(50),
	    @nodes     VARCHAR(50),
	    @nodelevel     VARCHAR(50),
	    @tags		VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;		
		DECLARE @product_subtype_id VARCHAR(50)
		DECLARE @SonGuid VARCHAR(50)
		DECLARE @parent_node VARCHAR(50)
		DECLARE @node VARCHAR(50)
		DECLARE @Parentnumber INT
		DECLARE @Sonnumber INT
		IF @nodelevel='2'
			BEGIN
				/**通过子记录获取父记录*/
				SELECT @product_subtype_id=product_subtype_id,@parent_node=parent_node
				FROM dbo.T_ProductBom
				WHERE GUID = @nodesid
				AND node_level='2'
						
				SELECT @Parentnumber=number
				FROM dbo.T_ProductBom
				WHERE  node= @parent_node
				AND product_subtype_id=@product_subtype_id
				AND parent_node=@parent_node
				AND node_level='1'
			END
		ELSE
			BEGIN	
			/**通过父记录获取子记录*/
			SELECT @product_subtype_id=product_subtype_id,@node=node
			FROM dbo.T_ProductBom
			WHERE GUID = @nodesid
			AND node_level='1'
			
			SELECT @SonGuid=GUID,@Sonnumber=number
			FROM dbo.T_ProductBom
			WHERE   product_subtype_id=@product_subtype_id
			AND parent_node=@node
			AND node_level='2'
			END
		
		
		
	BEGIN TRAN;	
		BEGIN
		IF @nodelevel='2'
			BEGIN		
				UPDATE T_ProductBom SET number = @tags,number_sum=@tags*@Parentnumber
				WHERE  node= @nodes
				AND GUID=@nodesid 
			END
		ELSE
			BEGIN
				UPDATE T_ProductBom SET number = @tags,number_sum=@tags
				WHERE   GUID=@nodesid
				
				UPDATE T_ProductBom SET number_sum=@tags*@Sonnumber
				WHERE GUID=@SonGuid 
			END
		END	
    COMMIT TRAN;
END






GO


