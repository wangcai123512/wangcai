USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_GetCashFlowItems]    Script Date: 12/20/2016 17:33:45 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_GetCashFlowItems]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_GetCashFlowItems]
GO

USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_GetCashFlowItems]    Script Date: 12/20/2016 17:33:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	获取现金流量项目
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetCashFlowItems]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT *
    FROM dbo.T_CashFlowItem
END

GO


