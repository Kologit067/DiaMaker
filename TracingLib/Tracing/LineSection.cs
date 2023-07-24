using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiaMakerModel.Tracing
{
    [Flags]
    public enum PlacingKindEnum {Transit = 1};
    //--------------------------------------------------------------------------------------
    // class LineSection
    //--------------------------------------------------------------------------------------
    public class LineSection
    {
        private ConnectingLine line;
        private Section section;
        private PlacingKindEnum placingKind;
        //--------------------------------------------------------------------------------------
        public PlacingKindEnum PlacingKind
        {
            get
            {
                return  placingKind;
            }
        }
        //--------------------------------------------------------------------------------------
        public ConnectingLine Line
        {
            get
            {
                return line;
            }
        }
        //--------------------------------------------------------------------------------------
        public Section Section
        {
            get
            {
                return section;
            }
        }
        //--------------------------------------------------------------------------------------
        public LineSection(ConnectingLine pLine, Section pSection)
        {
            line = pLine;
            section = pSection;
            placingKind = PlacingKindEnum.Transit;
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
}
