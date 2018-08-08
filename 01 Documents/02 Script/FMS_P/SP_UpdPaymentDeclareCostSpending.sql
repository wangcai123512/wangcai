USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_UpdPaymentDeclareCostSpending]    Script Date: 02/06/2017 13:34:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<chenxiang>
-- Create date: <Create Date,,>
-- Description:	更新收入费用记录
-- =============================================
ALTER PROCEDURE [dbo].[SP_UpdPaymentDeclareCostSpending]
	@GUID NVARCHAR(40),
	@InvType NVARCHAR(40),
	@RPer NVARCHAR(40),
	@Date DATETIME=NULL,
	@Amount DECIMAL,
	@Remark NVARCHAR(200)=NULL,
	@Currency NVARCHAR(20),
	@State NVARCHAR(20),
	@Record NVARCHAR(20)= NULL,
	@C_GUID NVARCHAR(40),
	@voucherNo NVARCHAR(40)=NULL,
	@Profit_GUID NVARCHAR(40)
	
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

    -- Insert statements for procedure here
    BEGIN TRAN;
	DELETE dbo.T_DeclareCostSpending WHERE GUID = @GUID;
	
	INSERT INTO dbo.T_DeclareCostSpending(GUID,InvType,RPer,Date,Amount,Remark,Currency,State,C_GUID,voucherNo,Record,Profit_GUID,CostSum)
	VALUES(@GUID,@InvType,@RPer,@Date,@Amount,@Remark,@Currency,@State,@C_GUID,@voucherNo,@Record,@Profit_GUID,0);
	COMMIT TRAN;
END



GO


