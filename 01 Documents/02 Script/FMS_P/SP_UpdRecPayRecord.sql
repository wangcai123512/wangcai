-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	更新收付款记录
-- =============================================
ALTER PROCEDURE [dbo].[SP_UpdRecPayRecord]
	@ID NVARCHAR(40),
	@Flag NVARCHAR(1),
	@InvType NVARCHAR(20) = NULL,
	@InvTypeDts NVARCHAR(50) = NULL,
	@InvNo NVARCHAR(20) = NULL,
	@R_Per NVARCHAR(40) = NULL,
	@DLA NVARCHAR(40)='',
	@DDA NVARCHAR(40)='',
	@CLA NVARCHAR(40)='',
	@CDA NVARCHAR(40)='',
	@Amount DECIMAL(18,4),
	@Date DATE,
	@Remark NVARCHAR(200) = NULL,
	@Creator NVARCHAR(40),
	@CreateDate DATETIME,
	@C_GUID NVARCHAR(50),
	@RPable nvarchar(40)=null,
	@Currency nvarchar(5),
	@CFItemGuid nvarchar(40)=null,
	@CFPItemGuid nvarchar(40)=null,
	@B_GUID NVARCHAR(40),
	@BA_GUID NVARCHAR(40),
	@Record NVARCHAR(40)=null,
	@IE_GUID NVARCHAR(40)=null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	DECLARE @sumAmount  DECIMAL(18,4),
	 @sumAmount1  DECIMAL(18,4),
	 @sumAmount2  DECIMAL(18,4),
	 @sumAmount3  DECIMAL(18,4),
	 @sumAmount4  DECIMAL(18,4),
	 @state NVARCHAR(40)
	
	 if @Flag='P'
	Begin
	SET @state ='已付';
	end
	else
	Begin
	SET @state ='已收';
	end
    BEGIN TRAN;
    DELETE dbo.T_RecPayRecord WHERE RP_GUID = @ID;

	INSERT INTO dbo.T_RecPayRecord(RP_GUID,RP_Flag,InvType,InvTypeDts,Record,InvNo,RPer,SumAmount,Date,Remark,Creator,CreateDate
	,DebitLedgerAccount,DebitDetailsAccount,CreditLedgerAccount,CreditDetailsAccount,
	C_GUID,RPable,Currency,
	CFItemGuid,CFPItemGuid,B_GUID,BA_GUID,IE_GUID)
	VALUES(@ID,@Flag,@InvType,@InvTypeDts,@Record,@InvNo,@R_Per,@Amount,@Date,@Remark,@Creator,@CreateDate
	,@DLA,@DDA,@CLA,@CDA,@C_GUID,@RPable,@Currency,@CFItemGuid,@CFPItemGuid,
	@B_GUID,@BA_GUID,@IE_GUID);

	select @sumAmount= SUM(a.SumAmount) from dbo.T_RecPayRecord a where a.IE_GUID=@IE_GUID;

	select @sumAmount1 = b.Amount from dbo.T_DeclareCostSpending b where b.GUID = @IE_GUID;

	select @sumAmount2 = c.SumAmount from dbo.T_IERecord c where c.IE_GUID = @IE_GUID;

	select @sumAmount3 = d.Amount from dbo.T_DeclareCustomer d where d.GUID = @IE_GUID;
	
	select @sumAmount4 = e.Total from dbo.T_WageCost e where e.W_GUID = @IE_GUID;

	if(@sumAmount>@sumAmount1)
	Begin
	ROLLBACK TRAN;
	return
	end 
	if(@sumAmount>@sumAmount2)
	Begin
	ROLLBACK TRAN;
	return
	end
	if(@sumAmount>@sumAmount3)
	Begin
	ROLLBACK TRAN;
	return
	end 
	if(@sumAmount>@sumAmount4)
	Begin
	ROLLBACK TRAN;
	return
	end 
	if(@sumAmount=@sumAmount1)
	Begin
	update dbo.T_DeclareCostSpending  set State =@state where GUID=@IE_GUID
	end
	if(@sumAmount=@sumAmount3)
	Begin
	update dbo.T_DeclareCustomer set State =@state where GUID = @IE_GUID
	end
	if(@sumAmount=@sumAmount2)
	Begin
	update dbo.T_IERecord set State =@state where IE_GUID = @IE_GUID
	end
	if(@sumAmount=@sumAmount4)
	Begin
	update dbo.T_WageCost set State =@state where W_GUID = @IE_GUID
	update dbo.T_IERecord set State =@state where IE_GUID = @IE_GUID
	end
	COMMIT TRAN;
END