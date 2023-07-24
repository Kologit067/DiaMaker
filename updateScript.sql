GO

/****** Object:  StoredProcedure [dbo].[addAlgorithmPerfomance]    Script Date: 01/12/2016 15:50:59 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[addAlgorithmPerfomance]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[addAlgorithmPerfomance]
GO

USE [DiaTaskData]
GO

/****** Object:  UserDefinedTableType [dbo].[AlgorithmPerfomanceType]    Script Date: 01/12/2016 15:49:23 ******/
IF  EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'AlgorithmPerfomanceType' AND ss.name = N'dbo')
DROP TYPE [dbo].[AlgorithmPerfomanceType]
GO

USE [DiaTaskData]
GO

/****** Object:  UserDefinedTableType [dbo].[AlgorithmPerfomanceType]    Script Date: 01/12/2016 15:49:23 ******/
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
	TableSetAsNumber INT NULL,
	LastRoute VARCHAR(500) NULL
)
GO

CREATE PROCEDURE [dbo].[addAlgorithmPerfomance]
    @AlgorithmPerfomances dbo.[AlgorithmPerfomanceType] READONLY
AS    
BEGIN
		

	INSERT [AlgorithmPerfomance] ( DBName, [Tables],[Algorithm],[NumberOfIteration],[Duration], [DurationMilliSeconds], 
		[DateComplete], [IsComplete], [ElementCount], TableSetAsNumber, LastRoute)
	SELECT DBName, [Tables],[Algorithm],[NumberOfIteration],[Duration],[DurationMilliSeconds], 
		[DateComplete], [IsComplete], [ElementCount], TableSetAsNumber, LastRoute

	FROM @AlgorithmPerfomances 

END



GO




