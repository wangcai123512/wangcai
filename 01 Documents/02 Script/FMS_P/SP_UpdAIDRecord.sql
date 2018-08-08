USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_UpdPSTR]    Script Date: 01/21/2017 15:17:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	
-- =============================================
ALTER PROCEDURE [dbo].[SP_UpdPSTR]
	@AST_GUID NVARCHAR(40),
	@ASTTypeName NVARCHAR(40),
	@Remark NVARCHAR(40)=NULL,
	@Depreciation_year NVARCHAR(40)=NULL,
	@AST_ParentAidType NVARCHAR(40),
	@AID_FLAG NVARCHAR(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
		DECLARE @GUID VARCHAR(40) 	/**Éú³ÉGUID*/		
		SET  @GUID=NEWID();
	BEGIN TRAN;
		DELETE FROM dbo.T_AIDSubTypeRecord WHERE AST_GUID = @AST_GUID;
		
		INSERT INTO dbo.T_AIDSubTypeRecord
		VALUES (@AST_GUID,@ASTTypeName,@AST_ParentAidType,@Remark,@Depreciation_year);
		
		IF @AID_FLAG = 'P'
		BEGIN
			INSERT INTO dbo.T_ProductBom
					(GUID,product_subtype_id,parent_node,node,node_level,number,
					node_level_seq_no,number_sum)
					VALUES(@GUID ,@AST_GUID,NULL,@AST_GUID,0,
					1,0,1);
		END
	COMMIT TRAN;
END



GO


