USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_UpdProductBomDeatil]    Script Date: 02/15/2017 14:02:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




ALTER PROCEDURE [dbo].[SP_UpdProductBomDeatil]
	@nodesid NVARCHAR(50),
	@subnodes NVARCHAR(50),
	@tags INT
		
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
		DECLARE @node_level  INT
		DECLARE @number INT
		DECLARE @CountProductBom INT
		DECLARE @node_level_seq_no INT
		DECLARE @number_sum INT
		DECLARE @product_subtype_id  VARCHAR(50)				
		DECLARE @GUID VARCHAR(40) 	/**生成GUID*/
		DECLARE @node VARCHAR(50)
		DECLARE @ProductBom_Guid VARCHAR(50)
		DECLARE @ProductBom_number INT
		DECLARE @ProductBom_number_sum INT
		SET  @GUID=NEWID();
		
		SELECT @product_subtype_id=product_subtype_id,@node_level=node_level,
		@number=number,@node_level_seq_no=node_level_seq_no,@number_sum=number_sum,
		@node=node
		FROM dbo.T_ProductBom
		WHERE GUID = @nodesid
	     
	     
	    SELECT @CountProductBom=COUNT(*) FROM dbo.T_ProductBom WHERE 
	    product_subtype_id = @product_subtype_id
	    AND parent_node= @product_subtype_id
	    
	    --查询有没有相同的level为1的物料
	    SELECT @ProductBom_Guid=GUID,@ProductBom_number=number,
	    @ProductBom_number_sum=number_sum
	    FROM dbo.T_ProductBom WHERE node_level='1' 
		AND node=@subnodes AND parent_node=@node AND parent_node=@node		
		BEGIN TRAN;
			BEGIN
			-- 
			IF  EXISTS(SELECT * FROM dbo.T_ProductBom WHERE node_level='1' 
			AND node=@subnodes AND parent_node=@node AND parent_node=@node)
				BEGIN
					UPDATE dbo.T_ProductBom 
					SET number=@ProductBom_number+@tags,
					 number_sum=@ProductBom_number_sum+@tags		
					WHERE GUID=@ProductBom_Guid	
					
				END
			ELSE
				BEGIN
					INSERT INTO dbo.T_ProductBom
					(GUID,product_subtype_id,parent_node,node,node_level,number,
					node_level_seq_no,number_sum)
					VALUES(@GUID ,@product_subtype_id ,@node,@subnodes,'',
					@tags,'',1); 
					
					UPDATE dbo.T_ProductBom 
					SET node_level=@node_level+1, 
					number_sum=@number_sum*@tags,
					node_level_seq_no=@CountProductBom+1			
					WHERE GUID=@GUID
				END		
			END
		COMMIT TRAN;
END









GO


