using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Common.CommonLib.Interfaces
{
    //-------------------------------------------------------------------------------------------------------
    // Interface IDiaElement
    //-------------------------------------------------------------------------------------------------------
    public interface IConnectLineInfo : IDiaElement
    {
        PointCollection Points { get; }
    }
    //-------------------------------------------------------------------------------------------------------
}
