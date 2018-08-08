USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_SalesProduct]    Script Date: 11/25/2016 10:32:21 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_SalesProduct]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_SalesProduct]
GO

USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_SalesProduct]    Script Date: 11/25/2016 10:32:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<liujf >
-- Create date: 2016/11/25
-- Description:	核销产品 
-- =============================================
CREATE PROCEDURE [dbo].[SP_SalesProduct]
	@p_guid                 VARCHAR(40),            --要核销的产品GUID 
	@saled_count            DECIMAL(18,4),          --核销的产品数量	
	@c_guid                 VARCHAR(40),            --公司GUID 
	@ie_guid                VARCHAR(MAX),           --收入ID
	@RPer                   VARCHAR(40)=null,
	@Creator                VARCHAR(40)=NULL, 
	@AffirmDate             DATETIME=NULL,
	@end_date               DATETIME=NULL,
	@Amount                 DECIMAL(18,2)=null,
	@TaxationAmount         DECIMAL(18,2)=null,
	@TaxationType           VARCHAR(40)=null,
	@SumAmount              DECIMAL(18,2)=null,
	@Remark                 VARCHAR(200)=null,
	@Currency               VARCHAR(20)=NULL,  
	@result                 INT         OUT         --执行结果1=成功，-1=无权限不可核销
AS
BEGIN
 
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
		
	DECLARE @today  VARCHAR(10)	
	SET @today =CONVERT(VARCHAR(10),GETDATE(),111)
	
	-- 检查权限
	DECLARE @cnt    INT
	
	SELECT @cnt = COUNT(GUID)
	FROM dbo.T_Product
	WHERE C_GUID = @c_guid
	    AND GUID=@p_guid
	   
	IF @cnt = 0
	BEGIN
	   SET @result = -1 
	END
	ELSE 
	BEGIN
	    BEGIN TRAN;
	        -- 1. 更新产品的核销金额和库存
	        UPDATE dbo.T_Product
	        SET Saled_count = ISNULL(Saled_count,0) + @saled_count,
	            stock_count = stock_count -@saled_count
	        WHERE C_GUID = @c_guid
	            AND GUID=@p_guid  
        	
	        --2. 将传入的收入ID转换为表,批量写入核销明细表
	        -- 多条收入时 核销金额都一样 
            DECLARE @batch_id   VARCHAR(40);
            SET @batch_id =NEWID();
            
            PRINT '222'
            
	        IF @ie_guid <>''
	        BEGIN              
                INSERT INTO dbo.T_Product_Saled
                    ( id ,
                      sale_batch_id ,
                      product_guid ,
                      ie_guid ,
                      saled_date ,
                      saled_amount
                    ) 
                SELECT NEWID() AS id ,@batch_id, @p_guid,
                short_str AS ie_guid,@today AS saled_date, @saled_count
                FROM dbo.FUN_SPLIT(@ie_guid,',')
                
                IF @@ERROR>0
	            BEGIN
	                ROLLBACK;
	            END
	        END	        
	        ELSE  
	        BEGIN
	            --创建新的收入
	            DECLARE @new_ie_guid    VARCHAR(50)
	            SET @new_ie_guid= NEWID(); 
	            
	            INSERT INTO dbo.T_IERecord
	                    ( IE_GUID ,  IE_Flag , InvType , RPer ,  Creator ,
	                      CreateDate ,  C_GUID ,  AffirmDate , Date ,
	                      Amount , TaxationAmount , TaxationType , SumAmount ,
	                      Remark , Currency ,  State , Profit_GUID  
	                    )
	            VALUES  ( @new_ie_guid, 
	                      'I' , 
	                      N'主营业务收入' , 
	                      @RPer ,  
	                      @Creator , -- Creator - nvarchar(40)
	                      CONVERT(VARCHAR(20),GETDATE(),120) , -- CreateDate - datetime
	                      @c_guid , -- C_GUID - nvarchar(40)
	                      @AffirmDate , -- AffirmDate - datetime
	                        @end_date , -- Date - datetime
	                      @Amount , -- Amount - decimal
	                      @TaxationAmount , -- TaxationAmount - decimal
	                      @TaxationType, -- TaxationType - nvarchar(40)
	                      @SumAmount , -- SumAmount - decimal
	                      @Remark , -- Remark - nvarchar(200)
	                      @Currency, -- Currency - nvarchar(20) 
	                      N'应收' , -- State - nvarchar(50)
	                      N'D27CA8F5-A98C-41E4-8E49-E0BE34E93035'   -- Profit_GUID - nvarchar(40) 
	                    ) 
	            IF @@ERROR>0
	            BEGIN
	                ROLLBACK;
	            END    
	            
	            --写入关系表
	            INSERT INTO dbo.T_Product_Saled
                    ( id ,
                      sale_batch_id ,
                      product_guid ,
                      ie_guid ,
                      saled_date ,
                      saled_amount
                    )     
	            SELECT NEWID() AS id ,@batch_id, @p_guid,
                @new_ie_guid,@today AS saled_date, @saled_count 
	            IF @@ERROR>0
	            BEGIN
	                ROLLBACK;
	            END      
	        END 
	        
	        --成本外支出转成本
	        
	        EXEC SP_SaveCost @p_guid,@saled_count,@c_guid
	        
	        SET @result = 1 
    	    
	    COMMIT TRAN;
	END   
END

GO


