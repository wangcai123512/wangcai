USE [FMS_Develop]
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdQAAccounts]    Script Date: 06/18/2017 17:13:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,hdy>
-- Create date: <Create Date,,06/05/2017>
-- Description:	获取应收款更新快速关注应收款
-- =============================================
ALTER PROCEDURE [dbo].[SP_UpdQAAccounts]
	@c_guid NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
		DECLARE @CompanyCurrency NVARCHAR(50) /**查询统计货币*/
		DECLARE @NEWGUID VARCHAR(40) 	/**生成GUID*/
		DECLARE @SumAmountSUM FLOAT   /**获取总金额*/
		DECLARE @GETDATE VARCHAR(40)    /**获取当前时间*/
		DECLARE @SumAmountARBP FLOAT    /**期初应收账款*/
		DECLARE @CountSumDay VARCHAR(40)    
		
		SET  @NEWGUID = NEWID();	
		SET  @GETDATE = GETDATE();
		
		/**查询统计货币*/
		SELECT @CompanyCurrency= Code
		FROM  dbo.R_CompanyCurrceny 
		WHERE C_GUID=@C_GUID
		
	    /**查询期初应收账款*/
	    SELECT @SumAmountARBP= ISNULL(SumAmount,0) FROM  dbo.T_IERecord  IE
		LEFT JOIN dbo.T_BusinessPartner BP ON BP.BP_GUID = IE.RPer
        WHERE IE.C_GUID = @c_guid
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
		
	    /**统计临时表中总金额*/	
		SELECT @SumAmountSUM=ISNULL((SUM(SumAmount)+@SumAmountARBP),0)
		FROM @tempSumAmount
		
		UPDATE dbo.T_QuickAttention 
		SET attention_type_amount=@SumAmountSUM,
		statistical_time=GETDATE(),statistical_currency=@CompanyCurrency 
		WHERE  c_guid=@c_guid
		AND attention_type='应收款总金额'	 
		
		
		
			
			
END



