USE [FMS_Develop]
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdQuickAttention]    Script Date: 06/02/2017 09:32:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<chenxiang>
-- Create date: <Create Date,,>
-- Description:	更新所属公司快速关注列表
-- =============================================
ALTER PROCEDURE [dbo].[SP_UpdQuickAttention]
	@id NVARCHAR(40),
	@c_guid NVARCHAR(40),
	@attention_type NVARCHAR(40),
	@statistical_time DATETIME =NULL,
	@attention_type_amount DECIMAL(18,2),
	@statistical_currency NVARCHAR(40)=NULL,
	@attention_state NVARCHAR(40),
	@push_account NVARCHAR(40)=NULL,
	@push_frequency NVARCHAR(40)=NULL
	
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

    BEGIN TRAN;
	-- Insert statements for procedure here
	
	INSERT INTO dbo.T_QuickAttention
	        ( id ,
	          c_guid ,
	          attention_type ,
	          attention_type_amount ,
	          statistical_time ,
	          statistical_currency,
	          attention_state,
	          push_account,
	          push_frequency      
	        )
	VALUES(@id,@c_guid,@attention_type,@attention_type_amount,@statistical_time,
	@statistical_currency,@attention_state,@push_account,@push_frequency);
	
	COMMIT TRAN;
END

