using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib
{
    //-------------------------------------------------------------------------------------------------------
    // class PerformanceStatistics
    //-------------------------------------------------------------------------------------------------------
    public class PerformanceStatistics
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
        //-------------------------------------------------------------------------------------------------------
    }
    //-------------------------------------------------------------------------------------------------------
}
