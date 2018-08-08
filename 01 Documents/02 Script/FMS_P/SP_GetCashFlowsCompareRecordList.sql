USE [FMS_Develop]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetCashFlowsCompareRecordList]    Script Date: 04/05/2017 15:30:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,hdy,>
-- Create date: <Create Date,03/28/2017,>
-- Description:	获取账号下指定月份的现金流比较
-- =============================================
ALTER PROCEDURE [dbo].[SP_GetCashFlowsCompareRecordList]
	@C_GUID NVARCHAR(50),
	@BA_GUID NVARCHAR(50)= NULL,
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
		 DECLARE @CountN INT = 0 /**无汇率记录数*/
		 DECLARE @CountY INT = 0 /**有汇率记录数*/
		 
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
			/**有汇率的月份金额汇总**/		
		    SELECT  MONTH(R.Date) AS DateY,SUM(CASE WHEN R.RP_Flag = 'R' THEN R.SumAmount*Rate ELSE 0 END) - SUM(CASE WHEN R.RP_Flag = 'P' THEN R.SumAmount*Rate ELSE 0 END) AS SumAmountY
			FROM  dbo.T_RecPayRecord R
			LEFT JOIN dbo.T_BankAccount BA ON BA.BA_GUID = R.BA_GUID
			LEFT JOIN  dbo.T_RateHistory Y ON Y.C_GUID = R.C_GUID AND Y.TCurrency = R.Currency 
			WHERE R.C_GUID=@C_GUID	
			AND Y.CurrentRecord = '1'
			AND Y.FCurrency = @CompanyCurrency	
			AND(R.BA_GUID = @BA_GUID OR @BA_GUID IS NULL OR LEN(@BA_GUID) = 0)		       
			AND(CONVERT(varchar(100), R.Date, 20)>= @DateBegin OR @DateBegin IS NULL OR LEN(@DateBegin)= 0)
			AND(CONVERT(varchar(100), R.Date, 20)<	 DATEADD(month,1,@DateEnd) OR @DateEnd IS NULL OR LEN(@DateEnd)= 0)
			GROUP by MONTH(R.Date)
			
			
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
			SELECT  MONTH(R.Date) AS  DateN ,SUM(CASE WHEN R.RP_Flag = 'R' THEN R.SumAmount ELSE 0 END) - SUM(CASE WHEN R.RP_Flag = 'P' THEN R.SumAmount ELSE 0 END) AS SumAmountN
			FROM  dbo.T_RecPayRecord R
			WHERE R.C_GUID=@C_GUID	
			AND R.Currency = @CompanyCurrency
			AND(R.BA_GUID = @BA_GUID OR @BA_GUID IS NULL OR LEN(@BA_GUID) = 0)		       
			AND(CONVERT(varchar(100), R.Date, 20)>= @DateBegin OR @DateBegin IS NULL OR LEN(@DateBegin)= 0)
			AND(CONVERT(varchar(100), R.Date, 20)<	 DATEADD(month,1,@DateEnd) OR @DateEnd IS NULL OR LEN(@DateEnd)= 0)
			GROUP by MONTH(R.Date)
			
			SELECT @CountN=COUNT(*) FROM #CompareN
			SELECT @CountY=COUNT(*) FROM #CompareY
			
			

				IF @CountN >@CountY
					SELECT ISNULL((#CompareY.SumAmountY),0)+ISNULL((#CompareN.SumAmountN),0) AS SumAmount,#CompareN.DateN AS Date
					FROM #CompareN left join  #CompareY ON #CompareY.DateY=#CompareN.DateN
				ELSE
					SELECT ISNULL((#CompareY.SumAmountY),0)+ISNULL((#CompareN.SumAmountN),0) AS SumAmount,#CompareY.DateY AS Date
					FROM #CompareY left join  #CompareN ON #CompareY.DateY=#CompareN.DateN
			
			 DROP TABLE #CompareY
			 DROP TABLE #CompareN
			
		END				 
END



