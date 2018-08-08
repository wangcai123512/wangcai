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
-- Description:	物料与资产使用费用记录
-- =============================================
ALTER PROCEDURE [dbo].[SP_UpdUseResaleAIDRecord]
	@GUID VARCHAR(40),/**GUID**/
	@AmountUsed DECIMAL(18,4),/**使用金额**/
	@InventoryAmount DECIMAL(18,4),/**库存金额**/
	@State VARCHAR(40)=NULL,/**状态**/
    @C_GUID NVARCHAR(40),/**C_GUID**/
	@ManufacturedType NVARCHAR(40)=NULL,/**制造产品类别下**/
	@ManufacturedTypeSub NVARCHAR(40)=NULL,/**制造产品子类别下*/
	@Currency NVARCHAR(20)=NULL	/**货币*/	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	
	
			DECLARE @today  VARCHAR(10)				
			DECLARE @NEWGUID VARCHAR(40) 	/**生成GUID*/
			DECLARE @ProductAmount DECIMAL 	 /**product表中amount*/	
			DECLARE @ProductStock_Amount DECIMAL 	 /**product表中Stock_Amount*/	
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
		   -- 首先更新AID表中状态
		   
			UPDATE dbo.T_AIDRecord 
			SET State=@State  
			WHERE GUID=@GUID	
			
			    
			--同时需要向T_Product添加一条数据,需要判断T_Product有没有该类别和子类别,
			--当前添加的时间在表中有没有同一天的
			--满足条件则在当前已有记录金额加上使用金额
			--否则生成一条新的使用记录	
			
			IF @Product_GUID IS NULL
				INSERT INTO dbo.T_Product(GUID,C_GUID,Create_Date,Amount,TypeId,SubTypeId,
				Stock_Amount,Currency)
				VALUES(@NEWGUID,@C_GUID,@today,@AmountUsed,@ManufacturedType,
				@ManufacturedTypeSub,@AmountUsed,@Currency);	
						
				   											
			ELSE
			   UPDATE dbo.T_Product SET Amount=(@ProductAmount+@AmountUsed),
			   Stock_Amount=(@ProductStock_Amount+@AmountUsed)
			    
			   WHERE GUID=@Product_GUID
	
			--"使用"向T_AID_Product添加一条使用数据   
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


