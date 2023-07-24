using DiaMakerModel.Tracing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Windows.Media;
namespace DiaMakerMvc.Models
{
    public class ConnectLineInfoDto
    {
        public static double MultiplicationElementHeightDiaPanel = 1;
        public static double MultiplicationElementWidthDiaPanel = 1;
        //----------------------------------------------------------------------------------------------------------------------
        public ConnectLineInfoDto(ConnectingLine pConnectingLine)
        {
            Points = new List<PointDto>();
            for (int i = 0; i < pConnectingLine.Points.Count; i++)
                Points.Add(new PointDto() { X = pConnectingLine.Points[i].X, Y = pConnectingLine.Points[i].Y });
        }
        //----------------------------------------------------------------------------------------------------------------------
        public List<PointDto> Points { get; set; }
        //----------------------------------------------------------------------------------------------------------------------
    }

    public class PointDto
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
}