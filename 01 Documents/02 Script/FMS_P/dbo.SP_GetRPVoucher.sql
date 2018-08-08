-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetRPVoucher]
	@PageSize int= -1,
	@PageIndex int = 1,
	@Count int = 0 out,
	@FLAG NVARCHAR(1),
	@RP_GUID NVARCHAR(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @SumAmount decimal(18,2)
	DECLARE @Summary NVARCHAR (100)
	DECLARE @State NVARCHAR (50)
	DECLARE @Name NVARCHAR (100)
	DECLARE @Name2 NVARCHAR (100)
	DECLARE @temp TABLE(
	    rownumber int,
		Summary NVARCHAR (100),
		Name NVARCHAR (50),
		DebitAmount decimal(18,2), --借方金额
		CreditAmount decimal(18,2) --贷方金额
	 )
if(@FLAG= 'R')
BEGIN 	
	 select @State = RC.Record from T_RecPayRecord RC where RC.RP_GUID = @RP_GUID 
	 select @SumAmount =  ISNULL( SUM(ISNULL(RC.SumAmount,0)), 0 ) from T_RecPayRecord RC where RC.RP_GUID = @RP_GUID 
	 select @Summary = '收取'+BP.Name+RC.InvTypeDts from T_RecPayRecord RC LEFT JOIN T_BusinessPartner BP ON RC.RPer =bp.BP_GUID WHERE RC.RP_GUID = @RP_GUID
	 IF @State = '已销账'
	 BEGIN
	 select @Name =CASE RC.BA_GUID when '1'then G.Name else '银行存款'+'-'+RC.B_GUID END from T_IERPRecord IP
	 left join T_RecPayRecord RC on RC.RP_GUID = IP.RP_GUID
	 left join T_GeneralLedgerAccount G on G.LA_GUID=IP.RPLA_GUID and G.C_GUID=RC.C_GUID
     where RC.RP_GUID = @RP_GUID
	 
	 select @Name2 = G.Name from T_IERecord IE 
	 LEFT JOIN T_GeneralLedgerAccount G ON G.LA_GUID = ie.Profit_GUID
	 where IE.IE_GUID = (SELECT IE_GUID FROM T_IERPRecord WHERE RP_GUID=@RP_GUID)
	 
	 select @Name2 = G.Name from T_DeclareCostSpending DS
	 LEFT JOIN T_GeneralLedgerAccount G ON G.LA_GUID = DS.Profit_GUID
	 where DS.GUID = (SELECT IE_GUID FROM T_IERPRecord WHERE RP_GUID=@RP_GUID)
	
	 select @Name2 = G.Name from T_DeclareCustomer DC 
	 LEFT JOIN T_GeneralLedgerAccount G ON G.LA_GUID = DC.Profit_GUID
	 where DC.GUID = (SELECT IE_GUID FROM T_IERPRecord WHERE RP_GUID=@RP_GUID)
	 END
	 ELSE
	 BEGIN
	 select @Name = '应收账款'
	 END
	insert into @temp (rownumber,Summary,Name,DebitAmount)
	select row_number()over(order by @Summary desc),@Summary,@Name2,@SumAmount
	insert into @temp (rownumber,Summary,Name,CreditAmount)
	select row_number()over(order by @Summary desc),@Summary,@Name,@SumAmount
	insert into @temp (rownumber,Summary,Name,DebitAmount,CreditAmount)
    select row_number()over(order by @SumAmount desc),'附单据数'+'     '+'张',@SumAmount,@SumAmount,@SumAmount 
END
if(@FLAG= 'P')
BEGIN 	
	 select @State = RC.Record from T_RecPayRecord RC where RC.RP_GUID = @RP_GUID 
	 select @SumAmount =  ISNULL( SUM(ISNULL(RC.SumAmount,0)), 0 ) from T_RecPayRecord RC where RC.RP_GUID = @RP_GUID 
	 select @Summary = '支付'+BP.Name+RC.InvTypeDts from T_RecPayRecord RC LEFT JOIN T_BusinessPartner BP ON RC.RPer =bp.BP_GUID WHERE RC.RP_GUID = @RP_GUID
	 IF @State = '已销账'
	 BEGIN
	select @Name =CASE RC.BA_GUID when '1'then G.Name else '银行存款'+'-'+RC.B_GUID END from T_IERPRecord IP
	 left join T_RecPayRecord RC on RC.RP_GUID = IP.RP_GUID
	 left join T_GeneralLedgerAccount G on G.LA_GUID=IP.RPLA_GUID and G.C_GUID=RC.C_GUID
     where RC.RP_GUID = @RP_GUID
	 select @Name2 = G.Name from T_IERecord IE 
	 LEFT JOIN T_GeneralLedgerAccount G ON G.LA_GUID = ie.Profit_GUID
	 where IE.IE_GUID = (SELECT IE_GUID FROM T_IERPRecord WHERE RP_GUID=@RP_GUID)
	 
	 select @Name2 = G.Name from T_DeclareCostSpending DS
	 LEFT JOIN T_GeneralLedgerAccount G ON G.LA_GUID = DS.Profit_GUID
	 where DS.GUID = (SELECT IE_GUID FROM T_IERPRecord WHERE RP_GUID=@RP_GUID)
	
	 select @Name2 = G.Name from T_DeclareCustomer DC 
	 LEFT JOIN T_GeneralLedgerAccount G ON G.LA_GUID = DC.Profit_GUID
	 where DC.GUID = (SELECT IE_GUID FROM T_IERPRecord WHERE RP_GUID=@RP_GUID)
	 END
	 ELSE
	 BEGIN
	 select @Name = '应付账款'
	 END
	insert into @temp (rownumber,Summary,Name,DebitAmount)
	select row_number()over(order by @Summary desc),@Summary,@Name2,@SumAmount
	insert into @temp (rownumber,Summary,Name,CreditAmount)
	select row_number()over(order by @Summary desc),@Summary,@Name,@SumAmount
	insert into @temp (rownumber,Summary,Name,DebitAmount,CreditAmount)
    select row_number()over(order by @SumAmount desc),'附单据数'+'     '+'张',@SumAmount,@SumAmount,@SumAmount 
END
	SELECT @Count = COUNT(*) FROM @temp;
    select * from @temp
END