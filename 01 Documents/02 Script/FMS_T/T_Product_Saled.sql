USE [FMS_Develop]
GO

/****** Object:  Table [dbo].[T_Product_Saled]    Script Date: 11/28/2016 16:28:11 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[T_Product_Saled]') AND type in (N'U'))
DROP TABLE [dbo].[T_Product_Saled]
GO

USE [FMS_Develop]
GO

/****** Object:  Table [dbo].[T_Product_Saled]    Script Date: 11/28/2016 16:28:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[T_Product_Saled](
    id  VARCHAR(40)NOT NULL,
	[sale_batch_id] [varchar](40) NOT NULL,
	[product_guid] [varchar](40) NULL,
	[ie_guid] [varchar](40) NULL,
	saled_date VARCHAR(10),
	saled_amout DECIMAL(18,4)
 CONSTRAINT [PK_T_Product_Saled] PRIMARY KEY CLUSTERED 
(
	id ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


