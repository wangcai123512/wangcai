USE [FMS_Develop]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetRevenueCollectionRecordList]    Script Date: 04/11/2017 17:15:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<CHENXIANG>
-- Create date: <2015/09/08>
-- Description:	<查询所有应收账款>
-- =============================================
ALTER PROCEDURE [dbo].[SP_GetRevenueCollectionRecordList]
	-- Add the parameters for the stored procedure here
	@PageSize int = -1,
	@PageIndex int = 1,
	@Count int = 0 out,
	@C_GUID nvarchar(40),
	@BA_GUID NVARCHAR(40) = NULL
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
	AND(DC.RPer = @BA_GUID OR @BA_GUID IS NULL OR LEN(@BA_GUID) = 0)
 
	
	
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
	
	SELECT @Count = COUNT(*) FROM #DeclareCustomerCursor;
	
	SELECT T.GUID,T.InvType,T.RPer,T.RPerName,T.Amount,T.Amount_Used,(T.Amount-T.Amount_Used) AS ResidualAmount,T.Currency,T.State,T.Record,T.VoucherNo,T.Remark, CONVERT(VARCHAR(10),T.Date,111) AS Date,T.AGUID
		FROM #DeclareCustomerCursor T
		WHERE (T.rownumber>= ((@PageIndex - 1)*@PageSize) + 1
		AND T.rownumber <= @PageIndex*@PageSize )
		OR (@PageSize = -1)
		DROP TABLE #DeclareCustomerCursor
		DROP TABLE #Amount_Used
		END
	
END
