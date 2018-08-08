USE [FMS_Develop]
GO

/****** Object:  Table [dbo].[T_CashFlowItemTemplate]    Script Date: 12/21/2016 11:28:18 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[T_CashFlowItemTemplate]') AND type in (N'U'))
DROP TABLE [dbo].[T_CashFlowItemTemplate]
GO

USE [FMS_Develop]
GO

/****** Object:  Table [dbo].[T_CashFlowItemTemplate]    Script Date: 12/21/2016 11:28:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[T_CashFlowItemTemplate](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](500) NULL,
	[row_no] [varchar](5) NULL,
	[amount] [decimal](18, 4) NULL,
	[additional] [varchar](500) NULL,
	[row_no_add] [varchar](5) NULL,
	[amount_add] [decimal](18, 4) NULL,
 CONSTRAINT [PK_CashFlowItemTemplate] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


