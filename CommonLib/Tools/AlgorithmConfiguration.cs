using CommonLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Tools
{
    //--------------------------------------------------------------------------------------
    // class AlgorithmConfiguration
    //--------------------------------------------------------------------------------------
    public class AlgorithmConfiguration
    {

        public EmptyOrderEnum EmptyOrder { get; set; }
        public BaseEnumerationEnum BaseEnumeration { get; set; }
        public UsingLowestimationEnum UsingLowestimation { get; set; }
        public EliminationEnum Elimination { get; set; }
        public IsSortedEnum IsSorted { get; set; }
        public SortedTypeEnum SortedType { get; set; }
        //--------------------------------------------------------------------------------------
        public AlgorithmConfiguration(EmptyOrderEnum pEmptyOrder, BaseEnumerationEnum pBaseEnumeration,
            UsingLowestimationEnum pUsingLowestimation, EliminationEnum pElimination,
            IsSortedEnum pIsSorted, SortedTypeEnum pSortedType)
        {
            EmptyOrder = pEmptyOrder;
            BaseEnumeration = pBaseEnumeration;
            UsingLowestimation = pUsingLowestimation;
            Elimination = pElimination;
            IsSorted = pIsSorted;
            SortedType = pSortedType;
        }
        //--------------------------------------------------------------------------------------
        public AlgorithmConfiguration()
        {
            EmptyOrder = EmptyOrderEnum.EmptyFirst;
            BaseEnumeration = BaseEnumerationEnum.ByPlace;
            UsingLowestimation = UsingLowestimationEnum.NotUseLowEstimation;
            Elimination = EliminationEnum.FullEnum;
            IsSorted = IsSortedEnum.IsSortedNo;
            SortedType = SortedTypeEnum.SortedNone;
        }
       //--------------------------------------------------------------------------------------
        public string AlgorithmName
        {
            get
            {
                return (EmptyOrder == EmptyOrderEnum.EmptyFirst ? "EOF_" : "EOL_") +
                       (BaseEnumeration == BaseEnumerationEnum.ByPlace ? "BEP_" : "BEV_") +
                       (UsingLowestimation == UsingLowestimationEnum.WithUseLowEstation ? "LEY_" : "LEN_") +
                       (Elimination == EliminationEnum.FullEnum ? "ETF_" : (Elimination == EliminationEnum.StrictElim ? "ETS_" : "ETR_")) +
                       (IsSorted == IsSortedEnum.IsSortedNo ? "ISN_" : "ISY_") +
                       (SortedType == SortedTypeEnum.SortedNone ? "STN" : (SortedType == SortedTypeEnum.SortedBoth ? "STB" : (SortedType == SortedTypeEnum.SortedPlace ? "STP" : "STV")));
            }
        }
        //--------------------------------------------------------------------------------------
        public static AlgorithmConfiguration CreateFromName(string pAlgorithmName)
        {
            AlgorithmConfiguration algorithmConfiguration = new AlgorithmConfiguration();

            if (pAlgorithmName.Contains("EOF_"))
                algorithmConfiguration.EmptyOrder = EmptyOrderEnum.EmptyFirst;
            else if (pAlgorithmName.Contains("EOL_"))
                algorithmConfiguration.EmptyOrder = EmptyOrderEnum.EmptyLast;
            else
                throw new ArgumentException("can not define EmptyOrder.", "pAlgorithmName");

            if (pAlgorithmName.Contains("BEP_"))
                algorithmConfiguration.BaseEnumeration = BaseEnumerationEnum.ByPlace;
            else if (pAlgorithmName.Contains("BEV_"))
                algorithmConfiguration.BaseEnumeration = BaseEnumerationEnum.ByVertex;
            else
                throw new ArgumentException("can not define BaseEnumeration", "pAlgorithmName");

            if (pAlgorithmName.Contains("LEY_"))
                algorithmConfiguration.UsingLowestimation = UsingLowestimationEnum.WithUseLowEstation;
            else if (pAlgorithmName.Contains("LEN_"))
                algorithmConfiguration.UsingLowestimation = UsingLowestimationEnum.NotUseLowEstimation;
            else
                throw new ArgumentException("can not define BaseEnumeration", "pAlgorithmName");

            if (pAlgorithmName.Contains("ETF_"))
                algorithmConfiguration.Elimination = EliminationEnum.FullEnum;
            else if (pAlgorithmName.Contains("ETS_"))
                algorithmConfiguration.Elimination = EliminationEnum.StrictElim;
            else if (pAlgorithmName.Contains("ETR_"))
                algorithmConfiguration.Elimination = EliminationEnum.RughElim;
            else
                throw new ArgumentException("can not define BaseEnumeration", "pAlgorithmName");

            if (pAlgorithmName.Contains("ISN_"))
                algorithmConfiguration.IsSorted = IsSortedEnum.IsSortedNo;
            else if (pAlgorithmName.Contains("ISY_"))
                algorithmConfiguration.IsSorted = IsSortedEnum.IsSortedYes;
            else
                throw new ArgumentException("can not define IsSorted.", "pAlgorithmName");

            if (pAlgorithmName.Contains("STN"))
                algorithmConfiguration.SortedType = SortedTypeEnum.SortedNone;
            else if (pAlgorithmName.Contains("STB"))
                algorithmConfiguration.SortedType = SortedTypeEnum.SortedBoth;
            else if (pAlgorithmName.Contains("STP"))
                algorithmConfiguration.SortedType = SortedTypeEnum.SortedPlace;
            else if (pAlgorithmName.Contains("STV"))
                algorithmConfiguration.SortedType = SortedTypeEnum.SortedVertex;
            else
                throw new ArgumentException("can not define IsSorted.", "pAlgorithmName");

            return algorithmConfiguration;
        }
        //--------------------------------------------------------------------------------------
        public static ObservableCollection<T> CreateObservableCollection<T>()
        {
            ObservableCollection<T> result = new ObservableCollection<T>();

            if (typeof(T) == typeof(EmptyOrderEnum) || typeof(T) == typeof(BaseEnumerationEnum) || typeof(T) == typeof(UsingLowestimationEnum) ||
                typeof(T) == typeof(EliminationEnum) || typeof(T) == typeof(IsSortedEnum) || typeof(T) == typeof(SortedTypeEnum))
            {
                foreach (T t in Enum.GetValues(typeof(T)))
                    result.Add(t);
            }

            return result;
        }
        //--------------------------------------------------------------------------------------
        public static List<AlgorithmConfiguration> CreateAllPossibleConfiguration()
        {
            List<AlgorithmConfiguration> result;
            var query = from eo in CreateObservableCollection<EmptyOrderEnum>()
                        from be in CreateObservableCollection<BaseEnumerationEnum>()
                        from ul in CreateObservableCollection<UsingLowestimationEnum>()
                        from en in CreateObservableCollection<EliminationEnum>()
                        from ss in CreateObservableCollection<IsSortedEnum>()
                        from st in CreateObservableCollection<SortedTypeEnum>()
                        select new AlgorithmConfiguration(eo,be,ul,en,ss,st);
            result = query.ToList();
            return result;
        }
        //--------------------------------------------------------------------------------------
        public static List<string> CreateAlgorithmNameAllPossibleConfiguration()
        {
            return CreateAllPossibleConfiguration().Select(c => c.AlgorithmName).ToList();
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
}
