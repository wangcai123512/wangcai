USE [FMS_Develop]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetCashInFlowsAccountRecordList]    Script Date: 04/20/2017 13:29:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,hdy,>
-- Create date: <Create Date,03/31/2017,>
-- Description:	获取现金流水账
-- =============================================
ALTER  PROCEDURE [dbo].[SP_GetCashInFlowsAccountRecordList]
    @PageSize INT = -1,
	@PageIndex INT = 1,
	@Count INT = 0 OUT,	
	@C_GUID NVARCHAR(50),
	@BA_GUID NVARCHAR(50)= NULL,
	@DateBegin DATETIME = NULL,
	@DateEnd DATETIME = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
		 DECLARE @NEWGUID VARCHAR(40) 	/**生成GUID*/
		 DECLARE @InitialAmount FLOAT 	/**科目余额*/	
		 DECLARE  @InitialAmountTime DATETIME 	/**科目余额初始化初始时间*/
		 SET  @NEWGUID=NEWID();
		 
		  /**查询科目余额**/
		 SELECT @InitialAmount=InitialAmount
		 FROM dbo.T_BankAccount
		 WHERE  BA_GUID=@BA_GUID 
		 
		 SELECT  TOP 1 @InitialAmountTime=InitialDate 
	     FROM dbo.T_BeginningBalance
         WHERE C_GUID=@C_GUID
	BEGIN
		/**查询账号下时间范围内的净现金流**/
		/**既要转换货币又要加收款减付款还要汇总金额*/	
		DECLARE @temp TABLE(
			rownumber INT,
			TNEWGUID NVARCHAR(40),
			RecSumAmount NVARCHAR(40),
			PaySumAmount NVARCHAR(40),
			RecSumAmountZ   NVARCHAR(40),
			PaySumAmountZ   NVARCHAR(40),
			Currency NVARCHAR(40),
			AccountAbbreviation NVARCHAR(40),
			Date DATETIME,
			InitialAmount NVARCHAR(40),
			CreateDate DATETIME
			
		 )	
		BEGIN
			 INSERT INTO @temp		 
			 SELECT  ROW_NUMBER()OVER(ORDER BY @NEWGUID DESC) rownumber,@NEWGUID,
			(CASE WHEN R.RP_Flag = 'R' THEN R.SumAmount ELSE 0 END) AS RecSumAmount,
			(CASE WHEN R.RP_Flag = 'P' THEN R.SumAmount ELSE 0 END) AS PaySumAmount,			
			(CASE WHEN( R.RP_Flag = 'R' )THEN R.SumAmount WHEN( R.RP_Flag = 'R' ) 
			THEN R.SumAmount ELSE  0 END ) AS RecSumAmountZ,
		    (CASE WHEN( R.RP_Flag = 'P' )THEN R.SumAmount WHEN( R.RP_Flag = 'P' ) 
			THEN R.SumAmount ELSE  0 END) AS PaySumAmountZ,		
			R.Currency AS Currency,BA.AccountAbbreviation AS AccountAbbreviation,
		    CONVERT(VARCHAR(10),R.Date,111),(CASE WHEN @DateBegin<=@InitialAmountTime
		    THEN @InitialAmount  ELSE 0 END)AS InitialAmounts,R.CreateDate 			
			FROM  dbo.T_RecPayRecord R
			LEFT JOIN dbo.T_BankAccount BA ON BA.BA_GUID= R.BA_GUID
			WHERE R.C_GUID=@C_GUID		
			AND(R.BA_GUID = @BA_GUID OR @BA_GUID IS NULL OR LEN(@BA_GUID) = 0)	        
			AND(R.Date >= @DateBegin OR @DateBegin IS NULL OR LEN(@DateBegin)= 0)
			AND(R.Date < DATEADD(DAY,1,@DateEnd) OR @DateEnd IS NULL OR LEN(@DateEnd)= 0)
			ORDER BY R.Date ASC
			
		 			
			SELECT @Count = COUNT(*) FROM @temp;
			SELECT T.TNEWGUID,T.RecSumAmount,T.PaySumAmount,T.Currency,T.InitialAmount,
			T.AccountAbbreviation,CONVERT(VARCHAR(10),T.Date,111) AS Date,T.RecSumAmountZ,T.PaySumAmountZ,T.CreateDate   
			FROM @temp T
			WHERE (T.rownumber>= ((@PageIndex - 1)*@PageSize) + 1
			AND T.rownumber <= @PageIndex*@PageSize )
			OR (@PageSize = -1)	
		END				 
	END	  
END



