using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.CommonLib.Interfaces;
using OptimalPositionLib.Matrix;
using CommonLib;
using CommonLib.Tools;

namespace OptimalPositionLib
{
    //--------------------------------------------------------------------------------------
    // class VerteciesInTablePlace
    //--------------------------------------------------------------------------------------
    public class VerteciesInTablePlace : VerteciesInTableDefine
    {
        //--------------------------------------------------------------------------------------
        public VerteciesInTablePlace(IGraph<IVertex> pGraph, IMatrixOfDistance pMatrixofDistance, string pAlgorithmName, int pMaxDurability)
            : base(pGraph, pMatrixofDistance, pGraph.Vertices.Count, pMatrixofDistance.Length, pAlgorithmName, pMaxDurability)
        {
            algorithmConfiguration.BaseEnumeration = BaseEnumerationEnum.ByPlace;
        }
        //--------------------------------------------------------------------------------------
        public VerteciesInTablePlace(IGraph<IVertex> pGraph, IMatrixOfDistance pMatrixofDistance, AlgorithmConfiguration pAlgorithmConfiguration, int pMaxDurability)
            : base(pGraph, pMatrixofDistance, pGraph.Vertices.Count, pMatrixofDistance.Length, pAlgorithmConfiguration, pMaxDurability)
        {
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
}
