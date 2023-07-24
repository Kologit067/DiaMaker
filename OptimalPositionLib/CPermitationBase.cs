using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace OptimalPositionLib
{
    // EmptyFirst/EmptyLast
    // ByVertex/ByPlace
    // WithUseLowEst/NotUseLowEst
    // FullEnum/StructElim/RughElim
    //--------------------------------------------------------------------------------------
    // class CPermitationBase
    //--------------------------------------------------------------------------------------
    public class CPermitationBase
    {
        //--------------------------------------------------------------------------------------
        protected Int64 fCurrentState;		// номера уже включенные в текущую последовательность
        protected int fCurrentPosition;		// текущая глубина при обходе дерева
        protected int[] fRoute;				// маршрут обхода
        protected int fSize;					// размерность задачи
        protected bool fOutQueryStop;			// 
        protected bool fIsStop;					// признак останова
        private int fMaxEmpty;
        private int fCurrentNumberOfEmpty;
        protected int fNumberOfPlace;
        protected long fIterationCount;
        protected long fElapsedTicks;
        private long fDurationMilliSeconds;
        protected Func<int, bool, int> NextPointMethod;
        protected int startNumber;
        //--------------------------------------------------------------------------------------
        public int[] Route
        {
            get
            {
                return fRoute;
            }
        }
        //--------------------------------------------------------------------------------------
        public long IterationCount
        {
            get
            {
                return fIterationCount;
            }
        }
        //--------------------------------------------------------------------------------------
        public long ElapsedTicks
        {
            get
            {
                return fElapsedTicks;
            }
        }
        //--------------------------------------------------------------------------------------
        public long DurationMilliSeconds
        {
            get
            {
                return fDurationMilliSeconds;
            }
        }
        //--------------------------------------------------------------------------------------
        public CPermitationBase(int pSize, int pNumberOfPlace)
        {
            fSize = pSize;
            fNumberOfPlace = pNumberOfPlace;
            fMaxEmpty = Math.Max(fNumberOfPlace - fSize, 0);
            fIsStop = false;
            fOutQueryStop = false;

            // empty last/first 
            NextPointMethod = NextPointEmptyFirst;
            startNumber = 0;
        }
        //--------------------------------------------------------------------------------------
        public virtual void Execute()
        {
            Timer stateTimer = new Timer(TimerAction, null, 120000, 120000);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            InitialData();
            while (fCurrentPosition >= 0 && !fIsStop)
            {
                if (IsEliminable())	// если возможно отсечь всю ветвь дерева
                {
                    if (!Back())				// то возвращаемся назад
                        break;
                }
                else
                    Forward();			// если невозможно то вперед
            }
            stopwatch.Stop();
            fElapsedTicks = stopwatch.ElapsedTicks;
            fDurationMilliSeconds = stopwatch.ElapsedMilliseconds;
            stateTimer.Dispose();
        }
        //--------------------------------------------------------------------------------------
        private void TimerAction(object pObj)
        {
            Stop();
        }
        //--------------------------------------------------------------------------------------
        public virtual bool IsEliminable()
        {
            fIterationCount++;
            if (fCurrentPosition == fNumberOfPlace )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //--------------------------------------------------------------------------------------
        private bool Back()
        {
            while (fCurrentPosition > 0)
            {
                // меняем текущий пункт вычетаем соотвестсвующий участок из текущей суммы
                fCurrentPosition--;	 // на одну позицию назад (вниз по дереву)
                BackAction(fCurrentPosition);
                int lNextPoint = NextPoint(fRoute[fCurrentPosition], false);	// найти первый незадействованный
                if (lNextPoint >= 0) // если есть еще непросмотренные пункты на данном уровне
                {
                    fRoute[fCurrentPosition] = lNextPoint;	// 
                    fCurrentPosition++;
                    return true;
                }
            }
            return false;
        }
        //--------------------------------------------------------------------------------------
        protected virtual void BackAction(int pCurrentPosition)
        {
            
        }
        //--------------------------------------------------------------------------------------
        private int NextPoint(int pStartNumber, bool pIsForward)
        {
            return NextPointMethod(pStartNumber, pIsForward);
        }
        //--------------------------------------------------------------------------------------
        protected int NextPointEmptyFirst(int pStartNumber, bool pIsForward)
        {
            int lFreeNumber = pStartNumber;

            if (pIsForward && lFreeNumber == 0 && fCurrentNumberOfEmpty < fMaxEmpty)
            {
                fCurrentNumberOfEmpty++;
            }
            else
            {
                lFreeNumber++;
                Int64 lMaska = 1;
                lMaska = lMaska << (lFreeNumber-1);

                while ((fCurrentState & lMaska) != 0) // && lFreeNumber != fCurrentPosition)
                {
                    lMaska = lMaska << 1;
                    lFreeNumber++;
                }

                if (lFreeNumber > fSize)
                    lFreeNumber = -1;
                else
                {
                    Int64 lTmp = (1 << (lFreeNumber - 1));
                    fCurrentState = fCurrentState | lMaska;
                }
            }
            if (!pIsForward)
            {
                if (pStartNumber > 0)
                    fCurrentState = fCurrentState & (~(1 << (pStartNumber - 1)));
                else
                    fCurrentNumberOfEmpty--;
            }
            return lFreeNumber;
        }
        //--------------------------------------------------------------------------------------
        protected int NextPointEmptyLast(int pStartNumber, bool pIsForward)
        {
            int lFreeNumber = pStartNumber == -1 ? 1 : (pStartNumber == 0 ? fSize + 1 : pStartNumber + 1);

            //lFreeNumber++;
            Int64 lMaska = 1;
            lMaska = lMaska << (lFreeNumber - 1);

            while ((fCurrentState & lMaska) != 0) // && lFreeNumber != fCurrentPosition)
            {
                lMaska = lMaska << 1;
                lFreeNumber++;
            }

            if (lFreeNumber <= fSize)
            {
                Int64 lTmp = (1 << (lFreeNumber - 1));
                fCurrentState = fCurrentState | lMaska;
            }
            else if ((pIsForward || pStartNumber != 0) && fCurrentNumberOfEmpty < fMaxEmpty)
            {
                lFreeNumber = 0;
                fCurrentNumberOfEmpty++;
            }
            else
                lFreeNumber = -1;

            if (!pIsForward)
            {
                if (pStartNumber > 0)
                    fCurrentState = fCurrentState & (~(1 << (pStartNumber - 1)));
                else
                    fCurrentNumberOfEmpty--;
            }
            return lFreeNumber;
        }
        //--------------------------------------------------------------------------------------
        private void Forward()
        {
            // на одну позицию вперед (вниз по дереву)
            fRoute[fCurrentPosition] = NextPoint(startNumber, true);	// найти первый незадействованный
            fCurrentPosition++;
        }
        //--------------------------------------------------------------------------------------
        public virtual void InitialData()
        {
            fRoute = new int[fNumberOfPlace];
            fCurrentState = 0;
            fRoute[0] = 0;
            fCurrentPosition = 0;
        }
        //--------------------------------------------------------------------------------------
        public string RouteAsString
        {
            get
            {
                if (fRoute != null && fRoute.Length > 0)
                    return string.Join(",",fRoute.Select(i => i.ToString()));
                return "Empty";
            }
        }
        //--------------------------------------------------------------------------------------
        public void Stop()
        {
            fOutQueryStop = true;
            fIsStop = true;
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
    // class CPermitationBaseEmptyLast
    //--------------------------------------------------------------------------------------
    public class CPermitationBaseEmptyLast : CPermitationBase
    {
        //--------------------------------------------------------------------------------------
        public CPermitationBaseEmptyLast(int pSize, int pNumberOfPlace)
            : base(pSize, pNumberOfPlace)
        {
            // empty last/first 
            NextPointMethod = NextPointEmptyLast;
            startNumber = -1;
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
}
