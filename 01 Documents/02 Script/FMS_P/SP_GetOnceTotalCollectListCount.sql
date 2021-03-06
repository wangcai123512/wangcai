USE [FMS_Develop]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetOnceTotalCollectListCount]    Script Date: 06/18/2017 16:03:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,hdy>
-- Create date: <Create Date,,05/09/2017>
-- Description:	获取一级费用科目分类的总成本与费用汇总行
-- =============================================
ALTER PROCEDURE [dbo].[SP_GetOnceTotalCollectListCount]
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
		DECLARE @SumAmountWage FLOAT   /**获取期初税费总金额*/
		DECLARE @CountWage NVARCHAR(50) /**统计应付工资余额*/

		 /**查询统计货币*/
		SELECT @CompanyCurrency= Code
		FROM  dbo.R_CompanyCurrceny 
		WHERE C_GUID=@C_GUID
			
			
		/**统计一级费用科目分类的总成本与费用列表金额汇总*/
		DECLARE @tempSumAmount TABLE(
			SumAmount	FLOAT
		)				
		BEGIN
		 INSERT INTO @tempSumAmount
		SELECT SUM(CASE  WHEN Y.TCurrency = IE.Currency AND Y.FCurrency = @CompanyCurrency	
			AND IE.IE_Flag= 'E' AND IE.State='应付'  
			THEN IE.SumAmount*Rate
			WHEN  IE.Currency =@CompanyCurrency AND IE.IE_Flag= 'E' AND  IE.State='应付'  THEN round(IE.SumAmount/11,5)
			ELSE 0 END)
			FROM  dbo.T_IERecord IE    
			LEFT JOIN  dbo.T_RateHistory Y ON Y.C_GUID = IE.C_GUID        
			WHERE IE.C_GUID=@C_GUID 
			AND IE.AffirmDate>Y.Date  
		END			

		
		/**查询应付工资余额是否有记录*/
			 SELECT   @CountWage=COUNT(*)
			 FROM dbo.T_BeginningBalance a 
			 WHERE a.C_GUID=@C_GUID
			 AND a.Acc_GUID = (SELECT b.LA_GUID FROM dbo.T_GeneralLedgerAccount b WHERE b.Name = '应付工资余额 ')
			BEGIN 
				IF @CountWage>0
				 SELECT   @SumAmountWage=ISNULL(a.Money ,0)
				 FROM dbo.T_BeginningBalance a 
				 WHERE a.C_GUID=@C_GUID
				 AND a.Acc_GUID = (SELECT b.LA_GUID FROM dbo.T_GeneralLedgerAccount b WHERE b.Name = '应付工资余额 ')
				 ELSE
				 SET	@SumAmountWage=0
			END			
		
	    /**统计临时表中总金额**/	
		SELECT @SumAmountSUM = (SUM(SumAmount)+@SumAmountWage)
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



