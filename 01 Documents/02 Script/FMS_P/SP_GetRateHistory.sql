USE [FMS_Develop]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetRateHistory]    Script Date: 05/24/2017 14:31:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<CHENXIANG>
-- Create date: <2015/09/08>
-- Description:	<查询统计货币与汇率信息>
-- =============================================
ALTER PROCEDURE [dbo].[SP_GetRateHistory]
	-- Add the parameters for the stored procedure here
	@C_GUID nvarchar(40),
	@Current nvarchar(40),
	@FCurrency nvarchar(40)
	
	AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
  		select T.GUID,CONVERT(nvarchar(10),T.Date,120) as DateS,T.FAmount,T.FCurrency,T.TAmount,T.TCurrency,T.CurrentRecord,T.Currency
		from dbo.T_RateHistory T
		where T.C_GUID=@C_GUID and CurrentRecord=@Current
		ORDER BY Currency
		
		
		
		
		/**   
		这里的列表查询查询当前公司所有统计货币（包括切换的货币）的列表 
		**/
		/**   
		AND Currency =@FCurrency 
		**/
		
		
	
END
