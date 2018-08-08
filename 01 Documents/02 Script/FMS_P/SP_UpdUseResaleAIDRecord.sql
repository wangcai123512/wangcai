USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_UpdUseResaleAIDRecord]    Script Date: 12/28/2016 15:53:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<hdy>
-- Create date: <Create Date,16.11.14,>
-- Description:	�������ʲ�ʹ�÷��ü�¼
-- =============================================
ALTER PROCEDURE [dbo].[SP_UpdUseResaleAIDRecord]
	@GUID VARCHAR(40),/**GUID**/
	@AmountUsed DECIMAL(18,4),/**ʹ�ý��**/
	@InventoryAmount DECIMAL(18,4),/**�����**/
	@State VARCHAR(40)=NULL,/**״̬**/
    @C_GUID NVARCHAR(40),/**C_GUID**/
	@ManufacturedType NVARCHAR(40)=NULL,/**�����Ʒ�����**/
	@ManufacturedTypeSub NVARCHAR(40)=NULL,/**�����Ʒ�������*/
	@Currency NVARCHAR(20)=NULL	/**����*/	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	
	
			DECLARE @today  VARCHAR(10)				
			DECLARE @NEWGUID VARCHAR(40) 	/**����GUID*/
			DECLARE @ProductAmount DECIMAL 	 /**product����amount*/	
			DECLARE @ProductStock_Amount DECIMAL 	 /**product����Stock_Amount*/	
			SET @today =CONVERT(VARCHAR(10),GETDATE(),111)
			SET  @NEWGUID=NEWID();
			DECLARE @Product_GUID  VARCHAR(40)
			
			
			SELECT @Product_GUID=GUID,@ProductAmount=Amount,@ProductStock_Amount=Stock_Amount
			FROM dbo.T_Product 
			WHERE C_GUID = @C_GUID
			AND(TypeId = @ManufacturedType)
			AND(SubTypeId = @ManufacturedTypeSub)
			AND(Create_Date= @today)
			
				
	BEGIN TRAN; 
		BEGIN  
		   -- ���ȸ���AID����״̬
		   
			UPDATE dbo.T_AIDRecord 
			SET State=@State  
			WHERE GUID=@GUID	
			
			    
			--ͬʱ��Ҫ��T_Product���һ������,��Ҫ�ж�T_Product��û�и����������,
			--��ǰ��ӵ�ʱ���ڱ�����û��ͬһ���
			--�����������ڵ�ǰ���м�¼������ʹ�ý��
			--��������һ���µ�ʹ�ü�¼	
			
			IF @Product_GUID IS NULL
				INSERT INTO dbo.T_Product(GUID,C_GUID,Create_Date,Amount,TypeId,SubTypeId,
				Stock_Amount,Currency)
				VALUES(@NEWGUID,@C_GUID,@today,@AmountUsed,@ManufacturedType,
				@ManufacturedTypeSub,@AmountUsed,@Currency);	
						
				   											
			ELSE
			   UPDATE dbo.T_Product SET Amount=(@ProductAmount+@AmountUsed),
			   Stock_Amount=(@ProductStock_Amount+@AmountUsed)
			    
			   WHERE GUID=@Product_GUID
	
			--"ʹ��"��T_AID_Product���һ��ʹ������   
			IF @Product_GUID IS NULL 
		      INSERT INTO dbo.T_AID_Product(GUID,AID_Guid,Use_AID_Amount,Product_Guid)
			    VALUES(@NEWGUID,@GUID,@AmountUsed,@NEWGUID);	
				
			 ELSE			
			    INSERT INTO dbo.T_AID_Product(GUID,AID_Guid,Use_AID_Amount,Product_Guid)
			    VALUES(@NEWGUID,@GUID,@AmountUsed,@Product_GUID);
			   
		END
    COMMIT TRAN;
END


GO


