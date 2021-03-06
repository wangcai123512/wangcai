USE [FMS_Develop]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetPartner]    Script Date: 12/29/2016 14:06:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[SP_GetPartner] 
	-- Add the parameters for the stored procedure here
	@C_GUID NVARCHAR(50),
@ID NVARCHAR(40)=NULL,
@BPName NVARCHAR(100)=NULL,
@IsCustomer NVARCHAR(40)=NULL,
@IsSupplier NVARCHAR(40)=NULL,
@IsPartner NVARCHAR(40)=NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT P.BP_GUID,P.IsCustomer,P.IsPartner,P.IsSupplier,P.Name,ChineseFullName,
	EnglishFullName,Website,OrganizationCode,IndustryInvolved,RegisteredAddress,Remark 
	FROM dbo.T_BusinessPartner P
	WHERE (P.BP_GUID = @ID OR @ID IS NULL OR LEN(@ID) = 0 ) AND P.C_GUID=@C_GUID  
	AND (P.Name = @BPName OR @BPName IS NULL OR LEN(@BPName) = 0 )
	AND (P.IsCustomer= @IsCustomer OR @IsCustomer IS NULL OR LEN(@IsCustomer) = 0 )
	AND (P.IsSupplier = @IsSupplier OR @IsSupplier IS NULL OR LEN(@IsSupplier) = 0 )
	AND (P.IsPartner = @IsPartner OR @IsPartner IS NULL OR LEN(@IsPartner) = 0 )
END
