using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Interfaces;

namespace DataBase.Classes
{
    //-------------------------------------------------------------------------------------------------------
    // class CVertex
    //-------------------------------------------------------------------------------------------------------
    public class CVertex : IVertex
    {
        private string _fName;
        private List<int> _fEndPoints = new List<int>();
        //-------------------------------------------------------------------------------------------------------
        public string Name 
        {
            get
            {
                return _fName;
            }
        }
        //-------------------------------------------------------------------------------------------------------
        public List<int> EndPoints
        {
            get
            {
                return _fEndPoints;
            }
        }
        //-------------------------------------------------------------------------------------------------------
        public CVertex(string pName)
        {
            _fName = pName;
        }
        //-------------------------------------------------------------------------------------------------------
        public override string ToString()
        {
            return Name + string.Join(",", EndPoints);
        }
        //-------------------------------------------------------------------------------------------------------
    }
    //-------------------------------------------------------------------------------------------------------
}
