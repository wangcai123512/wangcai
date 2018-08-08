USE [FMS_Develop]
GO

/****** Object:  UserDefinedFunction [dbo].[F_ReportDateCalculate]    Script Date: 02/21/2017 11:16:19 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[F_ReportDateCalculate]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[F_ReportDateCalculate]
GO

USE [FMS_Develop]
GO

/****** Object:  UserDefinedFunction [dbo].[F_ReportDateCalculate]    Script Date: 02/21/2017 11:16:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION [dbo].[F_ReportDateCalculate](
    @report_date    VARCHAR(10),
	@period_type    VARCHAR(50),        --报表周期类型(月度=month,季度=quarter,年度=year)
	@report_fix    VARCHAR(5)
	)    
RETURNS  @tmp TABLE(        
    begin_date      DATE,      
    end_date        DATE, 
    report_fix      VARCHAR(10)
)    
AS   
BEGIN    
    DECLARE     @CurrentRepBeginDate DATE 
	DECLARE     @CurrentRepEndDate DATE 
	DECLARE     @quarter INT   --季度 
	DECLARE     @Year       INT ;
	
	IF @period_type='year'
	BEGIN
	    SET @CurrentRepBeginDate= CAST(@report_date +'/01/01' AS DATE); 
	    SET @CurrentRepEndDate = DATEADD( DAY,-1, DATEADD(YEAR ,1,@CurrentRepBeginDate));  
	    SET @report_fix=@report_fix+'Y'
	    
	END
	ELSE IF @period_type='month'
	BEGIN
	    SET @CurrentRepBeginDate= CAST(@report_date +'/01' AS DATE); 
	    SET @CurrentRepEndDate = DATEADD( DAY,-1, DATEADD(MONTH ,1,@CurrentRepBeginDate));   
	    SET @report_fix=@report_fix+'M'
	    
	END
	ELSE IF @period_type='quarter'
	BEGIN
	    SET @CurrentRepBeginDate= CAST(@report_date +'/01' AS DATE); 
	    
	    SET @quarter = DATEPART(MONTH ,@CurrentRepBeginDate)
	    SET @Year=DATEPART(YEAR ,@CurrentRepBeginDate)
	     
	    SET @CurrentRepBeginDate= CAST( (CAST(@Year AS VARCHAR(4))  +'/01/01') AS DATE); 
	      
	    SET @CurrentRepBeginDate = DATEADD( MONTH ,(@quarter-1)*3+1, @CurrentRepBeginDate);   
	
	    --季度的结束日期计算
	    SET @CurrentRepEndDate = DATEADD( MONTH ,@quarter*3+1, @CurrentRepBeginDate);   
	    
	    SET @CurrentRepEndDate = DATEADD( DAY,-1,  @CurrentRepEndDate);  
	    SET @report_fix=@report_fix+'Q'
	     
	END 
	INSERT INTO @tmp
	        ( begin_date ,
	          end_date ,
	          report_fix
	        ) 
	VALUES( @CurrentRepBeginDate  ,@CurrentRepEndDate ,@report_fix )
    RETURN     
END 

GO


