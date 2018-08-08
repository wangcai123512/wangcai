USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_UpdIncomeStatement]    Script Date: 01/04/2017 15:27:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_UpdIncomeStatement]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_UpdIncomeStatement]
GO

USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_UpdIncomeStatement]    Script Date: 01/04/2017 15:27:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	更新利润表
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdIncomeStatement]
	@C_ID NVarChar(40),
	@report_date    VARCHAR(10),
	@period_type    VARCHAR(50),            --报表周期类型(月度=month,季度=quarter,年度=year)
	@error_msg  VARCHAR(500)    OUTPUT      --执行异常结果信息 
AS
BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON; 
	
    DECLARE	@RepNo      VARCHAR(40),            --报表编号
            @rep_guid   VARCHAR(40)             --报表GUID

    SET @RepNo=''
    SET @error_msg=''

    EXEC	[dbo].SP_GenPerviewIncomeStatement  
            @C_ID = @C_ID,
            @report_date = @report_date,
            @period_type = @period_type,
            @RepNo = @RepNo OUTPUT 

    --如果没有报名号 说明执行利润表 数据统计出错。
    --创建报表失败
    IF	@RepNo=''
    BEGIN
        SET @error_msg='初始化利润表数据异常'
        RETURN
    END
 
    SET @rep_guid=NEWID()
    
     BEGIN TRAN
        -- 创建报告主表
        INSERT INTO dbo.T_Report
        ( Rep_GUID , Type,RepNo ,C_GUID,period_type,rep_date)
        VALUES  
        (@rep_guid ,'PL',@RepNo , @C_ID,@period_type,@report_date)
        
        IF @@ERROR>0
        BEGIN
           
            SET @error_msg= '创建报告主表异常:'+CAST(@@ERROR AS VARCHAR(10))
            ROLLBACK TRAN    
        END
        

        -- 创建报告明细
        --先常用的现金流量项目
        INSERT INTO dbo.T_ReportDetails
        ( GUID ,rep_guid ,row_no ,name ,beginning_amount,ending_amount ) 
        SELECT NEWID(),@rep_guid,row_no,item_name,amount,amount_sum
        FROM dbo.T_IncomeStatementTemplate
        IF @@ERROR>0
        BEGIN
            SET @error_msg= '创建利润表项目明细异常:'+CAST(@@ERROR AS VARCHAR(10))
            ROLLBACK TRAN    
        END 
        
    COMMIT TRAN
  
	
	SET NOCOUNT OFF;
	SET XACT_ABORT OFF;
    
END

GO


