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
    // class VerteciesInTableVertex
    //--------------------------------------------------------------------------------------
    public class VerteciesInTableVertex : VerteciesInTableDefine
    {
        //--------------------------------------------------------------------------------------
        public VerteciesInTableVertex(IGraph<IVertex> pGraph, IMatrixOfDistance pMatrixofDistance, string pAlgorithmName, int pMaxDurability)
            : base(pGraph, pMatrixofDistance, pMatrixofDistance.Length, pGraph.Vertices.Count, pAlgorithmName, pMaxDurability)
        {
            algorithmConfiguration.BaseEnumeration = BaseEnumerationEnum.ByVertex;
        }
        //--------------------------------------------------------------------------------------
        public VerteciesInTableVertex(IGraph<IVertex> pGraph, IMatrixOfDistance pMatrixofDistance, AlgorithmConfiguration pAlgorithmConfiguration, int pMaxDurability)
            : base(pGraph, pMatrixofDistance, pMatrixofDistance.Length, pGraph.Vertices.Count, pAlgorithmConfiguration, pMaxDurability)
        {
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
}
