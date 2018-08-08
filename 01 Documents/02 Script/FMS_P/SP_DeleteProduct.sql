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
-- Description:	ɾ����Ʒ 
-- =============================================
CREATE PROCEDURE [dbo].[SP_DeleteProduct]
	@guid               VARCHAR(40),        --Ҫɾ����GUID 
	@c_guid             VARCHAR(40),        --��˾GUID 
	@result             INT         OUT     --ִ�н��1=�ɹ���-1=����ɾ��
AS
BEGIN
 
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	
	--1. �ȼ��Ҫɾ���Ĳ�Ʒ�Ƿ��Ѿ�ʹ�û����
 
	DECLARE @saled_amount    DECIMAL(18,4) 
	 
	 
	SET @saled_amount=-1
	
	SET @result=1   --Ĭ�ϳɹ�
	
	SELECT @saled_amount= ISNULL(Saled_count  ,0)
	FROM dbo.T_Product
	WHERE GUID = @guid
	    AND C_GUID = @c_guid
	    
	IF( @saled_amount=0)
	BEGIN
	    BEGIN TRAN;
	       
	        --3. ɾ����Ʒ
	        DELETE FROM dbo.T_Product
	        WHERE GUID=@guid 
	            AND C_GUID=@c_guid
	        
	        IF @@ERROR>0
	        BEGIN
	            ROLLBACK;
	        END
	        
	        --ɾ����Ʒ��ϸ
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
	    SET @result=-1   --����ɾ��	    
	END 
    
END

GO


