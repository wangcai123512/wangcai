-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	更新收入费用记录
-- =============================================
 ALTER PROCEDURE [dbo].[SP_UpdReceivablesDeclareCustomer]
	@GUID NVARCHAR(40),
	@InvType NVARCHAR(40),
	@RPer NVARCHAR(40),
	@Date DATETIME=NULL,
	@Amount DECIMAL,
	@Remark NVARCHAR(200)=null,
	@Currency NVARCHAR(20),
	@State NVARCHAR(20),
	@C_GUID NVARCHAR(40),
	@VoucherNo NVARCHAR(40),
	@Record NVARCHAR(40),
	@Profit_GUID NVARCHAR(40)
	
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

    -- Insert statements for procedure here
    BEGIN TRAN;
	DELETE dbo.T_DeclareCustomer WHERE GUID = @GUID;
	
	INSERT INTO dbo.T_DeclareCustomer(GUID,InvType,RPer,[Date],Amount,Remark,Currency,State,C_GUID,VoucherNo,Record,Profit_GUID)
	VALUES(@GUID,@InvType,@RPer,@Date,@Amount,@Remark,@Currency,@State,@C_GUID,@VoucherNo,@Record,@Profit_GUID);
	COMMIT TRAN;
END