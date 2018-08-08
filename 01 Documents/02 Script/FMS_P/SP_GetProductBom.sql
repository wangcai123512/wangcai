-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,hdy>
-- Create date: <Create Date,01/16/2017>
-- Description:	»ñÈ¡ProductBom
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetProductBom]
	@PageSize INT = -1,
	@PageIndex INT = 1,
	@Count INT = 0 OUT,
	@root NVARCHAR(40) 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @temp TABLE(
		rownumber INT,
		GUID NVARCHAR(40),
		product_subtype_id NVARCHAR(40),
		parent_node NVARCHAR(40),
		node NVARCHAR(40),
		node_level INT,
		number INT,
		node_level_seq_no INT,
		node_level_seq_number INT,
		ASTTypeName NVARCHAR(40)
	 )	
    BEGIN
		INSERT INTO @temp
		SELECT ROW_NUMBER()OVER(ORDER BY PB.GUID DESC) rownumber,GUID,product_subtype_id,parent_node,node,
		node_level,number,node_level_seq_no,node_level_seq_number,ATR.ASTTypeName AS ASTTypeName
		FROM dbo.T_ProductBom PB
		LEFT JOIN dbo.T_AIDSubTypeRecord ATR ON PB.product_subtype_id = ATR.AST_GUID
		WHERE PB.product_subtype_id=@root

		SELECT @Count = COUNT(*) FROM @temp;
		SELECT T.GUID,T.product_subtype_id,T.parent_node,T.node,T.node_level,T.number,T.node_level_seq_no,
		T.node_level_seq_number,T.ASTTypeName
		FROM @temp T
		WHERE (T.rownumber>= ((@PageIndex - 1)*@PageSize) + 1
		AND T.rownumber <= @PageIndex*@PageSize )
		OR (@PageSize = -1)
	END
END
GO