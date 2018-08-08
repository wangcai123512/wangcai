USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_GetAssetTypeCatId]    Script Date: 12/27/2016 18:21:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	获取类别id
-- =============================================
ALTER PROCEDURE [dbo].[SP_GetAssetTypeCatId]
@C_GUID NVARCHAR(50),
@InvType NVARCHAR(100),
@AssetType NVARCHAR(100)
--@AID_Flag NVARCHAR(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
	SELECT P.AT_GUID
	FROM dbo.T_AIDTypeRecord p 
	WHERE  C_GUID=@C_GUID 
	AND AidTypeName=@AssetType
	AND Asset_class=@InvType
	--AND AID_FLAG=@AID_Flag
	
END





GO


