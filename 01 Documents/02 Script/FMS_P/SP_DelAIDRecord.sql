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
-- Description:	删除收入费用记录
-- =============================================
ALTER PROCEDURE [dbo].[SP_DelAIDRecord]
	@ID NVARCHAR(40),
	@result             INT         OUT     --执行结果1=成功，-1=不可删除	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	DECLARE 	@RecPayRecord_GUID 	  VARCHAR(40)
	SET @result=1   --默认成功
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
	    SET @result=-1   --不可删除	    
	END
END






GO


