using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArrayExtensions
{
    public static class IntArrayExtension
    {
        public static int[] InversePermitation(this int[] permitation)
        {
            Int32[] inverse = new Int32[permitation.Length];

            for (int i = 0; i < permitation.Length; i++)
                    inverse[permitation[i]] = i;
            return inverse;
        }
        public static int[] InversePermitationFrom1(this int[] permitation)
        {
            Int32[] inverse = new Int32[permitation.Length];

            for (int i = 0; i < permitation.Length; i++)
                inverse[permitation[i]-1] = i;
            return inverse;
        }

    }
}
