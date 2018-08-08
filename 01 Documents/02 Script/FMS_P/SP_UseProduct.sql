USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_UseProduct]    Script Date: 11/25/2016 10:32:21 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_UseProduct]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_UseProduct]
GO

USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_UseProduct]    Script Date: 11/25/2016 10:32:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<liujf >
-- Create date: 2016/11/25
-- Description:	使用产品 
-- =============================================
CREATE PROCEDURE [dbo].[SP_UseProduct]
	@from_guid          VARCHAR(40),        --使用的产品来源
	@use_amount         DECIMAL(18,4),      --使用金额
	@type_to            VARCHAR(40),        --新产品类别ID
	@sub_type_to        VARCHAR(40),        --新产品子类别ID
	@result            	INT     OUT         --执行结果1=成功，-1=不可以使用到从物料组装过来的产品
AS
BEGIN
 
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	
	-- 检查使用是否正确
	-- 1.不可以使用到从物料组装过来的产品
	DECLARE @count  INT ;  
	DECLARE @today  VARCHAR(10);
	
	SET @today =CONVERT(VARCHAR(10),GETDATE(),111)
	
	SELECT @count=COUNT(GUID)
	FROM dbo.T_Product 
	WHERE TypeId=@type_to
	    AND SubTypeId=@sub_type_to
	    AND ISNULL(Parent_GUID,'')=''
	IF(@count>0)
	BEGIN
	    SET @result=-1;
	    RETURN;
	END
	
	BEGIN TRAN;
	    --1. 更新产品来源的已使用金额，库存
	    UPDATE dbo.T_Product
	    SET Used_Amount=Used_Amount+@use_amount,
	        Stock_Amount=Stock_Amount-@use_amount
	    WHERE GUID=@from_guid
    	
    	
	    --2. 添加新的产品 
	    -- 同一天 同1个产品类别的累计
	    IF EXISTS(SELECT C_GUID 
	                FROM dbo.T_Product
	                WHERE TypeId=@type_to AND SubTypeId=@sub_type_to AND Create_Date=@today)
	    BEGIN
	    
	    END 
	    ELSE
	    BEGIN
	        INSERT dbo.T_Product
	                ( GUID ,
	                  C_GUID ,
	                  Create_Date ,
	                  Amount ,
	                  Currency ,
	                  Used_Amount ,
	                  Saled_Amount ,
	                  Stock_Amount ,
	                  TypeId ,
	                  SubTypeId ,
	                  Parent_GUID
	                ) 
	        SELECT NEWID(),C_GUID,CONVERT(VARCHAR(10),GETDATE(),111),@use_amount,Currency,0,0,@use_amount,@type_to,@sub_type_to,@from_guid
	        FROM dbo.T_Product
	        WHERE GUID=@from_guid
	    END
	    
	    
    	
	    IF @@ERROR>0
	    BEGIN
	        ROLLBACK;
	    END
	    
	COMMIT TRAN;
	SET @result=1;  --执行成功
    
END

GO


