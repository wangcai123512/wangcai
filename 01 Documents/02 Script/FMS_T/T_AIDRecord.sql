USE [FMS_Develop]
GO

/****** Object:  Table [dbo].[T_AIDRecord]    Script Date: 01/23/2017 10:02:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[T_AIDRecord](
	[GUID] [NVARCHAR](40) NOT NULL,
	[C_GUID] [NVARCHAR](40) NULL,
	[Date] [DATETIME] NULL,
	[Number] [INT] NULL,
	[Inventory_Number] [INT] NULL,
	[Amount] [DECIMAL](18, 4) NULL,
	[Currency] [NVARCHAR](40) NULL,
	[RPer] [NVARCHAR](40) NULL,
	[InvType] [NVARCHAR](40) NULL,
	[Description] [NVARCHAR](40) NULL,
	[DepreciationPeriod] [INT] NULL,
	[SurplusValue] [DECIMAL](18, 4) NULL,
	[State] [NVARCHAR](40) NULL,
	[Remark] [NVARCHAR](40) NULL,
	[CostType] [VARCHAR](1) NULL,
	[GUID_Parent] [NCHAR](40) NULL,
	[SubType] [VARCHAR](50) NULL,
 CONSTRAINT [PK_T_AIDRecord] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_AIDRecord', @level2type=N'COLUMN',@level2name=N'Number'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_AIDRecord', @level2type=N'COLUMN',@level2name=N'Inventory_Number'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ʼ���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_AIDRecord', @level2type=N'COLUMN',@level2name=N'Amount'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Ӧ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_AIDRecord', @level2type=N'COLUMN',@level2name=N'RPer'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_AIDRecord', @level2type=N'COLUMN',@level2name=N'InvType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ʣ����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_AIDRecord', @level2type=N'COLUMN',@level2name=N'SurplusValue'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ɱ��������(B=Ӫҵ�ɱ�,S=���۷���,M=�������)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_AIDRecord', @level2type=N'COLUMN',@level2name=N'CostType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'GUID_parent���ж�Ӧ��GUID���ɵ���GUID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_AIDRecord', @level2type=N'COLUMN',@level2name=N'GUID_Parent'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_AIDRecord', @level2type=N'COLUMN',@level2name=N'SubType'
GO


