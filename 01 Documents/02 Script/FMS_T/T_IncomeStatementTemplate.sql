USE [FMS_Develop]
GO

/****** Object:  Table [dbo].[T_IncomeStatementTemplate]    Script Date: 01/04/2017 13:57:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[T_IncomeStatementTemplate]') AND type in (N'U'))
DROP TABLE [dbo].[T_IncomeStatementTemplate]
GO

USE [FMS_Develop]
GO

/****** Object:  Table [dbo].[T_IncomeStatementTemplate]    Script Date: 01/04/2017 13:57:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[T_IncomeStatementTemplate](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[item_code] [varchar](50) NULL,
	[item_name] [varchar](500) NULL,
	[row_no] [int] NULL,
	[amount] [decimal](18, 4) NULL,
	[amount_sum] [decimal](18, 4) NULL,
 CONSTRAINT [PK_T_IncomeStatementTemplate] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'本月数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_IncomeStatementTemplate', @level2type=N'COLUMN',@level2name=N'amount'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'本年累计数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_IncomeStatementTemplate', @level2type=N'COLUMN',@level2name=N'amount_sum'
GO


