USE [FMS_Develop]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetOnceClassifyTotalListCount]    Script Date: 05/25/2017 15:06:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,hdy>
-- Create date: <Create Date,,04/20/2017>
-- Description:	获取一级费用科目分类的总成本与费用列表
-- =============================================
ALTER PROCEDURE [dbo].[SP_GetOnceClassifyTotalListCount]
	@PageSize INT = -1,
	@PageIndex INT = 1,
	@dateEnd DATETIME ,
	@dateBegin DATETIME,
	@Count INT = 0 OUT,
	@C_GUID NVARCHAR(50),
	@InvType NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
		DECLARE @CompanyCurrency NVARCHAR(50) /**查询统计货币*/
		DECLARE @SumAmountSUM FLOAT   /**获取应付款总金额*/

		 /**查询统计货币*/
		SELECT @CompanyCurrency= Code
		FROM  dbo.R_CompanyCurrceny 
		WHERE C_GUID=@C_GUID
			
		/**统计一级费用科目分类的总成本与费用列表*/
		DECLARE @tempSumAmount TABLE(
			SumAmount	FLOAT	
		)				
		BEGIN
			INSERT INTO @tempSumAmount	
			SELECT SUM(CASE  WHEN Y.TCurrency = IE.Currency AND Y.FCurrency = @CompanyCurrency	 THEN IE.SumAmount*Rate
			WHEN  IE.Currency =@CompanyCurrency   THEN IE.SumAmount
			ELSE 0 END)
			FROM  dbo.T_IERecord IE    
			LEFT JOIN  dbo.T_RateHistory Y ON Y.C_GUID = IE.C_GUID        
			WHERE IE.C_GUID=@C_GUID 
			AND(IE.AffirmDate >= @dateBegin)
			AND(IE.AffirmDate < DATEADD(day,1,@dateEnd))
			AND IE.InvType =@InvType
			AND IE.IE_Flag= 'E' 
			AND IE.State='应付'
			AND IE.AffirmDate > Y.Date 
		 END  
		  /**查询应付款总金额*/	
		  SELECT @SumAmountSUM=SUM(SumAmount)
		  FROM @tempSumAmount
		  DECLARE @temp TABLE(
			rownumber INT,
			IE_GUID NVARCHAR(40),
			IEGroup NVARCHAR(40),
			SumAmount NVARCHAR(40),
			Currency NVARCHAR(40),
			ReceiveRatio	NVARCHAR(40)	
		 )	
		BEGIN
			INSERT INTO @temp	
		    /**统计一级费用科目分类的总成本与费用列表*/
			SELECT  ROW_NUMBER()OVER(ORDER BY NEWID() DESC) rownumber,'',IE.IEGroup,
			SUM(IE.SumAmount) AS SumAmount,
			IE.Currency,(SUM(IE.SumAmount))/@SumAmountSUM
			FROM  dbo.T_IERecord IE
			LEFT JOIN dbo.T_BusinessPartner BP ON BP.BP_GUID = IE.RPer
			WHERE IE.C_GUID=@C_GUID	
			AND IE.IE_Flag= 'E'
			AND IE.State='应付'			
			AND(IE.AffirmDate >= @dateBegin)
			AND(IE.AffirmDate < DATEADD(day,1,@dateEnd))
			AND IE.InvType =@InvType
			GROUP BY IE.Currency,IE.IEGroup     
			
			UPDATE @temp SET IE_GUID=NEWID()
			
			SELECT @Count = COUNT(*) FROM @temp;
			SELECT T.IE_GUID,T.IEGroup,T.SumAmount,T.Currency,(CAST(round(T.ReceiveRatio,4)AS NUMERIC(5,4))) AS ReceiveRatio
			FROM @temp T
			WHERE (T.rownumber>= ((@PageIndex - 1)*@PageSize) + 1
			AND T.rownumber <= @PageIndex*@PageSize )
			OR (@PageSize = -1)          
		END		
			
END



