USE [DiaTaskData]

IF NOT EXISTS (SELECT 1 FROM SYS.OBJECTS obj INNER JOIN SYS.COLUMNS col ON (obj.object_id = col.object_id)
			   WHERE obj.name = 'AlgorithmPerfomance' AND col.name = 'CountTerminal')
	ALTER TABLE [AlgorithmPerfomance]
	ADD CountTerminal INT;

IF NOT EXISTS (SELECT 1 FROM SYS.OBJECTS obj INNER JOIN SYS.COLUMNS col ON (obj.object_id = col.object_id)
			   WHERE obj.name = 'AlgorithmPerfomance' AND col.name = 'UpdateOptcount')
	ALTER TABLE [AlgorithmPerfomance]
	ADD UpdateOptcount INT

IF NOT EXISTS (SELECT 1 FROM SYS.OBJECTS obj INNER JOIN SYS.COLUMNS col ON (obj.object_id = col.object_id)
			   WHERE obj.name = 'AlgorithmPerfomance' AND col.name = 'OptimalValue')
	ALTER TABLE [AlgorithmPerfomance]
	ADD OptimalValue INT

IF NOT EXISTS (SELECT 1 FROM SYS.OBJECTS obj INNER JOIN SYS.COLUMNS col ON (obj.object_id = col.object_id)
			   WHERE obj.name = 'AlgorithmPerfomance' AND col.name = 'ElemenationCount')
	ALTER TABLE [AlgorithmPerfomance]
	ADD ElemenationCount INT

IF NOT EXISTS (SELECT 1 FROM SYS.OBJECTS obj INNER JOIN SYS.COLUMNS col ON (obj.object_id = col.object_id)
			   WHERE obj.name = 'AlgorithmPerfomance' AND col.name = 'OptimalRoute')
	ALTER TABLE [AlgorithmPerfomance]
	ADD OptimalRoute VARCHAR(500)

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[addAlgorithmPerfomance]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[addAlgorithmPerfomance]
GO

IF  EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'AlgorithmPerfomanceType' AND ss.name = N'dbo')
DROP TYPE [dbo].[AlgorithmPerfomanceType]
GO

CREATE TYPE [dbo].[AlgorithmPerfomanceType] AS TABLE(
	[DBName] [nvarchar](500) NOT NULL,
	[Tables] [nvarchar](max) NOT NULL,
	[Algorithm] [nvarchar](max) NOT NULL,
	[NumberOfIteration] [bigint] NULL,
	[Duration] [bigint] NULL,
	[DurationMilliSeconds] [bigint] NULL,
	[DateComplete] [datetime] NULL,
	[IsComplete] [bit] NULL,
	[ElementCount] [int] NULL,
	[TableSetAsNumber] [int] NULL,
	[LastRoute] [varchar](500) NULL,
	[CountTerminal] [int] NULL,
	[UpdateOptcount] [int] NULL,
	[OptimalValue] [int] NULL,
	[ElemenationCount] [int] NULL,
	[OptimalRoute] [varchar](500) NULL
)
GO



CREATE PROCEDURE [dbo].[addAlgorithmPerfomance]
    @AlgorithmPerfomances dbo.[AlgorithmPerfomanceType] READONLY
AS    
BEGIN
		

	INSERT [AlgorithmPerfomance] ( DBName, [Tables],[Algorithm],[NumberOfIteration],[Duration], [DurationMilliSeconds], 
		[DateComplete], [IsComplete], [ElementCount], TableSetAsNumber, LastRoute,
		[CountTerminal],[UpdateOptcount],[OptimalValue],[ElemenationCount],[OptimalRoute])
	SELECT DBName, [Tables],[Algorithm],[NumberOfIteration],[Duration],[DurationMilliSeconds], 
		[DateComplete], [IsComplete], [ElementCount], TableSetAsNumber, LastRoute,
		[CountTerminal],[UpdateOptcount],[OptimalValue],[ElemenationCount],[OptimalRoute]
	FROM @AlgorithmPerfomances 
	

END




GO

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AlgorithmPerfomance]') AND type in (N'U'))
	CREATE TABLE [dbo].[AlgorithmOptimalValueChange](
		[AlgorithmOptimalValueChangeId] [int] IDENTITY(1,1) NOT NULL,
		[DBName] [nvarchar](500) NOT NULL,
		[Algorithm] [nvarchar](max) NOT NULL,
		[TableSetAsNumber] [int] NULL,
		[NumberOfIteration] [bigint] NULL,
		[Duration] [bigint] NULL,
		[DurationMilliSeconds] [bigint] NULL,
		[OptimalValue] [int] NULL,
		[OptimalRoute] [varchar](500) NULL,
	PRIMARY KEY CLUSTERED 
	(
		[AlgorithmOptimalValueChangeId] ASC
	)
	) ON [PRIMARY]

GO



