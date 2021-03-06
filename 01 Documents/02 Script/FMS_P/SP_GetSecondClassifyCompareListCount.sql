USE [FMS_Develop]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetSecondClassifyCompareListCount]    Script Date: 05/25/2017 15:16:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,hdy,>
-- Create date: <Create Date,05/16/2017,>
-- Description:	获取二级费用科目分类的成本与费用比较
-- =============================================
ALTER PROCEDURE [dbo].[SP_GetSecondClassifyCompareListCount]
	@C_GUID NVARCHAR(50),
	@DateBegin DATETIME = NULL,
	@DateEnd DATETIME = NULL,
	@IEGroup NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
			 
		 DECLARE @CompanyCurrency NVARCHAR(50) /**查询统计货币*/
		 DECLARE @RPRSumAmount_Y INT = 0/**收款T_RateHistory有值*/
		 DECLARE @RPRMonth_Y INT = 0/**收款T_RateHistory有值*/
		 DECLARE @RPRSumAmount_N INT = 0/**收款T_RateHistory无值*/
		 DECLARE @RPRMonth_N INT = 0/**收款T_RateHistory无值*/
		 DECLARE @CountN INT = 0
		 DECLARE @CountY INT = 0
		 
		 /**查询统计货币*/
		 SELECT @CompanyCurrency= Code
		 FROM  dbo.R_CompanyCurrceny 
		 WHERE C_GUID=@C_GUID
		 
		BEGIN	
			CREATE TABLE #CompareY
		    (
					DateY VARCHAR(20),
					SumAmountY INT
		    )
			INSERT INTO #CompareY
			(
				  DateY,
				  SumAmountY 		         
		    )			
			/**有汇率的月份金额汇总(IE表有汇率)**/		
		    SELECT  MONTH(IE.AffirmDate) AS DateY,SUM(CASE WHEN IE.IE_Flag = 'E' THEN IE.SumAmount*Rate ELSE 0 END)  AS SumAmountY	    
			FROM  dbo.T_IERecord IE
			LEFT JOIN dbo.T_BankAccount BA ON BA.BA_GUID = IE.BA_GUID
			LEFT JOIN  dbo.T_RateHistory Y ON Y.C_GUID = IE.C_GUID AND Y.TCurrency = IE.Currency  
			WHERE IE.C_GUID=@C_GUID	
			AND Y.CurrentRecord = '1'
			AND Y.FCurrency = @CompanyCurrency	
			AND IE.IEGroup=@IEGroup  
			AND IE.AffirmDate > Y.Date    
			AND(CONVERT(varchar(100), IE.AffirmDate, 20)>= @DateBegin OR @DateBegin IS NULL OR LEN(@DateBegin)= 0)
			AND(CONVERT(varchar(100), IE.AffirmDate, 20)<DATEADD(month,1,@DateEnd) OR @DateEnd IS NULL OR LEN(@DateEnd)= 0)
			GROUP by MONTH(IE.AffirmDate)
			
			
			CREATE TABLE #CompareN
		    (
				DateN VARCHAR(20),
				SumAmountN INT
		    )
			INSERT INTO #CompareN
			(
				DateN,
				SumAmountN 		         
		    )	
			
			/**无汇率的月份金额汇总**/		
			SELECT  MONTH(IE.AffirmDate) AS  DateN ,SUM(CASE WHEN IE.IE_Flag = 'E' THEN IE.SumAmount ELSE 0 END)  AS SumAmountN
			FROM  dbo.T_IERecord IE
			WHERE IE.C_GUID=@C_GUID	
			AND IE.Currency = @CompanyCurrency
			AND IE.IEGroup=@IEGroup        
			AND(CONVERT(varchar(100), IE.AffirmDate, 20)>= @DateBegin OR @DateBegin IS NULL OR LEN(@DateBegin)= 0)
			AND(CONVERT(varchar(100), IE.AffirmDate, 20)<DATEADD(month,1,@DateEnd) OR @DateEnd IS NULL OR LEN(@DateEnd)= 0)
			GROUP by MONTH(IE.AffirmDate)
			
			SELECT @CountN=COUNT(*) FROM #CompareN 
			SELECT @CountY=COUNT(*) FROM #CompareY
						
				IF @CountN >@CountY
					SELECT ISNULL((#CompareY.SumAmountY),0)+ISNULL((#CompareN.SumAmountN),0) AS SumAmount,#CompareN.DateN AS CompareMonth
					FROM #CompareN left join  #CompareY ON #CompareY.DateY=#CompareN.DateN
				ELSE
					SELECT ISNULL((#CompareY.SumAmountY),0)+ISNULL((#CompareN.SumAmountN),0) AS SumAmount,#CompareY.DateY AS CompareMonth
					FROM #CompareY left join  #CompareN ON #CompareY.DateY=#CompareN.DateN
			
			 DROP TABLE #CompareY
			 DROP TABLE #CompareN
			
		END				 
END



