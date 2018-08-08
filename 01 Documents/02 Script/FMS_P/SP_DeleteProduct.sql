USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_DeleteProduct]    Script Date: 11/25/2016 10:32:21 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_DeleteProduct]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_DeleteProduct]
GO

USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_DeleteProduct]    Script Date: 11/25/2016 10:32:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<liujf >
-- Create date: 2016/11/25
-- Description:	删除产品 
-- =============================================
CREATE PROCEDURE [dbo].[SP_DeleteProduct]
	@guid               VARCHAR(40),        --要删除的GUID 
	@c_guid             VARCHAR(40),        --公司GUID 
	@result             INT         OUT     --执行结果1=成功，-1=不可删除
AS
BEGIN
 
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	
	--1. 先检查要删除的产品是否已经使用或核销
 
	DECLARE @saled_amount    DECIMAL(18,4) 
	 
	 
	SET @saled_amount=-1
	
	SET @result=1   --默认成功
	
	SELECT @saled_amount= ISNULL(Saled_count  ,0)
	FROM dbo.T_Product
	WHERE GUID = @guid
	    AND C_GUID = @c_guid
	    
	IF( @saled_amount=0)
	BEGIN
	    BEGIN TRAN;
	       
	        --3. 删除产品
	        DELETE FROM dbo.T_Product
	        WHERE GUID=@guid 
	            AND C_GUID=@c_guid
	        
	        IF @@ERROR>0
	        BEGIN
	            ROLLBACK;
	        END
	        
	        --删除产品明细
	        DELETE FROM dbo.T_Product_Details
	        WHERE product_id=@guid
	        IF @@ERROR>0
	        BEGIN
	            ROLLBACK;
	        END
    	
	    COMMIT TRAN;
	END 
	ELSE
	BEGIN
	    SET @result=-1   --不可删除	    
	END 
    
END

GO


