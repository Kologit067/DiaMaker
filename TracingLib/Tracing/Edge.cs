using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiaMakerModel.Tracing
{
    //--------------------------------------------------------------------------------------
    // class Edge
    //--------------------------------------------------------------------------------------
    public class Edge
    {
        private List<ConnectingLine> connectingLines;
        private SectionVertex section1;
        private SectionVertex section2;
        private SectionVertex startSection;
        private SectionVertex endSection;
        //--------------------------------------------------------------------------------------
        public Edge(SectionVertex pSection1, SectionVertex pSection2)
        {
            startSection = pSection1;
            endSection = pSection2;
            if ((int)startSection.PositionX < (int)endSection.PositionX || ((int)startSection.PositionX == (int)endSection.PositionX && (int)startSection.PositionY < (int)endSection.PositionY))
            {
                section1 = pSection1;
                section2 = pSection2;
            }
            else
            {
                section1 = pSection2;
                section2 = pSection1;
            }

        }
        //-------------------------------------------------------------------------------------
        public SectionVertex Section1
        {
            get
            {
                return section1;
            }
        }
        //-------------------------------------------------------------------------------------
        public SectionVertex Section2
        {
            get
            {
                return section2;
            }
        }
        //-------------------------------------------------------------------------------------
        public SectionVertex StartSection
        {
            get
            {
                return startSection;
            }
        }
        //-------------------------------------------------------------------------------------
        public SectionVertex EndSection
        {
            get
            {
                return endSection;
            }
        }
        //-------------------------------------------------------------------------------------
        public int X1
        {
            get
            {
                return section1.IinMatrix;
            }
        }
        //-------------------------------------------------------------------------------------
        public int Y1
        {
            get
            {
                return section1.JinMatrix;
            }
        }
        //-------------------------------------------------------------------------------------
        public int X2
        {
            get
            {
                return section2.IinMatrix;
            }
        }
        //-------------------------------------------------------------------------------------
        public int Y2
        {
            get
            {
                return section2.JinMatrix;
            }
        }
        //-------------------------------------------------------------------------------------
        public List<ConnectingLine> ConnectingLines
        {
            get
            {
                return connectingLines;
            }
        }
        //--------------------------------------------------------------------------------------
        public bool IsAlternate 
        {
            get
            {
                return connectingLines.Count > 1;
            }
        }
        //--------------------------------------------------------------------------------------
        internal ConnectingLine GetConnectingLine(int pVectorAsNumber, int pPosition)
        {
            int lIndex = (pVectorAsNumber >> pPosition) & 1;
            return connectingLines[lIndex];
        }
        //--------------------------------------------------------------------------------------

        internal void CreateConnectingLine(Section[,] pSectionMatrix, Tracing pTracing)
        {
        //DiagonalTopLeftFromRight = 64, DiagonalTopLeftFromBottom = 128, DiagonalTopLeft = 192,
        //DiagonalTopRightFromTop = 256, DiagonalTopRightFromRight = 512, DiagonalTopRight = 768,
        //BottomRight = 18, TopLeft = 9, Direct = 36, EqualLevel

            connectingLines = new List<ConnectingLine>();
            if (X1 == X2 && Y2 - Y1 == 2) /// VerticalDirect
            {
                ConnectingLine line = CreatwConnectingLine(pTracing, TypeOfLineEnum.VerticalDirect, TypeOfTerminalEnum.Bottom, TypeOfTerminalEnum.Top,
                    Y1 + 1, X1, X2, X1, Y1 + 1, Y1 + 1);
                LineSection ls = new LineSection(line, pSectionMatrix[X1, Y1 + 1]);
                connectingLines.Add(line);
            }
            else if (X1 == X2 && Y2 - Y1 > 2)  // VerticalLeft VerticalRight
            {
                if (X1 > 0)
                {
                    ConnectingLine lineLeft = CreatwConnectingLine(pTracing, TypeOfLineEnum.VerticalLeft, TypeOfTerminalEnum.Left, TypeOfTerminalEnum.Left,
                        Y2, X1 - 1, X1 - 1, X1 - 1, Y1, Y2);
                    connectingLines.Add(lineLeft);
                }
                if (X1 < pSectionMatrix.GetLength(0)-1)
                {
                    ConnectingLine lineRight = CreatwConnectingLine(pTracing, TypeOfLineEnum.VerticalRight, TypeOfTerminalEnum.Right, TypeOfTerminalEnum.Right,
                        Y2, X1 + 1, X1 + 1, X1 + 1, Y1, Y2);
                    connectingLines.Add(lineRight);
                }
            }
            else if (X2 - X1 == 2 && Y2 == Y1)  // HorisontalDirect
            {
                ConnectingLine line = CreatwConnectingLine(pTracing, TypeOfLineEnum.HorisontalDirect, TypeOfTerminalEnum.Right, TypeOfTerminalEnum.Left,
                    Y1, X1 + 1, X1 + 1, X1 + 1, Y1, Y1);
                connectingLines.Add(line);
            }
            else if (X2 - X1 > 2 && Y2 == Y1)  // HorisontalTop, HorisontalBottom
            {
                if (Y1 > 0)
                {
                    ConnectingLine lineTop = CreatwConnectingLine(pTracing, TypeOfLineEnum.HorisontalTop, TypeOfTerminalEnum.Top, TypeOfTerminalEnum.Top,
                        Y1 - 1, X1, X2, X2, Y1 - 1, Y1 - 1);
                    connectingLines.Add(lineTop);
                }
                if (Y1 < pSectionMatrix.GetLength(1) - 1)
                {
                    ConnectingLine lineBottom = CreatwConnectingLine(pTracing, TypeOfLineEnum.HorisontalBottom, TypeOfTerminalEnum.Bottom, TypeOfTerminalEnum.Bottom,
                        Y1 + 1, X1, X2, X2, Y1 + 1, Y1 + 1);

                    connectingLines.Add(lineBottom);
                }
            }
            else if (Y2 - Y1 == 2 && X2 - X1 == 2)  // DiagonalTopLeft
            {
                ConnectingLine line1 = CreatwConnectingLine(pTracing, TypeOfLineEnum.DiagonalTopLeft, TypeOfTerminalEnum.Right, TypeOfTerminalEnum.Left,
                    Y2, X1 + 1, X1 + 1, X1 + 1, Y1, Y2);

                connectingLines.Add(line1);

                ConnectingLine line2 = CreatwConnectingLine(pTracing, TypeOfLineEnum.DiagonalTopLeft, TypeOfTerminalEnum.Bottom, TypeOfTerminalEnum.Top,
                    Y1 + 1, X1, X2, X2, Y1 + 1, Y1 + 1);

                connectingLines.Add(line2);
            }
            else if (Y2 - Y1 == 2 && X2 - X1 > 2)  // DiagonalTopLeft
            {
                ConnectingLine line = CreatwConnectingLine(pTracing, TypeOfLineEnum.DiagonalTopLeft, TypeOfTerminalEnum.Bottom, TypeOfTerminalEnum.Top,
                    Y1 + 1, X1, X2, X2, Y1 + 1, Y1 + 1);

                connectingLines.Add(line);
            }
            else if (Y2 - Y1 > 2 && X2 - X1 == 2)  // DiagonalTopLeft
            {
                ConnectingLine line1 = CreatwConnectingLine(pTracing, TypeOfLineEnum.DiagonalTopLeft, TypeOfTerminalEnum.Right, TypeOfTerminalEnum.Left,
                    Y2, X1 + 1, X1 + 1, X1 + 1, Y1, Y2);

                connectingLines.Add(line1);
            }
            else if (Y2 - Y1 > 2 && X2 - X1 > 2)  // DiagonalTopLeft
            {
                ConnectingLine line1 = CreatwConnectingLine(pTracing, TypeOfLineEnum.DiagonalTopLeft, TypeOfTerminalEnum.Right, TypeOfTerminalEnum.Top,
                    Y2 - 1, X1 + 1, X2, X1 + 1, Y1, Y2 - 1);

                connectingLines.Add(line1);

                ConnectingLine line2 = CreatwConnectingLine(pTracing, TypeOfLineEnum.DiagonalTopLeft, TypeOfTerminalEnum.Bottom, TypeOfTerminalEnum.Left,
                    Y1 + 1, X1, X2 - 1, X2 - 1, Y1 + 1, Y2);

                connectingLines.Add(line2);
            }
            else if (Y1 - Y2 == 2 && X2 - X1 == 2)  // DiagonalTopRight
            {
                ConnectingLine line1 = CreatwConnectingLine(pTracing, TypeOfLineEnum.DiagonalTopRight, TypeOfTerminalEnum.Right, TypeOfTerminalEnum.Left,
                    Y2, X1 + 1, X1 + 1, X1 + 1, Y2, Y1);

                connectingLines.Add(line1);

                ConnectingLine line2 = CreatwConnectingLine(pTracing, TypeOfLineEnum.DiagonalTopRight, TypeOfTerminalEnum.Top, TypeOfTerminalEnum.Bottom,
                    Y1 - 1, X1, X2, X2, Y1 - 1, Y1 - 1);

                connectingLines.Add(line2);
            }
            else if (Y1 - Y2 == 2 && X2 - X1 > 2)  // DiagonalTopRight
            {
                ConnectingLine line2 = CreatwConnectingLine(pTracing, TypeOfLineEnum.DiagonalTopRight, TypeOfTerminalEnum.Top, TypeOfTerminalEnum.Bottom,
                    Y1 - 1, X1, X2, X2, Y1 - 1, Y1 - 1);

                connectingLines.Add(line2);
            }
            else if (Y1 - Y2 > 2 && X2 - X1 == 2)  // DiagonalTopRight
            {
                ConnectingLine line1 = CreatwConnectingLine(pTracing, TypeOfLineEnum.DiagonalTopRight, TypeOfTerminalEnum.Right, TypeOfTerminalEnum.Left,
                    Y2, X1 + 1, X1 + 1, X1 + 1, Y2, Y1);

                connectingLines.Add(line1);
            }
            else if (Y1 - Y2 > 2 && X2 - X1 > 2)  // DiagonalTopRight
            {
                ConnectingLine line1 = CreatwConnectingLine(pTracing, TypeOfLineEnum.DiagonalTopRight, TypeOfTerminalEnum.Top, TypeOfTerminalEnum.Left,
                    Y1 - 1, X1, X2 - 1, X2 - 1, Y1 - 1, Y2);

                connectingLines.Add(line1);

                ConnectingLine line2 = CreatwConnectingLine(pTracing, TypeOfLineEnum.DiagonalTopRight, TypeOfTerminalEnum.Right, TypeOfTerminalEnum.Bottom, 
                    Y2 + 1, X1 + 1, X2, X1 + 1, Y1, Y2 + 1);

                connectingLines.Add(line2);
            }
            if ( connectingLines.Count == 0)
                throw new Exception("Logical error in CreateConnectingLine.No lines were created.");

        }
        //--------------------------------------------------------------------------------------
        public ConnectingLine CreatwConnectingLine(Tracing pTracing, TypeOfLineEnum pTypeOfLine, 
            TypeOfTerminalEnum pTerminalType1, TypeOfTerminalEnum pTerminalType2,
            int pHorisontalY, int pHorisontalX1, int pHorisontalX2, int pVerticalX, 
            int pVerticalY1, int pVerticalY2)
        {

            ConnectingLine line = new ConnectingLine(this, pTypeOfLine,  pTerminalType1,  pTerminalType2,
             pHorisontalY,  pHorisontalX1,  pHorisontalX2,  pVerticalX,  pVerticalY1,  pVerticalY2);

            section1.AddLine(line, pTerminalType1);
            section2.AddLine(line, pTerminalType2);

            if (pHorisontalX1 != pHorisontalX2)
            {
                Strip horisontalStrip = pTracing.HorisontalStrips[(pHorisontalY - 1)/2];
                horisontalStrip.AddLine(line);
            }
            if (pVerticalY1 != pVerticalY2)
            {
                Strip verticalStrip = pTracing.VerticalStrips[(pVerticalX - 1) / 2];
                verticalStrip.AddLine(line);
            }
            return line;
        }
        //--------------------------------------------------------------------------------------
        public override string ToString()
        {
            return Section1.Name + " -> " + Section2.Name;
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
}
  