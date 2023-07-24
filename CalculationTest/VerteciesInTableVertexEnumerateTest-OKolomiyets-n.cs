using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DiaMakerModel;
using Common.CommonLib.Interfaces;
using Caculation.GraphLib;
using System.Linq;
using OptimalPositionLib;
using OptimalPositionLib.Matrix;
using DiaMakerModel.Tracing;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace CalculationTest
{
    //--------------------------------------------------------------------------------------
    // class VerteciesInTableVertexEnumerateTest
    //--------------------------------------------------------------------------------------
    // add to table
    // binary representation of set of selected elements
    // last route
    [TestClass]
    public class VerteciesInTableVertexEnumerateTest
    {
        private const int BUFFERSIZE = 100; 
        private List<PerformanceStatistics> performanceStatistics = new List<PerformanceStatistics>();
        private string serverName = "BOTANIK2008";
//        private string serverName = "CRM";
        private string connectionString = @"Data Source=.\{PlaceHolder};Initial Catalog=Vocabulary;Integrated Security=true";
        private string connectionStringDTD = @"Data Source=.\{PlaceHolder};Initial Catalog=DiaTaskData;Integrated Security=true";
        private string[] AlgorythmList;
        private string[] AlgorythmWithRouteList;
        private string[] AlgorythmUnitedList;
        private string[] AlgorythmCompositeList;

        private int lMaxDurability = 120000;
        //------------------------------------------------------------------------------------------------------------------
        [TestInitialize()]
        public void Initialize()
        {
            if (Environment.MachineName.ToUpper() == "OKOLOMIYETS-N")
                serverName = "CRM";
            connectionString = connectionString.Replace("{PlaceHolder}", serverName);
            connectionStringDTD = connectionStringDTD.Replace("{PlaceHolder}", serverName);
            AlgorythmList = new string[]{"Enumerate", "EnumerateAdvanced", "ByVertexEnumerate", "ByVertexEnumerateAdvanced",
            "EnumerateWithLowEstimate", "EnumerateAdvancedWithLowEstimate", "ByVertexEnumerateWithLowEstimate", 
            "ByVertexEnumerateAdvancedWithLowEstimate"};
            AlgorythmUnitedList = new string[] { "Enumerate", "Union", "UnionParallel" };
            AlgorythmWithRouteList = new string[] { "Enumerate", "EnumerateAdvanced", "EnumerateAdvancedWithLowEstimate" };
            AlgorythmCompositeList = new string[]{
                "Enumerate",
                "EOF_BEP_LEY_ETF", 
                "EOF_BEP_LEY_ETS", 
                "EOF_BEP_LEY_ETR", 
                "EOF_BEP_LEN_ETF", 
                "EOF_BEP_LEN_ETS", 
                "EOF_BEP_LEN_ETR", 
                "EOF_BEV_LEY_ETF", 
                "EOF_BEV_LEY_ETS", 
                "EOF_BEV_LEY_ETR", 
                "EOF_BEV_LEN_ETF", 
                "EOF_BEV_LEN_ETS", 
                "EOF_BEV_LEN_ETR", 
                "EOL_BEP_LEY_ETF", 
                "EOL_BEP_LEY_ETS", 
                "EOL_BEP_LEY_ETR", 
                "EOL_BEP_LEN_ETF", 
                "EOL_BEP_LEN_ETS", 
                "EOL_BEP_LEN_ETR", 
                "EOL_BEV_LEY_ETF", 
                "EOL_BEV_LEY_ETS", 
                "EOL_BEV_LEY_ETR", 
                "EOL_BEV_LEN_ETF", 
                "EOL_BEV_LEN_ETS", 
                "EOL_BEV_LEN_ETR"
            };
        }
        //------------------------------------------------------------------------------------------------------------------
        [TestMethod]
        public void OptimalRouteTestNorth()
        {
            // 9 tables
            OptimalRouteTest(AlgorythmList, "North");
        }
        //------------------------------------------------------------------------------------------------------------------
        [TestMethod]
        public void OptimalRouteWithAsyncTestNorth()
        {
            // 9 tables
            OptimalRouteTest(AlgorythmList, "North", true, true);
        }
        //------------------------------------------------------------------------------------------------------------------
        [TestMethod]
        public void OptimalRouteWithRouteTestNorth()
        {
            // 9 tables
//            OptimalRouteTest(AlgorythmWithRouteList, "North", true, false, true);
            OptimalRouteTest(AlgorythmList, "North", true, false, true);
        }
        //------------------------------------------------------------------------------------------------------------------
        [TestMethod]
        public void OptimalRouteUnitedTestNorth()
        {
            // 9 tables
            OptimalRouteTest(AlgorythmUnitedList, "North");
        }
        //------------------------------------------------------------------------------------------------------------------
        [TestMethod]
        public void OptimalRouteByListTestNorth()
        {
            // 9 tables
            List<int> list = new List<int> { 9, 10, 31, 44, 45 };
            OptimalRouteByListTest(AlgorythmList, "North", list);
        }
        //------------------------------------------------------------------------------------------------------------------
        [TestMethod]
        public void OptimalRouteTestCompositeNorth()
        {
            // 9 tables
            OptimalRouteTest(AlgorythmCompositeList, "North");
        }
        //------------------------------------------------------------------------------------------------------------------
        [TestMethod]
        public void OptimalRouteTestEast()
        {
            // 12 tables
            OptimalRouteTest(AlgorythmList, "East", false);
        }
        //------------------------------------------------------------------------------------------------------------------
        [TestMethod]
        public void OptimalRouteTestWest()
        {
            // 16 tables
            OptimalRouteTest(AlgorythmList, "West");
        }
        //------------------------------------------------------------------------------------------------------------------
        public void OptimalRouteTest(string[] pAlgorythmList, string pOptimalRouteDataBasesName, bool pIdTestResult = true, bool pIsAsyncTest = false, bool pIsTestRoute = false)
        {
            
            // Arrange 
            DataBaseRepository dataBaseRepository = DataBaseRepository.CreateInstance(connectionString);
            TableKeys tableKeys = dataBaseRepository.GetTables(pOptimalRouteDataBasesName);
            int maxVector = (1 << (tableKeys.Tables.Values.Count)) - 1;
            int current = 0;
            List<Table> tables = tableKeys.Tables.Values.ToList();
            performanceStatistics.Clear();

            foreach (string a in pAlgorythmList)
            {
                string deleteError = DeletePerformanceStatisticsForAlgorythm(a, pOptimalRouteDataBasesName);
                Assert.IsNull(deleteError, deleteError);
            }

            while (++current <= maxVector)
            {
                OptimalRouteTestBody(current, pAlgorythmList, pOptimalRouteDataBasesName, tableKeys, tables, pIdTestResult, pIsAsyncTest, pIsTestRoute );

            }

            if (performanceStatistics.Count > 0)
                SavePerformanceStatisticToDataBase();

        }
        //------------------------------------------------------------------------------------------------------------------
        public void OptimalRouteByListTest(string[] pAlgorythmList, string pOptimalRouteDataBasesName, List<int> pVectorList, bool pIsTestResult = true, bool pIsAsyncTest = false, bool pIsTestRoute = false)
        {

            // Arrange 
            DataBaseRepository dataBaseRepository = DataBaseRepository.CreateInstance(connectionString);
            TableKeys tableKeys = dataBaseRepository.GetTables(pOptimalRouteDataBasesName);
            int maxVector = (1 << (tableKeys.Tables.Values.Count)) - 1;
            int current = 0;
            List<Table> tables = tableKeys.Tables.Values.ToList();
            performanceStatistics.Clear();

            foreach (string a in pAlgorythmList)
            {
                string deleteError = DeletePerformanceStatisticsForAlgorythm(a, pOptimalRouteDataBasesName);
                Assert.IsNull(deleteError, deleteError);
            }

            for (int i = 0; i < pVectorList.Count; i++ )
            {
                OptimalRouteTestBody(pVectorList[i], pAlgorythmList, pOptimalRouteDataBasesName, tableKeys, tables, pIsTestResult, pIsAsyncTest, pIsTestRoute);
            }

            if (performanceStatistics.Count > 0)
                SavePerformanceStatisticToDataBase();

        }
        //------------------------------------------------------------------------------------------------------------------
        public void OptimalRouteTestBody(int current, string[] pAlgorythmList, string pOptimalRouteDataBasesName, TableKeys tableKeys, List<Table> tables, bool pIsTestResult = true, bool pIsAsyncTest = false, bool pIsTestRoute = false)
        {
            VerteciesInTableDefine[] AlgorythmObjects = new VerteciesInTableDefine[pAlgorythmList.Length];
            for (int i = 0; i < tables.Count; i++)
            {
                if ((current & (1 << i)) > 0)
                    tables[i].IsSelected = true;
                else
                    tables[i].IsSelected = false;
            }
            IGraph<IVertex> graph = tableKeys.CreateGraph();
            PresentationData presentationData = new PresentationData(100, 100, graph);
            var sizes = presentationData.Sizes;
            Assert.AreNotEqual(sizes.Item1, 0);
            Assert.AreNotEqual(sizes.Item2, 0);

            IMatrixOfDistance lMatrixofDistance = new CMatrixOfDistance(sizes.Item1, sizes.Item2);
            string lSelectedTable = string.Join(", ", graph.Vertices.Select(v => v.Name));

            // Act 
            for (int i = 0; i < pAlgorythmList.Length; i++)
                AlgorythmObjects[i] = VerteciesInTableDefine.Create(pAlgorythmList[i], graph, lMatrixofDistance, lMaxDurability);
            for (int i = 0; i < AlgorythmObjects.Length; i++)
            {
                AlgorythmObjects[i].Execute();
                if (pIsAsyncTest)
                {
                    string syncRoute = AlgorythmObjects[i].OptimalRouteAsString;
                    int syncOpt = AlgorythmObjects[i].OptimalWeight;
                    IAsyncResult lAsyncResult = AlgorythmObjects[i].BeginExecute(null);
                    AlgorythmObjects[i].EndExecute(lAsyncResult);
                    // Assert
                    if (AlgorythmObjects[i].IsComplete && syncOpt != AlgorythmObjects[i].OptimalWeight)
                    {
                        Assert.AreEqual(syncOpt, AlgorythmObjects[i].OptimalWeight, "Synchronose result <> asynchronous " + syncOpt.ToString() + " <> " + AlgorythmObjects[i].OptimalWeight.ToString() + "(" + AlgorythmObjects[i].AlgorythmName + ")");
                    }
                    if (AlgorythmObjects[i].IsComplete && syncRoute != AlgorythmObjects[i].OptimalRouteAsString)
                    {
                        Assert.AreEqual(syncOpt, AlgorythmObjects[i].OptimalWeight, "Synchronose route <> asynchronous " + syncRoute + " <> " + AlgorythmObjects[i].OptimalRouteAsString + "(" + AlgorythmObjects[i].AlgorythmName + ")");
                    }
                }
            }

            for (int i = 0; i < AlgorythmObjects.Length; i++)
                SavePerformanceStatistic(lSelectedTable, AlgorythmObjects[i], pOptimalRouteDataBasesName, current);

            // Assert
            for (int i = 0; i < AlgorythmObjects.Length; i++)
                SavePerformanceStatistic(lSelectedTable, AlgorythmObjects[i], pOptimalRouteDataBasesName, current);

            if (pIsTestResult)
            {
                for (int i = 1; i < AlgorythmObjects.Length; i++)
                {
                    if (AlgorythmObjects[0].IsComplete && AlgorythmObjects[i].IsComplete &&
                        AlgorythmObjects[0].OptimalWeight != AlgorythmObjects[i].OptimalWeight)
                    {
                        Assert.AreEqual(AlgorythmObjects[0].OptimalWeight, AlgorythmObjects[i].OptimalWeight, AlgorythmObjects[0].AlgorythmName + " <> " + AlgorythmObjects[i].AlgorythmName + ". Selected tables: " + lSelectedTable);
                    }
                    if (pIsTestRoute)
                    {
                        if (AlgorythmObjects[0].IsComplete && AlgorythmObjects[i].IsComplete &&
                            AlgorythmObjects[0].OptimalRouteAsString != AlgorythmObjects[i].OptimalRouteAsString)
                        {
                            Assert.AreEqual(AlgorythmObjects[0].OptimalRouteAsString, AlgorythmObjects[i].OptimalRouteAsString, "OptRoute of " + AlgorythmObjects[0].AlgorythmName + " <> " + AlgorythmObjects[i].AlgorythmName + ". Selected tables: " + lSelectedTable);
                        }
                    }
                }
            }
        }
        //--------------------------------------------------------------------------------------
        private void SavePerformanceStatistic(string pSelectedTable, VerteciesInTableDefine pEnumerate, string pOptimalRouteDataBasesName, int pCurrentVector)
        {
            performanceStatistics.Add(new PerformanceStatistics()
            {
                DBName = pOptimalRouteDataBasesName,
                Tables = pSelectedTable,
                Algorithm = pEnumerate.AlgorythmName,
                NumberOfIteration = pEnumerate.IterationCount,
                Duration = pEnumerate.ElapsedTicks,
                DurationMilliSeconds = pEnumerate.DurationMilliSeconds,
                DateComplete = DateTime.Now,
                IsComplete = pEnumerate.IsComplete,
                ElementCount = pEnumerate.Graph.Vertices.Count,
                TableSetAsNumber = pCurrentVector,
                LastRoute = pEnumerate.RouteAsString,
                CountTerminal = pEnumerate.CountTerminal,
                UpdateOptcount = pEnumerate.UpdateOptcount,
                OptimalValue = pEnumerate.OptimalWeight,
                ElemenationCount = pEnumerate.ElemenationCount,
                OptimalRoute = pEnumerate.OptimalRouteAsString
            });
            if (performanceStatistics.Count > BUFFERSIZE)
                SavePerformanceStatisticToDataBase();
        }
        //--------------------------------------------------------------------------------------
        [TestMethod]
        public void ApproximationAlgorithmTest()
        {
            // Arrange 
            DataBaseRepository dataBaseRepository = DataBaseRepository.CreateInstance(connectionString);
            string dataBasesName = "North";
            TableKeys tableKeys = dataBaseRepository.GetTables(dataBasesName);
            int maxVector = (1 << (tableKeys.Tables.Values.Count - 1)) - 1;
            int current = 0;
            var tables = tableKeys.Tables.Values.ToList();

            StringBuilder difResults = new StringBuilder();

            while (++current < maxVector)
            {
                for (int i = 0; i < tables.Count; i++)
                {
                    if ((current & (1 << i)) > 0)
                        tables[i].IsSelected = true;
                    else
                        tables[i].IsSelected = false;
                }
                IGraph<IVertex> graph = tableKeys.CreateGraph();
                PresentationData presentationData = new PresentationData(100, 100, graph);
                var sizes = presentationData.Sizes;
                Assert.AreNotEqual(sizes.Item1, 0);
                Assert.AreNotEqual(sizes.Item2, 0);

                IMatrixOfDistance lMatrixofDistance = new CMatrixOfDistance(sizes.Item1, sizes.Item2); 
                string lSelectedTable = string.Join(", ", graph.Vertices.Select(v => v.Name));

                // Act 
                VerteciesInTableDefine enumerate = VerteciesInTableDefine.Create("Enumerate", graph, lMatrixofDistance, lMaxDurability);
                VerteciesInTableDefine approximate = VerteciesInTableDefine.Create("Approximate", graph, lMatrixofDistance, lMaxDurability);
                enumerate.Execute();
                approximate.Execute();

                // Assert
                if (enumerate.OptimalWeight > approximate.OptimalWeight)
                {
                    Assert.AreEqual(enumerate.OptimalWeight, approximate.OptimalWeight, "Approximation is better than exact algorythm. Selected tables: " + lSelectedTable);
                }
                if (enumerate.OptimalWeight < approximate.OptimalWeight)
                {
                    difResults.Append(lSelectedTable).Append(" ").Append(enumerate.OptimalWeight.ToString()).Append("/").AppendLine(approximate.OptimalWeight.ToString());
                }
            }
            string s = difResults.ToString();
            Console.Write(s);
        }
        //--------------------------------------------------------------------------------------
        public string SavePerformanceStatisticToDataBase()
        {
            string error = null;
            
            try
            {
                DataTable addedPerformance = new DataTable();
                addedPerformance.Columns.Add("DBName", System.Type.GetType("System.String"));
                addedPerformance.Columns.Add("Tables", System.Type.GetType("System.String"));
                addedPerformance.Columns.Add("Algorithm", System.Type.GetType("System.String"));
                addedPerformance.Columns.Add("NumberOfIteration", System.Type.GetType("System.Int64"));
                addedPerformance.Columns.Add("Duration", System.Type.GetType("System.Int64"));
                addedPerformance.Columns.Add("DurationMilliSeconds", System.Type.GetType("System.Int64"));
                addedPerformance.Columns.Add("DateComplete", System.Type.GetType("System.DateTime"));
                addedPerformance.Columns.Add("IsComplete", System.Type.GetType("System.Boolean"));
                addedPerformance.Columns.Add("ElementCount", System.Type.GetType("System.Int32"));
                addedPerformance.Columns.Add("TableSetAsNumber", System.Type.GetType("System.Int32"));
                addedPerformance.Columns.Add("LastRoute", System.Type.GetType("System.String"));
                addedPerformance.Columns.Add("CountTerminal", System.Type.GetType("System.Int64"));
                addedPerformance.Columns.Add("UpdateOptcount", System.Type.GetType("System.Int64"));
                addedPerformance.Columns.Add("OptimalValue", System.Type.GetType("System.Int64"));
                addedPerformance.Columns.Add("ElemenationCount", System.Type.GetType("System.Int64"));
                addedPerformance.Columns.Add("OptimalRoute", System.Type.GetType("System.String"));


                foreach (var ps in performanceStatistics)
                {
                    addedPerformance.Rows.Add(ps.DBName, ps.Tables, ps.Algorithm, ps.NumberOfIteration, ps.Duration,
                        ps.DurationMilliSeconds, ps.DateComplete, ps.IsComplete, ps.ElementCount, ps.TableSetAsNumber, 
                        ps.LastRoute, ps.CountTerminal, ps.UpdateOptcount, ps.OptimalValue, ps.ElemenationCount, ps.OptimalRoute);
                }
                performanceStatistics.Clear();
                SqlConnection connection = new SqlConnection(connectionStringDTD);
                connection.Open();
                try
                {
                    SqlCommand addCommand = new SqlCommand("addAlgorithmPerfomance", connection);
                    addCommand.CommandType = CommandType.StoredProcedure;
                    addCommand.CommandTimeout = 300;
                    SqlParameter tvpParam = addCommand.Parameters.AddWithValue("@AlgorithmPerfomances", addedPerformance);
                    tvpParam.SqlDbType = SqlDbType.Structured;
                    tvpParam.TypeName = "dbo.AlgorithmPerfomanceType";
                    addCommand.ExecuteNonQuery();
                }
                finally
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                error = ex.ToString();
            }
            return error;
        }
        //--------------------------------------------------------------------------------------
        public string DeletePerformanceStatisticsForAlgorythm(string pAlgorythm, string pOptimalRouteDataBasesName)
        {
            string error = null;
            try
            {
                SqlConnection connection = new SqlConnection(connectionStringDTD);
                connection.Open();
                try
                {
                    SqlCommand addCommand = new SqlCommand("DeletePerformanceStatisticsForAlgorythm", connection);
                    addCommand.CommandType = CommandType.StoredProcedure;
                    addCommand.CommandTimeout = 300;
                    addCommand.Parameters.AddWithValue("@DBName", pOptimalRouteDataBasesName).SqlDbType = SqlDbType.VarChar;
                    addCommand.Parameters.AddWithValue("@Algorithm", pAlgorythm).SqlDbType = SqlDbType.VarChar;
                    addCommand.ExecuteNonQuery();
                }
                finally
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                error = ex.ToString();
            }
            return error;
        }
        //--------------------------------------------------------------------------------------
        private class PerformanceStatistics
        {
            public string DBName { get; set; }
            public string Tables { get; set; }
            public string Algorithm { get; set; }
            public long NumberOfIteration { get; set; }
            public long Duration { get; set; }
            public long DurationMilliSeconds { get; set; }
            public DateTime DateComplete { get; set; }
            public bool IsComplete { get; set; }
            public int ElementCount { get; set; }
            public int TableSetAsNumber { get; set; }
            public string LastRoute { get; set; }
            public long CountTerminal { get; set; }
            public long UpdateOptcount { get; set; }
            public long OptimalValue { get; set; }
            public long ElemenationCount { get; set; }
            public string OptimalRoute { get; set; }
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
}
