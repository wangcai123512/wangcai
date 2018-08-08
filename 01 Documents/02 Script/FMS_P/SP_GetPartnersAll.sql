USE [FMS_Develop]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetPartnersAll]    Script Date: 12/29/2016 14:29:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	获取所有公司客户
-- =============================================
create PROCEDURE [dbo].[SP_GetPartnersAll]
@C_GUID NVARCHAR(50),
@ID NVARCHAR(40)=NULL,
@BPName NVARCHAR(100)=NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT P.BP_GUID,P.IsCustomer,P.IsPartner,P.IsSupplier,P.Name,ChineseFullName,
	EnglishFullName,Website,OrganizationCode,IndustryInvolved,RegisteredAddress,Remark 
	FROM dbo.T_BusinessPartner P
	WHERE (P.BP_GUID = @ID OR @ID IS NULL OR LEN(@ID) = 0 ) AND P.C_GUID=@C_GUID  AND ( p.IsCustomer = 1 OR p.IsSupplier = 1 OR p.IsPartner = 1 )
	AND (P.Name = @BPName OR @BPName IS NULL OR LEN(@BPName) = 0 )
END