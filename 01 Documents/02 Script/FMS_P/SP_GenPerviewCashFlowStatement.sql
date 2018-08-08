USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_GenPerviewCashFlowStatement]    Script Date: 12/20/2016 17:13:15 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_GenPerviewCashFlowStatement]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_GenPerviewCashFlowStatement]
GO

USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_GenPerviewCashFlowStatement]    Script Date: 12/20/2016 17:13:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	生成现金流量预览表
-- remark:      还差 补充资料的金额待统计   2016/12/21 liujf
-- =============================================
CREATE PROCEDURE [dbo].[SP_GenPerviewCashFlowStatement]
	@C_ID NVarChar(40),
    @report_date    VARCHAR(10),
	@period_type    VARCHAR(50),        --报表周期类型(月度=month,季度=quarter,年度=year)
    @RepNo	NVARCHAR(40) OUT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE     @Year       INT ;
    DECLARE     @Month      INT ;
	DECLARE     @CurrentRepBeginDate DATE ;
	DECLARE     @CurrentRepEndDate DATE ;
	DECLARE     @quarter INT ;  --季度
	
	DECLARE @report_fix   VARCHAR(10)
	SET @report_fix='CF'
    
    --调用函数 计算日期
    SELECT @CurrentRepBeginDate= begin_date,
           @CurrentRepEndDate= end_date,
           @report_fix=report_fix
    FROM [F_ReportDateCalculate](@report_date,@period_type,@report_fix)
    
     
	--初始化现金流量模板
	UPDATE dbo.T_CashFlowItemTemplate
	SET amount=NULL, amount_add=NULL
	
    --生成报表数据
    DECLARE  @Result TABLE
    (
		RGUID NVARCHAR(40),
		Name NVARCHAR(100),
        EndingValue DECIMAL(18,2),
        AccGrp NVARCHAR(40),
        Code VARCHAR(5),
        RP_Flag NVARCHAR(2),
        AddCode int,
        AddName NVARCHAR(100),
        AddBeginningValue DECIMAL(18,2),
        AddEndingValue DECIMAL(18,2)
    )

    INSERT INTO @Result(RGUID,Name,AccGrp,Code,EndingValue,RP_Flag)
    SELECT CFI.R_GUID ,CFI.Name ,CFI.PID ,CFI.No ,(ISNULL(RP.Amount,0)) ,CFI.RP_Flag
    FROM dbo.T_CashFlowItem CFI
    LEFT JOIN 
    (SELECT CFItemGuid,SUM(SumAmount) AS Amount
		FROM dbo.T_RecPayRecord 
		WHERE C_GUID = @C_ID 
		    AND DATE BETWEEN @CurrentRepBeginDate AND @CurrentRepEndDate		    
		GROUP BY CFItemGuid
		)RP
		ON  RP.CFItemGuid = CFI.R_GUID;
		
	DECLARE  @TmpResult TABLE
    (
        EndingValue DECIMAL(18,2),
        AccGrp NVARCHAR(40),
        RP_Flag NVARCHAR(2)
    )
    
	INSERT INTO @TmpResult(AccGrp,RP_Flag,EndingValue)
	SELECT R.AccGrp,R.RP_Flag,SUM(R.EndingValue)
	FROM @Result R
	GROUP BY R.AccGrp,R.RP_Flag
	
	DECLARE @I_VAL DECIMAL(18,2);
	DECLARE @O_VAL DECIMAL(18,2);
	
	SELECT @I_VAL = TR.EndingValue
	FROM @TmpResult TR
	WHERE TR.AccGrp = '5C552CC3-4008-4AE4-9CBA-423B6AAA486A' AND TR.RP_Flag = 'R';
	
	INSERT INTO @Result(RGUID,Name,AccGrp,Code,EndingValue,RP_Flag)
	SELECT NEWID(),'现金流入小计','5C552CC3-4008-4AE4-9CBA-423B6AAA486A',9,@I_VAL,'RR';
	
	SELECT @O_VAL = TR.EndingValue
	FROM @TmpResult TR
	WHERE TR.AccGrp = '5C552CC3-4008-4AE4-9CBA-423B6AAA486A' AND TR.RP_Flag = 'P';
	
	INSERT INTO @Result(RGUID,Name,AccGrp,Code,EndingValue,RP_Flag)
	SELECT NEWID(),'现金流出小计','5C552CC3-4008-4AE4-9CBA-423B6AAA486A',20,@O_VAL,'PP';

	INSERT INTO @Result(RGUID,Name,AccGrp,Code,EndingValue)
	SELECT NEWID(),'经营活动产生的现金流量净额','5C552CC3-4008-4AE4-9CBA-423B6AAA486A',21,@I_VAL-@O_VAL;

	SELECT @I_VAL = TR.EndingValue
	FROM @TmpResult TR
	WHERE TR.AccGrp = '56E9F013-BB86-4B5E-95D6-A33F9B697AF9' AND TR.RP_Flag = 'R';

	INSERT INTO @Result(RGUID,Name,AccGrp,Code,EndingValue,RP_Flag)
	SELECT NEWID(),'现金流入小计','56E9F013-BB86-4B5E-95D6-A33F9B697AF9',29,@I_VAL,'RR';
	
	SELECT @O_VAL = TR.EndingValue
	FROM @TmpResult TR
	WHERE TR.AccGrp = '56E9F013-BB86-4B5E-95D6-A33F9B697AF9' AND TR.RP_Flag = 'P';
	
	INSERT INTO @Result(RGUID,Name,AccGrp,Code,EndingValue,RP_Flag)
	SELECT NEWID(),'现金流出小计','56E9F013-BB86-4B5E-95D6-A33F9B697AF9',36,@O_VAL,'PP';
	
	INSERT INTO @Result(RGUID,Name,AccGrp,Code,EndingValue)
	SELECT NEWID(),'投资活动产生的现金流量净额','56E9F013-BB86-4B5E-95D6-A33F9B697AF9',37,@I_VAL-@O_VAL;
	
	SELECT @I_VAL = TR.EndingValue
	FROM @TmpResult TR
	WHERE TR.AccGrp = 'DDCEC6F9-EC23-4F5C-BF96-A3D373AAA2C7' AND TR.RP_Flag = 'R';
	
	INSERT INTO @Result(RGUID,Name,AccGrp,Code,EndingValue,RP_Flag)
	SELECT NEWID(),'现金流入小计','DDCEC6F9-EC23-4F5C-BF96-A3D373AAA2C7',44,@I_VAL,'RR';

	SELECT @O_VAL = TR.EndingValue
	FROM @TmpResult TR
	WHERE TR.AccGrp = 'DDCEC6F9-EC23-4F5C-BF96-A3D373AAA2C7' AND TR.RP_Flag = 'P';
	
	INSERT INTO @Result(RGUID,Name,AccGrp,Code,EndingValue,RP_Flag)
	SELECT NEWID(),'现金流出小计','DDCEC6F9-EC23-4F5C-BF96-A3D373AAA2C7',53,@O_VAL,'PP';

	INSERT INTO @Result(RGUID,Name,AccGrp,Code,EndingValue)
	SELECT NEWID(),'筹资活动产生的现金流量净额','DDCEC6F9-EC23-4F5C-BF96-A3D373AAA2C7',54,@I_VAL-@O_VAL;
	
	INSERT INTO @Result(RGUID,Name,AccGrp,Code,EndingValue)
	SELECT NEWID(),'现金及现金等价物净增加额',NULL,56,SUM(EndingValue)
	FROM @Result
	WHERE Code = 21 OR Code = 37 OR Code = 54;
	
 
	
	--SELECT  *
	-- 更新模板的现金流量表的项目和金额
	UPDATE a
	SET a.amount=b.EndingValue
	FROM T_CashFlowItemTemplate AS a
	INNER JOIN @Result AS b ON  a.row_no=b.Code
	
	SELECT  
	        id ,
	        name ,
	        row_no ,	
	        amount,        
	        additional ,	
	        row_no_add,       
	        amount_add 	      
	FROM T_CashFlowItemTemplate 
	ORDER BY id
	
	--还差 补充资料的金额
	
	--SET @Year = DATEPART(YEAR,@CurrentRepBeginDate);
	--SET @Month = DATEPART(MONTH,@CurrentRepBeginDate);	 
	SET @RepNo = @report_fix + @report_date;
 
	 
END



GO


