using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Interfaces;

namespace DataBase.Classes
{
    //-------------------------------------------------------------------------------------------------------
    // class CGraph
    //-------------------------------------------------------------------------------------------------------
    public class Graph<T> : IGraph<T> where T : IVertex
    {
        private List<T> _fVertices = new List<T>();
        //-------------------------------------------------------------------------------------------------------
        public List<T> Vertices 
        {
            get
            {
                return _fVertices;
            }
        }
        //-------------------------------------------------------------------------------------------------------
    }
    //-------------------------------------------------------------------------------------------------------
}
