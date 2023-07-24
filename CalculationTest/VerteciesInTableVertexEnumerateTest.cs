using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DiaMakerModel;
using Common.CommonLib.Interfaces;
using System.Linq;
using OptimalPositionLib;
using OptimalPositionLib.Matrix;
using DiaMakerModel.Tracing;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using CommonLib;

namespace CalculationTest
{
    //--------------------------------------------------------------------------------------
    // class VerteciesInTableVertexEnumerateTest
    //--------------------------------------------------------------------------------------
    // add to table
    // binary representation of set of selected elements
    // last route
    [TestClass]
    public partial class VerteciesInTableVertexEnumerateTest
    {
        private const int BUFFERSIZE = 100;
        private List<PerformanceStatistics> performanceStatistics = new List<PerformanceStatistics>();
        private List<AlgorithmOptimalValueChange> algorithmOptimalValueChanges = new List<AlgorithmOptimalValueChange>();
        private string serverName = "BOTANIK2008";
//        private string serverName = "CRM";
        private string connectionString = @"Data Source=.\{PlaceHolder};Initial Catalog=Vocabulary;Integrated Security=true";
        private string connectionStringDTD = @"Data Source=.\{PlaceHolder};Initial Catalog=DiaTaskData;Integrated Security=true";
        private IList<string> AlgorithmList;
        private IList<string> AlgorithmWithRouteList;
        private IList<string> AlgorithmWithRoute2List;
        private IList<string> AlgorithmUnitedList;
        private IList<string> AlgorithmCompositeList;
        private IList<string> AlgorithmCompositeOnlyForceList;
        private IList<string> AlgorithmSortedList;
        private IList<string> AlgorithmSortedAdvList;
        private IList<string> AlgorithmVertexSortedList;
        private IList<string> AlgorithmVertexSortedAdvList;
        private IList<string> AlgorithmCompositeAllList;
        private IList<string> AlgorithmCompositeAllListRefined;
        private IList<string> AlgorithmIndList;

