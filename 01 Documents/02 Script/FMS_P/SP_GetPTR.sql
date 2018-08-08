	USE [FMS_Develop]
	GO

	/****** Object:  StoredProcedure [dbo].[SP_GetPTR]    Script Date: 02/09/2017 09:53:47 ******/
	SET ANSI_NULLS ON
	GO

	SET QUOTED_IDENTIFIER ON
	GO

	-- =============================================
	-- Author:		<Author,,Name>
	-- Create date: <Create Date,,>
	-- Description:	获取物料类别
	-- =============================================
	ALTER PROCEDURE [dbo].[SP_GetPTR]
	@C_GUID NVARCHAR(50),
	@Flag NVARCHAR(1),
	@Count INT = 0 OUT
	AS
	BEGIN
		-- SET NOCOUNT ON added to prevent extra result sets from
		-- interfering with SELECT statements.
		SET NOCOUNT ON;	
		 DECLARE @temp TABLE(
			rownumber INT,
			GUID NVARCHAR(50),
			AT_GUID NVARCHAR(50),
			AidTypeName  NVARCHAR(40),
			AID_FLAG NVARCHAR(1),
			C_GUID NVARCHAR(40),
			AST_ParentAidType NVARCHAR(40),
			Remark NVARCHAR(40),
			Asset_class NVARCHAR(40),
			Depreciation_year NVARCHAR(40)
		 )		 
		INSERT INTO @temp 		
		SELECT ROW_NUMBER()OVER(ORDER BY ATR.AT_GUID DESC) rownumber,ATR.AT_GUID,ATR.AT_GUID,ATR.AidTypeName,ATR.AID_FLAG,ATR.C_GUID,NULL,NULL,ATR.Asset_class,NULL
		FROM dbo.T_AIDTypeRecord ATR
		WHERE ATR.C_GUID=@C_GUID 
		AND (ATR.AID_FLAG=@Flag OR @Flag IS NULL OR LEN(@Flag) = 0)	
		UNION
		SELECT ROW_NUMBER()OVER(ORDER BY AST.AST_GUID DESC) rownumber,AST.AST_GUID,AST.AST_ParentAidType,AST.ASTTypeName,ATR.AID_FLAG,NULL,ATR.AidTypeName,AST.Remark,ATR.Asset_class,AST.Depreciation_year
		FROM dbo.T_AIDSubTypeRecord AST
		LEFT JOIN dbo.T_AIDTypeRecord ATR ON ATR.AT_GUID = AST.AST_ParentAidType 
		WHERE ATR.C_GUID=@C_GUID 
		
			
		AND (ATR.AID_FLAG=@Flag OR @Flag IS NULL OR LEN(@Flag) = 0)
			UPDATE 	@temp SET Asset_class='' WHERE Depreciation_year IS NOT NULL 
			SELECT @Count = COUNT(*) FROM @temp;
		
		SELECT  *
			FROM @temp T ORDER BY AT_GUID ,AST_ParentAidType;
			
	END

	GO


