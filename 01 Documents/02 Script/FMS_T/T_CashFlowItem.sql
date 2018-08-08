USE [FMS_Develop]
GO

/****** Object:  Table [dbo].[T_CashFlowItem]    Script Date: 12/21/2016 11:11:06 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[T_CashFlowItem]') AND type in (N'U'))
DROP TABLE [dbo].[T_CashFlowItem]
GO

USE [FMS_Develop]
GO

/****** Object:  Table [dbo].[T_CashFlowItem]    Script Date: 12/21/2016 11:11:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[T_CashFlowItem](
	[R_GUID] [nvarchar](40) NOT NULL,
	[No] [varchar](5) NULL,
	[Name] [nvarchar](100) NULL,
	[PID] [nvarchar](40) NULL,
	[RP_Flag] [nvarchar](1) NULL,
 CONSTRAINT [PK_CASHFLOWITEM] PRIMARY KEY NONCLUSTERED 
(
	[R_GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


