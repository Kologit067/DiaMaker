#define TESTREGIM

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib;

namespace OptimalPositionLib
{
    //--------------------------------------------------------------------------------------
    // class VerteciesInTableDefine
    //--------------------------------------------------------------------------------------
    public partial class VerteciesInTableDefine
    {
#if TESTREGIM
        //--------------------------------------------------------------------------------------
        partial void AddOptimalValueChangeDataBody()
        {
            AlgorithmOptimalValueChange aov = new AlgorithmOptimalValueChange()
            {
                Algorithm = AlgorithmName,
                NumberOfIteration = fIterationCount,
                Duration = stopwatch.ElapsedTicks,
                DurationMilliSeconds = stopwatch.ElapsedMilliseconds,
                OptimalValue = fOptimalWeight,
                OptimalRoute = OptimalRouteAsString,
                OptimalNative = NativeRouteAsString
            };
            algorithmOptimalValueChanges.Add(aov);
        }
        //--------------------------------------------------------------------------------------
#endif
    }
    //--------------------------------------------------------------------------------------
}
