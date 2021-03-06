USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_UpdResaleAIDRecord]    Script Date: 02/07/2017 15:39:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<hdy>
-- Create date: <Create Date,16.11.16,>
-- Description:	物料与资产转售费用记录
-- =============================================
ALTER PROCEDURE [dbo].[SP_UpdResaleAIDRecord]
	-- Add the parameters for the stored procedure here
	@GUID NVARCHAR(40),
	@C_GUID NVARCHAR(40),
	@AID_Flag NVARCHAR(1),
	@InvType NVARCHAR(40)=NULL,
	@RPer NVARCHAR(40)=NULL,
	@Amount DECIMAL(18,2),
	@InventoryAmount DECIMAL=NULL,
	@ResaleActualAmount DECIMAL(18,2),
	@State NVARCHAR(40)=NULL,
    @GUIDTW NVARCHAR(40),
    @Creator NVARCHAR(40)=NULL,   
    @Date DATETIME=NULL,
    @AffirmDate DATETIME=NULL,
    @Pnumber NVARCHAR(40)=NULL,
    @Currency NVARCHAR(40)=NULL,
    @OriginalAmount DECIMAL=NULL,
    @TaxationType NVARCHAR(40)=NULL,
    @TaxationAmount DECIMAL(18,2),
    @SumAmount DECIMAL(18,2),
	@Remark NVARCHAR(200)=NULL,	
	@Description NVARCHAR(100)=NULL,	
	@States NVARCHAR(40)=NULL,
	@CostType  VARCHAR(1)=NULL,
	@ResaleNumber INT,
	@Inventory_Number INT,
	@Detailed_Categories NVARCHAR(40)=NULL

AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
			DECLARE @NewGUID VARCHAR(40)	/**生成GUID*/
			DECLARE @Declarestate VARCHAR(40)
			DECLARE @DeclareCostSpendingRPer VARCHAR(40)
			DECLARE @DeclareCostSpendingstate VARCHAR(40)	
			SET  @NewGUID=NEWID();
			DECLARE @CostSum DECIMAL=NULL

			SELECT @CostSum=CostSum,@Declarestate=State,@DeclareCostSpendingRPer=RPer
			FROM dbo.T_DeclareCostSpending 
			WHERE GUID=@GUID
			
			IF  @Declarestate='未付'
				SET @DeclareCostSpendingstate = '应付'
			ELSE 	
				SET @DeclareCostSpendingstate = '已付'	
					
    BEGIN TRAN;
    BEGIN
 
		
    UPDATE dbo.T_AIDRecord SET State=@State,Inventory_Number=@Inventory_Number-@ResaleNumber WHERE GUID=@GUID;
	
	
	--转售到营业成本中--
	INSERT INTO dbo.T_IERecord(IE_GUID,C_GUID,IE_Flag,InvType,RPer,
	[CreateDate],Amount,Remark,Currency,IEDescription,TaxationAmount,
	TaxationType,SumAmount,State,Material_type,GUID_Parent,InvNo,Creator,IEGroup,Resale_Amount,[AffirmDate],[Date])
		
	VALUES(@NewGUID,@C_GUID,'E','营业成本',@DeclareCostSpendingRPer,GETDATE(),'0',
	@Remark,@Currency,@Description,'0',@TaxationType,
	@Amount,@DeclareCostSpendingstate,@CostType,@GUID,@Pnumber,
	@Creator,@Detailed_Categories,'0',@AffirmDate,@Date);	
	
	--转售到营业外收入中--
		INSERT INTO dbo.T_IERecord(IE_GUID,C_GUID,IE_Flag,InvType,RPer,
		[CreateDate],Amount,Remark,Currency,IEDescription,TaxationAmount,
		TaxationType,SumAmount,State,Material_type,GUID_Parent,
		InvNo,Creator,IEGroup,Resale_Amount,[AffirmDate],[Date])
		
		VALUES(@GUIDTW,@C_GUID,@AID_Flag,@InvType,@RPer,GETDATE(),
		@ResaleActualAmount,@Remark,@Currency,@Description,@TaxationAmount,
		@TaxationType,@SumAmount,@States,@CostType,@GUID,
		@Pnumber,@Creator,'',@Amount,@AffirmDate,@Date);
	
	UPDATE dbo.T_DeclareCostSpending SET CostSum = @CostSum+@Amount
	WHERE GUID = @GUID
		
	END
	COMMIT TRAN;
END










GO

