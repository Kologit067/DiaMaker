using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DiaMakerModel.Tracing
{
    //--------------------------------------------------------------------------------------
    // class Strip
    //--------------------------------------------------------------------------------------
    public abstract class Strip

    {
        protected Tracing owner;
        protected List<ConnectingLine> lines = new List<ConnectingLine>();
//        protected ConnectingLine[] lineOrder;
        protected Dictionary<ConnectingLine, double> linePosition = new Dictionary<ConnectingLine, double>();
        protected Section[] sections;
        //--------------------------------------------------------------------------------------
        public Strip(Tracing pOwner,Section[] pSections)
        {
            owner = pOwner;
            sections = pSections;
        }
        //--------------------------------------------------------------------------------------
        public abstract void DefineLineOrder(int[,] pConnectingLineRelation);
        //--------------------------------------------------------------------------------------
        public void DefineLinePosition()
        {
            double lIndent = GetStripIndent();
            double lWidth = GetStripWidth();
            double lDelta = (lWidth - 2 * lIndent) / (lines.Count + 1);
            double lCurrentPosition = StartPosition + lDelta;
            for (int i = 0; i < lines.Count; i++)
            {
                linePosition.Add(lines[i], lCurrentPosition);
                lCurrentPosition += lDelta;
            }
            //double lDelta = lineOrder.Length == 1 ? 0 : (lWidth - 2 * lIndent) / (lineOrder.Length - 1);
            //double lCurrentPosition = lineOrder.Length == 1 ? lWidth / 2 : lIndent;
            //for (int i = 0; i < lineOrder.Length; i++)
            //{
            //    linePosition.Add(lineOrder[i], lCurrentPosition);
            //    lCurrentPosition += lDelta;
            //}
        }
        //--------------------------------------------------------------------------------------
        protected abstract double GetStripWidth();
        //--------------------------------------------------------------------------------------
        protected abstract double GetStripIndent();
        //--------------------------------------------------------------------------------------
        public abstract Point GetNextPoint(ConnectingLine pLine, Point pPoint);
        //--------------------------------------------------------------------------------------
        protected abstract double StartPosition
        {
            get;
        }
        //--------------------------------------------------------------------------------------
        public void AddLine(ConnectingLine pLine)
        {
            lines.Add(pLine);
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
}
