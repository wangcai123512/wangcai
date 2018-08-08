 USE [FMS_Develop]
GO

/****** Object:  View [dbo].[V_Prodcut_List]    Script Date: 01/19/2017 11:00:40 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[V_Prodcut_List]'))
DROP VIEW [dbo].[V_Prodcut_List]
GO

USE [FMS_Develop]
GO

/****** Object:  View [dbo].[V_Prodcut_List]    Script Date: 01/19/2017 11:00:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[V_Prodcut_List]
AS
SELECT     dbo.T_AIDTypeRecord.AidTypeName AS type_name, dbo.T_Product.GUID, dbo.T_Product.C_GUID, dbo.T_Product.Create_Date, dbo.T_Product.Currency, 
                      dbo.T_Product.TypeId, dbo.T_Product.SubTypeId, dbo.T_AIDSubTypeRecord.ASTTypeName AS sub_type_name, dbo.T_Product.stock_count, 
                      dbo.T_Product.used_count, ISNULL(dbo.T_Product.saled_count,0)saled_count
FROM         dbo.T_Product INNER JOIN
                      dbo.T_AIDTypeRecord ON dbo.T_Product.TypeId = dbo.T_AIDTypeRecord.AT_GUID INNER JOIN
                      dbo.T_AIDSubTypeRecord ON dbo.T_Product.SubTypeId = dbo.T_AIDSubTypeRecord.AST_GUID

GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[36] 2[16] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "T_Product"
            Begin Extent = 
               Top = 24
               Left = 79
               Bottom = 242
               Right = 331
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T_AIDTypeRecord"
            Begin Extent = 
               Top = 6
               Left = 369
               Bottom = 181
               Right = 524
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T_AIDSubTypeRecord"
            Begin Extent = 
               Top = 220
               Left = 476
               Bottom = 431
               Right = 661
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'V_Prodcut_List'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'V_Prodcut_List'
GO


