USE [master]
GO
/****** Object:  Database [FMS_Develop]    Script Date: 06/02/2016 14:21:26 ******/
CREATE DATABASE [FMS_Develop] ON  PRIMARY 
( NAME = N'FMS_Develop', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\FMS_Develop.mdf' , SIZE = 141312KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'FMS_Develop_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\FMS_Develop_log.ldf' , SIZE = 164672KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [FMS_Develop] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [FMS_Develop].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [FMS_Develop] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [FMS_Develop] SET ANSI_NULLS OFF
GO
ALTER DATABASE [FMS_Develop] SET ANSI_PADDING OFF
GO
ALTER DATABASE [FMS_Develop] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [FMS_Develop] SET ARITHABORT OFF
GO
ALTER DATABASE [FMS_Develop] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [FMS_Develop] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [FMS_Develop] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [FMS_Develop] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [FMS_Develop] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [FMS_Develop] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [FMS_Develop] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [FMS_Develop] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [FMS_Develop] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [FMS_Develop] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [FMS_Develop] SET  DISABLE_BROKER
GO
ALTER DATABASE [FMS_Develop] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [FMS_Develop] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [FMS_Develop] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [FMS_Develop] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [FMS_Develop] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [FMS_Develop] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [FMS_Develop] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [FMS_Develop] SET  READ_WRITE
GO
ALTER DATABASE [FMS_Develop] SET RECOVERY FULL
GO
ALTER DATABASE [FMS_Develop] SET  MULTI_USER
GO
ALTER DATABASE [FMS_Develop] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [FMS_Develop] SET DB_CHAINING OFF
GO
EXEC sys.sp_db_vardecimal_storage_format N'FMS_Develop', N'ON'
GO
USE [FMS_Develop]
GO
/****** Object:  UserDefinedFunction [dbo].[FUN_SPLIT]    Script Date: 06/02/2016 14:21:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[FUN_SPLIT](@Long_str Nvarchar(4000),@split_str Nvarchar(100))    
RETURNS  @tmp TABLE(        
    ID          inT     IDENTITY PRIMARY KEY,      
    short_str   Nvarchar(4000)    
)    
AS   
BEGIN   
    DECLARE @long_str_Tmp varchar(8000),@short_str varchar(8000),@split_str_length int   
    SET @split_str_length = LEN(@split_str)    
    IF CHARINDEX(@split_str,@Long_str)=1 
         SET @long_str_Tmp=SUBSTRING(@Long_str,@split_str_length+1,LEN(@Long_str)-@split_str_length)
    ELSE
         SET @long_str_Tmp=@Long_str
    IF CHARINDEX(REVERSE(@split_str),REVERSE(@long_str_Tmp))>1    
        SET @long_str_Tmp=@long_str_Tmp+@split_str    
    ELSE   
        SET @long_str_Tmp=@long_str_Tmp    
    IF CHARINDEX(@split_str,@long_str_Tmp)=0
        Insert INTO @tmp select @long_str_Tmp 
    ELSE
        BEGIN
            WHILE CHARINDEX(@split_str,@long_str_Tmp)>0    
                BEGIN   
                    SET @short_str=SUBSTRING(@long_str_Tmp,1,CHARINDEX(@split_str,@long_str_Tmp)-1)    
                    DECLARE @long_str_Tmp_LEN INT,@split_str_Position_END int   
                    SET @long_str_Tmp_LEN = LEN(@long_str_Tmp)    
                    SET @split_str_Position_END = LEN(@short_str)+@split_str_length    
                    SET @long_str_Tmp=REVERSE(SUBSTRING(REVERSE(@long_str_Tmp),1,@long_str_Tmp_LEN-@split_str_Position_END))
                    IF @short_str<>'' Insert INTO @tmp select @short_str    
                END           
        END
    RETURN     
END
GO
/****** Object:  UserDefinedFunction [dbo].[FUN_Depreciation_Years]    Script Date: 06/02/2016 14:21:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	固定资产年数和折旧
-- =============================================
CREATE FUNCTION [dbo].[FUN_Depreciation_Years]
(
	@AssetsCost DECIMAL(18,4),
	@SalvageValue DECIMAL(18,4),
	@Life INT,
	@RegisterDate DATETIME,
	@Summary DATETIME
)
RETURNS DECIMAL(18,4)
AS
BEGIN
	-- @Result当前资产值
	DECLARE @Result decimal(18,4);
	SET @Result = @AssetsCost;
	--年数总和
	DECLARE @YEARS INT;
	SET @YEARS = @Life*(@Life+1)/2;
	--仍可使用年
	DECLARE @USEYEAR INT;
	SET @USEYEAR = @LIFE;
	--月折旧额
	DECLARE @MonthDepreciationValue DECIMAL(18,4)
	
	WHILE ((@LIFE- @USEYEAR) < datediff(year ,@RegisterDate,@Summary) AND @USEYEAR >= 0)
		BEGIN
			SET @Result = @Result- (@AssetsCost - @SalvageValue)*@USEYEAR/@YEARS;
			SET @USEYEAR = @USEYEAR - 1;
		END
	
	SET @MonthDepreciationValue = (@AssetsCost - @SalvageValue)*(@USEYEAR/@YEARS)/12;
	SET @Result = @Result- datediff(month ,DATEADD(YEAR,(@LIFE- @USEYEAR),@RegisterDate),@Summary)*@MonthDepreciationValue;
	
	-- Return the result of the function
	RETURN @Result;
END
GO
/****** Object:  UserDefinedFunction [dbo].[FUN_Depreciation_Overage]    Script Date: 06/02/2016 14:21:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	固定资产余额法折旧
-- =============================================
CREATE FUNCTION [dbo].[FUN_Depreciation_Overage]
(
	@AssetsCost DECIMAL(18,4),
	@SalvageValue DECIMAL(18,4),
	@Life INT,
	@RegisterDate DATETIME,
	@Summary DATETIME
)
RETURNS DECIMAL(18,4)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result decimal(18,4);
	SET @Result = 0;
	--仍可使用年
	DECLARE @USEYEAR INT;
	SET @USEYEAR = @LIFE;
	--累计折旧
	DECLARE @SumDepreciation DECIMAL(18,4);
	SET @SumDepreciation = 0;
	--最后两年折旧额
	DECLARE @Base DECIMAL(18,4);
	SET @Base = 0;
	--相差年数
	DECLARE @DiffDate INT;
	IF datediff(year,@RegisterDate,@Summary)> @LIFE 
		SET @DiffDate = @LIFE;
	ELSE
		SET @DiffDate = datediff(year,@RegisterDate,@Summary);
	--折旧过程
	WHILE(@DiffDate > 0 AND @USEYEAR >= 0 AND @USEYEAR +@DiffDate > @LIFE)
	BEGIN
		IF(@USEYEAR > 2)
		BEGIN
			SET @SumDepreciation = @SumDepreciation+(@AssetsCost - @SumDepreciation) * 2 / @Life;			
		END
		ELSE
		BEGIN
			IF (@Base = 0)
			BEGIN		
				SET @Base = (@Result- @SalvageValue)/2
			END
			SET @SumDepreciation = @SumDepreciation + @Base;
		END
		SET @USEYEAR = @USEYEAR - 1;
	END
	
	--当年月折旧
	
	IF(@USEYEAR > 2)
	BEGIN
		SET @SumDepreciation = @SumDepreciation+(@AssetsCost - @SumDepreciation) * 2 / @Life/12*datediff(month ,DATEADD(YEAR,(@LIFE- @USEYEAR),@RegisterDate),@Summary);			
	END
	ELSE
	BEGIN
		IF (@Base = 0)
		BEGIN		
			SET @Base = (@Result- @SalvageValue)/2/12*datediff(month ,DATEADD(YEAR,(@LIFE- @USEYEAR),@RegisterDate),@Summary);
		END
		SET @SumDepreciation = @SumDepreciation + @Base;
	END
	-- Return the result of the function
	SET @Result = @AssetsCost - @SumDepreciation;
	RETURN @Result;
END
GO
/****** Object:  UserDefinedFunction [dbo].[FUN_Depreciation_Line]    Script Date: 06/02/2016 14:21:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	固定资产线性折旧
-- =============================================
CREATE FUNCTION [dbo].[FUN_Depreciation_Line] 
(
	@AssetsCost DECIMAL(18,4),
	@SalvageValue DECIMAL(18,4),
	@Life INT,
	@RegisterDate DATETIME,
	@Summary DATETIME
)
RETURNS DECIMAL(18,4)
AS
BEGIN
	-- @Result当前资产值
	DECLARE @Result decimal(18,4);
	SET @Result = 0;
	--@YearlyDepreciation年折旧额
	DECLARE @YearlyDepreciation DECIMAL(18,4);
	SET @YearlyDepreciation =(@AssetsCost - @SalvageValue)/@Life;
	IF	(datediff(year ,@RegisterDate,@Summary) >= @LIFE)
		SET @Result = @SalvageValue;
	ELSE
		SET @Result = @AssetsCost - datediff(month ,@RegisterDate,@Summary)*(@YearlyDepreciation/12);
		
	-- Return the result of the function
	RETURN @Result;

END
GO
/****** Object:  Table [dbo].[R_CompanyCurrceny]    Script Date: 06/02/2016 14:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[R_CompanyCurrceny](
	[R_GUID] [nvarchar](40) NOT NULL,
	[C_GUID] [nvarchar](40) NULL,
	[Code] [nvarchar](3) NULL,
 CONSTRAINT [PK_R_CpmpanyCurrceny] PRIMARY KEY CLUSTERED 
(
	[R_GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[R_CompanyAccount]    Script Date: 06/02/2016 14:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[R_CompanyAccount](
	[R_GUID] [nvarchar](40) NOT NULL,
	[LA_GUID] [nvarchar](40) NULL,
	[C_GUID] [nvarchar](40) NULL,
 CONSTRAINT [PK_R_CompanyAccount] PRIMARY KEY CLUSTERED 
(
	[R_GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_WageCost]    Script Date: 06/02/2016 14:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_WageCost](
	[W_GUID] [nvarchar](40) NOT NULL,
	[C_GUID] [nvarchar](40) NULL,
	[Date] [datetime] NULL,
	[Employee] [nvarchar](40) NULL,
	[Cash] [decimal](18, 2) NULL,
	[PersonalTaxes] [decimal](18, 2) NULL,
	[SocialSecurity] [decimal](18, 2) NULL,
	[Total] [decimal](18, 2) NULL,
 CONSTRAINT [PK_T_WageCost] PRIMARY KEY CLUSTERED 
(
	[W_GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_User]    Script Date: 06/02/2016 14:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_User](
	[U_GUID] [nvarchar](40) NOT NULL,
	[C_GUID] [nvarchar](40) NULL,
	[UserName] [nvarchar](40) NULL,
	[Password] [nvarchar](40) NULL,
	[LoginName] [nvarchar](40) NULL,
	[State] [int] NULL,
	[EnterC_GUID] [nvarchar](40) NULL,
 CONSTRAINT [PK_T_User] PRIMARY KEY CLUSTERED 
(
	[U_GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_Tax]    Script Date: 06/02/2016 14:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_Tax](
	[T_GUID] [nvarchar](40) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Rate] [decimal](10, 4) NULL,
	[Type] [nvarchar](40) NULL,
	[C_GUID] [nvarchar](40) NULL,
 CONSTRAINT [PK_T_Tax] PRIMARY KEY CLUSTERED 
(
	[T_GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_ReportDetails]    Script Date: 06/02/2016 14:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_ReportDetails](
	[GUID] [nvarchar](40) NOT NULL,
	[RGUID] [nvarchar](40) NOT NULL,
	[Rep_GUID] [nvarchar](40) NOT NULL,
	[Code] [nvarchar](20) NULL,
	[Name] [nvarchar](50) NULL,
	[BeginningValue] [decimal](18, 2) NULL,
	[EndingValue] [decimal](18, 2) NULL,
	[AccGrp] [nvarchar](40) NULL,
 CONSTRAINT [PK_T_REPORTDETAILS] PRIMARY KEY NONCLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_Report]    Script Date: 06/02/2016 14:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_Report](
	[Rep_GUID] [nvarchar](40) NOT NULL,
	[RepNo] [nvarchar](40) NULL,
	[Type] [nvarchar](20) NULL,
	[Year] [int] NULL,
	[Month] [int] NULL,
	[C_GUID] [nvarchar](40) NULL,
 CONSTRAINT [PK_T_REPORT] PRIMARY KEY NONCLUSTERED 
(
	[Rep_GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_RecPayRecord]    Script Date: 06/02/2016 14:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_RecPayRecord](
	[RP_GUID] [nvarchar](40) NOT NULL,
	[RP_Flag] [nvarchar](1) NULL,
	[C_GUID] [nvarchar](40) NULL,
	[RPable] [nvarchar](40) NULL,
	[InvType] [nvarchar](20) NULL,
	[InvNo] [nvarchar](20) NULL,
	[RPer] [nvarchar](40) NULL,
	[DebitLedgerAccount] [nvarchar](40) NULL,
	[DebitDetailsAccount] [nvarchar](40) NULL,
	[CreditLedgerAccount] [nvarchar](40) NULL,
	[CreditDetailsAccount] [nvarchar](40) NULL,
	[SumAmount] [decimal](18, 2) NULL,
	[Date] [date] NULL,
	[Remark] [nvarchar](200) NULL,
	[Creator] [nvarchar](40) NULL,
	[CreateDate] [datetime] NULL,
	[Currency] [nvarchar](5) NULL,
	[CFItemGuid] [nvarchar](40) NULL,
	[CFPItemGuid] [nvarchar](40) NULL,
	[B_GUID] [nvarchar](40) NULL,
	[BA_GUID] [nvarchar](40) NULL,
	[InvTypeDts] [nvarchar](50) NULL,
	[IE_GUID] [nvarchar](40) NULL,
 CONSTRAINT [PK_T_ReceivablesRecord] PRIMARY KEY CLUSTERED 
(
	[RP_GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_RecPayHistoryRecord]    Script Date: 06/02/2016 14:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_RecPayHistoryRecord](
	[RP_GUID] [nvarchar](40) NOT NULL,
	[RP_Flag] [nvarchar](1) NULL,
	[C_GUID] [nvarchar](40) NULL,
	[InvType] [nvarchar](20) NULL,
	[InvNo] [nvarchar](20) NULL,
	[R_Per] [nvarchar](40) NULL,
	[DebitLedgerAccount] [nvarchar](40) NULL,
	[DebitDetailsAccount] [nvarchar](40) NULL,
	[CreditLedgerAccount] [nvarchar](40) NULL,
	[CreditDetailsAccount] [nvarchar](40) NULL,
	[Amount] [decimal](18, 2) NULL,
	[Date] [date] NULL,
	[Remark] [nvarchar](200) NULL,
	[Creator] [nvarchar](40) NULL,
	[CreateDate] [datetime] NULL,
	[Currency] [nvarchar](5) NULL,
	[CFItem] [nvarchar](100) NULL,
	[CFPItem] [nvarchar](100) NULL,
	[Bank] [nvarchar](100) NULL,
	[BankAccount] [nvarchar](100) NULL,
 CONSTRAINT [PK_T_RecPayHistoryRecord] PRIMARY KEY CLUSTERED 
(
	[RP_GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_Receivables]    Script Date: 06/02/2016 14:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_Receivables](
	[R_GUID] [nvarchar](40) NOT NULL,
	[C_GUID] [nvarchar](40) NULL,
	[Payer] [nvarchar](40) NULL,
	[Date] [date] NULL,
	[InvType] [nvarchar](20) NULL,
	[InvNo] [nvarchar](20) NULL,
	[B_GUID] [nvarchar](40) NULL,
	[BA_GUID] [nvarchar](40) NULL,
	[Money] [decimal](18, 2) NULL,
	[Currency] [nvarchar](5) NULL,
 CONSTRAINT [PK_T_Receivables] PRIMARY KEY CLUSTERED 
(
	[R_GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_RateHistory]    Script Date: 06/02/2016 14:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_RateHistory](
	[GUID] [nvarchar](40) NOT NULL,
	[Date] [datetime] NULL,
	[FAmount] [decimal](18, 4) NULL,
	[FCurrency] [nvarchar](50) NULL,
	[TAmount] [decimal](18, 4) NULL,
	[TCurrency] [nvarchar](50) NULL,
	[C_GUID] [nvarchar](50) NULL,
 CONSTRAINT [PK_T_RateHistory] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_Payable]    Script Date: 06/02/2016 14:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_Payable](
	[R_GUID] [nvarchar](40) NOT NULL,
	[C_GUID] [nvarchar](40) NULL,
	[Date] [date] NULL,
	[InvType] [nvarchar](20) NULL,
	[InvNo] [nvarchar](20) NULL,
	[Payee] [nvarchar](40) NULL,
	[B_GUID] [nvarchar](40) NULL,
	[BA_GUID] [nvarchar](40) NULL,
	[Money] [decimal](18, 2) NULL,
	[Currency] [nvarchar](5) NULL,
	[AffirmDate] [date] NULL,
 CONSTRAINT [PK_T_Payable] PRIMARY KEY CLUSTERED 
(
	[R_GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_ModuleList]    Script Date: 06/02/2016 14:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
	[Block] [int] NULL,
 CONSTRAINT [PK_T_ModuleList] PRIMARY KEY CLUSTERED 
(
	[Guid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_InvType]    Script Date: 06/02/2016 14:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_InvType](
	[Key] [nvarchar](40) NOT NULL,
	[Name] [nvarchar](40) NULL,
 CONSTRAINT [PK_T_InvType] PRIMARY KEY CLUSTERED 
(
	[Key] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_IEWriteOff]    Script Date: 06/02/2016 14:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_IEWriteOff](
	[R_GUID] [nvarchar](40) NOT NULL,
	[C_GUID] [nvarchar](40) NOT NULL,
	[IE_Flag] [nvarchar](1) NULL,
	[DebitLedgerAccount] [varchar](100) NOT NULL,
	[DebitDetailsAccount] [varchar](100) NULL,
	[CreditLedgerAccount] [varchar](100) NOT NULL,
	[CreditDetailsAccount] [varchar](100) NULL,
	[Amount] [decimal](18, 4) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Remark] [varchar](200) NULL,
	[Creator] [varchar](40) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[Currency] [nvarchar](5) NULL,
 CONSTRAINT [PK_T_IEWRITEOFF] PRIMARY KEY NONCLUSTERED 
(
	[R_GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[T_IERecord]    Script Date: 06/02/2016 14:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_IERecord](
	[IE_GUID] [nvarchar](40) NOT NULL,
	[IE_Flag] [nvarchar](1) NULL,
	[InvType] [nvarchar](40) NULL,
	[InvNo] [nvarchar](20) NULL,
	[RPer] [nvarchar](40) NULL,
	[Creator] [nvarchar](40) NULL,
	[CreateDate] [datetime] NULL,
	[C_GUID] [nvarchar](40) NULL,
	[AffirmDate] [datetime] NULL,
	[Date] [datetime] NULL,
	[Amount] [decimal](18, 4) NULL,
	[TaxationAmount] [decimal](18, 4) NULL,
	[TaxationType] [nvarchar](40) NULL,
	[SumAmount] [decimal](18, 4) NULL,
	[Remark] [nvarchar](200) NULL,
	[Currency] [nvarchar](20) NULL,
	[B_GUID] [nvarchar](40) NULL,
	[BA_GUID] [nvarchar](40) NULL,
	[IEGroup] [nvarchar](40) NULL,
	[IEDescription] [nvarchar](500) NULL,
	[RP_GUID] [nvarchar](40) NULL,
	[State] [nvarchar](50) NULL,
	[Profit_GUID] [nvarchar](40) NULL,
 CONSTRAINT [PK_T_INCOMERECORD] PRIMARY KEY NONCLUSTERED 
(
	[IE_GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_IEHistoryRecord]    Script Date: 06/02/2016 14:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_IEHistoryRecord](
	[IE_GUID] [nvarchar](40) NOT NULL,
	[IE_Flag] [nvarchar](1) NULL,
	[InvType] [nvarchar](20) NULL,
	[InvNo] [nvarchar](20) NULL,
	[RPer] [nvarchar](40) NULL,
	[Amount] [decimal](18, 4) NULL,
	[Tax] [decimal](18, 4) NULL,
	[Date] [date] NULL,
	[Creator] [nvarchar](40) NULL,
	[CreateDate] [datetime] NULL,
	[C_GUID] [nvarchar](40) NULL,
	[AffirmDate] [datetime] NULL,
	[SumAmount] [decimal](18, 4) NULL,
	[Currency] [nvarchar](20) NULL,
	[Remark] [nvarchar](200) NULL,
 CONSTRAINT [PK_T_IEHistoryRecord] PRIMARY KEY CLUSTERED 
(
	[IE_GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_IEHistoryDetails]    Script Date: 06/02/2016 14:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_IEHistoryDetails](
	[R_GUID] [nvarchar](40) NOT NULL,
	[IE_GUID] [nvarchar](40) NOT NULL,
	[DebitLedgerAccount] [nvarchar](40) NULL,
	[DebitDetailsAccount] [nvarchar](40) NULL,
	[CreditLedgerAccount] [nvarchar](40) NULL,
	[CreditDetailsAccount] [nvarchar](40) NULL,
	[Money] [decimal](18, 2) NULL,
	[C_GUID] [nvarchar](40) NULL,
	[Currency] [nvarchar](5) NULL,
 CONSTRAINT [PK_T_IEHistoryDetails] PRIMARY KEY CLUSTERED 
(
	[R_GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_IEDetails]    Script Date: 06/02/2016 14:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_IEDetails](
	[R_GUID] [nvarchar](40) NOT NULL,
	[IE_GUID] [nvarchar](40) NOT NULL,
	[DebitLedgerAccount] [nvarchar](40) NULL,
	[DebitDetailsAccount] [nvarchar](40) NULL,
	[CreditLedgerAccount] [nvarchar](40) NULL,
	[CreditDetailsAccount] [nvarchar](40) NULL,
	[Money] [decimal](18, 2) NULL,
	[C_GUID] [nvarchar](40) NULL,
	[Remark] [nvarchar](200) NULL,
	[Currency] [nvarchar](5) NULL,
 CONSTRAINT [PK_T_IncomeDetails] PRIMARY KEY CLUSTERED 
(
	[R_GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_GeneralLedgerAccount]    Script Date: 06/02/2016 14:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_GeneralLedgerAccount](
	[LA_GUID] [nvarchar](40) NOT NULL,
	[AccGroup] [int] NULL,
	[AccCode] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_T_GeneralLedgerAccount_1] PRIMARY KEY CLUSTERED 
(
	[LA_GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_ExpenseType]    Script Date: 06/02/2016 14:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_ExpenseType](
	[ET_GUID] [nvarchar](40) NOT NULL,
	[C_GUID] [nvarchar](40) NULL,
	[ExpenseType] [nvarchar](100) NULL,
	[ExpenseFlag] [nvarchar](1) NULL,
	[SaleFlag] [nvarchar](1) NULL,
	[ManageFlag] [nvarchar](1) NULL,
	[FinanceFlag] [nvarchar](1) NULL,
	[SalaryFlag] [nvarchar](1) NULL,
	[TaxFlag] [nvarchar](1) NULL,
 CONSTRAINT [PK_T_ExpenseType] PRIMARY KEY CLUSTERED 
(
	[ET_GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_DetailedCategories]    Script Date: 06/02/2016 14:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_DetailedCategories](
	[GUID] [nvarchar](40) NOT NULL,
	[Name] [nvarchar](40) NULL,
	[State] [nvarchar](40) NULL,
 CONSTRAINT [PK_T_DetailedCategories] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_DetailedAccount]    Script Date: 06/02/2016 14:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_DetailedAccount](
	[DA_GUID] [nvarchar](40) NOT NULL,
	[AccCode] [int] NULL,
	[Name] [nvarchar](100) NULL,
	[ParentAccGuid] [nvarchar](40) NULL,
	[C_GUID] [nvarchar](40) NULL,
	[D_GUID] [nvarchar](40) NULL,
 CONSTRAINT [PK_T_DETAILEDACCOUNT] PRIMARY KEY NONCLUSTERED 
(
	[DA_GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_DeclareCustomer]    Script Date: 06/02/2016 14:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_DeclareCustomer](
	[GUID] [nvarchar](40) NOT NULL,
	[InvType] [nvarchar](40) NULL,
	[RPer] [nvarchar](40) NULL,
	[Amount] [decimal](18, 4) NULL,
	[Currency] [nvarchar](20) NULL,
	[State] [nvarchar](20) NULL,
	[Remark] [nvarchar](200) NULL,
	[Date] [datetime] NULL,
	[C_GUID] [nvarchar](40) NULL,
 CONSTRAINT [PK_T_DeclareCustomer] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_DeclareCostSpending]    Script Date: 06/02/2016 14:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_DeclareCostSpending](
	[GUID] [nvarchar](40) NOT NULL,
	[InvType] [nvarchar](40) NULL,
	[RPer] [nvarchar](40) NULL,
	[Amount] [decimal](18, 4) NULL,
	[Currency] [nvarchar](20) NULL,
	[State] [nvarchar](20) NULL,
	[Remark] [nvarchar](200) NULL,
	[Date] [datetime] NULL,
	[C_GUID] [nvarchar](40) NULL,
 CONSTRAINT [PK_T_DeclareCostSpending] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_CurrencyCode]    Script Date: 06/02/2016 14:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_CurrencyCode](
	[Code] [nvarchar](3) NOT NULL,
	[IsCommon] [bit] NULL,
 CONSTRAINT [PK_T_CurrencyCode] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_CompanySetting]    Script Date: 06/02/2016 14:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_CompanySetting](
	[R_GUID] [nvarchar](40) NOT NULL,
	[C_GUID] [nvarchar](40) NULL,
	[StandardCoin] [nvarchar](3) NULL,
	[ReportPeriod] [nvarchar](10) NULL,
	[ReportStartDate] [datetime] NULL,
	[AuditDate] [datetime] NULL,
 CONSTRAINT [PK_T_CompanySetting] PRIMARY KEY CLUSTERED 
(
	[R_GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_Company]    Script Date: 06/02/2016 14:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_Company](
	[C_GUID] [nvarchar](40) NOT NULL,
	[MasterCompanyGuid] [nvarchar](40) NULL,
	[Name] [nvarchar](100) NULL,
	[Address] [nvarchar](50) NULL,
	[Contacter] [nvarchar](50) NULL,
	[ContactWay] [nvarchar](50) NULL,
	[Type] [nvarchar](50) NULL,
	[AuditDate] [nvarchar](50) NULL,
	[ChineseFullName] [nvarchar](50) NULL,
	[EnglishFullName] [nvarchar](50) NULL,
	[Website] [nvarchar](100) NULL,
	[OrganizationCode] [nvarchar](40) NULL,
	[IndustryInvolved] [nvarchar](40) NULL,
	[RegisteredAddress] [nvarchar](40) NULL,
	[Remark] [nvarchar](40) NULL,
	[LOGO] [nvarchar](400) NULL,
	[BusinessLicense] [nvarchar](400) NULL,
 CONSTRAINT [PK_T_Company] PRIMARY KEY CLUSTERED 
(
	[C_GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_CashFlowItem]    Script Date: 06/02/2016 14:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_CashFlowItem](
	[R_GUID] [nvarchar](40) NOT NULL,
	[No] [int] NULL,
	[Name] [nvarchar](100) NULL,
	[PID] [nvarchar](40) NULL,
	[RP_Flag] [nvarchar](1) NULL,
 CONSTRAINT [PK_CASHFLOWITEM] PRIMARY KEY NONCLUSTERED 
(
	[R_GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_BusinessPartner]    Script Date: 06/02/2016 14:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_BusinessPartner](
	[BP_GUID] [nvarchar](40) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[IsSupplier] [bit] NULL,
	[IsCustomer] [bit] NULL,
	[IsPartner] [bit] NULL,
	[C_GUID] [nvarchar](40) NULL,
	[ChineseFullName] [nvarchar](50) NULL,
	[EnglishFullName] [nvarchar](50) NULL,
	[Website] [nvarchar](100) NULL,
	[OrganizationCode] [nvarchar](40) NULL,
	[IndustryInvolved] [nvarchar](40) NULL,
	[RegisteredAddress] [nvarchar](40) NULL,
	[Remark] [nvarchar](100) NULL,
 CONSTRAINT [PK_T_BUSINESSPARTNER] PRIMARY KEY NONCLUSTERED 
(
	[BP_GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_BusinessCost]    Script Date: 06/02/2016 14:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_BusinessCost](
	[BC_GUID] [nvarchar](40) NOT NULL,
	[BC_Flag] [nvarchar](1) NULL,
	[InvType] [nvarchar](40) NULL,
	[InvNo] [nvarchar](20) NULL,
	[RPer] [nvarchar](40) NULL,
	[Creator] [nvarchar](40) NULL,
	[CreateDate] [datetime] NULL,
	[C_GUID] [nvarchar](40) NULL,
	[AffirmDate] [datetime] NULL,
	[Date] [datetime] NULL,
	[Amount] [decimal](18, 4) NULL,
	[TaxationAmount] [decimal](18, 4) NULL,
	[TaxationType] [nvarchar](40) NULL,
	[SumAmount] [decimal](18, 4) NULL,
	[Remark] [nvarchar](200) NULL,
	[Currency] [nvarchar](20) NULL,
	[B_GUID] [nvarchar](40) NULL,
	[BA_GUID] [nvarchar](40) NULL,
 CONSTRAINT [PK_T_BusinessCost] PRIMARY KEY CLUSTERED 
(
	[BC_GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_BeginningBalance]    Script Date: 06/02/2016 14:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_BeginningBalance](
	[R_GUID] [nvarchar](40) NOT NULL,
	[C_GUID] [nvarchar](40) NULL,
	[Acc_GUID] [nvarchar](40) NULL,
	[Money] [decimal](18, 2) NULL,
 CONSTRAINT [PK_T_BeginningBalance] PRIMARY KEY CLUSTERED 
(
	[R_GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_BankAccount]    Script Date: 06/02/2016 14:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_BankAccount](
	[BA_GUID] [nvarchar](40) NOT NULL,
	[B_GUID] [nvarchar](40) NULL,
	[Account] [nvarchar](100) NULL,
	[C_GUID] [nvarchar](40) NULL,
	[AccountName] [nvarchar](40) NULL,
	[AccountCurrency] [nvarchar](40) NULL,
	[AccountAbbreviation] [nvarchar](40) NULL,
	[AccountType] [nvarchar](40) NULL,
	[BankAddress] [nvarchar](100) NULL,
	[SwiftCode] [nvarchar](40) NULL,
	[Amount] [decimal](18, 4) NULL,
 CONSTRAINT [PK_T_BankAccount] PRIMARY KEY CLUSTERED 
(
	[BA_GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'账户余额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_BankAccount', @level2type=N'COLUMN',@level2name=N'Amount'
GO
/****** Object:  Table [dbo].[T_Bank]    Script Date: 06/02/2016 14:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_Bank](
	[B_GUID] [nvarchar](40) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[C_GUID] [nvarchar](40) NULL,
 CONSTRAINT [PK_T_Bank] PRIMARY KEY CLUSTERED 
(
	[B_GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_Balance]    Script Date: 06/02/2016 14:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_Balance](
	[Inital_GUID] [nvarchar](40) NOT NULL,
	[C_GUID] [nvarchar](40) NULL,
	[MonetaryFunds] [decimal](18, 4) NULL,
	[ShorttermInvestments] [decimal](18, 4) NULL,
	[NotesReceivable] [decimal](18, 4) NULL,
	[SubsidiesReceivable] [decimal](18, 4) NULL,
	[DividendReceivable] [decimal](18, 4) NULL,
	[Inventories] [decimal](18, 4) NULL,
	[LongtermDebtOneYear] [decimal](18, 4) NULL,
	[OtherCcurrentAssets] [decimal](18, 4) NULL,
	[LongtermInvestments] [decimal](18, 4) NULL,
	[FixedAssetsNBV] [decimal](18, 4) NULL,
	[ConstructionInProgress] [decimal](18, 4) NULL,
	[IntangibleAssets] [decimal](18, 4) NULL,
	[Deferred] [decimal](18, 4) NULL,
	[ShorttermLoans] [decimal](18, 4) NULL,
	[NotesPayable] [decimal](18, 4) NULL,
	[AccountsPayable] [decimal](18, 4) NULL,
	[AdvancesFromCustomers] [decimal](18, 4) NULL,
	[AccruedPayroll] [decimal](18, 4) NULL,
	[TaxesPayable] [decimal](18, 4) NULL,
	[LongtermLiabiltiesDueWithinaYear] [decimal](18, 4) NULL,
	[OtherCurrentLiabilities] [decimal](18, 4) NULL,
	[LongtermBorrowings] [decimal](18, 4) NULL,
	[LongtermPayables] [decimal](18, 4) NULL,
	[OtherLongtermLliabilities] [decimal](18, 4) NULL,
	[Currency] [nvarchar](40) NULL,
	[Flag] [nvarchar](40) NULL,
	[Date] [datetime] NULL,
	[BankAccount1] [nvarchar](40) NULL,
	[BankAccount1Money] [decimal](18, 4) NULL,
	[BankAccount2] [nvarchar](40) NULL,
	[BankAccount2Money] [decimal](18, 4) NULL,
 CONSTRAINT [PK_T_Balance] PRIMARY KEY CLUSTERED 
(
	[Inital_GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备用现金余额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Balance', @level2type=N'COLUMN',@level2name=N'MonetaryFunds'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'短期投资余额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Balance', @level2type=N'COLUMN',@level2name=N'ShorttermInvestments'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应收票据余额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Balance', @level2type=N'COLUMN',@level2name=N'NotesReceivable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应收补贴余额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Balance', @level2type=N'COLUMN',@level2name=N'SubsidiesReceivable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应收股利余额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Balance', @level2type=N'COLUMN',@level2name=N'DividendReceivable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'存货余额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Balance', @level2type=N'COLUMN',@level2name=N'Inventories'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'一年内到期的长期债券投资余额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Balance', @level2type=N'COLUMN',@level2name=N'LongtermDebtOneYear'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'其他流动资产余额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Balance', @level2type=N'COLUMN',@level2name=N'OtherCcurrentAssets'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'长期投资余额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Balance', @level2type=N'COLUMN',@level2name=N'LongtermInvestments'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'固定资产余额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Balance', @level2type=N'COLUMN',@level2name=N'FixedAssetsNBV'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'在建工程余额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Balance', @level2type=N'COLUMN',@level2name=N'ConstructionInProgress'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'无形资产余额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Balance', @level2type=N'COLUMN',@level2name=N'IntangibleAssets'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'延递资产余额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Balance', @level2type=N'COLUMN',@level2name=N'Deferred'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'短期借款余额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Balance', @level2type=N'COLUMN',@level2name=N'ShorttermLoans'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应付票据余额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Balance', @level2type=N'COLUMN',@level2name=N'NotesPayable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应付账款余额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Balance', @level2type=N'COLUMN',@level2name=N'AccountsPayable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'预收账款余额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Balance', @level2type=N'COLUMN',@level2name=N'AdvancesFromCustomers'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应付工资余额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Balance', @level2type=N'COLUMN',@level2name=N'AccruedPayroll'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应交税金' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Balance', @level2type=N'COLUMN',@level2name=N'TaxesPayable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'一年内到期的长期负债余额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Balance', @level2type=N'COLUMN',@level2name=N'LongtermLiabiltiesDueWithinaYear'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'其他流动负债余额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Balance', @level2type=N'COLUMN',@level2name=N'OtherCurrentLiabilities'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'长期借款余额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Balance', @level2type=N'COLUMN',@level2name=N'LongtermBorrowings'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'长期应付款余额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Balance', @level2type=N'COLUMN',@level2name=N'LongtermPayables'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'其他长期负债余额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Balance', @level2type=N'COLUMN',@level2name=N'OtherLongtermLliabilities'
GO
/****** Object:  Table [dbo].[T_Attachment]    Script Date: 06/02/2016 14:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_Attachment](
	[A_GUID] [nvarchar](40) NULL,
	[FileName] [nvarchar](200) NULL,
	[FileType] [nvarchar](500) NULL,
	[FlieData] [varbinary](max) NULL,
	[FR_GUID] [nvarchar](40) NULL,
	[FileRemark] [nvarchar](500) NULL,
	[Number] [nvarchar](100) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[T_AssetsGroup]    Script Date: 06/02/2016 14:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_AssetsGroup](
	[AG_GUID] [nvarchar](40) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[DepreciationMethod] [int] NULL,
	[Life] [int] NULL,
	[SalvageRate] [decimal](5, 2) NULL,
	[C_GUID] [nvarchar](40) NULL,
 CONSTRAINT [PK_T_ASSETSGROUP] PRIMARY KEY NONCLUSTERED 
(
	[AG_GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_Assets]    Script Date: 06/02/2016 14:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_Assets](
	[A_GUID] [nvarchar](40) NOT NULL,
	[No] [nvarchar](100) NULL,
	[Name] [nvarchar](100) NULL,
	[RegisterDate] [date] NULL,
	[PurchaseDate] [date] NULL,
	[ScrapType] [nvarchar](10) NULL,
	[ScrapDate] [date] NULL,
	[AG_GUID] [nvarchar](40) NULL,
	[AssetsCost] [decimal](18, 4) NULL,
	[Creator] [nvarchar](40) NULL,
	[C_GUID] [nvarchar](40) NULL,
 CONSTRAINT [PK_T_ASSETS] PRIMARY KEY NONCLUSTERED 
(
	[A_GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_AssetCostType]    Script Date: 06/02/2016 14:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_AssetCostType](
	[code] [varchar](2) NOT NULL,
	[name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_AssetCostType] PRIMARY KEY CLUSTERED 
(
	[code] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[T_AIDRecord]    Script Date: 06/02/2016 14:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_AIDRecord](
	[GUID] [nvarchar](40) NOT NULL,
	[C_GUID] [nvarchar](40) NULL,
	[AID_Flag] [nvarchar](1) NULL,
	[Date] [datetime] NULL,
	[Amount] [decimal](18, 4) NULL,
	[Currency] [nvarchar](40) NULL,
	[RPer] [nvarchar](40) NULL,
	[InvType] [nvarchar](40) NULL,
	[Description] [nvarchar](40) NULL,
	[DepreciationPeriod] [int] NULL,
	[SurplusValue] [decimal](18, 4) NULL,
	[State] [nvarchar](40) NULL,
	[Remark] [nvarchar](40) NULL,
	[CostType] [varchar](1) NULL,
 CONSTRAINT [PK_T_AIDRecord] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'成本费用类别(B=营业成本,S=销售费用,M=管理费用)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_AIDRecord', @level2type=N'COLUMN',@level2name=N'CostType'
GO
/****** Object:  StoredProcedure [dbo].[SP_VaildBeginningBalance]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	验证期初数
-- =============================================
CREATE PROCEDURE [dbo].[SP_VaildBeginningBalance]
	@C_ID NVARCHAR(40)
AS
BEGIN
	SET NOCOUNT ON;

    SELECT * 
    FROM dbo.T_BeginningBalance
    WHERE @C_ID = C_GUID
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdWriteOffRecord]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<魏礼明>
-- Create date: <2015/05/07>
-- Description:	<保存销账记录>
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdWriteOffRecord]
	-- Add the parameters for the stored procedure here
	@R_GUID nvarchar(40),
	@C_GUID nvarchar(40),
	@IE_Flag nvarchar(4)=null,
	@DebitLedgerAccount nvarchar(40),
	@DebitDetailsAccount nvarchar(40)=null,
	@CreditLedgerAccount nvarchar(40),
	@CreditDetailsAccount nvarchar(40)=null,
	@Amount decimal(18,4),
	@Date datetime,
	@Remark nvarchar(200)=null,
	@Creator nvarchar(40),
	@CreateDate datetime,
	@Currency nvarchar(5)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

    -- Insert statements for procedure here
	BEGIN TRAN;
	
	INSERT INTO dbo.T_IEWriteOff(R_GUID,C_GUID,IE_Flag,DebitLedgerAccount,DebitDetailsAccount,CreditLedgerAccount,CreditDetailsAccount,Amount,Date,Remark,Creator,CreateDate,Currency)
	VALUES(@R_GUID,@C_GUID,@IE_Flag,@DebitLedgerAccount,@DebitDetailsAccount,@CreditLedgerAccount,@CreditDetailsAccount,@Amount,@Date,@Remark,@Creator,@CreateDate,@Currency);
	IF(@IE_Flag='I')
	BEGIN
	delete from dbo.T_Receivables where R_GUID=@R_GUID;
	
    END
    IF(@IE_Flag='E')
    BEGIN
	delete from dbo.T_Payable where R_GUID=@R_GUID;
	
    END
	COMMIT TRAN;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdWageCost]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<chenxiang>
-- Create date: <Create Date,,>
-- Description:	更新收入费用记录
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdWageCost]
	@W_GUID NVARCHAR(40),
	@C_GUID NVARCHAR(40),
	@Date DATETIME,
	@Employee NVARCHAR(40),
	@Cash DECIMAL(18,2),
	@PersonalTaxes DECIMAL(18,2),
	@SocialSecurity DECIMAL(18,2),
	@Total DECIMAL(18,2)
	
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

    BEGIN TRAN;
	DELETE dbo.T_WageCost WHERE W_GUID = @W_GUID;
	-- Insert statements for procedure here
	INSERT INTO dbo.T_WageCost(W_GUID,C_GUID,Date,Employee,Cash,PersonalTaxes,SocialSecurity,Total)
	VALUES(@W_GUID,@C_GUID,@Date,@Employee,@Cash,@PersonalTaxes,@SocialSecurity,@Total);
	COMMIT TRAN;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdUsingLedgerAcc]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	更新总账科目使用状态
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdUsingLedgerAcc]
	@AccCodes NVARCHAR(4000),
	@C_ID NVARCHAR(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	
	DECLARE @AccCode NVARCHAR(40);
	DECLARE USER_CUR CURSOR FOR 
	SELECT CAST(short_str AS NVARCHAR(40)) FROM dbo.FUN_SPLIT(@AccCodes,',') T;
	
	BEGIN TRAN;
	DELETE dbo.R_CompanyAccount  WHERE C_GUID = @C_ID;
	OPEN USER_CUR;
	FETCH NEXT FROM USER_CUR INTO @AccCode;
	WHILE @@FETCH_STATUS = 0
	BEGIN
		INSERT INTO dbo.R_CompanyAccount(R_GUID,LA_GUID,C_GUID)
		VALUES(NEWID(),@AccCode,@C_ID);
		FETCH NEXT FROM USER_CUR INTO @AccCode;
	END
	CLOSE USER_CUR;
	DEALLOCATE USER_CUR;
	COMMIT TRAN;  
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdUserState]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	更新用户
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdUserState]

	@LoginName NVARCHAR(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	
	BEGIN TRAN;
    UPDATE T_User SET State = 1
	WHERE LoginName = @LoginName
    COMMIT TRAN;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdUserInfos]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<魏礼明>
-- Create date: <2015/04/09>
-- Description:	<保存用户信息>
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdUserInfos]
	-- Add the parameters for the stored procedure here
	@U_GUID NVARCHAR(50),
	@UserName NVARCHAR(50),
	@LoginName NVARCHAR(50),
	@C_GUID NVARCHAR(50),
	@Password NVARCHAR(50),
	@State int,
	@EnterC_GUID NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
SET XACT_ABORT ON;
    -- Insert statements for procedure here
	 BEGIN TRAN;
    DELETE dbo.T_User WHERE U_GUID=@U_GUID;
	INSERT INTO dbo.T_User VALUES(@U_GUID,@C_GUID,@UserName,@Password,@LoginName,@State,@EnterC_GUID)
	 COMMIT TRAN;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdUserInfo]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<魏礼明>
-- Create date: <2015/04/09>
-- Description:	<保存用户信息>
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdUserInfo]
	-- Add the parameters for the stored procedure here
	@U_GUID NVARCHAR(50),
	@UserName NVARCHAR(50),
	@LoginName NVARCHAR(50),
	@C_GUID NVARCHAR(50),
	@Password NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
SET XACT_ABORT ON;
    -- Insert statements for procedure here
	 BEGIN TRAN;
    DELETE dbo.T_User WHERE U_GUID=@U_GUID;
	INSERT INTO dbo.T_User VALUES(@U_GUID,@C_GUID,@UserName,@Password,@LoginName)
	 COMMIT TRAN;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdUser]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	更新用户
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdUser]
	@UserGuid NVARCHAR(40),
	@UserName NVARCHAR(40),
	@Password NVARCHAR(40),
	@CompanyGuid NVARCHAR(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	
	BEGIN TRAN;
    DELETE T_User WHERE U_GUID = @UserGuid;
    INSERT INTO T_User(U_GUID,C_GUID,UserName,[Password])
    VALUES(@UserGuid,@CompanyGuid,@UserName,@Password);
    COMMIT TRAN;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdTax]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<chenxiang>
-- Create date: <Create Date,,>
-- Description:	更新收入费用记录
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdTax]
	@T_GUID NVARCHAR(40),
	@Type NVARCHAR(40),
	@Name NVARCHAR(40),
	@Rate DECIMAL,
	@C_GUID NVARCHAR(40)
	
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

    BEGIN TRAN;
	DELETE dbo.T_Tax WHERE T_GUID = @T_GUID;
	-- Insert statements for procedure here
	INSERT INTO dbo.T_Tax(T_GUID,Type,Name,Rate,C_GUID)
	VALUES(@T_GUID,@Type,@Name,@Rate,@C_GUID);
	COMMIT TRAN;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdRR]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<chenxiang>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdRR]
	-- Add the parameters for the stored procedure here
	@Flag nvarchar(1),
	@ID nvarchar(40),
	@IE_GUID nvarchar(40),
	@InvType nvarchar(100)=null,
	@InvTypeDts nvarchar(100)=null,
	@CFItemGuid nvarchar(40)=null
	 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE T_RecPayRecord
	SET IE_GUID=@IE_GUID,InvType=@InvType,InvTypeDts=@InvTypeDts,CFItemGuid=@CFItemGuid
	WHERE RP_Flag=@Flag AND RP_GUID=@ID; 
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdRegInfo]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	更新注册信息
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdRegInfo]
	@U_GUID NVARCHAR(40),
	@C_GUID NVARCHAR(40),
	@UserName NVARCHAR(40) = NULL,
	@Password NVARCHAR(40),
	@MasterCompanyGuid NVARCHAR(40) = NULL,
	@CompanyName NVARCHAR(100),
	@Address NVARCHAR(50)= NULL,
	@Contacter NVARCHAR(50)= NULL,
	@ContactWay NVARCHAR(50)= NULL,
	@Type NVARCHAR(50)= NULL,
	@AuditDate DATETIME,
	@LoginName NVARCHAR(50),
	@State INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

    BEGIN TRAN;
    INSERT INTO T_Company(C_GUID,MasterCompanyGuid,Name,Address,Contacter,ContactWay,Type,AuditDate) 
    VALUES(@C_GUID,@MasterCompanyGuid,@CompanyName,@Address,@Contacter,@ContactWay,@Type,@AuditDate);
    INSERT INTO T_User(U_GUID,C_GUID,UserName,Password,LoginName,State)
    VALUES(@U_GUID,@C_GUID,@UserName,@Password,@LoginName,@State);

    DECLARE @LAS NVARCHAR(MAX) = 
    '2D2C780B-8191-4A5A-A1EE-0A2D969F2299,'+ --	应付职工薪酬
	'B2FC372F-EB9F-4005-B66F-FC07F44BD711,'+ --	盈余公积
	'082CD9EB-9947-43C4-A7C6-F2B7FAB6EE54,'+ --	所得税费用
	'271FDD30-2FAC-49B5-9F46-023424ABB4C7,'+ --	应收账款
	'76FF2B6B-932A-4CA1-850C-758310496AEB,'+ --	银行存款
	'DB57CF51-0328-4F82-842B-4710FB65AAA7,'+ --	专项应付款
	'88C60FC8-2FCB-41CD-B721-C58A981961B0,'+ --	预付账款
	'F9E8B745-CB32-449E-B410-209B9D43B7A3,'+ --	库存现金
	'994F056A-1461-4E6C-A25B-B81B90BBBB63,'+ --	本年利润
	'1F500FDD-1460-45DC-BE8F-39F5ACCE5D95,'+ --	利润分配
	'547E5A1A-1C20-4249-92C8-67FFFFBD38E7,'+ --	管理费用
	'28C49483-7FD7-4741-B154-B36EF52469D2,'+ --	其他业务收入
	'F85560AA-4951-4214-AF7F-5B890C9524B2,'+ --	财务费用
	'806373A3-41D2-4F40-AC51-2C4C82E318DE,'+ --	手续费及佣金支出
	'D27CA8F5-A98C-41E4-8E49-E0BE34E93035,'+ --	主营业务收入
	'76929BB2-2DF1-43BF-B33B-2F9D3FB851FB,'+ --	其他应付款
	'6e5d48b6-adc5-43b8-94e0-9c26e3277184,'+ --	待摊费用
	'1AC24C5E-FAB4-43EA-A303-A448604FC6C2,'+ --	其他业务成本
	'132B92DE-1469-411F-A75F-C04B61E507D1,'+ --	营业税金及附加
	'7C252091-900B-460C-8DBB-9F0DA2DC5506,'+ --	资本公积
	'B5920041-15AC-45A8-AC5C-8E8E83AB9076,'+ --	累计摊销
	'85BDCCDC-9D49-48F0-83A4-3F47B203DDC9,'+ --	长期待摊费用
	'86D57E6A-6207-46CF-A7E0-03C36A10DBBF,'+ --	汇兑损益
	'33CBFEF1-6B00-4B50-AD4F-3C98830F8B05,'+ --	应付账款
	'CD0D907D-5ED1-4785-ACFB-F59C2A8B920C,'+ --	以前年度损益调整
	'C2CDA7BC-C4D0-4B72-91CD-5EDEE7ED3B6A,'+ --	累计折旧
	'51BFDD3E-2253-4FBF-A946-19C18C25C6FC,'+ --	主营业务成本
	'4F380EB2-C1BC-483C-B229-A7FAEA03D054,'+ --	营业外支出
	'794F3D3A-D3AD-431F-988C-9CFBC2A1D207,'+ --	利息收入
	'EF58B046-0BDE-464E-96D2-8F731366349A,'+ --	预收账款
	'65BC8EDB-949A-4EF2-BCCD-AB5FE10DC88E,'+ --	营业外收入
	'FD7ED40C-6BA4-4C22-8029-6F1509441BEA,'+ --	固定资产
	'DC83D8A5-31F6-4DFE-B093-87F90A234E53,'+ --	销售费用
	'80FC4775-3169-42B0-A558-F3AE813DBF14,'+ --	其他应收款
	'60E8BAAA-2043-479F-9D13-E9753F5BA512,'+ --	实收资本
	'A7C86EFD-D448-4784-AFC2-758FCA90D9B8'; --	应交税费
    DECLARE @CurLA NVARCHAR(40);
    DECLARE USER_CUR CURSOR FOR 
	SELECT CAST(short_str AS NVARCHAR(40)) FROM dbo.FUN_SPLIT(@LAS,',') T;
	
	OPEN USER_CUR;
	FETCH NEXT FROM USER_CUR INTO @CurLA;
	WHILE @@FETCH_STATUS = 0
	BEGIN
		INSERT INTO dbo.R_CompanyAccount(R_GUID,LA_GUID,C_GUID)
		VALUES(NEWID(),@CurLA,@C_GUID);
		FETCH NEXT FROM USER_CUR INTO @CurLA;
	END
	CLOSE USER_CUR;
	DEALLOCATE USER_CUR;
	
    COMMIT TRAN;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdRecPayRecord]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	更新收付款记录
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdRecPayRecord]
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
	@IE_GUID NVARCHAR(40)=null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
    
    BEGIN TRAN;
    DELETE dbo.T_RecPayRecord WHERE RP_GUID = @ID;
	INSERT INTO dbo.T_RecPayRecord(RP_GUID,RP_Flag,InvType,InvTypeDts,InvNo,RPer,SumAmount,Date,Remark,Creator,CreateDate
	,DebitLedgerAccount,DebitDetailsAccount,CreditLedgerAccount,CreditDetailsAccount,
	C_GUID,RPable,Currency,
	CFItemGuid,CFPItemGuid,B_GUID,BA_GUID,IE_GUID)
	VALUES(@ID,@Flag,@InvType,@InvTypeDts,@InvNo,@R_Per,@Amount,@Date,@Remark,@Creator,@CreateDate
	,@DLA,@DDA,@CLA,@CDA,@C_GUID,@RPable,@Currency,@CFItemGuid,@CFPItemGuid,
	@B_GUID,@BA_GUID,@IE_GUID);
	
	COMMIT TRAN;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdReceivablesDeclareCustomer]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	更新收入费用记录
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdReceivablesDeclareCustomer]
	@GUID NVARCHAR(40),
	@InvType NVARCHAR(40),
	@RPer NVARCHAR(40),
	@Date DATETIME=NULL,
	@Amount DECIMAL,
	@Remark NVARCHAR(200)=null,
	@Currency NVARCHAR(20),
	@State nvarchar(20),
	@C_GUID NVARCHAR(40)
	
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

    -- Insert statements for procedure here
    BEGIN TRAN;
	DELETE dbo.T_DeclareCustomer WHERE GUID = @GUID;
	
	INSERT INTO dbo.T_DeclareCustomer(GUID,InvType,RPer,[Date],Amount,Remark,Currency,State,C_GUID)
	VALUES(@GUID,@InvType,@RPer,@Date,@Amount,@Remark,@Currency,@State,@C_GUID);
	COMMIT TRAN;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdReceivables]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<魏礼明>
-- Create date: <2015/05/04>
-- Description:	<更新应收账款>
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdReceivables]
	@R_GUID NVARCHAR(40),
	@C_GUID NVARCHAR(40),
	@Payer NVARCHAR(40),
	@Date DATE = NULL,
	@InvType NVARCHAR(20),
	@InvNo NVARCHAR(20)=NULL,
	@B_GUID NVARCHAR(40)=NULL,
	@BA_GUID NVARCHAR(40)=NULL,
	@Money DECIMAL,
	@Currency nvarchar(5)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

    -- Insert statements for procedure here
	 BEGIN TRAN;
	DELETE dbo.T_Receivables WHERE R_GUID = @R_GUID;
	
	INSERT INTO dbo.T_Receivables(R_GUID,C_GUID,Payer,Date,InvType,InvNo,B_GUID,BA_GUID,Money,Currency)
	VALUES(@R_GUID,@C_GUID,@Payer,@Date,@InvType,@InvNo,@B_GUID,@BA_GUID,@Money,@Currency);
	COMMIT TRAN;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdRateHistory]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<chenxiang>
-- Create date: <Create Date,,>
-- Description:	更新收入费用记录
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdRateHistory]
	@GUID NVARCHAR(40),
	@Date DATETIME,
	@FAmount DECIMAL(18,2),
	@FCurrency NVARCHAR(40),
	@TAmount DECIMAL(18,2),
	@TCurrency NVARCHAR(40),
	@C_GUID NVARCHAR(40)
	
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

    BEGIN TRAN;
	DELETE dbo.T_RateHistory WHERE GUID = @GUID;
	-- Insert statements for procedure here
	INSERT INTO dbo.T_RateHistory(GUID,Date,FAmount,FCurrency,TAmount,TCurrency,C_GUID)
	VALUES(@GUID,@Date,@FAmount,@FCurrency,@TAmount,@TCurrency,@C_GUID);
	COMMIT TRAN;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdPaymentDeclareCostSpending]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<chenxiang>
-- Create date: <Create Date,,>
-- Description:	更新收入费用记录
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdPaymentDeclareCostSpending]
	@GUID NVARCHAR(40),
	@InvType NVARCHAR(40),
	@RPer NVARCHAR(40),
	@Date DATETIME=NULL,
	@Amount DECIMAL,
	@Remark NVARCHAR(200)=null,
	@Currency NVARCHAR(20),
	@State nvarchar(20),
	@C_GUID NVARCHAR(40)
	
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

    -- Insert statements for procedure here
    BEGIN TRAN;
	DELETE dbo.T_DeclareCostSpending WHERE GUID = @GUID;
	
	INSERT INTO dbo.T_DeclareCostSpending(GUID,InvType,RPer,[Date],Amount,Remark,Currency,State,C_GUID)
	VALUES(@GUID,@InvType,@RPer,@Date,@Amount,@Remark,@Currency,@State,@C_GUID);
	COMMIT TRAN;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdPayable]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<魏礼明>
-- Create date: <2015/05/08>
-- Description:	<更新应付账款>
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdPayable]
	-- Add the parameters for the stored procedure here
	@R_GUID NVARCHAR(40),
	@C_GUID NVARCHAR(40),
	@Payee NVARCHAR(40)= NULL,
	@Date DATE = NULL,
	@InvType NVARCHAR(20),
	@InvNo NVARCHAR(20)=NULL,
	@B_GUID NVARCHAR(40)=null,
	@BA_GUID NVARCHAR(40)=null,
	@Money DECIMAL= NULL,
	@Currency nvarchar(5)= NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

    -- Insert statements for procedure here
	 BEGIN TRAN;
	DELETE dbo.T_Payable WHERE R_GUID = @R_GUID;
	
	INSERT INTO dbo.T_Payable(R_GUID,C_GUID,Payee,Date,InvType,InvNo,B_GUID,BA_GUID,Money,Currency)
	VALUES(@R_GUID,@C_GUID,@Payee,@Date,@InvType,@InvNo,@B_GUID,@BA_GUID,@Money,@Currency);
	COMMIT TRAN;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdPartner]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	更新合作伙伴
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdPartner]
	@ID NVARCHAR(40),
	@Name NVARCHAR(100),
	@IsCustomer BIT,
	@ISSupplier BIT,
	@IsPartner BIT,
	@C_GUID NVARCHAR(50),
	@ChineseFullName NVARCHAR(50)=null,
	@EnglishFullName NVARCHAR(50)=null,
	@Website NVARCHAR(50)=null,
	@OrganizationCode NVARCHAR(50)=null,
	@IndustryInvolved NVARCHAR(50)=null,
	@RegisteredAddress NVARCHAR(50)=null,
	@Remark NVARCHAR(100)=null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	
	BEGIN TRAN;
	DELETE FROM dbo.T_BusinessPartner WHERE BP_GUID = @ID;
    -- Insert statements for procedure here
	INSERT INTO dbo.T_BusinessPartner
	VALUES (@ID,@Name,@ISSupplier,@IsCustomer,@IsPartner,@C_GUID,@ChineseFullName,@EnglishFullName,@Website,@OrganizationCode,@IndustryInvolved,@RegisteredAddress,@Remark);
	COMMIT TRAN;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdLOGO]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<魏礼明>
-- Create date: <2014/04/08>
-- Description:	<更新公司信息>
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdLOGO] 
	-- Add the parameters for the stored procedure here
	@C_GUID NVARCHAR(50),
	@LOGO NVARCHAR(400)=null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

    -- Insert statements for procedure here
    BEGIN TRAN;
    
		UPDATE T_Company SET LOGO = @LOGO
		WHERE C_GUID = @C_GUID
	
	 COMMIT TRAN;
	END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdLedgerAccount]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	更新总账科目
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdLedgerAccount]
	@id NVARCHAR(40),
	@AccCode INT,
	@Name NVARCHAR(100),
	@AccGroup INT,
	@Useable bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	
	BEGIN TRAN;
	DELETE dbo.T_GeneralLedgerAccount WHERE lA_GUID = @id;
	
    INSERT INTO dbo.T_GeneralLedgerAccount(lA_GUID,AccGroup,AccCode,Name)
    VALUES(@id,@AccGroup,@AccCode,@Name);
    COMMIT TRAN;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdInvType]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<chenxiang>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdInvType]
	-- Add the parameters for the stored procedure here
	@Flag nvarchar(1),
	@ID nvarchar(40),
	@Type nvarchar(50),
	@TypeDts nvarchar(50),
	@IE_GUID nvarchar(40)=null,
	@Remark nvarchar(100)=null,
	@CFItemGuid nvarchar(40)  
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE T_RecPayRecord
	SET InvType=@Type,InvTypeDts=@TypeDts,IE_GUID=@IE_GUID,Remark=@Remark,CFItemGuid=@CFItemGuid
	WHERE RP_Flag=@Flag AND RP_GUID=@ID; 
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdInitialBalanceRecord]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdInitialBalanceRecord]
	-- Add the parameters for the stored procedure here
	@Inital_GUID NVARCHAR(40),
	@C_GUID NVARCHAR(40),
	@Currency NVARCHAR(40),
	@Date DATETIME,
	@MonetaryFunds DECIMAL(18,4),
	@ShorttermInvestments DECIMAL(18,4),
	@NotesReceivable DECIMAL(18,4),
	@SubsidiesReceivable DECIMAL(18,4),
	@DividendReceivable DECIMAL(18,4),
	@Inventories DECIMAL(18,4),
	@LongtermDebtOneYear DECIMAL(18,4),
	@OtherCcurrentAssets DECIMAL(18,4),
	@LongtermInvestments DECIMAL(18,4),
	@FixedAssetsNBV DECIMAL(18,4),
	@ConstructionInProgress DECIMAL(18,4),
	@IntangibleAssets DECIMAL(18,4),
	@Deferred DECIMAL(18,4),
	@ShorttermLoans DECIMAL(18,4),
	@NotesPayable DECIMAL(18,4),
	@AccountsPayable DECIMAL(18,4),
	@AdvancesFromCustomers DECIMAL(18,4),
	@AccruedPayroll DECIMAL(18,4),
	@LongtermLiabiltiesDueWithinaYear DECIMAL(18,4),
	@OtherCurrentLiabilities DECIMAL(18,4),
	@LongtermBorrowings DECIMAL(18,4),
	@LongtermPayables DECIMAL(18,4),
	@OtherLongtermLliabilities DECIMAL(18,4),
	@BankAccount1 NVARCHAR(40),
	@BankAccount2 NVARCHAR(40),
	@BankAccount1Money DECIMAL(18,4),
	@BankAccount2Money DECIMAL(18,4),
	@TaxesPayable DECIMAL(18,4)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	
	BEGIN TRAN;
	DELETE FROM dbo.T_Balance WHERE Inital_GUID = @Inital_GUID;
    -- Insert statements for procedure here
	INSERT INTO dbo.T_Balance(Inital_GUID,C_GUID,MonetaryFunds,ShorttermInvestments,NotesReceivable,SubsidiesReceivable,DividendReceivable,Inventories
,LongtermDebtOneYear,OtherCcurrentAssets,LongtermInvestments,FixedAssetsNBV,ConstructionInProgress,IntangibleAssets,Deferred,ShorttermLoans,NotesPayable
,AccountsPayable,AdvancesFromCustomers,AccruedPayroll,TaxesPayable,LongtermLiabiltiesDueWithinaYear,OtherCurrentLiabilities,LongtermBorrowings,LongtermPayables,OtherLongtermLliabilities
,Currency,Date,BankAccount1,BankAccount1Money,BankAccount2,BankAccount2Money
)VALUES (@Inital_GUID,@C_GUID,@MonetaryFunds,@ShorttermInvestments,@NotesReceivable,@SubsidiesReceivable,@DividendReceivable,@Inventories
,@LongtermDebtOneYear,@OtherCcurrentAssets,@LongtermInvestments,@FixedAssetsNBV,@ConstructionInProgress,@IntangibleAssets,@Deferred,@ShorttermLoans,@NotesPayable
,@AccountsPayable,@AdvancesFromCustomers,@AccruedPayroll,@TaxesPayable,@LongtermLiabiltiesDueWithinaYear,@OtherCurrentLiabilities,@LongtermBorrowings,@LongtermPayables,@OtherLongtermLliabilities
,@Currency,@Date,@BankAccount1,@BankAccount1Money,@BankAccount2,@BankAccount2Money);
	COMMIT TRAN;

END
GO
/****** Object:  StoredProcedure [dbo].[GetAIDRecord]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	获取公司
-- =============================================
CREATE PROCEDURE [dbo].[GetAIDRecord]
	@ID NVARCHAR(40),
	@C_GUID NVARCHAR(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT GUID,C_GUID,AID_Flag,Date,Amount,Currency,RPer,InvType,Description,DepreciationPeriod,SurplusValue,State,Remark
    FROM T_AIDRecord
    WHERE C_GUID = @C_GUID
    AND GUID=@ID
END
GO
/****** Object:  StoredProcedure [dbo].[SP_DelUser]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<魏礼明>
-- Create date: <2015/04/10>
-- Description:	<删除用户信息>
-- =============================================
CREATE PROCEDURE [dbo].[SP_DelUser]
	-- Add the parameters for the stored procedure here
	@U_GUID NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE FROM dbo.T_User WHERE U_GUID=@U_GUID
END
GO
/****** Object:  StoredProcedure [dbo].[SP_DelRecPayRecord]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	删除收付款记录
-- =============================================
CREATE PROCEDURE [dbo].[SP_DelRecPayRecord]
	@ID NVARCHAR(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

    -- Insert statements for procedure here
    BEGIN TRAN;
	DELETE dbo.T_RecPayRecord WHERE RP_GUID = @ID;
	COMMIT TRAN;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_DelPartner]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	删除商业伙伴
-- =============================================
CREATE PROCEDURE [dbo].[SP_DelPartner]
	@ID NVARCHAR(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE dbo.T_BusinessPartner WHERE BP_GUID = @ID; 
END
GO
/****** Object:  StoredProcedure [dbo].[SP_DelLedgerAccount]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	删除总账科目
-- =============================================
CREATE PROCEDURE [dbo].[SP_DelLedgerAccount] 
	-- Add the parameters for the stored procedure here
	@id NVARCHAR(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE dbo.T_GeneralLedgerAccount
	WHERE LA_GUID = @id;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_DelIERecord]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	删除收入费用记录
-- =============================================
CREATE PROCEDURE [dbo].[SP_DelIERecord]
	@ID NVARCHAR(40),
	@Flag nvarchar(4)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	
	BEGIN TRAN;
	IF(@Flag='I')
    BEGIN
	DELETE FROM dbo.T_Receivables WHERE R_GUID IN (SELECT R_GUID FROM dbo.T_IEDetails WHERE IE_GUID=@ID)
    END
	IF(@Flag='E')
    BEGIN
    DELETE FROM dbo.T_Payable WHERE R_GUID IN (SELECT R_GUID FROM dbo.T_IEDetails WHERE IE_GUID=@ID)
    END
    DELETE dbo.T_IEDetails WHERE IE_GUID = @ID;
    DELETE dbo.T_IERecord WHERE IE_GUID = @ID;
    COMMIT TRAN;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_DelExpenseTypeRecord]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[SP_DelExpenseTypeRecord]
	@ET_GUID NVARCHAR(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	BEGIN TRAN;
    DELETE dbo.T_ExpenseType  WHERE ET_GUID = @ET_GUID;
    COMMIT TRAN;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_DelEveryAttachment]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<chenxiang>
-- Create date: <20150720>
-- Description:	<一一删除记录下的每个附件>
-- =============================================
CREATE PROCEDURE [dbo].[SP_DelEveryAttachment] 
	-- Add the parameters for the stored procedure here
	@A_GUID NVARCHAR(50)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DELETE dbo.T_Attachment WHERE A_GUID = @A_GUID;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_DelDetailsAccount]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	删除明细科目
-- =============================================
CREATE PROCEDURE [dbo].[SP_DelDetailsAccount]
	@ID NVARCHAR(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    SET XACT_ABORT ON;
    
	BEGIN TRAN;
	DELETE dbo.T_DetailedAccount WHERE DA_GUID = @ID;
	COMMIT TRAN;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_DelDetails]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	删除银行
-- =============================================
CREATE PROCEDURE [dbo].[SP_DelDetails]
	@GUID NVARCHAR(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	BEGIN TRAN;
    DELETE dbo.T_DetailedCategories  WHERE GUID = @GUID;
    COMMIT TRAN;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_DelCompanyCy]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<魏礼明>
-- Create date: <2015/04/15>
-- Description:	<删除常用币制>
-- =============================================
CREATE PROCEDURE [dbo].[SP_DelCompanyCy]
	-- Add the parameters for the stored procedure here
	@C_GUID NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE dbo.R_CompanyCurrceny WHERE C_GUID=@C_GUID
END
GO
/****** Object:  StoredProcedure [dbo].[SP_DelBeginningBalance]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	删除期初数
-- =============================================
CREATE PROCEDURE [dbo].[SP_DelBeginningBalance]
	@C_ID NVARCHAR(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	BEGIN TRAN;
    DELETE dbo.T_BeginningBalance WHERE C_GUID = @C_ID;
    COMMIT TRAN;
    
END
GO
/****** Object:  StoredProcedure [dbo].[SP_DelBankAccount]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	删除银行账户
-- =============================================
CREATE PROCEDURE [dbo].[SP_DelBankAccount]
	@ID NVARCHAR(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DELETE dbo.T_BankAccount WHERE BA_GUID = @ID;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_DelBank]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	删除银行
-- =============================================
CREATE PROCEDURE [dbo].[SP_DelBank]
	@ID NVARCHAR(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	BEGIN TRAN;
    DELETE dbo.T_Bank  WHERE B_GUID = @ID;
    DELETE dbo.T_BankAccount  WHERE B_GUID = @ID; 
    COMMIT TRAN;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_DelAttachment]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	删除财务记录下的所有附件
-- =============================================
CREATE PROCEDURE [dbo].[SP_DelAttachment]
	@FR_ID NVARCHAR(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DELETE dbo.T_Attachment WHERE FR_GUID = @FR_ID;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_DelAIDRecord]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	删除收入费用记录
-- =============================================
CREATE PROCEDURE [dbo].[SP_DelAIDRecord]
	@ID NVARCHAR(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	
	BEGIN TRAN;
    DELETE dbo.T_AIDRecord WHERE GUID = @ID;
    COMMIT TRAN;
END
GO
/****** Object:  UserDefinedFunction [dbo].[FUN_Depreciation]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	固定资产折旧
-- =============================================
CREATE FUNCTION [dbo].[FUN_Depreciation]
(
	@AssetsCost DECIMAL(18,4),
	@SalvageValue DECIMAL(18,4),
	@Method INT ,
	@Life INT,
	@RegisterDate DATETIME,
	@Summary DATETIME
)
RETURNS DECIMAL(18,2)
AS
BEGIN
DECLARE @Result decimal(18,2);
SET @Result = 0;
IF @Method = 0
BEGIN--0:线性
	SET @Result = DBO.FUN_Depreciation_Line(@AssetsCost,@SalvageValue,@Life,@RegisterDate,@Summary);
	END
ELSE IF	@Method = 1
BEGIN--1:余额
	SET @Result = DBO.FUN_Depreciation_Overage(@AssetsCost,@SalvageValue,@Life,@RegisterDate,@Summary);
	END
ELSE
BEGIN--2:年数和
	SET @Result = DBO.FUN_Depreciation_Years(@AssetsCost,@SalvageValue,@Life,@RegisterDate,@Summary);
	END
	RETURN @Result;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_ChooseReceivablesDeclareCustomer]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<CHENXIANG>
-- Create date: <2015/09/08>
-- Description:	<查询所有应收账款>
-- =============================================
CREATE PROCEDURE [dbo].[SP_ChooseReceivablesDeclareCustomer]
	-- Add the parameters for the stored procedure here
	@PageSize int = -1,
	@PageIndex int = 1,
	@Count int = 0 out,
	@C_GUID nvarchar(40),
	@InvType nvarchar(40)=null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
   
    DECLARE @temp Table(
    rownumber int,
    GUID nvarchar(40),
    InvType nvarchar(40),
    RPer nvarchar(40),
    RPerName nvarchar(40),
    Amount decimal(18,4),
    Currency nvarchar(20),
    State nvarchar(20),
    Remark nvarchar(200),
    Date datetime,
    AGUID nvarchar(40)
    )
        
    insert into @temp
	select row_number()over(order by DC.Date desc) rownumber,DC.GUID,DC.InvType,DC.RPer,BP.Name RPerName,DC.Amount,DC.Currency,DC.State,DC.Remark,DC.Date,TA.A_GUID AGUID
	from dbo.T_DeclareCustomer DC
	LEFT JOIN dbo.T_BusinessPartner BP ON BP.BP_GUID = DC.RPer
	LEFT JOIN dbo.T_Attachment TA ON TA.FR_GUID = DC.GUID
	where DC.C_GUID=@C_GUID
	AND DC.State='未收'
	AND (DC.InvType = @InvType OR @InvType IS NULL OR LEN(@InvType)=0) 
	
	SELECT @Count = COUNT(*) FROM @temp;
	
	SELECT T.GUID,T.InvType,T.RPer,T.RPerName,T.Amount,T.Currency,T.State,T.Remark,T.Date,T.AGUID
		FROM @temp T
		WHERE (T.rownumber>= ((@PageIndex - 1)*@PageSize) + 1
		AND T.rownumber <= @PageIndex*@PageSize )
		OR (@PageSize = -1)
	
END
GO
/****** Object:  StoredProcedure [dbo].[SP_ChoosePaymentDeclare]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<CHENXIANG>
-- Create date: <2015/09/08>
-- Description:	<查询所有应收账款>
-- =============================================
CREATE PROCEDURE [dbo].[SP_ChoosePaymentDeclare]
	-- Add the parameters for the stored procedure here
	@PageSize int = -1,
	@PageIndex int = 1,
	@Count int = 0 out,
	@C_GUID nvarchar(40),
	@InvType nvarchar(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
   
    DECLARE @temp Table(
    rownumber int,
    GUID nvarchar(40),
    InvType nvarchar(40),
    RPer nvarchar(40),
    RPerName nvarchar(40),
    Amount decimal(18,4),
    Currency nvarchar(20),
    State nvarchar(20),
    Remark nvarchar(200),
    Date datetime,
    AGUID nvarchar(40)
    )
        
    insert into @temp
	select row_number()over(order by DC.Date desc) rownumber,DC.GUID,DC.InvType,DC.RPer,BP.Name RPerName,DC.Amount,DC.Currency,DC.State,DC.Remark,DC.Date,TA.A_GUID AGUID
	from dbo.T_DeclareCostSpending DC
	LEFT JOIN dbo.T_BusinessPartner BP ON BP.BP_GUID = DC.RPer
	LEFT JOIN dbo.T_Attachment TA ON TA.FR_GUID = DC.GUID
	where DC.C_GUID=@C_GUID
	AND DC.State='未付'
	AND (DC.InvType = @InvType OR @InvType IS NULL OR LEN(@InvType)=0) 
	
	SELECT @Count = COUNT(*) FROM @temp;
	
	SELECT T.GUID,T.InvType,T.RPer,T.RPerName,T.Amount,T.Currency,T.State,T.Remark,T.Date,T.AGUID
		FROM @temp T
		WHERE (T.rownumber>= ((@PageIndex - 1)*@PageSize) + 1
		AND T.rownumber <= @PageIndex*@PageSize )
		OR (@PageSize = -1)
	
END
GO
/****** Object:  StoredProcedure [dbo].[SP_AddAttachment]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<魏礼明>
-- Create date: <2015/05/20>
-- Description:	<新增附件>
-- =============================================
CREATE PROCEDURE [dbo].[SP_AddAttachment]
	-- Add the parameters for the stored procedure here
	@A_GUID nvarchar(50),
	@FileName nvarchar(200),
	@FileType nvarchar(500),
	@FR_GUID nvarchar(50),
	@FlieData VARBINARY(MAX),
	@FileRemark nvarchar(500) = NULL,
	@Number nvarchar(100)= NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO dbo.T_Attachment
    (A_GUID,FileName,FileType,FR_GUID,FlieData,FileRemark,Number)
    VALUES(@A_GUID,@FileName,@FileType,@FR_GUID,@FlieData,@FileRemark,@Number)
    end
GO
/****** Object:  StoredProcedure [dbo].[SP_GenPerviewCashFlowStatement]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	生成现金流量预览表
-- =============================================
CREATE PROCEDURE [dbo].[SP_GenPerviewCashFlowStatement]
	@C_ID NVarChar(40),
    @Year INT OUT,
    @Month INT OUT,
    @RepNo	NVARCHAR(40) OUT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @CurrentRepBeginDate DATE ;
	DECLARE @CurrentRepEndDate DATE ;
	
	--生成报表时间区间
	
	IF EXISTS (SELECT * FROM dbo.T_Report Rep WHERE  Rep.C_GUID = @C_ID AND Rep.Type='CF') 
	BEGIN 
		DECLARE @LastRepYear INT;
		DECLARE @LastRepMonth INT;
		
		SELECT TOP 1 @LastRepYear =Rep.[Year],@LastRepMonth = Rep.[Month]
		FROM dbo.T_Report Rep 
		WHERE  Rep.C_GUID = @C_ID AND Rep.Type='CF' 
		ORDER BY RepNo DESC;
		
		SET @CurrentRepBeginDate = DATEADD(year,@LastRepYear-1,CONVERT(DATE,'0001-1-1'));
		SET @CurrentRepBeginDate= DATEADD(month,@LastRepMonth,@CurrentRepBeginDate);	
	END
	ELSE
	BEGIN
		SELECT @CurrentRepBeginDate = ReportStartDate
		FROM dbo.T_CompanySetting
		WHERE C_GUID = @C_ID;
	END
	
	SET @Year = DATEPART(YEAR,@CurrentRepBeginDate);
	SET @Month = DATEPART(MONTH,@CurrentRepBeginDate);	
	SET @CurrentRepEndDate= DATEADD(month,1 ,@CurrentRepBeginDate);
	SET @RepNo = 'CF'+ CONVERT(NVARCHAR(8),@CurrentRepBeginDate,112);
    --生成报表数据
    DECLARE  @Result TABLE
    (
		RGUID NVARCHAR(40),
		Name NVARCHAR(100),
        EndingValue DECIMAL(18,2),
        AccGrp NVARCHAR(40),
        Code INT,
        RP_Flag NVARCHAR(1)
    )

    INSERT INTO @Result(RGUID,Name,AccGrp,Code,EndingValue,RP_Flag)
    SELECT CFI.R_GUID ,CFI.Name ,CFI.PID ,CFI.No ,(ISNULL(RP.Amount,0)) ,CFI.RP_Flag
    FROM dbo.T_CashFlowItem CFI
    LEFT JOIN 
    (SELECT CFItemGuid,SUM(SumAmount) AS Amount
		FROM dbo.T_RecPayRecord 
		WHERE C_GUID = @C_ID AND DATE BETWEEN @CurrentRepBeginDate AND @CurrentRepEndDate
		GROUP BY CFItemGuid
		)RP
		ON  RP.CFItemGuid = CFI.R_GUID;
		
	DECLARE  @TmpResult TABLE
    (
        EndingValue DECIMAL(18,2),
        AccGrp NVARCHAR(40),
        RP_Flag NVARCHAR(1)
    )
    
	INSERT INTO @TmpResult(AccGrp,RP_Flag,EndingValue)
	SELECT R.AccGrp,R.RP_Flag,SUM(R.EndingValue)
	FROM @Result R
	GROUP BY R.AccGrp,R.RP_Flag
	
	DECLARE @I_VAL DECIMAL(18,2);
	DECLARE @O_VAL DECIMAL(18,2);
	
	SELECT @I_VAL = TR.EndingValue
	FROM @TmpResult TR
	WHERE TR.AccGrp = '5C552CC3-4008-4AE4-9CBA-423B6AAA486A' AND TR.RP_Flag = 'R';
	
	INSERT INTO @Result(RGUID,Name,AccGrp,Code,EndingValue)
	SELECT NEWID(),'现金流入小计','5C552CC3-4008-4AE4-9CBA-423B6AAA486A',9,@I_VAL;
	
	SELECT @O_VAL = TR.EndingValue
	FROM @TmpResult TR
	WHERE TR.AccGrp = '5C552CC3-4008-4AE4-9CBA-423B6AAA486A' AND TR.RP_Flag = 'P';
	
	INSERT INTO @Result(RGUID,Name,AccGrp,Code,EndingValue)
	SELECT NEWID(),'现金流出小计','5C552CC3-4008-4AE4-9CBA-423B6AAA486A',20,@O_VAL;

	INSERT INTO @Result(RGUID,Name,AccGrp,Code,EndingValue)
	SELECT NEWID(),'经营活动产生的现金流量净额','5C552CC3-4008-4AE4-9CBA-423B6AAA486A',21,@I_VAL-@O_VAL;

	SELECT @I_VAL = TR.EndingValue
	FROM @TmpResult TR
	WHERE TR.AccGrp = '56E9F013-BB86-4B5E-95D6-A33F9B697AF9' AND TR.RP_Flag = 'R';

	INSERT INTO @Result(RGUID,Name,AccGrp,Code,EndingValue)
	SELECT NEWID(),'现金流入小计','56E9F013-BB86-4B5E-95D6-A33F9B697AF9',29,@I_VAL;
	
	SELECT @O_VAL = TR.EndingValue
	FROM @TmpResult TR
	WHERE TR.AccGrp = '56E9F013-BB86-4B5E-95D6-A33F9B697AF9' AND TR.RP_Flag = 'P';
	
	INSERT INTO @Result(RGUID,Name,AccGrp,Code,EndingValue)
	SELECT NEWID(),'现金流出小计','56E9F013-BB86-4B5E-95D6-A33F9B697AF9',36,@O_VAL;
	
	INSERT INTO @Result(RGUID,Name,AccGrp,Code,EndingValue)
	SELECT NEWID(),'投资活动产生的现金流量净额','56E9F013-BB86-4B5E-95D6-A33F9B697AF9',37,@I_VAL-@O_VAL;
	
	SELECT @I_VAL = TR.EndingValue
	FROM @TmpResult TR
	WHERE TR.AccGrp = 'DDCEC6F9-EC23-4F5C-BF96-A3D373AAA2C7' AND TR.RP_Flag = 'R';
	
	INSERT INTO @Result(RGUID,Name,AccGrp,Code,EndingValue)
	SELECT NEWID(),'现金流入小计','DDCEC6F9-EC23-4F5C-BF96-A3D373AAA2C7',44,@I_VAL;

	SELECT @O_VAL = TR.EndingValue
	FROM @TmpResult TR
	WHERE TR.AccGrp = 'DDCEC6F9-EC23-4F5C-BF96-A3D373AAA2C7' AND TR.RP_Flag = 'P';
	
	INSERT INTO @Result(RGUID,Name,AccGrp,Code,EndingValue)
	SELECT NEWID(),'现金流出小计','DDCEC6F9-EC23-4F5C-BF96-A3D373AAA2C7',53,@O_VAL;

	INSERT INTO @Result(RGUID,Name,AccGrp,Code,EndingValue)
	SELECT NEWID(),'筹资活动产生的现金流量净额','DDCEC6F9-EC23-4F5C-BF96-A3D373AAA2C7',54,@I_VAL-@O_VAL;
	
	INSERT INTO @Result(RGUID,Name,AccGrp,Code,EndingValue)
	SELECT NEWID(),'现金及现金等价物净增加额',NULL,56,SUM(EndingValue)
	FROM @Result
	WHERE Code = 21 OR Code = 37 OR Code = 54;
	
	SELECT *
	FROM @Result
	WHERE Isnull(ACCGRP  ,'')<>''
	ORDER BY AccGrp ,isnull(RP_flag,'zz') ,Code 
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetAllDetail]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,chenxiang>
-- Create date: <Create Date,,07/16/2015>
-- Description:	获取收付款记录
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetAllDetail]

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT Name,GUID
    FROM T_DetailedCategories
    WHERE State='启用'

END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetAIDRecordUp]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,chenxiang>
-- Create date: <Create Date,,07/16/2015>
-- Description:	获取采购记录
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetAIDRecordUp]
	@PageSize int = -1,
	@PageIndex int = 1,
	@Count int = 0 out,
	@Flag NVARCHAR(1) = NULL,
	@C_GUID NVARCHAR(50),
	@DateBegin DATETIME = NULL,
	@DateEnd DATETIME = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @temp TABLE(
		rownumber int,
		GUID NVARCHAR(40),
		Date DATE,
		Amount DECIMAL(18,4),
		Currency NVARCHAR(40),
		RPer NVARCHAR(40),
		RPerName NVARCHAR(40),
		InvType NVARCHAR(40),
		Description NVARCHAR(100),
		DepreciationPeriod INT,
		SurplusValue DECIMAL(18,4),
		State NVARCHAR(40),
		Remark NVARCHAR(200),
		A_GUID NVARCHAR(40)
	 )
	 
    -- Insert statements for procedure here

    BEGIN
		insert into @temp
		SELECT row_number()over(order by AID.Date desc) rownumber,AID.GUID,AID.Date,AID.Amount,AID.Currency,AID.RPer,BP.Name AS RPerName,AID.InvType,AID.Description,AID.DepreciationPeriod,AID.SurplusValue,AID.State,AID.Remark,TA.A_GUID AS A_GUID
		FROM dbo.T_AIDRecord AID
		LEFT JOIN dbo.T_BusinessPartner BP ON AID.RPer = BP.BP_GUID
		LEFT JOIN dbo.T_Attachment TA ON TA.FR_GUID = AID.GUID
		LEFT JOIN dbo.T_DeclareCostSpending TDCS ON TDCS.GUID = AID.GUID
		WHERE AID.C_GUID=@C_GUID
		AND (TDCS.State='未付')
		AND (AID.AID_Flag = @Flag or @Flag IS NULL OR LEN(@Flag) = 0)
		AND(AID.Date >= @DateBegin OR @DateBegin IS NULL)
		AND(AID.Date < DATEADD(day,1,@DateEnd) OR @DateEnd IS NULL)

		SELECT @Count = COUNT(*) FROM @temp;
		SELECT T.GUID,T.Date,T.Amount,T.Currency,T.RPer,T.RPerName,T.InvType,T.Description,T.DepreciationPeriod,T.SurplusValue,T.State,T.Remark,T.A_GUID
		FROM @temp T
		WHERE (T.rownumber>= ((@PageIndex - 1)*@PageSize) + 1
		AND T.rownumber <= @PageIndex*@PageSize )
		OR (@PageSize = -1)
	END
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetAIDRecord]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,chenxiang>
-- Create date: <Create Date,,07/16/2015>
-- Description:	获取采购记录
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetAIDRecord]
	@PageSize int = -1,
	@PageIndex int = 1,
	@Count int = 0 out,
	@Flag NVARCHAR(1) = NULL,
	@C_GUID NVARCHAR(50),
	@DateBegin DATETIME = NULL,
	@DateEnd DATETIME = NULL,
	@Customer NVARCHAR(40) = NULL,
	@Grp NVARCHAR(40)=NULL,
	@State NVARCHAR(40)=NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @temp TABLE(
		rownumber int,
		GUID NVARCHAR(40),
		Date DATE,
		Amount DECIMAL(18,4),
		Currency NVARCHAR(40),
		RPer NVARCHAR(40),
		RPerName NVARCHAR(40),
		InvType NVARCHAR(40),
		Description NVARCHAR(100),
		DepreciationPeriod INT,
		SurplusValue DECIMAL(18,4),
		State NVARCHAR(40),
		Remark NVARCHAR(200),
		A_GUID NVARCHAR(40)
	 )
	 
    -- Insert statements for procedure here

    BEGIN
		insert into @temp
		SELECT row_number()over(order by AID.Date desc) rownumber,GUID,Date,Amount,Currency,RPer,BP.Name AS RPerName,InvType,Description,DepreciationPeriod,SurplusValue,State,AID.Remark,TA.A_GUID AS A_GUID
		FROM dbo.T_AIDRecord AID
		LEFT JOIN dbo.T_BusinessPartner BP ON AID.RPer = BP.BP_GUID
		LEFT JOIN dbo.T_Attachment TA ON TA.FR_GUID = AID.GUID
		WHERE AID.C_GUID=@C_GUID
		AND (AID.AID_Flag = @Flag or @Flag IS NULL OR LEN(@Flag) = 0)
		AND(AID.Date >= @DateBegin OR @DateBegin IS NULL)
		AND(AID.Date < DATEADD(day,1,@DateEnd) OR @DateEnd IS NULL)
		AND(AID.RPer = @Customer OR @Customer IS NULL OR LEN(@Customer) = 0)
		AND(AID.InvType = @Grp OR @Grp IS NULL OR LEN(@Grp)=0)
		AND(AID.State = @State OR @State IS NULL OR LEN(@State)=0)

		SELECT @Count = COUNT(*) FROM @temp;
		SELECT T.GUID,T.Date,T.Amount,T.Currency,T.RPer,T.RPerName,T.InvType,T.Description,T.DepreciationPeriod,T.SurplusValue,T.State,T.Remark,T.A_GUID
		FROM @temp T
		WHERE (T.rownumber>= ((@PageIndex - 1)*@PageSize) + 1
		AND T.rownumber <= @PageIndex*@PageSize )
		OR (@PageSize = -1)
	END
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetCashFlowItems]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	获取现金流量项目
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetCashFlowItems]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT *
    FROM dbo.T_CashFlowItem
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetBeginningBalance]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	获取起初数
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetBeginningBalance]
	@C_ID NVARCHAR(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT '1' AS Acc_GUID,'' AS _parentId,0 AS Money ,'资产类' AS Acc_Name,'1' AS Acc_Code
    UNION
    SELECT '2' AS Acc_GUID,'' AS _parentId,0 AS Money,'负债类' AS Acc_Name,'2' AS Acc_Code
    UNION
    SELECT '4' AS Acc_GUID,'' AS _parentId,0 AS Money,'所有者权益' AS Acc_Name,'4' AS Acc_Code
    UNION
	SELECT LA.LA_GUID AS Acc_GUID,CONVERT (NVARCHAR(40),LA.AccGroup) AS _parentId,ISNULL(BB.Money,0),
	LA.Name AS Acc_Name,LA.AccCode AS Acc_Code
	FROM dbo.T_GeneralLedgerAccount LA
	INNER JOIN dbo.R_CompanyAccount R_CA ON R_CA.LA_GUID = LA.LA_GUID AND R_CA.C_GUID = @C_ID
	LEFT JOIN dbo.T_BeginningBalance BB ON BB.Acc_GUID = LA.LA_GUID AND BB.C_GUID = @C_ID
	WHERE LA.AccGroup = 1 OR  LA.AccGroup = 2 OR  LA.AccGroup = 4
	UNION
	SELECT DA.DA_GUID AS Acc_GUID,DA.ParentAccGuid AS _parentId,ISNULL(BB.Money,0),
	DA.Name AS Acc_Name,DA.AccCode Acc_Code
	FROM dbo.T_GeneralLedgerAccount LA
	INNER JOIN dbo.R_CompanyAccount R_CA ON R_CA.LA_GUID = LA.LA_GUID AND R_CA.C_GUID = @C_ID
	INNER JOIN dbo.T_DetailedAccount DA ON DA.ParentAccGuid = LA.LA_GUID AND DA.C_GUID = @C_ID
	LEFT JOIN dbo.T_BeginningBalance BB ON BB.Acc_GUID = DA.DA_GUID AND BB.C_GUID = @C_ID
	WHERE LA.AccGroup = 1 OR  LA.AccGroup = 2 OR  LA.AccGroup = 4
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetBanks]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	获取银行
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetBanks]
@ID NVARCHAR(40) = NULL,
@Name NVARCHAR(40) = NULL,
@C_ID NVARCHAR(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT B.B_GUID,B.Name,B.C_GUID
    FROM dbo.T_Bank B
    WHERE (B.B_GUID = @ID OR @ID IS NULL OR LEN(@ID) = 0)
    AND (B.Name = @Name OR @Name IS NULL OR LEN(@Name) = 0)
    AND (B.C_GUID = @C_ID)
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetBankAccounts]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	获取银行账户
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetBankAccounts]
	@ID NVARCHAR(40)= NULL,
	@B_ID NVARCHAR(40)= NULL,
	@Account NVARCHAR(40)= NULL,
	@C_ID NVARCHAR(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT BA.BA_GUID,BA.B_GUID,BA.Account,BA.C_GUID,BA.AccountName,BA.AccountCurrency,BA.AccountAbbreviation,BA.AccountType,BA.BankAddress,BA.SwiftCode
    FROM dbo.T_BankAccount BA
    WHERE (BA.BA_GUID = @ID OR @ID IS NULL OR LEN(@ID) = 0)
    AND (BA.B_GUID = @B_ID OR @B_ID IS NULL OR LEN(@B_ID) = 0)
    AND (BA.Account = @Account OR @Account IS NULL OR LEN(@Account) = 0)
    AND(@C_ID = BA.C_GUID)
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetAttachmentByID]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<chenxiang>
-- Create date: <20150720>
-- Description:	<查看附件>
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetAttachmentByID] 
	-- Add the parameters for the stored procedure here
	@File_GUID nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT A_GUID,FileName,FileType,FlieData,FR_GUID 
	FROM dbo.T_Attachment 
	WHERE A_GUID=@File_GUID
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetAttachment]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<chenxiang>
-- Create date: <20150720>
-- Description:	<查看附件>
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetAttachment] 
	-- Add the parameters for the stored procedure here
	@FR_GUID nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT A_GUID,FileName,FileType,FlieData,FR_GUID,FileRemark
	FROM dbo.T_Attachment 
	WHERE (FR_GUID=@FR_GUID OR @FR_GUID IS NULL OR LEN(@FR_GUID)=0)
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetAssetsGroups]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	获取固定资产分类
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetAssetsGroups]
@C_GUID NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT AG_GUID,Name,DepreciationMethod,Life,SalvageRate
	FROM dbo.T_AssetsGroup
	WHERE C_GUID=@C_GUID
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetExpenseTypeList]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,chenxiang>
-- Create date: <Create Date,,07/16/2015>
-- Description:	获取收付款记录
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetExpenseTypeList]
	@PageSize int = -1,
	@PageIndex int = 1,
	@ID nvarchar(40)=NULL,
	@Count int = 0 out
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @temp TABLE(
		rownumber int,
		ET_GUID NVARCHAR(40),
		ExpenseType NVARCHAR(40),
		ExpenseFlag NVARCHAR(1),
		SaleFlag NVARCHAR(1),
		ManageFlag NVARCHAR(1),
		FinanceFlag NVARCHAR(1),
		SalaryFlag NVARCHAR(1),
		TaxFlag NVARCHAR(1)
	 )
	 
    -- Insert statements for procedure here
    
		insert into @temp
		SELECT row_number()over(order by ExpenseType desc) rownumber,ET_GUID,ExpenseType,ExpenseFlag,SaleFlag,ManageFlag,FinanceFlag,SalaryFlag,TaxFlag
		FROM dbo.T_ExpenseType
		WHERE 1=1
		AND (ET_GUID=@ID OR @ID IS NULL OR LEN(@ID) = 0)
			
		SELECT @Count = COUNT(*) FROM @temp
		
		SELECT T.ET_GUID,T.ExpenseType,T.ExpenseFlag,T.SaleFlag,T.ManageFlag,T.FinanceFlag,T.SalaryFlag,T.TaxFlag
		FROM @temp T
		WHERE (T.rownumber>= ((@PageIndex - 1)*@PageSize) + 1
		AND T.rownumber <= @PageIndex*@PageSize )
		OR (@PageSize = -1)
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetDetailList]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,chenxiang>
-- Create date: <Create Date,,07/16/2015>
-- Description:	获取收付款记录
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetDetailList]
	@PageSize int = -1,
	@PageIndex int = 1,
	@Count int = 0 out
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @temp TABLE(
		rownumber int,
		GUID NVARCHAR(40),
		Name NVARCHAR(40),
		State NVARCHAR(40)
	 )
	 
    -- Insert statements for procedure here
    
		insert into @temp
		SELECT row_number()over(order by Name desc) rownumber,GUID,Name,State
		FROM dbo.T_DetailedCategories
		WHERE 1=1
			
		SELECT @Count = COUNT(*) FROM @temp
		
		SELECT T.GUID,T.Name,T.State
		FROM @temp T
		WHERE (T.rownumber>= ((@PageIndex - 1)*@PageSize) + 1
		AND T.rownumber <= @PageIndex*@PageSize )
		OR (@PageSize = -1)
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetDetailedAccountss]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	获取明细科目
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetDetailedAccountss]
@ID NVARCHAR(40)=NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT GUID,Name,State
	FROM T_DetailedCategories
	WHERE GUID=@ID
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetDetailedAccountsParentAccGuid]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetDetailedAccountsParentAccGuid] 
	@ID NVARCHAR(40)=NULL,
	@C_ID NVARCHAR(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT DA_GUID,C_GUID,AccCode,Name,ParentAccGuid,D_GUID
	FROM T_DetailedAccount
	WHERE (ParentAccGuid=@ID OR @ID IS NULL OR LEN(@ID) = 0)
	AND (C_GUID=@C_ID)
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetDetailedAccounts]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	获取明细科目
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetDetailedAccounts]
@ID NVARCHAR(40)=NULL,
@C_ID NVARCHAR(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT LA.LA_GUID AS DA_GUID,LA.AccCode,LA.Name,'' AS ParentAccGuid
	FROM dbo.T_GeneralLedgerAccount LA
	INNER JOIN dbo.R_CompanyAccount R_CA ON LA.LA_GUID = R_CA.LA_GUID AND R_CA.C_GUID= @C_ID
	WHERE (LA.LA_GUID = @ID OR @ID IS NULL OR LEN(@ID) = 0)
    UNION
    SELECT DA.DA_GUID,DA.AccCode,DA.Name,DA.ParentAccGuid
    FROM dbo.T_DetailedAccount DA
    WHERE (DA.DA_GUID = @ID OR @ID IS NULL OR LEN(@ID) = 0)
    AND (@C_ID = DA.C_GUID)
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetDeclareCustomer]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,chenxiang>
-- Create date: <Create Date,,07/16/2015>
-- Description:	获取收付款记录
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetDeclareCustomer]
	@PageSize int = -1,
	@PageIndex int = 1,
	@Count int = 0 out,
	@Flag NVARCHAR(1) = NULL,
	@C_GUID NVARCHAR(50),
	@InvTypeDts NVARCHAR(50)=NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @temp TABLE(
		rownumber int,
		RP_GUID NVARCHAR(40),
		InvType nvarchar(20),
		RPer nvarchar(40),
		SumAmount decimal(18,2),
		Date date,
		Remark nvarchar(200),
		R_PerName nvarchar(100),
		Currency NVARCHAR(5),
		B_GUID NVARCHAR(40),
		BA_GUID NVARCHAR(40),
		BankAccount NVARCHAR(100),
		A_GUID NVARCHAR(50)
	 )
	 
    -- Insert statements for procedure here
    
		insert into @temp
		SELECT row_number()over(order by PR.Date desc) rownumber,PR.RP_GUID,PR.InvType,PR.RPer,PR.SumAmount,PR.Date,PR.Remark,BP.Name R_PerName,
		PR.Currency,PR.B_GUID,PR.BA_GUID,BA.Account AS BankAccount,TA.A_GUID
		FROM dbo.T_RecPayRecord PR
		LEFT JOIN dbo.T_BusinessPartner BP ON PR.RPer = BP.BP_GUID
		LEFT JOIN dbo.T_BankAccount BA ON BA.BA_GUID = PR.BA_GUID
		LEFT JOIN dbo.T_Attachment TA ON TA.FR_GUID = PR.RP_GUID
		WHERE PR.C_GUID=@C_GUID
		AND (PR.RP_Flag = @Flag or @Flag IS NULL OR LEN(@Flag) = 0)
		AND(PR.InvTypeDts = @InvTypeDts OR @InvTypeDts IS NULL OR LEN(@InvTypeDts)=0)
		AND PR.RP_GUID NOT IN(SELECT C.RP_GUID FROM T_RecPayRecord C INNER JOIN dbo.T_IERecord TIE ON TIE.IE_GUID = PR.RP_GUID)
			
		SELECT @Count = COUNT(*) FROM @temp;
		SELECT T.RP_GUID,T.InvType,T.RPer,T.SumAmount,T.Date,T.Remark,T.R_PerName,Currency ,BA_GUID ,BankAccount,T.A_GUID 
		FROM @temp T
		WHERE (T.rownumber>= ((@PageIndex - 1)*@PageSize) + 1
		AND T.rownumber <= @PageIndex*@PageSize )
		OR (@PageSize = -1)
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetCurrency]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	获取币制
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetCurrency]
	@IsCommon BIT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT *
	FROM T_CurrencyCode C
	WHERE (C.IsCommon = @IsCommon OR @IsCommon IS NULL)
	
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetCompanySetting]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<魏礼明>
-- Create date: <2015/04/14>
-- Description:	<获取公司设置信息>
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetCompanySetting] 
	-- Add the parameters for the stored procedure here
	@ID NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM dbo.T_CompanySetting WHERE C_GUID=@ID;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetCompanyInformation]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	获取银行账户
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetCompanyInformation]
	@C_ID NVARCHAR(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT C_GUID,Name,Address,ChineseFullName,EnglishFullName,Website,OrganizationCode,IndustryInvolved,RegisteredAddress,Remark,LOGO,BusinessLicense
    FROM dbo.T_Company
    WHERE C_GUID=@C_ID
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetCompanyInfo]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<魏礼明>
-- Create date: <2015/04/08>
-- Description:	<获取公司信息>
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetCompanyInfo]
	-- Add the parameters for the stored procedure here
	@id NVARCHAR(50)=NULL,
	@Name NVARCHAR(50)=NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM dbo.T_Company WHERE (C_GUID=@id OR @id IS NULL OR LEN(@id)=0) AND (Name=@Name OR @Name IS NULL OR LEN(@Name)=0)
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetCompanyCurrceny]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<魏礼明>
-- Create date: <2015/04/14>
-- Description:	<获取常用币制>
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetCompanyCurrceny]
	-- Add the parameters for the stored procedure here
	@ID NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM dbo.R_CompanyCurrceny WHERE C_GUID=@ID
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetCompany]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	获取公司
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetCompany]
	@MasterCompanyGuid NVARCHAR(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT C.*
    FROM T_Company C
    WHERE C.C_GUID = @MasterCompanyGuid OR C.MasterCompanyGuid = @MasterCompanyGuid
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetReceivablesSelfListTwo]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,chenxiang>
-- Create date: <Create Date,,07/16/2015>
-- Description:	获取收付款记录
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetReceivablesSelfListTwo]
	@PageSize int = -1,
	@PageIndex int = 1,
	@Count int = 0 out,
	@ID NVARCHAR(40) = NULL,
	@Flag NVARCHAR(1) = NULL,
	@C_GUID NVARCHAR(50),
	@IsAll bit = 0,
	@ClassifyFlag bit = NULL,
	@dateBegin DATETIME = NULL,
	@dateEnd DATETIME = NULL,
	@customer NVARCHAR(40) = NULL,
	@incomeGrp NVARCHAR(20)=NULL,
	@incomeGrpdts NVARCHAR(40)=NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @temp TABLE(
		rownumber int,
		RP_GUID NVARCHAR(40),
		IE_GUID NVARCHAR(40),
		InvType nvarchar(40),
		InvTypeDts nvarchar(40),
		InvNo nvarchar(20),
		RPer nvarchar(40),
		SumAmount decimal(18,2),
		Date date,
		Remark nvarchar(200),
		Creator  nvarchar(40),
		CreateDate datetime,
		DebitLedgerAccount nvarchar(40),
		DebitDetailsAccount nvarchar(40),
		CreditLedgerAccount nvarchar(40),
		CreditDetailsAccount nvarchar(40),
		R_PerName nvarchar(100),
		DebitLedgerAccountName nvarchar(40),
		CreditLedgerAccountName nvarchar(40),
		Currency NVARCHAR(5),
		B_GUID NVARCHAR(40),
		BA_GUID NVARCHAR(40),
		BankAccount NVARCHAR(100),
		A_GUID NVARCHAR(50)
	 )
	 
    -- Insert statements for procedure here
		insert into @temp
			SELECT row_number()over(order by PR.Date desc) rownumber,PR.RP_GUID,PR.IE_GUID,PR.InvType,PR.InvTypeDts,PR.InvNo,PR.RPer,PR.SumAmount,PR.Date,PR.Remark,PR.Creator,PR.CreateDate
			,PR.DebitLedgerAccount,PR.DebitDetailsAccount,PR.CreditLedgerAccount,PR.CreditDetailsAccount,BP.Name R_PerName,
			LA2.Name AS DebitLedgerAccountName,LA1.Name AS CreditLedgerAccountName,
			PR.Currency,PR.B_GUID,PR.BA_GUID,BA.Account AS BankAccount,TA.A_GUID
			FROM dbo.T_RecPayRecord PR
			LEFT JOIN dbo.T_BusinessPartner BP ON PR.RPer = BP.BP_GUID
			LEFT JOIN dbo.T_GeneralLedgerAccount LA1 ON LA1.LA_GUID = PR.CreditLedgerAccount
			LEFT JOIN dbo.T_GeneralLedgerAccount LA2 ON LA2.LA_GUID = PR.DebitLedgerAccount
			LEFT JOIN dbo.T_BankAccount BA ON BA.BA_GUID = PR.BA_GUID
			LEFT JOIN dbo.T_Attachment TA ON TA.FR_GUID = PR.RP_GUID
			WHERE PR.RP_GUID NOT IN(SELECT PR1.RP_GUID FROM T_RecPayRecord PR1 
										WHERE PR1.RP_GUID IN(select A.IE_GUID FROM  T_RecPayRecord A WHERE A.RP_Flag = 'R' AND A.InvTypeDts = '收回投资的收款'))
			AND PR.C_GUID=@C_GUID
			AND (PR.RP_Flag = @Flag or @Flag IS NULL OR LEN(@Flag) = 0)
			AND(PR.Date >= @dateBegin OR @dateBegin IS NULL)
			AND(PR.Date < DATEADD(day,1,@dateEnd) OR @dateEnd IS NULL)
			AND(PR.RPer = @customer OR @customer IS NULL OR LEN(@customer) = 0)
			AND(PR.InvType = @incomeGrp OR @incomeGrp IS NULL OR LEN(@incomeGrp)=0)
			AND(PR.InvTypeDts = @incomeGrpdts OR @incomeGrpdts IS NULL OR LEN(@incomeGrpdts)=0)
			
		SELECT @Count = COUNT(*) FROM @temp;
		SELECT T.RP_GUID,T.IE_GUID,T.InvType,T.InvTypeDts,T.InvNo,T.RPer,T.SumAmount,T.Date,T.Remark,T.Creator,T.CreateDate
			,T.DebitLedgerAccount,T.DebitDetailsAccount,T.CreditLedgerAccount,T.CreditDetailsAccount,T.R_PerName,T.DebitLedgerAccountName,T.CreditLedgerAccountName
			,Currency ,BA_GUID ,BankAccount,T.A_GUID 
		FROM @temp T
		WHERE (T.rownumber>= ((@PageIndex - 1)*@PageSize) + 1
		AND T.rownumber <= @PageIndex*@PageSize )
		OR (@PageSize = -1)
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetReceivablesSelfListThree]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,chenxiang>
-- Create date: <Create Date,,07/16/2015>
-- Description:	获取收付款记录
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetReceivablesSelfListThree]
	@PageSize int = -1,
	@PageIndex int = 1,
	@Count int = 0 out,
	@ID NVARCHAR(40) = NULL,
	@Flag NVARCHAR(1) = NULL,
	@C_GUID NVARCHAR(50),
	@IsAll bit = 0,
	@ClassifyFlag bit = NULL,
	@dateBegin DATETIME = NULL,
	@dateEnd DATETIME = NULL,
	@customer NVARCHAR(40) = NULL,
	@incomeGrp NVARCHAR(20)=NULL,
	@incomeGrpdts NVARCHAR(40)=NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @temp TABLE(
		rownumber int,
		RP_GUID NVARCHAR(40),
		IE_GUID NVARCHAR(40),
		InvType nvarchar(40),
		InvTypeDts nvarchar(40),
		InvNo nvarchar(20),
		RPer nvarchar(40),
		SumAmount decimal(18,2),
		Date date,
		Remark nvarchar(200),
		Creator  nvarchar(40),
		CreateDate datetime,
		DebitLedgerAccount nvarchar(40),
		DebitDetailsAccount nvarchar(40),
		CreditLedgerAccount nvarchar(40),
		CreditDetailsAccount nvarchar(40),
		R_PerName nvarchar(100),
		DebitLedgerAccountName nvarchar(40),
		CreditLedgerAccountName nvarchar(40),
		Currency NVARCHAR(5),
		B_GUID NVARCHAR(40),
		BA_GUID NVARCHAR(40),
		BankAccount NVARCHAR(100),
		A_GUID NVARCHAR(50)
	 )
	 
    -- Insert statements for procedure here
		insert into @temp
			SELECT row_number()over(order by PR.Date desc) rownumber,PR.RP_GUID,PR.IE_GUID,PR.InvType,PR.InvTypeDts,PR.InvNo,PR.RPer,PR.SumAmount,PR.Date,PR.Remark,PR.Creator,PR.CreateDate
			,PR.DebitLedgerAccount,PR.DebitDetailsAccount,PR.CreditLedgerAccount,PR.CreditDetailsAccount,BP.Name R_PerName,
			LA2.Name AS DebitLedgerAccountName,LA1.Name AS CreditLedgerAccountName,
			PR.Currency,PR.B_GUID,PR.BA_GUID,BA.Account AS BankAccount,TA.A_GUID
			FROM dbo.T_RecPayRecord PR
			LEFT JOIN dbo.T_BusinessPartner BP ON PR.RPer = BP.BP_GUID
			LEFT JOIN dbo.T_GeneralLedgerAccount LA1 ON LA1.LA_GUID = PR.CreditLedgerAccount
			LEFT JOIN dbo.T_GeneralLedgerAccount LA2 ON LA2.LA_GUID = PR.DebitLedgerAccount
			LEFT JOIN dbo.T_BankAccount BA ON BA.BA_GUID = PR.BA_GUID
			LEFT JOIN dbo.T_Attachment TA ON TA.FR_GUID = PR.RP_GUID
			WHERE PR.RP_GUID NOT IN(SELECT PR1.RP_GUID FROM T_RecPayRecord PR1 
										WHERE PR1.RP_GUID IN(select A.IE_GUID FROM  T_RecPayRecord A WHERE A.RP_Flag = 'R' AND A.InvTypeDts = '收到的其他与投资活动有关的款'))
			AND PR.C_GUID=@C_GUID
			AND (PR.RP_Flag = @Flag or @Flag IS NULL OR LEN(@Flag) = 0)
			AND(PR.Date >= @dateBegin OR @dateBegin IS NULL)
			AND(PR.Date < DATEADD(day,1,@dateEnd) OR @dateEnd IS NULL)
			AND(PR.RPer = @customer OR @customer IS NULL OR LEN(@customer) = 0)
			AND(PR.InvType = @incomeGrp OR @incomeGrp IS NULL OR LEN(@incomeGrp)=0)
			AND(PR.InvTypeDts = @incomeGrpdts OR @incomeGrpdts IS NULL OR LEN(@incomeGrpdts)=0)
			
		SELECT @Count = COUNT(*) FROM @temp;
		SELECT T.RP_GUID,T.IE_GUID,T.InvType,T.InvTypeDts,T.InvNo,T.RPer,T.SumAmount,T.Date,T.Remark,T.Creator,T.CreateDate
			,T.DebitLedgerAccount,T.DebitDetailsAccount,T.CreditLedgerAccount,T.CreditDetailsAccount,T.R_PerName,T.DebitLedgerAccountName,T.CreditLedgerAccountName
			,Currency ,BA_GUID ,BankAccount,T.A_GUID 
		FROM @temp T
		WHERE (T.rownumber>= ((@PageIndex - 1)*@PageSize) + 1
		AND T.rownumber <= @PageIndex*@PageSize )
		OR (@PageSize = -1)
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetReceivablesSelfList]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,chenxiang>
-- Create date: <Create Date,,07/16/2015>
-- Description:	获取收付款记录
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetReceivablesSelfList]
	@PageSize int = -1,
	@PageIndex int = 1,
	@Count int = 0 out,
	@ID NVARCHAR(40) = NULL,
	@Flag NVARCHAR(1) = NULL,
	@C_GUID NVARCHAR(50),
	@IsAll bit = 0,
	@ClassifyFlag bit = NULL,
	@dateBegin DATETIME = NULL,
	@dateEnd DATETIME = NULL,
	@customer NVARCHAR(40) = NULL,
	@incomeGrp NVARCHAR(20)=NULL,
	@incomeGrpdts NVARCHAR(40)=NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @temp TABLE(
		rownumber int,
		RP_GUID NVARCHAR(40),
		IE_GUID NVARCHAR(40),
		InvType nvarchar(40),
		InvTypeDts nvarchar(40),
		InvNo nvarchar(20),
		RPer nvarchar(40),
		SumAmount decimal(18,2),
		Date date,
		Remark nvarchar(200),
		Creator  nvarchar(40),
		CreateDate datetime,
		DebitLedgerAccount nvarchar(40),
		DebitDetailsAccount nvarchar(40),
		CreditLedgerAccount nvarchar(40),
		CreditDetailsAccount nvarchar(40),
		R_PerName nvarchar(100),
		DebitLedgerAccountName nvarchar(40),
		CreditLedgerAccountName nvarchar(40),
		Currency NVARCHAR(5),
		B_GUID NVARCHAR(40),
		BA_GUID NVARCHAR(40),
		BankAccount NVARCHAR(100),
		A_GUID NVARCHAR(50)
	 )
	 
    -- Insert statements for procedure here
		insert into @temp
			SELECT row_number()over(order by PR.Date desc) rownumber,PR.RP_GUID,PR.IE_GUID,PR.InvType,PR.InvTypeDts,PR.InvNo,PR.RPer,PR.SumAmount,PR.Date,PR.Remark,PR.Creator,PR.CreateDate
			,PR.DebitLedgerAccount,PR.DebitDetailsAccount,PR.CreditLedgerAccount,PR.CreditDetailsAccount,BP.Name R_PerName,
			LA2.Name AS DebitLedgerAccountName,LA1.Name AS CreditLedgerAccountName,
			PR.Currency,PR.B_GUID,PR.BA_GUID,BA.Account AS BankAccount,TA.A_GUID
			FROM dbo.T_RecPayRecord PR
			LEFT JOIN dbo.T_BusinessPartner BP ON PR.RPer = BP.BP_GUID
			LEFT JOIN dbo.T_GeneralLedgerAccount LA1 ON LA1.LA_GUID = PR.CreditLedgerAccount
			LEFT JOIN dbo.T_GeneralLedgerAccount LA2 ON LA2.LA_GUID = PR.DebitLedgerAccount
			LEFT JOIN dbo.T_BankAccount BA ON BA.BA_GUID = PR.BA_GUID
			LEFT JOIN dbo.T_Attachment TA ON TA.FR_GUID = PR.RP_GUID
			WHERE PR.RP_GUID NOT IN(select IE_GUID FROM  T_RecPayRecord A WHERE A.RP_Flag = 'P' AND A.InvTypeDts = '偿还债务所支付的款')
			AND PR.C_GUID=@C_GUID
			AND (PR.RP_Flag = @Flag or @Flag IS NULL OR LEN(@Flag) = 0)
			AND(PR.Date >= @dateBegin OR @dateBegin IS NULL)
			AND(PR.Date < DATEADD(day,1,@dateEnd) OR @dateEnd IS NULL)
			AND(PR.RPer = @customer OR @customer IS NULL OR LEN(@customer) = 0)
			AND(PR.InvType = @incomeGrp OR @incomeGrp IS NULL OR LEN(@incomeGrp)=0)
			AND(PR.InvTypeDts = @incomeGrpdts OR @incomeGrpdts IS NULL OR LEN(@incomeGrpdts)=0)
			
		SELECT @Count = COUNT(*) FROM @temp;
		SELECT T.RP_GUID,T.IE_GUID,T.InvType,T.InvTypeDts,T.InvNo,T.RPer,T.SumAmount,T.Date,T.Remark,T.Creator,T.CreateDate
			,T.DebitLedgerAccount,T.DebitDetailsAccount,T.CreditLedgerAccount,T.CreditDetailsAccount,T.R_PerName,T.DebitLedgerAccountName,T.CreditLedgerAccountName
			,Currency ,BA_GUID ,BankAccount,T.A_GUID 
		FROM @temp T
		WHERE (T.rownumber>= ((@PageIndex - 1)*@PageSize) + 1
		AND T.rownumber <= @PageIndex*@PageSize )
		OR (@PageSize = -1)
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetReceivablesDeclareCustomer]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<CHENXIANG>
-- Create date: <2015/09/08>
-- Description:	<查询所有应收账款>
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetReceivablesDeclareCustomer]
	-- Add the parameters for the stored procedure here
	@PageSize int = -1,
	@PageIndex int = 1,
	@Count int = 0 out,
	@C_GUID nvarchar(40),
	@dateBegin DATETIME = NULL,
	@dateEnd DATETIME = NULL,
	@customer NVARCHAR(40) = NULL,
	@state NVARCHAR(40) = NULL,
	@currency NVARCHAR(40) = NULL,
	@incomeGrp NVARCHAR(20)=NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
   
    DECLARE @temp Table(
    rownumber int,
    GUID nvarchar(40),
    InvType nvarchar(40),
    RPer nvarchar(40),
    RPerName nvarchar(40),
    Amount decimal(18,4),
    Currency nvarchar(20),
    State nvarchar(20),
    Remark nvarchar(200),
    Date datetime,
    AGUID nvarchar(40)
    )
        
    insert into @temp
	select row_number()over(order by DC.Date desc) rownumber,DC.GUID,DC.InvType,DC.RPer,BP.Name RPerName,DC.Amount,DC.Currency,DC.State,DC.Remark,DC.Date,TA.A_GUID AGUID
	from dbo.T_DeclareCustomer DC
	LEFT JOIN dbo.T_BusinessPartner BP ON BP.BP_GUID = DC.RPer
	LEFT JOIN dbo.T_Attachment TA ON TA.FR_GUID = DC.GUID
	where DC.C_GUID=@C_GUID 
	AND(DC.Date >= @dateBegin OR @dateBegin IS NULL)
	AND(DC.Date < DATEADD(day,1,@dateEnd) OR @dateEnd IS NULL)
	AND(DC.RPer = @customer OR @customer IS NULL OR LEN(@customer) = 0)
	AND (DC.State=@state OR @state IS NULL OR LEN(@state) = 0)
	AND (DC.Currency=@currency OR @currency IS NULL OR LEN(@currency) = 0)
	AND (DC.InvType = @incomeGrp OR @incomeGrp IS NULL OR LEN(@incomeGrp)=0) 
	
	SELECT @Count = COUNT(*) FROM @temp;
	
	SELECT T.GUID,T.InvType,T.RPer,T.RPerName,T.Amount,T.Currency,T.State,T.Remark,T.Date,T.AGUID
		FROM @temp T
		WHERE (T.rownumber>= ((@PageIndex - 1)*@PageSize) + 1
		AND T.rownumber <= @PageIndex*@PageSize )
		OR (@PageSize = -1)
	
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetRateHistory]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<CHENXIANG>
-- Create date: <2015/09/08>
-- Description:	<查询所有应收账款>
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetRateHistory]
	-- Add the parameters for the stored procedure here
	@PageSize int = -1,
	@PageIndex int = 1,
	@Count int = 0 out,
	@C_GUID nvarchar(40),
	@DateBegin datetime=null,
	@DateEnd datetime=null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
   
    DECLARE @temp Table(
    rownumber int,
	GUID	nvarchar(40),
	Date	datetime,
	FAmount	decimal(18, 4),
	FCurrency	nvarchar(50),
	TAmount	decimal(18, 4),
	TCurrency	nvarchar(50)
	
    )
        
    insert into @temp
	select row_number()over(order by T.Date desc) rownumber,T.GUID,T.Date,T.FAmount,T.FCurrency,T.TAmount,T.TCurrency
	from dbo.T_RateHistory T
	where T.C_GUID=@C_GUID 
	AND(T.Date >= @DateBegin OR @DateBegin IS NULL)
	AND(T.Date < DATEADD(day,1,@DateEnd) OR @DateEnd IS NULL)
	
	SELECT @Count = COUNT(*) FROM @temp;
	
	SELECT t.GUID,t.Date,t.FAmount,t.FCurrency,t.TAmount,t.TCurrency
		FROM @temp t
		WHERE (t.rownumber>= ((@PageIndex - 1)*@PageSize) + 1
		AND t.rownumber <= @PageIndex*@PageSize )
		OR (@PageSize = -1)
	
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetPaymentDeclareCostSpendingList]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,chenxiang>
-- Create date: <Create Date,,07/16/2015>
-- Description:	获取收付款记录
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetPaymentDeclareCostSpendingList]
	@PageSize int = -1,
	@PageIndex int = 1,
	@Count int = 0 out,
	@Flag NVARCHAR(1) = NULL,
	@C_GUID NVARCHAR(50),
	@InvTypeDts NVARCHAR(50)=NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @temp TABLE(
		rownumber int,
		RP_GUID NVARCHAR(40),
		InvType nvarchar(20),
		RPer nvarchar(40),
		SumAmount decimal(18,2),
		Date date,
		Remark nvarchar(200),
		R_PerName nvarchar(100),
		Currency NVARCHAR(5),
		B_GUID NVARCHAR(40),
		BA_GUID NVARCHAR(40),
		BankAccount NVARCHAR(100),
		A_GUID NVARCHAR(50)
	 )
	 
    -- Insert statements for procedure here
    
		insert into @temp
		SELECT row_number()over(order by PR.Date desc) rownumber,PR.RP_GUID,PR.InvType,PR.RPer,PR.SumAmount,PR.Date,PR.Remark,BP.Name R_PerName,
		PR.Currency,PR.B_GUID,PR.BA_GUID,BA.Account AS BankAccount,TA.A_GUID
		FROM dbo.T_RecPayRecord PR
		LEFT JOIN dbo.T_BusinessPartner BP ON PR.RPer = BP.BP_GUID
		LEFT JOIN dbo.T_BankAccount BA ON BA.BA_GUID = PR.BA_GUID
		LEFT JOIN dbo.T_Attachment TA ON TA.FR_GUID = PR.RP_GUID
		WHERE PR.C_GUID=@C_GUID
		AND (PR.RP_Flag = @Flag or @Flag IS NULL OR LEN(@Flag) = 0)
		AND(PR.InvTypeDts = @InvTypeDts OR @InvTypeDts IS NULL OR LEN(@InvTypeDts)=0)
		AND PR.RP_GUID NOT IN(SELECT C.RP_GUID FROM T_RecPayRecord C INNER JOIN dbo.T_IERecord TIE ON TIE.IE_GUID = PR.RP_GUID)
		AND PR.RP_GUID IN(SELECT D.RP_GUID FROM T_RecPayRecord D INNER JOIN dbo.T_DeclareCostSpending TDCS ON TDCS.GUID = D.IE_GUID)
			
		SELECT @Count = COUNT(*) FROM @temp;
		SELECT T.RP_GUID,T.InvType,T.RPer,T.SumAmount,T.Date,T.Remark,T.R_PerName,Currency ,BA_GUID ,BankAccount,T.A_GUID 
		FROM @temp T
		WHERE (T.rownumber>= ((@PageIndex - 1)*@PageSize) + 1
		AND T.rownumber <= @PageIndex*@PageSize )
		OR (@PageSize = -1)
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetPaymentDeclareCostSpendingAll]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<CHENXIANG>
-- Create date: <2015/09/08>
-- Description:	<查询所有应收账款>
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetPaymentDeclareCostSpendingAll]
	-- Add the parameters for the stored procedure here
	@PageSize int = -1,
	@PageIndex int = 1,
	@dateBegin DATETIME = NULL,
	@dateEnd DATETIME = NULL,
	@PaymentGrp nvarchar(40)= NULL,
	@Count int = 0 out,
	@C_GUID nvarchar(40),
	@State nvarchar(40)=null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
   
    DECLARE @temp Table(
    rownumber int,
    GUID nvarchar(40),
    InvType nvarchar(40),
    RPer nvarchar(40),
    RPerName nvarchar(40),
    Amount decimal(18,4),
    Currency nvarchar(20),
    State nvarchar(20),
    Remark nvarchar(200),
    Date datetime,
    AGUID nvarchar(40)
    )
        
    insert into @temp
	select row_number()over(order by DC.Date desc) rownumber,DC.GUID,DC.InvType,DC.RPer,BP.Name RPerName,DC.Amount,DC.Currency,DC.State,DC.Remark,DC.Date,TA.A_GUID AGUID
	from dbo.T_DeclareCostSpending DC
	LEFT JOIN dbo.T_BusinessPartner BP ON BP.BP_GUID = DC.RPer
	LEFT JOIN dbo.T_Attachment TA ON TA.FR_GUID = DC.GUID
	where DC.C_GUID=@C_GUID 
	AND(DC.Date >= @dateBegin OR @dateBegin IS NULL)
	AND(DC.Date < DATEADD(day,1,@dateEnd) OR @dateEnd IS NULL)
	AND (DC.InvType=@PaymentGrp OR @PaymentGrp IS NULL OR LEN(@PaymentGrp) = 0)
	AND (DC.State=@State OR @State IS NULL OR LEN(@State) = 0)
	
	SELECT @Count = COUNT(*) FROM @temp;
	
	SELECT T.GUID,T.InvType,T.RPer,T.RPerName,T.Amount,T.Currency,T.State,T.Remark,T.Date,T.AGUID
		FROM @temp T
		WHERE (T.rownumber>= ((@PageIndex - 1)*@PageSize) + 1
		AND T.rownumber <= @PageIndex*@PageSize )
		OR (@PageSize = -1)
	
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetPaymentDeclareCostSpending]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<CHENXIANG>
-- Create date: <2015/09/08>
-- Description:	<查询所有应收账款>
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetPaymentDeclareCostSpending]
	-- Add the parameters for the stored procedure here
	@PageSize int = -1,
	@PageIndex int = 1,
	@Count int = 0 out,
	@C_GUID nvarchar(40),
	@State nvarchar(40)=null,
	@InvType nvarchar(40)=null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
   
    DECLARE @temp Table(
    rownumber int,
    GUID nvarchar(40),
    InvType nvarchar(40),
    RPer nvarchar(40),
    RPerName nvarchar(40),
    Amount decimal(18,4),
    Currency nvarchar(20),
    State nvarchar(20),
    Remark nvarchar(200),
    Date datetime,
    AGUID nvarchar(40)
    )
        
    insert into @temp
	select row_number()over(order by DC.Date desc) rownumber,DC.GUID,DC.InvType,DC.RPer,BP.Name RPerName,DC.Amount,DC.Currency,DC.State,DC.Remark,DC.Date,TA.A_GUID AGUID
	from dbo.T_DeclareCostSpending DC
	LEFT JOIN dbo.T_BusinessPartner BP ON BP.BP_GUID = DC.RPer
	LEFT JOIN dbo.T_Attachment TA ON TA.FR_GUID = DC.GUID
	where DC.C_GUID=@C_GUID 
	AND (DC.State=@State OR @State IS NULL OR LEN(@State) = 0)
	AND (DC.InvType=@InvType OR @InvType IS NULL OR LEN(@InvType) = 0)
	
	SELECT @Count = COUNT(*) FROM @temp;
	
	SELECT T.GUID,T.InvType,T.RPer,T.RPerName,T.Amount,T.Currency,T.State,T.Remark,T.Date,T.AGUID
		FROM @temp T
		WHERE (T.rownumber>= ((@PageIndex - 1)*@PageSize) + 1
		AND T.rownumber <= @PageIndex*@PageSize )
		OR (@PageSize = -1)
	
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetPayableRecord]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<CHENXIANG>
-- Create date: <2015/05/08>
-- Description:	<查询所有应付账款>
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetPayableRecord]
	-- Add the parameters for the stored procedure here
	@PageSize int = -1,
	@PageIndex int = 1,
	@Count int = 0 out,
	@RPer nvarchar(40)=null,
	@IEGroup nvarchar(40)=null,
	@C_GUID nvarchar(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	

    -- Insert statements for procedure here
      DECLARE @temp Table(
    rownumber int,
    R_GUID nvarchar(40),
    AffirmDate datetime,
    Payer nvarchar(40),
    InvNo nvarchar(20),
    InvType NVARCHAR(50),
    Money decimal(18,2),
    Currency nvarchar(5),
    Date datetime,
    Remark nvarchar(200),
    RPerName nvarchar(50),
    IEGroupName nvarchar(50)
    )
	
	insert into @temp
	select row_number()over(order by P.Date desc) rownumber,P.R_GUID,IE.AffirmDate,P.Payee,P.InvNo,P.InvType,IE.SumAmount Money,P.Currency,P.Date,IE.Remark,BP.Name RPerName,DC.Name IEGroupName
	from dbo.T_Payable P
	LEFT JOIN dbo.T_BusinessPartner BP ON BP.BP_GUID = P.Payee
	LEFT JOIN dbo.T_IERecord IE ON IE.IE_GUID = P.R_GUID
	LEFT JOIN dbo.T_DetailedCategories DC ON DC.GUID = IE.IEGroup
	where P.C_GUID=@C_GUID 
	and IE.State='应付' 
	and IE.InvType='营业成本' 
	and (IE.IEGroup=@IEGroup or @IEGroup is null or LEN(@IEGroup)=0) 
	and (IE.IEGroup<>'1544d862-b1ab-42b8-9e97-9c2e1704665c')
	
	SELECT @Count = COUNT(*) FROM @temp;
	SELECT T.R_GUID,T.AffirmDate,T.Currency,T.Date,T.InvNo,T.InvType,T.Money,T.Payer,T.Remark,T.RPerName,T.IEGroupName
		FROM @temp T
		WHERE (T.rownumber>= ((@PageIndex - 1)*@PageSize) + 1
		AND T.rownumber <= @PageIndex*@PageSize )
		OR (@PageSize = -1)	
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetPartners]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	获取合作伙伴
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetPartners]
@C_GUID NVARCHAR(50),
@ID NVARCHAR(40)=NULL,
@BPName NVARCHAR(100)=NULL
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
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetLedgerAccounts]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	获取总账科目
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetLedgerAccounts]
@id Nvarchar(40) = null,
@stat bit = null,
@c_id nvarchar(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT LA.LA_GUID,AccGroup,AccCode,LA.Name , 
	(CASE WHEN R_CA.R_GUID IS NULL THEN 0 ELSE 1 END) AS Useable,
	(CASE WHEN DA.ParentAccGuid IS NULL THEN 0 ELSE 1 END) AS IsLocked
	FROM dbo.T_GeneralLedgerAccount LA
	LEFT JOIN dbo.R_CompanyAccount R_CA ON R_CA.LA_GUID = LA.LA_GUID AND R_CA.C_GUID = @c_id
	LEFT JOIN (SELECT DISTINCT ParentAccGuid FROM dbo.T_DetailedAccount )DA ON LA.LA_GUID = DA.ParentAccGuid
	
	WHERE(LA.LA_GUID = @id OR @id IS NULL OR LEN(@ID) = 0)
	AND ((CASE WHEN R_CA.R_GUID IS NULL THEN 0 ELSE 1 END) = @stat OR @stat IS NULL )
	ORDER BY LA.AccGroup,LA.AccCode;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetInvType]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	获取单据类型
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetInvType]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT [Key],Name
	FROM dbo.T_InvType 
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetInitialBalanceRecord]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	获取合作伙伴
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetInitialBalanceRecord]
@C_GUID NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Inital_GUID,C_GUID,MonetaryFunds,ShorttermInvestments,NotesReceivable,SubsidiesReceivable,DividendReceivable,Inventories,LongtermDebtOneYear,OtherCcurrentAssets,LongtermInvestments,
FixedAssetsNBV,ConstructionInProgress,IntangibleAssets,Deferred,ShorttermLoans,NotesPayable,AccountsPayable,AdvancesFromCustomers,AccruedPayroll,TaxesPayable,LongtermLiabiltiesDueWithinaYear,
OtherCurrentLiabilities,LongtermBorrowings,LongtermPayables,OtherLongtermLliabilities,Currency,Flag,Date,BankAccount1,BankAccount1Money,BankAccount2,BankAccount2Money
	FROM dbo.T_Balance
	WHERE C_GUID=@C_GUID
	ORDER BY Date DESC 
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetIEWriteOffList]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<魏礼明>
-- Create date: <2015/05/07>
-- Description:	<查询销账记录>
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetIEWriteOffList]
	-- Add the parameters for the stored procedure here
	@PageSize int = -1,
	@PageIndex int = 1,
	@Count int = 0 out,
	@C_GUID nvarchar(40),
	@Flag nvarchar(4)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @temp Table(
    rownumber int,
    R_GUID nvarchar(40),
	C_GUID nvarchar(40),
	IE_Flag nvarchar(4),
	DebitLedgerAccount nvarchar(40),
	DebitLedgerAccountName nvarchar(40),
	DebitDetailsAccount nvarchar(40),
	DebitDetailsAccountName nvarchar(40),
	CreditLedgerAccount nvarchar(40),
	CreditLedgerAccountName nvarchar(40),
	CreditDetailsAccount nvarchar(40),
	CreditDetailsAccountName nvarchar(40),
	Amount decimal(18,4),
	Date datetime,
	Remark nvarchar(200),
	Creator nvarchar(40),
	CreateDate datetime
    )
     insert into @temp
     select row_number()over(order by TWO.Date desc) rownumber,TWO.R_GUID,TWO.C_GUID,TWO.IE_Flag,TWO.DebitLedgerAccount,LA2.Name,TWO.DebitDetailsAccount,DA2.Name,TWO.CreditLedgerAccount,LA1.Name,TWO.CreditDetailsAccount,DA1.Name,TWO.Amount,TWO.Date,TWO.Remark,TWO.Creator,TWO.CreateDate
	from dbo.T_IEWriteOff TWO
		LEFT JOIN dbo.T_GeneralLedgerAccount LA1 ON LA1.LA_GUID = TWO.CreditLedgerAccount
		LEFT JOIN dbo.T_GeneralLedgerAccount LA2 ON LA2.LA_GUID = TWO.DebitLedgerAccount
		LEFT JOIN dbo.T_DetailedAccount DA1 ON DA1.DA_GUID = TWO.CreditDetailsAccount
		LEFT JOIN dbo.T_DetailedAccount DA2 ON DA2.DA_GUID = TWO.DebitDetailsAccount
	where TWO.C_GUID=@C_GUID and TWO.IE_Flag=@Flag
	
	SELECT @Count = COUNT(*) FROM @temp;
	SELECT T.R_GUID,T.C_GUID,T.IE_Flag,T.DebitLedgerAccount ,T.DebitDetailsAccount,T.CreditLedgerAccount,
		T.CreditDetailsAccount,T.Amount,T.Date,T.Remark,T.Creator,T.CreateDate,T.CreditDetailsAccountName,
		T.CreditLedgerAccountName,T.DebitDetailsAccountName,T.DebitLedgerAccountName
		FROM @temp T
		WHERE (T.rownumber>= ((@PageIndex - 1)*@PageSize) + 1
		AND T.rownumber <= @PageIndex*@PageSize )
		OR (@PageSize = -1)
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdAttachment]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdAttachment] 
	-- Add the parameters for the stored procedure here
	@A_GUID NVARCHAR(40),
	@FileName NVARCHAR(200),
	@FileRemark NVARCHAR(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE T_Attachment
	SET  FileName=@FileName, FileRemark=@FileRemark
	WHERE A_GUID=@A_GUID;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdAssetsStat]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	更新固定资产状态
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdAssetsStat]
	@ID NVARCHAR(40),
	@Stat NVARCHAR(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	
    BEGIN TRAN;
    
    DECLARE @SCRAPDATE DATETIME;
    SELECT @SCRAPDATE = ScrapDate FROM dbo.T_Assets WHERE A_GUID = @ID;
    
    IF (@SCRAPDATE IS NULL)
    BEGIN
		UPDATE dbo.T_Assets SET ScrapType = @Stat,ScrapDate = SYSDATETIME() WHERE A_GUID = @ID;
    END
    ELSE 
    BEGIN
		UPDATE dbo.T_Assets SET ScrapType = @Stat WHERE A_GUID = @ID;
    END
    COMMIT TRAN;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdAssetsGroup]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	更新固定资产分类
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdAssetsGroup]
	@guid NVARCHAR(50),
	@NAME NVARCHAR(50),
	@METHOD INT,
	@LIFE INT,
	@SalvageRate DECIMAL,
	@C_GUID NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	
    BEGIN TRAN;
    DELETE FROM dbo.T_AssetsGroup WHERE AG_GUID = @guid;
	INSERT INTO dbo.T_AssetsGroup
	VALUES(@guid,@NAME,@METHOD,@LIFE,@SalvageRate,@C_GUID);
	COMMIT TRAN;

END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdAssets]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	更新固定资产信息
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdAssets]
	-- Add the parameters for the stored procedure here
	@ID NVARCHAR(50),
	@No NVARCHAR(100),
	@NAME NVARCHAR(100),
	@AG_Guid NVARCHAR(50),
	@RegDate DATETIME,
	@PurchaseDate DATETIME,
	@AssetsCost DECIMAL(18,4),
	@Creator NVARCHAR(50),
	@C_GUID NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	
	BEGIN TRAN;
	DELETE FROM dbo.T_Assets WHERE A_GUID = @ID;
    -- Insert statements for procedure here
	INSERT INTO dbo.T_Assets(A_GUID,No,Name,PurchaseDate,RegisterDate,AG_Guid,AssetsCost,Creator,C_GUID)
	VALUES (@ID,@No,@NAME,@PurchaseDate,@RegDate,@AG_Guid,@AssetsCost,@Creator,@C_GUID);
	COMMIT TRAN;

END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdAIDState]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<chenxiang>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdAIDState]
	-- Add the parameters for the stored procedure here
	@Flag nvarchar(1),
	@ID nvarchar(40),
	@State nvarchar(40)
	 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE T_AIDRecord
	SET State=@State
	WHERE AID_Flag=@Flag AND GUID=@ID; 
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdAIDRecord]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	更新收入费用记录
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdAIDRecord]
	@GUID NVARCHAR(40),
	@C_GUID NVARCHAR(40),
	@AID_Flag NVARCHAR(1),
	@InvType NVARCHAR(40)=null,
	@RPer NVARCHAR(40)=null,
	@Date DATETIME=NULL,
	@Amount DECIMAL=null,
	@Remark NVARCHAR(200)=null,
	@Currency NVARCHAR(40)=null,
	@Description NVARCHAR(100)=null,
	@DepreciationPeriod INT=0,
	@SurplusValue DECIMAL=null,
	@State NVARCHAR(40)=NULL,
	@CostType  VARCHAR(1)=NULL
	
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

    -- Insert statements for procedure here
    BEGIN TRAN;
	DELETE dbo.T_AIDRecord WHERE GUID = @GUID;
	
	INSERT INTO dbo.T_AIDRecord(GUID,C_GUID,AID_Flag,InvType,RPer,Date,Amount,Remark,Currency,Description,DepreciationPeriod,SurplusValue,State,CostType)
	VALUES(@GUID,@C_GUID,@AID_Flag,@InvType,@RPer,@Date,@Amount,@Remark,@Currency,@Description,@DepreciationPeriod,@SurplusValue,@State,@CostType);
	COMMIT TRAN;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetUsers]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	获取用户信息
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetUsers]
	@LoginName NVARCHAR(40),
	@Pwd NVARCHAR(40)=null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT *
	FROM T_User U
	WHERE U.LoginName = @LoginName 
	AND (U.Password = @Pwd OR @Pwd IS NULL OR LEN(@pwd)=0)
	AND (U.State = 1);
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetUserList]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<魏礼明>
-- Create date: <2015/04/09>
-- Description:	<获取用户列表信息>
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetUserList] 
	-- Add the parameters for the stored procedure here
	@C_GUID NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM dbo.T_User
	WHERE C_GUID=@C_GUID
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetUserLedgerAccounts]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	获取用户总账科目
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetUserLedgerAccounts]
	@c_id nvarchar(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT LA.LA_GUID,AccGroup,AccCode,LA.Name --, 
	--(CASE WHEN R_CA.R_GUID IS NULL THEN 0 ELSE 1 END) AS Useable,
	--(CASE WHEN DA.ParentAccGuid IS NULL THEN 0 ELSE 1 END) AS IsLocked
	FROM dbo.T_GeneralLedgerAccount LA
	INNER JOIN dbo.R_CompanyAccount R_CA ON R_CA.LA_GUID = LA.LA_GUID AND R_CA.C_GUID = @c_id
	--LEFT JOIN (SELECT DISTINCT ParentAccGuid FROM dbo.T_DetailedAccount )DA ON LA.LA_GUID = DA.ParentAccGuid
	ORDER BY LA.AccGroup,LA.AccCode;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetUserInfo]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<魏礼明>
-- Create date: <2015/04/09>
-- Description:	<获取用户信息>
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetUserInfo]
	-- Add the parameters for the stored procedure here
	@U_GUID NVARCHAR(50)=NULL,
	@Name NVARCHAR(50)=NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM dbo.T_User
	WHERE (U_GUID=@U_GUID OR @U_GUID IS NULL OR LEN(@U_GUID)=0) AND (LoginName=@Name OR @Name=NULL OR LEN(@Name)=0)
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetUserCurrency]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	获取用户币制
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetUserCurrency]
	@C_ID NVARCHAR(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT Code
    FROM R_CompanyCurrceny 
    WHERE C_GUID = @C_ID;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetTreeMenuItem]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	获取系统菜单
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetTreeMenuItem]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT GUID,ChineseName,EnglishName,OrderNumber,ModuleID,IsLastChild,URL,SubfunctionCode,
	[Level],ModuleID,ModuleState,Block
	FROM  dbo.T_ModuleList
	WHERE IsShowTree = 1 AND ModuleState <> 0;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetTaxNew]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<CHENXIANG>
-- Create date: <2015/09/08>
-- Description:	<查询所有应收账款>
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetTaxNew]
	-- Add the parameters for the stored procedure here
	@PageSize int = -1,
	@PageIndex int = 1,
	@Count int = 0 out,
	@C_GUID nvarchar(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
   
    DECLARE @temp Table(
    rownumber int,
    T_GUID nvarchar(40),
    Name nvarchar(40),
    Rate decimal ,
    Type nvarchar(40)
    )
        
    insert into @temp
	select row_number()over(order by T.Name desc) rownumber,T.T_GUID,T.Name,T.Rate,T.Type
	from dbo.T_Tax T
	where T.C_GUID=@C_GUID 
	
	SELECT @Count = COUNT(*) FROM @temp;
	
	SELECT t.T_GUID,t.Name,t.Rate,t.Type
		FROM @temp t
		WHERE (t.rownumber>= ((@PageIndex - 1)*@PageSize) + 1
		AND t.rownumber <= @PageIndex*@PageSize )
		OR (@PageSize = -1)
	
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetTax]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	获取税费类型
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetTax]
	@C_GUID NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM dbo.T_Tax
	WHERE C_GUID=@C_GUID
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetReports]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	获取报表列表
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetReports]
	@C_ID NVARCHAR(40),
	@Type NVARCHAR(5)= NULL,
	@PageSize INT = 10,
	@PageIndex INT = 0,
	@Count INT = 0 OUT 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @temp TABLE(
		rownumber int,
		Rep_GUID NVARCHAR(40),
		RepNo NVARCHAR(40),
		Year INT,
		Month INT
	)
	
	INSERT INTO @temp
		SELECT row_number()over(order by rep.RepNo DESC) rownumber,
		rep.Rep_GUID,rep.RepNo,rep.Year,rep.Month
		FROM dbo.T_Report rep
		WHERE rep.C_GUID = @C_ID AND (rep.Type = @Type OR @Type IS NULL);
	
	SELECT @Count = COUNT(t.Rep_GUID)
	FROM @temp t
	
    SELECT Rep_GUID,t.RepNo,t.Year,t.Month
    FROM @temp t
    WHERE (T.rownumber>= ((@PageIndex - 1)*@PageSize) + 1
	AND T.rownumber <= @PageIndex*@PageSize)
	OR (@PageIndex = 0);
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetReportDetails]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	获取报表明细
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetReportDetails]
	@RepID NVARCHAR(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	

    SELECT *
    FROM dbo.T_ReportDetails 
    WHERE Rep_GUID = @RepID
    
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetReport]    Script Date: 06/02/2016 14:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	获取报表
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetReport]
	@RepID NVARCHAR(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT *
    FROM dbo.T_Report Rep
    WHERE Rep.Rep_GUID = @RepID;
END
GO
/****** Object:  View [dbo].[V_Receivable]    Script Date: 06/02/2016 14:21:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[V_Receivable]
AS
SELECT     Rec.R_GUID, IER.AffirmDate, Rec.Payer, Rec.InvNo, IER.InvType, Rec.Money AS SumAmount, IER.Currency, IER.Date, IER.Remark, Rec.C_GUID, 
                      BP.Name AS RPerName, IER.State
FROM         dbo.T_Receivables AS Rec LEFT OUTER JOIN
                          (SELECT     RPable, SUM(SumAmount) AS Money
                            FROM          dbo.T_RecPayRecord
                            GROUP BY RPable) AS RP ON Rec.R_GUID = RP.RPable LEFT OUTER JOIN
                      dbo.T_IEWriteOff AS WOR ON WOR.R_GUID = Rec.R_GUID LEFT OUTER JOIN
                      dbo.T_BusinessPartner AS BP ON BP.BP_GUID = Rec.Payer LEFT OUTER JOIN
                      dbo.T_IERecord AS IER ON IER.IE_GUID = Rec.R_GUID
WHERE     (Rec.Money - ISNULL(RP.Money, 0) - ISNULL(WOR.Amount, 0) > 0)
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = -166
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Rec"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 125
               Right = 180
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "RP"
            Begin Extent = 
               Top = 6
               Left = 218
               Bottom = 95
               Right = 360
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "WOR"
            Begin Extent = 
               Top = 96
               Left = 218
               Bottom = 215
               Right = 408
            End
            DisplayFlags = 280
            TopColumn = 9
         End
         Begin Table = "BP"
            Begin Extent = 
               Top = 126
               Left = 38
               Bottom = 245
               Right = 182
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "IER"
            Begin Extent = 
               Top = 216
               Left = 220
               Bottom = 335
               Right = 388
            End
            DisplayFlags = 280
            TopColumn = 18
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'V_Receivable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'V_Receivable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'V_Receivable'
GO
/****** Object:  View [dbo].[V_Payable]    Script Date: 06/02/2016 14:21:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[V_Payable]
AS
SELECT     Pay.R_GUID, IER.AffirmDate, Pay.Payee AS Payer, Pay.InvNo, IER.InvType, Pay.Money AS SumAmount, IER.Currency, IER.Date, IER.Remark, Pay.C_GUID, 
                      BP.Name AS RPerName, IER.State, IER.IEGroup
FROM         dbo.T_Payable AS Pay LEFT OUTER JOIN
                          (SELECT     RPable, SUM(SumAmount) AS Money
                            FROM          dbo.T_RecPayRecord
                            GROUP BY RPable) AS RP ON Pay.R_GUID = RP.RPable LEFT OUTER JOIN
                      dbo.T_IEWriteOff AS WOR ON WOR.R_GUID = Pay.R_GUID LEFT OUTER JOIN
                      dbo.T_BusinessPartner AS BP ON BP.BP_GUID = Pay.Payee LEFT OUTER JOIN
                      dbo.T_IERecord AS IER ON IER.IE_GUID = Pay.R_GUID
WHERE     (Pay.Money - ISNULL(RP.Money, 0) - ISNULL(WOR.Amount, 0) > 0)
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Pay"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 125
               Right = 180
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "RP"
            Begin Extent = 
               Top = 6
               Left = 218
               Bottom = 95
               Right = 360
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "WOR"
            Begin Extent = 
               Top = 96
               Left = 218
               Bottom = 215
               Right = 557
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "BP"
            Begin Extent = 
               Top = 126
               Left = 38
               Bottom = 410
               Right = 182
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "IER"
            Begin Extent = 
               Top = 216
               Left = 220
               Bottom = 335
               Right = 388
            End
            DisplayFlags = 280
            TopColumn = 18
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
E' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'V_Payable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'nd
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'V_Payable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'V_Payable'
GO
/****** Object:  View [dbo].[V_IERecords]    Script Date: 06/02/2016 14:21:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[V_IERecords]
AS
SELECT     dbo.T_IERecord.RPer, dbo.T_IERecord.IE_GUID, dbo.T_IERecord.IE_Flag, dbo.T_IERecord.InvType, dbo.T_IERecord.InvNo, dbo.T_IERecord.Creator, 
                      dbo.T_IERecord.CreateDate, dbo.T_IERecord.C_GUID, dbo.T_IERecord.AffirmDate, dbo.T_IERecord.Date, dbo.T_IERecord.Amount, dbo.T_IERecord.TaxationAmount, 
                      dbo.T_IERecord.TaxationType, dbo.T_IERecord.SumAmount, dbo.T_IERecord.Remark, dbo.T_IERecord.Currency, dbo.T_IERecord.B_GUID, dbo.T_IERecord.BA_GUID, 
                      dbo.T_IERecord.IEGroup, dbo.T_IERecord.IEDescription, dbo.T_BusinessPartner.Name AS BPName, dbo.T_InvType.Name AS TypeName, dbo.T_IERecord.RP_GUID, 
                      dbo.T_IERecord.State
FROM         dbo.T_IERecord LEFT OUTER JOIN
                      dbo.T_InvType ON dbo.T_IERecord.InvType = dbo.T_InvType.[Key] LEFT OUTER JOIN
                      dbo.T_BusinessPartner ON dbo.T_IERecord.RPer = dbo.T_BusinessPartner.BP_GUID
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[46] 4[31] 2[9] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = -29
         Left = 0
      End
      Begin Tables = 
         Begin Table = "T_IERecord"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 206
               Right = 213
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T_InvType"
            Begin Extent = 
               Top = 36
               Left = 398
               Bottom = 229
               Right = 540
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T_BusinessPartner"
            Begin Extent = 
               Top = 163
               Left = 243
               Bottom = 282
               Right = 387
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'V_IERecords'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'V_IERecords'
GO
/****** Object:  View [dbo].[V_IERecord]    Script Date: 06/02/2016 14:21:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[V_IERecord]--收入费用记录
AS 
	SELECT IER.IE_GUID,IER.Amount,IER.SumAmount,IER.CreateDate,IER.Creator,IER.Date,IER.AffirmDate, IER.Currency,IER.InvNo,IER.InvType,Inv.Name AS TypeName,IER.RPer,IER.TaxationAmount
	,BP.Name RPerName,IER.IE_Flag,IER.C_GUID,IER.Remark
	FROM dbo.T_IERecord IER
	LEFT JOIN dbo.T_BusinessPartner BP ON BP.BP_GUID = IER.RPer
	LEFT JOIN dbo.T_InvType Inv ON Inv.[Key] = IER.InvType
	UNION ALL
	SELECT IEHR.IE_GUID,IEHR.Amount,IEHR.SumAmount,IEHR.CreateDate,IEHR.Creator,IEHR.Date,IEHR.AffirmDate,IEHR.Currency,IEHR.InvNo,Null AS InvType,IEHR.InvType AS TypeName,'',0 as TaxationAmount
	,IEHR.RPer RPerName,IEHR.IE_Flag,IEHR.C_GUID,IEHR.Remark
	FROM dbo.T_IEHistoryRecord IEHR
GO
/****** Object:  View [dbo].[V_IEDetails]    Script Date: 06/02/2016 14:21:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[V_IEDetails]--收入费用明细
AS
SELECT IED.IE_GUID,IED.Money,IED.C_GUID,LA2.Name AS DebitLedgerAccount,DA2.Name AS DebitDetailsAccount,
LA1.Name AS CreditLedgerAccount,DA1.Name AS CreditDetailsAccount,IED.Currency
FROM dbo.T_IEDetails IED 
LEFT JOIN dbo.T_GeneralLedgerAccount LA1 ON LA1.LA_GUID = IED.CreditLedgerAccount
LEFT JOIN dbo.T_GeneralLedgerAccount LA2 ON LA2.LA_GUID = IED.DebitLedgerAccount
LEFT JOIN dbo.T_DetailedAccount DA1 ON DA1.DA_GUID = IED.CreditDetailsAccount
LEFT JOIN dbo.T_DetailedAccount DA2 ON DA2.DA_GUID = IED.DebitDetailsAccount
UNION
SELECT IE_GUID,Money,C_GUID,IEHD.Currency,
DebitLedgerAccount,DebitDetailsAccount,CreditLedgerAccount,CreditDetailsAccount
FROM dbo.T_IEHistoryDetails IEHD
GO
/****** Object:  View [dbo].[V_FinanceRecord]    Script Date: 06/02/2016 14:21:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[V_FinanceRecord]
AS
SELECT     TMP.DebitLedgerAccount, TMP.DebitDetailsAccount, LAD.AccGroup AS DebitAccGroup, TMP.CreditLedgerAccount, TMP.CreditDetailsAccount, 
                      LAC.AccGroup AS CreditAccGroup, TMP.Money, TMP.C_GUID, TMP.Date
FROM         (SELECT     IE.DebitLedgerAccount, IE.DebitDetailsAccount, IE.CreditLedgerAccount, IE.CreditDetailsAccount, IE.Money, IE_H.C_GUID, IE_H.Date
                       FROM          dbo.T_IEDetails AS IE LEFT OUTER JOIN
                                              dbo.T_IERecord AS IE_H ON IE.IE_GUID = IE_H.IE_GUID
                       UNION ALL
                       SELECT     DebitLedgerAccount, DebitDetailsAccount, CreditLedgerAccount, CreditDetailsAccount, SumAmount AS Money, C_GUID, Date
                       FROM         dbo.T_RecPayRecord AS RP
                       UNION ALL
                       SELECT     DebitLedgerAccount, DebitDetailsAccount, CreditLedgerAccount, CreditDetailsAccount, Amount AS Money, C_GUID, Date
                       FROM         dbo.T_IEWriteOff AS WOR) AS TMP LEFT OUTER JOIN
                      dbo.T_GeneralLedgerAccount AS LAC ON TMP.CreditLedgerAccount = LAC.LA_GUID LEFT OUTER JOIN
                      dbo.T_GeneralLedgerAccount AS LAD ON TMP.DebitLedgerAccount = LAD.LA_GUID
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[8] 2[33] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "LAC"
            Begin Extent = 
               Top = 6
               Left = 266
               Bottom = 125
               Right = 408
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "LAD"
            Begin Extent = 
               Top = 126
               Left = 38
               Bottom = 245
               Right = 180
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "TMP"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 125
               Right = 228
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'V_FinanceRecord'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'V_FinanceRecord'
GO
/****** Object:  View [dbo].[V_CompanyAccounts]    Script Date: 06/02/2016 14:21:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[V_CompanyAccounts]--公司科目
AS
	SELECT '1' AS Acc_GUID,'' AS _parentId,'资产类' AS Acc_Name,'1' AS Acc_Code,NULL AS C_GUID
    UNION
    SELECT '2' AS Acc_GUID,'' AS _parentId,'负债类' AS Acc_Name,'2' AS Acc_Code,NULL AS C_GUID
    UNION
    SELECT '4' AS Acc_GUID,'' AS _parentId,'所有者权益' AS Acc_Name,'4' AS Acc_Code,NULL AS C_GUID
    UNION
    SELECT '5' AS Acc_GUID,'' AS _parentId,'成本类' AS Acc_Name,'4' AS Acc_Code,NULL AS C_GUID
    UNION
    SELECT '6' AS Acc_GUID,'' AS _parentId,'收入类' AS Acc_Name,'4' AS Acc_Code,NULL AS C_GUID
    UNION
    SELECT '7' AS Acc_GUID,'' AS _parentId,'费用类' AS Acc_Name,'4' AS Acc_Code,NULL AS C_GUID
    UNION
	SELECT LA.LA_GUID AS Acc_GUID,CONVERT (NVARCHAR(40),LA.AccGroup) AS _parentId,
	LA.Name AS Acc_Name,LA.AccCode AS Acc_Code,R_CA.C_GUID
	FROM dbo.T_GeneralLedgerAccount LA
	INNER JOIN dbo.R_CompanyAccount R_CA ON R_CA.LA_GUID = LA.LA_GUID 
	UNION
	SELECT DA.DA_GUID AS Acc_GUID,DA.ParentAccGuid AS _parentId,
	DA.Name AS Acc_Name,DA.AccCode Acc_Code,R_CA.C_GUID
	FROM dbo.T_GeneralLedgerAccount LA
	INNER JOIN dbo.R_CompanyAccount R_CA ON R_CA.LA_GUID = LA.LA_GUID
	INNER JOIN dbo.T_DetailedAccount DA ON DA.ParentAccGuid = LA.LA_GUID AND DA.C_GUID = R_CA.C_GUID
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdIEState]    Script Date: 06/02/2016 14:21:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<chenxiang>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdIEState]
	-- Add the parameters for the stored procedure here
	@Flag nvarchar(1),
	@ID nvarchar(40),
	@State nvarchar(40)
	 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE T_IERecord
	SET State=@State
	WHERE IE_Flag=@Flag AND IE_GUID=@ID; 
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdIERecord]    Script Date: 06/02/2016 14:21:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	更新收入费用记录
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdIERecord]
	@IE_GUID NVARCHAR(40),
	@IE_Flag NVARCHAR(1),
	@InvType NVARCHAR(20)=null,
	@InvNo NVARCHAR(20)=null,
	@RPer NVARCHAR(40)=null,
	@Creator NVARCHAR(40),
	@CreateDate DATETIME,
	@C_GUID NVARCHAR(50),
	@AffirmDate DATETIME=NULL,
	@Date DATETIME=NULL,
	@Amount DECIMAL=null,
	@TaxationAmount DECIMAL=null,
	@TaxationType NVARCHAR(40)=null,
	@SumAmount DECIMAL=null,
	@Remark NVARCHAR(200)=null,
	@Currency NVARCHAR(20),
	@B_GUID NVARCHAR(40)=null,
	@BA_GUID nvarchar(40)=null,
	@IEGroup nvarchar(40)=null,
	@IEDescription nvarchar(500)=null,
	@Profit_GUID nvarchar(40)=null,
	@State nvarchar(40)
	
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

    -- Insert statements for procedure here
    BEGIN TRAN;
	DELETE dbo.T_IERecord WHERE IE_GUID = @IE_GUID;
	
	INSERT INTO dbo.T_IERecord(IE_GUID,IE_Flag,InvType,InvNo,RPer,Creator,CreateDate,C_GUID,AffirmDate,[Date],Amount,TaxationAmount,TaxationType,SumAmount,Remark,Currency,B_GUID,BA_GUID,IEGroup,IEDescription,Profit_GUID,State)
	VALUES(@IE_GUID,@IE_Flag,@InvType,@InvNo,@RPer,@Creator,@CreateDate,@C_GUID,@AffirmDate,@Date,@Amount,@TaxationAmount,@TaxationType,@SumAmount,@Remark,@Currency,@B_GUID,@BA_GUID,@IEGroup,@IEDescription,@Profit_GUID,@State);
	COMMIT TRAN;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdIEDetails]    Script Date: 06/02/2016 14:21:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	更新收入费用明细
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdIEDetails]
@ID NVARCHAR(40),
@IE_GUID NVARCHAR(40),
@DLA NVARCHAR(40),
@DDA NVARCHAR(40),
@CLA NVARCHAR(40),
@CDA NVARCHAR(40),
@Money DECIMAL,
@C_GUID NVARCHAR(50),
@Remark nvarchar(200)=null,
@Currency nvarchar(5)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

    -- Insert statements for procedure here
    BEGIN TRAN;
	DELETE dbo.T_IEDetails WHERE R_GUID = @ID;
	
	INSERT INTO dbo.T_IEDetails(R_GUID,IE_GUID,
	DebitLedgerAccount,DebitDetailsAccount,
	CreditLedgerAccount,CreditDetailsAccount,Money,C_GUID,Remark,Currency)
	VALUES(@ID,@IE_GUID,@DLA,@DDA,@CLA,@CDA,@Money,@C_GUID,@Remark,@Currency);
	COMMIT TRAN;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdExpenseTypeRecord]    Script Date: 06/02/2016 14:21:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdExpenseTypeRecord]
	@GUID NVARCHAR(40),
	@ET_GUID NVARCHAR(40),
	@ExpenseType NVARCHAR(40),
	@ExpenseFlag NVARCHAR(1),
	@SaleFlag NVARCHAR(1),
	@ManageFlag NVARCHAR(1),
	@FinanceFlag NVARCHAR(1),
	@SalaryFlag NVARCHAR(1),
	@TaxFlag NVARCHAR(1)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	
	BEGIN TRAN;
	
	DELETE dbo.T_ExpenseType WHERE ET_GUID=@GUID;
    INSERT INTO dbo.T_ExpenseType(ET_GUID,ExpenseType,ExpenseFlag,SaleFlag,ManageFlag,FinanceFlag,SalaryFlag,TaxFlag)
    VALUES (@ET_GUID,@ExpenseType,@ExpenseFlag,@SaleFlag,@ManageFlag,@FinanceFlag,@SalaryFlag,@TaxFlag);
	
	COMMIT TRAN;
    
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdDetailstate]    Script Date: 06/02/2016 14:21:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	更新银行
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdDetailstate]
	@GUID NVARCHAR(40),
	@State NVARCHAR(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	
	BEGIN TRAN;

	UPDATE dbo.T_DetailedCategories
	SET State=@State
	WHERE GUID=@GUID
	
	COMMIT TRAN;
    
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdDetailsAccount]    Script Date: 06/02/2016 14:21:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	更新明细科目
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdDetailsAccount]
	@ID NVARCHAR(40),
	@NAME NVARCHAR(100),
	@PID NVARCHAR(40),
	@CODE INT,
	@C_ID NVARCHAR(40),
	@D_ID NVARCHAR(40)=NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	
	BEGIN TRAN;
    DELETE dbo.T_DetailedAccount WHERE DA_GUID = @ID;
    INSERT INTO dbo.T_DetailedAccount(DA_GUID,AccCode,Name,ParentAccGuid,C_GUID,D_GUID)
    VALUES (@ID,@CODE,@NAME,@PID,@C_ID,@D_ID);
    COMMIT TRAN;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdDetail]    Script Date: 06/02/2016 14:21:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	更新银行
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdDetail]
	@GUID NVARCHAR(40),
	@Name NVARCHAR(40),
	@State NVARCHAR(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	
	BEGIN TRAN;

	INSERT INTO dbo.T_DetailedCategories(GUID,Name,State)
	VALUES(@GUID,@Name,@State);
	
	COMMIT TRAN;
    
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdDeclareCustomerState]    Script Date: 06/02/2016 14:21:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<chenxiang>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdDeclareCustomerState]
	-- Add the parameters for the stored procedure here
	@ID nvarchar(40),
	@State nvarchar(40)
	 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE T_DeclareCustomer
	SET State=@State
	WHERE GUID=@ID; 
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdDeclareCostSpendingState]    Script Date: 06/02/2016 14:21:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<chenxiang>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdDeclareCostSpendingState]
	-- Add the parameters for the stored procedure here
	@ID nvarchar(40),
	@State nvarchar(40)
	 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE T_DeclareCostSpending
	SET State=@State
	WHERE GUID=@ID; 
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdCompanySetting]    Script Date: 06/02/2016 14:21:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<魏礼明>
-- Create date: <2015/04/14>
-- Description:	<保存公司设置>
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdCompanySetting]
	-- Add the parameters for the stored procedure here
	@R_GUID NVARCHAR(50),
	@C_GUID NVARCHAR(50),
	@StandardCoin NVARCHAR(50),
	@ReportStartDate DATETIME,
	@AuditDate DATETIME
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

    -- Insert statements for procedure here
	 BEGIN TRAN;
	DELETE dbo.T_CompanySetting WHERE C_GUID=@C_GUID;
	INSERT INTO dbo.T_CompanySetting( R_GUID,C_GUID,StandardCoin,ReportStartDate,AuditDate)
	VALUES( @R_GUID,@C_GUID,@StandardCoin,@ReportStartDate,@AuditDate);
	COMMIT TRAN;
	END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdCompanyInformation]    Script Date: 06/02/2016 14:21:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<魏礼明>
-- Create date: <2014/04/08>
-- Description:	<更新公司信息>
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdCompanyInformation] 
	-- Add the parameters for the stored procedure here
	@C_GUID NVARCHAR(50),
	@ChineseFullName NVARCHAR(50)=null,
	@EnglishFullName NVARCHAR(50)=null,
	@Website NVARCHAR(50)=null,
	@OrganizationCode NVARCHAR(50)=null,
	@IndustryInvolved NVARCHAR(50)=null,
	@RegisteredAddress NVARCHAR(50)=null,
	@LOGO NVARCHAR(400)=null,
	@Remark NVARCHAR(50)=null,
	@BusinessLicense NVARCHAR(400)=null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

    -- Insert statements for procedure here
    BEGIN TRAN;
    
		UPDATE T_Company SET
		ChineseFullName = @ChineseFullName,
		EnglishFullName = @EnglishFullName,
		Website = @Website,
		OrganizationCode = @OrganizationCode,
		IndustryInvolved = @IndustryInvolved,
		RegisteredAddress = @RegisteredAddress,
		LOGO = @LOGO,
		Remark = @Remark,
		BusinessLicense = @BusinessLicense
		WHERE C_GUID = @C_GUID
	
	 COMMIT TRAN;
	END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdCompanyCy]    Script Date: 06/02/2016 14:21:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<魏礼明>
-- Create date: <2015/04/14>
-- Description:	<保存常用币制>
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdCompanyCy]
	-- Add the parameters for the stored procedure here
	@R_GUID NVARCHAR(50),
	@C_GUID NVARCHAR(50),
	@Code NVARCHAR(5)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
    -- Insert statements for procedure here
	INSERT INTO dbo.R_CompanyCurrceny(R_GUID,C_GUID,Code) VALUES(@R_GUID,@C_GUID,@Code)
	
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdCompany]    Script Date: 06/02/2016 14:21:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<魏礼明>
-- Create date: <2014/04/08>
-- Description:	<更新公司信息>
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdCompany] 
	-- Add the parameters for the stored procedure here
	@C_GUID NVARCHAR(50),
	@Name NVARCHAR(50),
	@Address NVARCHAR(50),
	@Contacter NVARCHAR(50),
	@ContactWay NVARCHAR(50),
	@Type NVARCHAR(50),
	@AuditDate DATETIME,
	@MasterCompanyGuid NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

    -- Insert statements for procedure here
    BEGIN TRAN;
    IF NOT EXISTS (SELECT * FROM dbo.T_Company WHERE C_GUID=@C_GUID)
    BEGIN
		DECLARE @LAS NVARCHAR(MAX) = 
		'2D2C780B-8191-4A5A-A1EE-0A2D969F2299,'+ --	应付职工薪酬
		'B2FC372F-EB9F-4005-B66F-FC07F44BD711,'+ --	盈余公积
		'082CD9EB-9947-43C4-A7C6-F2B7FAB6EE54,'+ --	所得税费用
		'271FDD30-2FAC-49B5-9F46-023424ABB4C7,'+ --	应收账款
		'76FF2B6B-932A-4CA1-850C-758310496AEB,'+ --	银行存款
		'DB57CF51-0328-4F82-842B-4710FB65AAA7,'+ --	专项应付款
		'88C60FC8-2FCB-41CD-B721-C58A981961B0,'+ --	预付账款
		'F9E8B745-CB32-449E-B410-209B9D43B7A3,'+ --	库存现金
		'994F056A-1461-4E6C-A25B-B81B90BBBB63,'+ --	本年利润
		'1F500FDD-1460-45DC-BE8F-39F5ACCE5D95,'+ --	利润分配
		'547E5A1A-1C20-4249-92C8-67FFFFBD38E7,'+ --	管理费用
		'28C49483-7FD7-4741-B154-B36EF52469D2,'+ --	其他业务收入
		'F85560AA-4951-4214-AF7F-5B890C9524B2,'+ --	财务费用
		'806373A3-41D2-4F40-AC51-2C4C82E318DE,'+ --	手续费及佣金支出
		'D27CA8F5-A98C-41E4-8E49-E0BE34E93035,'+ --	主营业务收入
		'76929BB2-2DF1-43BF-B33B-2F9D3FB851FB,'+ --	其他应付款
		'6e5d48b6-adc5-43b8-94e0-9c26e3277184,'+ --	待摊费用
		'1AC24C5E-FAB4-43EA-A303-A448604FC6C2,'+ --	其他业务成本
		'132B92DE-1469-411F-A75F-C04B61E507D1,'+ --	营业税金及附加
		'7C252091-900B-460C-8DBB-9F0DA2DC5506,'+ --	资本公积
		'B5920041-15AC-45A8-AC5C-8E8E83AB9076,'+ --	累计摊销
		'85BDCCDC-9D49-48F0-83A4-3F47B203DDC9,'+ --	长期待摊费用
		'86D57E6A-6207-46CF-A7E0-03C36A10DBBF,'+ --	汇兑损益
		'33CBFEF1-6B00-4B50-AD4F-3C98830F8B05,'+ --	应付账款
		'CD0D907D-5ED1-4785-ACFB-F59C2A8B920C,'+ --	以前年度损益调整
		'C2CDA7BC-C4D0-4B72-91CD-5EDEE7ED3B6A,'+ --	累计折旧
		'51BFDD3E-2253-4FBF-A946-19C18C25C6FC,'+ --	主营业务成本
		'4F380EB2-C1BC-483C-B229-A7FAEA03D054,'+ --	营业外支出
		'794F3D3A-D3AD-431F-988C-9CFBC2A1D207,'+ --	利息收入
		'EF58B046-0BDE-464E-96D2-8F731366349A,'+ --	预收账款
		'65BC8EDB-949A-4EF2-BCCD-AB5FE10DC88E,'+ --	营业外收入
		'FD7ED40C-6BA4-4C22-8029-6F1509441BEA,'+ --	固定资产
		'DC83D8A5-31F6-4DFE-B093-87F90A234E53,'+ --	销售费用
		'80FC4775-3169-42B0-A558-F3AE813DBF14,'+ --	其他应收款
		'60E8BAAA-2043-479F-9D13-E9753F5BA512,'+ --	实收资本
		'A7C86EFD-D448-4784-AFC2-758FCA90D9B8'; --	应交税费
		DECLARE @CurLA NVARCHAR(40);
		DECLARE USER_CUR CURSOR FOR 
		SELECT CAST(short_str AS NVARCHAR(40)) FROM dbo.FUN_SPLIT(@LAS,',') T;
		
		OPEN USER_CUR;
		FETCH NEXT FROM USER_CUR INTO @CurLA;
		WHILE @@FETCH_STATUS = 0
		BEGIN
			INSERT INTO dbo.R_CompanyAccount(R_GUID,LA_GUID,C_GUID)
			VALUES(NEWID(),@CurLA,@C_GUID);
			FETCH NEXT FROM USER_CUR INTO @CurLA;
		END
		CLOSE USER_CUR;
		DEALLOCATE USER_CUR;
	END
	
	DELETE dbo.T_Company WHERE C_GUID=@C_GUID;
	INSERT INTO dbo.T_Company (C_GUID,MasterCompanyGuid,Name,Address,Contacter,ContactWay,Type,AuditDate)
	VALUES(@C_GUID,@MasterCompanyGuid,@Name,@Address,@Contacter,@ContactWay,@Type,@AuditDate)
	
	
	 COMMIT TRAN;
	END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdCashFlowStatement]    Script Date: 06/02/2016 14:21:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	更新现金流量表
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdCashFlowStatement]
	@C_ID NVarChar(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

    DECLARE @CurrentRepBeginDate DATE ;
	DECLARE @CurrentRepEndDate DATE ;
	
	--生成报表时间区间
	
	IF EXISTS (SELECT * FROM dbo.T_Report Rep WHERE  Rep.C_GUID = @C_ID AND Rep.Type='CF') 
	BEGIN 
		DECLARE @LastRepYear INT;
		DECLARE @LastRepMonth INT;
		
		SELECT TOP 1 @LastRepYear =Rep.[Year],@LastRepMonth = Rep.[Month]
		FROM dbo.T_Report Rep 
		WHERE  Rep.C_GUID = @C_ID AND Rep.Type='CF' 
		ORDER BY RepNo DESC;
		
		SET @CurrentRepBeginDate = DATEADD(year,@LastRepYear-1,CONVERT(DATE,'0001-1-1'));
		SET @CurrentRepBeginDate= DATEADD(month,@LastRepMonth,@CurrentRepBeginDate);	
	END
	ELSE
	BEGIN
		SELECT @CurrentRepBeginDate = ReportStartDate
		FROM dbo.T_CompanySetting
		WHERE C_GUID = @C_ID;
	END
		
	SET @CurrentRepEndDate= DATEADD(month,1 ,@CurrentRepBeginDate);
    --生成报表数据
    DECLARE  @Result TABLE
    (
		RGUID NVARCHAR(40),
		Name NVARCHAR(100),
        EndingValue DECIMAL(18,2),
        AccGrp NVARCHAR(40),
        Code INT,
        RP_Flag NVARCHAR(1)
    )

    INSERT INTO @Result(RGUID,Name,AccGrp,Code,EndingValue,RP_Flag)
    SELECT CFI.R_GUID ,CFI.Name ,CFI.PID ,CFI.No ,(ISNULL(RP.Amount,0)) ,CFI.RP_Flag
    FROM dbo.T_CashFlowItem CFI
    LEFT JOIN 
    (SELECT CFItemGuid,SUM(SumAmount) AS Amount
		FROM dbo.T_RecPayRecord 
		WHERE C_GUID = @C_ID AND DATE BETWEEN @CurrentRepBeginDate AND @CurrentRepEndDate
		GROUP BY CFItemGuid
		)RP
		ON  RP.CFItemGuid = CFI.R_GUID;
	
	DECLARE @repID NVARCHAR(40) = NEWID();
	DECLARE @repNo	NVARCHAR(40) = 'BS'+ CONVERT(NVARCHAR(8),@CurrentRepBeginDate,112);
	
	IF NOT EXISTS (SELECT * FROM dbo.T_Report WHERE C_GUID = @C_ID AND RepNo = @repNo)
	BEGIN
		BEGIN TRAN;
		--插入报表信息
		INSERT INTO dbo.T_Report(Rep_GUID,RepNo,Type,C_GUID,Year,Month)
		VALUES(@repID,@repNo,'CF',@C_ID,
		DATEPART(YEAR,@CurrentRepBeginDate),
		DATEPART(MONTH,@CurrentRepBeginDate));
		--插入报表明细
		DECLARE @TmpRepDtl TABLE
		(
			AccGuid NVARCHAR(40),
			AccCode INT,
			AccName NVARCHAR(100),
			AccGrp NVARCHAR(40),
			BeginningValue DECIMAL(18,2),
			EndingValue DECIMAL(18,2)
		);
		
		INSERT INTO @TmpRepDtl
		SELECT R.RGUID,R.Code,R.Name,R.AccGrp,0,R.EndingValue
		FROM @Result R
		
		INSERT INTO dbo.T_ReportDetails(GUID,RGUID,Rep_GUID,Code,Name,AccGrp,BeginningValue,EndingValue)
		SELECT NEWID(),AccGuid,@repID,AccCode,AccName ,AccGrp ,BeginningValue,EndingValue
		FROM @TmpRepDtl ;

		--转储至历史数据(所见即所得)
		DECLARE @RepCount INT = 0;
		SELECT @RepCount = COUNT(*) 
		FROM T_Report Rep 
		WHERE Rep.C_GUID = @C_ID 
		AND Rep.Year = DATEPART(YEAR,@CurrentRepBeginDate)
		AND Rep.Month = DATEPART(Month,@CurrentRepBeginDate)
		IF @RepCount = 3
		BEGIN
			INSERT INTO dbo.T_IEHistoryRecord
			SELECT IE_GUID,IE_Flag,Inv.Name,InvNo,BP.Name,Amount,TaxationAmount Tax,Date,Creator,CreateDate,IE.C_GUID,IE.AffirmDate,IE.SumAmount,IE.Currency,IE.Remark
			FROM dbo.T_IERecord IE
			LEFT JOIN dbo.T_BusinessPartner BP ON BP.BP_GUID = IE.RPer
			LEFT JOIN dbo.T_InvType Inv ON Inv.[Key] = IE.InvType
			WHERE IE.C_GUID = @C_ID AND DATE BETWEEN @CurrentRepBeginDate AND @CurrentRepEndDate;
			
			INSERT INTO dbo.T_IEHistoryDetails
			SELECT R_GUID,IE.IE_GUID,DLA.Name,DDA.Name,CLA.Name,CDA.Name,Money,IE.C_GUID,IED.Currency
			FROM dbo.T_IERecord IE
			LEFT JOIN dbo.T_IEDetails IED ON IED.IE_GUID = IE.IE_GUID
			LEFT JOIN dbo.T_GeneralLedgerAccount DLA ON DLA.LA_GUID = IED.DebitLedgerAccount 
			LEFT JOIN dbo.T_DetailedAccount DDA ON DDA.DA_GUID = IED.DebitDetailsAccount
			LEFT JOIN dbo.T_GeneralLedgerAccount CLA ON CLA.LA_GUID = IED.CreditLedgerAccount
			LEFT JOIN dbo.T_DetailedAccount CDA ON CDA.DA_GUID = IED.DebitDetailsAccount 
			WHERE IE.C_GUID = @C_ID AND DATE BETWEEN @CurrentRepBeginDate AND @CurrentRepEndDate;
			
			INSERT INTO dbo.T_RecPayHistoryRecord(RP_GUID,RP_Flag,C_GUID,InvType,InvNo,R_Per,
			DebitLedgerAccount,DebitDetailsAccount,CreditLedgerAccount,CreditDetailsAccount,Amount,
			Date,Remark,Creator,CreateDate,Currency,CFItem,CFPItem)
			SELECT RP_GUID,RP.RP_Flag,RP.C_GUID,Inv.Name,InvNo,BP.Name,
			DLA.Name,DDA.Name,CLA.Name,CDA.Name,
			RP.SumAmount,Date,RP.Remark,Creator,CreateDate,
			RP.Currency,CFI.Name,CFIP.Name
			FROM dbo.T_RecPayRecord RP
			LEFT JOIN dbo.T_BusinessPartner BP ON BP.BP_GUID = RP.RPer
			LEFT JOIN dbo.T_InvType Inv ON Inv.[Key] = RP.InvType
			LEFT JOIN dbo.T_GeneralLedgerAccount DLA ON DLA.LA_GUID = RP.DebitLedgerAccount 
			LEFT JOIN dbo.T_DetailedAccount DDA ON DDA.DA_GUID = RP.DebitDetailsAccount
			LEFT JOIN dbo.T_GeneralLedgerAccount CLA ON CLA.LA_GUID = RP.CreditLedgerAccount
			LEFT JOIN dbo.T_DetailedAccount CDA ON CDA.DA_GUID = RP.CreditDetailsAccount
			LEFT JOIN dbo.T_CashFlowItem CFI ON CFI.R_GUID = RP.CFItemGuid
			LEFT JOIN dbo.T_CashFlowItem CFIP ON CFIP.R_GUID = RP.CFPItemGuid
			WHERE RP.C_GUID = @C_ID AND DATE BETWEEN @CurrentRepBeginDate AND @CurrentRepEndDate;
			
			--DELETE dbo.T_IEDetails
			--WHERE IE_GUID IN (
			--SELECT IE.IE_GUID
			--FROM dbo.T_IERecord IE 
			--WHERE C_GUID = @C_ID AND DATE BETWEEN @CurrentRepBeginDate AND @CurrentRepEndDate);
			
			--DELETE dbo.T_IERecord  
			--WHERE C_GUID = @C_ID AND DATE BETWEEN @CurrentRepBeginDate AND @CurrentRepEndDate;;
			
			--DELETE dbo.T_RecPayRecord  
			--WHERE C_GUID = @C_ID AND DATE BETWEEN @CurrentRepBeginDate AND @CurrentRepEndDate;;
		END
		COMMIT TRAN;
	END
	ELSE
	BEGIN
		RAISERROR('Exists',12,1);
	END
	
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdBusinessLicense]    Script Date: 06/02/2016 14:21:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<魏礼明>
-- Create date: <2014/04/08>
-- Description:	<更新公司信息>
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdBusinessLicense] 
	-- Add the parameters for the stored procedure here
	@C_GUID NVARCHAR(50),
	@BusinessLicense NVARCHAR(400)=null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

    -- Insert statements for procedure here
    BEGIN TRAN;
    
		UPDATE T_Company SET BusinessLicense = @BusinessLicense
		WHERE C_GUID = @C_GUID
	
	 COMMIT TRAN;
	END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdBeginningBalance]    Script Date: 06/02/2016 14:21:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	更新期初数
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdBeginningBalance]
	@C_ID NVARCHAR(40),
	@LA_ID NVARCHAR(40),
	@Money DECIMAL(18,2)
AS
BEGIN

	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	

	BEGIN TRAN;

	
	INSERT INTO dbo.T_BeginningBalance (R_GUID,C_GUID,Acc_GUID,Money)
	VALUES(NEWID(),@C_ID,@LA_ID,@Money);
	COMMIT TRAN;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdBankAccount]    Script Date: 06/02/2016 14:21:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	更新银行账号
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdBankAccount]
	@BA_GUID NVARCHAR(40),
	@B_GUID NVARCHAR(40),
	@C_GUID NVARCHAR(40),
	@Account NVARCHAR(100),
	@AccountName NVARCHAR(40),
	@AccountCurrency NVARCHAR(40),
	@AccountAbbreviation NVARCHAR(40),
	@AccountType NVARCHAR(40),
	@BankAddress NVARCHAR(100),
	@SwiftCode NVARCHAR(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
    -- Insert statements for procedure here
	BEGIN TRAN;
	DELETE dbo.T_BankAccount WHERE BA_GUID = @BA_GUID;
	
	INSERT INTO dbo.T_BankAccount(BA_GUID,B_GUID,Account,C_GUID,AccountName,AccountCurrency,
	AccountAbbreviation,AccountType,BankAddress,SwiftCode)
	VALUES (@BA_GUID,@B_GUID,@Account,@C_GUID,@AccountName,@AccountCurrency,@AccountAbbreviation,
	@AccountType,@BankAddress,@SwiftCode);
	COMMIT TRAN;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdBank]    Script Date: 06/02/2016 14:21:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	更新银行
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdBank]
	@B_GUID NVARCHAR(40),
	@C_GUID NVARCHAR(40),
	@Name NVARCHAR(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	
	BEGIN TRAN;
	DELETE dbo.T_Bank WHERE B_GUID = @B_GUID;
	
	INSERT INTO dbo.T_Bank(B_GUID,Name,C_GUID)
	VALUES(@B_GUID,@Name,@C_GUID);
	COMMIT TRAN;
    
END
GO
/****** Object:  View [dbo].[V_RerPayRecord]    Script Date: 06/02/2016 14:21:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[V_RerPayRecord]--收付款记录
AS
	SELECT PR.RP_GUID, PR.C_GUID,PR.RP_Flag,PR.InvType, Inv.Name AS TypeName,PR.InvNo,PR.SumAmount,PR.Date,PR.Remark,PR.Creator,PR.CreateDate
	,LA2.Name AS DebitLedgerAccount,DA2.Name AS DebitDetailsAccount,
	LA1.Name AS CreditLedgerAccount,DA1.Name AS CreditDetailsAccount,
	BP.Name R_PerName,PR.Currency,
	PR.CFItemGuid,PR.CFPItemGuid,CFI.Name AS CFItemName,CFIP.Name AS CFPItemName
	FROM dbo.T_RecPayRecord PR
	LEFT JOIN dbo.T_BusinessPartner BP ON PR.RPer = BP.BP_GUID
	LEFT JOIN dbo.T_GeneralLedgerAccount LA1 ON LA1.LA_GUID = PR.CreditLedgerAccount
	LEFT JOIN dbo.T_GeneralLedgerAccount LA2 ON LA2.LA_GUID = PR.DebitLedgerAccount
	LEFT JOIN dbo.T_DetailedAccount DA1 ON DA1.DA_GUID = PR.CreditDetailsAccount
	LEFT JOIN dbo.T_DetailedAccount DA2 ON DA2.DA_GUID = PR.DebitDetailsAccount
	LEFT JOIN dbo.T_InvType Inv ON Inv.[Key] = PR.InvType
	LEFT JOIN dbo.T_CashFlowItem CFI ON CFI.R_GUID = PR.CFItemGuid
	LEFT JOIN dbo.T_CashFlowItem CFIP ON CFIP.R_GUID = PR.CFPItemGuid
	UNION ALL
	SELECT PR.RP_GUID,PR.C_GUID,PR.RP_Flag,NULL AS InvType,PR.InvType AS TypeName,PR.InvNo,PR.Amount,PR.Date,PR.Remark,PR.Creator,PR.CreateDate
	,PR.DebitLedgerAccount,PR.DebitDetailsAccount,PR.CreditLedgerAccount,PR.CreditDetailsAccount,
	PR.R_Per AS R_PerName,PR.Currency,
	NULL,NULL,PR.CFItem AS CFItemName,PR.CFPItem AS CFPItemName
	FROM dbo.T_RecPayHistoryRecord PR
GO
/****** Object:  StoredProcedure [dbo].[SP_GenPerviewIncomeStatements]    Script Date: 06/02/2016 14:21:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	生成利润预览表
-- =============================================
CREATE PROCEDURE [dbo].[SP_GenPerviewIncomeStatements]
	@C_ID NVARCHAR(40),
	@Year INT OUT,
    @Month INT OUT,
    @RepNo	NVARCHAR(40) OUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @CurrentRepBeginDate DATE ;
	DECLARE @CurrentRepEndDate DATE ;
	
	--生成报表时间区间
	
	IF EXISTS (SELECT * FROM dbo.T_Report Rep WHERE  Rep.C_GUID = @C_ID AND Rep.Type='PL') 
	BEGIN 
		DECLARE @LastRepYear INT;
		DECLARE @LastRepMonth INT;
		
		SELECT TOP 1 @LastRepYear =Rep.[Year],@LastRepMonth = Rep.[Month]
		FROM dbo.T_Report Rep 
		WHERE  Rep.C_GUID = @C_ID AND Rep.Type='PL' 
		ORDER BY RepNo DESC;
		
		SET @CurrentRepBeginDate = DATEADD(year,@LastRepYear-1,CONVERT(DATE,'0001-1-1'));
		SET @CurrentRepBeginDate= DATEADD(month,@LastRepMonth,@CurrentRepBeginDate);	
	END
	ELSE
	BEGIN
		SELECT @CurrentRepBeginDate = ReportStartDate
		FROM dbo.T_CompanySetting
		WHERE C_GUID = @C_ID;
	END
	
	SET @Year = DATEPART(YEAR,@CurrentRepBeginDate);
	SET @Month = DATEPART(MONTH,@CurrentRepBeginDate);	
	SET @CurrentRepEndDate= DATEADD(month,1 ,@CurrentRepBeginDate);
	SET @RepNo = 'PL'+ CONVERT(NVARCHAR(8),@CurrentRepBeginDate,112);
    --生成报表数据
    DECLARE @TmpV_PR TABLE
    (
		Profit_GUID NVARCHAR(40),
		AccGroup INT,
		Flag NVARCHAR(40),
		Money DECIMAL(18,2)
    );
    
    INSERT INTO @TmpV_PR(Profit_GUID,AccGroup,Money,Flag)
    SELECT FR.Profit_GUID,GLA.AccGroup,FR.Amount as Money,FR.IE_Flag as Flag
    FROM dbo.T_IERecord FR
    LEFT JOIN dbo.T_GeneralLedgerAccount AS GLA ON FR.Profit_GUID = GLA.LA_GUID
    WHERE FR.C_GUID = @C_ID AND DATE BETWEEN @CurrentRepBeginDate AND @CurrentRepEndDate;
    
    DECLARE  @TmpFR TABLE
    (
		ACC_GUID NVARCHAR(40),
        Value DECIMAL(18,2),
        AccGrp INT
    )
    --(收入)
    INSERT INTO @TmpFR(ACC_GUID,Value,AccGrp)
    SELECT FR.Profit_GUID AS ACC_GUID,(FR.Money) AS Value,FR.AccGroup AS AccGrp
    FROM @TmpV_PR FR
    WHERE Flag='I'  
    --AND   FR.AccGroup = 6 OR FR.AccGroup = 7 OR FR.AccGroup = 5;
    
    --(营业税金)
    INSERT INTO @TmpFR(ACC_GUID,Value,AccGrp)
    SELECT '132B92DE-1469-411F-A75F-C04B61E507D1',(FR.TaxationAmount) AS Value,GLA.AccGroup AS AccGrp
    FROM T_IERecord FR
    LEFT JOIN dbo.T_GeneralLedgerAccount AS GLA ON FR.Profit_GUID = GLA.LA_GUID
    WHERE FR.IE_Flag='I' 
    AND GLA.AccGroup = 6 OR GLA.AccGroup = 7 OR GLA.AccGroup = 5;
    
    --(费用)
    INSERT INTO @TmpFR(ACC_GUID,Value,AccGrp)
    SELECT FR.Profit_GUID AS ACC_GUID,(0-FR.Money) AS Value,FR.AccGroup AS AccGrp
    FROM @TmpV_PR FR
    WHERE Flag='E'
    --AND FR.AccGroup = 6 OR FR.AccGroup = 7 OR FR.AccGroup = 5;
    
  
  SELECT LA.AccGroup AS AccGrp,LA.Name,LA.AccCode AS Code ,ISNULL(T.CurrentValue,0) AS EndingValue
  FROM dbo.R_CompanyAccount R_CA
  LEFT JOIN dbo.T_GeneralLedgerAccount LA ON R_CA.LA_GUID = LA.LA_GUID
  LEFT JOIN (
		SELECT AccGrp,ACC_GUID,SUM(Value) AS CurrentValue
		FROM @TmpFR
		GROUP BY AccGrp,ACC_GUID
		HAVING SUM(Value)<> 0
	) AS T ON T.ACC_GUID = R_CA.LA_GUID
	WHERE R_CA.C_GUID = @C_ID AND (LA.AccGroup = 6 OR LA.AccGroup = 7 )
END


--ROLLBACK
GO
/****** Object:  StoredProcedure [dbo].[SP_GenPerviewIncomeStatement]    Script Date: 06/02/2016 14:21:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	生成利润预览表
-- =============================================
CREATE PROCEDURE [dbo].[SP_GenPerviewIncomeStatement]
	@C_ID NVARCHAR(40),
	@Year INT OUT,
    @Month INT OUT,
    @RepNo	NVARCHAR(40) OUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @CurrentRepBeginDate DATE ;
	DECLARE @CurrentRepEndDate DATE ;
	
	--生成报表时间区间
	
	IF EXISTS (SELECT * FROM dbo.T_Report Rep WHERE  Rep.C_GUID = @C_ID AND Rep.Type='PL') 
	BEGIN 
		DECLARE @LastRepYear INT;
		DECLARE @LastRepMonth INT;
		
		SELECT TOP 1 @LastRepYear =Rep.[Year],@LastRepMonth = Rep.[Month]
		FROM dbo.T_Report Rep 
		WHERE  Rep.C_GUID = @C_ID AND Rep.Type='PL' 
		ORDER BY RepNo DESC;
		
		SET @CurrentRepBeginDate = DATEADD(year,@LastRepYear-1,CONVERT(DATE,'0001-1-1'));
		SET @CurrentRepBeginDate= DATEADD(month,@LastRepMonth,@CurrentRepBeginDate);	
	END
	ELSE
	BEGIN
		SELECT @CurrentRepBeginDate = ReportStartDate
		FROM dbo.T_CompanySetting
		WHERE C_GUID = @C_ID;
	END
	
	SET @Year = DATEPART(YEAR,@CurrentRepBeginDate);
	SET @Month = DATEPART(MONTH,@CurrentRepBeginDate);	
	SET @CurrentRepEndDate= DATEADD(month,1 ,@CurrentRepBeginDate);
	SET @RepNo = 'PL'+ CONVERT(NVARCHAR(8),@CurrentRepBeginDate,112);
    --生成报表数据
    DECLARE @TmpV_PR TABLE
    (
		DebitLedgerAccount NVARCHAR(40),
		DebitAccGroup INT,
		CreditLedgerAccount NVARCHAR(40),
		CreditAccGroup INT,
		Money DECIMAL(18,2)
    );
    
    INSERT INTO @TmpV_PR(DebitLedgerAccount,DebitAccGroup,CreditLedgerAccount,CreditAccGroup,Money)
    SELECT FR.DebitLedgerAccount,FR.DebitAccGroup,FR.CreditLedgerAccount,FR.CreditAccGroup,FR.Money
    FROM dbo.V_FinanceRecord FR
    WHERE FR.C_GUID = @C_ID AND DATE BETWEEN @CurrentRepBeginDate AND @CurrentRepEndDate;
    
    DECLARE  @TmpFR TABLE
    (
		ACC_GUID NVARCHAR(40),
        Value DECIMAL(18,2),
        AccGrp INT
    )
    
    INSERT INTO @TmpFR(ACC_GUID,Value,AccGrp)
    SELECT FR.DebitLedgerAccount AS ACC_GUID,(0-FR.Money) AS Value,FR.DebitAccGroup AS AccGrp
    FROM @TmpV_PR FR
    WHERE  FR.DebitAccGroup = 6 OR FR.DebitAccGroup = 7 OR FR.DebitAccGroup = 5;
    
    INSERT INTO @TmpFR(ACC_GUID,Value,AccGrp)
    SELECT FR.CreditLedgerAccount AS ACC_GUID,FR.Money AS Value, FR.CreditAccGroup AS AccGrp
    FROM @TmpV_PR FR
    WHERE FR.CreditAccGroup = 6 OR FR.CreditAccGroup = 7 OR FR.CreditAccGroup = 5;
    
    --INSERT INTO @TmpFR(ACC_GUID,Value,AccGrp)
    --SELECT FR.DebitLedgerAccount AS ACC_GUID,(0-FR.Money) AS Value,FR.DebitAccGroup AS AccGrp
    --FROM @TmpV_PR FR
    --WHERE  FR.DebitAccGroup = 7 ;
    
    --INSERT INTO @TmpFR(ACC_GUID,Value,AccGrp)
    --SELECT FR.CreditLedgerAccount AS ACC_GUID,FR.Money AS Value, FR.CreditAccGroup AS AccGrp
    --FROM @TmpV_PR FR
    --WHERE FR.CreditAccGroup = 7;
    
  
  SELECT LA.AccGroup AS AccGrp,LA.Name,LA.AccCode AS Code ,ISNULL(T.CurrentValue,0) AS EndingValue
  FROM dbo.R_CompanyAccount R_CA
  LEFT JOIN dbo.T_GeneralLedgerAccount LA ON R_CA.LA_GUID = LA.LA_GUID
  LEFT JOIN (
		SELECT AccGrp,ACC_GUID,SUM(Value) AS CurrentValue
		FROM @TmpFR
		GROUP BY AccGrp,ACC_GUID
		HAVING SUM(Value)<> 0
	) AS T ON T.ACC_GUID = R_CA.LA_GUID
	WHERE R_CA.C_GUID = @C_ID AND (LA.AccGroup = 6 OR LA.AccGroup = 7 )
	--AND ( ISNULL(T.CurrentValue,0) <> 0)
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdBalanceSheet]    Script Date: 06/02/2016 14:21:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	更新资产负债表
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdBalanceSheet]
	@C_ID NVarChar(40)
AS
BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	DECLARE @CurrentRepBeginDate DATE ;
	DECLARE @CurrentRepEndDate DATE ;
	
	--生成报表时间区间
	
	IF EXISTS (SELECT * FROM dbo.T_Report Rep WHERE  Rep.C_GUID = @C_ID AND Rep.Type='BS') 
	BEGIN 
		DECLARE @LastRepYear INT;
		DECLARE @LastRepMonth INT;
		
		SELECT TOP 1 @LastRepYear =Rep.[Year],@LastRepMonth = Rep.[Month]
		FROM dbo.T_Report Rep 
		WHERE  Rep.C_GUID = @C_ID AND Rep.Type='BS' 
		ORDER BY RepNo DESC;
		
		SET @CurrentRepBeginDate = DATEADD(year,@LastRepYear-1,CONVERT(DATE,'0001-1-1'));
		SET @CurrentRepBeginDate= DATEADD(month,@LastRepMonth,@CurrentRepBeginDate);	
	END
	ELSE
	BEGIN
		SELECT @CurrentRepBeginDate = ReportStartDate
		FROM dbo.T_CompanySetting
		WHERE C_GUID = @C_ID;
	END
	
	SET @CurrentRepEndDate= DATEADD(month,1 ,@CurrentRepBeginDate);
	
    --生成报表数据
    DECLARE @TmpV_PR TABLE
    (
		DebitLedgerAccount NVARCHAR(40),
		DebitDetailsAccount NVARCHAR(40),
		DebitAccGroup INT,
		CreditLedgerAccount NVARCHAR(40),
		CreditDetailsAccount NVARCHAR(40),
		CreditAccGroup INT,
		Money DECIMAL(18,2)
    );
    
    INSERT INTO @TmpV_PR(DebitLedgerAccount,DebitDetailsAccount,DebitAccGroup,CreditLedgerAccount,CreditDetailsAccount,CreditAccGroup,Money)
    SELECT FR.DebitLedgerAccount,FR.DebitDetailsAccount,FR.DebitAccGroup,
		   FR.CreditLedgerAccount,FR.CreditDetailsAccount,FR.CreditAccGroup,FR.Money
    FROM dbo.V_FinanceRecord FR
    WHERE FR.C_GUID = @C_ID AND DATE BETWEEN @CurrentRepBeginDate AND @CurrentRepEndDate;
    
    DECLARE  @TmpFR TABLE
    (
		LA_GUID NVARCHAR(40),
		DA_GUID NVARCHAR(40),
        Value DECIMAL(18,2),
        AccGrp INT
    )
    
    INSERT INTO @TmpFR(LA_GUID,DA_GUID,Value,AccGrp)
    SELECT FR.DebitLedgerAccount,FR.DebitDetailsAccount,FR.Money ,FR.DebitAccGroup 
    FROM @TmpV_PR FR
    WHERE  FR.DebitAccGroup = 1 ;
    
    INSERT INTO @TmpFR(LA_GUID,DA_GUID,Value,AccGrp)
    SELECT FR.CreditLedgerAccount,FR.CreditDetailsAccount,(0-FR.Money) , FR.CreditAccGroup
    FROM @TmpV_PR FR
    WHERE FR.CreditAccGroup = 1 ;
    
    INSERT INTO @TmpFR(LA_GUID,DA_GUID,Value,AccGrp)
    SELECT FR.DebitLedgerAccount,FR.DebitDetailsAccount,(0-FR.Money) ,FR.DebitAccGroup 
    FROM @TmpV_PR FR
    WHERE  FR.DebitAccGroup = 2 OR FR.DebitAccGroup = 4 ;
    
    INSERT INTO @TmpFR(LA_GUID,DA_GUID,Value,AccGrp)
    SELECT FR.CreditLedgerAccount,FR.CreditDetailsAccount ,FR.Money , FR.CreditAccGroup 
    FROM @TmpV_PR FR
    WHERE FR.CreditAccGroup = 2  OR FR.CreditAccGroup = 4;

	DECLARE @repID NVARCHAR(40) = NEWID();
	DECLARE @repNo	NVARCHAR(40) = 'BS'+ CONVERT(NVARCHAR(8),@CurrentRepBeginDate,112);
	
	IF NOT EXISTS (SELECT * FROM dbo.T_Report WHERE C_GUID=@C_ID AND RepNo = @repNo)
	BEGIN
		BEGIN TRAN;
		--插入报表信息
		INSERT INTO dbo.T_Report(Rep_GUID,RepNo,Type,C_GUID,Year,Month)
		VALUES(@repID,@repNo,'BS',@C_ID,
		DATEPART(YEAR,@CurrentRepBeginDate),
		DATEPART(MONTH,@CurrentRepBeginDate));;
		--插入报表明细
		DECLARE @TmpRepDtl TABLE
		(
			AccGuid NVARCHAR(40),
			AccCode INT,
			AccName NVARCHAR(100),
			AccGrp INT,
			BeginningValue DECIMAL(18,2),
			EndingValue DECIMAL(18,2)
		);
		
		INSERT INTO @TmpRepDtl
		SELECT LA.LA_GUID,LA.AccCode,LA.Name ,LA.AccGroup ,ISNULL(BB.Money,0) AS BeginningValue,(ISNULL(BB.Money,0) + ISNULL(T.CurrentValue,0)) AS EndingValue
		FROM dbo.R_CompanyAccount R_CA
		LEFT JOIN dbo.T_GeneralLedgerAccount LA ON R_CA.LA_GUID = LA.LA_GUID
		LEFT JOIN dbo.T_BeginningBalance BB ON BB.C_GUID = @C_ID AND BB.Acc_GUID = R_CA.LA_GUID
		LEFT JOIN (
			SELECT AccGrp,LA_GUID,SUM(Value) AS CurrentValue
			FROM @TmpFR
			GROUP BY AccGrp,LA_GUID
			HAVING SUM(Value)<> 0
		) AS T ON T.LA_GUID = R_CA.LA_GUID
		WHERE R_CA.C_GUID = @C_ID AND (LA.AccGroup = 1 OR LA.AccGroup = 2 OR LA.AccGroup = 4)
		AND (ISNULL(BB.Money,0) <> 0 OR (ISNULL(BB.Money,0) + ISNULL(T.CurrentValue,0)) <> 0);
		
		INSERT INTO dbo.T_ReportDetails(RGUID,Rep_GUID,Code,Name,AccGrp,BeginningValue,EndingValue)
		SELECT NEWID(),@repID,AccCode,AccName ,AccGrp ,BeginningValue,EndingValue
		FROM @TmpRepDtl ;
		--更新期初数
		DECLARE @Cur_Tmp TABLE
		(
			Acc_GUID NVARCHAR(40),
			Money DECIMAL(18,2),
			pID NVARCHAR(40)
		)
		DECLARE @BB_Tmp TABLE
		(
			Acc_GUID NVARCHAR(40),
			Money DECIMAL(18,2)
		)
			--本期期初数
		INSERT INTO @BB_Tmp
		SELECT BB.Acc_GUID ,Money 
		FROM dbo.T_BeginningBalance BB
		WHERE BB.C_GUID = @C_ID;
			--本期发生额
		INSERT INTO @Cur_Tmp
		SELECT BB.Acc_GUID,ISNULL(T.Value,0) ,DA.ParentAccGuid
		FROM dbo.T_BeginningBalance BB
		LEFT JOIN (
			SELECT (CASE WHEN LEN(DA_GUID) > 0 THEN DA_GUID 
						 WHEN LEN(DA_GUID) = 0 THEN LA_GUID END) AS Acc_GUID,SUM(Value) AS Value
			FROM @TmpFR
			GROUP BY (CASE WHEN LEN(DA_GUID) > 0 THEN DA_GUID 
						 WHEN LEN(DA_GUID) = 0 THEN LA_GUID END)
			HAVING SUM(Value) <> 0
		) AS T ON BB.Acc_GUID = T.Acc_GUID
		LEFT JOIN dbo.T_DetailedAccount DA ON BB.Acc_GUID = DA.DA_GUID AND DA.C_GUID = @C_ID;
		
		DECLARE USER_CUR CURSOR FOR SELECT pID,Money FROM @Cur_Tmp WHERE Money <> 0;
		DECLARE @pAcc NVARCHAR(40);
		DECLARE @curVal DECIMAL(18,2);
		OPEN USER_CUR;
		FETCH NEXT FROM USER_CUR INTO @pAcc,@curVal;
		WHILE @@FETCH_STATUS = 0
		BEGIN
			WHILE @pAcc IS NOT NULL
			BEGIN
				UPDATE @Cur_Tmp SET Money = Money + @curVal WHERE Acc_GUID = @pAcc;
				SELECT @pAcc = pID FROM @Cur_Tmp WHERE Acc_GUID = @pAcc;
			END
			FETCH NEXT FROM USER_CUR INTO @pAcc,@curVal;
		END
		CLOSE USER_CUR;
		DEALLOCATE USER_CUR;
		
		DELETE dbo.T_BeginningBalance WHERE C_GUID = @C_ID;
		INSERT INTO dbo.T_BeginningBalance(R_GUID,C_GUID,Acc_GUID,Money)
		SELECT NEWID(),@C_ID,T1.Acc_GUID,(T1.Money + T2.Money)
		FROM @BB_Tmp T1
		INNER JOIN @Cur_Tmp T2 ON T1.Acc_GUID = T2.Acc_GUID;

		--转储至历史数据(所见即所得)
		DECLARE @RepCount INT = 0;
		SELECT @RepCount = COUNT(*) 
		FROM T_Report Rep 
		WHERE Rep.C_GUID = @C_ID 
		AND Rep.Year = DATEPART(YEAR,@CurrentRepBeginDate)
		AND Rep.Month = DATEPART(Month,@CurrentRepBeginDate)
		IF @RepCount = 3
		BEGIN
			INSERT INTO dbo.T_IEHistoryRecord
			SELECT IE_GUID,IE_Flag,Inv.Name,InvNo,BP.Name,Amount,Tax,Date,Creator,CreateDate,IE.C_GUID
			FROM dbo.T_IERecord IE
			LEFT JOIN dbo.T_BusinessPartner BP ON BP.BP_GUID = IE.RPer
			LEFT JOIN dbo.T_InvType Inv ON Inv.[Key] = IE.InvType
			WHERE IE.C_GUID = @C_ID AND DATE BETWEEN @CurrentRepBeginDate AND @CurrentRepEndDate;
			
			INSERT INTO dbo.T_IEHistoryDetails
			SELECT R_GUID,IE.IE_GUID,DLA.Name,DDA.Name,CLA.Name,CDA.Name,Money,IE.C_GUID,IED.Currency
			FROM dbo.T_IERecord IE
			LEFT JOIN dbo.T_IEDetails IED ON IED.IE_GUID = IE.IE_GUID
			LEFT JOIN dbo.T_GeneralLedgerAccount DLA ON DLA.LA_GUID = IED.DebitLedgerAccount 
			LEFT JOIN dbo.T_DetailedAccount DDA ON DDA.DA_GUID = IED.DebitDetailsAccount
			LEFT JOIN dbo.T_GeneralLedgerAccount CLA ON CLA.LA_GUID = IED.CreditLedgerAccount
			LEFT JOIN dbo.T_DetailedAccount CDA ON CDA.DA_GUID = IED.DebitDetailsAccount 
			WHERE IE.C_GUID = @C_ID AND DATE BETWEEN @CurrentRepBeginDate AND @CurrentRepEndDate;
			
			INSERT INTO dbo.T_RecPayHistoryRecord(RP_GUID,RP_Flag,C_GUID,InvType,InvNo,R_Per,
			DebitLedgerAccount,DebitDetailsAccount,CreditLedgerAccount,CreditDetailsAccount,Amount,
			Date,Remark,Creator,CreateDate,Currency,CFItem,CFPItem)
			SELECT RP_GUID,RP.RP_Flag,RP.C_GUID,Inv.Name,InvNo,BP.Name,
			DLA.Name,DDA.Name,CLA.Name,CDA.Name,
			Amount,Date,Remark,Creator,CreateDate,
			RP.Currency,CFI.Name,CFIP.Name
			FROM dbo.T_RecPayRecord RP
			LEFT JOIN dbo.T_BusinessPartner BP ON BP.BP_GUID = RP.R_Per
			LEFT JOIN dbo.T_InvType Inv ON Inv.[Key] = RP.InvType
			LEFT JOIN dbo.T_GeneralLedgerAccount DLA ON DLA.LA_GUID = RP.DebitLedgerAccount 
			LEFT JOIN dbo.T_DetailedAccount DDA ON DDA.DA_GUID = RP.DebitDetailsAccount
			LEFT JOIN dbo.T_GeneralLedgerAccount CLA ON CLA.LA_GUID = RP.CreditLedgerAccount
			LEFT JOIN dbo.T_DetailedAccount CDA ON CDA.DA_GUID = RP.CreditDetailsAccount
			LEFT JOIN dbo.T_CashFlowItem CFI ON CFI.R_GUID = RP.CFItemGuid
			LEFT JOIN dbo.T_CashFlowItem CFIP ON CFIP.R_GUID = RP.CFPItemGuid
			WHERE RP.C_GUID = @C_ID AND DATE BETWEEN @CurrentRepBeginDate AND @CurrentRepEndDate;
			
			DELETE dbo.T_IEDetails
			WHERE IE_GUID IN (
			SELECT IE.IE_GUID
			FROM dbo.T_IERecord IE 
			WHERE C_GUID = @C_ID AND DATE BETWEEN @CurrentRepBeginDate AND @CurrentRepEndDate);
			
			DELETE dbo.T_IERecord  
			WHERE C_GUID = @C_ID AND DATE BETWEEN @CurrentRepBeginDate AND @CurrentRepEndDate;;
			
			DELETE dbo.T_RecPayRecord  
			WHERE C_GUID = @C_ID AND DATE BETWEEN @CurrentRepBeginDate AND @CurrentRepEndDate;;
		END
		COMMIT TRAN;
	END
	ELSE
	BEGIN
		RAISERROR('Exists',12,1);
	END
	
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetRecPayRecordCopy]    Script Date: 06/02/2016 14:21:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,chenxiang>
-- Create date: <Create Date,,07/16/2015>
-- Description:	获取收付款记录
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetRecPayRecordCopy]
	@PageSize int = -1,
	@PageIndex int = 1,
	@Count int = 0 out,
	@ID NVARCHAR(40) = NULL,
	@Flag NVARCHAR(1) = NULL,
	@C_GUID NVARCHAR(50),
	@IsAll bit = 0,
	@ClassifyFlag bit = NULL,
	@dateBegin DATETIME = NULL,
	@dateEnd DATETIME = NULL,
	@customer NVARCHAR(40) = NULL,
	@incomeGrp NVARCHAR(20)=NULL,
	@incomeGrpdts NVARCHAR(40)=NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @temp TABLE(
		rownumber int,
		RP_GUID NVARCHAR(40),
		IE_GUID NVARCHAR(40),
		InvType nvarchar(40),
		InvTypeDts nvarchar(40),
		InvNo nvarchar(20),
		RPer nvarchar(40),
		SumAmount decimal(18,2),
		Date date,
		Remark nvarchar(200),
		Creator  nvarchar(40),
		CreateDate datetime,
		DebitLedgerAccount nvarchar(40),
		DebitDetailsAccount nvarchar(40),
		CreditLedgerAccount nvarchar(40),
		CreditDetailsAccount nvarchar(40),
		R_PerName nvarchar(100),
		DebitLedgerAccountName nvarchar(40),
		CreditLedgerAccountName nvarchar(40),
		Currency NVARCHAR(5),
		B_GUID NVARCHAR(40),
		BA_GUID NVARCHAR(40),
		BankAccount NVARCHAR(100),
		A_GUID NVARCHAR(50)
	 )
	 
    -- Insert statements for procedure here
    IF (@ID IS NULL)
    BEGIN
		IF(@IsAll = 0)
		BEGIN
		insert into @temp
			SELECT row_number()over(order by PR.Date desc) rownumber,PR.RP_GUID,PR.IE_GUID,PR.InvType,PR.InvTypeDts,PR.InvNo,PR.RPer,PR.SumAmount,PR.Date,PR.Remark,PR.Creator,PR.CreateDate
			,PR.DebitLedgerAccount,PR.DebitDetailsAccount,PR.CreditLedgerAccount,PR.CreditDetailsAccount,BP.Name R_PerName,
			LA2.Name AS DebitLedgerAccountName,LA1.Name AS CreditLedgerAccountName,
			PR.Currency,PR.B_GUID,PR.BA_GUID,BA.Account AS BankAccount,TA.A_GUID
			FROM dbo.T_RecPayRecord PR
			LEFT JOIN dbo.T_BusinessPartner BP ON PR.RPer = BP.BP_GUID
			LEFT JOIN dbo.T_GeneralLedgerAccount LA1 ON LA1.LA_GUID = PR.CreditLedgerAccount
			LEFT JOIN dbo.T_GeneralLedgerAccount LA2 ON LA2.LA_GUID = PR.DebitLedgerAccount
			LEFT JOIN dbo.T_BankAccount BA ON BA.BA_GUID = PR.BA_GUID
			LEFT JOIN dbo.T_Attachment TA ON TA.FR_GUID = PR.RP_GUID
			WHERE PR.C_GUID=@C_GUID
			AND (PR.RP_Flag = @Flag or @Flag IS NULL OR LEN(@Flag) = 0)
			AND (@ClassifyFlag IS NULL OR (@ClassifyFlag = 1 AND (CFItemGuid IS NOT NULL OR CFPItemGuid IS NOT NULL)) OR (@ClassifyFlag = 0 AND CFItemGuid IS NULL AND CFPItemGuid IS NULL))
			AND(PR.Date >= @dateBegin OR @dateBegin IS NULL)
			AND(PR.Date < DATEADD(day,1,@dateEnd) OR @dateEnd IS NULL)
			AND(PR.RPer = @customer OR @customer IS NULL OR LEN(@customer) = 0)
			AND(PR.InvType = @incomeGrp OR @incomeGrp IS NULL OR LEN(@incomeGrp)=0)
			AND(PR.InvTypeDts = @incomeGrpdts OR @incomeGrpdts IS NULL OR LEN(@incomeGrpdts)=0)
		END
		ELSE
		BEGIN
		insert into @temp
			SELECT row_number()over(order by PR.Date desc) rownumber,PR.RP_GUID,PR.InvType,PR.InvNo,PR.RPer,PR.Amount,PR.Date,PR.Remark,PR.Creator,PR.CreateDate
			,PR.DebitLedgerAccount,PR.DebitDetailsAccount,PR.CreditLedgerAccount,PR.CreditDetailsAccount,BP.Name R_PerName,
			LA2.Name AS DebitLedgerAccountName,LA1.Name AS CreditLedgerAccountName
			FROM dbo.V_RecPayRecord PR
			LEFT JOIN dbo.T_BusinessPartner BP ON PR.RPer = BP.BP_GUID
			LEFT JOIN dbo.T_GeneralLedgerAccount LA1 ON LA1.LA_GUID = PR.CreditLedgerAccount
			LEFT JOIN dbo.T_GeneralLedgerAccount LA2 ON LA2.LA_GUID = PR.DebitLedgerAccount
			WHERE (PR.RP_Flag = @Flag or @Flag IS NULL OR LEN(@Flag) = 0) 
			AND PR.C_GUID=@C_GUID;
			
		END
		SELECT @Count = COUNT(*) FROM @temp;
		SELECT T.RP_GUID,T.IE_GUID,T.InvType,T.InvTypeDts,T.InvNo,T.RPer,T.SumAmount,T.Date,T.Remark,T.Creator,T.CreateDate
			,T.DebitLedgerAccount,T.DebitDetailsAccount,T.CreditLedgerAccount,T.CreditDetailsAccount,T.R_PerName,T.DebitLedgerAccountName,T.CreditLedgerAccountName
			,Currency ,BA_GUID ,BankAccount,T.A_GUID 
		FROM @temp T
		WHERE (T.rownumber>= ((@PageIndex - 1)*@PageSize) + 1
		AND T.rownumber <= @PageIndex*@PageSize )
		OR (@PageSize = -1)
	END
	ELSE
	BEGIN
		IF EXISTS (SELECT RP_GUID FROM dbo.T_RecPayRecord PR WHERE PR.RP_GUID = @ID)
		BEGIN
			SELECT PR.RP_GUID,PR.InvType,PR.InvNo,PR.RPer,PR.SumAmount,PR.Date,PR.Remark,PR.Creator,PR.CreateDate,PR.Currency,PR.CFItemGuid,PR.CFPItemGuid,
			PR.DebitLedgerAccount,LA2.Name AS DebitLedgerAccountName,
			PR.DebitDetailsAccount,DA2.Name AS DebitDetailsAccountName,
			PR.CreditLedgerAccount,LA1.Name AS CreditLedgerAccountName,
			PR.CreditDetailsAccount,DA1.Name AS CreditDetailsAccountName,
			BP.Name R_PerName,IT.Name TypeName,CFI1.Name AS CFItemName,CFI2.Name AS CFPItemName,
			PR.Currency,PR.B_GUID,PR.BA_GUID,BA.Account AS BankAccount
			FROM dbo.T_RecPayRecord PR
			LEFT JOIN dbo.T_BusinessPartner BP ON PR.RPer = BP.BP_GUID
			LEFT JOIN dbo.T_GeneralLedgerAccount LA1 ON LA1.LA_GUID = PR.CreditLedgerAccount
			LEFT JOIN dbo.T_GeneralLedgerAccount LA2 ON LA2.LA_GUID = PR.DebitLedgerAccount
			LEFT JOIN dbo.T_DetailedAccount DA1 ON DA1.DA_GUID = PR.CreditDetailsAccount
			LEFT JOIN dbo.T_DetailedAccount DA2 ON DA2.DA_GUID = PR.DebitDetailsAccount
			LEFT JOIN dbo.T_InvType IT ON PR.InvType=IT.[Key]
			LEFT JOIN dbo.T_CashFlowItem CFI1 ON PR.CFItemGuid=CFI1.R_GUID
			LEFT JOIN dbo.T_CashFlowItem CFI2 ON PR.CFPItemGuid=CFI2.R_GUID
			LEFT JOIN dbo.T_BankAccount BA ON BA.BA_GUID = PR.BA_GUID
			WHERE PR.RP_GUID = @ID 
		END
		ELSE
		BEGIN
			SELECT PR.RP_GUID,PR.InvType,PR.InvNo,PR.R_PerName,PR.SumAmount,PR.Date,PR.Remark,PR.Creator,PR.CreateDate,PR.Currency,PR.TypeName,PR.CFItemGuid,PR.CFPItemGuid,PR.CFItemName,PR.CFPItemName
			,PR.DebitLedgerAccount AS DebitLedgerAccountName,
			PR.DebitDetailsAccount AS DebitDetailsAccountName,
			PR.CreditLedgerAccount AS CreditLedgerAccountName,
			PR.CreditDetailsAccount AS CreditDetailsAccountName
			FROM dbo.V_RerPayRecord PR
			WHERE PR.RP_GUID = @ID 
		END
	END
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetRecPayRecord]    Script Date: 06/02/2016 14:21:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	获取收付款记录
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetRecPayRecord]
	@PageSize int = -1,
	@PageIndex int = 1,
	@Count int = 0 out,
	@ID NVARCHAR(40) = NULL,
	@Flag NVARCHAR(1) = NULL,
	@C_GUID NVARCHAR(50),
	@IsAll bit = 0,
	@ClassifyFlag bit = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @temp TABLE(
		rownumber int,
		RP_GUID NVARCHAR(40),
		InvType nvarchar(20),
		InvNo nvarchar(20),
		R_Per nvarchar(40),
		SumAmount decimal(18,2),
		Date date,
		Remark nvarchar(200),
		Creator  nvarchar(40),
		CreateDate datetime,
		DebitLedgerAccount nvarchar(40),
		DebitDetailsAccount nvarchar(40),
		CreditLedgerAccount nvarchar(40),
		CreditDetailsAccount nvarchar(40),
		R_PerName nvarchar(100),
		DebitLedgerAccountName nvarchar(40),
		CreditLedgerAccountName nvarchar(40),
		Currency NVARCHAR(5),
		B_GUID NVARCHAR(40),
		BA_GUID NVARCHAR(40),
		BankAccount NVARCHAR(100)
	 )
	 
    -- Insert statements for procedure here
    IF (@ID IS NULL)
    BEGIN
		IF(@IsAll = 0)
		BEGIN
		insert into @temp
			SELECT row_number()over(order by PR.Date desc) rownumber,PR.RP_GUID,PR.InvType,PR.InvNo,PR.RPer,PR.SumAmount,PR.Date,PR.Remark,PR.Creator,PR.CreateDate
			,PR.DebitLedgerAccount,PR.DebitDetailsAccount,PR.CreditLedgerAccount,PR.CreditDetailsAccount,BP.Name R_PerName,
			LA2.Name AS DebitLedgerAccountName,LA1.Name AS CreditLedgerAccountName,
			PR.Currency,PR.B_GUID,PR.BA_GUID,BA.Account AS BankAccount
			FROM dbo.T_RecPayRecord PR
			LEFT JOIN dbo.T_BusinessPartner BP ON PR.RPer = BP.BP_GUID
			LEFT JOIN dbo.T_GeneralLedgerAccount LA1 ON LA1.LA_GUID = PR.CreditLedgerAccount
			LEFT JOIN dbo.T_GeneralLedgerAccount LA2 ON LA2.LA_GUID = PR.DebitLedgerAccount
			LEFT JOIN dbo.T_BankAccount BA ON BA.BA_GUID = PR.BA_GUID
			WHERE PR.C_GUID=@C_GUID
			AND (PR.RP_Flag = @Flag or @Flag IS NULL OR LEN(@Flag) = 0)
			AND (@ClassifyFlag IS NULL OR (@ClassifyFlag = 1 AND (CFItemGuid IS NOT NULL OR CFPItemGuid IS NOT NULL)) OR (@ClassifyFlag = 0 AND CFItemGuid IS NULL AND CFPItemGuid IS NULL));
			
		END
		ELSE
		BEGIN 
		insert into @temp
			SELECT row_number()over(order by PR.Date desc) rownumber,PR.RP_GUID,PR.InvType,PR.InvNo,PR.RPer,PR.SumAmount,PR.Date,PR.Remark,PR.Creator,PR.CreateDate
			,PR.DebitLedgerAccount,PR.DebitDetailsAccount,PR.CreditLedgerAccount,PR.CreditDetailsAccount,BP.Name R_PerName,
			LA2.Name AS DebitLedgerAccountName,LA1.Name AS CreditLedgerAccountName,
			PR.Currency,PR.B_GUID,PR.BA_GUID,BA.Account AS BankAccount
			FROM dbo.T_RecPayRecord PR
			LEFT JOIN dbo.T_BusinessPartner BP ON PR.RPer = BP.BP_GUID
			LEFT JOIN dbo.T_GeneralLedgerAccount LA1 ON LA1.LA_GUID = PR.CreditLedgerAccount
			LEFT JOIN dbo.T_GeneralLedgerAccount LA2 ON LA2.LA_GUID = PR.DebitLedgerAccount
			LEFT JOIN dbo.T_BankAccount BA ON BA.BA_GUID = PR.BA_GUID
			WHERE (PR.RP_Flag = @Flag or @Flag IS NULL OR LEN(@Flag) = 0) 
			AND PR.C_GUID=@C_GUID;
			
		END
		SELECT @Count = COUNT(*) FROM @temp;
		SELECT T.RP_GUID,T.InvType,T.InvNo,T.R_Per,T.SumAmount,T.Date,T.Remark,T.Creator,T.CreateDate
			,T.DebitLedgerAccount,T.DebitDetailsAccount,T.CreditLedgerAccount,T.CreditDetailsAccount,T.R_PerName,T.DebitLedgerAccountName,T.CreditLedgerAccountName
			,Currency ,BA_GUID ,BankAccount 
		FROM @temp T
		WHERE (T.rownumber>= ((@PageIndex - 1)*@PageSize) + 1
		AND T.rownumber <= @PageIndex*@PageSize )
		OR (@PageSize = -1)
	END
	ELSE
	BEGIN
		IF EXISTS (SELECT RP_GUID FROM dbo.T_RecPayRecord PR WHERE PR.RP_GUID = @ID)
		BEGIN
			SELECT PR.RP_GUID,PR.InvType,PR.InvNo,PR.RPer,PR.SumAmount,PR.Date,PR.Remark,PR.Creator,PR.CreateDate,PR.Currency,PR.CFItemGuid,PR.CFPItemGuid,
			PR.DebitLedgerAccount,LA2.Name AS DebitLedgerAccountName,
			PR.DebitDetailsAccount,DA2.Name AS DebitDetailsAccountName,
			PR.CreditLedgerAccount,LA1.Name AS CreditLedgerAccountName,
			PR.CreditDetailsAccount,DA1.Name AS CreditDetailsAccountName,
			BP.Name R_PerName,IT.Name TypeName,CFI1.Name AS CFItemName,CFI2.Name AS CFPItemName,
			PR.Currency,PR.B_GUID,PR.BA_GUID,BA.Account AS BankAccount
			FROM dbo.T_RecPayRecord PR
			LEFT JOIN dbo.T_BusinessPartner BP ON PR.RPer = BP.BP_GUID
			LEFT JOIN dbo.T_GeneralLedgerAccount LA1 ON LA1.LA_GUID = PR.CreditLedgerAccount
			LEFT JOIN dbo.T_GeneralLedgerAccount LA2 ON LA2.LA_GUID = PR.DebitLedgerAccount
			LEFT JOIN dbo.T_DetailedAccount DA1 ON DA1.DA_GUID = PR.CreditDetailsAccount
			LEFT JOIN dbo.T_DetailedAccount DA2 ON DA2.DA_GUID = PR.DebitDetailsAccount
			LEFT JOIN dbo.T_InvType IT ON PR.InvType=IT.[Key]
			LEFT JOIN dbo.T_CashFlowItem CFI1 ON PR.CFItemGuid=CFI1.R_GUID
			LEFT JOIN dbo.T_CashFlowItem CFI2 ON PR.CFPItemGuid=CFI2.R_GUID
			LEFT JOIN dbo.T_BankAccount BA ON BA.BA_GUID = PR.BA_GUID
			WHERE PR.RP_GUID = @ID 
		END
		ELSE
		BEGIN
			SELECT PR.RP_GUID,PR.InvType,PR.InvNo,PR.R_PerName,PR.SumAmount,PR.Date,PR.Remark,PR.Creator,PR.CreateDate,PR.Currency,PR.TypeName,PR.CFItemGuid,PR.CFPItemGuid,PR.CFItemName,PR.CFPItemName
			,PR.DebitLedgerAccount AS DebitLedgerAccountName,
			PR.DebitDetailsAccount AS DebitDetailsAccountName,
			PR.CreditLedgerAccount AS CreditLedgerAccountName,
			PR.CreditDetailsAccount AS CreditDetailsAccountName
			FROM dbo.V_RerPayRecord PR
			WHERE PR.RP_GUID = @ID 
		END
	END
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetRecord]    Script Date: 06/02/2016 14:21:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<魏礼明>
-- Create date: <2015/05/07>
-- Description:	<获取应收/付账款项>
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetRecord]
	-- Add the parameters for the stored procedure here
	@ID nvarchar(40),
	@C_GUID nvarchar(40),
	@Flag nvarchar(4)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    IF(@Flag='I')
    BEGIN
	select * from V_Receivable where R_GUID=@ID and C_GUID=@C_GUID
	END
	IF(@Flag='E')
	BEGIN
	select * from V_Payable where R_GUID=@ID and C_GUID=@C_GUID
	END
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetIEs]    Script Date: 06/02/2016 14:21:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	获取收入费用
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetIEs]
@PageSize int = -1,
@PageIndex int = 1,
@Count int = 0 out,
@ID NVARCHAR(40)= NULL,
@Flag NVARCHAR(1) = NULL,
@C_GUID NVARCHAR(50),
@IsAll bit = 0,
@dateBegin DATETIME = NULL,
@dateEnd DATETIME = NULL,
@zdateBegin DATETIME = NULL,
@zdateEnd DATETIME = NULL,
@customer NVARCHAR(40) = NULL,
@state NVARCHAR(40) = NULL,
@incomeGrp NVARCHAR(20)=NULL,
@currency NVARCHAR(20)=NULL,
@IncomeGrpDts NVARCHAR(50)=NULL

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    IF @ID IS NULL
    BEGIN
     DECLARE @temp TABLE(
		rownumber int,
		IE_GUID NVARCHAR(50),
		Amount  decimal(18, 4),
		SumAmount DECIMAL(18,4),
		Currency NVARCHAR(20),
		CreateDate datetime,
		Creator  nvarchar(40),
		Date date,
		AffirmDate DATE,
		InvNo nvarchar(20),
		InvType nvarchar(20),
		RPer nvarchar(40),
		Tax decimal(18, 4),
		RPerName nvarchar(100),
		Remark NVARCHAR(500),
		IEGroup NVARCHAR(40),
		IEDescription NVARCHAR(500),
		State NVARCHAR(40),
		A_GUID NVARCHAR(50)
	 )
	 
	 DECLARE @temp1 TABLE(
		FR_GUID NVARCHAR(50)
	 )
	 
	 INSERT INTO @temp1
	 select FR_GUID from dbo.T_Attachment group by FR_GUID
	 
	 
		IF(@IsAll = 0)
		BEGIN
		insert into @temp
			SELECT row_number()over(order by IER.AffirmDate desc) rownumber,IER.IE_GUID,IER.Amount,IER.SumAmount,
			IER.Currency,IER.CreateDate,IER.Creator,IER.Date,IER.AffirmDate,IER.InvNo,IER.InvType,IER.RPer,IER.TaxationAmount Tax,
			BP.Name RPerName,IER.Remark,IER.IEGroup,IER.IEDescription,IER.State,TA.A_GUID
			FROM dbo.T_IERecord IER
			LEFT JOIN dbo.T_BusinessPartner BP ON BP.BP_GUID = IER.RPer
			LEFT JOIN dbo.T_Attachment TA ON TA.FR_GUID = IER.IE_GUID
			WHERE (IER.IE_Flag = @Flag OR @Flag IS NULL OR LEN(@Flag) = 0) 
			AND IER.C_GUID=@C_GUID;
			
		END
		ELSE
		BEGIN
		insert into @temp
			SELECT  row_number()over(order by V_IER.AffirmDate desc) rownumber,V_IER.IE_GUID,V_IER.Amount,
			V_IER.SumAmount,V_IER.Currency,V_IER.CreateDate,V_IER.Creator,V_IER.Date,V_IER.AffirmDate,
			V_IER.InvNo,V_IER.InvType,V_IER.RPer,V_IER.TaxationAmount Tax
			,V_IER.BPName RPerName,V_IER.Remark,V_IER.IEGroup,V_IER.IEDescription,V_IER.State,TA.FR_GUID as A_GUID
			FROM V_IERecords V_IER
			LEFT JOIN dbo.V_IEDetails V_IED ON V_IED.IE_GUID = V_IER.IE_GUID 
			LEFT JOIN @temp1 TA ON TA.FR_GUID = V_IER.IE_GUID 
			WHERE (V_IER.IE_Flag = @Flag OR @Flag IS NULL OR LEN(@Flag) = 0)
			AND V_IER.C_GUID=@C_GUID
			AND(V_IER.AffirmDate >= @dateBegin OR @dateBegin IS NULL)
			AND(V_IER.AffirmDate < DATEADD(day,1,@dateEnd) OR @dateEnd IS NULL)
			AND(V_IER.Date >= @zdateBegin OR @zdateBegin IS NULL)
			AND(V_IER.Date < DATEADD(day,1,@zdateEnd) OR @zdateEnd IS NULL)
			AND(V_IER.RPer = @customer OR @customer IS NULL OR LEN(@customer) = 0)
			AND (V_IER.State=@state OR @state IS NULL OR LEN(@state) = 0)
			AND(V_IER.InvType = @incomeGrp OR @incomeGrp IS NULL OR LEN(@incomeGrp)=0)
			AND(V_IER.Currency = @currency OR @currency IS NULL OR LEN(@currency)=0)
			AND(V_IER.IEGroup = @IncomeGrpDts OR @IncomeGrpDts IS NULL OR LEN(@IncomeGrpDts)=0)
		END
		
		SELECT @Count = COUNT(*) FROM @temp;

		SELECT  T.IE_GUID,T.Amount,T.SumAmount,T.Currency,T.CreateDate,T.Creator ,T.Date,AffirmDate ,T.InvNo,
		T.InvType,T.RPer,T.Tax,T.RPerName,T.Remark,T.IEGroup,T.IEDescription,T.State,T.A_GUID
		FROM @temp T
		WHERE (T.rownumber>= ((@PageIndex - 1)*@PageSize) + 1
		AND T.rownumber <= @PageIndex*@PageSize )
		OR (@PageSize = -1)

	END
	ELSE
	BEGIN
		IF EXISTS (SELECT IE_GUID FROM dbo.T_IERecord IER WHERE IE_GUID = @ID)
		BEGIN
			SELECT IER.IE_GUID,IER.InvType,IER.InvNo,IER.RPer,IER.C_GUID,IER.AffirmDate,IER.Date,IER.Amount,IER.TaxationAmount,IER.TaxationType,IER.SumAmount,IER.Currency,IER.Remark,IER.B_GUID,IER.BA_GUID
			,BP.Name RPerName,IT.Name TypeName,IER.IEGroup,IER.IEDescription,IER.State
			FROM dbo.T_IERecord IER
			LEFT JOIN dbo.T_BusinessPartner BP ON BP.BP_GUID = IER.RPer
			LEFT JOIN dbo.T_InvType IT ON IER.InvType=IT.[Key]
			WHERE IER.IE_GUID = @ID
		END
		ELSE
		BEGIN
			SELECT V_IER.IE_GUID,V_IER.Amount,V_IER.CreateDate,V_IER.Creator,V_IER.Date,V_IER.InvNo,V_IER.InvType,V_IER.RPer,V_IER.TaxationAmount
			,V_IER.TypeName,V_IER.BPName,V_IER.IEGroup,V_IER.IEDescription,V_IER.State
			FROM V_IERecords V_IER
			WHERE V_IER.IE_GUID = @ID 
		END
	END
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetIEDetails]    Script Date: 06/02/2016 14:21:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	获取收入费用明细
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetIEDetails]
	@IE_ID NVARCHAR(40)=NULL,
	@C_GUID NVARCHAR(50),
	@IsAll bit = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF (@IsAll = 0)
    BEGIN
		SELECT *,
		LA1.Name AS CreditLedgerAccountName,
		LA2.Name AS DebitLedgerAccountName,
		DA1.Name AS CreditDetailsAccountName,
		DA2.Name AS DebitDetailsAccountName
		FROM dbo.T_IEDetails D
		LEFT JOIN dbo.T_GeneralLedgerAccount LA1 ON LA1.LA_GUID = D.CreditLedgerAccount
		LEFT JOIN dbo.T_GeneralLedgerAccount LA2 ON LA2.LA_GUID = D.DebitLedgerAccount
		LEFT JOIN dbo.T_DetailedAccount DA1 ON DA1.DA_GUID = D.CreditDetailsAccount
		LEFT JOIN dbo.T_DetailedAccount DA2 ON DA2.DA_GUID = D.DebitDetailsAccount
		WHERE (D.IE_GUID = @IE_ID OR @IE_ID IS NULL OR LEN(@IE_ID)= 0) 
		AND D.C_GUID=@C_GUID
	END
	ELSE
	BEGIN
		SELECT D.IE_GUID,D.Money,D.C_GUID,
		D.DebitLedgerAccount AS DebitLedgerAccountName,
		D.DebitDetailsAccount AS DebitDetailsAccountName,
		D.CreditLedgerAccount AS CreditLedgerAccountName,
		D.CreditDetailsAccount AS CreditDetailsAccountName
		FROM dbo.V_IEDetails D
		WHERE (D.IE_GUID = @IE_ID OR @IE_ID IS NULL OR LEN(@IE_ID)= 0)
		AND D.C_GUID = @C_GUID
	END
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetChooseReceivablesRecord]    Script Date: 06/02/2016 14:21:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<魏礼明>
-- Create date: <2015/05/04>
-- Description:	<查询所有应收账款>
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetChooseReceivablesRecord]
	-- Add the parameters for the stored procedure here
	@PageSize int = -1,
	@PageIndex int = 1,
	@Count int = 0 out,
	@RPer nvarchar(40)=null,
	@C_GUID nvarchar(40),
	@R_GUID nvarchar(40)=null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
   
    DECLARE @temp Table(
    rownumber int,
    R_GUID nvarchar(40),
    AffirmDate datetime,
    Payer nvarchar(40),
    InvNo nvarchar(20),
    InvType nvarchar(50),
    Money decimal(18,2),
    Currency nvarchar(5),
    Date datetime,
    Remark nvarchar(200),
    RPerName nvarchar(50)
   
    )
        
    insert into @temp
	select row_number()over(order by VR.Date desc) rownumber,VR.R_GUID,VR.AffirmDate,VR.Payer,VR.InvNo,VR.InvType,VR.SumAmount Money,
	VR.Currency,VR.Date,VR.Remark,VR.RPerName
	from dbo.V_Receivable VR
	where (VR.Payer=@RPer or @RPer is null or LEN(@RPer)=0) 
	and VR.C_GUID=@C_GUID 
	and VR.State='应收' 
	and (VR.InvType='主营业务收入' or VR.InvType='非主营业务收入')
	and (VR.R_GUID=@R_GUID or @R_GUID is null or LEN(@R_GUID)=0)
	
	SELECT @Count = COUNT(*) FROM @temp;
	SELECT T.R_GUID,T.AffirmDate,T.Currency,T.InvNo,T.Date,T.InvType,T.Money,T.Payer,T.Remark,T.RPerName
		FROM @temp T
		WHERE (T.rownumber>= ((@PageIndex - 1)*@PageSize) + 1
		AND T.rownumber <= @PageIndex*@PageSize )
		OR (@PageSize = -1)

	--SELECT * FROM dbo.V_Receivable
	--where C_GUID=@C_GUID and R_GUID=@R_GUID;
	
	--SELECT @Count = COUNT(*) FROM dbo.V_Receivable;
	
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetChoosePayablesRecord]    Script Date: 06/02/2016 14:21:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<魏礼明>
-- Create date: <2015/05/08>
-- Description:	<查询所有应付账款>
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetChoosePayablesRecord]
	-- Add the parameters for the stored procedure here
	@PageSize int = -1,
	@PageIndex int = 1,
	@Count int = 0 out,
	@RPer nvarchar(40)=null,
	@IEGroup nvarchar(40)=null,
	@C_GUID nvarchar(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	

    -- Insert statements for procedure here
      DECLARE @temp Table(
    rownumber int,
    R_GUID nvarchar(40),
    AffirmDate datetime,
    Payer nvarchar(40),
    InvNo nvarchar(20),
    InvType NVARCHAR(50),
    Money decimal(18,2),
    Currency nvarchar(5),
    Date datetime,
    Remark nvarchar(200),
    RPerName nvarchar(50)
   
    )
	
	insert into @temp
	select row_number()over(order by VP.Date desc) rownumber,VP.R_GUID,VP.AffirmDate,VP.Payer,VP.InvNo,VP.InvType,VP.SumAmount Money,
	VP.Currency,VP.Date,VP.Remark,VP.RPerName
	from dbo.V_Payable VP
	where (VP.Payer=@RPer or @RPer is null or LEN(@RPer)=0) 
	and VP.C_GUID=@C_GUID and VP.State='应付' 
	and (VP.IEGroup=@IEGroup or @IEGroup is null or LEN(@IEGroup)=0) 
	
	SELECT @Count = COUNT(*) FROM @temp;
	SELECT T.R_GUID,T.AffirmDate,T.Currency,T.Date,T.InvNo,T.InvType,T.Money,T.Payer,T.Remark,T.RPerName
		FROM @temp T
		WHERE (T.rownumber>= ((@PageIndex - 1)*@PageSize) + 1
		AND T.rownumber <= @PageIndex*@PageSize )
		OR (@PageSize = -1)	
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetAssetses]    Script Date: 06/02/2016 14:21:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	获取固定资产
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetAssetses]
	@ID NVARCHAR(40) = NULL,
	@PageSize INT = 10,
	@PageIndex INT = 0,
	@Flag INT = 0, --状态标志：0：所有，1：正在使用，2：可出售,3：未使用
	@SummaryDate datetime = null,
	@TotalCount INT = 0 OUT ,
	@C_GUID NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	    DECLARE @temp TABLE(
		rownumber int,
		A_GUID NVARCHAR(50),
		[No]  NVARCHAR(100),
		Name  NVARCHAR(100),
		PurchaseDate DATETIME,
		RegisterDate DATETIME,
		ScrapType NVARCHAR(10),
		ScrapDate DATETIME,
		AG_GUID NVARCHAR(50),
		AssetsCost DECIMAL(18,4),
		Creator NVARCHAR(50),
		C_GUID NVARCHAR(50),
		CurrentValue DECIMAL(18,4)
		)
	
	IF	(@Flag = 0)
	BEGIN
		INSERT INTO @temp
		SELECT row_number()over(order by A.NO ) rownumber,
		A.A_GUID,A.No,A.Name,A.PurchaseDate,A.RegisterDate,A.ScrapType,A.ScrapDate,A.AG_GUID,A.AssetsCost,A.Creator,A.C_GUID,
        dbo.FUN_Depreciation(A.AssetsCost,A.AssetsCost * AG.SalvageRate / 100,AG.DepreciationMethod,AG.Life,A.RegisterDate,ISNULL(A.ScrapDate,ISNULL(@SummaryDate,SYSDATETIME())))
		FROM dbo.T_Assets A
		LEFT JOIN dbo.T_AssetsGroup AG ON AG.AG_GUID = A.AG_GUID
		WHERE (A.A_GUID = @ID OR @ID IS NULL OR LEN(@ID)=0) AND A.C_GUID=@C_GUID;
	END
	ELSE IF (@Flag = 1)
	BEGIN
		INSERT INTO @temp
		SELECT row_number()over(order by A.NO ) rownumber,
		A.A_GUID,A.No,A.Name,A.PurchaseDate,A.RegisterDate,A.ScrapType,A.ScrapDate,A.AG_GUID,A.AssetsCost,A.Creator,A.C_GUID,0
		FROM dbo.T_Assets A
		WHERE A.ScrapDate IS NULL AND 
		(A.A_GUID = @ID OR @ID IS NULL OR LEN(@ID)=0) AND A.C_GUID=@C_GUID;
	END
	ELSE IF (@Flag = 2)
	BEGIN
		INSERT INTO @temp
		SELECT row_number()over(order by A.NO ) rownumber,
		A.A_GUID,A.No,A.Name,A.PurchaseDate,A.RegisterDate,A.ScrapType,A.ScrapDate,A.AG_GUID,A.AssetsCost,A.Creator,A.C_GUID,0
		FROM dbo.T_Assets A
		WHERE (A.ScrapType = 'Scrap' OR A.ScrapType IS NULL OR LEN(A.ScrapType)=0)   
		AND (A.A_GUID = @ID OR @ID IS NULL OR LEN(@ID)=0) AND A.C_GUID=@C_GUID;
	END
	
	
	SELECT @TotalCount = COUNT(*) FROM @temp;
	
	SELECT T.A_GUID,T.AG_GUID,T.AssetsCost,T.Name,T.No,T.PurchaseDate,T.RegisterDate,T.ScrapDate,T.ScrapType,T.CurrentValue
	FROM @temp T
	WHERE (T.rownumber>= ((@PageIndex - 1)*@PageSize) + 1
	AND T.rownumber <= @PageIndex*@PageSize)
	OR (@PageIndex = 0);
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetAccountSummaryDetails]    Script Date: 06/02/2016 14:21:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	获取科目汇总明细
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetAccountSummaryDetails]
	@C_ID NVARCHAR(40),
	@AccID NVARCHAR(40) = Null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE  @TmpFR TABLE
	(
		Acc_GUID NVARCHAR(40),
		Value DECIMAL(18,2),
		Date DATE,
		AccGrp INT
	)
	
	IF @AccID IS NOT NULL
	BEGIN
		DECLARE @CurrentRepBeginDate DATE;
		DECLARE @CurrentRepEndDate DATE;
		
		DECLARE @TmpV_PR TABLE
		(
			DebitLedgerAccount NVARCHAR(40),
			DebitDetailsAccount NVARCHAR(40),
			DebitAccGroup INT,
			CreditLedgerAccount NVARCHAR(40),
			CreditDetailsAccount NVARCHAR(40),
			CreditAccGroup INT,
			Date DATE,
			Money DECIMAL(18,2)
		);
	    
		INSERT INTO @TmpV_PR(DebitLedgerAccount,DebitDetailsAccount,DebitAccGroup,CreditLedgerAccount,CreditDetailsAccount,CreditAccGroup,Money,Date)
		SELECT FR.DebitLedgerAccount,FR.DebitDetailsAccount,FR.DebitAccGroup,
			   FR.CreditLedgerAccount,FR.CreditDetailsAccount,FR.CreditAccGroup,FR.Money,FR.Date
		FROM dbo.V_FinanceRecord FR
		WHERE FR.C_GUID = @C_ID ;--AND DATE BETWEEN @CurrentRepBeginDate AND @CurrentRepEndDate;
	    
		INSERT INTO @TmpFR(Acc_GUID,Value,AccGrp,Date)
		SELECT ISNULL(FR.DebitDetailsAccount,FR.DebitLedgerAccount) ,FR.Money ,FR.DebitAccGroup,FR.Date
		FROM @TmpV_PR FR
		WHERE  FR.DebitAccGroup = 1 ;
	    
		INSERT INTO @TmpFR(Acc_GUID,Value,AccGrp,Date)
		SELECT ISNULL(FR.CreditDetailsAccount,FR.CreditLedgerAccount) ,(0-FR.Money) , FR.CreditAccGroup ,FR.Date
		FROM @TmpV_PR FR
		WHERE FR.CreditAccGroup = 1 ;
	    
		INSERT INTO @TmpFR(Acc_GUID,Value,AccGrp,Date)
		SELECT ISNULL(FR.DebitDetailsAccount,FR.DebitLedgerAccount),(0-FR.Money),FR.DebitAccGroup,FR.Date
		FROM @TmpV_PR FR
		WHERE  FR.DebitAccGroup = 2 OR FR.DebitAccGroup = 4 
		OR FR.DebitAccGroup = 7 OR FR.DebitAccGroup = 6 OR FR.DebitAccGroup = 5;
	    
		INSERT INTO @TmpFR(Acc_GUID,Value,AccGrp,Date)
		SELECT ISNULL(FR.CreditDetailsAccount,FR.CreditLedgerAccount),FR.Money , FR.CreditAccGroup ,FR.Date
		FROM @TmpV_PR FR
		WHERE FR.CreditAccGroup = 2  OR FR.CreditAccGroup = 4 
		OR FR.CreditAccGroup = 7 OR FR.CreditAccGroup = 6 OR FR.CreditAccGroup = 5;
	    
		SELECT Date,Value AS Amount
		FROM @TmpFR
		WHERE Acc_GUID = @AccID
		Order by Date;
	END
	ELSE
	BEGIN
		SELECT Date,Value AS Amount
		FROM @TmpFR
	END
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetAccountSummary]    Script Date: 06/02/2016 14:21:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	获取科目汇总数据
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetAccountSummary]
	@C_ID NVARCHAR(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @CurrentRepBeginDate DATE;
	DECLARE @CurrentRepEndDate DATE;
	
    DECLARE @TmpV_PR TABLE
    (
		DebitLedgerAccount NVARCHAR(40),
		DebitDetailsAccount NVARCHAR(40),
		DebitAccGroup INT,
		CreditLedgerAccount NVARCHAR(40),
		CreditDetailsAccount NVARCHAR(40),
		CreditAccGroup INT,
		Money DECIMAL(18,2)
    );
    
    INSERT INTO @TmpV_PR(DebitLedgerAccount,DebitDetailsAccount,DebitAccGroup,CreditLedgerAccount,CreditDetailsAccount,CreditAccGroup,Money)
    SELECT FR.DebitLedgerAccount,FR.DebitDetailsAccount,FR.DebitAccGroup,
		   FR.CreditLedgerAccount,FR.CreditDetailsAccount,FR.CreditAccGroup,FR.Money
    FROM dbo.V_FinanceRecord FR
    WHERE FR.C_GUID = @C_ID ;--AND DATE BETWEEN @CurrentRepBeginDate AND @CurrentRepEndDate;
    
    DECLARE  @TmpFR TABLE
    (
		Acc_GUID NVARCHAR(40),
        Value DECIMAL(18,2),
        AccGrp INT
    )
    
    INSERT INTO @TmpFR(Acc_GUID,Value,AccGrp)
    SELECT ISNULL(FR.DebitDetailsAccount,FR.DebitLedgerAccount) ,FR.Money ,FR.DebitAccGroup 
    FROM @TmpV_PR FR
    WHERE  FR.DebitAccGroup = 1 ;
    
    INSERT INTO @TmpFR(Acc_GUID,Value,AccGrp)
    SELECT ISNULL(FR.CreditDetailsAccount,FR.CreditLedgerAccount) ,(0-FR.Money) , FR.CreditAccGroup 
    FROM @TmpV_PR FR
    WHERE FR.CreditAccGroup = 1 ;
    
    INSERT INTO @TmpFR(Acc_GUID,Value,AccGrp)
    SELECT ISNULL(FR.DebitDetailsAccount,FR.DebitLedgerAccount),(0-FR.Money),FR.DebitAccGroup
    FROM @TmpV_PR FR
    WHERE  FR.DebitAccGroup = 2 OR FR.DebitAccGroup = 4 
    OR FR.DebitAccGroup = 7 OR FR.DebitAccGroup = 6 OR FR.DebitAccGroup = 5;
    
    INSERT INTO @TmpFR(Acc_GUID,Value,AccGrp)
    SELECT ISNULL(FR.CreditDetailsAccount,FR.CreditLedgerAccount),FR.Money , FR.CreditAccGroup 
    FROM @TmpV_PR FR
    WHERE FR.CreditAccGroup = 2  OR FR.CreditAccGroup = 4 
    OR FR.CreditAccGroup = 7 OR FR.CreditAccGroup = 6 OR FR.CreditAccGroup = 5;
    
    SELECT CA.Acc_GUID,CA._parentId,CA.Acc_Name,CA.Acc_Code, FR.Value AS Money
    FROM dbo.V_CompanyAccounts CA
    LEFT JOIN (SELECT Acc_GUID,SUM(Value) AS Value FROM @TmpFR GROUP BY Acc_GUID) FR ON CA.Acc_GUID = FR.Acc_GUID 
    WHERE (CA.C_GUID = @C_ID OR CA.C_GUID IS NULL)
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GenPerviewBalanceSheet]    Script Date: 06/02/2016 14:21:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	生成资产负债预览表
-- =============================================
CREATE PROCEDURE [dbo].[SP_GenPerviewBalanceSheet]
    @C_ID NVarChar(40),
    @Year INT OUT,
    @Month INT OUT,
    @RepNo	NVARCHAR(40) OUT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @CurrentRepBeginDate DATE ;
	DECLARE @CurrentRepEndDate DATE ;
	
	--生成报表时间区间
	
	IF EXISTS (SELECT * FROM dbo.T_Report Rep WHERE  Rep.C_GUID = @C_ID AND Rep.Type='BS') 
	BEGIN 
		DECLARE @LastRepYear INT;
		DECLARE @LastRepMonth INT;
		
		SELECT TOP 1 @LastRepYear =Rep.[Year],@LastRepMonth = Rep.[Month]
		FROM dbo.T_Report Rep 
		WHERE  Rep.C_GUID = @C_ID AND Rep.Type='BS' 
		ORDER BY RepNo DESC;
		
		SET @CurrentRepBeginDate = DATEADD(year,@LastRepYear-1,CONVERT(DATE,'0001-1-1'));
		SET @CurrentRepBeginDate= DATEADD(month,@LastRepMonth,@CurrentRepBeginDate);	
	END
	ELSE
	BEGIN
		SELECT @CurrentRepBeginDate = ReportStartDate
		FROM dbo.T_CompanySetting
		WHERE C_GUID = @C_ID;
	END
	
	SET @Year = DATEPART(YEAR,@CurrentRepBeginDate);
	SET @Month = DATEPART(MONTH,@CurrentRepBeginDate);	
	SET @CurrentRepEndDate= DATEADD(month,1 ,@CurrentRepBeginDate);
	SET @RepNo = 'BS'+ CONVERT(NVARCHAR(8),@CurrentRepBeginDate,112);
    --生成报表数据
    DECLARE @TmpV_PR TABLE
    (
		DebitLedgerAccount NVARCHAR(40),
		DebitAccGroup INT,
		CreditLedgerAccount NVARCHAR(40),
		CreditAccGroup INT,
		Money DECIMAL(18,2)
    );
    
    INSERT INTO @TmpV_PR(DebitLedgerAccount,DebitAccGroup,CreditLedgerAccount,CreditAccGroup,Money)
    SELECT FR.DebitLedgerAccount,FR.DebitAccGroup,FR.CreditLedgerAccount,FR.CreditAccGroup,FR.Money
    FROM dbo.V_FinanceRecord FR
    WHERE FR.C_GUID = @C_ID AND DATE BETWEEN @CurrentRepBeginDate AND @CurrentRepEndDate;
    
    DECLARE  @TmpFR TABLE
    (
		ACC_GUID NVARCHAR(40),
        Value DECIMAL(18,2),
        AccGrp INT
    )
    
    INSERT INTO @TmpFR(ACC_GUID,Value,AccGrp)
    SELECT FR.DebitLedgerAccount AS ACC_GUID,FR.Money AS Value,FR.DebitAccGroup AS AccGrp
    FROM @TmpV_PR FR
    WHERE  FR.DebitAccGroup = 1 ;
    
    INSERT INTO @TmpFR(ACC_GUID,Value,AccGrp)
    SELECT FR.CreditLedgerAccount AS ACC_GUID,(0-FR.Money) AS Value, FR.CreditAccGroup AS AccGrp
    FROM @TmpV_PR FR
    WHERE FR.CreditAccGroup = 1 ;
    
    INSERT INTO @TmpFR(ACC_GUID,Value,AccGrp)
    SELECT FR.DebitLedgerAccount AS ACC_GUID,(0-FR.Money) AS Value,FR.DebitAccGroup AS AccGrp
    FROM @TmpV_PR FR
    WHERE  FR.DebitAccGroup = 2 OR FR.DebitAccGroup = 4 ;
    
    INSERT INTO @TmpFR(ACC_GUID,Value,AccGrp)
    SELECT FR.CreditLedgerAccount AS ACC_GUID,FR.Money AS Value, FR.CreditAccGroup AS AccGrp
    FROM @TmpV_PR FR
    WHERE FR.CreditAccGroup = 2  OR FR.CreditAccGroup = 4;
  
  SELECT LA.AccGroup AS AccGrp,LA.Name ,ISNULL(BB.Money,0) AS BeginningValue,(ISNULL(BB.Money,0) + ISNULL(T.CurrentValue,0)) AS EndingValue
  FROM dbo.R_CompanyAccount R_CA
  INNER JOIN dbo.T_GeneralLedgerAccount LA ON R_CA.LA_GUID = LA.LA_GUID
  LEFT JOIN dbo.T_BeginningBalance BB ON R_CA.LA_GUID = BB.Acc_GUID AND BB.C_GUID = @C_ID
  LEFT JOIN (
		SELECT AccGrp,ACC_GUID,SUM(Value) AS CurrentValue
		FROM @TmpFR
		GROUP BY AccGrp,ACC_GUID
		HAVING SUM(Value)<> 0
	) AS T ON T.ACC_GUID = R_CA.LA_GUID
	WHERE R_CA.C_GUID = @C_ID AND (LA.AccGroup = 1 OR LA.AccGroup = 2 OR LA.AccGroup = 4)
	AND (ISNULL(BB.Money,0) <> 0 OR (ISNULL(BB.Money,0) + ISNULL(T.CurrentValue,0)) <> 0)
END
GO
/****** Object:  StoredProcedure [dbo].[SP_ChooseReceivablesRecord]    Script Date: 06/02/2016 14:21:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<魏礼明>
-- Create date: <2015/05/04>
-- Description:	<查询所有应收账款>
-- =============================================
CREATE PROCEDURE [dbo].[SP_ChooseReceivablesRecord]
	-- Add the parameters for the stored procedure here
	@RPer nvarchar(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select * from dbo.V_Receivable where Payer=@RPer
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdIncomeStatement]    Script Date: 06/02/2016 14:21:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	更新利润表
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdIncomeStatement]
	@C_ID NVarChar(40)
AS
BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	DECLARE @CurrentRepBeginDate DATE ;
	DECLARE @CurrentRepEndDate DATE ;
	
	--生成报表时间区间
	
	IF EXISTS (SELECT * FROM dbo.T_Report Rep WHERE  Rep.C_GUID = @C_ID AND Rep.Type='PL') 
	BEGIN 
		DECLARE @LastRepYear INT;
		DECLARE @LastRepMonth INT;
		
		SELECT TOP 1 @LastRepYear =Rep.[Year],@LastRepMonth = Rep.[Month]
		FROM dbo.T_Report Rep 
		WHERE  Rep.C_GUID = @C_ID AND Rep.Type='PL' 
		ORDER BY RepNo DESC;
		
		SET @CurrentRepBeginDate = DATEADD(year,@LastRepYear-1,CONVERT(DATE,'0001-1-1'));
		SET @CurrentRepBeginDate= DATEADD(month,@LastRepMonth,@CurrentRepBeginDate);	
	END
	ELSE
	BEGIN
		SELECT @CurrentRepBeginDate = ReportStartDate
		FROM dbo.T_CompanySetting
		WHERE C_GUID = @C_ID;
	END
	
	SET @CurrentRepEndDate= DATEADD(month,1 ,@CurrentRepBeginDate);
	
    --生成报表数据
    DECLARE @TmpV_PR TABLE
    (
		DebitLedgerAccount NVARCHAR(40),
		DebitAccGroup INT,
		CreditLedgerAccount NVARCHAR(40),
		CreditAccGroup INT,
		Money DECIMAL(18,2)
    );
    
    INSERT INTO @TmpV_PR(DebitLedgerAccount,DebitAccGroup,CreditLedgerAccount,CreditAccGroup,Money)
    SELECT FR.DebitLedgerAccount,FR.DebitAccGroup,FR.CreditLedgerAccount,FR.CreditAccGroup,FR.Money
    FROM dbo.V_FinanceRecord FR
    WHERE FR.C_GUID = @C_ID AND DATE BETWEEN @CurrentRepBeginDate AND @CurrentRepEndDate;
    
    DECLARE  @TmpFR TABLE
    (
		ACC_GUID NVARCHAR(40),
        Value DECIMAL(18,2),
        AccGrp INT
    )
    
    INSERT INTO @TmpFR(ACC_GUID,Value,AccGrp)
    SELECT FR.DebitLedgerAccount AS ACC_GUID,(0-FR.Money) AS Value,FR.DebitAccGroup AS AccGrp
    FROM @TmpV_PR FR
    WHERE  FR.DebitAccGroup = 6 ;
    
    INSERT INTO @TmpFR(ACC_GUID,Value,AccGrp)
    SELECT FR.CreditLedgerAccount AS ACC_GUID,FR.Money AS Value, FR.CreditAccGroup AS AccGrp
    FROM @TmpV_PR FR
    WHERE FR.CreditAccGroup = 6 ;
    
    INSERT INTO @TmpFR(ACC_GUID,Value,AccGrp)
    SELECT FR.DebitLedgerAccount AS ACC_GUID,(0-FR.Money) AS Value,FR.DebitAccGroup AS AccGrp
    FROM @TmpV_PR FR
    WHERE  FR.DebitAccGroup = 7 ;
    
    INSERT INTO @TmpFR(ACC_GUID,Value,AccGrp)
    SELECT FR.CreditLedgerAccount AS ACC_GUID,FR.Money AS Value, FR.CreditAccGroup AS AccGrp
    FROM @TmpV_PR FR
    WHERE FR.CreditAccGroup = 7;

	DECLARE @repID NVARCHAR(40) = NEWID();
	DECLARE @repNo	NVARCHAR(40) = 'PL'+ CONVERT(NVARCHAR(8),@CurrentRepBeginDate,112);
	
	IF NOT EXISTS (SELECT * FROM dbo.T_Report WHERE C_GUID=@C_ID AND RepNo = @repNo)
	BEGIN
		BEGIN TRAN;
		--插入报表信息
		INSERT INTO dbo.T_Report(Rep_GUID,RepNo,Type,C_GUID,Year,Month)
		VALUES(@repID,@repNo,'PL',@C_ID,
		DATEPART(YEAR,@CurrentRepBeginDate),
		DATEPART(MONTH,@CurrentRepBeginDate));;
		--插入报表明细
		DECLARE @TmpRepDtl TABLE
		(
			AccGuid NVARCHAR(40),
			AccCode INT,
			AccName NVARCHAR(100),
			AccGrp INT,
			BeginningValue DECIMAL(18,2),
			EndingValue DECIMAL(18,2)
		);
		
		INSERT INTO @TmpRepDtl
		SELECT LA.LA_GUID,LA.AccCode,LA.Name ,LA.AccGroup ,ISNULL(BB.Money,0) AS BeginningValue,(ISNULL(BB.Money,0) + ISNULL(T.CurrentValue,0)) AS EndingValue
		FROM dbo.R_CompanyAccount R_CA
		LEFT JOIN dbo.T_GeneralLedgerAccount LA ON R_CA.LA_GUID = LA.LA_GUID
		LEFT JOIN dbo.T_BeginningBalance BB ON BB.C_GUID = @C_ID AND BB.Acc_GUID = R_CA.LA_GUID
		LEFT JOIN (
		SELECT AccGrp,ACC_GUID,SUM(Value) AS CurrentValue
		FROM @TmpFR
		GROUP BY AccGrp,ACC_GUID
		HAVING SUM(Value)<> 0
		) AS T ON T.ACC_GUID = R_CA.LA_GUID
		WHERE R_CA.C_GUID = @C_ID AND (LA.AccGroup = 6 OR LA.AccGroup = 7);
		
		INSERT INTO dbo.T_ReportDetails(RGUID,Rep_GUID,Code,Name,AccGrp,BeginningValue,EndingValue)
		SELECT NEWID(),@repID,AccCode,AccName ,AccGrp ,BeginningValue,EndingValue
		FROM @TmpRepDtl ;

		--转储至历史数据(所见即所得)
		DECLARE @RepCount INT = 0;
		SELECT @RepCount = COUNT(*) 
		FROM T_Report Rep 
		WHERE Rep.C_GUID = @C_ID 
		AND Rep.Year = DATEPART(YEAR,@CurrentRepBeginDate)
		AND Rep.Month = DATEPART(Month,@CurrentRepBeginDate)
		IF @RepCount = 3
		BEGIN
			INSERT INTO dbo.T_IEHistoryRecord
			SELECT IE_GUID,IE_Flag,Inv.Name,InvNo,BP.Name,Amount,Tax,Date,Creator,CreateDate,IE.C_GUID
			FROM dbo.T_IERecord IE
			LEFT JOIN dbo.T_BusinessPartner BP ON BP.BP_GUID = IE.RPer
			LEFT JOIN dbo.T_InvType Inv ON Inv.[Key] = IE.InvType
			WHERE IE.C_GUID = @C_ID AND DATE BETWEEN @CurrentRepBeginDate AND @CurrentRepEndDate;
			
			INSERT INTO dbo.T_IEHistoryDetails
			SELECT R_GUID,IE.IE_GUID,DLA.Name,DDA.Name,CLA.Name,CDA.Name,Money,IE.C_GUID,IED.Currency
			FROM dbo.T_IERecord IE
			LEFT JOIN dbo.T_IEDetails IED ON IED.IE_GUID = IE.IE_GUID
			LEFT JOIN dbo.T_GeneralLedgerAccount DLA ON DLA.LA_GUID = IED.DebitLedgerAccount 
			LEFT JOIN dbo.T_DetailedAccount DDA ON DDA.DA_GUID = IED.DebitDetailsAccount
			LEFT JOIN dbo.T_GeneralLedgerAccount CLA ON CLA.LA_GUID = IED.CreditLedgerAccount
			LEFT JOIN dbo.T_DetailedAccount CDA ON CDA.DA_GUID = IED.DebitDetailsAccount 
			WHERE IE.C_GUID = @C_ID AND DATE BETWEEN @CurrentRepBeginDate AND @CurrentRepEndDate;
			
			INSERT INTO dbo.T_RecPayHistoryRecord(RP_GUID,RP_Flag,C_GUID,InvType,InvNo,R_Per,
			DebitLedgerAccount,DebitDetailsAccount,CreditLedgerAccount,CreditDetailsAccount,Amount,
			Date,Remark,Creator,CreateDate,Currency,CFItem,CFPItem)
			SELECT RP_GUID,RP.RP_Flag,RP.C_GUID,Inv.Name,InvNo,BP.Name,
			DLA.Name,DDA.Name,CLA.Name,CDA.Name,
			Amount,Date,Remark,Creator,CreateDate,
			RP.Currency,CFI.Name,CFIP.Name
			FROM dbo.T_RecPayRecord RP
			LEFT JOIN dbo.T_BusinessPartner BP ON BP.BP_GUID = RP.R_Per
			LEFT JOIN dbo.T_InvType Inv ON Inv.[Key] = RP.InvType
			LEFT JOIN dbo.T_GeneralLedgerAccount DLA ON DLA.LA_GUID = RP.DebitLedgerAccount 
			LEFT JOIN dbo.T_DetailedAccount DDA ON DDA.DA_GUID = RP.DebitDetailsAccount
			LEFT JOIN dbo.T_GeneralLedgerAccount CLA ON CLA.LA_GUID = RP.CreditLedgerAccount
			LEFT JOIN dbo.T_DetailedAccount CDA ON CDA.DA_GUID = RP.CreditDetailsAccount
			LEFT JOIN dbo.T_CashFlowItem CFI ON CFI.R_GUID = RP.CFItemGuid
			LEFT JOIN dbo.T_CashFlowItem CFIP ON CFIP.R_GUID = RP.CFPItemGuid
			WHERE RP.C_GUID = @C_ID AND DATE BETWEEN @CurrentRepBeginDate AND @CurrentRepEndDate;
			
			DELETE dbo.T_IEDetails
			WHERE IE_GUID IN (
			SELECT IE.IE_GUID
			FROM dbo.T_IERecord IE 
			WHERE C_GUID = @C_ID AND DATE BETWEEN @CurrentRepBeginDate AND @CurrentRepEndDate);
			
			DELETE dbo.T_IERecord  
			WHERE C_GUID = @C_ID AND DATE BETWEEN @CurrentRepBeginDate AND @CurrentRepEndDate;;
			
			DELETE dbo.T_RecPayRecord  
			WHERE C_GUID = @C_ID AND DATE BETWEEN @CurrentRepBeginDate AND @CurrentRepEndDate;;
		END
		COMMIT TRAN;
	END
	ELSE
	BEGIN
		RAISERROR('Exists',12,1);
	END
END
GO
