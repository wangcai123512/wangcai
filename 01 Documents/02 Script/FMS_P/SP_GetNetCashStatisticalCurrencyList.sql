USE [FMS_Develop]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetNetCashStatisticalCurrencyList]    Script Date: 03/30/2017 14:50:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,hdy,>
-- Create date: <Create Date,03/28/2017>
-- Description:	获取账号下指定日期的现金流（统计货币部分）
-- =============================================
ALTER PROCEDURE [dbo].[SP_GetNetCashStatisticalCurrencyList]	
	@PageSize INT = -1,
	@PageIndex INT = 1,
	@Count INT = 0 OUT,	
	@C_GUID NVARCHAR(50),
	@DateBegin DATETIME = NULL,
	@DateEnd DATETIME = NULL
AS

BEGIN

	SET NOCOUNT ON;
		DECLARE @RPRSumAmount_R1 INT = 0/**收款T_RateHistory有值*/
		DECLARE @RPRSumAmount_R2 INT = 0/**收款T_RateHistory无值*/
		DECLARE @RPRSumAmount_R INT = 0/*收款总和（有值+无值）*/
		DECLARE @RPRSumAmount_P1 INT = 0/**付款T_RateHistory有值*/
		DECLARE @RPRSumAmount_P2 INT = 0/**付款T_RateHistory无值*/
		DECLARE @RPRSumAmount_P INT = 0/*付款总和（有值+无值）*/
		DECLARE @RPRSumAmount_COUNT INT = 0/*结果值*/
		DECLARE @CompanyCurrency NVARCHAR(50)
		DECLARE @NEWGUID VARCHAR(40) 	/**生成GUID*/
		SET  @NEWGUID=NEWID();	
		
	
		/**查询统计货币*/
		SELECT @CompanyCurrency= Code
		FROM  dbo.R_CompanyCurrceny 
		WHERE C_GUID=@C_GUID
	
	
		/**收款T_RateHistory有值*/
		 SELECT @RPRSumAmount_R1  =  isnull(SUM(SumAmount*Rate) ,0) 
		 FROM  dbo.T_RecPayRecord R
		 LEFT JOIN  dbo.T_RateHistory Y ON Y.C_GUID = R.C_GUID AND Y.TCurrency = R.Currency 
		 WHERE R.C_GUID=@C_GUID
		 AND Y.CurrentRecord = '1'
		 AND Y.FCurrency = @CompanyCurrency
		 AND R.RP_Flag = 'R'	    
		 AND(R.Date >= @DateBegin OR @DateBegin IS NULL OR LEN(@DateBegin)= 0)
		 AND(R.Date < DATEADD(DAY,1,@DateEnd) OR @DateEnd IS NULL OR LEN(@DateEnd)= 0)	 
		 
		 /**收款T_RateHistory无值*/
		 SELECT @RPRSumAmount_R2  =  isnull(SUM(SumAmount),0) 
		 FROM  dbo.T_RecPayRecord R
		 WHERE R.C_GUID=@C_GUID
		 AND R.RP_Flag = 'R'	   
		 AND R.Currency = @CompanyCurrency
		 AND(R.Date >= @DateBegin OR @DateBegin IS NULL OR LEN(@DateBegin)= 0)
		 AND(R.Date < DATEADD(DAY,1,@DateEnd) OR @DateEnd IS NULL OR LEN(@DateEnd)= 0)	 
		
		/**付款T_RateHistory有值*/
		 SELECT @RPRSumAmount_P1  =  isnull(SUM(SumAmount*Rate) ,0) 
		 FROM  dbo.T_RecPayRecord R
		 LEFT JOIN  dbo.T_RateHistory Y ON Y.C_GUID = R.C_GUID AND Y.TCurrency = R.Currency 
		 WHERE R.C_GUID=@C_GUID
		 AND Y.CurrentRecord = '1'
		 AND Y.FCurrency = @CompanyCurrency
		 AND R.RP_Flag = 'P'	    
		 AND(R.Date >= @DateBegin OR @DateBegin IS NULL OR LEN(@DateBegin)= 0)
		 AND(R.Date < DATEADD(DAY,1,@DateEnd) OR @DateEnd IS NULL OR LEN(@DateEnd)= 0)	 
		 
		 /**付款T_RateHistory无值*/
		 SELECT @RPRSumAmount_P2  =  isnull(SUM(SumAmount),0) 
		 FROM  dbo.T_RecPayRecord R
		 WHERE R.C_GUID=@C_GUID
		 AND R.RP_Flag = 'P'	   
		 AND R.Currency = @CompanyCurrency
		 AND(R.Date >= @DateBegin OR @DateBegin IS NULL OR LEN(@DateBegin)= 0)
		 AND(R.Date < DATEADD(DAY,1,@DateEnd) OR @DateEnd IS NULL OR LEN(@DateEnd)= 0)
		
		
		
		SET @RPRSumAmount_R = @RPRSumAmount_R1+@RPRSumAmount_R2;
		SET @RPRSumAmount_P = @RPRSumAmount_P1+@RPRSumAmount_P2;
		SET @RPRSumAmount_COUNT = @RPRSumAmount_R-@RPRSumAmount_P;
		
		 			 		 	 
		DECLARE @temp TABLE(
			rownumber INT,
			SumAmount NVARCHAR(40),
			Currency NVARCHAR(40)
		 )	 	 
	
		BEGIN
			INSERT INTO @temp	
    		VALUES ( 2,@RPRSumAmount_COUNT,@CompanyCurrency)	
			
		
			
			SELECT @Count = COUNT(*) FROM @temp;
			SELECT T.SumAmount,T.Currency
			FROM @temp T
			WHERE (T.rownumber>= ((@PageIndex - 1)*@PageSize) + 1
			AND T.rownumber <= @PageIndex*@PageSize )
			OR (@PageSize = -1)		
		END
END




