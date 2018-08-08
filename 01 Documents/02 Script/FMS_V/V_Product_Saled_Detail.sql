USE [FMS_Develop]
GO

/****** Object:  View [dbo].[V_Product_Saled_Detail]    Script Date: 02/15/2017 17:07:52 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[V_Product_Saled_Detail]'))
DROP VIEW [dbo].[V_Product_Saled_Detail]
GO

USE [FMS_Develop]
GO

/****** Object:  View [dbo].[V_Product_Saled_Detail]    Script Date: 02/15/2017 17:07:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[V_Product_Saled_Detail]
AS
SELECT     dbo.T_Product_Saled.id, dbo.T_Product_Saled.sale_batch_id, dbo.T_Product_Saled.product_guid, dbo.T_Product_Saled.ie_guid, dbo.T_Product_Saled.saled_date, 
                      dbo.T_BusinessPartner.Name AS customer_name, dbo.T_Product.C_GUID, dbo.T_IERecord.Currency, dbo.T_Product_Saled.saled_amount, 
                      dbo.T_IERecord.SumAmount
FROM         dbo.T_Product_Saled INNER JOIN
                      dbo.T_Product ON dbo.T_Product_Saled.product_guid = dbo.T_Product.GUID INNER JOIN
                      dbo.T_IERecord ON dbo.T_Product_Saled.ie_guid = dbo.T_IERecord.IE_GUID INNER JOIN
                      dbo.T_BusinessPartner ON dbo.T_IERecord.RPer = dbo.T_BusinessPartner.BP_GUID

GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[49] 4[27] 2[6] 3) )"
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
         Begin Table = "T_Product_Saled"
            Begin Extent = 
               Top = 6
               Left = 295
               Bottom = 188
               Right = 469
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T_Product"
            Begin Extent = 
               Top = 5
               Left = 555
               Bottom = 268
               Right = 722
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T_IERecord"
            Begin Extent = 
               Top = 32
               Left = 20
               Bottom = 230
               Right = 188
            End
            DisplayFlags = 280
            TopColumn = 5
         End
         Begin Table = "T_BusinessPartner"
            Begin Extent = 
               Top = 205
               Left = 286
               Bottom = 324
               Right = 466
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
         Table = 4065
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
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'V_Product_Saled_Detail'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'V_Product_Saled_Detail'
GO


