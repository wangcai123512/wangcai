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
-- Description:	�����ֽ�������
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdCashFlowStatement]
	@C_ID       VARCHAR(40),                --��˾ID
	@report_date    VARCHAR(10),
	@period_type    VARCHAR(50),            --������������(�¶�=month,����=quarter,���=year)
	@error_msg  VARCHAR(500)    OUTPUT      --ִ���쳣�����Ϣ 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	 
	SET XACT_ABORT ON;

   
	DECLARE	@RepNo      VARCHAR(40),            --������
		    @rep_guid   VARCHAR(40)             --����GUID
	
	SET @RepNo=''
	SET @error_msg=''
	
    EXEC	[dbo].SP_GenPerviewCashFlowStatement
		    @C_ID = @C_ID,
		    @report_date = @report_date ,
		    @period_type = @period_type ,
		    @RepNo = @RepNo OUTPUT 
    
    --���û�б����� ˵��ִ���ʲ���ծ������ͳ�Ƴ���
    --��������ʧ��
    IF	@RepNo=''
    BEGIN
        SET @error_msg='��ʼ���ֽ�������쳣'
        RETURN
    END	     
    
    SET @rep_guid=NEWID()
    
    BEGIN TRAN
        -- ������������
        INSERT INTO dbo.T_Report
        ( Rep_GUID , Type,RepNo ,C_GUID,period_type,rep_date)
        VALUES  
        (@rep_guid ,'CF',@RepNo , @C_ID,@period_type,@report_date)
         
        IF @@ERROR>0
        BEGIN
           
            SET @error_msg= '�������������쳣:'+CAST(@@ERROR AS VARCHAR(10))
            ROLLBACK TRAN    
        END
        

        -- ����������ϸ
        --�ȳ��õ��ֽ�������Ŀ
        INSERT INTO dbo.T_ReportDetails
        ( GUID ,rep_guid ,row_no ,name ,beginning_amount ) 
        SELECT NEWID(),@rep_guid,row_no,name,amount
        FROM dbo.T_CashFlowItemTemplate
        IF @@ERROR>0
        BEGIN
            SET @error_msg= '�������泣�õ��ֽ�������Ŀ��ϸ�쳣:'+CAST(@@ERROR AS VARCHAR(10))
            ROLLBACK TRAN    
        END

        --��д������ֽ���������
        INSERT INTO dbo.T_ReportDetails
        ( GUID ,rep_guid ,row_no ,name ,beginning_amount   ) 
        SELECT NEWID(),@rep_guid,row_no_add,additional,amount_add
        FROM dbo.T_CashFlowItemTemplate
        IF @@ERROR>0
        BEGIN
            SET @error_msg= '�������油����ֽ�������ϸ�쳣:'+CAST(@@ERROR AS VARCHAR(10))
            ROLLBACK TRAN    
        END
        
    COMMIT TRAN
  
	
	SET NOCOUNT OFF;
	SET XACT_ABORT OFF;
	
END

GO


