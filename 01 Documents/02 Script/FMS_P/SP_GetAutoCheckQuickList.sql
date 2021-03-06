USE [FMS_Develop]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetAutoCheckQuickList]    Script Date: 06/18/2017 17:15:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<CHENXIANG>
-- Create date: <2015/05/08>
-- Description:	<查询需要发送的快速关注列表>
-- =============================================
ALTER PROCEDURE [dbo].[SP_GetAutoCheckQuickList]
	-- Add the parameters for the stored procedure here
	@PageSize int = -1,
	@PageIndex int = 1,
	@Count int = 0 out,
	@push_isselect nvarchar(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	

    -- Insert statements for procedure here
    DECLARE @temp Table(
    rownumber int,
    c_guid nvarchar(40),
    push_account nvarchar(40),
    push_content nvarchar(500)
    )
	IF @push_isselect = 'isall'
		BEGIN
			INSERT into @temp
			SELECT row_number()over(order by NEWID() desc) rownumber,c_guid,push_account,
			[values]=stuff((select ','+attention_type+' : '+CONVERT(NVARCHAR(40), attention_type_amount)
			+' , '+CONVERT(NVARCHAR(40),statistical_currency)
			FROM dbo.T_QuickAttention AS b WHERE b.c_guid=a.c_guid  for xml path('')), 1, 1, '') 
			FROM dbo.T_QuickAttention a
			WHERE attention_state='1' 
			AND c_guid<>'66666666-6666-6666-6666-666666666666' AND (push_frequency='month' 
						OR  push_frequency='week'  OR push_frequency='day')
			group by c_guid,push_account
		END
	IF @push_isselect = 'isweek'
		BEGIN
			INSERT into @temp
			SELECT row_number()over(order by NEWID() desc) rownumber,c_guid,push_account,
			[values]=stuff((select ','+attention_type+' : '+CONVERT(NVARCHAR(40), attention_type_amount)
			+' , '+CONVERT(NVARCHAR(40),statistical_currency)
			FROM dbo.T_QuickAttention AS b WHERE b.c_guid=a.c_guid  for xml path('')), 1, 1, '') 
			FROM dbo.T_QuickAttention a
			WHERE attention_state='1' 
			AND c_guid<>'66666666-6666-6666-6666-666666666666' AND ( push_frequency='week'  OR push_frequency='day')
			group by c_guid,push_account	
		END
	IF @push_isselect = 'ismonth'
		BEGIN
			INSERT into @temp
			SELECT row_number()over(order by NEWID() desc) rownumber,c_guid,push_account,
			[values]=stuff((select ','+attention_type+' : '+CONVERT(NVARCHAR(40), attention_type_amount)
			+' , '+CONVERT(NVARCHAR(40),statistical_currency)
			FROM dbo.T_QuickAttention AS b WHERE b.c_guid=a.c_guid  for xml path('')), 1, 1, '') 
			FROM dbo.T_QuickAttention a
			WHERE attention_state='1' 
			AND c_guid<>'66666666-6666-6666-6666-666666666666' AND (push_frequency='month' 
						OR   push_frequency='day')
			group by c_guid,push_account
		END
	IF @push_isselect = 'isday'
		BEGIN
			INSERT into @temp
			SELECT row_number()over(order by NEWID() desc) rownumber,c_guid,push_account,
			[values]=stuff((select ','+attention_type+' : '+CONVERT(NVARCHAR(40), attention_type_amount)
			+' , '+CONVERT(NVARCHAR(40),statistical_currency)
			FROM dbo.T_QuickAttention AS b WHERE b.c_guid=a.c_guid  for xml path('')), 1, 1, '') 
			FROM dbo.T_QuickAttention a
			WHERE attention_state='1' 
			AND c_guid<>'66666666-6666-6666-6666-666666666666' AND (push_frequency='day')
			group by c_guid,push_account,a.statistical_currency
		END
	SELECT @Count = COUNT(*) FROM @temp;
	SELECT T.c_guid,T.push_account,T.push_content
		FROM @temp T
		WHERE (T.rownumber>= ((@PageIndex - 1)*@PageSize) + 1
		AND T.rownumber <= @PageIndex*@PageSize )
		OR (@PageSize = -1)	
END
