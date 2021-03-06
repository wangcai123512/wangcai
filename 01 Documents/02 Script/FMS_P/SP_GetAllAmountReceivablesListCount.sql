USE [FMS_Develop]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetAllAmountReceivablesListCount]    Script Date: 06/16/2017 16:56:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,hdy>
-- Create date: <Create Date,,04/23/2017>
-- Description:	获取客户应收款总记录汇总
-- =============================================
ALTER PROCEDURE [dbo].[SP_GetAllAmountReceivablesListCount]
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
		DECLARE @GETDATE VARCHAR(40)    /**获取当前时间*/
		DECLARE @SumAmountARBP FLOAT    /**期初应收账款*/
		DECLARE @CountBegin VARCHAR(40)    /**查看期初初始账款表条件数量*/
		DECLARE @CountSumDay VARCHAR(40)    
		
		SET  @NEWGUID = NEWID();	
		SET  @GETDATE = GETDATE();
		
		/**查询统计货币*/
		SELECT @CompanyCurrency= Code 
		FROM  dbo.R_CompanyCurrceny   
		WHERE C_GUID=@C_GUID          
		
		/**查询数据库中是否有该数据然后判断赋值*/
	    SELECT	@CountBegin=COUNT(*)  FROM  dbo.T_IERecord  IE  
		LEFT JOIN dbo.T_BusinessPartner BP ON BP.BP_GUID = IE.RPer
        WHERE IE.C_GUID = @C_GUID
		AND   InvType = '初始账款' 
		AND   IE_Flag='I'
		AND   State='应收'
		AND   BP.IsCustomer ='1'
		
		BEGIN	
			 
			IF @CountBegin>0
			SELECT	@SumAmountARBP=SumAmount  FROM  dbo.T_IERecord  IE  
			LEFT JOIN dbo.T_BusinessPartner BP ON BP.BP_GUID = IE.RPer
			WHERE IE.C_GUID = @C_GUID
			AND 	InvType = '初始账款' 
			AND     IE_Flag='I'
			AND      State='应收'
			AND     BP.IsCustomer ='1'
			ELSE
				SET @SumAmountARBP='0'
		END
		
		/**首先统计相同客户以及货币然后将货币与统计货币不同的客户的货币进行转换的临时表*/
		DECLARE @tempSumAmount TABLE(
			SumAmount	FLOAT	
		)				
		BEGIN
			
		    INSERT INTO @tempSumAmount	        
			SELECT SUM(CASE  WHEN Y.TCurrency = IE.Currency AND Y.FCurrency = @CompanyCurrency	
			AND IE.IE_Flag= 'I' AND IE.State='应收' AND IE.InvType<>'初始账款' THEN IE.SumAmount*Rate
			WHEN  IE.Currency =@CompanyCurrency AND IE.IE_Flag= 'I' AND IE.State='应收'  AND IE.InvType<>'初始账款' 
			THEN round(IE.SumAmount/11,5)/**汇率表设计...目前只能改成这样*/
			ELSE 0 END)  
			FROM  dbo.T_IERecord IE    
			LEFT JOIN dbo.T_RecPayRecord RP ON RP.IE_GUID = IE.IE_GUID     
			LEFT JOIN  dbo.T_RateHistory Y ON Y.C_GUID = IE.C_GUID        
			LEFT JOIN dbo.T_BusinessPartner BP ON BP.BP_GUID = IE.RPer                
			WHERE IE.C_GUID=@C_GUID
			AND IE.AffirmDate>y.Date	   		
			GROUP BY IE.RPer,IE.Currency								    
		END	
		
	    /**统计临时表中总金额*/
		SELECT @SumAmountSUM=SUM(SumAmount)+@SumAmountARBP
		FROM @tempSumAmount
		/**统计总天数*/
		DECLARE @tempSumDay TABLE(
			SumDay	FLOAT	
		)		 
		 BEGIN
			INSERT INTO @tempSumDay		
			SELECT  SUM((DATEDIFF(DAY,IE.AffirmDate,IE.Date))+1)*((SUM(IE.SumAmount)-
			SUM(CASE WHEN RP.Record = '已销账' AND RP.IE_GUID = IE.IE_GUID 
			THEN RP.SumAmount ELSE 0 END))/@SumAmountSUM)  AS TotalDays    
			FROM  dbo.T_IERecord IE
			LEFT JOIN dbo.T_BusinessPartner BP ON BP.BP_GUID = IE.RPer
			LEFT JOIN dbo.T_RecPayRecord RP ON RP.IE_GUID = IE.IE_GUID
			WHERE IE.C_GUID=@C_GUID	
			AND IE.IE_Flag= 'I'
			AND IE.State='应收'
			AND IE.InvType<>'初始账款'
			GROUP BY IE.Currency,BP.Name      
		END	
				
		SELECT @CountSumDay=SUM(CAST(round(SumDay,0)AS NUMERIC(5,0)))
		
		
		FROM @tempSumDay	
		 /*将统计临时表中总金额以及id插入到临时表*/  	  
		 DECLARE @temp TABLE(
			IE_GUID NVARCHAR(40),
			SumAmount NVARCHAR(40),
			Sumday  NVARCHAR(40)
		 )	
		 BEGIN
			INSERT INTO @temp
			VALUES(NEWID(),@SumAmountSUM,@CountSumDay)		

			SELECT @Count = COUNT(*) FROM @temp;          
			SELECT T.IE_GUID,T.SumAmount,T.Sumday                
			FROM @temp T
		 END		
			
END



