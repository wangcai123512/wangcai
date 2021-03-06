USE [FMS_Develop]
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdYearFromNeCashFlows]    Script Date: 06/18/2017 17:11:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,hdy,>
-- Create date: <Create Date,06/05/2017>
-- Description:	获取某时间的净现金流之后更新快速关注的的金额
-- =============================================
ALTER PROCEDURE [dbo].[SP_UpdYearFromNeCashFlows]	
	@c_guid NVARCHAR(50),
	@attention_type NVARCHAR(50),
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
		WHERE C_GUID=@c_guid
	
	
		/**收款T_RateHistory有值*/
		 SELECT @RPRSumAmount_R1  =  isnull(SUM(SumAmount*Rate) ,0) 
		 FROM  dbo.T_RecPayRecord R
		 LEFT JOIN  dbo.T_RateHistory Y ON Y.C_GUID = R.C_GUID AND Y.TCurrency = R.Currency 
		 WHERE R.C_GUID=@c_guid
		 AND Y.CurrentRecord = '1'
		 AND Y.FCurrency = @CompanyCurrency
		 AND R.RP_Flag = 'R'	    
		 AND(R.Date >= @DateBegin OR @DateBegin IS NULL OR LEN(@DateBegin)= 0)
		 AND(R.Date < DATEADD(DAY,1,@DateEnd) OR @DateEnd IS NULL OR LEN(@DateEnd)= 0)	 
		 
		 /**收款T_RateHistory无值*/
		 SELECT @RPRSumAmount_R2  =  isnull(SUM(SumAmount),0) 
		 FROM  dbo.T_RecPayRecord R
		 WHERE R.C_GUID=@c_guid
		 AND R.RP_Flag = 'R'	   
		 AND R.Currency = @CompanyCurrency
		 AND(R.Date >= @DateBegin OR @DateBegin IS NULL OR LEN(@DateBegin)= 0)
		 AND(R.Date < DATEADD(DAY,1,@DateEnd) OR @DateEnd IS NULL OR LEN(@DateEnd)= 0)	 
		
		/**付款T_RateHistory有值*/
		 SELECT @RPRSumAmount_P1  =  isnull(SUM(SumAmount*Rate) ,0) 
		 FROM  dbo.T_RecPayRecord R
		 LEFT JOIN  dbo.T_RateHistory Y ON Y.C_GUID = R.C_GUID AND Y.TCurrency = R.Currency 
		 WHERE R.C_GUID=@c_guid
		 AND Y.CurrentRecord = '1'
		 AND Y.FCurrency = @CompanyCurrency
		 AND R.RP_Flag = 'P'	    
		 AND(R.Date >= @DateBegin OR @DateBegin IS NULL OR LEN(@DateBegin)= 0)
		 AND(R.Date < DATEADD(DAY,1,@DateEnd) OR @DateEnd IS NULL OR LEN(@DateEnd)= 0)	 
		 
		 /**付款T_RateHistory无值*/
		 SELECT @RPRSumAmount_P2  =  isnull(SUM(SumAmount),0) 
		 FROM  dbo.T_RecPayRecord R
		 WHERE R.C_GUID=@c_guid
		 AND R.RP_Flag = 'P'	   
		 AND R.Currency = @CompanyCurrency
		 AND(R.Date >= @DateBegin OR @DateBegin IS NULL OR LEN(@DateBegin)= 0)
		 AND(R.Date < DATEADD(DAY,1,@DateEnd) OR @DateEnd IS NULL OR LEN(@DateEnd)= 0)
		
		
		
		SET @RPRSumAmount_R = @RPRSumAmount_R1+@RPRSumAmount_R2;
		SET @RPRSumAmount_P = @RPRSumAmount_P1+@RPRSumAmount_P2;
		/***/
		SET @RPRSumAmount_COUNT = @RPRSumAmount_R-@RPRSumAmount_P;
		
		UPDATE dbo.T_QuickAttention 
		SET attention_type_amount=@RPRSumAmount_COUNT ,
		statistical_time=GETDATE(),statistical_currency=@CompanyCurrency
		WHERE c_guid=@c_guid
		AND attention_type=@attention_type 			 		 	 
		
END




