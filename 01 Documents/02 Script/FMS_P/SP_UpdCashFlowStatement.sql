USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_UpdCashFlowStatement]    Script Date: 01/03/2017 16:44:55 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_UpdCashFlowStatement]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_UpdCashFlowStatement]
GO

USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_UpdCashFlowStatement]    Script Date: 01/03/2017 16:44:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	更新现金流量表
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdCashFlowStatement]
	@C_ID       VARCHAR(40),                --公司ID
	@report_date    VARCHAR(10),
	@period_type    VARCHAR(50),            --报表周期类型(月度=month,季度=quarter,年度=year)
	@error_msg  VARCHAR(500)    OUTPUT      --执行异常结果信息 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	 
	SET XACT_ABORT ON;

   
	DECLARE	@RepNo      VARCHAR(40),            --报表编号
		    @rep_guid   VARCHAR(40)             --报表GUID
	
	SET @RepNo=''
	SET @error_msg=''
	
    EXEC	[dbo].SP_GenPerviewCashFlowStatement
		    @C_ID = @C_ID,
		    @report_date = @report_date ,
		    @period_type = @period_type ,
		    @RepNo = @RepNo OUTPUT 
    
    --如果没有报名号 说明执行资产负债表数据统计出错。
    --创建报表失败
    IF	@RepNo=''
    BEGIN
        SET @error_msg='初始化现金表数据异常'
        RETURN
    END	     
    
    SET @rep_guid=NEWID()
    
    BEGIN TRAN
        -- 创建报告主表
        INSERT INTO dbo.T_Report
        ( Rep_GUID , Type,RepNo ,C_GUID,period_type,rep_date)
        VALUES  
        (@rep_guid ,'CF',@RepNo , @C_ID,@period_type,@report_date)
         
        IF @@ERROR>0
        BEGIN
           
            SET @error_msg= '创建报告主表异常:'+CAST(@@ERROR AS VARCHAR(10))
            ROLLBACK TRAN    
        END
        

        -- 创建报告明细
        --先常用的现金流量项目
        INSERT INTO dbo.T_ReportDetails
        ( GUID ,rep_guid ,row_no ,name ,beginning_amount ) 
        SELECT NEWID(),@rep_guid,row_no,name,amount
        FROM dbo.T_CashFlowItemTemplate
        IF @@ERROR>0
        BEGIN
            SET @error_msg= '创建报告常用的现金流量项目明细异常:'+CAST(@@ERROR AS VARCHAR(10))
            ROLLBACK TRAN    
        END

        --再写补充的现金流量数据
        INSERT INTO dbo.T_ReportDetails
        ( GUID ,rep_guid ,row_no ,name ,beginning_amount   ) 
        SELECT NEWID(),@rep_guid,row_no_add,additional,amount_add
        FROM dbo.T_CashFlowItemTemplate
        IF @@ERROR>0
        BEGIN
            SET @error_msg= '创建报告补充的现金流量明细异常:'+CAST(@@ERROR AS VARCHAR(10))
            ROLLBACK TRAN    
        END
        
    COMMIT TRAN
  
	
	SET NOCOUNT OFF;
	SET XACT_ABORT OFF;
	
END

GO


