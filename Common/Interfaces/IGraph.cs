using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    //-------------------------------------------------------------------------------------------------------
    // class IGraph
    //-------------------------------------------------------------------------------------------------------
    public interface IGraph<T> where T : IVertex
    {
        List<T> Vertices { get; }
    }
    //-------------------------------------------------------------------------------------------------------
}
