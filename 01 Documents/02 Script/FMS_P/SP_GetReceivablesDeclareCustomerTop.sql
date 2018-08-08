USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_GetReceivablesDeclareCustomerTop]    Script Date: 12/09/2016 11:27:00 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_GetReceivablesDeclareCustomerTop]') AND type IN (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_GetReceivablesDeclareCustomerTop]
GO

USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_GetReceivablesDeclareCustomerTop]    Script Date: 12/09/2016 11:27:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<CHENXIANG>
-- Create date: <2015/09/08>
-- Description:	<查询所有应收账款>
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetReceivablesDeclareCustomerTop]
	-- Add the parameters for the stored procedure here
	@Count INT = 0 OUT,
	@C_GUID NVARCHAR(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
   
    DECLARE @temp TABLE(
    rownumber INT,
    GUID NVARCHAR(40),
    InvType NVARCHAR(40),
    RPer NVARCHAR(40),
    RPerName NVARCHAR(40),
    Amount DECIMAL(18,4),
    Currency NVARCHAR(20),
    State NVARCHAR(20),
    Remark NVARCHAR(200),
    Date VARCHAR(20),
    AGUID NVARCHAR(40),
    VoucherNo NVARCHAR(40)
    )
        
    INSERT INTO @temp
	SELECT ROW_NUMBER()OVER(ORDER BY DC.Date DESC) rownumber,DC.GUID,
	DC.InvType,DC.RPer,BP.Name RPerName,DC.Amount,
	DC.Currency,DC.State,DC.Remark,
	CONVERT(VARCHAR(10),DC.Date,111) AS Date,
	TA.A_GUID AGUID,DC.VoucherNo
	FROM dbo.T_DeclareCustomer DC
	LEFT JOIN dbo.T_BusinessPartner BP ON BP.BP_GUID = DC.RPer
	LEFT JOIN dbo.T_Attachment TA ON TA.FR_GUID = DC.GUID
	WHERE DC.C_GUID=@C_GUID AND dc.State <> '关闭'
	
	SELECT @Count = COUNT(*) FROM @temp;
	SELECT T.GUID,T.InvType,T.RPer,T.RPerName,T.Amount,T.Currency,T.State,T.Remark,T.Date,T.AGUID,T.VoucherNo
		FROM @temp T
	
END

GO


