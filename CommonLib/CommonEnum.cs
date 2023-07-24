using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib
{
    public enum EmptyOrderEnum { EmptyFirst, EmptyLast }
    public enum BaseEnumerationEnum { ByVertex, ByPlace }
    public enum UsingLowestimationEnum { WithUseLowEstation, NotUseLowEstimation }
    public enum EliminationEnum { FullEnum, StrictElim, RughElim }
    public enum IsSortedEnum { IsSortedNo, IsSortedYes }
    public enum SortedTypeEnum { SortedNone, SortedPlace, SortedVertex, SortedBoth }
}
