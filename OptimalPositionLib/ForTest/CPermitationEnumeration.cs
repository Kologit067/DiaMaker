using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalPositionLib
{
    //--------------------------------------------------------------------------------------
    // class CPermitationEnumeration
    //--------------------------------------------------------------------------------------
    public class CPermitationEnumeration : PermitationBase
    {
        private List<int[]> fResultSet = new List<int[]>();
        private List<string> fResultStringList = new List<string>();
        private HashSet<string> fResultStringSet = new HashSet<string>();
        //--------------------------------------------------------------------------------------
        public CPermitationEnumeration(int pSize, int pNumberOfPlace, int pMaxDurability)
            : base(pSize, pNumberOfPlace, pMaxDurability)
        {
        }
        //--------------------------------------------------------------------------------------
        public override bool IsEliminable()
        {
            fIterationCount++;
            if (fCurrentPosition == fNumberOfPlace)
            {
                int[] lNewVector = fRoute.Select(i => i).ToArray(); //new int[fNumberOfPlace];
                fResultSet.Add(lNewVector);
                fResultStringList.Add(RouteAsString);
                ResultStringSet.Add(RouteAsString);
                fCountTerminal++;
                
                return true;
            }
            else
            {
                return false;
            }
        }
        //--------------------------------------------------------------------------------------
        public List<int[]> ResultSet
        {
            get
            {
                return fResultSet;
            }
        }
        //--------------------------------------------------------------------------------------
        public List<string> ResultStringList
        {
            get
            {
                return fResultStringList;
            }
        }
        //--------------------------------------------------------------------------------------
        public HashSet<string> ResultStringSet
        {
            get
            {
                return fResultStringSet;
            }
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
    // class CPermitationEmptyLastEnumeration
    //--------------------------------------------------------------------------------------
    public class CPermitationEmptyLastEnumeration : PermitationEmptyLast
    {
        private List<int[]> fResultSet = new List<int[]>();
        private List<string> fResultStringList = new List<string>();
        private HashSet<string> fResultStringSet = new HashSet<string>();
        //--------------------------------------------------------------------------------------
        public CPermitationEmptyLastEnumeration(int pSize, int pNumberOfPlace, int pMaxDurability)
            : base(pSize, pNumberOfPlace, pMaxDurability)
        {
        }
        //--------------------------------------------------------------------------------------
        public override bool IsEliminable()
        {
            fIterationCount++;
            if (fCurrentPosition == fNumberOfPlace)
            {
                int[] lNewVector = fRoute.Select(i => i).ToArray(); //new int[fNumberOfPlace];
                fResultSet.Add(lNewVector);
                fResultStringList.Add(RouteAsString);
                ResultStringSet.Add(RouteAsString);
                return true;
            }
            else
            {
                return false;
            }
        }
        //--------------------------------------------------------------------------------------
        public List<int[]> ResultSet
        {
            get
            {
                return fResultSet;
            }
        }
        //--------------------------------------------------------------------------------------
        public List<string> ResultStringList
        {
            get
            {
                return fResultStringList;
            }
        }
        //--------------------------------------------------------------------------------------
        public HashSet<string> ResultStringSet
        {
            get
            {
                return fResultStringSet;
            }
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
    // class CPermitationBaseEmptyLastEnumeration
    //--------------------------------------------------------------------------------------
    public class CPermitationBaseEmptyLastEnumeration : CPermitationBaseEmptyLast
    {
        private List<int[]> fResultSet = new List<int[]>();
        private List<string> fResultStringList = new List<string>();
        private HashSet<string> fResultStringSet = new HashSet<string>();
        //--------------------------------------------------------------------------------------
        public CPermitationBaseEmptyLastEnumeration(int pSize, int pNumberOfPlace, int pMaxDurability)
            : base(pSize, pNumberOfPlace, pMaxDurability)
        {
        }
        //--------------------------------------------------------------------------------------
        public override bool IsEliminable()
        {
            fIterationCount++;
            if (fCurrentPosition == fNumberOfPlace)
            {
                int[] lNewVector = fRoute.Select(i => i).ToArray(); //new int[fNumberOfPlace];
                fResultSet.Add(lNewVector);
                fResultStringList.Add(RouteAsString);
                ResultStringSet.Add(RouteAsString);
                fCountTerminal++;
                
                return true;
            }
            else
            {
                return false;
            }
        }
        //--------------------------------------------------------------------------------------
        public List<int[]> ResultSet
        {
            get
            {
                return fResultSet;
            }
        }
        //--------------------------------------------------------------------------------------
        public List<string> ResultStringList
        {
            get
            {
                return fResultStringList;
            }
        }
        //--------------------------------------------------------------------------------------
        public HashSet<string> ResultStringSet
        {
            get
            {
                return fResultStringSet;
            }
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
}
