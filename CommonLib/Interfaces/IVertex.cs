using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.CommonLib.Interfaces
{
    //-------------------------------------------------------------------------------------------------------
    // Interface IVertex
    //-------------------------------------------------------------------------------------------------------
    public interface IVertex
    {
        string Name { get;}
        List<int> EndPoints
        {
            get;
        }
        List<int> StartPoints
        {
            get;
        }
        int Weight
        {
            get;
        }
        //-------------------------------------------------------------------------------------------------------
        IEnumerable<int> AdjacentVertices
        {
            get;
        }
        //-------------------------------------------------------------------------------------------------------
        bool ContaintVertex(int pCurrentVertex);
        //-------------------------------------------------------------------------------------------------------
    }
    //-------------------------------------------------------------------------------------------------------
}
