using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.CommonLib.Interfaces;

namespace DiaMakerModel.Tracing
{
    //--------------------------------------------------------------------------------------
    // class PresentationData
    //--------------------------------------------------------------------------------------
    public class PresentationData
    {
        public static double RelationVertexStripHorisontal = 1.0;
        public static double RelationVertexStripVertical = 1.0;
        public static double MinimalElementWidth = 200;
        public static double MinimalElementHeight = 75;
        private double widthStrip = 0;
        private double heightStrip = 0;
        private double widthVertex = 0;
        private double heightVertex = 0;
        private double panelWidth;
        private double panelHeight;
        private Tuple<int, int> sizes;
        private double horisontalStripIndent = 0;
        private double verticalStripIndent = 0;
        //--------------------------------------------------------------------------------------
        public PresentationData(double pPanelWidth, double pPanelHeight, IGraph<IVertex> pGraph)
        {
            MinimalMultiplicationHeightDiaPanel = 1;
            MinimalMultiplicationWidthDiaPanel = 1;
            this.panelWidth = pPanelWidth;
            this.panelHeight = pPanelHeight;
            double minimalPanelWidth;
            double minimalPanelHeight;
            //sizes = GetMatrixSize(pGraph);
            sizes = GetMatrixSizeGreedy(pGraph, pPanelWidth / pPanelHeight);
            widthStrip = (panelWidth / (RelationVertexStripHorisontal * sizes.Item1 + sizes.Item1 - 1));
            heightStrip = (panelHeight / (RelationVertexStripVertical * sizes.Item2 + sizes.Item2 - 1));
            widthVertex = (widthStrip * RelationVertexStripHorisontal);
            heightVertex = (heightStrip * RelationVertexStripVertical);
            minimalPanelHeight = MinimalElementHeight * sizes.Item2 + (MinimalElementHeight / RelationVertexStripVertical) * (sizes.Item2 - 1);
            minimalPanelWidth = MinimalElementWidth * sizes.Item1 + (MinimalElementWidth / RelationVertexStripHorisontal) * (sizes.Item1 - 1);
            MinimalMultiplicationHeightDiaPanel = minimalPanelHeight / panelHeight;
            MinimalMultiplicationWidthDiaPanel = minimalPanelWidth / panelWidth;
        }
        //--------------------------------------------------------------------------------------
        public double MinimalMultiplicationHeightDiaPanel { get; set; }
        public double MinimalMultiplicationWidthDiaPanel { get; set; }
        //--------------------------------------------------------------------------------------
        public double HorisontalStripIndent
        {
            get
            {
                return horisontalStripIndent;
            }
        }
        //--------------------------------------------------------------------------------------
        public double VerticalStripIndent
        {
            get
            {
                return verticalStripIndent;
            }
        }
        //--------------------------------------------------------------------------------------
        public double WidthStrip
        {
            get
            {
                return widthStrip;
            }
        }
        //--------------------------------------------------------------------------------------
        public double HeightStrip
        {
            get
            {
                return heightStrip;
            }
        }
        //--------------------------------------------------------------------------------------
        public double WidthVertex
        {
            get
            {
                return widthVertex;
            }
        }
        //--------------------------------------------------------------------------------------
        public double HeightVertex
        {
            get
            {
                return heightVertex;
            }
        }
        //--------------------------------------------------------------------------------------
        public Tuple<int, int> Sizes
        {
            get
            {
                return sizes;
            }
        }
        //--------------------------------------------------------------------------------------
        private Tuple<int, int> GetMatrixSize(IGraph<IVertex> pGraph)
        {
            if ( pGraph.Vertices.Count == 0)
                return new Tuple<int, int>(0, 0);
            double ProportionX = 3;
            double ProportionY = 2;
            double yf = Math.Sqrt(ProportionY * pGraph.Vertices.Count / ProportionX);
            double xf = pGraph.Vertices.Count / yf;
            if (yf < 3.0)
                yf = 3.0;
            if (xf < 3.0)
                xf = 3.0;
            return new Tuple<int, int>((int)Math.Ceiling(xf), (int)Math.Ceiling(yf));
        }
        //--------------------------------------------------------------------------------------
        private Tuple<int, int> GetMatrixSizeGreedy(IGraph<IVertex> pGraph, double pProportion)
        {
            if (pGraph.Vertices.Count == 0)
                return new Tuple<int, int>(0, 0);
            for (int i = 3; i < 10; i++)
            {
                for (int j = Math.Max(3, (int)Math.Floor(i / pProportion)); j <= i; j++)
                {
                    if (i * j >= pGraph.Vertices.Count)
                        return new Tuple<int, int>(i, j);
                }
            }
            return new Tuple<int, int>(0,0);
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
}
