using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Common;
using System.Windows.Media;

namespace DiaMakerModel.Tracing
{
    //----------------------------------------------------------------------------------------------------------------------
    // class ArrowData
    //----------------------------------------------------------------------------------------------------------------------
    public class ArrowData
    {
        private const double ArrowWidth = 4;
        private const double ArrowLength = 7;
        private PointCollection points = new PointCollection();
        private TypeOfTerminalEnum typeOfTerminal;
        private Point tip;
        //-------------------------------------------------------------------------------------
        public ArrowData( TypeOfTerminalEnum pTypeOfTerminal, Point pTip)
        {
            typeOfTerminal = pTypeOfTerminal;
            tip = pTip;
            switch (typeOfTerminal)
            {
                case TypeOfTerminalEnum.Bottom:
                    points = new PointCollection() { pTip, new Point() { X = pTip.X - ArrowWidth / 2, Y = pTip.Y + ArrowLength }, new Point() { X = pTip.X + ArrowWidth/2, Y = pTip.Y + ArrowLength }, new Point() { X = pTip.X, Y = pTip.Y } };
                    break;
                case TypeOfTerminalEnum.Top:
                    points = new PointCollection() { pTip, new Point() { X = pTip.X - ArrowWidth / 2, Y = pTip.Y - ArrowLength }, new Point() { X = pTip.X + ArrowWidth/2, Y = pTip.Y - ArrowLength }, new Point() { X = pTip.X, Y = pTip.Y } };
                    break;
                case TypeOfTerminalEnum.Left:
                    points = new PointCollection() { pTip, new Point() { X = pTip.X - ArrowLength, Y = pTip.Y - ArrowWidth / 2 }, new Point() { X = pTip.X - ArrowLength, Y = pTip.Y + ArrowWidth / 2 }, new Point() { X = pTip.X, Y = pTip.Y } };
                    break;
                case TypeOfTerminalEnum.Right:
                    points = new PointCollection() { pTip, new Point() { X = pTip.X + ArrowLength, Y = pTip.Y - ArrowWidth / 2 }, new Point() { X = pTip.X + ArrowLength, Y = pTip.Y + ArrowWidth / 2 }, new Point() { X = pTip.X, Y = pTip.Y } };
                    break;
            }
        }
        //-------------------------------------------------------------------------------------
        public PointCollection Points
        {
            get
            {
                return points;
            }
        }
        //--------------------------------------------------------------------------------------
        public TypeOfTerminalEnum TypeOfTerminal
        {
            get
            {
                return typeOfTerminal;
            }
        }
        //--------------------------------------------------------------------------------------
        public Point Tip
        {
            get
            {
                return tip;
            }
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
}
