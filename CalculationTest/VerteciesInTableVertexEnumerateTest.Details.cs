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
using CommonLib.Tools;

namespace CalculationTest
{
    //--------------------------------------------------------------------------------------
    // class VerteciesInTableVertexEnumerateTest
    //--------------------------------------------------------------------------------------
    // add to table
    // binary representation of set of selected elements
    // last route
    public partial class VerteciesInTableVertexEnumerateTest
    {
        //------------------------------------------------------------------------------------------------------------------
        [TestInitialize()]
        public void Initialize()
        {
            if (Environment.MachineName.ToUpper() == "OKOLOMIYETS-N")
                serverName = "CRM";
            connectionString = connectionString.Replace("{PlaceHolder}", serverName);
            connectionStringDTD = connectionStringDTD.Replace("{PlaceHolder}", serverName);
            AlgorithmList = new string[]{"Enumerate", "EnumerateAdvanced", "ByVertexEnumerate", "ByVertexEnumerateAdvanced",
            "EnumerateWithLowEstimate", "EnumerateAdvancedWithLowEstimate", "ByVertexEnumerateWithLowEstimate", 
            "ByVertexEnumerateAdvancedWithLowEstimate"};
            AlgorithmUnitedList = new string[] { "Enumerate", "Union", "UnionParallel" };
            AlgorithmSortedList = new string[] { "Enumerate", "EnumerateSortedPlace", "EnumerateSorted" };
            AlgorithmSortedAdvList = new string[] { "EnumerateAdvanced", "EnumerateAdvancedSortedPlace", "EnumerateAdvancedSorted" };
            AlgorithmVertexSortedList = new string[] { "ByVertexEnumerate", "ByVertexEnumerateSorted", "ByVertexEnumerateSortedVertex" };
            AlgorithmVertexSortedAdvList = new string[] { "ByVertexEnumerate", "ByVertexEnumerateAdvanced", "ByVertexEnumerateAdvancedSorted", "ByVertexEnumerateAdvancedSortedVertex" };
            AlgorithmWithRouteList = new string[] { "Enumerate", "EnumerateAdvanced", "EnumerateWithLowEstimate", "EnumerateAdvancedWithLowEstimate" };
            AlgorithmWithRoute2List = new string[] { "ByVertexEnumerate", "ByVertexEnumerateAdvanced", "ByVertexEnumerateWithLowEstimate", "ByVertexEnumerateAdvancedWithLowEstimate" };
            AlgorithmCompositeList = AlgorithmConfiguration.CreateAlgorithmNameAllPossibleConfiguration().Where(a => !a.Contains("ETR_")).ToList();
            AlgorithmCompositeList.Insert(0, "Enumerate");
            AlgorithmCompositeOnlyForceList = AlgorithmConfiguration.CreateAlgorithmNameAllPossibleConfiguration().Where(a => a.Contains("ETR_")).ToList();
            AlgorithmCompositeOnlyForceList.Insert(0, "Enumerate");
            AlgorithmCompositeAllList = AlgorithmConfiguration.CreateAlgorithmNameAllPossibleConfiguration();
            AlgorithmCompositeAllList.Insert(0, "Enumerate");
            AlgorithmCompositeAllListRefined = AlgorithmConfiguration.CreateAlgorithmNameAllPossibleConfiguration().Where(
                a =>
                    !(a.Contains("BEV_") && a.Contains("EOL_")) &&
                    !(a.Contains("ISN_") && a.Contains("STB")) &&
                    !(a.Contains("ISN_") && a.Contains("STP")) &&
                    !(a.Contains("ISN_") && a.Contains("STV")) &&
                    !(a.Contains("ISY_") && a.Contains("STN")) 
                    ).ToList();
            /*
                        new string[]{
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
                         * */
        }
        //------------------------------------------------------------------------------------------------------------------
    }
    //------------------------------------------------------------------------------------------------------------------
}
