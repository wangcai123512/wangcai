USE [FMS_Develop]
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdateQuickAList]    Script Date: 06/07/2017 16:29:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<胡东杨>
-- Create date: <2017/06/06>
-- Description:	<编辑快速关注>
-- =============================================
ALTER PROCEDURE [dbo].[SP_UpdateQuickAList] 
	-- Add the parameters for the stored procedure here
	@c_guid NVARCHAR(50),
	@attention_type NVARCHAR(50),
	@push_account NVARCHAR(50),
	@push_frequency NVARCHAR(50)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	UPDATE dbo.T_QuickAttention SET attention_state='1',push_account=@push_account
	,push_frequency=@push_frequency
	WHERE c_guid=@c_guid AND attention_type=@attention_type
END


