USE [FMS_Develop]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetSupplierTotalCollectListCount]    Script Date: 06/18/2017 16:27:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,hdy>
-- Create date: <Create Date,,05/15/2017>
-- Description:	查询供应商成本与费用汇总
-- =============================================
ALTER PROCEDURE [dbo].[SP_GetSupplierTotalCollectListCount]
	@PageSize INT = -1,
	@PageIndex INT = 1,
	@Count INT = 0 OUT,	
	@C_GUID NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
		DECLARE @CompanyCurrency NVARCHAR(50) /**查询统计货币*/
		DECLARE @SumAmountSUM FLOAT   /**获取总金额*/
		
		 /**查询统计货币*/
		SELECT @CompanyCurrency= Code
		FROM  dbo.R_CompanyCurrceny 
		WHERE C_GUID=@C_GUID
			
			
		/**查询供应商成本与费用汇总*/
		DECLARE @tempSumAmount TABLE(
			SumAmount	FLOAT
		)				
		BEGIN
			INSERT INTO @tempSumAmount	
			SELECT SUM(CASE  WHEN Y.TCurrency = IE.Currency AND Y.FCurrency = @CompanyCurrency	
			AND IE.IE_Flag= 'E' AND IE.State='应付'  THEN IE.SumAmount*Rate
			WHEN  IE.Currency =@CompanyCurrency AND IE.IE_Flag= 'E' AND IE.State='应付'  
			THEN round(IE.SumAmount/11,5)
			ELSE 0 END)
			FROM  dbo.T_IERecord IE     
			LEFT JOIN  dbo.T_RateHistory Y ON Y.C_GUID = IE.C_GUID    
			LEFT JOIN dbo.T_BusinessPartner BP ON BP.BP_GUID = IE.RPer    
			WHERE IE.C_GUID=@C_GUID 
			AND BP.IsSupplier = 1
			AND IE.AffirmDate>Y.Date
		END	
		
	    /**统计临时表中总金额*/	
		SELECT @SumAmountSUM=SUM(SumAmount)
		FROM @tempSumAmount

		 /*将统计临时表中总金额以及id插入到临时表*/  	  
		 DECLARE @temp TABLE(
			IE_GUID NVARCHAR(40),
			SumAmount NVARCHAR(40)
		 )	
		 BEGIN
			INSERT INTO @temp
			VALUES(NEWID(),@SumAmountSUM)		
			SELECT @Count = COUNT(*) FROM @temp;          
			SELECT T.IE_GUID,T.SumAmount           
			FROM @temp T
		 END					
END



