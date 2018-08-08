USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_GetAIDRecord]    Script Date: 02/08/2017 15:08:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,chenxiang>
-- Create date: <Create Date,,07/16/2015>
-- Description:	获取采购记录
-- =============================================
ALTER PROCEDURE [dbo].[SP_GetAIDRecord]
	@PageSize INT = -1,
	@PageIndex INT = 1,
	@Count INT = 0 OUT,
	@Flag NVARCHAR(1) = NULL,
	@C_GUID NVARCHAR(50),
	@DateBegin DATETIME = NULL,
	@DateEnd DATETIME = NULL,
	@Customer NVARCHAR(40) = NULL,
	@Type NVARCHAR(40)=NULL,
	@TypeSub NVARCHAR(40)=NULL,
	@State NVARCHAR(40)=NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;	 
    BEGIN	
     CREATE TABLE #AIDRecordCursor
	 (
	    rownumber INT,
		ID NVARCHAR(40),
		Amount DECIMAL(18, 4),
		Amount_Used DECIMAL(18, 4),         --已使用
		Resale_Value DECIMAL(18, 4),        --已转售
		Residual_Amount DECIMAL(18, 4),     --剩余金额
		Currency NVARCHAR(40),
		RPer NVARCHAR(40),
		AidTypeName NVARCHAR(40),
		ASTTypeName NVARCHAR(40),
		State  NVARCHAR(40),
		Remark NCHAR(40),
		date VARCHAR(20),
		RPerName VARCHAR(100),                --供应商名称
		MaterialNumber VARCHAR(100),                --进货数量
		Inventory_Number VARCHAR(100)                --实时库存数量
	 )

        INSERT INTO #AIDRecordCursor
        (rownumber,
         ID,
          Amount,
          Amount_Used,
          Resale_Value,
          Currency,
          RPer,
          AidTypeName,
          ASTTypeName,
          State,
          Remark,
          date,
          RPerName,
          MaterialNumber,
          Inventory_Number
        )
        SELECT  ROW_NUMBER()OVER( ORDER BY a.GUID), a.GUID ,  a.Amount, 0,0,
        a.Currency,a.RPer,c.AidTypeName,d. ASTTypeName,a.State,a.Remark,
        CONVERT(VARCHAR(10),date,111) AS date,b.Name,a.Number,a.Inventory_Number  
		FROM dbo.T_AIDRecord a
		INNER JOIN dbo.T_BusinessPartner b ON a.RPer = b.BP_GUID	
		INNER JOIN dbo.T_AIDTypeRecord c ON a.InvType = c.AT_GUID
		INNER JOIN dbo.T_AIDSubTypeRecord d ON a.SubType = d.AST_GUID			 
		WHERE a.C_GUID=@C_GUID	
		
	
		
		AND(c.AID_Flag = @Flag OR @Flag IS NULL OR LEN(@Flag) = 0)
		AND(a.Date >= @DateBegin OR @DateBegin IS NULL)
		AND(a.Date < DATEADD(DAY,1,@DateEnd) OR @DateEnd IS NULL)
		AND(a.RPer = @Customer OR @Customer IS NULL OR LEN(@Customer) = 0)
		AND(a.InvType = @Type OR @Type IS NULL OR LEN(@Type)=0)
		AND(a.SubType = @TypeSub OR @TypeSub IS NULL OR LEN(@TypeSub)=0)
		AND(a.State = @State OR @State IS NULL OR LEN(@State)=0)
        AND(a.GUID_Parent IS NULL)
              
        --2.//主数据下面的子数据进行汇总
        -- 按照AID_GUID汇总已使用数据       
        
        SELECT b.ID , SUM(a.Use_AID_Amount)AS Amount_Used 
        INTO #Amount_Used
        FROM dbo.T_AID_Product a
        INNER JOIN #AIDRecordCursor b ON a.AID_Guid = b.ID    
        GROUP BY b.ID
              
        --更新临时表中已使用
        UPDATE a
        SET a.Amount_Used=b.Amount_Used
        FROM #AIDRecordCursor AS a
        INNER JOIN #Amount_Used AS b ON a.ID=b.ID
        
        --汇总 已转售
        SELECT b.ID , SUM(a.Resale_Amount)AS Resale_Value 
        INTO #Resale_Value
        FROM dbo.T_IERecord a
        INNER JOIN #AIDRecordCursor b 
        ON a.GUID_Parent = b.ID AND a.IE_Flag='I'   
        GROUP BY b.ID
        
        --更新临时表中已转售
        UPDATE a
        SET a.Resale_Value=b.Resale_Value
        FROM #AIDRecordCursor AS a
        INNER JOIN #Resale_Value AS b ON a.ID=b.ID
         
        --总行数 
		SELECT @Count = COUNT(ID) FROM #AIDRecordCursor;		
		SELECT  		      
		        RPerName ,
		        Currency ,
		        ID AS GUID,
		        Amount ,
		        AidTypeName,
                ASTTypeName,
		        Remark ,
		        Amount_Used AS AmountUsed,
		        Resale_Value AS ResaleValue,
		       (Amount-Amount_Used-Resale_Value) AS ResidualAmount,
		        RPer ,
		        Date,
		        State,
		        Inventory_Number,
		        MaterialNumber
		  FROM #AIDRecordCursor	
		WHERE ( rownumber>= ((@PageIndex - 1)*@PageSize) + 1
		AND  rownumber <= @PageIndex*@PageSize )
		OR (@PageSize = -1)	 		
	    DROP TABLE #AIDRecordCursor
	    DROP TABLE #Amount_Used
	    DROP TABLE #Resale_Value
	END
	 
END



GO