        private int lMaxDurability = 30000;
        //------------------------------------------------------------------------------------------------------------------
        [TestMethod]
        public void OptimalRouteTestNorth()
        {
            // 9 tables
            OptimalRouteTest(AlgorithmList, "North");
        }
        //------------------------------------------------------------------------------------------------------------------
        [TestMethod]
        public void OptimalRouteSortedTestNorth()
        {
            // 9 tables
            OptimalRouteTest(AlgorithmSortedList, "North");
        }
        //------------------------------------------------------------------------------------------------------------------
        [TestMethod]
        public void OptimalRouteSortedAdvTestNorth()
        {
            // 9 tables
            OptimalRouteTest(AlgorithmSortedAdvList, "North");
        }
        //------------------------------------------------------------------------------------------------------------------
        [TestMethod]
        public void OptimalRouteVertexSortedTestNorth()
        {
            // 9 tables
            OptimalRouteTest(AlgorithmVertexSortedList, "North");
        }
        //------------------------------------------------------------------------------------------------------------------
        [TestMethod]
        public void OptimalRouteVertexSortedAdvTestNorth()
        {
            // 9 tables
            OptimalRouteTest(AlgorithmVertexSortedAdvList, "North");
        }
        //------------------------------------------------------------------------------------------------------------------
        [TestMethod]
        public void OptimalRouteWithAsyncTestNorth()
        {
            // 9 tables
            OptimalRouteTest(AlgorithmList, "North", true, true);
        }
        //------------------------------------------------------------------------------------------------------------------
        [TestMethod]
        public void OptimalRouteWithRouteTestNorth()
        {
            // 9 tables
            OptimalRouteTest(AlgorithmWithRouteList, "North", true, false, true);
            OptimalRouteTest(AlgorithmWithRoute2List, "North", true, false, true);
            //            OptimalRouteTest(AlgorithmList, "North", true, false, true);
        }
        //------------------------------------------------------------------------------------------------------------------
        [TestMethod]
        public void OptimalRouteUnitedTestNorth()
        {
            // 9 tables
            OptimalRouteTest(AlgorithmUnitedList, "North");
        }
        //------------------------------------------------------------------------------------------------------------------
        [TestMethod]
        public void OptimalRouteByListTestNorth()
        {
            // 9 tables
            lMaxDurability = Int32.MaxValue;
            AlgorithmIndList = new List<string>() { "EOL_BEV_LEN_ETR_ISN_STB" };
            List<int> list = new List<int> { 507 };
            OptimalRouteByListTest(AlgorithmIndList, "North", list);
        }
        //------------------------------------------------------------------------------------------------------------------
        [TestMethod]
        public void OptimalRouteTestCompositeNorth()
        {
            // 9 tables
            OptimalRouteTest(AlgorithmCompositeList, "North");
        }
        //------------------------------------------------------------------------------------------------------------------
        [TestMethod]
        public void OptimalRouteTestCompositeOnlyForcedNorth()
        {
            // 9 tables
            OptimalRouteTest(AlgorithmCompositeOnlyForceList, "North", false, false, false);
        }
        //------------------------------------------------------------------------------------------------------------------
        [TestMethod]
        public void OptimalRouteTestCustomNorth()
        {
            // 9 tables
            lMaxDurability = Int32.MaxValue;
//            AlgorithmIndList = new List<string>() { "EOF_BEV_LEN_ETF_ISN_STB", "EOF_BEV_LEN_ETR_ISN_STB" };
            AlgorithmIndList = new List<string>() { "EOF_BEV_LEN_ETR_ISY_STP" };
            List<int> list = new List<int> { 507 };
            OptimalRouteByListTest(AlgorithmIndList, "North", list, false);
        }
        //------------------------------------------------------------------------------------------------------------------
        [TestMethod]
        public void OptimalRouteTestCompositeAllNorth()
        {
            // 9 tables
            OptimalRouteTest(AlgorithmCompositeAllList, "North", false, false, false);
        }
        //------------------------------------------------------------------------------------------------------------------
        [TestMethod]
        public void OptimalRouteTestEast()
        {
            // 12 tables
            OptimalRouteTest(AlgorithmList, "East", false);
        }
        //------------------------------------------------------------------------------------------------------------------
        [TestMethod]
        public void OptimalRouteTestCompositeEast()
        {
            // 12 tables
            OptimalRouteTest(AlgorithmCompositeAllListRefined, "East", false, false, false);
        }
        //------------------------------------------------------------------------------------------------------------------
        [TestMethod]
        public void OptimalRouteTestWest()
        {
            // 16 tables
            OptimalRouteTest(AlgorithmList, "West");
        }
        //------------------------------------------------------------------------------------------------------------------
        public void OptimalRouteTest(IList<string> pAlgorithmList, string pOptimalRouteDataBasesName, bool pIsTestResult = true, bool pIsAsyncTest = false, bool pIsTestRoute = false)
        {
            
            // Arrange 
            DataBaseRepository dataBaseRepository = DataBaseRepository.CreateInstance(connectionString);
            TableKeys tableKeys = dataBaseRepository.GetTables(pOptimalRouteDataBasesName);
            int maxVector = (1 << (tableKeys.Tables.Values.Count)) - 1;
            int current = 0;
            List<Table> tables = tableKeys.Tables.Values.ToList();
            performanceStatistics.Clear();
            algorithmOptimalValueChanges.Clear();

            foreach (string a in pAlgorithmList)
            {
                string deleteError = DeletePerformanceStatisticsForAlgorithm(a, pOptimalRouteDataBasesName);
                Assert.IsNull(deleteError, deleteError);
            }

            while (++current <= maxVector)
            {
                OptimalRouteTestBody(current, pAlgorithmList, pOptimalRouteDataBasesName, tableKeys, tables, pIsTestResult, pIsAsyncTest, pIsTestRoute );

            }

            if (performanceStatistics.Count > 0)
                SavePerformanceStatisticToDataBase();

        }
        //------------------------------------------------------------------------------------------------------------------
        public void OptimalRouteByListTest(IList<string> pAlgorithmList, string pOptimalRouteDataBasesName, List<int> pVectorList, bool pIsTestResult = true, bool pIsAsyncTest = false, bool pIsTestRoute = false)
        {

            // Arrange 
            DataBaseRepository dataBaseRepository = DataBaseRepository.CreateInstance(connectionString);
            TableKeys tableKeys = dataBaseRepository.GetTables(pOptimalRouteDataBasesName);
 //           int maxVector = (1 << (tableKeys.Tables.Values.Count)) - 1;
//            int current = 0;
            List<Table> tables = tableKeys.Tables.Values.ToList();
            performanceStatistics.Clear();
            algorithmOptimalValueChanges.Clear();

            foreach (string a in pAlgorithmList)
            {
                string deleteError = DeletePerformanceStatisticsForAlgorithm(a, pOptimalRouteDataBasesName);
                Assert.IsNull(deleteError, deleteError);
            }

            for (int i = 0; i < pVectorList.Count; i++ )
            {
                OptimalRouteTestBody(pVectorList[i], pAlgorithmList, pOptimalRouteDataBasesName, tableKeys, tables, pIsTestResult, pIsAsyncTest, pIsTestRoute);
            }

            if (performanceStatistics.Count > 0)
                SavePerformanceStatisticToDataBase();

        }
        //------------------------------------------------------------------------------------------------------------------
        public void OptimalRouteTestBody(int current, IList<string> pAlgorithmList, string pOptimalRouteDataBasesName, TableKeys tableKeys, List<Table> tables, bool pIsTestResult = true, bool pIsAsyncTest = false, bool pIsTestRoute = false)
        {
            VerteciesInTableDefine[] AlgorithmObjects = new VerteciesInTableDefine[pAlgorithmList.Count];
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
            lMatrixofDistance.Generate();
            string lSelectedTable = string.Join(", ", graph.Vertices.Select(v => v.Name));

            // Act 
            for (int i = 0; i < pAlgorithmList.Count; i++)
                AlgorithmObjects[i] = VerteciesInTableDefine.Create(pAlgorithmList[i], graph, lMatrixofDistance, lMaxDurability);
            for (int i = 0; i < AlgorithmObjects.Length; i++)
            {
                AlgorithmObjects[i].ExecuteBody();
                if (pIsAsyncTest)
                {
                    string syncRoute = AlgorithmObjects[i].OptimalRouteAsString;
                    int syncOpt = AlgorithmObjects[i].OptimalWeight;
                    IAsyncResult lAsyncResult = AlgorithmObjects[i].BeginExecute(null);
                    AlgorithmObjects[i].EndExecute(lAsyncResult);
                    if (!lAsyncResult.IsCompleted)
                        throw new Exception("Incorrect working of EndExecute.");
                    // Assert
                    if (AlgorithmObjects[i].IsComplete && syncOpt != AlgorithmObjects[i].OptimalWeight)
                    {
                        Assert.AreEqual(syncOpt, AlgorithmObjects[i].OptimalWeight, "Synchronose result <> asynchronous " + syncOpt.ToString() + " <> " + AlgorithmObjects[i].OptimalWeight.ToString() + "(" + AlgorithmObjects[i].AlgorithmName + ")");
                    }
                    if (AlgorithmObjects[i].IsComplete && syncRoute != AlgorithmObjects[i].OptimalRouteAsString)
                    {
                        Assert.AreEqual(syncOpt, AlgorithmObjects[i].OptimalWeight, "Synchronose route <> asynchronous " + syncRoute + " <> " + AlgorithmObjects[i].OptimalRouteAsString + "(" + AlgorithmObjects[i].AlgorithmName + ")");
                    }
                }
            }

            // Assert
            for (int i = 0; i < AlgorithmObjects.Length; i++)
                SavePerformanceStatistic(lSelectedTable, AlgorithmObjects[i], pOptimalRouteDataBasesName, current);

            if (pIsTestResult)
            {
                for (int i = 1; i < AlgorithmObjects.Length; i++)
                {
                    if (AlgorithmObjects[0].IsComplete && AlgorithmObjects[i].IsComplete &&
                        AlgorithmObjects[0].OptimalWeight != AlgorithmObjects[i].OptimalWeight)
                    {
                        Assert.AreEqual(AlgorithmObjects[0].OptimalWeight, AlgorithmObjects[i].OptimalWeight, AlgorithmObjects[0].AlgorithmName + " <> " + AlgorithmObjects[i].AlgorithmName + ". Selected tables: " + lSelectedTable);
                    }
                    if (pIsTestRoute)
                    {
                        if (AlgorithmObjects[0].IsComplete && AlgorithmObjects[i].IsComplete &&
                            AlgorithmObjects[0].OptimalRouteAsString != AlgorithmObjects[i].OptimalRouteAsString)
                        {
                            Assert.AreEqual(AlgorithmObjects[0].OptimalRouteAsString, AlgorithmObjects[i].OptimalRouteAsString, "OptRoute of " + AlgorithmObjects[0].AlgorithmName + " <> " + AlgorithmObjects[i].AlgorithmName + ". Selected tables: " + lSelectedTable);
                        }
                    }
                }
            }
        }
        //--------------------------------------------------------------------------------------
        private void SavePerformanceStatistic(string pSelectedTable, VerteciesInTableDefine pEnumerate, string pOptimalRouteDataBasesName, int pCurrentVector)
        {
            for (int i = 0; i < pEnumerate.AlgorithmOptimalValueChanges.Count; i++)
            {
                pEnumerate.AlgorithmOptimalValueChanges[i].DBName = pOptimalRouteDataBasesName;
                pEnumerate.AlgorithmOptimalValueChanges[i].TableSetAsNumber = pCurrentVector;
                algorithmOptimalValueChanges.Add(pEnumerate.AlgorithmOptimalValueChanges[i]);
            }
            performanceStatistics.Add(new PerformanceStatistics()
            {
                DBName = pOptimalRouteDataBasesName,
                Tables = pSelectedTable,
                Algorithm = pEnumerate.AlgorithmName,
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
                lMatrixofDistance.Generate();
                string lSelectedTable = string.Join(", ", graph.Vertices.Select(v => v.Name));

                // Act 
                VerteciesInTableDefine enumerate = VerteciesInTableDefine.Create("Enumerate", graph, lMatrixofDistance, lMaxDurability);
                VerteciesInTableDefine approximate = VerteciesInTableDefine.Create("Approximate", graph, lMatrixofDistance, lMaxDurability);
                enumerate.ExecuteBody();
                approximate.ExecuteBody();

                // Assert
                if (enumerate.OptimalWeight > approximate.OptimalWeight)
                {
                    Assert.AreEqual(enumerate.OptimalWeight, approximate.OptimalWeight, "Approximation is better than exact algorithm. Selected tables: " + lSelectedTable);
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

                DataTable optimalValueChanges = new DataTable();
                optimalValueChanges.Columns.Add("DBName", System.Type.GetType("System.String"));
                optimalValueChanges.Columns.Add("Algorithm", System.Type.GetType("System.String"));
                optimalValueChanges.Columns.Add("TableSetAsNumber", System.Type.GetType("System.Int32"));
                optimalValueChanges.Columns.Add("NumberOfIteration", System.Type.GetType("System.Int64"));
                optimalValueChanges.Columns.Add("Duration", System.Type.GetType("System.Int64"));
                optimalValueChanges.Columns.Add("DurationMilliSeconds", System.Type.GetType("System.Int64"));
                optimalValueChanges.Columns.Add("OptimalValue", System.Type.GetType("System.Int64"));
                optimalValueChanges.Columns.Add("OptimalRoute", System.Type.GetType("System.String"));
                optimalValueChanges.Columns.Add("OptimalNative", System.Type.GetType("System.String"));

                foreach (var ps in performanceStatistics)
                {
                    addedPerformance.Rows.Add(ps.DBName, ps.Tables, ps.Algorithm, ps.NumberOfIteration, ps.Duration,
                        ps.DurationMilliSeconds, ps.DateComplete, ps.IsComplete, ps.ElementCount, ps.TableSetAsNumber,
                        ps.LastRoute, ps.CountTerminal, ps.UpdateOptcount, ps.OptimalValue, ps.ElemenationCount, ps.OptimalRoute);
                }

                foreach (var aov in algorithmOptimalValueChanges)
                {
                    optimalValueChanges.Rows.Add(aov.DBName, aov.Algorithm, aov.TableSetAsNumber, aov.NumberOfIteration, aov.Duration,
                        aov.DurationMilliSeconds, aov.OptimalValue, aov.OptimalRoute, aov.OptimalNative);
                }

                performanceStatistics.Clear();
                algorithmOptimalValueChanges.Clear();
                
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
                    SqlParameter tvpParam1 = addCommand.Parameters.AddWithValue("@AlgorithmOptimalValueChange", optimalValueChanges);
                    tvpParam1.SqlDbType = SqlDbType.Structured;
                    tvpParam1.TypeName = "dbo.AlgorithmOptimalValueChangeType";
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
        public string DeletePerformanceStatisticsForAlgorithm(string pAlgorithm, string pOptimalRouteDataBasesName)
        {
            string error = null;
            try
            {
                SqlConnection connection = new SqlConnection(connectionStringDTD);
                connection.Open();
                try
                {
                    SqlCommand addCommand = new SqlCommand("dbo.DeletePerformanceStatisticsForAlgorythm", connection);
                    addCommand.CommandType = CommandType.StoredProcedure;
                    addCommand.CommandTimeout = 300;
                    addCommand.Parameters.AddWithValue("@DBName", pOptimalRouteDataBasesName).SqlDbType = SqlDbType.VarChar;
                    addCommand.Parameters.AddWithValue("@Algorithm", pAlgorithm).SqlDbType = SqlDbType.VarChar;
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
    }
    //--------------------------------------------------------------------------------------
}
