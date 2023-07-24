using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public interface IRectangleInfo : IDiaElement
    {
        string Name { get;  }
        double Left { get;  }
        double Top { get;  }
        double Height { get;  }
        double Width { get;  }

    }
}
