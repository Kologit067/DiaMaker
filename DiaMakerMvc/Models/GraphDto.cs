using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiaMakerMvc.Models
{
    public class GraphDto
    {
        public List<RectangleInfoDto> Rectangles {get; set;}
        public List<ConnectLineInfoDto> ConnectLines { get; set; }
        public List<ArrowInfoDto> Arrows { get; set; }
    }
}