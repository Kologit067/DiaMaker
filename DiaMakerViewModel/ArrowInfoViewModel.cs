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
    // class ArrowInfoViewModel
    //----------------------------------------------------------------------------------------------------------------------
    public class ArrowInfoViewModel : DiaElementViewModel
    {
        private ArrowData arrowData;
        public static double MultiplicationElementHeightDiaPanel = 1;
        public static double MultiplicationElementWidthDiaPanel = 1;
        //----------------------------------------------------------------------------------------------------------------------
        public ArrowInfoViewModel(ArrowData pArrowData)
        {
            arrowData = pArrowData;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public PointCollection Points
        {
            get
            {
                //return arrowData.Points;
                PointCollection points = new PointCollection();
                for (int i = 0; i < arrowData.Points.Count; i++)
                    points.Add(new Point(arrowData.Points[i].X * MultiplicationElementWidthDiaPanel,
                        arrowData.Points[i].Y * MultiplicationElementHeightDiaPanel));
                return points;

            }
        }
        //--------------------------------------------------------------------------------------
        public override string ToString()
        {
            return arrowData.ToString();
        }
        //----------------------------------------------------------------------------------------------------------------------
        public override void NotifyUI()
        {
            OnPropertyChanged("Points");
        }
        //----------------------------------------------------------------------------------------------------------------------
    }
    //----------------------------------------------------------------------------------------------------------------------}
}