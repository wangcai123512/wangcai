USE [FMS_Develop]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetCompareTotalListCount]    Script Date: 05/25/2017 15:02:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,hdy>
-- Create date: <Create Date,05/11/2017,>
-- Description:	获取总成本与费用的比较
-- =============================================
ALTER PROCEDURE [dbo].[SP_GetCompareTotalListCount]
	@C_GUID NVARCHAR(50),	
	@DateBegin DATETIME = NULL,
	@DateEnd DATETIME = NULL
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
		 DECLARE @SumAmountWage FLOAT   /**获取期初税费总金额*/
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
			/**有汇率的月份金额汇总(IE表有汇率,T_RecPayRecord有汇率和没汇率两种情况)**/		
		    SELECT  MONTH(IE.AffirmDate) AS DateY,SUM(CASE WHEN IE.IE_Flag = 'E' THEN IE.SumAmount*Rate ELSE 0 END)  AS SumAmountY		    
			FROM  dbo.T_IERecord IE
			LEFT JOIN dbo.T_BankAccount BA ON BA.BA_GUID = IE.BA_GUID
			LEFT JOIN  dbo.T_RateHistory Y ON Y.C_GUID = IE.C_GUID AND Y.TCurrency = IE.Currency 
			WHERE IE.C_GUID=@C_GUID	
			AND Y.CurrentRecord = '1'
			AND Y.FCurrency = @CompanyCurrency	
			AND IE.AffirmDate > Y.Date      
			AND(CONVERT(VARCHAR(100), IE.AffirmDate, 20)>= @DateBegin OR @DateBegin IS NULL OR LEN(@DateBegin)= 0)
			AND(CONVERT(VARCHAR(100), IE.AffirmDate, 20)<DATEADD(MONTH,1,@DateEnd) OR @DateEnd IS NULL OR LEN(@DateEnd)= 0)
			GROUP BY MONTH(IE.AffirmDate)
			
			
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
			
			/**无汇率的月份金额汇总（T_RecPayRecord有汇率和没汇率两种情况）**/		
			SELECT  MONTH(IE.AffirmDate) AS  DateN ,SUM(CASE WHEN IE.IE_Flag = 'E' THEN IE.SumAmount ELSE 0 END) AS SumAmountN
			FROM  dbo.T_IERecord IE
			WHERE IE.C_GUID=@C_GUID	
			AND IE.Currency = @CompanyCurrency     
			AND(CONVERT(varchar(100), IE.AffirmDate, 20)>= @DateBegin OR @DateBegin IS NULL OR LEN(@DateBegin)= 0)
			AND(CONVERT(varchar(100), IE.AffirmDate, 20)<DATEADD(month,1,@DateEnd) OR @DateEnd IS NULL OR LEN(@DateEnd)= 0)
			GROUP by MONTH(IE.AffirmDate)
			
			
			
			/**统计期初余额（应付工资）*/
			select @SumAmountWage=ISNULL(a.Money,0)  
			FROM dbo.T_BeginningBalance a 
			WHERE a.C_GUID=@C_GUID
			and a.Acc_GUID = (select b.LA_GUID from dbo.T_GeneralLedgerAccount b where b.Name = '应付工资余额 ')
			
			SELECT @CountN=COUNT(*) FROM #CompareN 
			SELECT @CountY=COUNT(*) FROM #CompareY
						
				IF @CountN >@CountY
					SELECT ISNULL((#CompareY.SumAmountY),0)+ISNULL((#CompareN.SumAmountN),0)+@SumAmountWage 
					AS SumAmount,#CompareN.DateN AS CompareMonth
					FROM #CompareN left join  #CompareY ON #CompareY.DateY=#CompareN.DateN
				ELSE
					SELECT ISNULL((#CompareY.SumAmountY),0)+ISNULL((#CompareN.SumAmountN),0)+@SumAmountWage  
					AS SumAmount,#CompareY.DateY AS CompareMonth
					FROM #CompareY left join  #CompareN ON #CompareY.DateY=#CompareN.DateN
			
			 DROP TABLE #CompareY
			 DROP TABLE #CompareN
			
		END				 
END



