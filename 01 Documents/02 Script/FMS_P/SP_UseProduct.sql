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
-- Description:	ʹ�ò�Ʒ 
-- =============================================
CREATE PROCEDURE [dbo].[SP_UseProduct]
	@from_guid          VARCHAR(40),        --ʹ�õĲ�Ʒ��Դ
	@use_amount         DECIMAL(18,4),      --ʹ�ý��
	@type_to            VARCHAR(40),        --�²�Ʒ���ID
	@sub_type_to        VARCHAR(40),        --�²�Ʒ�����ID
	@result            	INT     OUT         --ִ�н��1=�ɹ���-1=������ʹ�õ���������װ�����Ĳ�Ʒ
AS
BEGIN
 
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	
	-- ���ʹ���Ƿ���ȷ
	-- 1.������ʹ�õ���������װ�����Ĳ�Ʒ
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
	    --1. ���²�Ʒ��Դ����ʹ�ý����
	    UPDATE dbo.T_Product
	    SET Used_Amount=Used_Amount+@use_amount,
	        Stock_Amount=Stock_Amount-@use_amount
	    WHERE GUID=@from_guid
    	
    	
	    --2. ����µĲ�Ʒ 
	    -- ͬһ�� ͬ1����Ʒ�����ۼ�
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
	SET @result=1;  --ִ�гɹ�
    
END

GO


