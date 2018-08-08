USE [FMS_Develop]
GO

/****** Object:  Table [dbo].[T_BalanceSheetTemplate]    Script Date: 12/23/2016 11:18:25 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[T_BalanceSheetTemplate]') AND type in (N'U'))
DROP TABLE [dbo].[T_BalanceSheetTemplate]
GO

USE [FMS_Develop]
GO

/****** Object:  Table [dbo].[T_BalanceSheetTemplate]    Script Date: 12/23/2016 11:18:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[T_BalanceSheetTemplate](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[asset_item_name] [varchar](500) NULL,
	[asset_row_no] [int] NULL,
	[asset_start_amount] [decimal](18, 4) NULL,
	[asset_end_amount] [decimal](18, 4) NULL,
	[debt_item_name] [varchar](500) NULL,
	[debt_row_no] [int] NULL,
	[debt_start_amount] [decimal](18, 4) NULL,
	[debt_end_amount] [decimal](18, 4) NULL,
 CONSTRAINT [PK_T_BalanceSheetTemplate] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


