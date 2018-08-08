USE [FMS_Develop]
GO

/****** Object:  StoredProcedure [dbo].[SP_DelAIDRecord]    Script Date: 01/03/2017 15:45:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	ɾ��������ü�¼
-- =============================================
ALTER PROCEDURE [dbo].[SP_DelAIDRecord]
	@ID NVARCHAR(40),
	@result             INT         OUT     --ִ�н��1=�ɹ���-1=����ɾ��	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	DECLARE 	@RecPayRecord_GUID 	  VARCHAR(40)
	SET @result=1   --Ĭ�ϳɹ�
		SELECT @RecPayRecord_GUID=RP_GUID
		FROM dbo.T_RecPayRecord 
		WHERE IE_GUID='@ID'
           			
	IF @RecPayRecord_GUID IS NULL
	BEGIN
	    BEGIN TRAN;
		DELETE dbo.T_AIDRecord WHERE GUID = @ID;  
        COMMIT TRAN;
    END
    	ELSE
	BEGIN
	    SET @result=-1   --����ɾ��	    
	END
END






GO


