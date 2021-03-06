	USE [FMS_Develop]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetTotalAmountReceivablesListCount]    Script Date: 06/06/2017 11:00:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,hdy>
-- Create date: <Create Date,,04/20/2017>
-- Description:	获取客户应收款总金额列表
-- =============================================
ALTER PROCEDURE [dbo].[SP_GetTotalAmountReceivablesListCount]
	@PageSize INT = -1,
	@PageIndex INT = 1,
	@Count INT = 0 OUT,
	@C_GUID NVARCHAR(50),
	@RPer NVARCHAR(40) 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
		DECLARE @CompanyCurrency NVARCHAR(50) /**查询统计货币*/
		DECLARE @NEWGUID VARCHAR(40) 	/**生成GUID*/
		DECLARE @GETDATE VARCHAR(40)    /**获取当前时间*/
		DECLARE @SumAmountSUM FLOAT   /**获取应付款总金额*/
		DECLARE @SumAmountARBP FLOAT    /**期初应收账款*/
		SET  @NEWGUID = NEWID();	
		SET  @GETDATE = GETDATE();
		
		 /**查询统计货币*/
		SELECT @CompanyCurrency= Code
		FROM  dbo.R_CompanyCurrceny 
		WHERE C_GUID=@C_GUID
			
			
		 /**查询期初应收账款*/
	    SELECT @SumAmountARBP=ISNULL(SumAmount,0) FROM  dbo.T_IERecord  IE
		LEFT JOIN dbo.T_BusinessPartner BP ON BP.BP_GUID = IE.RPer
        WHERE IE.C_GUID = @C_GUID
		AND 	InvType = '初始账款' 
		AND     IE_Flag='I'
		AND      State='应收'
		AND     BP.IsCustomer ='1'		
			
			
		/**首先统计相同客户以及货币然后将货币与统计货币不同的客户的货币进行转换的临时表*/
		DECLARE @tempSumAmount TABLE(
			SumAmount	FLOAT	
		)				
		BEGIN
			INSERT INTO @tempSumAmount	
			/**首先统计相同客户以及货币然后将货币与统计货币不同的客户的货币进行转换*/  
			SELECT  SUM(CASE  WHEN Y.TCurrency = IE.Currency AND Y.FCurrency = @CompanyCurrency	
			AND IE.IE_Flag= 'I' AND IE.State='应收'   AND IE.InvType<>'初始账款'
			THEN IE.SumAmount*Rate 
			WHEN  IE.Currency =@CompanyCurrency  AND IE.InvType<>'初始账款'
			AND IE.IE_Flag= 'I' AND IE.State='应收'  
			THEN IE.SumAmount
			ELSE 0 END) -SUM(CASE WHEN RP.Record = '已销账' AND RP.IE_GUID = IE.IE_GUID   
			AND Y.TCurrency = RP.Currency AND Y.FCurrency = @CompanyCurrency THEN RP.SumAmount*Rate     
			WHEN RP.Record = '已销账' AND RP.IE_GUID = IE.IE_GUID    
			AND RP.Currency=@CompanyCurrency THEN RP.SumAmount ELSE 0 END) AS SumAmount     
			FROM  dbo.T_IERecord IE    
			LEFT JOIN dbo.T_RecPayRecord RP ON RP.IE_GUID = IE.IE_GUID     
			LEFT JOIN  dbo.T_RateHistory Y ON Y.C_GUID = IE.C_GUID        
			WHERE IE.C_GUID=@C_GUID   
			GROUP BY IE.RPer,IE.Currency 
		 END  
		  /**查询应付款总金额*/	
		  SELECT @SumAmountSUM=SUM(SumAmount)+@SumAmountARBP
		  FROM @tempSumAmount
		  DECLARE @temp TABLE(
			rownumber INT,
			IE_GUID NVARCHAR(40),
			RPerName NVARCHAR(40),
			SumAmount NVARCHAR(40),
			Currency NVARCHAR(40),
			TotalDays NVARCHAR(40),
			OverdueDays NVARCHAR(40),
			ReceiveRatio	NVARCHAR(40)	
		 )	
		BEGIN
			INSERT INTO @temp		
			/**查询账号下时间范围内的净现金流**/
			SELECT  ROW_NUMBER()OVER(ORDER BY @NEWGUID DESC) rownumber,'',BP.Name AS RPerName,
			SUM(IE.SumAmount)-SUM(CASE WHEN RP.Record = '已销账' AND RP.IE_GUID = IE.IE_GUID THEN RP.SumAmount ELSE 0 END) AS SumAmount,
			IE.Currency AS Currency,SUM(DATEDIFF(DAY,IE.AffirmDate,IE.Date))*((SUM(IE.SumAmount)-SUM(CASE WHEN RP.Record = '已销账' AND RP.IE_GUID = IE.IE_GUID THEN RP.SumAmount ELSE 0 END))/@SumAmountSUM)  AS TotalDays,
			SUM(CASE WHEN DATEDIFF(DAY,IE.Date,@GETDATE)>0
			 THEN DATEDIFF(DAY,IE.Date,@GETDATE) ELSE 0 END) AS OverdueDays,
			((SUM(IE.SumAmount)-SUM(CASE WHEN RP.Record = '已销账' AND RP.IE_GUID = IE.IE_GUID THEN RP.SumAmount ELSE 0 END))/@SumAmountSUM)  AS ReceiveRatio     
			FROM  dbo.T_IERecord IE
			LEFT JOIN dbo.T_BusinessPartner BP ON BP.BP_GUID = IE.RPer
			LEFT JOIN dbo.T_RecPayRecord RP ON RP.IE_GUID = IE.IE_GUID
			WHERE IE.C_GUID=@C_GUID	
			AND IE.IE_Flag= 'I'
			AND IE.State='应收'
			AND IE.InvType<>'初始账款'
			AND(IE.RPer = @RPer OR @RPer IS NULL OR LEN(@RPer) = 0)			
			GROUP BY IE.Currency,BP.Name
			ORDER BY substring(BP.Name,1,1),SumAmount DESC, TotalDays DESC,
			OverdueDays DESC,ReceiveRatio DESC     
			

			UPDATE @temp SET IE_GUID=NEWID()
			
			SELECT @Count = COUNT(*) FROM @temp;
			SELECT T.IE_GUID,T.RPerName,T.SumAmount,T.Currency, (CAST(round(T.TotalDays,0)AS NUMERIC(5,0))) AS TotalDays,T.OverdueDays,(CAST(round(T.ReceiveRatio,4)AS NUMERIC(5,4))) AS ReceiveRatio
			FROM @temp T
			WHERE (T.rownumber>= ((@PageIndex - 1)*@PageSize) + 1
			AND T.rownumber <= @PageIndex*@PageSize )
			OR (@PageSize = -1)          
		END		
			
END



