/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2008                    */
/* Created on:     2014/12/17 14:44:16                          */
/*==============================================================*/


/*==============================================================*/
/* Table: T_Assets                                              */
/*==============================================================*/
create table T_Assets (
   A_GUID               nvarchar(40)         not null,
   No                   nvarchar(100)        null,
   Name                 nvarchar(100)        null,
   RegisterDate         datetime             null,
   ScrapType            nvarchar(10)         null,
   ScrapDate            datetime             null,
   AG_GUID              nvarchar(40)         null,
   SalvageValue         decimal(18,4)        null,
   AssetsCost           decimal(18,4)        null,
   Creator              nvarchar(40)         null,
   constraint PK_T_ASSETS primary key nonclustered (A_GUID)
)
go

/*==============================================================*/
/* Table: T_AssetsGroup                                         */
/*==============================================================*/
create table T_AssetsGroup (
   AG_GUID              nvarchar(40)         not null,
   Name                 nvarchar(50)         null,
   DepreciationMethod   int                  null,
   Life                 int                  null,
   constraint PK_T_ASSETSGROUP primary key nonclustered (AG_GUID)
)
go

/*==============================================================*/
/* Table: T_BusinessPartner                                     */
/*==============================================================*/
create table T_BusinessPartner (
   BP_GUID              nvarchar(40)         not null,
   Name                 nvarchar(100)        null,
   IsSupplier           bit                  null,
   IsCustomer           bit                  null,
   IsPartner            bit                  null,
   constraint PK_T_BUSINESSPARTNER primary key nonclustered (BP_GUID)
)
go

/*==============================================================*/
/* Table: T_DetailedAccount                                     */
/*==============================================================*/
create table T_DetailedAccount (
   DA_GUID              nvarchar(40)         not null,
   Name                 nvarchar(100)        null,
   ParentAccGuid        nvarchar(40)         null,
   constraint PK_T_DETAILEDACCOUNT primary key nonclustered (DA_GUID)
)
go

/*==============================================================*/
/* Table: T_ExpenditureDetails                                  */
/*==============================================================*/
create table T_ExpenditureDetails (
   ER_GUID              nvarchar(40)         not null,
   DebitLedgerAccount   nvarchar(40)         null,
   DebitDetailsAccount  nvarchar(40)         null,
   CreditLedgerAccount  nvarchar(40)         null,
   CreditDetailsAccount nvarchar(40)         null,
   Money                decimal(18,4)        null,
   constraint PK_T_EXPENDITUREDETAILS primary key nonclustered (ER_GUID)
)
go

/*==============================================================*/
/* Table: T_ExpenditureRecord                                   */
/*==============================================================*/
create table T_ExpenditureRecord (
   ER_GUID              nvarchar(40)         not null,
   InvType              nvarchar(20)         null,
   InvNo                nvarchar(20)         null,
   Payee                nvarchar(40)         null,
   Money                decimal(18,4)        null,
   Tax                  decimal(18,4)        null,
   Date                 datetime             null,
   Creater              nvarchar(40)         null,
   CreateDate           datetime             null,
   IsRecorded           bit                  null,
   constraint PK_T_EXPENDITURERECORD primary key nonclustered (ER_GUID)
)
go

/*==============================================================*/
/* Table: T_GeneralLedgerAccount                                */
/*==============================================================*/
create table T_GeneralLedgerAccount (
   AccCode              int                  not null,
   Name                 nvarchar(100)        null,
   Useable				bit					 not null,	
   constraint PK_T_GENERALLEDGERACCOUNT primary key nonclustered (AccCode)
)
go

/*==============================================================*/
/* Table: T_IncomeDetails                                       */
/*==============================================================*/
create table T_IncomeDetails (
   IR_GUID              nvarchar(40)         not null,
   DebitLedgerAccount   nvarchar(40)         null,
   DebitDetailsAccount  nvarchar(40)         null,
   CreditLedgerAccount  nvarchar(40)         null,
   CreditDetailsAccount nvarchar(40)         null,
   Money                decimal(18,4)        null,
   constraint PK_T_INCOMEDETAILS primary key nonclustered (IR_GUID)
)
go

/*==============================================================*/
/* Table: T_IncomeRecord                                        */
/*==============================================================*/
create table T_IncomeRecord (
   IR_GUID              nvarchar(40)         not null,
   InvType              nvarchar(20)         null,
   InvNo                nvarchar(20)         null,
   Payer                nvarchar(40)         null,
   Amount               decimal(18,4)        null,
   Tax                  decimal(18,4)        null,
   Date                 datetime             null,
   Creater              nvarchar(40)         null,
   CreaterDate          datetime             null,
   IsRecorded           bit                  null,
   constraint PK_T_INCOMERECORD primary key nonclustered (IR_GUID)
)
go

