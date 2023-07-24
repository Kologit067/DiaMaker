using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Interfaces
{
    public interface IScrollIntoViewAction
    {
        event Action<object> MainGridScrollIntoView;
        event Action DataGridCommitEdit;
    }
}
