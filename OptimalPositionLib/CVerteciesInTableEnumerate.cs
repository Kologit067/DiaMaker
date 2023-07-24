using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.CommonLib.Interfaces;
using OptimalPositionLib.Matrix;

namespace OptimalPositionLib
{
    //--------------------------------------------------------------------------------------
    // class CVerteciesInTableEnumerate
    //--------------------------------------------------------------------------------------
    public class CVerteciesInTableEnumerate : VerteciesInTablePlace 
    {
//        private List<string> fResultStringSet = new List<string>();
//        protected int[] fOptimalRoute = null;				// маршрут обхода
        ////--------------------------------------------------------------------------------------
        //public int[] OptimalRoute
        //{
        //    get
        //    {
        //        return fOptimalRoute;
        //    }
        //}
        //--------------------------------------------------------------------------------------
        public CVerteciesInTableEnumerate(IGraph<IVertex> pGraph, IMatrixOfDistance pMatrixofDistance, string pAlgorythmName)
            : base(pGraph, pMatrixofDistance, pAlgorythmName)
        {
            //fGraph = pGraph;
            //fMatrixofDistance = pMatrixofDistance;
            //fOptimalRoute = new int[fNumberOfPlace];
            fOptimalWeight = int.MaxValue;
        }
        //--------------------------------------------------------------------------------------
        public override bool IsEliminable()
        {
            fIterationCount++;
            if (fCurrentPosition == fNumberOfPlace)
            {
                int lWeightOfRoute = ComputeWeight(fRoute);
                if (lWeightOfRoute < fOptimalWeight)
                {
                    for (int i = 0; i < fRoute.Length; i++ )
                        fOptimalRoute[i] = fRoute[i];
                    fOptimalWeight = lWeightOfRoute;
                    if (optimalStorage.Count >= 10)
                        optimalStorage.Dequeue();
                    optimalStorage.Enqueue(Tuple.Create(fOptimalRoute.ToArray(), fOptimalWeight));
                    changeOptimalNumber++;
                    ChangeOptimalWeightAction();
                }
//                fResultStringSet.Add(RouteAsString);
                return true;
            }
            else
            {
                return false;
            }
        }
        //--------------------------------------------------------------------------------------
        public string OptimalRouteAsString
        {
            get
            {
                if (fOptimalRoute != null && fOptimalRoute.Length > 0)
                    return string.Join(",", fOptimalRoute.Select(i => i.ToString()));
                return "Empty";
            }
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
    // class CVerteciesInTableEnumerateWithLowEstimate
    //--------------------------------------------------------------------------------------
    public class CVerteciesInTableEnumerateWithLowEstimate : CVerteciesInTableEnumerate
    {
        //--------------------------------------------------------------------------------------
        public CVerteciesInTableEnumerateWithLowEstimate(IGraph<IVertex> pGraph, IMatrixOfDistance pMatrixofDistance, string pAlgorythmName)
            : base(pGraph, pMatrixofDistance, pAlgorythmName)
        {
        }
        //--------------------------------------------------------------------------------------
        protected override void ChangeOptimalWeightAction()
        {
            if (LowEstimate >= fOptimalWeight)
                fIsStop = true;
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------
}
