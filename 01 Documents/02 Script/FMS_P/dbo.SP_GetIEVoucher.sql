-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetIEVoucher]
	@PageSize int= -1,
	@PageIndex int = 1,
	@Count int = 0 out,
	@FLAG NVARCHAR(1),
	@IE_GUID NVARCHAR(40)


AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @SumAmount decimal(18,2)
	DECLARE @IESumAmount decimal(18,2)
	DECLARE @Summary NVARCHAR (100)
	DECLARE @Invtype NVARCHAR (50)
	DECLARE @IEGroup NVARCHAR (50)
	DECLARE @RP_GUID NVARCHAR (50)
	
	 DECLARE @temp TABLE(
	    rownumber int,
		Summary NVARCHAR (100),
		Name NVARCHAR (50),
		DebitAmount decimal(18,2), --借方金额
		CreditAmount decimal(18,2) --贷方金额
	 )
if(@FLAG= 'E')
BEGIN 	
select @SumAmount =  ISNULL( SUM(ISNULL(RC.Amount,0)), 0 ) from T_IERPRecord RC where RC.IE_GUID = @IE_GUID 
select @IESumAmount = IE.SumAmount from T_IERecord IE where IE.IE_GUID = @IE_GUID 
select @Invtype = IE.InvType from T_IERecord IE where IE.IE_GUID = @IE_GUID 
select @IEGroup = D.Name from T_IERecord IE 
left join dbo.T_DetailedAccount D on D.DA_GUID=IE.IEGroup
where IE.IE_GUID = @IE_GUID
if (@Invtype = '税费')
BEGIN
select @Summary = '应付税费：'+IE.IEGroup from T_IERecord IE WHERE IE.IE_GUID = @IE_GUID
END
if(@Invtype = '初始账款')
BEGIN
set @Summary = '应付初始账款'
END
if(@Invtype not in ('初始账款','税费'))
BEGIN
select @Summary = ISNULL(IE.Summary,'应付'+BP.Name+D.Name) from T_IERecord IE LEFT JOIN T_BusinessPartner BP ON IE.RPer =bp.BP_GUID
left join dbo.T_DetailedAccount D on D.DA_GUID=IE.IEGroup
 WHERE IE.IE_GUID = @IE_GUID
END
if(@Invtype != '初始账款')
BEGIN
insert into @temp (rownumber,Summary,Name,DebitAmount)
select row_number()over(order by IE.Date desc),@Summary,ISNULL(G.Name,'增值税'),IE.SumAmount from T_IERecord IE
LEFT JOIN T_BusinessPartner BP ON IE.RPer =bp.BP_GUID
LEFT JOIN T_GeneralLedgerAccount G ON G.LA_GUID = IE.Profit_GUID
WHERE IE.IE_GUID = @IE_GUID
END
ELSE
BEGIN
insert into @temp (rownumber,Summary,Name,DebitAmount)
select row_number()over(order by IE.Date desc),@Summary,'初始账款',IE.SumAmount from T_IERecord IE
LEFT JOIN T_BusinessPartner BP ON IE.RPer =bp.BP_GUID
LEFT JOIN T_GeneralLedgerAccount G ON G.LA_GUID = IE.Profit_GUID
WHERE IE.IE_GUID = @IE_GUID
END
if @SumAmount>0
BEGIN

insert into @temp (rownumber,Summary,Name,CreditAmount)
--select row_number()over(order by RC.Date desc),@Summary,'银行存款'+'-'+RC.B_GUID,Sum(IP.Amount) from T_IERPRecord IP
--left join T_RecPayRecord RC on RC.RP_GUID = IP.RP_GUID 
--where IP.IE_GUID=@IE_GUID 
select row_number()over(order by RC.B_GUID desc),@Summary,CASE RC.BA_GUID when '1'then G.Name else '银行存款'+'-'+RC.B_GUID END,Sum(IP.Amount) from T_IERPRecord IP
left join T_RecPayRecord RC on RC.RP_GUID = IP.RP_GUID
left join T_GeneralLedgerAccount G on G.LA_GUID=IP.RPLA_GUID and G.C_GUID=RC.C_GUID
where IP.IE_GUID=@IE_GUID 
group by RC.B_GUID,RC.BA_GUID,G.Name
END 
if @SumAmount <> @IESumAmount
BEGIN 
if (@Invtype = '税费')
BEGIN
insert into @temp (rownumber,Summary,Name,CreditAmount)
select row_number()over(order by @Summary desc),@Summary,'应交税费',@IESumAmount-@SumAmount
END
ELSE
BEGIN
insert into @temp (rownumber,Summary,Name,CreditAmount)
select row_number()over(order by @Summary desc),@Summary,'应付账款',@IESumAmount-@SumAmount
END
 
END
insert into @temp (rownumber,Summary,Name,DebitAmount,CreditAmount)
select row_number()over(order by @IESumAmount desc),'附单据数'+'     '+'张',@IESumAmount,@IESumAmount,@IESumAmount 
END
if(@FLAG= 'I')
BEGIN 	
select @SumAmount =  ISNULL( SUM(ISNULL(RC.Amount,0)), 0 ) from T_IERPRecord RC where RC.IE_GUID = @IE_GUID 
select @IESumAmount = IE.SumAmount from T_IERecord IE where IE.IE_GUID = @IE_GUID 
select @Summary = ISNULL(IE.Summary,'应收'+BP.Name) from T_IERecord IE LEFT JOIN T_BusinessPartner BP ON IE.RPer =bp.BP_GUID WHERE IE.IE_GUID = @IE_GUID
 insert into @temp (rownumber,Summary,Name,CreditAmount)
select row_number()over(order by IE.Date desc),@Summary,G.Name,IE.SumAmount from T_IERecord IE
LEFT JOIN T_BusinessPartner BP ON IE.RPer =bp.BP_GUID
LEFT JOIN T_GeneralLedgerAccount G ON G.LA_GUID = ie.Profit_GUID
WHERE IE.IE_GUID = @IE_GUID
if @SumAmount>0
BEGIN
insert into @temp (rownumber,Summary,Name,DebitAmount)
select row_number()over(order by RC.B_GUID desc),@Summary,CASE RC.BA_GUID when '1'then G.Name else '银行存款'+'-'+RC.B_GUID END,Sum(IP.Amount) from T_IERPRecord IP
left join T_RecPayRecord RC on RC.RP_GUID = IP.RP_GUID
left join T_GeneralLedgerAccount G on G.LA_GUID=IP.RPLA_GUID and G.C_GUID=RC.C_GUID
where IP.IE_GUID=@IE_GUID 
group by RC.B_GUID,RC.BA_GUID,G.Name
END 
if @SumAmount <> @IESumAmount
BEGIN 
insert into @temp (rownumber,Summary,Name,DebitAmount)
select row_number()over(order by @Summary desc),@Summary,'应收账款',@IESumAmount-@SumAmount 
END
insert into @temp (rownumber,Summary,Name,DebitAmount,CreditAmount)
select row_number()over(order by @IESumAmount desc),'附单据数'+'     '+'张',@IESumAmount,@IESumAmount,@IESumAmount 
END
SELECT @Count = COUNT(*) FROM @temp;
select * from @temp

END