USE [FMS_Develop]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetReceivablesDeclareCustomer]    Script Date: 01/03/2017 18:24:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<CHENXIANG>
-- Create date: <2015/09/08>
-- Description:	<查询所有应收账款>
-- =============================================
ALTER PROCEDURE [dbo].[SP_GetReceivablesDeclareCustomer]
	-- Add the parameters for the stored procedure here
	@PageSize int = -1,
	@PageIndex int = 1,
	@Count int = 0 out,
	@C_GUID nvarchar(40),
	@dateBegin DATETIME = NULL,
	@dateEnd DATETIME = NULL,
	@customer NVARCHAR(40) = NULL,
	@state NVARCHAR(40) = NULL,
	@currency NVARCHAR(40) = NULL,
	@incomeGrp NVARCHAR(20)=NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
  
   BEGIN
    CREATE TABLE #DeclareCustomerCursor(
    rownumber int,
    GUID nvarchar(40),
    InvType nvarchar(40),
    RPer nvarchar(40),
    RPerName nvarchar(40),
    Amount decimal(18,4),
	Amount_Used DECIMAL(18, 4),
	Residual_Amount DECIMAL(18, 4),
    Currency nvarchar(20),
    State nvarchar(20),
	Record nvarchar(20),
    Remark nvarchar(200),
    Date datetime,
    AGUID nvarchar(40),
	VoucherNo nvarchar(40),
	C_GUID nvarchar(40)
	
    )
        
    insert into #DeclareCustomerCursor
	select row_number()over(order by DC.Date desc) rownumber,DC.GUID,DC.InvType,DC.RPer,BP.Name RPerName,DC.Amount,0,0,DC.Currency,DC.State,DC.Record,DC.Remark,DC.Date,TA.A_GUID AGUID,DC.VoucherNo,DC.C_GUID
	from dbo.T_DeclareCustomer DC
	LEFT JOIN dbo.T_BusinessPartner BP ON BP.BP_GUID = DC.RPer
	LEFT JOIN dbo.T_Attachment TA ON TA.FR_GUID = DC.GUID
	where DC.C_GUID=@C_GUID 
	AND(DC.Date >= @dateBegin OR @dateBegin IS NULL)
	AND(DC.Date < DATEADD(day,1,@dateEnd) OR @dateEnd IS NULL)
	AND(DC.RPer = @customer OR @customer IS NULL OR LEN(@customer) = 0)
	AND (DC.State=@state OR @state IS NULL OR LEN(@state) = 0)
	AND (DC.Currency=@currency OR @currency IS NULL OR LEN(@currency) = 0)
	AND (DC.InvType = @incomeGrp OR @incomeGrp IS NULL OR LEN(@incomeGrp)=0) 
	
	
	 SELECT b.GUID , SUM(a.SumAmount)AS Amount_Used 
        INTO #Amount_Used
        FROM dbo.T_RecPayRecord a
        INNER JOIN #DeclareCustomerCursor b ON a.IE_GUID= b.GUID    
        GROUP BY b.GUID
              
        --更新临时表中已销账
        UPDATE a
        SET a.Amount_Used=b.Amount_Used
        FROM #DeclareCustomerCursor AS a
        INNER JOIN #Amount_Used AS b ON a.GUID=b.GUID
	
	SELECT @Count = COUNT(*) FROM @temp;
	
	SELECT T.GUID,T.InvType,T.RPer,T.RPerName,T.Amount,T.Amount_Used,(T.Amount-T.Amount_Used) AS ResidualAmount,T.Currency,T.State,T.Record,T.VoucherNo,T.Remark, CONVERT(VARCHAR(10),T.Date,111) AS Date,T.AGUID
		FROM #DeclareCustomerCursor T
		WHERE (T.rownumber>= ((@PageIndex - 1)*@PageSize) + 1
		AND T.rownumber <= @PageIndex*@PageSize )
		OR (@PageSize = -1)
		DROP TABLE #DeclareCustomerCursor
		DROP TABLE #Amount_Used
		END
	
END
