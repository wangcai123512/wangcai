USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_UpdBalanceSheet]    Script Date: 12/20/2016 17:35:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_UpdBalanceSheet]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_UpdBalanceSheet]
GO

USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_UpdBalanceSheet]    Script Date: 12/20/2016 17:35:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	�����ʲ���ծ��
-- =============================================
CREATE PROCEDURE [dbo].[SP_UpdBalanceSheet]
	@C_ID       VARCHAR(40),                --��˾ID
	@report_date    VARCHAR(10),
	@period_type    VARCHAR(50),            --������������(�¶�=month,����=quarter,���=year)
	@error_msg  VARCHAR(500)    OUTPUT      --ִ���쳣�����Ϣ 
AS
BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	
    DECLARE	@RepNo      VARCHAR(40),            --������
		    @rep_guid   VARCHAR(40)             --����GUID
	
	SET @RepNo=''
	SET @error_msg='' 
		   
    EXEC	[dbo].[SP_GenPerviewBalanceSheet]
		    @C_ID = @C_ID,
		    @report_date = @report_date ,
		    @period_type = @period_type ,
		    @RepNo = @RepNo OUTPUT 
    
    --���û�б����� ˵��ִ���ʲ���ծ������ͳ�Ƴ���
    --��������ʧ��
    IF	@RepNo=''
    BEGIN
        SET @error_msg='��ʼ���ʲ���ծ�������쳣'
        RETURN
    END	    
 
    SET @rep_guid=NEWID()
    
    BEGIN TRAN
        -- ������������
        INSERT INTO dbo.T_Report
        ( Rep_GUID , Type,RepNo ,C_GUID,period_type,rep_date)
        VALUES  
        (@rep_guid ,'BS',@RepNo , @C_ID,@period_type,@report_date)
        
        IF @@ERROR>0
        BEGIN
           
            SET @error_msg= '�������������쳣:'+CAST(@@ERROR AS VARCHAR(10))
            ROLLBACK TRAN    
        END
        

        -- ����������ϸ
        --��д�ʲ�
        INSERT INTO dbo.T_ReportDetails
        ( GUID ,rep_guid ,row_no ,name ,beginning_amount ,ending_amount ) 
        SELECT NEWID(),@rep_guid,asset_row_no,asset_item_name,asset_start_amount,asset_end_amount
        FROM dbo.T_BalanceSheetTemplate
        IF @@ERROR>0
        BEGIN
            SET @error_msg= '���������ʲ���ϸ�쳣:'+CAST(@@ERROR AS VARCHAR(10))
            ROLLBACK TRAN    
        END

        --��д��ծ
        INSERT INTO dbo.T_ReportDetails
        ( GUID ,rep_guid ,row_no ,name ,beginning_amount ,ending_amount ) 
        SELECT NEWID(),@rep_guid,debt_row_no,debt_item_name,debt_start_amount,debt_end_amount
        FROM dbo.T_BalanceSheetTemplate
        IF @@ERROR>0
        BEGIN
            SET @error_msg= '�������渺ծ��ϸ�쳣:'+CAST(@@ERROR AS VARCHAR(10))
            ROLLBACK TRAN    
        END
        
    COMMIT TRAN
  
	
	SET NOCOUNT OFF;
	SET XACT_ABORT OFF;
END
GO


