using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib
{
    //-------------------------------------------------------------------------------------------------------
    // class AlgorithmOptimalValueChange
    //-------------------------------------------------------------------------------------------------------
    public class AlgorithmOptimalValueChange
    {
        public string DBName { get; set; }
        public int TableSetAsNumber { get; set; }
        public string Algorithm { get; set; }
        public long NumberOfIteration { get; set; }
        public long Duration { get; set; }
        public long DurationMilliSeconds { get; set; }
        public long OptimalValue { get; set; }
        public string OptimalRoute { get; set; }
        public string OptimalNative { get; set; }
        //-------------------------------------------------------------------------------------------------------
    }
    //-------------------------------------------------------------------------------------------------------
}
