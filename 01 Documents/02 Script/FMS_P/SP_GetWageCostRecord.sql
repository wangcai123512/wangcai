USE [FMS_Develop]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetWageCostRecord]    Script Date: 05/22/2017 10:14:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[SP_GetWageCostRecord]  
	@PageSize int= -1,
	@PageIndex int = 1,
	@Count int = 0 out,
	@C_GUID nvarchar(40),
	@State nvarchar(40)=null,
	@W_GUID nvarchar(40)=null
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN
	CREATE TABLE #WageCostCursor(
    rownumber int,
    W_GUID nvarchar(40),
	Employee  nvarchar(40),
	InvType  nvarchar(40),
    Total decimal(18,4),  
	Amount_Used DECIMAL(18, 4), 
	Residual_Amount DECIMAL(18, 4),
	Cash DECIMAL(18, 4),
	PersonalTaxes DECIMAL(18, 4),
	SocialSecurity DECIMAL(18, 4),
    Currency nvarchar(20),
    State nvarchar(20),
    Date datetime
    )
	 insert into #WageCostCursor
	(
		rownumber,
		W_GUID ,
		Employee,
		InvType,
		Total ,  
		Amount_Used , 
		Residual_Amount ,
		Cash,
		PersonalTaxes,
		SocialSecurity,
		Currency ,
		State ,
		Date 
	)
	select row_number()over(order by DC.Date desc) rownumber,dc.W_GUID,DC.Employee,IE.InvType,DC.Total,0,0,DC.Cash,DC.PersonalTaxes,DC.SocialSecurity,DC.Currency,DC.State,DC.Date
	from dbo.T_WageCost DC
	LEFT JOIN dbo.T_IERecord IE ON IE.IE_GUID=DC.W_GUID
	where DC.C_GUID=@C_GUID 
	AND (DC.State=@State OR @State IS NULL OR LEN(@State) = 0)
	AND (DC.W_GUID=@W_GUID OR @W_GUID IS NULL OR LEN(@W_GUID)=0)
	 
	 SELECT b.W_GUID , SUM(a.SumAmount)AS Amount_Used 
        INTO #Amount_Used
        FROM dbo.T_RecPayRecord a
        INNER JOIN #WageCostCursor b ON a.IE_GUID = b.W_GUID    
        GROUP BY b.W_GUID

		UPDATE a
        SET a.Amount_Used=b.Amount_Used
        FROM #WageCostCursor AS a
        INNER JOIN #Amount_Used AS b ON a.W_GUID=b.W_GUID
		
		SELECT @Count = COUNT(W_GUID) FROM #WageCostCursor;
	
	    SELECT T.W_GUID,T.Employee,T.InvType,T.Total,T.Currency,T.State,T.Date,T.Cash,T.SocialSecurity,T.PersonalTaxes,
		T.Amount_Used,(T.Total-T.Amount_Used) AS ResidualAmount FROM #WageCostCursor T
		WHERE (T.rownumber>= ((@PageIndex - 1)*@PageSize) + 1
		AND T.rownumber <= @PageIndex*@PageSize )
		OR (@PageSize = -1)
		 DROP TABLE #WageCostCursor
	    DROP TABLE #Amount_Used
	END
	
END

