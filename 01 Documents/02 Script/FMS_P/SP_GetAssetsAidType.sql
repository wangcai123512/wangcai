USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_GetAssetsAidType]    Script Date: 11/30/2016 18:37:11 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_GetAssetsAidType]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_GetAssetsAidType]
GO

USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_GetAssetsAidType]    Script Date: 11/30/2016 18:37:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,hdy>
-- Create date: <Create 11.30,,>
-- Description:	获取资产分类
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetAssetsAidType]
	@C_GUID NVARCHAR(50),--公司GUID
	@Asset_class VARCHAR(40)='' --资产分类
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT AidTypeName,AT_GUID
	FROM T_AIDTypeRecord 
	WHERE(C_GUID = @C_GUID  )
		and AID_FLAG='A'
		AND (Asset_class = @Asset_class  )   	
	ORDER BY AidTypeName
	
END 

GO


