 

USE [FMS_Develop]
GO

/****** Object:  Table [dbo].[T_Product]    Script Date: 11/23/2016 16:16:00 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[T_Product]') AND type in (N'U'))
DROP TABLE [dbo].[T_Product]
GO

USE [FMS_Develop]
GO

/****** Object:  Table [dbo].[T_Product]    Script Date: 11/23/2016 16:16:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[T_Product](
	[GUID] [nvarchar](40) NOT NULL,
	[C_GUID] [nvarchar](40) NULL,
	[Create_Date] [varchar](20) NULL, 
	[Currency] [nvarchar](40) NULL,	 
	 stock_count   INT,
	 used_count INT,
	 [saled_count] INT,
	[TypeId] [varchar](40) NOT NULL,
	[SubTypeId] [varchar](40) NOT NULL 
 CONSTRAINT [PK_T_Product] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO 
 

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'产品类别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Product', @level2type=N'COLUMN',@level2name=N'TypeId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'产品子类别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Product', @level2type=N'COLUMN',@level2name=N'SubTypeId'
GO 


