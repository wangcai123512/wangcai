USE [FMS_Develop]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetAllAmountOverdueRListCount]    Script Date: 06/16/2017 16:07:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,hdy>
-- Create date: <Create Date,,05/03/2017>
-- Description:	获取客户逾期应收款总记录汇总
-- =============================================
ALTER PROCEDURE [dbo].[SP_GetAllAmountOverdueRListCount]
	@PageSize INT = -1,
	@PageIndex INT = 1,
	@Count INT = 0 OUT,	
	@C_GUID NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
		DECLARE @CompanyCurrency NVARCHAR(50) /**查询统计货币*/
		DECLARE @NEWGUID VARCHAR(40) 	/**生成GUID*/
		DECLARE @SumAmountSUM FLOAT   /**获取总金额*/
		SET  @NEWGUID = NEWID();	
		
		 /**查询统计货币*/
		SELECT @CompanyCurrency= Code
		FROM  dbo.R_CompanyCurrceny 
		WHERE C_GUID=@C_GUID
			
			
		/**首先统计相同客户以及货币然后将货币与统计货币不同的客户的货币进行转换的临时表*/
		
		/**IE对应的应付说明还没有收款结束还有钱没有收其次用总金额减去已收的金额等于还未收款金额,其次判断当前日期与账期截止日期*/
		
		DECLARE @tempSumAmount TABLE(
			SumAmount	FLOAT	
		)				
		BEGIN
			INSERT INTO @tempSumAmount	
			SELECT SUM(CASE  WHEN Y.TCurrency = IE.Currency AND Y.FCurrency = @CompanyCurrency AND    
			GETDATE()>IE.Date AND IE.IE_Flag= 'I' AND IE.State='应收' AND IE.InvType<>'初始账款' THEN IE.SumAmount*Rate
			WHEN  IE.Currency =@CompanyCurrency AND IE.IE_Flag= 'I' AND IE.State='应收'  AND IE.InvType<>'初始账款'
			AND GETDATE()>IE.Date THEN IE.SumAmount
			ELSE 0 END) -SUM(CASE WHEN RP.Record = '已销账' AND RP.IE_GUID = IE.IE_GUID   
			AND Y.TCurrency = RP.Currency AND Y.FCurrency = @CompanyCurrency THEN RP.SumAmount*Rate      
			WHEN RP.Record = '已销账' AND RP.IE_GUID = IE.IE_GUID                          
			AND RP.Currency=@CompanyCurrency THEN RP.SumAmount ELSE 0 END)   
			FROM  dbo.T_IERecord IE    
			LEFT JOIN dbo.T_RecPayRecord RP ON RP.IE_GUID = IE.IE_GUID     
			LEFT JOIN  dbo.T_RateHistory Y ON Y.C_GUID = IE.C_GUID 
			AND IE.AffirmDate > Y.Date   	          
			WHERE IE.C_GUID=@C_GUID   
			GROUP BY IE.RPer,IE.Currency		
		END	
		
	    /**统计临时表中总金额*/	
		SELECT @SumAmountSUM=SUM(SumAmount)
		FROM @tempSumAmount
		 
		 /*将统计临时表中总金额以及id插入到临时表*/  	  
		 DECLARE @temp TABLE(
			IE_GUID NVARCHAR(40),
			SumAmount NVARCHAR(40)
		 )	
		 BEGIN
			INSERT INTO @temp
			VALUES(NEWID(),@SumAmountSUM)		

			SELECT @Count = COUNT(*) FROM @temp;          
			SELECT T.IE_GUID,T.SumAmount                
			FROM @temp T
		 END		
			
END



