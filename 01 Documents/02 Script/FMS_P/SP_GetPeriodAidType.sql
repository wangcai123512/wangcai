USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_GetPeriodAidType]    Script Date: 12/27/2016 12:39:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	获取资产分期
-- =============================================
ALTER PROCEDURE [dbo].[SP_GetPeriodAidType] 
	@Parent_ID NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT Depreciation_year
	FROM dbo.T_AIDSubTypeRecord  
	WHERE AST_GUID=@Parent_ID  
	ORDER BY ASTTypeName
	
END 

GO


