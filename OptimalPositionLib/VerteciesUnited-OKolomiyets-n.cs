using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common.CommonLib.Interfaces;
using CommonLib;

namespace OptimalPositionLib
{
    
    //--------------------------------------------------------------------------------------
    // class VerteciesUnited
    //--------------------------------------------------------------------------------------
    public class VerteciesUnited : VerteciesInTableDefine
    {
        protected VerteciesInTableDefine enumerateAlgorythm;
        protected VerteciesInTableDefine approximateAlgorythm;
        //--------------------------------------------------------------------------------------
        public VerteciesUnited(IGraph<IVertex> pGraph, IMatrixOfDistance pMatrixofDistance, string pAlgorythmName, int pMaxDurability)
            : base( pAlgorythmName)
        {
            enumerateAlgorythm = new VerteciesInTablePlaceEnumerateAdv(pGraph, pMatrixofDistance, pAlgorythmName, pMaxDurability);
            approximateAlgorythm = new VerticesInTableApproximate(pGraph, pMatrixofDistance, pAlgorythmName, pMaxDurability);
        }
        //--------------------------------------------------------------------------------------
        public override void Execute()
        {
           
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            approximateAlgorythm.Execute();
            enumerateAlgorythm.Execute();

            SetData();

            stopwatch.Stop();
            fElapsedTicks = stopwatch.ElapsedTicks;
            fDurationMilliSeconds = stopwatch.ElapsedMilliseconds;

        }
        //--------------------------------------------------------------------------------------
        protected void SetData()
        {
            fGraph = enumerateAlgorythm.Graph;
            if (enumerateAlgorythm.OptimalWeight <= approximateAlgorythm.OptimalWeight)
            {
                fOptimalRoute = enumerateAlgorythm.OptimalRoute;
                optimalStorage = enumerateAlgorythm.OptimalStorage;
                changeOptimalNumber = enumerateAlgorythm.ChangeOptimalNumber;
                fOptimalWeight = enumerateAlgorythm.OptimalWeight;
                lowEstimate = enumerateAlgorythm.LowEstimate;
            }
            else
            {
                fOptimalRoute = approximateAlgorythm.OptimalRoute;
                optimalStorage = approximateAlgorythm.OptimalStorage;
                changeOptimalNumber = approximateAlgorythm.ChangeOptimalNumber;
                fOptimalWeight = approximateAlgorythm.OptimalWeight;
                lowEstimate = approximateAlgorythm.LowEstimate;
            }
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
    // class VerteciesUnitedParallel
    //--------------------------------------------------------------------------------------
    public class VerteciesUnitedParallel : VerteciesUnited
    {
        //--------------------------------------------------------------------------------------
        public VerteciesUnitedParallel(IGraph<IVertex> pGraph, IMatrixOfDistance pMatrixofDistance, string pAlgorythmName, int pMaxDurability)
            : base( pGraph, pMatrixofDistance, pAlgorythmName, pMaxDurability)
        {
        }
        //--------------------------------------------------------------------------------------
        public override void Execute()
        {
           
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Parallel.Invoke(
            () => approximateAlgorythm.Execute(),
            () => enumerateAlgorythm.Execute()
            );

            SetData();

            stopwatch.Stop();
            fElapsedTicks = stopwatch.ElapsedTicks;
            fDurationMilliSeconds = stopwatch.ElapsedMilliseconds;

       
        }
    }
}
