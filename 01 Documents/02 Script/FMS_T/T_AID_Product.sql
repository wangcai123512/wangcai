USE [FMS_Develop]
GO

/****** Object:  Table [dbo].[T_AID_Product]    Script Date: 11/29/2016 15:01:37 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[T_AID_Product]') AND type in (N'U'))
DROP TABLE [dbo].[T_AID_Product]
GO

USE [FMS_Develop]
GO

/****** Object:  Table [dbo].[T_AID_Product]    Script Date: 11/29/2016 15:01:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[T_AID_Product](
	[GUID] [varchar](40) NOT NULL,
	[AID_Guid] [varchar](40) NULL,
	[Use_AID_Amount] [decimal](18, 4) NULL,
	[Product_Guid] [varchar](40) NULL,
 CONSTRAINT [PK_T_AID_Product] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


