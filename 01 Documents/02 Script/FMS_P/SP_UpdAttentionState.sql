USE [FMS_Develop]
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdAttentionState]    Script Date: 06/07/2017 16:29:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<胡东杨>
-- Create date: <2017/06/06>
-- Description:	<更改快速关注状态为0>
-- =============================================
ALTER PROCEDURE [dbo].[SP_UpdAttentionState] 
	-- Add the parameters for the stored procedure here
	@c_guid NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	UPDATE dbo.T_QuickAttention SET attention_state='0' WHERE c_guid=@c_guid
END


