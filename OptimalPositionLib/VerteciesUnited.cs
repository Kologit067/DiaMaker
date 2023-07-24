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
        protected VerteciesInTableDefine enumerateAlgorithm;
        protected VerteciesInTableDefine approximateAlgorithm;
        //--------------------------------------------------------------------------------------
        public VerteciesUnited(IGraph<IVertex> pGraph, IMatrixOfDistance pMatrixofDistance, string pAlgorithmName, int pMaxDurability)
            : base( pAlgorithmName)
        {
            enumerateAlgorithm = new VerteciesInTablePlaceEnumerateAdv(pGraph, pMatrixofDistance, pAlgorithmName, pMaxDurability);
            approximateAlgorithm = new VerticesInTableApproximate(pGraph, pMatrixofDistance, pAlgorithmName, pMaxDurability);
        }
        //--------------------------------------------------------------------------------------
        public override void ExecuteBody()
        {
           
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            approximateAlgorithm.ExecuteBody();
            enumerateAlgorithm.ExecuteBody();

            SetData();

            stopwatch.Stop();
            fElapsedTicks = stopwatch.ElapsedTicks;
            fDurationMilliSeconds = stopwatch.ElapsedMilliseconds;

        }
        //--------------------------------------------------------------------------------------
        protected void SetData()
        {
            fGraph = enumerateAlgorithm.Graph;
            if (enumerateAlgorithm.OptimalWeight <= approximateAlgorithm.OptimalWeight)
            {
                fOptimalRoute = enumerateAlgorithm.OptimalRoute;
                optimalStorage = enumerateAlgorithm.OptimalStorage;
                changeOptimalNumber = enumerateAlgorithm.ChangeOptimalNumber;
                fOptimalWeight = enumerateAlgorithm.OptimalWeight;
                lowEstimate = enumerateAlgorithm.LowEstimate;
            }
            else
            {
                fOptimalRoute = approximateAlgorithm.OptimalRoute;
                optimalStorage = approximateAlgorithm.OptimalStorage;
                changeOptimalNumber = approximateAlgorithm.ChangeOptimalNumber;
                fOptimalWeight = approximateAlgorithm.OptimalWeight;
                lowEstimate = approximateAlgorithm.LowEstimate;
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
        public VerteciesUnitedParallel(IGraph<IVertex> pGraph, IMatrixOfDistance pMatrixofDistance, string pAlgorithmName, int pMaxDurability)
            : base( pGraph, pMatrixofDistance, pAlgorithmName, pMaxDurability)
        {
        }
        //--------------------------------------------------------------------------------------
        public override void ExecuteBody()
        {
           
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Parallel.Invoke(
            () => approximateAlgorithm.ExecuteBody(),
            () => enumerateAlgorithm.ExecuteBody()
            );

            SetData();

            stopwatch.Stop();
            fElapsedTicks = stopwatch.ElapsedTicks;
            fDurationMilliSeconds = stopwatch.ElapsedMilliseconds;

       
        }
    }
}
