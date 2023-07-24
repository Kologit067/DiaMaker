using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DiaMakerModel.Tracing
{
    //--------------------------------------------------------------------------------------
    // class HorisontalStrip
    //--------------------------------------------------------------------------------------
    public class HorisontalStrip : Strip
    {
        private double positionY;
        //--------------------------------------------------------------------------------------
        public HorisontalStrip(Tracing pOwner,Section[] pSections, double pPositionY) : base(pOwner,pSections)
        {
            positionY = pPositionY;
        }
        //--------------------------------------------------------------------------------------
        public override Point GetNextPoint(ConnectingLine pLine, Point pPoint)
        {
            double lVerticalPosition = linePosition[pLine];
            return new Point() { X = pPoint.X, Y = lVerticalPosition };
        }
        //--------------------------------------------------------------------------------------
        protected override double GetStripWidth()
        {
            return owner.HorisontalStripWidth;
        }
        //--------------------------------------------------------------------------------------
        protected override double GetStripIndent()
        {
            return owner.HorisontalStripIndent;
        }
        //--------------------------------------------------------------------------------------
        protected override double StartPosition
        {
            get
            {
                return positionY;
            }
        }
        //--------------------------------------------------------------------------------------
        public override void DefineLineOrder(int[,] pConnectingLineRelation)
        {

            HorisontalLineComparer lHorisontalLineComparer = new HorisontalLineComparer(pConnectingLineRelation);
            lines.Sort(lHorisontalLineComparer);

        }
        //--------------------------------------------------------------------------------------
        private class HorisontalLineComparer : IComparer<ConnectingLine>
        {
            private int[,] connectingLineRelation;
            //--------------------------------------------------------------------------------------
            public HorisontalLineComparer(int[,] pConnectingLineRelation)
            {
                connectingLineRelation = pConnectingLineRelation;
            }
            //--------------------------------------------------------------------------------------
            public int Compare(ConnectingLine pFirstLine, ConnectingLine pSecondLine)
            {
                if (pFirstLine.TypeOfLine == TypeOfLineEnum.HorisontalTop && pSecondLine.TypeOfLine == TypeOfLineEnum.HorisontalTop)
                {
                    if (pFirstLine.HorisontalX1 >= pSecondLine.HorisontalX1 && pFirstLine.HorisontalX2 < pSecondLine.HorisontalX2)
                        return 1;
                    else if (pFirstLine.HorisontalX1 > pSecondLine.HorisontalX1 && pFirstLine.HorisontalX2 <= pSecondLine.HorisontalX2)
                        return 1;
                    else if (pFirstLine.HorisontalX1 <= pSecondLine.HorisontalX1 && pFirstLine.HorisontalX2 > pSecondLine.HorisontalX2)
                        return -1;
                    else if (pFirstLine.HorisontalX1 < pSecondLine.HorisontalX1 && pFirstLine.HorisontalX2 >= pSecondLine.HorisontalX2)
                        return -1;
                    else if (pFirstLine.HorisontalX1 < pSecondLine.HorisontalX1)
                        return 1;
                    else
                        return -1;
                }
                else if (pFirstLine.TypeOfLine == TypeOfLineEnum.HorisontalTop)
                {
                    return 1;
                }
                else if (pSecondLine.TypeOfLine == TypeOfLineEnum.HorisontalTop)
                {
                    return -1;
                }
                else if (pFirstLine.TypeOfLine == TypeOfLineEnum.HorisontalBottom && pSecondLine.TypeOfLine == TypeOfLineEnum.HorisontalBottom)
                {
                    if (pFirstLine.HorisontalX1 >= pSecondLine.HorisontalX1 && pFirstLine.HorisontalX2 < pSecondLine.HorisontalX2)
                        return -1;
                    else if (pFirstLine.HorisontalX1 > pSecondLine.HorisontalX1 && pFirstLine.HorisontalX2 <= pSecondLine.HorisontalX2)
                        return -1;
                    else if (pFirstLine.HorisontalX1 <= pSecondLine.HorisontalX1 && pFirstLine.HorisontalX2 > pSecondLine.HorisontalX2)
                        return 1;
                    else if (pFirstLine.HorisontalX1 < pSecondLine.HorisontalX1 && pFirstLine.HorisontalX2 >= pSecondLine.HorisontalX2)
                        return 1;
                    else if (pFirstLine.HorisontalX1 < pSecondLine.HorisontalX1)
                        return -1;
                    else
                        return 1;
                }
                else if (pFirstLine.TypeOfLine == TypeOfLineEnum.HorisontalBottom)
                {
                    return -1;
                }
                else if (pSecondLine.TypeOfLine == TypeOfLineEnum.HorisontalBottom)
                {
                    return 1;
                }
                else if (pFirstLine.TypeOfLine == TypeOfLineEnum.DiagonalTopLeft)
                {
                    if (pSecondLine.TypeOfLine == TypeOfLineEnum.DiagonalTopLeft)
                    {
                        int r = connectingLineRelation[pFirstLine.Index, pSecondLine.Index];
                        return r;
                    }
                    else if (pSecondLine.TypeOfLine == TypeOfLineEnum.HorisontalTop)
                        return 1;
                    else
                        return -1;
                }
                else if (pFirstLine.TypeOfLine == TypeOfLineEnum.DiagonalTopRight)
                {
                    if (pSecondLine.TypeOfLine == TypeOfLineEnum.DiagonalTopRight)
                    {
                        int r = connectingLineRelation[pFirstLine.Index, pSecondLine.Index];
                        return r;
                    }
                    else if (pSecondLine.TypeOfLine == TypeOfLineEnum.HorisontalTop || (pSecondLine.TypeOfLine == TypeOfLineEnum.DiagonalTopLeft))
                        return 1;
                    else
                        return -1;
                }
                else if (pFirstLine.TypeOfLine == TypeOfLineEnum.HorisontalBottom)
                {
                    if (pSecondLine.TypeOfLine == TypeOfLineEnum.HorisontalBottom)
                    {
                        if (pFirstLine.HorisontalX1 >= pSecondLine.HorisontalX1 && pFirstLine.HorisontalX2 < pSecondLine.HorisontalX2)
                            return 1;
                        else if (pFirstLine.HorisontalX1 > pSecondLine.HorisontalX1 && pFirstLine.HorisontalX2 <= pSecondLine.HorisontalX2)
                            return 1;
                        else if (pFirstLine.HorisontalX1 <= pSecondLine.HorisontalX1 && pFirstLine.HorisontalX2 > pSecondLine.HorisontalX2)
                            return -1;
                        else if (pFirstLine.HorisontalX1 < pSecondLine.HorisontalX1 && pFirstLine.HorisontalX2 >= pSecondLine.HorisontalX2)
                            return -1;
                        else if (pFirstLine.HorisontalX1 < pSecondLine.HorisontalX1)
                            return -1;
                        else
                            return 1;
                    }
                    else
                        return -1;
                }
                throw new Exception("Logical error in HorisontalLineComparer");
            }
            //-------------------------------------------------------------------------------------
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
}
