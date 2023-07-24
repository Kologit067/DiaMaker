USE [DiaTaskData]

IF NOT EXISTS (SELECT 1 FROM SYS.OBJECTS obj INNER JOIN SYS.COLUMNS col ON (obj.object_id = col.object_id)
			   WHERE obj.name = 'AlgorithmPerfomanceArchive' AND col.name = 'CountTerminal')
	ALTER TABLE AlgorithmPerfomanceArchive
	ADD CountTerminal INT;

IF NOT EXISTS (SELECT 1 FROM SYS.OBJECTS obj INNER JOIN SYS.COLUMNS col ON (obj.object_id = col.object_id)
			   WHERE obj.name = 'AlgorithmPerfomanceArchive' AND col.name = 'UpdateOptcount')
	ALTER TABLE AlgorithmPerfomanceArchive
	ADD UpdateOptcount INT

IF NOT EXISTS (SELECT 1 FROM SYS.OBJECTS obj INNER JOIN SYS.COLUMNS col ON (obj.object_id = col.object_id)
			   WHERE obj.name = 'AlgorithmPerfomanceArchive' AND col.name = 'OptimalValue')
	ALTER TABLE AlgorithmPerfomanceArchive
	ADD OptimalValue INT

IF NOT EXISTS (SELECT 1 FROM SYS.OBJECTS obj INNER JOIN SYS.COLUMNS col ON (obj.object_id = col.object_id)
			   WHERE obj.name = 'AlgorithmPerfomanceArchive' AND col.name = 'ElemenationCount')
	ALTER TABLE AlgorithmPerfomanceArchive
	ADD ElemenationCount INT

IF NOT EXISTS (SELECT 1 FROM SYS.OBJECTS obj INNER JOIN SYS.COLUMNS col ON (obj.object_id = col.object_id)
			   WHERE obj.name = 'AlgorithmPerfomanceArchive' AND col.name = 'OptimalRoute')
	ALTER TABLE AlgorithmPerfomanceArchive
	ADD OptimalRoute VARCHAR(500)

USE [DiaTaskData]
GO

/****** Object:  StoredProcedure [dbo].[addAlgorithmPerfomance]    Script Date: 02/05/2016 17:08:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[SaveDataPerfomanceToArchive]
AS    
BEGIN
	
	DECLARE @VersionNumber INT = (SELECT ISNULL(MAX(VersionNumber),0) + 1 FROM AlgorithmPerfomanceArchive)

	INSERT AlgorithmPerfomanceArchive ( VersionNumber, DBName, [Tables],[Algorithm],[NumberOfIteration],[Duration], [DurationMilliSeconds], 
		[DateComplete], [IsComplete], [ElementCount], TableSetAsNumber, LastRoute,
		[CountTerminal],[UpdateOptcount],[OptimalValue],[ElemenationCount],[OptimalRoute])
	SELECT @VersionNumber, DBName, [Tables],[Algorithm],[NumberOfIteration],[Duration],[DurationMilliSeconds], 
		[DateComplete], [IsComplete], [ElementCount], TableSetAsNumber, LastRoute,
		[CountTerminal],[UpdateOptcount],[OptimalValue],[ElemenationCount],[OptimalRoute]
	FROM [AlgorithmPerfomance]
	

END





GO