/*==============================================================*/
/* Table: T_Report                                              */
/*==============================================================*/
create table T_Report (
   Rep_GUID             nvarchar(40)         not null,
   Type                 nvarchar(20)         null,
   Year                 int                  null,
   Month                int                  null,
   constraint PK_T_REPORT primary key nonclustered (Rep_GUID)
)
go

/*==============================================================*/
/* Table: T_ReportDetails                                       */
/*==============================================================*/
create table T_ReportDetails (
   RGUID                nvarchar(40)         not null,
   Rep_GUID             nvarchar(40)         not null,
   Code                 nvarchar(20)         null,
   Value                decimal(18,4)        null,
   constraint PK_T_REPORTDETAILS primary key nonclustered (RGUID)
)
go

/*==============================================================*/
/* Table: T_ModuleList                                       */
/*==============================================================*/
CREATE TABLE [dbo].[T_ModuleList](
	[Guid] [nvarchar](40) NOT NULL,
	[ChineseName] [nvarchar](200) NULL,
	[EnglishName] [nvarchar](200) NULL,
	[OrderNumber] [int] NULL,
	[ModuleID] [nvarchar](40) NULL,
	[IsShowTree] [bit] NULL,
	[IsLastChild] [bit] NULL,
	[URL] [nvarchar](200) NULL,
	[SubfunctionCode] [nvarchar](200) NULL,
	[ModuleState] [int] NULL,
	[Level] [int] NULL,
 CONSTRAINT [PK_T_ModuleList] PRIMARY KEY CLUSTERED ([Guid] ASC)
)
go
-----------------------------------------------------------------------
INSERT INTO T_GeneralLedgerAccount(lA_GUID,AccCode,Name,Useable)
SELECT '0258BA34-D874-41B1-9703-80F74D93A9AF', 1003, '存放中央银行存款', 0 UNION
SELECT '0316D846-1E9D-41DE-8ED4-EE1D620382E7', 2232, N'应付股利', 0 UNION 
SELECT '082CD9EB-9947-43C4-A7C6-F2B7FAB6EE54', 6801, N'所得税费用', 0 UNION
SELECT '0BEED359-02DC-48EB-9773-73E010099385', 2101, N'交易性金融负债', 0 UNION
SELECT '132B92DE-1469-411F-A75F-C04B61E507D1', 6403, N'营业税金及附加', 0 UNION
SELECT '1433E395-62B0-451E-BCB3-24B7CD5E97C4', 1411, N'周转材料', 0 UNION
SELECT '1522A65D-72DF-4495-992B-2849F92FB42B', 3002, N'货币兑换', 0 UNION
SELECT '177F9162-46D6-469B-95D7-60AD19032B55', 6031, N'保费收入', 0 UNION
SELECT '1837010F-8238-4FB8-97E3-47062A7CD3BA', 1502, N'持有至到期投资减值准备', 0 UNION
SELECT '189C2786-40AA-4A81-895D-FEDABAF12F88', 1311, N'代理兑付证券', 0 UNION
SELECT '18C8CD38-507D-49F3-BDEF-B659459589BE', 6541, N'分出保费', 0 UNION
SELECT '1AC24C5E-FAB4-43EA-A303-A448604FC6C2', 6402, N'其他业务成本', 0 UNION
SELECT '1ADC295B-B911-4CFA-B390-057EF7AC1195', 1441, N'抵债资产', 0 UNION
SELECT '1BFB4755-65F1-43B5-9B8B-E330DBD75AF1', 2312, N'代理承销证券款', 0 UNION
SELECT '1F500FDD-1460-45DC-BE8F-39F5ACCE5D95', 4104, N'利润分配', 0 UNION
SELECT '1F5BA064-5C95-4D8D-9F4B-E0D73C964B66', 1521, N'持有至到期投资', 0 UNION
SELECT '1F5BA064-5C95-4D8D-9F4B-E0D73C964B66', 1526, N'投资性房地产', 0 UNION
SELECT '20201B8A-D383-48C6-9127-1F22521EA6D7', 1131, N'应收股利', 0 UNION
SELECT '205A3E43-05E1-4FCE-8581-B96C0F25D233', 1623, N'公益性生物资产', 0 UNION
SELECT '2253B4EC-A168-4D6F-965C-46E174EB926C', 1303, N'贷款', 0 UNION
SELECT '2348E907-8C11-4910-83CE-7291EEF502E9', 2012, N'同业存放', 0 UNION
SELECT '239825A2-6DB2-438B-9980-198BCDE909BF', 1606, N'固定资产清理', 0 UNION
SELECT '260959BD-0F68-4F14-A641-48C0A373AA7A', 1821, N'独立账户资产', 0 UNION
SELECT '267C6497-5821-4251-9A96-13CF100CEB5E', 4102, N'一般风险准备', 0 UNION
SELECT '271FDD30-2FAC-49B5-9F46-023424ABB4C7', 1122, N'应收账款', 0 UNION
SELECT '28AFA3DD-2FFB-4A0A-9440-9550BE626B56', 5402, N'工程结算', 0 UNION
SELECT '28C49483-7FD7-4741-B154-B36EF52469D2', 6051, N'其他业务收入', 0 UNION
SELECT '2B75530A-895D-4EB6-8247-EA69F33B1933', 1321, N'代理业务资产', 0 UNION
SELECT '2D074DF6-6823-4F2A-90C6-D2C625BD19FC', 3001, N'清算资金往来', 0 UNION
SELECT '2D2C780B-8191-4A5A-A1EE-0A2D969F2299', 2211, N'应付职工薪酬', 0 UNION
SELECT '2EB560DB-09AA-4429-92AD-D9F45798A96E', 2401, N'递延收益', 0 UNION
SELECT '2FE56E14-5D86-4CA9-847F-EAA28256F4CE', 2201, N'应付票据', 0 UNION
SELECT '32B7AC9E-B1F0-4581-A027-298029C6FE4A', 1408, N'委托加工物资', 0 UNION
SELECT '33CBFEF1-6B00-4B50-AD4F-3C98830F8B05', 2202, N'应付账款', 0 UNION
SELECT '343E6161-E1EE-48F0-841C-2AF338C8090A', 2003, N'拆入资金', 0 UNION
SELECT '3748F45E-60F3-4B24-818B-115B8A904DA4', 5001, N'生产成本', 0 UNION
SELECT '3B2BD2C9-C7BB-4186-942F-67405AB06BAA', 2111, N'卖出回购金融资产款', 0 UNION
SELECT '3C0EE546-CCF5-43BD-81AA-6E9F0637763A', 6542, N'分保费用', 0 UNION
SELECT '40D3F836-6ED1-49EE-9F36-154942D095FB', 2602, N'保险责任准备金', 0 UNION
SELECT '41C968F2-7D51-4F9F-83B6-EC0F4381ECD0', 1401, N'材料采购', 0 UNION
SELECT '435FAECB-01CF-418D-A8AE-7EC40CF60E49', 6531, N'退保金', 0 UNION
SELECT '45318399-83BA-4E96-A1E8-A15085CD41FA', 1301, N'贴现资产', 0 UNION
SELECT '475710EF-0DAD-4E3A-966B-38224C68A2A2', 3201, N'套期工具', 0 UNION
SELECT '47F46089-EB1F-43F1-A81B-D4B5111C3967', 6021, N'手续费及佣金收入', 0 UNION
SELECT '4A134472-92C0-4A48-ABF6-29296C0EDDDF', 1021, N'结算备付金', 0 UNION
SELECT '4AB5339C-C052-42C4-9381-3B93298697B2', 2002, N'存入保证金', 0 UNION
SELECT '4D76BAF2-C55A-4371-B532-A27125D33E6E', 6501, N'提取未到期责任准备金', 0 UNION
SELECT '4D955E56-C09A-471A-97E8-9FEACFD402B4', 2251, N'应付保单红利', 0 UNION
SELECT '4EC68325-B615-4EAD-B749-12312FD970E7', 1212, N'应收分保合同准备金', 0 UNION
SELECT '4EFD6B49-DC29-4B59-B0B7-CBAA67464893', 1532, N'为实现融资收益', 0 UNION
SELECT '4F380EB2-C1BC-483C-B229-A7FAEA03D054', 6711, N'营业外支出', 0 UNION
SELECT '503F8962-B248-41AC-B647-17D694B8AB3A', 5403, N'机械作业', 0 UNION
SELECT '51BE12F2-E98A-493E-9447-215461537284', 1621, N'生产性生物资产', 0 UNION
SELECT '51BFDD3E-2253-4FBF-A946-19C18C25C6FC', 6401, N'主营业务成本', 0 UNION
SELECT '53E9B5B2-C557-40F4-BCC4-4B5A8F326BD3', 1703, N'无形资产减值准备', 0 UNION
SELECT '541DB837-64F4-4D8B-A9E4-BE2AF758AB81', 1632, N'累计折耗', 0 UNION
SELECT '547E5A1A-1C20-4249-92C8-67FFFFBD38E7', 6602, N'管理费用', 0 UNION
SELECT '5BE3D3F4-876E-479F-9C8D-94EB25CDCBF9', 2011, N'吸收存款', 0 UNION
SELECT '5D572C97-33E3-47AB-A159-95B1479D2068', 1132, N'应收利息', 0 UNION
SELECT '5E52B8E4-7429-43E9-961D-77DF104D6219', 1302, N'拆出资金', 0 UNION
SELECT '60E8BAAA-2043-479F-9D13-E9753F5BA512', 4001, N'实收资本', 0 UNION
SELECT '6239D693-34AB-481D-A8B6-A8504896010D', 1031, N'存出保证金', 0 UNION
SELECT '65BC8EDB-949A-4EF2-BCCD-AB5FE10DC88E', 6301, N'营业外收入', 0 UNION
SELECT '69ADF21A-A463-4B20-9483-B8AB501F6F5C', 2231, N'应付利息', 0 UNION
SELECT '6AE7EF4C-1E46-4839-951A-6514CAF6F6A1', 1511, N'长期股权投资', 0 UNION
SELECT '6C6E8F07-3226-4BA9-B175-36F106EE09FD', 1111, N'买入返售金融资产', 0 UNION
SELECT '6D76DBA0-C465-49AC-AC90-BEF2C23B9C04', 2311, N'代理买卖证券款', 0 UNION
SELECT '6EE7EC93-9E59-4691-9458-541C759852FF', 2502, N'应付债务', 0 UNION
SELECT '6F1405AD-3AE4-4F7B-B9A1-C869D30C526D', 2901, N'递延所得税负债', 0 UNION
SELECT '6FC75464-5F98-42A7-860F-7DA10C5F1CD5', 6203, N'摊回分保费用', 0 UNION
SELECT '72BB4117-4320-4A71-A1F8-D0096B16A7B2', 1461, N'融资租赁资产', 0 UNION
SELECT '731350A2-EE43-4F7A-8A12-1610F3B471C4', 6502, N'提取保险责任准备金', 0 UNION
SELECT '73A6ADFF-A5EC-4B1B-ACAE-977DC69B3CAA', 2021, N'贴现负债', 0 UNION
SELECT '73F97A8C-0E8E-4630-9C4C-113225297422', 4201, N'库存股', 0 UNION
SELECT '740CF627-461C-4134-B2AC-A5EE5B1E05D2', 1431, N'贵金属', 0 UNION
SELECT '76929BB2-2DF1-43BF-B33B-2F9D3FB851FB', 2241, N'其他应付款', 0 UNION
SELECT '76FF2B6B-932A-4CA1-850C-758310496AEB', 1002, N'银行存款', 0 UNION
SELECT '794F3D3A-D3AD-431F-988C-9CFBC2A1D207', 6011, N'利息收入', 0 UNION
SELECT '79BAEF27-D79B-4A69-9862-C154C609B1CF', 2261, N'应付分保账款', 0 UNION
SELECT '79C23856-8E3C-4AC8-905C-7681C1D1F565', 2501, N'长期借款', 0 UNION
SELECT '7C252091-900B-460C-8DBB-9F0DA2DC5506', 4002, N'资本公积', 0 UNION
SELECT '7CD10C46-03C5-4657-8563-FC443F86B5C5', 6041, N'租赁收入', 0 UNION
SELECT '7E6B5233-DC38-44F5-BF18-CD087A8B84DF', 1211, N'应收分保账款', 0 UNION
SELECT '7E6BB639-808E-4A35-8772-4B3C0223A069', 1811, N'递延所得税资产', 0 UNION
SELECT '806373A3-41D2-4F40-AC51-2C4C82E318DE', 6421, N'手续费及佣金支出', 0 UNION
SELECT '80FC4775-3169-42B0-A558-F3AE813DBF14', 1221, N'其他应收款', 0 UNION
SELECT '81669133-D2BC-47BB-9A95-371CA99F90C6', 1541, N'存出资本保证金', 0 UNION
SELECT '83AEB2C9-4958-4595-897F-5257090C5828', 2001, N'短期借款', 0 UNION
SELECT '84C4921A-B597-40A5-8A4F-647D7DDE7997', 2313, N'代理兑付证券款', 0 UNION
SELECT '859C6612-A31E-4D9E-8D60-4AC48C73E2FC', 1231, N'坏账准备', 0 UNION
SELECT '85BDCCDC-9D49-48F0-83A4-3F47B203DDC9', 1801, N'长期待摊费用', 0 UNION
SELECT '8669965A-8B58-4D3E-A0E6-9657654854E1', 1304, N'贷款损失准备', 0 UNION
SELECT '86D57E6A-6207-46CF-A7E0-03C36A10DBBF', 6061, N'汇兑损益', 0 UNION
SELECT '88C60FC8-2FCB-41CD-B721-C58A981961B0', 1123, N'预付账款', 0 UNION
SELECT '8AD73A3D-0969-4C4A-8474-449221E8F598', 3101, N'衍生工具', 0 UNION
SELECT '8E8E560D-061A-4EB7-B07D-F5C652F8C89A', 1622, N'生产性生物资产累计折旧', 0 UNION
SELECT '989F9B14-E584-4C42-AB26-2A934008B111', 6411, N'利息支出', 0 UNION
SELECT '994F056A-1461-4E6C-A25B-B81B90BBBB63', 4103, N'本年利润', 0 UNION
SELECT '9C34B37B-B923-404B-BE81-21B1F40EAA6C', 1531, N'长期应收款', 0 UNION
SELECT 'A0E772D8-AB1F-4060-AAC1-4E91417A9679', 6701, N'资产减值损失', 0 UNION
SELECT 'A4B9C617-7AC2-45B3-8F82-36811E6D4C81', 2611, N'保户储金', 0 UNION
SELECT 'A70D3F7D-3398-4A9E-AED5-FA304BF49218', 2314, N'代理业务负债', 0 UNION
SELECT 'A7C86EFD-D448-4784-AFC2-758FCA90D9B8', 2221, N'应交税费', 0 UNION
SELECT 'A8C6C022-826B-483A-84EF-D0FA81A60E64', 2801, N'预计负债', 0 UNION
SELECT 'AFD5C62D-6B25-4654-BEFC-F1B137ECEF0C', 5201, N'劳务成本', 0 UNION
SELECT 'B04A872E-0FFB-45EE-A8C5-968CAF2CCDAC', 5101, N'制造费用', 0 UNION
SELECT 'B1C31745-6E4F-490C-9E9C-A616CEE414A2', 1407, N'商品进销差价', 0 UNION
SELECT 'B1F44906-51D6-47F4-B6EC-7B678B5E7CD5', 1701, N'无形资产', 0 UNION
SELECT 'B2FC372F-EB9F-4005-B66F-FC07F44BD711', 4101, N'盈余公积', 0 UNION
SELECT 'B3DF2F07-8FF0-4A39-849A-CC3B667DFD47', 6202, N'摊回赔付支出', 0 UNION
SELECT 'B571A051-F410-4A43-BF8A-5BC3172CA4AE', 1011, N'存放同业', 0 UNION
SELECT 'B5920041-15AC-45A8-AC5C-8E8E83AB9076', 1702, N'累计摊销', 0 UNION
SELECT 'B5F88808-B376-464B-8BFC-A900AF37BC03', 1405, N'库存商品', 0 UNION
SELECT 'B62C76EB-E285-4401-A67D-7C3EF47D02F1', 1403, N'原材料', 0 UNION
SELECT 'B78669D1-4E4A-41E6-BF6D-12E79304B87E', 1406, N'发出商品', 0 UNION
SELECT 'B81DA2B4-8E4B-4685-ACFB-E0056BD01DA3', 1121, N'应收票据', 0 UNION
SELECT 'BB7BD724-8B46-4B53-B476-6123863A0F77', 6201, N'摊回保险责任准备金', 0 UNION
SELECT 'BBCE9EEB-5DC9-4C48-9A98-E7A50AEE3CA1', 1605, N'工程物资', 0 UNION
SELECT 'BC75A24C-FC9A-4CE6-9D62-063AC34EFE9D', 1603, N'固定资产减值准备', 0 UNION
SELECT 'BF3D6765-8D67-484D-8783-A7F82796EF95', 6111, N'投资收益', 0 UNION
SELECT 'C1BFAD58-6668-4FC3-9C46-2451E550FD71', 1201, N'应收代位追偿款', 0 UNION
SELECT 'C2CDA7BC-C4D0-4B72-91CD-5EDEE7ED3B6A', 1602, N'累计折旧', 0 UNION
SELECT 'C41EC0EA-9738-4C29-9A18-DD8F3FA19EDB', 1503, N'可供出售金融资产', 0 UNION
SELECT 'C70456B5-0FAF-4605-AE51-2A9F27B1E1B0', 2702, N'未确认融资费用', 0 UNION
SELECT 'C87533DF-211A-4E6B-9C16-C6CE8F4E017E', 1421, N'消耗性生物资产', 0 UNION
SELECT 'CC935E8A-3316-459E-BD5C-07A90E75FC2F', 1404, N'材料成本差异', 0 UNION
SELECT 'CCC98407-5F5A-4B14-96BA-9A1063BA6407', 1611, N'未担保余值', 0 UNION
SELECT 'CD0D907D-5ED1-4785-ACFB-F59C2A8B920C', 6901, N'以前年度损益调整', 0 UNION
SELECT 'D27CA8F5-A98C-41E4-8E49-E0BE34E93035', 6001, N'主营业务收入', 0 UNION
SELECT 'D5A9A0E4-68EE-4027-9122-AD9761A507D8', 2601, N'未到期责任准备金', 0 UNION
SELECT 'D5FD986C-D385-473E-B201-2531E06AF009', 2004, N'向中央银行借款', 0 UNION
SELECT 'D68CF0AD-77F7-42C5-A1B3-2C4D1E2406BC', 2701, N'长期应付款', 0 UNION
SELECT 'DB482691-775B-4EF9-9A6A-2A192AD2784A', 6101, N'公允价值变动损益', 0 UNION
SELECT 'DB57CF51-0328-4F82-842B-4710FB65AAA7', 2711, N'专项应付款', 0 UNION
SELECT 'DBA20768-A354-4157-B98C-3756464D1F3D', 5401, N'工程施工', 0 UNION
SELECT 'DC83D8A5-31F6-4DFE-B093-87F90A234E53', 6601, N'销售费用', 0 UNION
SELECT 'DCA7F8E1-E5D6-441A-8EF1-1EAD19C650FB', 1512, N'长期股权投资减值准备', 0 UNION
SELECT 'DD629154-AA54-47C9-B4C8-5E1593675357', 1711, N'商誉', 0 UNION
SELECT 'DE322C45-F282-485C-8CBB-974D7E58B7E3', 5301, N'研发支出', 0 UNION
SELECT 'DEBF693F-E812-4AEF-A490-2B1C969420B4', 3202, N'被套期项目', 0 UNION
SELECT 'DF4C9ED8-880B-4FF9-90CC-5059887097CF', 1471, N'存货跌价准备', 0 UNION
SELECT 'E22214AB-E8A0-4AE3-9A74-1B621F0EE972', 1101, N'交易性金融资产', 0  UNION
SELECT 'EB017F0E-D547-42EF-B84B-9ABBE7A2641D', 1402, N'在途物资', 0  UNION
SELECT 'EBFB80B1-1EC5-41A6-9C3B-5FA6830C0530', 1501, N'待摊费用', 0  UNION
SELECT 'ED86780D-5973-4374-A219-E3D638BE7558', 6511, N'赔付支出', 0 UNION
SELECT 'EE193A4A-FF73-4159-8C5C-435B16E1F6E7', 6604, N'勘探费用', 0 UNION
SELECT 'EF58B046-0BDE-464E-96D2-8F731366349A', 2203, N'预收账款', 0 UNION
SELECT 'EF6FE005-71D3-4A42-A713-76C295A4FBA1', 1901, N'待处理财产损益', 0 UNION
SELECT 'F6BCE485-1830-40CC-9C90-0CB371FAA302', 1012, N'其他货币资金', 0 UNION
SELECT 'F780C9C6-854A-4787-BFFC-4D343D6CC987', 1451, N'损余物资', 0 UNION
SELECT 'F80A9F02-898B-4221-94DB-8C4AD8F20D6E', 1631, N'油气资产', 0 UNION
SELECT 'F85560AA-4951-4214-AF7F-5B890C9524B2', 6603, N'财务费用', 0 UNION
SELECT 'F87B2195-F0BD-4DB8-A55C-CFF991C70D75', 2621, N'独立账户负债', 0 UNION
SELECT 'F8D027C5-7329-4A1D-AEB7-25662B577705', 1604, N'在建工程', 0 UNION
SELECT 'F9E8B745-CB32-449E-B410-209B9D43B7A3', 1001, N'库存现金', 0 UNION
SELECT 'FBB692FD-669B-4E64-B7E9-77A5AF76BA30', 6521, N'保单红利支出', 0 UNION
SELECT 'FD7ED40C-6BA4-4C22-8029-6F1509441BEA', 1601, N'固定资产', 0 

