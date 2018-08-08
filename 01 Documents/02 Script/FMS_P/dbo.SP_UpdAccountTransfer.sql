	-- =============================================
	-- Author:		<Author,hdy,>
	-- Create date: <Create Date,04/13/2017,>
	-- Description:	提交内部转账信息
	-- =============================================
	CREATE PROCEDURE [dbo].[SP_UpdAccountTransfer]
		@OutBankAccount NVARCHAR(50),
		@InBankAccount NVARCHAR(50),
		@OutAmout DECIMAL,
		@OutDate NVARCHAR(50),
		@C_GUID NVARCHAR(50)
	AS
	BEGIN
		-- SET NOCOUNT ON added to prevent extra result sets from
		-- interfering with SELECT statements.
		SET NOCOUNT ON;
			 DECLARE @Rate FLOAT 	/**汇率*/	
			 DECLARE @AccountCurrencyF NVARCHAR(50)  /**FRom货币*/
			 DECLARE @AccountCurrencyT NVARCHAR(50)   /**To货币*/
			 DECLARE @NEWGUID VARCHAR(40) /**生成GUID*/
			 DECLARE @NEWGUIDTwice VARCHAR(40) /**生成GUID*/
			 SET @NEWGUID=NEWID();
			 SET @NEWGUIDTwice=NEWID();
			 /**FRom货币*/
			 SELECT @AccountCurrencyF=AccountCurrency
			 FROM dbo.T_BankAccount
			 WHERE  BA_GUID=@OutBankAccount
			 AND C_GUID=@C_GUID
			 
			 /**To货币*/
			 SELECT @AccountCurrencyT=AccountCurrency
			 FROM dbo.T_BankAccount
			 WHERE  BA_GUID=@InBankAccount
			 AND C_GUID=@C_GUID
			 
			 /**汇率*/
			 SELECT @Rate=Rate
			 FROM  dbo.T_RateHistory
			 WHERE C_GUID=@C_GUID
			 AND    FCurrency=@AccountCurrencyF
			 AND    TCurrency=@AccountCurrencyT
			 AND    CurrentRecord='1'
			
		BEGIN
				/**这里需要修改转进和转出账号的货币还要添加两条记录到RecPayRecord*/
				
				
				UPDATE dbo.T_BankAccount
				SET Amount=Amount-@OutAmout
				WHERE BA_GUID=@OutBankAccount; 			
				BEGIN
					IF @AccountCurrencyF=@AccountCurrencyT
						UPDATE dbo.T_BankAccount 
						SET Amount=(Amount+(@OutAmout*1))
						WHERE BA_GUID=@InBankAccount;
					ELSE
						UPDATE dbo.T_BankAccount 
						SET Amount=(Amount+(@OutAmout*@Rate))
						WHERE BA_GUID=@InBankAccount;
				END
				
				IF @@ERROR>0
				BEGIN
					ROLLBACK;
				END
				
				/**添加一条付款*/
				INSERT INTO dbo.T_RecPayRecord(RP_GUID,RP_Flag,InvType,InvTypeDts,Record,InvNo,RPer,SumAmount,Date,Remark,Creator,CreateDate,
				C_GUID,RPable,Currency,B_GUID,BA_GUID,IE_GUID)
				VALUES(@NEWGUID,'P','内部转账',NULL,NULL,NULL,NULL,@OutAmout,@OutDate,NULL,NULL,NULL
				,@C_GUID,NULL,@AccountCurrencyF,
				NULL,@OutBankAccount,NULL);
				
				IF @@ERROR>0
				BEGIN
					ROLLBACK;
				END
				
				/**添加一条收款看有无汇率*/
				BEGIN
					IF @AccountCurrencyF=@AccountCurrencyT
						INSERT INTO dbo.T_RecPayRecord(RP_GUID,RP_Flag,InvType,InvTypeDts,Record,InvNo,RPer,SumAmount,Date,Remark,Creator,CreateDate,
						C_GUID,RPable,Currency,B_GUID,BA_GUID,IE_GUID)
						VALUES(@NEWGUIDTwice,'R','内部转账',NULL,NULL,NULL,NULL,@OutAmout,@OutDate,NULL,NULL,NULL
						,@C_GUID,NULL,@AccountCurrencyF,
						NULL,@InBankAccount,NULL);
					ELSE
						INSERT INTO dbo.T_RecPayRecord(RP_GUID,RP_Flag,InvType,InvTypeDts,Record,InvNo,RPer,SumAmount,Date,Remark,Creator,CreateDate,
						C_GUID,RPable,Currency,B_GUID,BA_GUID,IE_GUID)
						VALUES(@NEWGUIDTwice,'R','内部转账',NULL,NULL,NULL,NULL,@OutAmout,@OutDate,NULL,NULL,NULL
						,@C_GUID,NULL,@AccountCurrencyF,
						NULL,(@InBankAccount*@Rate),NULL);
				END							 
		END	 
				 
	END