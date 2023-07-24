using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public interface ITableInfo
    {
        string Name { get; set; }
        bool IsSelected { get; set; }

    }
}
