USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_UpdRateHistory]    Script Date: 03/29/2017 13:43:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<chenxiang>
-- Create date: <Create Date,,>
-- Description:	更新收入费用记录
-- =============================================
ALTER PROCEDURE [dbo].[SP_UpdRateHistory]
	@GUID NVARCHAR(40),
	@Date DATETIME,
	@FAmount DECIMAL(18,2),
	@FCurrency NVARCHAR(40),
	@TAmount DECIMAL(18,2),
	@TCurrency NVARCHAR(40),
	@C_GUID NVARCHAR(40),
	@Currency NVARCHAR(40)=NULL
	
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

    BEGIN TRAN;
	-- Insert statements for procedure here
	INSERT INTO dbo.T_RateHistory(GUID,Date,FAmount,FCurrency,TAmount,TCurrency,C_GUID,CurrentRecord,Currency,Rate)
	VALUES(@GUID,@Date,@FAmount,@FCurrency,@TAmount,@TCurrency,@C_GUID,'1',@Currency,Convert(numeric(18,4),@FAmount/@TAmount));

	UPDATE dbo.T_RateHistory SET CurrentRecord = '0' WHERE GUID != @GUID AND C_GUID = @C_GUID AND FCurrency = @FCurrency AND TCurrency = @TCurrency
	COMMIT TRAN;
END

GO


