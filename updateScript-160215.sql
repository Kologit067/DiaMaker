USE [DiaTaskData]

USE [DiaTaskData]
GO

/****** Object:  UserDefinedFunction [dbo].[PV_ComparePerfomance2Algorithm]    Script Date: 02/15/2016 07:31:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

 CREATE FUNCTION [dbo].[PV_ComparePerfomance2Algorithm]
(     
       
       @Algorith1 VARCHAR(100),
       @Algorith2 VARCHAR(100)
)
RETURNS TABLE
AS
RETURN
(
SELECT a1.[DBName],
       a1.[Tables],
       a1.[ElementCount],
       a1.[TableSetAsNumber]
      ,a1.[NumberOfIteration] as a1_NumberOfIteration
      ,a2.[NumberOfIteration] as a2_NumberOfIteration
      ,a1.[DurationMilliSeconds] as a1_DurationMilliSeconds
      ,a2.[DurationMilliSeconds] as a2_DurationMilliSeconds
      ,a1.[Duration] as a1_Duration
      ,a2.[Duration] as a2_Duration
      ,a1.[ElemenationCount] as a1_ElemenationCount
      ,a2.[ElemenationCount] as a2_ElemenationCount
      ,a1.[OptimalValue] as a1_OptimalValue
      ,a2.[OptimalValue] as a2_OptimalValue
      ,a1.[OptimalRoute] as a1_OptimalRoute
      ,a2.[OptimalRoute] as a2_OptimalRoute
      ,a1.[CountTerminal] as a1_CountTerminal
      ,a2.[CountTerminal] as a2_CountTerminal
      ,a1.[UpdateOptcount] as a1_UpdateOptcount
      ,a2.[UpdateOptcount] as a2_UpdateOptcount
      ,a1.[IsComplete] as a1_IsComplete
      ,a2.[IsComplete] as a2_IsComplete
      ,a1.[LastRoute] as a1_LastRoute
      ,a2.[LastRoute] as a2_LastRoute
FROM [dbo].[AlgorithmPerfomance] a1
INNER JOIN [dbo].[AlgorithmPerfomance] a2
ON a1.[DBName] = a2.[DBName] AND a1.[TableSetAsNumber] = a2.[TableSetAsNumber] AND
	a1.[Algorithm] = @Algorith1 AND a2.[Algorithm] = @Algorith2

)
GO


USE [DiaTaskData]
GO

/****** Object:  View [dbo].[vw_AlgorithmName]    Script Date: 02/15/2016 07:37:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[vw_AlgorithmName]
AS
SELECT DISTINCT [Algorithm]
FROM [dbo].[AlgorithmPerfomance]

GO


USE [DiaTaskData]
GO

/****** Object:  View [dbo].[vw_Enumerate_vs_ByVertexEnumerate]    Script Date: 02/15/2016 07:37:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vw_Enumerate_vs_ByVertexEnumerate]
AS
SELECT [DBName],
       [Tables],
       [ElementCount],
       [TableSetAsNumber]
      ,a1_NumberOfIteration
      ,a2_NumberOfIteration
      ,a1_DurationMilliSeconds
      ,a2_DurationMilliSeconds
      ,a1_Duration
      ,a2_Duration
      ,a1_ElemenationCount
      ,a2_ElemenationCount
      ,a1_OptimalValue
      ,a2_OptimalValue
      ,a1_OptimalRoute
      ,a2_OptimalRoute
      ,a1_CountTerminal
      ,a2_CountTerminal
      ,a1_UpdateOptcount
      ,a2_UpdateOptcount
      ,a1_IsComplete
      ,a2_IsComplete
      ,a1_LastRoute
      ,a2_LastRoute
FROM PV_ComparePerfomance2Algorithm('Enumerate','ByVertexEnumerate')
--WHERE [ElementCount] = 2
--	AND a1_NumberOfIteration > a2_NumberOfIteration
--ORDER BY [TableSetAsNumber] DESC
GO


USE [DiaTaskData]
GO

/****** Object:  View [dbo].[vw_place_eof_vs_place_eol]    Script Date: 02/15/2016 07:37:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[vw_place_eof_vs_place_eol]
AS
SELECT [DBName],
       [Tables],
       [ElementCount],
       [TableSetAsNumber]
      ,a1_NumberOfIteration
      ,a2_NumberOfIteration
      ,a1_DurationMilliSeconds
      ,a2_DurationMilliSeconds
      ,a1_Duration
      ,a2_Duration
      ,a1_ElemenationCount
      ,a2_ElemenationCount
      ,a1_OptimalValue
      ,a2_OptimalValue
      ,a1_OptimalRoute
      ,a2_OptimalRoute
      ,a1_CountTerminal
      ,a2_CountTerminal
      ,a1_UpdateOptcount
      ,a2_UpdateOptcount
      ,a1_IsComplete
      ,a2_IsComplete
      ,a1_LastRoute
      ,a2_LastRoute
FROM PV_ComparePerfomance2Algorithm('EOF_BEP_LEN_ETF_ISN_STN','EOL_BEP_LEN_ETF_ISN_STN')
--WHERE [ElementCount] = 2
--	AND a1_NumberOfIteration > a2_NumberOfIteration
--ORDER BY [TableSetAsNumber] DESC

GO


USE [DiaTaskData]
GO

/****** Object:  View [dbo].[vw_place_vs_placeadv]    Script Date: 02/15/2016 07:38:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[vw_place_vs_placeadv]
AS
SELECT [DBName],
       [Tables],
       [ElementCount],
       [TableSetAsNumber]
      ,a1_NumberOfIteration
      ,a2_NumberOfIteration
      ,a1_DurationMilliSeconds
      ,a2_DurationMilliSeconds
      ,a1_Duration
      ,a2_Duration
      ,a1_ElemenationCount
      ,a2_ElemenationCount
      ,a1_OptimalValue
      ,a2_OptimalValue
      ,a1_OptimalRoute
      ,a2_OptimalRoute
      ,a1_CountTerminal
      ,a2_CountTerminal
      ,a1_UpdateOptcount
      ,a2_UpdateOptcount
      ,a1_IsComplete
      ,a2_IsComplete
      ,a1_LastRoute
      ,a2_LastRoute
FROM PV_ComparePerfomance2Algorithm('Enumerate','EnumerateAdvanced')
--WHERE [ElementCount] = 2
--	AND a1_NumberOfIteration > a2_NumberOfIteration
--ORDER BY [TableSetAsNumber] DESC

GO


