USE [FMS_Develop]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetNetCashLocalCurrencyList]    Script Date: 03/30/2017 15:01:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,hdy,>
-- Create date: <Create Date,03/28/2017,>
-- Description:	获取账号下指定日期的现金流（本币汇总部分）
-- =============================================
ALTER PROCEDURE [dbo].[SP_GetNetCashLocalCurrencyList]
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
		SET  @NEWGUID=NEWID();	
			 
		DECLARE @temp TABLE(
			rownumber INT,
			SumAmount NVARCHAR(40),
			Currency NVARCHAR(40)
		 )	
		BEGIN
			INSERT INTO @temp		 
			/**查询账号下时间范围内的净现金流**/
			SELECT  ROW_NUMBER()OVER(ORDER BY @NEWGUID DESC) rownumber,
			SUM(CASE WHEN R.RP_Flag = 'R' THEN R.SumAmount ELSE 0 END)-SUM(CASE WHEN R.RP_Flag = 'P' THEN R.SumAmount ELSE 0 END) AS SumAmount ,
			Currency
			FROM  dbo.T_RecPayRecord R
			LEFT JOIN dbo.T_BankAccount BA ON BA.BA_GUID= R.BA_GUID
			WHERE R.C_GUID=@C_GUID		
			AND(R.BA_GUID = @BA_GUID OR @BA_GUID IS NULL OR LEN(@BA_GUID) = 0)	        
			AND(R.Date >= @DateBegin OR @DateBegin IS NULL OR LEN(@DateBegin)= 0)
			AND(R.Date < DATEADD(DAY,1,@DateEnd) OR @DateEnd IS NULL OR LEN(@DateEnd)= 0)
			GROUP BY Currency
		 
			
			SELECT @Count = COUNT(*) FROM @temp;
			SELECT T.SumAmount,T.Currency
			FROM @temp T
			WHERE (T.rownumber>= ((@PageIndex - 1)*@PageSize) + 1
			AND T.rownumber <= @PageIndex*@PageSize )
			OR (@PageSize = -1)	
		END				 
END



