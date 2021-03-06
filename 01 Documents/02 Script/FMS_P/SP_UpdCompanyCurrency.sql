USE [FMS_Develop]
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdCompanyCurrency]    Script Date: 04/18/2017 09:58:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE[dbo].[SP_UpdCompanyCurrency] 
	@C_GUID NVARCHAR(50),
	@Code NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   update dbo.R_CompanyCurrceny set Code=@Code where C_GUID=@C_GUID;
   
   DECLARE @id NVARCHAR(50) = NEWID();
   INSERT INTO dbo.T_RateHistory(GUID,Date,C_GUID,CurrentRecord,Currency)
	VALUES(@id,getdate(),@C_GUID,'1',@Code);
END
