USE [FMS_Develop]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetCustomerCashInFlowsRecordList]    Script Date: 04/11/2017 11:12:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,hdy>
-- Create date: <Create Date,17.4.11,>
-- Description:	<Description,客户流入现金,>
-- =============================================
ALTER PROCEDURE [dbo].[SP_GetCustomerCashInFlowsRecordList]
	-- Add the parameters for the stored procedure here
	@PageSize int = -1,
    @PageIndex int = 1,
	@Count int = 0 out,
	@Flag NVARCHAR(1) = NULL,
	@C_GUID NVARCHAR(50),
	@dateBegin DATETIME = NULL,
	@dateEnd DATETIME = NULL,
	@BA_GUID NVARCHAR(40) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	 DECLARE @temp TABLE(
		rownumber int,
		RP_GUID NVARCHAR(50),
		Record NVARCHAR(40),
		InvType nvarchar(20),
		InvNo nvarchar(20),
		RPer nvarchar(40),
		SumAmount  decimal(18, 4),
		RPerName nvarchar(100),
		Remark NVARCHAR(500),
		Creator  nvarchar(40),
		CreateDate datetime,
		Date date,
		CFItemGuid  nvarchar(40),
		Currency NVARCHAR(20),
		B_GUID  nvarchar(40),
		BA_GUID  nvarchar(40),
		AccountAbbreviation nvarchar(40),
		InvTypeDts NVARCHAR(500),
		A_GUID int,
		IE_GUID nvarchar(40)
	 )
	 insert into @temp
	 select row_number()over(order by RC.Date desc) rownumber,RC.RP_GUID,Record,RC.InvType,RC.InvNo,RC.RPer,RC.SumAmount,BP.Name RPerName,RC.Remark,
	 RC.Creator,CONVERT(VARCHAR(10),RC.CreateDate,111), CONVERT(VARCHAR(10),RC.Date,111),RC.CFItemGuid,RC.Currency,BA.B_GUID,BA.BA_GUID,BA.AccountAbbreviation,RC.InvTypeDts,case when TA.A_GUID is null then NULL else 1 end,RC.IE_GUID
	 from dbo.T_RecPayRecord RC
	 LEFT JOIN dbo.T_BusinessPartner BP ON BP.BP_GUID = RC.RPer
	 LEFT JOIN dbo.T_Attachment TA ON TA.FR_GUID = RC.RP_GUID
	 LEFT JOIN dbo.T_BankAccount BA ON BA.BA_GUID = RC.BA_GUID
	 where RC.C_GUID=@C_GUID
	 AND(RC.Date >= @dateBegin OR @dateBegin IS NULL OR LEN(@dateBegin) = 0)
	 AND(RC.Date < DATEADD(day,1,@dateEnd) OR @dateEnd IS NULL OR LEN(@dateEnd) = 0)
	 AND(RC.RP_Flag = @Flag OR @Flag IS NULL OR LEN(@Flag) = 0) 
	 AND(RC.RPer = @BA_GUID OR @BA_GUID IS NULL OR LEN(@BA_GUID) = 0)
	 AND(BP.IsCustomer = 1 )
    -- Insert statements for procedure here
	SELECT @Count = COUNT(*) FROM @temp; 
	select T.RP_GUID,T.SumAmount,T.B_GUID,T.BA_GUID,T.AccountAbbreviation,T.Record,T.InvType,T.RPerName,T.RPer,T.Remark,T.Currency,T.CFItemGuid,CONVERT(VARCHAR(10),T.CreateDate,111) AS CreateDate, CONVERT(VARCHAR(10),T.Date,111) AS Date ,T.InvTypeDts,T.IE_GUID,SUM(T.A_GUID) as A_GUID
	FROM @temp T
	WHERE (T.rownumber>= ((@PageIndex - 1)*@PageSize) + 1
		AND T.rownumber <= @PageIndex*@PageSize )
		OR (@PageSize = -1)
	    group by T.RP_GUID,T.SumAmount,T.B_GUID,T.BA_GUID,t.AccountAbbreviation,T.Record,T.InvType,T.RPerName,T.RPer,T.Remark,T.Currency,T.CFItemGuid,T.CreateDate,T.InvTypeDts,T.IE_GUID,T.Date
		order by T.CreateDate desc
END

