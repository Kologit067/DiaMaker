using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Common.CommonLib.Interfaces;
using System.Windows.Media;

namespace DiaMakerModel.Tracing
{
    [Flags]
    public enum TypeOfLineEnum
    {
        VerticalLeft = 1, VerticalRight = 2, VerticalDirect = 4, Vertical = 7,
        HorisontalTop = 8, HorisontalBottom = 16, HorisontalDirect = 32, Horisontal = 25,
        DiagonalTopLeftFromRight = 64, DiagonalTopLeftFromBottom = 128, DiagonalTopLeft = 192,
        DiagonalTopRightFromTop = 256, DiagonalTopRightFromRight = 512, DiagonalTopRight = 768,
        BottomRight = 18, TopLeft = 9, Direct = 36, EqualLevel = 27
    }
    public enum TypeOfTerminalEnum
    {
        Left = 1, Right = 2, Top = 4, Bottom = 8
    }
    //--------------------------------------------------------------------------------------
    // class ConnectingLine
    //--------------------------------------------------------------------------------------
    public class ConnectingLine : IConnectLineInfo
    {
        private Point startPoint;                       // need to do
        private Point endPoint;                         // need to do
        private Point firstPoint;                       // need to do
        private Point lastPoint;                         // need to do
//        private List<LineSection> sectionSequance;      // need to do
//        private Dictionary<Tuple<int, int>, LineSection> sectionDictionary;      // need to do
//        private Dictionary<Section, LineSection> sectionDictionary; 

