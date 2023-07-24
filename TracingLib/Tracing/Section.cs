using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.CommonLib.Interfaces;
using Common;
using System.Windows;

namespace DiaMakerModel.Tracing
{
    //--------------------------------------------------------------------------------------
    // class Section
    //--------------------------------------------------------------------------------------
    public abstract class Section
    {
        protected double positionX;
        protected double positionY;
        protected PresentationData presentationData;
        //--------------------------------------------------------------------------------------
        public Section(double pPositionX, double pPositionY, PresentationData pPresentationData)
        {
            positionX = pPositionX;
            positionY = pPositionY;
            presentationData = pPresentationData;
        }
        //--------------------------------------------------------------------------------------
        public double PositionX
        {
            get
            {
                return positionX;
            }
        }
        //--------------------------------------------------------------------------------------
        public double PositionY
        {
            get
            {
                return positionY;
            }
        }
        protected static double VerticalPading = 2;
        protected static double HorisontalPading = 2;
        //--------------------------------------------------------------------------------------
        public double Left
        {
            get
            {
                return positionX;
            }
        }
        //--------------------------------------------------------------------------------------
        public double Top
        {
            get
            {
                return positionY;
            }
        }
        //--------------------------------------------------------------------------------------
        public double Right
        {
            get
            {
                return positionX + Width;
            }
        }
        //--------------------------------------------------------------------------------------
        public double Bottom
        {
            get
            {
                return positionY + Height;
            }
        }
        //--------------------------------------------------------------------------------------
        public double MiddleHorisontal
        {
            get
            {
                return positionX + Width/2;
            }
        }
        //--------------------------------------------------------------------------------------
        public double MiddleVertical
        {
            get
            {
                return positionY + Height/2;
            }
        }
        //--------------------------------------------------------------------------------------
        public abstract double Width
        {
            get;
        }
        //--------------------------------------------------------------------------------------
        public abstract double Height
        {
            get;
        }
        //--------------------------------------------------------------------------------------
        public override string ToString()
        {
            return "Left: " + Math.Round(Left).ToString() + "; Top: " + Math.Round(Top).ToString();
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
    // class SectionVertex
    //--------------------------------------------------------------------------------------
    public class SectionVertex : Section, IRectangleInfo
    {
        private IVertex vertex;
        private int iinMatrix;
        private int jinMatrix;
        private List<ConnectingLine> topLines;
        private List<ConnectingLine> topLeftLines;
        private List<ConnectingLine> topRightLines;
        private ConnectingLine topDirectLine;
        private List<ConnectingLine> bottomLines;
        private List<ConnectingLine> bottomLeftLines;
        private List<ConnectingLine> bottomRightLines;
        private ConnectingLine bottomDirectLine;
        private List<ConnectingLine> leftLines;
        private List<ConnectingLine> leftTopLines;
        private List<ConnectingLine> leftBottomLines;
        private ConnectingLine leftDirectLine;
        private List<ConnectingLine> rightLines;
        private List<ConnectingLine> rightTopLines;
        private List<ConnectingLine> rightBottomLines;
        private ConnectingLine rightDirectLine;
        private Dictionary<ConnectingLine, Point> linePosition = new Dictionary<ConnectingLine, Point>();
        //--------------------------------------------------------------------------------------
        public SectionVertex(IVertex pVertex, double pPositionX, double pPositionY, int pIinMatrix, int pJinMatrix, PresentationData pPresentationData)
            : base(pPositionX, pPositionY, pPresentationData)
        {
            this.vertex = pVertex;
            iinMatrix = pIinMatrix;
            jinMatrix = pJinMatrix;
            topLines = new List<ConnectingLine>();
            topLeftLines = new List<ConnectingLine>();
            topRightLines = new List<ConnectingLine>();
            bottomLines = new List<ConnectingLine>();
            bottomLeftLines = new List<ConnectingLine>();
            bottomRightLines = new List<ConnectingLine>();
            leftLines = new List<ConnectingLine>();
            leftTopLines = new List<ConnectingLine>();
            leftBottomLines = new List<ConnectingLine>();
            rightLines = new List<ConnectingLine>();
            rightTopLines = new List<ConnectingLine>();
            rightBottomLines = new List<ConnectingLine>();
        }
        //--------------------------------------------------------------------------------------
        public string Name
        {
            get
            {
                if (vertex != null)
                {
                    return vertex.Name;
                }
                return "Empty";
            }
        }
        //--------------------------------------------------------------------------------------
        public int IinMatrix
        {
            get
            {
                return iinMatrix;
            }
        }
        //--------------------------------------------------------------------------------------
        public int JinMatrix
        {
            get
            {
                return jinMatrix;
            }
        }
        //--------------------------------------------------------------------------------------
        public bool IsEmptyPlace
        {
            get
            {
                if (vertex == null)
                    return true;
                else
                    return false;
            }
        }
        //--------------------------------------------------------------------------------------
        public override double Width
        {
            get
            {
                return presentationData.WidthVertex;
            }
        }
        //--------------------------------------------------------------------------------------
        public override double Height
        {
            get
            {
                return presentationData.HeightVertex;
            }
        }
        //--------------------------------------------------------------------------------------
        public List<ConnectingLine> TopLeftLines
        {
            get
            {
                return topLeftLines;
            }
        }
        //--------------------------------------------------------------------------------------
        public List<ConnectingLine> TopRightLines
        {
            get
            {
                return topRightLines;
            }
        }
        //--------------------------------------------------------------------------------------
        public List<ConnectingLine> BottomLeftLines
        {
            get
            {
                return bottomLeftLines;
            }
        }
        //--------------------------------------------------------------------------------------
        public List<ConnectingLine> BottomRightLines
        {
            get
            {
                return bottomRightLines;
            }
        }
        //--------------------------------------------------------------------------------------
        public List<ConnectingLine> LeftTopLines
        {
            get
            {
                return leftTopLines;
            }
        }
        //--------------------------------------------------------------------------------------
        public List<ConnectingLine> LeftBottomLines
        {
            get
            {
                return leftBottomLines;
            }
        }
        //--------------------------------------------------------------------------------------
        public List<ConnectingLine> RightTopLines
        {
            get
            {
                return rightTopLines;
            }
        }
        //--------------------------------------------------------------------------------------
        public List<ConnectingLine> RightBottomLines
        {
            get
            {
                return rightBottomLines;
            }
        }
        //--------------------------------------------------------------------------------------
        public void AddLine(ConnectingLine pLine, TypeOfTerminalEnum pTerminalType)
        {
            switch (pTerminalType)
            {
                case TypeOfTerminalEnum.Bottom:
                    bottomLines.Add(pLine);
                    break;
                case TypeOfTerminalEnum.Top:
                    topLines.Add(pLine);
                    break;
                case TypeOfTerminalEnum.Left:
                    leftLines.Add(pLine);
                    break;
                case TypeOfTerminalEnum.Right:
                    rightLines.Add(pLine);
                    break;
            }
        }
        //--------------------------------------------------------------------------------------
        public override string ToString()
        {
            return Name + " " + base.ToString();
        }
        //--------------------------------------------------------------------------------------
        public void DefineLineOrder(int[,] pConnectingLineRelation)
        {
            DefineLineOrderTop(pConnectingLineRelation);
            DefineLineOrderBottom(pConnectingLineRelation);
            DefineLineOrderLeft(pConnectingLineRelation);
            DefineLineOrderRight(pConnectingLineRelation);
        }
        //--------------------------------------------------------------------------------------
        private void DefineLineOrderRight(int[,] pConnectingLineRelation)
        {
            foreach (var l in rightLines)
            {
                if (l.TypeOfLine == TypeOfLineEnum.VerticalRight && l.Owner.Section1 == this || (l.TypeOfLine & TypeOfLineEnum.DiagonalTopLeft) == l.TypeOfLine && l.Owner.Section1 == this)
                    rightBottomLines.Add(l);
                else if (l.TypeOfLine == TypeOfLineEnum.VerticalRight && l.Owner.Section2 == this || (l.TypeOfLine & TypeOfLineEnum.DiagonalTopRight) == l.TypeOfLine && l.Owner.Section1 == this)
                    rightTopLines.Add(l);
                else if (l.TypeOfLine == TypeOfLineEnum.HorisontalDirect)
                    rightDirectLine = l;
            }

            RightBottomTerminalLineComparer lRightBottomTerminalLineComparer = new RightBottomTerminalLineComparer(pConnectingLineRelation);
            rightBottomLines.Sort(lRightBottomTerminalLineComparer);
            RightTopTerminalLineComparer lRightTopTerminalLineComparer = new RightTopTerminalLineComparer(pConnectingLineRelation);
            rightTopLines.Sort(lRightTopTerminalLineComparer);
        }
        //--------------------------------------------------------------------------------------
        private void DefineLineOrderLeft(int[,] pConnectingLineRelation)
        {
            foreach (var l in leftLines)
            {
                if (l.TypeOfLine == TypeOfLineEnum.VerticalLeft && l.Owner.Section1 == this || (l.TypeOfLine & TypeOfLineEnum.DiagonalTopRight) == l.TypeOfLine && l.Owner.Section2 == this)
                    leftBottomLines.Add(l);
                else if (l.TypeOfLine == TypeOfLineEnum.VerticalLeft && l.Owner.Section2 == this || (l.TypeOfLine & TypeOfLineEnum.DiagonalTopLeft) == l.TypeOfLine && l.Owner.Section2 == this)
                    leftTopLines.Add(l);
                else if (l.TypeOfLine == TypeOfLineEnum.HorisontalDirect)
                    leftDirectLine = l;
            }

            LeftBottomTerminalLineComparer lLeftBottomTerminalLineComparer = new LeftBottomTerminalLineComparer(pConnectingLineRelation);
            leftBottomLines.Sort(lLeftBottomTerminalLineComparer);
            LeftTopTerminalLineComparer lLeftTopTerminalLineComparer = new LeftTopTerminalLineComparer(pConnectingLineRelation);
            leftTopLines.Sort(lLeftTopTerminalLineComparer);
        }
        //--------------------------------------------------------------------------------------
        private void DefineLineOrderBottom(int[,] pConnectingLineRelation)
        {
            foreach (var l in bottomLines)
            {
                if (l.TypeOfLine == TypeOfLineEnum.HorisontalBottom && l.Owner.Section1 == this || (l.TypeOfLine & TypeOfLineEnum.DiagonalTopLeft) == l.TypeOfLine && l.Owner.Section1 == this)
                    bottomRightLines.Add(l);
                else if (l.TypeOfLine == TypeOfLineEnum.HorisontalBottom && l.Owner.Section2 == this || (l.TypeOfLine & TypeOfLineEnum.DiagonalTopRight) == l.TypeOfLine && l.Owner.Section2 == this)
                    bottomLeftLines.Add(l);
                else if (l.TypeOfLine == TypeOfLineEnum.VerticalDirect)
                    bottomDirectLine = l;
            }

            BottomLeftTerminalLineComparer lBottomLeftTerminalLineComparer = new BottomLeftTerminalLineComparer(pConnectingLineRelation);
            bottomLeftLines.Sort(lBottomLeftTerminalLineComparer);
            BottomRightTerminalLineComparer lBottomRightTerminalLineComparer = new BottomRightTerminalLineComparer(pConnectingLineRelation);
            bottomRightLines.Sort(lBottomRightTerminalLineComparer);
        }
        //--------------------------------------------------------------------------------------
        private void DefineLineOrderTop(int[,] pConnectingLineRelation)
        {
            foreach (var l in topLines)
            {
                if (l.TypeOfLine == TypeOfLineEnum.HorisontalTop && l.Owner.Section1 == this || (l.TypeOfLine & TypeOfLineEnum.DiagonalTopRight) == l.TypeOfLine && l.Owner.Section1 == this)
                    topRightLines.Add(l);
                else if (l.TypeOfLine == TypeOfLineEnum.HorisontalTop && l.Owner.Section2 == this || (l.TypeOfLine & TypeOfLineEnum.DiagonalTopLeft) == l.TypeOfLine && l.Owner.Section2 == this)
                    topLeftLines.Add(l);
                else if (l.TypeOfLine == TypeOfLineEnum.VerticalDirect)
                    topDirectLine = l;
            }

            TopRightTerminalLineComparer lTopRightTerminalLineComparer = new TopRightTerminalLineComparer(pConnectingLineRelation);
            topRightLines.Sort(lTopRightTerminalLineComparer);
            TopLeftTerminalLineComparer lTopLeftTerminalLineComparer = new TopLeftTerminalLineComparer(pConnectingLineRelation);
            topLeftLines.Sort(lTopLeftTerminalLineComparer);
        }
        //--------------------------------------------------------------------------------------
        public void DefineTopLinePosition(Dictionary<ConnectingLine, double> pLinePositionInStrip)
        {
            // anchor point for line on sides of rectangle
            // top side
            foreach (ConnectingLine line in topLines)
            {
                if (pLinePositionInStrip.ContainsKey(line))
                    linePosition.Add(line, new Point(pLinePositionInStrip[line], Top));
            }
        }
        //--------------------------------------------------------------------------------------
        public void DefineBottomLinePosition(Dictionary<ConnectingLine, double> pLinePositionInStrip)
        {
            // anchor point for line on sides of rectangle
            // top side
            foreach (ConnectingLine line in bottomLines)
            {
                if (pLinePositionInStrip.ContainsKey(line))
                    linePosition.Add(line, new Point(pLinePositionInStrip[line], Bottom));
            }
        }
        //--------------------------------------------------------------------------------------
        public void DefineLeftLinePosition(Dictionary<ConnectingLine, double> pLinePositionInStrip)
        {
            // anchor point for line on sides of rectangle
            // top side
            foreach (ConnectingLine line in leftLines)
            {
                if (pLinePositionInStrip.ContainsKey(line))
                    linePosition.Add(line, new Point(Left, pLinePositionInStrip[line]));
            }
        }
        //--------------------------------------------------------------------------------------
        public void DefineRightLinePosition(Dictionary<ConnectingLine, double> pLinePositionInStrip)
        {
            // anchor point for line on sides of rectangle
            // top side
            foreach (ConnectingLine line in rightLines)
            {
                if (pLinePositionInStrip.ContainsKey(line))
                    linePosition.Add(line, new Point(Right, pLinePositionInStrip[line]));
            }
        }
        //--------------------------------------------------------------------------------------
        public void DefineLinePosition()
        {
            // anchor point for line on sides of rectangle
            // top side
            if (topDirectLine != null)
                linePosition.Add(topDirectLine, new Point(MiddleHorisontal, Top));

            // bottom side
            if (bottomDirectLine != null)
                linePosition.Add(bottomDirectLine, new Point(MiddleHorisontal, Bottom));

            // Left side
            if (leftDirectLine != null)
                linePosition.Add(leftDirectLine, new Point(Left, MiddleVertical));

            // Right side
            if (rightDirectLine != null)
                linePosition.Add(rightDirectLine, new Point(Right, MiddleVertical));
        }
        //--------------------------------------------------------------------------------------
        public Point GetTerminalPoint(ConnectingLine connectingLine)
        {
            try
            {
                return linePosition[connectingLine];
            }
            catch (Exception e)
            {
                throw new Exception("Logical error in GetTerminalPoint", e);
            }
        }
        //--------------------------------------------------------------------------------------
        private class BottomRightTerminalLineComparer : IComparer<ConnectingLine>
        {
            private int[,] connectingLineRelation;
            //--------------------------------------------------------------------------------------
            public BottomRightTerminalLineComparer(int[,] pConnectingLineRelation)
            {
                connectingLineRelation = pConnectingLineRelation;
            }
            //--------------------------------------------------------------------------------------
            public int Compare(ConnectingLine pFirstLine, ConnectingLine pSecondLine)
            {
                if (pFirstLine.TypeOfLine == TypeOfLineEnum.HorisontalBottom && pSecondLine.TypeOfLine == TypeOfLineEnum.HorisontalBottom)
                {
                    if (pFirstLine.HorisontalX2 < pSecondLine.HorisontalX2)
                        return 1;
                    else
                        return -1;
                }
                if (pFirstLine.TypeOfLine == TypeOfLineEnum.HorisontalBottom)
                {
                    return 1;
                }
                if (pSecondLine.TypeOfLine == TypeOfLineEnum.HorisontalBottom)
                {
                    return -1;
                }
                else if ((pFirstLine.TypeOfLine & TypeOfLineEnum.DiagonalTopLeft) == pFirstLine.TypeOfLine)
                {
                    if ((pSecondLine.TypeOfLine & TypeOfLineEnum.DiagonalTopLeft) == pSecondLine.TypeOfLine)
                    {
                        int r = connectingLineRelation[pFirstLine.Index, pSecondLine.Index];
                        return r * (-1);
                    }
                    else
                        return 1;
                }
                else
                {
                    throw new Exception("Logical error in BottomRightTerminalLineComparer");
                }
            }
            //--------------------------------------------------------------------------------------
        }
        //--------------------------------------------------------------------------------------
        private class BottomLeftTerminalLineComparer : IComparer<ConnectingLine>
        {
            private int[,] connectingLineRelation;
            //--------------------------------------------------------------------------------------
            public BottomLeftTerminalLineComparer(int[,] pConnectingLineRelation)
            {
                connectingLineRelation = pConnectingLineRelation;
            }
            //--------------------------------------------------------------------------------------
            public int Compare(ConnectingLine pFirstLine, ConnectingLine pSecondLine)
            {
                if (pFirstLine.TypeOfLine == TypeOfLineEnum.HorisontalBottom && pSecondLine.TypeOfLine == TypeOfLineEnum.HorisontalBottom)
                {
                    if (pFirstLine.HorisontalX1 < pSecondLine.HorisontalX1)
                        return 1;
                    else
                        return -1;
                }
                else if (pFirstLine.TypeOfLine == TypeOfLineEnum.HorisontalBottom)
                {
                    return -1;
                }
                else if (pSecondLine.TypeOfLine == TypeOfLineEnum.HorisontalBottom)
                {
                    return 1;
                }
                else if ((pFirstLine.TypeOfLine & TypeOfLineEnum.DiagonalTopRight) == pFirstLine.TypeOfLine)
                {
                    if ((pSecondLine.TypeOfLine & TypeOfLineEnum.DiagonalTopRight) == pSecondLine.TypeOfLine)
                    {
                        int r = connectingLineRelation[pFirstLine.Index, pSecondLine.Index];
                        return r;
                    }
                    else
                        return -1;
                }
                else
                {
                    throw new Exception("Logical error in BottomLetTerminalLineComparer");
                }
            }
            //--------------------------------------------------------------------------------------
        }
        //--------------------------------------------------------------------------------------
        private class TopRightTerminalLineComparer : IComparer<ConnectingLine>
        {
            private int[,] connectingLineRelation;
            //--------------------------------------------------------------------------------------
            public TopRightTerminalLineComparer(int[,] pConnectingLineRelation)
            {
                connectingLineRelation = pConnectingLineRelation;
            }
            //--------------------------------------------------------------------------------------
            public int Compare(ConnectingLine pFirstLine, ConnectingLine pSecondLine)
            {
                if (pFirstLine.TypeOfLine == TypeOfLineEnum.HorisontalTop && pSecondLine.TypeOfLine == TypeOfLineEnum.HorisontalTop)
                {
                    if (pFirstLine.HorisontalX2 < pSecondLine.HorisontalX2)
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
                else if ((pFirstLine.TypeOfLine & TypeOfLineEnum.DiagonalTopRight) == pFirstLine.TypeOfLine)
                {
                    if ((pSecondLine.TypeOfLine & TypeOfLineEnum.DiagonalTopRight) == pSecondLine.TypeOfLine)
                    {
                        int r = connectingLineRelation[pFirstLine.Index, pSecondLine.Index];
                        return r;
                    }
                    else
                        return -1;
                }
                else
                {
                    throw new Exception("Logical error in TopRightTerminalLineComparer");
                }
            }
            //--------------------------------------------------------------------------------------
        }
        //--------------------------------------------------------------------------------------
        private class TopLeftTerminalLineComparer : IComparer<ConnectingLine>
        {
            private int[,] connectingLineRelation;
            //--------------------------------------------------------------------------------------
            public TopLeftTerminalLineComparer(int[,] pConnectingLineRelation)
            {
                connectingLineRelation = pConnectingLineRelation;
            }
            //--------------------------------------------------------------------------------------
            public int Compare(ConnectingLine pFirstLine, ConnectingLine pSecondLine)
            {
                if (pFirstLine.TypeOfLine == TypeOfLineEnum.HorisontalTop && pSecondLine.TypeOfLine == TypeOfLineEnum.HorisontalTop)
                {
                    if (pFirstLine.HorisontalX1 < pSecondLine.HorisontalX1)
                        return 1;
                    else
                        return -1;
                }
                else if (pFirstLine.TypeOfLine == TypeOfLineEnum.HorisontalTop)
                {
                    return -1;
                }
                else if (pSecondLine.TypeOfLine == TypeOfLineEnum.HorisontalTop)
                {
                    return 1;
                }
                else if ((pFirstLine.TypeOfLine & TypeOfLineEnum.DiagonalTopLeft) == pFirstLine.TypeOfLine)
                {
                    if ((pSecondLine.TypeOfLine & TypeOfLineEnum.DiagonalTopLeft) == pSecondLine.TypeOfLine)
                    {
                        int r = connectingLineRelation[pFirstLine.Index, pSecondLine.Index];
                        return r * (-1);
                    }
                    else
                        return -1;
                }
                else
                {
                    throw new Exception("Logical error in TopLeftTerminalLineComparer");
                }
            }
            //--------------------------------------------------------------------------------------
        }
        //--------------------------------------------------------------------------------------
        private class LeftTopTerminalLineComparer : IComparer<ConnectingLine>
        {
            private int[,] connectingLineRelation;
            //--------------------------------------------------------------------------------------
            public LeftTopTerminalLineComparer(int[,] pConnectingLineRelation)
            {
                connectingLineRelation = pConnectingLineRelation;
            }
            //--------------------------------------------------------------------------------------
            public int Compare(ConnectingLine pFirstLine, ConnectingLine pSecondLine)
            {
                if (pFirstLine.TypeOfLine == TypeOfLineEnum.VerticalLeft && pSecondLine.TypeOfLine == TypeOfLineEnum.VerticalLeft)
                {
                    if (pFirstLine.VerticalY1 < pSecondLine.VerticalY1)
                        return 1;
                    else
                        return -1;
                }
                else if (pFirstLine.TypeOfLine == TypeOfLineEnum.VerticalLeft)
                {
                    return -1;
                }
                else if (pSecondLine.TypeOfLine == TypeOfLineEnum.VerticalLeft)
                {
                    return 1;
                }
                else if ((pFirstLine.TypeOfLine & TypeOfLineEnum.DiagonalTopLeft) == pFirstLine.TypeOfLine)
                {
                    if ((pSecondLine.TypeOfLine & TypeOfLineEnum.DiagonalTopLeft) == pSecondLine.TypeOfLine)
                    {
                        int r = connectingLineRelation[pFirstLine.Index, pSecondLine.Index];
                        return r;
                    }
                    else
                        return -1;
                }
                else
                {
                    throw new Exception("Logical error in LeftTopTerminalLineComparer");
                }
            }
            //--------------------------------------------------------------------------------------
        }
        //--------------------------------------------------------------------------------------
        private class LeftBottomTerminalLineComparer : IComparer<ConnectingLine>
        {
            private int[,] connectingLineRelation;
            //--------------------------------------------------------------------------------------
            public LeftBottomTerminalLineComparer(int[,] pConnectingLineRelation)
            {
                connectingLineRelation = pConnectingLineRelation;
            }
            //--------------------------------------------------------------------------------------
            public int Compare(ConnectingLine pFirstLine, ConnectingLine pSecondLine)
            {
                if (pFirstLine.TypeOfLine == TypeOfLineEnum.VerticalLeft && pSecondLine.TypeOfLine == TypeOfLineEnum.VerticalLeft)
                {
                    if (pFirstLine.VerticalY2 < pSecondLine.VerticalY2)
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
                else if ((pFirstLine.TypeOfLine & TypeOfLineEnum.DiagonalTopRight) == pFirstLine.TypeOfLine)
                {
                    if ((pSecondLine.TypeOfLine & TypeOfLineEnum.DiagonalTopRight) == pSecondLine.TypeOfLine)
                    {
                        int r = connectingLineRelation[pFirstLine.Index, pSecondLine.Index];
                        return r;
                    }
                    else
                        return -1;
                }
                else
                {
                    throw new Exception("Logical error in LeftBottomTerminalLineComparer");
                }
            }
            //--------------------------------------------------------------------------------------
        }
        //--------------------------------------------------------------------------------------
        private class RightTopTerminalLineComparer : IComparer<ConnectingLine>
        {
            private int[,] connectingLineRelation;
            //--------------------------------------------------------------------------------------
            public RightTopTerminalLineComparer(int[,] pConnectingLineRelation)
            {
                connectingLineRelation = pConnectingLineRelation;
            }
            //--------------------------------------------------------------------------------------
            public int Compare(ConnectingLine pFirstLine, ConnectingLine pSecondLine)
            {
                if (pFirstLine.TypeOfLine == TypeOfLineEnum.VerticalRight && pSecondLine.TypeOfLine == TypeOfLineEnum.VerticalRight)
                {
                    if (pFirstLine.VerticalY1 < pSecondLine.VerticalY1)
                        return 1;
                    else
                        return -1;
                }
                else if (pFirstLine.TypeOfLine == TypeOfLineEnum.VerticalRight)
                {
                    return -1;
                }
                else if (pSecondLine.TypeOfLine == TypeOfLineEnum.VerticalRight)
                {
                    return 1;
                }
                else if ((pFirstLine.TypeOfLine & TypeOfLineEnum.DiagonalTopRight) == pFirstLine.TypeOfLine)
                {
                    if ((pSecondLine.TypeOfLine & TypeOfLineEnum.DiagonalTopRight) == pSecondLine.TypeOfLine)
                    {
                        int r = connectingLineRelation[pFirstLine.Index, pSecondLine.Index];
                        return r;
                    }
                    else
                        return -1;
                }
                else
                {
                    throw new Exception("Logical error in RightTopTerminalLineComparer");
                }
            }
            //--------------------------------------------------------------------------------------
        }
        //--------------------------------------------------------------------------------------
        private class RightBottomTerminalLineComparer : IComparer<ConnectingLine>
        {
            private int[,] connectingLineRelation;
            //--------------------------------------------------------------------------------------
            public RightBottomTerminalLineComparer(int[,] pConnectingLineRelation)
            {
                connectingLineRelation = pConnectingLineRelation;
            }
            //--------------------------------------------------------------------------------------
            public int Compare(ConnectingLine pFirstLine, ConnectingLine pSecondLine)
            {
                if (pFirstLine.TypeOfLine == TypeOfLineEnum.VerticalRight && pSecondLine.TypeOfLine == TypeOfLineEnum.VerticalRight)
                {
                    if (pFirstLine.VerticalY2 < pSecondLine.VerticalY2)
                        return 1;
                    else
                        return -1;
                }
                else if (pFirstLine.TypeOfLine == TypeOfLineEnum.VerticalRight)
                {
                    return 1;
                }
                else if (pSecondLine.TypeOfLine == TypeOfLineEnum.VerticalRight)
                {
                    return -1;
                }
                else if ((pFirstLine.TypeOfLine & TypeOfLineEnum.DiagonalTopLeft) == pFirstLine.TypeOfLine)
                {
                    if ((pSecondLine.TypeOfLine & TypeOfLineEnum.DiagonalTopLeft) == pSecondLine.TypeOfLine)
                    {
                        int r = connectingLineRelation[pFirstLine.Index, pSecondLine.Index];
                        return r;
                    }
                    else
                        return -1;
                }
                else
                {
                    throw new Exception("Logical error in RightTopTerminalLineComparer");
                }
            }
            //--------------------------------------------------------------------------------------
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
    // class SectionInStrip
    //--------------------------------------------------------------------------------------
    public abstract class SectionInStrip : Section
    {
        protected Dictionary<ConnectingLine, double> linePosition = new Dictionary<ConnectingLine, double>();
        //--------------------------------------------------------------------------------------
        public SectionInStrip(double pPositionX, double pPositionY, PresentationData pPresentationData)
            : base(pPositionX, pPositionY, pPresentationData)
        {
        }
        //--------------------------------------------------------------------------------------
        public virtual void DefineLineOrder()
        {
        }
        //--------------------------------------------------------------------------------------
        public virtual void DefineLinePosition()
        {
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
    // class SectionInVerticalStrip
    //--------------------------------------------------------------------------------------
    public class SectionInVerticalStrip : SectionInStrip
    {
        private SectionVertex nextLeft;
        private SectionVertex nextRight;
        private List<ConnectingLine> topSideLines;
        private List<ConnectingLine> bottomSideLines;
        //--------------------------------------------------------------------------------------
        public SectionInVerticalStrip(double pPositionX, double pPositionY, PresentationData pPresentationData)
            : base(pPositionX, pPositionY,pPresentationData)
        {
        }
        //--------------------------------------------------------------------------------------
        public override double Width
        {
            get
            {
                return presentationData.WidthStrip;
            }
        }
        //--------------------------------------------------------------------------------------
        public override double Height
        {
            get
            {
                return presentationData.HeightVertex;
            }
        }
        //--------------------------------------------------------------------------------------
        public void SetNeighbor(SectionVertex pNextLeft, SectionVertex pNextRight)
        {
            nextLeft = pNextLeft;
            nextRight = pNextRight;
        }
        //--------------------------------------------------------------------------------------
        public override void DefineLineOrder()
        {
            topSideLines = new List<ConnectingLine>();
            for (int i1 = 0, i2 = 0; i1 < nextLeft.RightTopLines.Count || i2 < nextRight.LeftTopLines.Count; i1++,i2++)
            {
                if (i1 < nextLeft.RightTopLines.Count)
                    topSideLines.Add(nextLeft.RightTopLines[i1]);
                if (i2 < nextRight.LeftTopLines.Count)
                    topSideLines.Add(nextRight.LeftTopLines[i2]);
            }
            bottomSideLines = new List<ConnectingLine>();
            for (int i1 = 0, i2 = 0; i1 < nextLeft.RightBottomLines.Count || i2 < nextRight.LeftBottomLines.Count; i1++, i2++)
            {
                if (i1 < nextLeft.RightBottomLines.Count)
                    bottomSideLines.Add(nextLeft.RightBottomLines[i1]);
                if (i2 < nextRight.LeftBottomLines.Count)
                    bottomSideLines.Add(nextRight.LeftBottomLines[i2]);
            }
        }
        //--------------------------------------------------------------------------------------
        public override void DefineLinePosition()
        {

            double lVerticalDelta = (Height / 2 - HorisontalPading) / (topSideLines.Count + 1);
            double lCurrentVertical = Top + VerticalPading + lVerticalDelta;
            foreach (ConnectingLine line in topSideLines)
            {
                linePosition.Add(line, lCurrentVertical);
                lCurrentVertical += lVerticalDelta;
            }
            lVerticalDelta = (Height / 2 - HorisontalPading) / (bottomSideLines.Count + 1);
            lCurrentVertical = MiddleVertical + lVerticalDelta;
            foreach (ConnectingLine line in bottomSideLines)
            {
                linePosition.Add(line, lCurrentVertical);
                lCurrentVertical += lVerticalDelta;
            }

            nextLeft.DefineRightLinePosition(linePosition);
            nextRight.DefineLeftLinePosition(linePosition);
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
    // class SectionInHorisontalStrip
    //--------------------------------------------------------------------------------------
    public class SectionInHorisontalStrip : SectionInStrip
    {
        private SectionVertex nextTop;
        private SectionVertex nextBottom;
        private List<ConnectingLine> leftSideLines;
        private List<ConnectingLine> rightSideLines;
        //--------------------------------------------------------------------------------------
        public SectionInHorisontalStrip(double pPositionX, double pPositionY, PresentationData pPresentationData)
            : base(pPositionX, pPositionY, pPresentationData)
        {
        }
        //--------------------------------------------------------------------------------------
        public override double Width
        {
            get
            {
                return presentationData.WidthVertex;
            }
        }
        //--------------------------------------------------------------------------------------
        public override double Height
        {
            get
            {
                return presentationData.HeightStrip;
            }
        }
        //--------------------------------------------------------------------------------------
        public void SetNeighbor(SectionVertex pNextTop, SectionVertex pNextBottom)
        {
            nextTop = pNextTop;
            nextBottom = pNextBottom;
        }
        //--------------------------------------------------------------------------------------
        public override void DefineLineOrder()
        {
            leftSideLines = new List<ConnectingLine>();
            for (int i1 = 0, i2 = 0; i1 < nextTop.BottomLeftLines.Count || i2 < nextBottom.TopLeftLines.Count; i1++, i2++)
            {
                if (i1 < nextTop.BottomLeftLines.Count)
                    leftSideLines.Add(nextTop.BottomLeftLines[i1]);
                if (i2 < nextBottom.TopLeftLines.Count)
                    leftSideLines.Add(nextBottom.TopLeftLines[i2]);
            }
            rightSideLines = new List<ConnectingLine>();
            for (int i1 = 0, i2 = 0; i1 < nextTop.BottomRightLines.Count || i2 < nextBottom.TopRightLines.Count; i1++, i2++)
            {
                if (i1 < nextTop.BottomRightLines.Count)
                    rightSideLines.Add(nextTop.BottomRightLines[i1]);
                if (i2 < nextBottom.TopRightLines.Count)
                    rightSideLines.Add(nextBottom.TopRightLines[i2]);
            }
        }
        //--------------------------------------------------------------------------------------
        public override void DefineLinePosition()
        {
            double lHorisontalDelta = (Width / 2 - HorisontalPading) / (leftSideLines.Count + 1);
            double lCurrentHorisontal = Left + HorisontalPading + lHorisontalDelta;
            foreach (ConnectingLine line in leftSideLines)
            {
                linePosition.Add(line, lCurrentHorisontal);
                lCurrentHorisontal += lHorisontalDelta;
            }
            lHorisontalDelta = (Width / 2 - HorisontalPading) / (rightSideLines.Count + 1);
            lCurrentHorisontal = MiddleHorisontal + lHorisontalDelta;
            foreach (ConnectingLine line in rightSideLines)
            {
                linePosition.Add(line, lCurrentHorisontal);
                lCurrentHorisontal += lHorisontalDelta;
            }

            nextTop.DefineBottomLinePosition(linePosition);
            nextBottom.DefineTopLinePosition(linePosition);
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
    // class SectionIntersection
    //--------------------------------------------------------------------------------------
    public class SectionIntersection : SectionInStrip
    {
        //--------------------------------------------------------------------------------------
        public SectionIntersection(double pPositionX, double pPositionY, PresentationData pPresentationData)
            : base(pPositionX, pPositionY,pPresentationData)
        {
        }
        //--------------------------------------------------------------------------------------
        public override double Width
        {
            get
            {
                return presentationData.WidthStrip;
            }
        }
        //--------------------------------------------------------------------------------------
        public override double Height
        {
            get
            {
                return presentationData.HeightStrip;
            }
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
}
