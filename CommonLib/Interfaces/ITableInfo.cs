using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.CommonLib.Interfaces
{
    //-------------------------------------------------------------------------------------------------------
    // Interface ITableInfo
    //-------------------------------------------------------------------------------------------------------
    public interface ITableInfo
    {
        string Name { get; set; }
        bool IsSelected { get; set; }

        //-------------------------------------------------------------------------------------------------------
    }
    //-------------------------------------------------------------------------------------------------------
}
