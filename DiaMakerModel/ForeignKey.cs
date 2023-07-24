using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiaMakerModel
{
    //----------------------------------------------------------------------------------------------------------------------
    // class ForeignKey
    //---------------------------------------------------------------------------------------------------------------------
    public class ForeignKey
    {
        public string Name { get; set; }
        public Table TableFrom { get; set; }
        public Table TableTo { get; set; }
        public string KeyFrom { get; set; }
        public string KeyTo { get; set; }
    }
    //---------------------------------------------------------------------------------------------------------------------
}
