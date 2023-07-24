using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using DiaMakerModel.Tracing;

namespace DiaMakerViewModel
{
    //----------------------------------------------------------------------------------------------------------------------
    // class ConnectLineInfoViewModel
    //----------------------------------------------------------------------------------------------------------------------
    public class ConnectLineInfoViewModel : DiaElementViewModel
    {
        private ConnectingLine connectingLine;
        public static double MultiplicationElementHeightDiaPanel = 1;
        public static double MultiplicationElementWidthDiaPanel = 1;
        //----------------------------------------------------------------------------------------------------------------------
        public ConnectLineInfoViewModel(ConnectingLine pConnectingLine)
        {
            connectingLine = pConnectingLine;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public PointCollection Points 
        {
            get
            {
                PointCollection points = new PointCollection();
                for (int i = 0; i < connectingLine.Points.Count; i++)
                    points.Add(new Point(connectingLine.Points[i].X * MultiplicationElementWidthDiaPanel ,
                        connectingLine.Points[i].Y * MultiplicationElementHeightDiaPanel));
                return points;
            }
        }
        //--------------------------------------------------------------------------------------
        public override string ToString()
        {
            return connectingLine.ToString();
        }
        //----------------------------------------------------------------------------------------------------------------------
        public override void NotifyUI()
        {
            OnPropertyChanged("Points");
        }
        //----------------------------------------------------------------------------------------------------------------------
    }
    //----------------------------------------------------------------------------------------------------------------------
}
