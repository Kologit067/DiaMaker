using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DiaMakerModel.Tracing
{
    //--------------------------------------------------------------------------------------
    // class VerticalStrip
    //--------------------------------------------------------------------------------------
    public class VerticalStrip : Strip
    {
        private double positionX;
        //--------------------------------------------------------------------------------------
        public VerticalStrip(Tracing pOwner, Section[] pSections, double pPositionX)
            : base(pOwner,pSections)
        {
            positionX = pPositionX;
        }
        //--------------------------------------------------------------------------------------
        public override Point GetNextPoint(ConnectingLine pLine, Point pPoint)
        {
            double lHorisontalPosition = linePosition[pLine];
            return new Point() { X = lHorisontalPosition, Y = pPoint.Y };
        }
        //--------------------------------------------------------------------------------------
        protected override double GetStripWidth()
        {
            return owner.VerticalStripWidth;
        }
        //--------------------------------------------------------------------------------------
        protected override double GetStripIndent()
        {
            return owner.VerticalStripIndent;
        }
        //--------------------------------------------------------------------------------------
        protected override double StartPosition
        {
            get
            {
                return positionX;
            }
        }
        //--------------------------------------------------------------------------------------
        public override void DefineLineOrder(int[,] pConnectingLineRelation)
        {

            VerticalLineComparer lVerticalLineComparer = new VerticalLineComparer(pConnectingLineRelation);
            lines.Sort(lVerticalLineComparer);

        }
        //--------------------------------------------------------------------------------------
        private class VerticalLineComparer : IComparer<ConnectingLine>
        {
            private int[,] connectingLineRelation;
            //--------------------------------------------------------------------------------------
            public VerticalLineComparer(int[,] pConnectingLineRelation)
            {
                connectingLineRelation = pConnectingLineRelation;
            }
            //--------------------------------------------------------------------------------------
            public int Compare(ConnectingLine pFirstLine, ConnectingLine pSecondLine)
            {
                if (pFirstLine.TypeOfLine == TypeOfLineEnum.VerticalLeft && pSecondLine.TypeOfLine == TypeOfLineEnum.VerticalLeft)
                {
                    if (pFirstLine.VerticalY1 >= pSecondLine.VerticalY1 && pFirstLine.VerticalY2 < pSecondLine.VerticalY2)
                        return 1;
                    else if (pFirstLine.VerticalY1 > pSecondLine.VerticalY1 && pFirstLine.VerticalY2 <= pSecondLine.VerticalY2)
                        return 1;
                    else if (pFirstLine.VerticalY1 <= pSecondLine.VerticalY1 && pFirstLine.VerticalY2 > pSecondLine.VerticalY2)
                        return -1;
                    else if (pFirstLine.VerticalY1 < pSecondLine.VerticalY1 && pFirstLine.VerticalY2 >= pSecondLine.VerticalY2)
                        return -1;
                    else if (pFirstLine.VerticalY1 < pSecondLine.VerticalY1)
                        return 1;
                    else
                        return -1;
                }
                else if (pFirstLine.TypeOfLine == TypeOfLineEnum.VerticalLeft)
                {
                    return 1;
                }
                else if (pSecondLine.TypeOfLine == TypeOfLineEnum.VerticalLeft)
                {
                    return -1;
                }
                else if (pFirstLine.TypeOfLine == TypeOfLineEnum.VerticalRight && pSecondLine.TypeOfLine == TypeOfLineEnum.VerticalRight)
                {
                    if (pFirstLine.VerticalY1 >= pSecondLine.VerticalY1 && pFirstLine.VerticalY2 < pSecondLine.VerticalY2)
                        return -1;
                    else if (pFirstLine.VerticalY1 > pSecondLine.VerticalY1 && pFirstLine.VerticalY2 <= pSecondLine.VerticalY2)
                        return -1;
                    else if (pFirstLine.VerticalY1 <= pSecondLine.VerticalY1 && pFirstLine.VerticalY2 > pSecondLine.VerticalY2)
                        return 1;
                    else if (pFirstLine.VerticalY1 < pSecondLine.VerticalY1 && pFirstLine.VerticalY2 >= pSecondLine.VerticalY2)
                        return 1;
                    else if (pFirstLine.VerticalY1 < pSecondLine.VerticalY1)
                        return -1;
                    else
                        return 1;
                }
                else if (pFirstLine.TypeOfLine == TypeOfLineEnum.VerticalRight)
                {
                    return -1;
                }
                else if (pSecondLine.TypeOfLine == TypeOfLineEnum.VerticalRight)
                {
                    return 1;
                }
                else if (pFirstLine.TypeOfLine == TypeOfLineEnum.DiagonalTopRight)
                {
                    if (pSecondLine.TypeOfLine == TypeOfLineEnum.DiagonalTopRight)
                    {
                        int r = connectingLineRelation[pFirstLine.Index, pSecondLine.Index];
                        return r;
                    }
                    else if (pSecondLine.TypeOfLine == TypeOfLineEnum.VerticalLeft)
                        return 1;
                    else
                        return -1;
                }
                else if (pFirstLine.TypeOfLine == TypeOfLineEnum.DiagonalTopLeft)
                {
                    if (pSecondLine.TypeOfLine == TypeOfLineEnum.DiagonalTopLeft)
                    {
                        int r = (-1) * connectingLineRelation[pFirstLine.Index, pSecondLine.Index];
                        return r;
                    }
                    else if (pSecondLine.TypeOfLine == TypeOfLineEnum.VerticalLeft || (pSecondLine.TypeOfLine == TypeOfLineEnum.DiagonalTopRight))
                        return 1;
                    else
                        return -1;
                }
                else if (pFirstLine.TypeOfLine == TypeOfLineEnum.VerticalRight)
                {
                    if (pSecondLine.TypeOfLine == TypeOfLineEnum.VerticalRight)
                    {
                        if (pFirstLine.VerticalY1 >= pSecondLine.VerticalY1 && pFirstLine.VerticalY2 < pSecondLine.VerticalY2)
                            return 1;
                        else if (pFirstLine.VerticalY1 > pSecondLine.VerticalY1 && pFirstLine.VerticalY2 <= pSecondLine.VerticalY2)
                            return 1;
                        else if (pFirstLine.VerticalY1 <= pSecondLine.VerticalY1 && pFirstLine.VerticalY2 > pSecondLine.VerticalY2)
                            return -1;
                        else if (pFirstLine.VerticalY1 < pSecondLine.VerticalY1 && pFirstLine.VerticalY2 >= pSecondLine.VerticalY2)
                            return -1;
                        else if (pFirstLine.VerticalY1 < pSecondLine.VerticalY1)
                            return -1;
                        else
                            return 1;
                    }
                    else
                        return -1;
                }
                throw new Exception("Logical error in VerticalLineComparer");
            }
            //--------------------------------------------------------------------------------------
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
}
