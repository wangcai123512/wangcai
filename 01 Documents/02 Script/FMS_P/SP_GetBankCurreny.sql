USE [FMS_Develop]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetBankCurreny]    Script Date: 04/17/2017 19:17:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	获取账户货币
-- =============================================
ALTER PROCEDURE [dbo].[SP_GetBankCurreny]
@ID NVARCHAR(40) = NULL,
@C_ID NVARCHAR(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT B.AccountCurrency
    FROM dbo.T_BankAccount B
    WHERE (B.BA_GUID = @ID OR @ID IS NULL OR LEN(@ID) = 0)
    AND (B.C_GUID = @C_ID)
END
