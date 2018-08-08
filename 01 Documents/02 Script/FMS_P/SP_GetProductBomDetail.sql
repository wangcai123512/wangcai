USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_GetProductBomDetail]    Script Date: 02/15/2017 16:03:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





-- =============================================
-- Author:		<Author,,hdy>
-- Create date: <Create Date,,2/15/2017>
-- Description:	ªÒ»°ProductBomœÍ«È
-- =============================================
ALTER PROCEDURE [dbo].[SP_GetProductBomDetail]
	@ID NVARCHAR(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here	
    SELECT PB.number AS number,PB.number_sum AS tags
	FROM dbo.T_ProductBom PB
    WHERE PB.GUID=@ID 

END





GO


