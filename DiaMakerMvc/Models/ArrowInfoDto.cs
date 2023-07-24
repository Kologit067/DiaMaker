using DiaMakerModel.Tracing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiaMakerMvc.Models
{
    public class ArrowInfoDto
    {
        //----------------------------------------------------------------------------------------------------------------------
        public ArrowInfoDto(ArrowData pArrowData)
        {
                Points = new List<PointDto>();
                for (int i = 0; i < pArrowData.Points.Count; i++)
                    Points.Add(new PointDto() { X = pArrowData.Points[i].X, Y = pArrowData.Points[i].Y });
        }
        //----------------------------------------------------------------------------------------------------------------------
        public List<PointDto> Points{ get; set;}
        //----------------------------------------------------------------------------------------------------------------------
    }
}