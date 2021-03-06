USE [FMS_Develop]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetTotalAmountOverdueRListCount]    Script Date: 06/16/2017 16:12:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
	-- =============================================
	-- Author:		<Author,,hdy>
	-- Create date: <Create Date,,05/04/2017>
	-- Description:	获取客户逾期应收款总金额
	-- =============================================
	ALTER PROCEDURE [dbo].[SP_GetTotalAmountOverdueRListCount]
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
			SET  @NEWGUID = NEWID();	
			SET  @GETDATE = GETDATE();
			
			 /**查询统计货币*/
			SELECT @CompanyCurrency= Code
			FROM  dbo.R_CompanyCurrceny 
			WHERE C_GUID=@C_GUID
				
			/**首先统计相同客户以及货币然后将货币与统计货币不同的客户的货币进行转换的临时表*/
			DECLARE @tempSumAmount TABLE(
				SumAmount	FLOAT	
			)				
			BEGIN
				INSERT INTO @tempSumAmount	
				/**首先统计相同客户以及货币然后将货币与统计货币不同的客户的货币进行转换*/  
				SELECT  SUM(CASE  WHEN Y.TCurrency = IE.Currency AND Y.FCurrency = @CompanyCurrency 
				AND GETDATE()>IE.Date	AND IE.IE_Flag= 'I' AND IE.State='应收' 
				AND IE.InvType<>'初始账款' THEN IE.SumAmount*Rate
				WHEN  IE.Currency =@CompanyCurrency  AND GETDATE()>IE.Date AND IE.IE_Flag= 'I' 
				AND IE.State='应收' AND IE.InvType<>'初始账款'  THEN IE.SumAmount
				ELSE 0 END) -SUM(CASE WHEN RP.Record = '已销账' AND RP.IE_GUID = IE.IE_GUID   
				AND Y.TCurrency = RP.Currency AND Y.FCurrency = @CompanyCurrency THEN RP.SumAmount*Rate     
				WHEN RP.Record = '已销账' AND RP.IE_GUID = IE.IE_GUID    
				AND RP.Currency=@CompanyCurrency THEN RP.SumAmount ELSE 0 END) AS SumAmount     
				FROM  dbo.T_IERecord IE    
				LEFT JOIN dbo.T_RecPayRecord RP ON RP.IE_GUID = IE.IE_GUID     
				LEFT JOIN  dbo.T_RateHistory Y ON Y.C_GUID = IE.C_GUID        
				WHERE IE.C_GUID=@C_GUID 
			    AND IE.AffirmDate > Y.Date   	  
				GROUP BY IE.RPer,IE.Currency 
			 END  
			  /**查询应付款总金额*/	
			  SELECT @SumAmountSUM=SUM(SumAmount)
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
				SELECT  ROW_NUMBER()OVER(ORDER BY @NEWGUID DESC) rownumber,'',BP.Name AS RPerName,
				SUM(IE.SumAmount)-SUM(CASE WHEN RP.Record = '已销账' AND RP.IE_GUID = IE.IE_GUID 
				THEN RP.SumAmount ELSE 0 END) AS SumAmount,IE.Currency AS Currency,SUM(DATEDIFF(DAY,IE.AffirmDate,IE.Date))
				AS TotalDays,SUM(DATEDIFF(DAY,IE.Date,@GETDATE)) AS OverdueDays,((SUM(IE.SumAmount)-SUM(CASE WHEN RP.Record = '已销账'
				AND RP.IE_GUID = IE.IE_GUID THEN RP.SumAmount ELSE 0 END))/@SumAmountSUM)  AS ReceiveRatio     
				FROM  dbo.T_IERecord IE
				LEFT JOIN dbo.T_BusinessPartner BP ON BP.BP_GUID = IE.RPer
				LEFT JOIN dbo.T_RecPayRecord RP ON RP.IE_GUID = IE.IE_GUID
				WHERE IE.C_GUID=@C_GUID	
				AND IE.IE_Flag= 'I'
				AND IE.State='应收'
				AND IE.InvType<>'初始账款'
				AND(IE.RPer = @RPer OR @RPer IS NULL OR LEN(@RPer) = 0)		
				AND GETDATE()>IE.Date	
				GROUP BY IE.Currency,BP.Name

				UPDATE @temp SET IE_GUID=NEWID()
				
				
				SELECT @Count = COUNT(*) FROM @temp;
				SELECT T.IE_GUID,T.RPerName,T.SumAmount,T.Currency,T.TotalDays,T.OverdueDays,(CAST(round(T.ReceiveRatio,4)AS NUMERIC(5,4))) AS ReceiveRatio
				FROM @temp T
				WHERE (T.rownumber>= ((@PageIndex - 1)*@PageSize) + 1
				AND T.rownumber <= @PageIndex*@PageSize )
				OR (@PageSize = -1)	
			END		
				
	END



