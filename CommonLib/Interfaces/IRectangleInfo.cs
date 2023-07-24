using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.CommonLib.Interfaces
{
    //-------------------------------------------------------------------------------------------------------
    // Interface IRectangleInfo
    //-------------------------------------------------------------------------------------------------------
    public interface IRectangleInfo : IDiaElement
    {
        string Name { get;  }
        double Left { get;  }
        double Top { get;  }
        double Height { get;  }
        double Width { get;  }
    }
    //-------------------------------------------------------------------------------------------------------
}
