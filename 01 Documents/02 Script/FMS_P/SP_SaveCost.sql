USE [FMS_Develop]
GO
/****** Object:  StoredProcedure [dbo].[SP_SaveCost]    Script Date: 2017/2/13 16:20:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[SP_SaveCost] 
	@ProductID NVARCHAR (50), --产品id
	@Sumnum DECIMAL(18,4), --售出产品数量
	@C_GUID NVARCHAR (50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @num int
    DECLARE @num1 int
	DECLARE @count int
	DECLARE @count1 int
	DECLARE @qty int --n个产品使用的每种原材料总数
	DECLARE @reper NVARCHAR (50)
	DECLARE @costsum decimal
	DECLARE @state1 NVARCHAR (50)
	DECLARE @state2 NVARCHAR (50)
	DECLARE @Invtype NVARCHAR (50)
	DECLARE @DetailType TABLE(
	    id INT IDENTITY(1, 1) ,
		item_code NVARCHAR(50),
		qty int
	)
	insert into @DetailType (item_code,qty) 
  select p.item_code ,p.qty*@Sumnum AS qty FROM T_Product_Details P 
  where product_id=@ProductID  and level_no !=0
  set @count = (select max(T.id) from @DetailType T);
 
  --select T.id,T.item_code,T.qty from @temp T;
 
    DECLARE @temp2 TABLE(
		id INT IDENTITY(1, 1) ,
		GUID NVARCHAR(50),
		Date datetime,
		Inventory_Number int,
		CostSum decimal(18, 2),
		CostNum int
	)
   set @num=0
    
   while @num<@count 
   
  BEGIN
   set @qty = (select T.qty from @DetailType T where t.id=(@num+1));
	insert into @temp2 (GUID,Date,Inventory_Number,CostSum,CostNum)
   select A.GUID,A.Date,A.Inventory_Number,A.Amount/A.Number*@qty AS CostSum,--CostSum原材料成本金额
    case when A.Inventory_Number-(A.NextNnm-@qty) <0 then 0  when A.NextNnm<@qty then A.Inventory_Number else A.Inventory_Number-(A.NextNnm-@qty)end as CostNum  --CostNum 成本数量
   from (
   select a.*,SUM(ISNULL(b.Number,0))+a.Inventory_Number AS NextNnm from T_AIDRecord a 
   LEFT JOIN T_AIDRecord b on a.Date>b.Date and a.SubType=b.SubType
   where a.SubType=(select b.item_code from @DetailType b where b.id=(@num+1))  
   and a.C_GUID=@C_GUID and a.Inventory_Number IS not NULL 
    group by a.Amount,a.C_GUID,a.Currency,a.Date,a.DepreciationPeriod,a.Description,a.GUID,a.GUID_Parent,a.InvType,a.Number,a.Remark,a.RPer,a.State,a.SubType,a.SurplusValue,a.Inventory_Number,a.CostType
   ) A
   set @num=@num+1   
   END
   select * from @DetailType
   select * from @temp2
   set @count1 = (select max(T.id) from @temp2 T);
   set @num1=0
    
   while @num1<@count1 
   BEGIN
   
   set @reper =(select RPer from T_DeclareCostSpending where GUID = (select T.GUID from @temp2 T where T.id =(@num1+1)))
   set @costsum = (select CostSum from @temp2 where GUID = (select T.GUID from @temp2 T where T.id =(@num1+1)))
   set @state1 = (select State from T_DeclareCostSpending where GUID = (select T.GUID from @temp2 T where T.id =(@num1+1)))
   IF @state1 = '已付'
   BEGIN
   set @state2 = '已付'
   END
   ElSE
   BEGIN
   set @state2 = '应付'
   END
  
   set @Invtype = (select InvType from T_DeclareCostSpending where GUID = (select T.GUID from @temp2 T where T.id =(@num1+1)))
   IF @Invtype = '直接物料采购'
   BEGIN
   set @Invtype = '直接物料'
   END
   ElSE
   BEGIN
   set @Invtype = '间接物料'
   END
   update T_AIDRecord set Inventory_Number = Inventory_Number - (select T.CostNum from @temp2 T where T.id =(@num1+1)) where GUID = (select T.GUID from @temp2 T where T.id =(@num1+1))
   update T_DeclareCostSpending set CostSum = @costsum where GUID = (select T.GUID from @temp2 T where T.id =(@num1+1))
   select * from T_DeclareCostSpending where GUID = (select T.GUID from @temp2 T where T.id =(@num1+1))
   select * from T_AIDRecord  where GUID = (select T.GUID from @temp2 T where T.id =(@num1+1))
   insert into T_IERecord (IE_GUID,IE_Flag,InvType,RPer,C_GUID,CreateDate,AffirmDate,SumAmount,IEGroup,State,GUID_Parent,Profit_GUID)values(newid(),'E','营业成本',@reper,@C_GUID,convert(varchar(10),getdate(),111),convert(varchar(10),getdate(),111),@costsum,@Invtype,@state2,(select T.GUID from @temp2 T where T.id =(@num1+1)),'51BFDD3E-2253-4FBF-A946-19C18C25C6FC')
   select * from T_IERecord where GUID_Parent=(select T.GUID from @temp2 T where T.id =(@num1+1))
   set @num1=@num1+1 
   END

  END
