USE [FMS_Develop]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetQuickAttentionModel]    Script Date: 06/01/2017 16:39:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,hdy>
-- Create date: <Create Date,17/6/1,>
-- Description:	<Description,查询快速关注模板,>
-- =============================================
ALTER PROCEDURE [dbo].[SP_GetQuickAttentionModel]
	@c_guid nvarchar(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT  *	from dbo.T_QuickAttention T where c_guid=@c_guid
END
