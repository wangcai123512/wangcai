alter table dbo.T_AIDTypeRecord add  Depreciation_year nvarchar(40)
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'折旧年份' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_AIDTypeRecord', @level2type=N'COLUMN',@level2name=N'Depreciation_year'