INSERT [T_ModuleList] ([GUID], [ChineseName], [EnglishName], [OrderNumber], [ModuleID], [IsShowTree], [IsLastChild], [URL], [SubfunctionCode], [ModuleState], [Level]) 
--
SELECT '28192530-A4E3-4B00-B17C-40A622D426CB','商业伙伴管理','Business Partner Management',102,NULL,1,0,NULL,NULL,1,1 UNION
SELECT '36C862B2-797A-4206-8C46-CBBC2DF9D695','设置商业伙伴','Business Partner Setting',10201,'28192530-A4E3-4B00-B17C-40A622D426CB',1,1,'/BusinessPartnerSetting/Index','Business_Partner_Setting',2,2 UNION
--
SELECT 'F9D11E38-AA0B-41BB-A340-87103206403F','基本设置','Base Setting',103,NULL,1,0,NULL,NULL,1,1 UNION
SELECT 'EC1DADF3-394D-4371-B640-4D68C2055C23','科目设置','Account Setting',103,'F9D11E38-AA0B-41BB-A340-87103206403F',1,0,NULL,NULL,1,2 UNION
SELECT '3C81E819-649F-44A0-8A5D-A4967DA256AB','总账科目','General Ledger Account',10301,'EC1DADF3-394D-4371-B640-4D68C2055C23',1,1,'/GeneralLedgerAccount/Index','General_Ledger_Account',2,3 UNION
SELECT 'CA0E31E8-9D74-4C0F-8D29-ACBD17BF4C2B','明细科目','Detailed Account',10302,'EC1DADF3-394D-4371-B640-4D68C2055C23',1,1,'/DetailedAccount/Index','Detailed_Account',2,3 UNION
SELECT '2B0E21E8-9A84-4C3F-8A23-A1BD17B44C2B','营业成本明细','Business Cost',10303,'EC1DADF3-394D-4371-B640-4D68C2055C23',1,1,'/BusinessCost/Index','Business_Cost',2,3 UNION
--
SELECT '78C9F455-C2D7-451D-AB46-F38ACCC0A92F','收入管理','Income Management',104,NULL,1,0,NULL,NULL,1,1 UNION
SELECT '0475805C-F329-413D-8D30-00D4DC79EABA','记录收入','Income Record',10401,'78C9F455-C2D7-451D-AB46-F38ACCC0A92F',1,1,'/IncomeRecord/Index','Income_Record',2,2 UNION
SELECT '8CC40D5A-C59C-4E6D-AC55-F5C7D7B6A388','核销收入','Income Witer Off',10402,'78C9F455-C2D7-451D-AB46-F38ACCC0A92F',1,1,'/IncomeWiterOff/Index','Income_Witer_Off',2,2 UNION
SELECT '6AD43E8F-B16A-402E-A079-28AC3DB49B3F','查询收入','Income Query',10403,'78C9F455-C2D7-451D-AB46-F38ACCC0A92F',1,1,'/IncomeQuery/Index','Income_Query',2,2 UNION
SELECT '850D1AB8-D1F4-4FD2-84E9-52FC85CF0D6A','查询与编辑收入记录','Income Query',10404,'78C9F455-C2D7-451D-AB46-F38ACCC0A92F',1,1,'/IncomeQuery/Index','Income_Query',2,2 UNION
--
SELECT '331212ED-E16C-45D7-BB33-79EF41EFF6C0','费用管理','Expenditure Management',105,NULL,1,0,NULL,NULL,1,1 UNION
SELECT '1DCB249C-8B38-4211-B30A-9632ACFF9C9B','记录费用','Expenditure Record',10501,'331212ED-E16C-45D7-BB33-79EF41EFF6C0',1,1,'/ExpenseRecord/Index','Expenditure_Record',2,2 UNION
SELECT '8231D841-B33D-4E83-A662-C4190EE41AAA','核销费用','Expenditure Witer Off',10502,'331212ED-E16C-45D7-BB33-79EF41EFF6C0',1,1,'/ExpenseWiterOff/Index','Expenditure_Witer_Off',2,2 UNION
SELECT 'EC211739-A8D6-4960-917E-6C85325B3B9A','查询费用','Expenditure Query',10503,'331212ED-E16C-45D7-BB33-79EF41EFF6C0',1,1,'/ExpenseQuery/Index','Expenditure_Query',2,2 UNION
SELECT '883E5013-D4E1-4AF7-927B-DDAD2EA4AA70','查询与编辑费用记录','Expenditure Query',10504,'331212ED-E16C-45D7-BB33-79EF41EFF6C0',1,1,'/ExpenseQuery/Index','Expenditure_Query',2,2 UNION
--
SELECT '90641EE3-0E32-4E31-853F-C66574D89578','收款管理','Receivables Management',106,NULL,1,0,NULL,NULL,1,1 UNION
SELECT '4BC7906D-2014-4673-9056-7C79A04CFC06','记录收款','Receivables Record',10601,'90641EE3-0E32-4E31-853F-C66574D89578',1,1,'/ReceivablesRecord/Index','Receivables_Record',2,2 UNION
SELECT 'CE9CBAC2-4FDB-4C5F-8B3B-3AF14F53C894','查询与归类收款','Receivables Classify',10602,'90641EE3-0E32-4E31-853F-C66574D89578',1,1,'/ReceivablesClassify/Index','Receivables_Classify',2,2 UNION
SELECT '165F77BC-6A36-4F83-9E67-97CF8E17BE24','查询收款','Receivables Query',10605,'90641EE3-0E32-4E31-853F-C66574D89578',1,1,'/ReceivablesQuery/Index','Receivables_Query',2,2 UNION
--
SELECT 'D8DE0960-081D-468D-B7D6-584E837D1B20','付款管理','Payment Management',107,NULL,1,0,NULL,NULL,1,1 UNION
SELECT 'C76316FB-1C41-490C-A158-EA7BA1D78053','记录付款','Payment Record',10701,'D8DE0960-081D-468D-B7D6-584E837D1B20',1,1,'/PaymentRecord/Index','Payment_Record',2,2 UNION
SELECT 'D9086B26-C463-47B0-8472-C4ED6A336B82','查询与归类付款','Payment Classify',10702,'D8DE0960-081D-468D-B7D6-584E837D1B20',1,1,'/PaymentClassify/Index','Payment_Classify',2,2 UNION
SELECT '9B849D53-9506-423C-9456-7B91CE6C79F6','查询付款','Payment Query',10705,'D8DE0960-081D-468D-B7D6-584E837D1B20',1,1,'/PaymentQuery/Index','Payment_Query',2,2 UNION
--
SELECT 'AD0BA10F-A124-4206-BCB6-44FB7F89E08D','固定资产管理','Fixed Assets Management',108,NULL,1,0,NULL,NULL,1,1 UNION
SELECT '7D95A133-D4CD-4633-8169-11731A6A7C57','固定资产分类','Fixed Assets Group',10801,'AD0BA10F-A124-4206-BCB6-44FB7F89E08D',1,1,'/FixedAssetsGroup/Index','Fixed_Assets_Group',2,2 UNION
SELECT 'CDC550F4-116B-4F43-A2C9-08E574A9FE8C','注册固定资产','Fixed Assets Register',10802,'AD0BA10F-A124-4206-BCB6-44FB7F89E08D',1,1,'/FixedAssetsRegister/Index','Fixed_Assets_Register',2,2 UNION
SELECT '2F702515-A015-4D0B-A82C-B33A8EC1AE94','注销固定资产','Fixed Assets Written Off',10803,'AD0BA10F-A124-4206-BCB6-44FB7F89E08D',1,1,'/FixedAssetsWrittenOff/Index','Fixed_Assets_Written_Off',2,2 UNION
SELECT 'C017AB30-56E5-4211-9B15-A6F12ABDE5F9','查询固定资产','Fixed Assets Query',10804,'AD0BA10F-A124-4206-BCB6-44FB7F89E08D',1,1,'/FixedAssetsQuery/Index','Fixed_Assets_Query',2,2 UNION
--
SELECT '42B6A334-0E43-47A5-8DF3-5EC2B4AB5DEF','报表管理','Report Management',109,NULL,1,0,NULL,NULL,1,1 UNION
SELECT '531FEBB6-7F3A-426C-9727-C9355679A858','资产负债表','Balance Sheet',10901,'42B6A334-0E43-47A5-8DF3-5EC2B4AB5DEF',1,1,'/BalanceSheet/Index','Balance_Sheet',2,2 UNION
SELECT '5FD67196-6C4B-4213-9BB6-665592D7511C','现金流量表','Cash Flow Statements',10902,'42B6A334-0E43-47A5-8DF3-5EC2B4AB5DEF',1,1,'/CashFlowStatements/Index','Cash_Flow_Statements',2,2 UNION
SELECT '2CD6E91F-EDBD-43F9-A3E8-3A480539E94B','利润表','Income Statement',10903,'42B6A334-0E43-47A5-8DF3-5EC2B4AB5DEF',1,1,'/IncomeStatement/Index','Income_Statement',2,2 UNION
SELECT '0E28CD05-3138-4327-AF78-13B0E9D6BBB5','科目汇总查询','Account Summary',10904,'42B6A334-0E43-47A5-8DF3-5EC2B4AB5DEF',1,1,'/AccountSummary/Index','Account_Summary',2,2 UNION
--
SELECT '59BE9856-1773-4D99-B0ED-F9AF5BB7E3F6','账号管理','Account Management',110,NULL,1,0,NULL,NULL,1,1 UNION
SELECT '756D2B8C-6B79-46FF-9BF9-13106C637FC5','账号管理','Account Management',11001,'59BE9856-1773-4D99-B0ED-F9AF5BB7E3F6',1,1,'/AccountManagement/Index','Account_Management',2,2 ;