        private PointCollection points = new PointCollection();
        private List<Strip> strips = new List<Strip>(); // need to do
        public int Index { get; set; }
        public int IndexInOptimalList { get; set; }
        private TypeOfLineEnum typeOfLine;
        private Edge owner;
        private TypeOfTerminalEnum terminalType1;
        private TypeOfTerminalEnum terminalType2;
        private int verticalX;
        private int verticalY1;
        private int verticalY2;
        private int horisontalY;
        private int horisontalX1;
        private int horisontalX2;
        //--------------------------------------------------------------------------------------
        public ConnectingLine(Edge pOwner, TypeOfLineEnum pTypeOfLine, TypeOfTerminalEnum pTerminalType1, TypeOfTerminalEnum pTerminalType2,
            int pHorisontalY, int pHorisontalX1, int pHorisontalX2, int pVerticalX, int pVerticalY1, int pVerticalY2)
        {
            owner = pOwner;
            typeOfLine = pTypeOfLine;
            terminalType1 = pTerminalType1;
            terminalType2 = pTerminalType2;
            verticalX = pVerticalX;
            verticalY1 = pVerticalY1;
            verticalY2 = pVerticalY2;
            horisontalY = pHorisontalY;
            horisontalX1 = pHorisontalX1;
            horisontalX2 = pHorisontalX2;
        }
        //--------------------------------------------------------------------------------------
        public TypeOfLineEnum TypeOfLine
        {
            get
            {
                return typeOfLine;
            }
        }
        //--------------------------------------------------------------------------------------
        public PointCollection Points
        {
            get
            {
                return points;
            }
        }
        //--------------------------------------------------------------------------------------
        public TypeOfTerminalEnum TerminalType1
        {
            get
            {
                return terminalType1;
            }
        }
        //--------------------------------------------------------------------------------------
        public TypeOfTerminalEnum TerminalType2
        {
            get
            {
                return terminalType2;
            }
        }
        //--------------------------------------------------------------------------------------
        public Edge Owner
        {
            get
            {
                return owner;
            }
            set
            {
                owner = value;
            }
        }
        //--------------------------------------------------------------------------------------
        public int VerticalX
        {
            get
            {
                return verticalX;
            }
            set
            {
                verticalX = value;
            }
        }
        //--------------------------------------------------------------------------------------
        public int VerticalY1
        {
            get
            {
                return verticalY1;
            }
            set
            {
                verticalY1 = value;
            }
        }
        //--------------------------------------------------------------------------------------
        public int VerticalY2
        {
            get
            {
                return verticalY2;
            }
            set
            {
                verticalY2 = value;
            }
        }
        //--------------------------------------------------------------------------------------
        public int HorisontalY
        {
            get
            {
                return horisontalY;
            }
            set
            {
                horisontalY = value;
            }
        }
        //--------------------------------------------------------------------------------------
        public int HorisontalX1
        {
            get
            {
                return horisontalX1;
            }
            set
            {
                horisontalX1 = value;
            }
        }
        //--------------------------------------------------------------------------------------
        public int HorisontalX2
        {
            get
            {
                return horisontalX2;
            }
            set
            {
                horisontalX2 = value;
            }
        }
        ////--------------------------------------------------------------------------------------
        //public List<LineSection> SectionSequance      // need to do
        //{
        //    get
        //    {
        //        return sectionSequance;
        //    }
        //    set
        //    {
        //        sectionSequance = value;
        //    }
        //}
        //--------------------------------------------------------------------------------------
        public void DefineSequanceOfPoint()
        {
            firstPoint = owner.Section1.GetTerminalPoint(this);
            lastPoint = owner.Section2.GetTerminalPoint(this);
            startPoint = owner.StartSection.GetTerminalPoint(this);
            endPoint = owner.EndSection.GetTerminalPoint(this);
            Point lNextPoint = firstPoint;
            points.Add(lNextPoint);
            for (int i = 0; i < strips.Count; i++)
            {
                lNextPoint = strips[i].GetNextPoint(this, lNextPoint);
                points.Add(lNextPoint);
            }
            if (strips.Count > 0)
            {
                if (strips[strips.Count - 1] is HorisontalStrip)
                    lNextPoint = new Point(lastPoint.X, lNextPoint.Y);
                else
                    lNextPoint = new Point(lNextPoint.X, lastPoint.Y);
                points.Add(lNextPoint);
            }
            points.Add(lastPoint);
        }
        //--------------------------------------------------------------------------------------
        internal Tuple<int,int> DefineRelation(ConnectingLine pOtherLine)
        {
            int lIntersect = 0;
            int lOrder = 0;
            if (TypeOfLine == pOtherLine.TypeOfLine)
            {
                if ((TypeOfLine & TypeOfLineEnum.Direct) == TypeOfLine)
                {
                    return new Tuple<int, int>(lOrder, lIntersect);
                }
                if ((TypeOfLine & TypeOfLineEnum.Vertical) == TypeOfLine)
                {
                    if (TypeOfLine == TypeOfLineEnum.VerticalLeft)
                    {
                        if (VerticalY1 >= pOtherLine.VerticalY1 && VerticalY2 < pOtherLine.VerticalY2)
                            lOrder = -1;
                        else if (VerticalY1 > pOtherLine.VerticalY1 && VerticalY2 <= pOtherLine.VerticalY2)
                            lOrder = -1;
                        else if (VerticalY1 <= pOtherLine.VerticalY1 && VerticalY2 > pOtherLine.VerticalY2)
                            lOrder = 1;
                        else if (VerticalY1 < pOtherLine.VerticalY1 && VerticalY2 >= pOtherLine.VerticalY2)
                            lOrder = 1;
                        else if (VerticalY1 < pOtherLine.VerticalY1)
                            lOrder = -1;
                        else
                            lOrder = 1;
                    }
                    else if (TypeOfLine == TypeOfLineEnum.VerticalRight)
                    {
                        if (VerticalY1 >= pOtherLine.VerticalY1 && VerticalY2 < pOtherLine.VerticalY2)
                            lOrder = 1;
                        else if (VerticalY1 > pOtherLine.VerticalY1 && VerticalY2 <= pOtherLine.VerticalY2)
                            lOrder = 1;
                        else if (VerticalY1 <= pOtherLine.VerticalY1 && VerticalY2 > pOtherLine.VerticalY2)
                            lOrder = -1;
                        else if (VerticalY1 < pOtherLine.VerticalY1 && VerticalY2 >= pOtherLine.VerticalY2)
                            lOrder = -1;
                        else if (VerticalY1 < pOtherLine.VerticalY1)
                            lOrder = -1;
                        else
                            lOrder = 1;
                    }
                    if (VerticalY1 < pOtherLine.verticalY1 && VerticalY2 < pOtherLine.VerticalY2 && pOtherLine.VerticalY1 < VerticalY2 ||
                        VerticalY1 > pOtherLine.VerticalY1 && VerticalY2 > pOtherLine.VerticalY2 && pOtherLine.VerticalY1 > VerticalY2)
                        lIntersect = 1;
                    return new Tuple<int, int>(lOrder, lIntersect);
                }
                if ((TypeOfLine & TypeOfLineEnum.Horisontal) == TypeOfLine)
                {
                    if (HorisontalX1 < pOtherLine.HorisontalX1)
                        lOrder = -1;
                    if (HorisontalX1 > pOtherLine.HorisontalX1)
                        lOrder = 1;
                    if (HorisontalX2 < pOtherLine.HorisontalX2)
                        lOrder = -1;
                    else
                        lOrder = 1;
                    if (TypeOfLine == TypeOfLineEnum.HorisontalTop)
                    {
                        if (HorisontalX1 >= pOtherLine.HorisontalX1 && HorisontalX2 < pOtherLine.HorisontalX2)
                            lOrder = -1;
                        else if (HorisontalX1 > pOtherLine.HorisontalX1 && HorisontalX2 <= pOtherLine.HorisontalX2)
                            lOrder = -1;
                        else if (HorisontalX1 <= pOtherLine.HorisontalX1 && HorisontalX2 > pOtherLine.HorisontalX2)
                            lOrder = 1;
                        else if (HorisontalX1 < pOtherLine.HorisontalX1 && HorisontalX2 >= pOtherLine.HorisontalX2)
                            lOrder = 1;
                        else if (HorisontalX1 < pOtherLine.HorisontalX1)
                            lOrder = -1;
                        else
                            lOrder = 1;
                    }
                    else if (TypeOfLine == TypeOfLineEnum.HorisontalBottom)
                    {
                        if (HorisontalX1 >= pOtherLine.HorisontalX1 && HorisontalX2 < pOtherLine.HorisontalX2)
                            lOrder = 1;
                        else if (HorisontalX1 > pOtherLine.HorisontalX1 && HorisontalX2 <= pOtherLine.HorisontalX2)
                            lOrder = 1;
                        else if (HorisontalX1 <= pOtherLine.HorisontalX1 && HorisontalX2 > pOtherLine.HorisontalX2)
                            lOrder = -1;
                        else if (HorisontalX1 < pOtherLine.HorisontalX1 && HorisontalX2 >= pOtherLine.HorisontalX2)
                            lOrder = -1;
                        else if (HorisontalX1 < pOtherLine.HorisontalX1)
                            lOrder = -1;
                        else
                            lOrder = 1;
                    }
                    if (HorisontalX1 < pOtherLine.HorisontalX1 && HorisontalX2 < pOtherLine.HorisontalX2 && pOtherLine.HorisontalX1 < HorisontalX2 ||
                        HorisontalX1 > pOtherLine.HorisontalX1 && HorisontalX2 > pOtherLine.HorisontalX2 && pOtherLine.HorisontalX1 > HorisontalX2)
                        lIntersect = 1;
                    return new Tuple<int, int>(lOrder, lIntersect);
                }
                if ((TypeOfLine & TypeOfLineEnum.DiagonalTopLeft) == TypeOfLine)
                {
                    if (HorisontalY < pOtherLine.HorisontalY || 
                        HorisontalY == pOtherLine.HorisontalY && HorisontalX1 > pOtherLine.HorisontalX1 ||
                        HorisontalY == pOtherLine.HorisontalY && HorisontalX1 == pOtherLine.HorisontalX1 && HorisontalX2 > pOtherLine.HorisontalX2 ||
                        HorisontalY == pOtherLine.HorisontalY && HorisontalX1 == pOtherLine.HorisontalX1 && HorisontalX2 == pOtherLine.HorisontalX2 && VerticalY1 < pOtherLine.VerticalY1 ||
                        HorisontalY == pOtherLine.HorisontalY && HorisontalX1 == pOtherLine.HorisontalX1 && HorisontalX2 == pOtherLine.HorisontalX2 && VerticalY1 == pOtherLine.VerticalY1 && VerticalY2 < pOtherLine.VerticalY2)
                        lOrder = -1;
                    else
                        lOrder = 1;

                    // coomon horisontal or vertical
                    if (HorisontalY == pOtherLine.HorisontalY &&
                        (HorisontalX1 < pOtherLine.HorisontalX1 && HorisontalX2 > pOtherLine.HorisontalX2 && pOtherLine.HorisontalX1 < HorisontalX2 ||
                         HorisontalX1 > pOtherLine.HorisontalX1 && HorisontalX2 < pOtherLine.HorisontalX2 && pOtherLine.HorisontalX1 > HorisontalX2))
                        lIntersect = 1;
                    if (VerticalX == pOtherLine.VerticalX &&
                        (VerticalY1 < pOtherLine.VerticalY1 && VerticalY2 > pOtherLine.VerticalY2 && pOtherLine.VerticalY1 < VerticalY2 ||
                         VerticalY1 > pOtherLine.VerticalY1 && VerticalY2 < pOtherLine.VerticalY2 && pOtherLine.VerticalY1 > VerticalY2))
                        lIntersect = 1;
                    // intersection first horisontal and second vertical or second horisontal and first vertical
                    if (HorisontalY > pOtherLine.VerticalY1 && HorisontalY < pOtherLine.VerticalY2 && pOtherLine.VerticalX > HorisontalX1 && pOtherLine.VerticalX < HorisontalX2)
                        lIntersect += 1;
                    if (pOtherLine.HorisontalY > VerticalY1 && pOtherLine.HorisontalY < VerticalY2 && VerticalX > pOtherLine.HorisontalX1 && VerticalX < pOtherLine.HorisontalX2)
                        lIntersect += 1;
                    //  first and second horisontal and vertical in same strip
                    if (HorisontalY == pOtherLine.HorisontalY && VerticalX == pOtherLine.VerticalX &&
                        (
                        HorisontalX1 < pOtherLine.HorisontalX1 && VerticalY2 < pOtherLine.VerticalY2 ||
                        HorisontalX1 > pOtherLine.HorisontalX1 && VerticalY2 > pOtherLine.VerticalY2 ||
                        HorisontalX2 < pOtherLine.HorisontalX2 && VerticalY1 < pOtherLine.VerticalY1 ||
                        HorisontalX2 > pOtherLine.HorisontalX2 && VerticalY1 > pOtherLine.VerticalY1
                        ))
                        lIntersect += 1;

                }
                if ((TypeOfLine & TypeOfLineEnum.DiagonalTopRight) == TypeOfLine)
                {
                    if (HorisontalY < pOtherLine.HorisontalY ||
                        HorisontalY == pOtherLine.HorisontalY && HorisontalX1 < pOtherLine.HorisontalX1 ||
                        HorisontalY == pOtherLine.HorisontalY && HorisontalX1 == pOtherLine.HorisontalX1 && HorisontalX2 < pOtherLine.HorisontalX2 ||
                        HorisontalY == pOtherLine.HorisontalY && HorisontalX1 == pOtherLine.HorisontalX1 && HorisontalX2 == pOtherLine.HorisontalX2 && VerticalY1 < pOtherLine.VerticalY1 ||
                        HorisontalY == pOtherLine.HorisontalY && HorisontalX1 == pOtherLine.HorisontalX1 && HorisontalX2 == pOtherLine.HorisontalX2 && VerticalY1 == pOtherLine.VerticalY1 && VerticalY2 < pOtherLine.VerticalY2)
                        lOrder = -1;
                    else
                        lOrder = 1;

                    // coomon horisontal or vertical
                    if (HorisontalY == pOtherLine.HorisontalY &&
                        (HorisontalX1 < pOtherLine.HorisontalX1 && HorisontalX2 > pOtherLine.HorisontalX2 && pOtherLine.HorisontalX1 < HorisontalX2 ||
                         HorisontalX1 > pOtherLine.HorisontalX1 && HorisontalX2 < pOtherLine.HorisontalX2 && pOtherLine.HorisontalX1 > HorisontalX2))
                        lIntersect = 1;
                    if (VerticalX == pOtherLine.VerticalX &&
                        (VerticalY1 < pOtherLine.VerticalY1 && VerticalY2 > pOtherLine.VerticalY2 && pOtherLine.VerticalY1 < VerticalY2 ||
                         VerticalY1 > pOtherLine.VerticalY1 && VerticalY2 < pOtherLine.VerticalY2 && pOtherLine.VerticalY1 > VerticalY2))
                        lIntersect = 1;
                    // intersection first horisontal and second vertical or second horisontal and first vertical
                    if (HorisontalY > pOtherLine.VerticalY1 && HorisontalY < pOtherLine.VerticalY2 && pOtherLine.VerticalX > HorisontalX1 && pOtherLine.VerticalX < HorisontalX2)
                        lIntersect += 1;
                    if (pOtherLine.HorisontalY > VerticalY1 && pOtherLine.HorisontalY < VerticalY2 && VerticalX > pOtherLine.HorisontalX1 && VerticalX < pOtherLine.HorisontalX2)
                        lIntersect += 1;
                    //  first and second horisontal and vertical in same strip
                    if (HorisontalY == pOtherLine.HorisontalY && VerticalX == pOtherLine.VerticalX &&
                        (
                        HorisontalX1 < pOtherLine.HorisontalX1 && VerticalY2 > pOtherLine.VerticalY2 ||
                        HorisontalX1 > pOtherLine.HorisontalX1 && VerticalY2 < pOtherLine.VerticalY2 ||
                        HorisontalX2 < pOtherLine.HorisontalX2 && VerticalY1 > pOtherLine.VerticalY1 ||
                        HorisontalX2 > pOtherLine.HorisontalX2 && VerticalY1 < pOtherLine.VerticalY1
                        ))
                        lIntersect += 1;

                }
            }
            else
            {
                if (HorisontalY < pOtherLine.HorisontalY || HorisontalY == pOtherLine.HorisontalY && VerticalX < pOtherLine.VerticalX)
                    lOrder = -1;
                else
                    lOrder = 1;

                if (TypeOfLine == TypeOfLineEnum.HorisontalDirect && (HorisontalY > pOtherLine.VerticalY1 && HorisontalY < pOtherLine.VerticalY2 ||
                    HorisontalY < pOtherLine.VerticalY1 && HorisontalY > pOtherLine.VerticalY2) &&
                    HorisontalX1 == pOtherLine.VerticalX)
                    lIntersect += 1;
                if (TypeOfLine == TypeOfLineEnum.VerticalDirect && (VerticalX > pOtherLine.HorisontalX1 && VerticalX < pOtherLine.HorisontalX2 ||
                    VerticalX < pOtherLine.HorisontalX1 && VerticalX > pOtherLine.HorisontalX2) &&
                    verticalY1 == pOtherLine.HorisontalY)
                    lIntersect += 1;
                if (pOtherLine.TypeOfLine == TypeOfLineEnum.HorisontalDirect && (pOtherLine.HorisontalY > VerticalY1 && pOtherLine.HorisontalY < VerticalY2 ||
                    pOtherLine.HorisontalY < VerticalY1 && pOtherLine.HorisontalY > VerticalY2) &&
                    pOtherLine.HorisontalX1 == VerticalX)
                    lIntersect += 1;
                if (pOtherLine.TypeOfLine == TypeOfLineEnum.VerticalDirect && (pOtherLine.VerticalX > HorisontalX1 && pOtherLine.VerticalX < HorisontalX2 ||
                    pOtherLine.VerticalX < HorisontalX1 && pOtherLine.VerticalX > HorisontalX2) &&
                    pOtherLine.VerticalY1 == HorisontalY)
                    lIntersect += 1;

                // intersection first horisontal and second vertical or second horisontal and first vertical
                if (HorisontalY > pOtherLine.VerticalY1 && HorisontalY < pOtherLine.VerticalY2 && pOtherLine.VerticalX > HorisontalX1 && pOtherLine.VerticalX < HorisontalX2)
                    lIntersect += 1;
                if (pOtherLine.HorisontalY > VerticalY1 && pOtherLine.HorisontalY < VerticalY2 && VerticalX > pOtherLine.HorisontalX1 && VerticalX < pOtherLine.HorisontalX2)
                    lIntersect += 1;

                if (lIntersect == 0 
                    &&
                    ((TypeOfLine & TypeOfLineEnum.DiagonalTopLeft) == TypeOfLine && (pOtherLine.TypeOfLine & TypeOfLineEnum.DiagonalTopRight) == pOtherLine.TypeOfLine ||
                    (TypeOfLine & TypeOfLineEnum.DiagonalTopRight) == TypeOfLine && (pOtherLine.TypeOfLine & TypeOfLineEnum.DiagonalTopLeft) == pOtherLine.TypeOfLine))
                    if ( (VerticalX == pOtherLine.VerticalX
                        &&
                        (
                        Math.Min(VerticalY1, VerticalY2) == Math.Min(pOtherLine.VerticalY1, pOtherLine.VerticalY2) && Math.Max(VerticalY1, VerticalY2) == Math.Max(pOtherLine.VerticalY1, pOtherLine.VerticalY2) ||
                        VerticalY1 < pOtherLine.VerticalY1 && VerticalY1 > pOtherLine.VerticalY2 ||
                        VerticalY1 > pOtherLine.VerticalY1 && VerticalY1 < pOtherLine.VerticalY2 ||
                        VerticalY2 < pOtherLine.VerticalY1 && VerticalY2 > pOtherLine.VerticalY2 ||
                        VerticalY2 > pOtherLine.VerticalY1 && VerticalY2 < pOtherLine.VerticalY2 ||
                        pOtherLine.VerticalY1 < VerticalY1 && pOtherLine.VerticalY1 > VerticalY2 ||
                        pOtherLine.VerticalY1 > VerticalY1 && pOtherLine.VerticalY1 < VerticalY2 ||
                        pOtherLine.VerticalY2 < VerticalY1 && pOtherLine.VerticalY2 > VerticalY2 ||
                        pOtherLine.VerticalY2 > VerticalY1 && pOtherLine.VerticalY2 < VerticalY2 ))
                        ||
                        (HorisontalY == pOtherLine.HorisontalY
                        &&
                        (
                        HorisontalX1 == pOtherLine.HorisontalX2 && HorisontalX2 == pOtherLine.HorisontalX1 ||
                        HorisontalX1 < pOtherLine.HorisontalX1 && HorisontalX1 > pOtherLine.HorisontalX2 ||
                        HorisontalX1 > pOtherLine.HorisontalX1 && HorisontalX1 < pOtherLine.HorisontalX2 ||
                        HorisontalX2 < pOtherLine.HorisontalX1 && HorisontalX2 > pOtherLine.HorisontalX2 ||
                        HorisontalX2 > pOtherLine.HorisontalX1 && HorisontalX2 < pOtherLine.HorisontalX2 ||
                        pOtherLine.HorisontalX1 < HorisontalX1 && pOtherLine.HorisontalX1 > HorisontalX2 ||
                        pOtherLine.HorisontalX1 > HorisontalX1 && pOtherLine.HorisontalX1 < HorisontalX2 ||
                        pOtherLine.HorisontalX2 < HorisontalX1 && pOtherLine.HorisontalX2 > HorisontalX2 ||
                        pOtherLine.HorisontalX2 > HorisontalX1 && pOtherLine.HorisontalX2 < HorisontalX2
                        )))
                        lIntersect += 1;
            }
            return new Tuple<int, int>(lOrder, lIntersect);
        }
        //--------------------------------------------------------------------------------------
        //public static Tuple<int, int, int> DefineRelaytionForDirectType(ConnectingLine pDirectLine, ConnectingLine pOtherLine)
        //{
        //    LineSection otherSectionLine;
        //    pOtherLine.sectionDictionary.TryGetValue(pDirectLine.SectionSequance[0].Section, out otherSectionLine);
        //    if (otherSectionLine != null && otherSectionLine.PlacingKind == PlacingKindEnum.Transit)
        //        return new Tuple<int, int, int>(0, 1, 0);
        //    return new Tuple<int, int, int>(0, 0, 0);
        //}
        //--------------------------------------------------------------------------------------
        public void CreateStripList(List<HorisontalStrip> pHorisontalStrips, List<VerticalStrip> pVerticalStrips)
        {
            if (HorisontalX1 != HorisontalX2 || VerticalY1 != VerticalY2)
            {
                if (TerminalType1 == TypeOfTerminalEnum.Bottom || TerminalType1 == TypeOfTerminalEnum.Top && HorisontalX1 != HorisontalX2)
                {
                    strips.Add(pHorisontalStrips[(HorisontalY - 1) / 2]);
                    if (VerticalY1 != VerticalY2)
                        strips.Add(pVerticalStrips[(VerticalX - 1) / 2]);
                }
                else if (TerminalType1 == TypeOfTerminalEnum.Left || TerminalType1 == TypeOfTerminalEnum.Right && VerticalY1 != VerticalY2)
                {
                    strips.Add(pVerticalStrips[(VerticalX - 1) / 2]);
                    if (HorisontalX1 != HorisontalX2)
                        strips.Add(pHorisontalStrips[(HorisontalY - 1) / 2]);
                }
            }
        }
        //--------------------------------------------------------------------------------------
        public override string ToString()
        {
            return Owner.ToString() + "  " + terminalType1;
        }
        //--------------------------------------------------------------------------------------

        internal ArrowData CreateArrow()
        {
            if ((Math.Abs(endPoint.X - lastPoint.X) < 0.1) && (Math.Abs(endPoint.Y - lastPoint.Y) < 0.1))
                return new ArrowData(terminalType2, endPoint);
            else
                return new ArrowData(terminalType1, endPoint);
        }
    }
    //--------------------------------------------------------------------------------------
}
