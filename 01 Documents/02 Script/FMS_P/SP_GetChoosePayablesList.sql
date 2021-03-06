USE [FMS_Develop]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetChoosePayablesList]    Script Date: 2017/7/18 15:09:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[SP_GetChoosePayablesList]
	-- Add the parameters for the stored procedure here
	@PageSize int= -1,
	@PageIndex int = 1,
	@Count int = 0 out,
	@C_GUID nvarchar(40),
	@State nvarchar(40)=null,
	@InvType nvarchar(40)=null,
	@customer nvarchar(40)=null,
	@remark nvarchar(40)=null

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @Type nvarchar(40)
	BEGIN
	CREATE TABLE #IERecordCursor(
    rownumber int,
    IE_GUID nvarchar(40),
    InvType nvarchar(40),
	IEGroup nvarchar(40),
	InvNo nvarchar(20),
    RPer nvarchar(40),
    RPerName nvarchar(40),
    Amount decimal(18,2),  
	DisAmount DECIMAL(18,2),
    Currency nvarchar(20),
    State nvarchar(20),
    Remark nvarchar(200),
    Date datetime,
	AffirmDate datetime,
    AGUID int

    )
   IF(@InvType !='税费'  OR @InvType IS NULL OR LEN(@InvType) = 0)
	BEGIN
    insert into #IERecordCursor
	(
		rownumber,
		IE_GUID ,
		IEGroup ,
		InvType ,
		InvNo ,
		RPer ,
		RPerName ,
		Amount ,  
		DisAmount ,
		Currency ,
		State ,
		Remark ,
		Date ,
		AffirmDate,
		AGUID

		
	)
	
	select row_number()over(order by DC.AffirmDate desc) rownumber,DC.IE_GUID,DC.IEGroup,DC.InvType,DC.InvNo,DC.RPer,BP.Name RPerName,DC.SumAmount,dc.DisAmount,DC.Currency,DC.State,DC.Remark,DC.Date, DC.AffirmDate, case when TA.A_GUID is null then NULL else 1 end
	from dbo.T_IERecord DC
	LEFT JOIN dbo.T_BusinessPartner BP ON BP.BP_GUID = DC.RPer
	LEFT JOIN dbo.T_Attachment TA ON TA.FR_GUID = DC.IE_GUID
	where DC.C_GUID=@C_GUID 
	AND (DC.RPer= @customer OR @customer IS NULL OR Len(@customer) = 0)
	AND isnull(DC.RPer,'') != 'e5e38321-4549-4c24-9f3a-3cd70ee1d591'
	AND (DC.IEGroup NOT IN ('直接物料','间接物料','固定采购','无形资产') OR DC.IEGroup IS NULL)
	AND (DC.State=@State OR @State IS NULL OR LEN(@State) = 0)
	AND (DC.InvType !='税费')
	AND (DC.InvType=@InvType OR @InvType IS NULL OR LEN(@InvType) = 0 or DC.InvType='初始账款')
	AND (DC.Remark=@remark OR @remark IS NULL OR LEN(@remark) = 0)


	insert into #IERecordCursor(
		IE_GUID ,
		IEGroup ,
		InvType ,
		Amount ,  
		DisAmount ,
		State 
	)

	select  DC.IE_GUID, DC.InvType, DC.Amount,DC.DisAmount,DC.State, DC.IEGroup ,CASE WHEN TS.Flag ='TAX' THEN  '增值税' else '企业所得税' end
	from dbo.T_IERecord DC
	LEFT JOIN T_TaxSettlement TS on DC.C_GUID = TS.C_GUID

	END

	IF(@InvType='税费')
	BEGIN
    insert into #IERecordCursor
	(
		rownumber,
		IE_GUID ,
		InvType ,
		IEGroup ,
		InvNo ,
		RPer ,
		RPerName ,
		Amount ,   
		DisAmount ,
		Currency ,
		State ,
		Remark ,
		Date ,
		AffirmDate,
		AGUID
	)
	
	select row_number()over(order by DC.AffirmDate desc) rownumber,DC.IE_GUID,DC.InvType,DC.IEGroup,DC.InvNo,DC.RPer,BP.Name RPerName,DC.SumAmount,DC.DisAmount,DC.Currency,DC.State,DC.Remark,DC.Date,DC.AffirmDate, case when TA.A_GUID is null then NULL else 1 end
	from dbo.T_IERecord DC
	LEFT JOIN dbo.T_BusinessPartner BP ON BP.BP_GUID = DC.RPer
	LEFT JOIN dbo.T_Attachment TA ON TA.FR_GUID = DC.IE_GUID
	where DC.C_GUID=@C_GUID 
	AND (DC.RPer= @customer OR @customer IS NULL OR Len(@customer) = 0)
	AND isnull(DC.RPer,'') != 'e5e38321-4549-4c24-9f3a-3cd70ee1d591'
	AND (DC.IEGroup NOT IN ('直接物料','间接物料','固定采购','无形资产') OR DC.IEGroup IS NULL)
	AND (DC.State=@State OR @State IS NULL OR LEN(@State) = 0)
	AND (DC.InvType='税费')
	AND (DC.Remark=@remark OR @remark IS NULL OR LEN(@remark) = 0)
	END
	
	

	SELECT @Count = COUNT(IE_GUID) FROM #IERecordCursor;
	
	SELECT T.IE_GUID,T.InvType,T.InvNo,T.RPer,T.RPerName,T.Amount,T.DisAmount,T.Currency,T.IEGroup,T.State,T.Remark,T.Date,T.AffirmDate,SUM(T.AGUID) as A_GUID
	    FROM #IERecordCursor T
		WHERE (T.rownumber>= ((@PageIndex - 1)*@PageSize) + 1
		AND T.rownumber <= @PageIndex*@PageSize )
		OR (@PageSize = -1)
		GROUP BY T.IE_GUID,T.InvType,T.InvNo,T.RPer,T.RPerName,T.Amount,T.DisAmount,T.Currency,T.IEGroup,T.State,T.Remark,T.Date,T.AffirmDate
		 DROP TABLE #IERecordCursor
	    
	END
END
