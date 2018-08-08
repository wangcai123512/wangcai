USE [FMS_Develop]
GO

/****** Object:  Table [dbo].[T_QuickAttention]    Script Date: 06/01/2017 14:56:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[T_QuickAttention](
	[id] [nvarchar](40) NOT NULL,
	[c_guid] [nvarchar](40) NULL,
	[attention_type] [nvarchar](40) NULL,
	[attention_type_amount] [nvarchar](40) NULL,
	[statistical_time] [nvarchar](40) NULL,
	[statistical_currency] [nvarchar](40) NULL,
	[attention_state] [int] NULL,
	[push_account] [nvarchar](40) NULL,
	[push_frequency] [nvarchar](40) NULL
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'当前记录主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_QuickAttention', @level2type=N'COLUMN',@level2name=N'id'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'公司GUID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_QuickAttention', @level2type=N'COLUMN',@level2name=N'c_guid'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'关注类别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_QuickAttention', @level2type=N'COLUMN',@level2name=N'attention_type'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'关注类型金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_QuickAttention', @level2type=N'COLUMN',@level2name=N'attention_type_amount'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'统计时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_QuickAttention', @level2type=N'COLUMN',@level2name=N'statistical_time'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'统计货币' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_QuickAttention', @level2type=N'COLUMN',@level2name=N'statistical_currency'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0:未关注,1:已关注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_QuickAttention', @level2type=N'COLUMN',@level2name=N'attention_state'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'推送账号(没有给类型可根据微信号,Email带有@,短信是手机号类型判断)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_QuickAttention', @level2type=N'COLUMN',@level2name=N'push_account'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'推送频率(day:每日;week:每周;month:每月;quarter:每季)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_QuickAttention', @level2type=N'COLUMN',@level2name=N'push_frequency'
GO

