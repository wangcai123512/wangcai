USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_GetReports]    Script Date: 12/23/2016 13:59:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_GetReports]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_GetReports]
GO

USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_GetReports]    Script Date: 12/23/2016 13:59:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	获取报表列表
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetReports]
	@C_ID NVARCHAR(40),
	@Type NVARCHAR(5)= NULL,
	@PageSize INT = 10,
	@PageIndex INT = 0,
	@Count INT = 0 OUT 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @temp TABLE(
		rownumber int,
		Rep_GUID NVARCHAR(40),
		RepNo NVARCHAR(40),
		Year INT,
		Month INT
	)
	
	INSERT INTO @temp
		SELECT row_number()over(order by rep.RepNo DESC) rownumber,
		rep.Rep_GUID,rep.RepNo,rep.Year,rep.Month
		FROM dbo.T_Report rep
		WHERE rep.C_GUID = @C_ID AND (rep.Type = @Type OR @Type IS NULL);
	
	SELECT @Count = COUNT(t.Rep_GUID)
	FROM @temp t
	
    SELECT Rep_GUID,t.RepNo,t.Year,t.Month
    FROM @temp t
    WHERE (T.rownumber>= ((@PageIndex - 1)*@PageSize) + 1
	AND T.rownumber <= @PageIndex*@PageSize)
	OR (@PageIndex = 0);
	ORDER BY t.YEAR DESC,t.MONTH DESC
END

GO


