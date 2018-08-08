USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_GetAIDRecordDetail]    Script Date: 01/09/2017 17:48:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,hudongyang>
-- Create date: <Create Date,,11/24/2016>
-- Description:	获取采购详情记录
-- =============================================
ALTER PROCEDURE [dbo].[SP_GetAIDRecordDetail]
	@ID NVARCHAR(40),
	@C_GUID NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here	
    SELECT GUID,a.C_GUID,c.AID_Flag,Amount,Currency,RPer,InvType,Description,
    DepreciationPeriod,SurplusValue,State,a.Remark,CostType,GUID_Parent,SubType,c.Asset_class,AidTypeName,
    CONVERT(VARCHAR(10),Date,111) AS Date,(d.Depreciation_year) AS Depreciation_year 
	FROM dbo.T_AIDRecord a	
	INNER JOIN dbo.T_AIDTypeRecord c ON a.InvType = c.AT_GUID
    INNER JOIN dbo.T_AIDSubTypeRecord d ON a.SubType = d.AST_GUID
    WHERE a.C_GUID=@C_GUID 
    AND(a.GUID=@ID)
END



GO


