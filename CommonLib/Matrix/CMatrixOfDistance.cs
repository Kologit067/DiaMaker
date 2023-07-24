using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.CommonLib.Interfaces;

namespace OptimalPositionLib.Matrix
{
    //-------------------------------------------------------------------------------------------------------
    // class CMatrixOfDistance
    //-------------------------------------------------------------------------------------------------------
    public class CMatrixOfDistance : IMatrixOfDistance
    {
        private int _fWidth;
        private int _fHeight;
        private int _fLength;
        private int[,] _fMatrix;
        //-------------------------------------------------------------------------------------------------------
        public CMatrixOfDistance(int pWidth, int pHeight)
        {
            _fHeight = pHeight;
            _fWidth = pWidth;
            _fLength = _fWidth * _fHeight;
            _fMatrix = new int[_fLength, _fLength];
 //           Generate();
        }
        //-------------------------------------------------------------------------------------------------------
        public void Generate()
        {
            for (int i = 0; i < _fLength; i++)
            {
                for (int j = 0; j < _fLength; j++)
                {
                    int lFirstX = i / _fHeight;
                    int lFirstY = i % _fHeight;
                    int lSecondX = j / _fHeight;
                    int lSecondY = j % _fHeight;
                    int lDistanceX = Math.Abs(lFirstX - lSecondX);
                    int lDistanceY = Math.Abs(lFirstY - lSecondY);
                    int lMaxDistance = Math.Max(lDistanceX, lDistanceY);
                    int lMinDistance = Math.Min(lDistanceX, lDistanceY);

                    int lBaseWeight = 0;
                    if (lMinDistance > 0)
                        lBaseWeight++;
                    if (lMaxDistance > 1)
                    {
                        lBaseWeight++;
                        int lDelta = 2;
                        int lCount = lMaxDistance + lMinDistance - 1;
                        while (lCount-- > 2)
                            lBaseWeight += lDelta++;
                    }
                    int lWeight = lBaseWeight;
                    if (lMinDistance == 0 && lMaxDistance > 1)
                    {
                        lWeight += 5;
                    }
                    else
                    {
                        if (lMinDistance > 0)
                            lWeight += lMinDistance - 1;
                    }
                    _fMatrix[i, j] = lWeight;
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------
        public void GenerateParallel()
        {

            Parallel.For(0, _fLength, i =>
            {
                for (int j = 0; j < _fLength; j++)
                {
                    int lFirstX = i / _fHeight;
                    int lFirstY = i % _fHeight;
                    int lSecondX = j / _fHeight;
                    int lSecondY = j % _fHeight;
                    int lDistanceX = Math.Abs(lFirstX - lSecondX);
                    int lDistanceY = Math.Abs(lFirstY - lSecondY);
                    int lMaxDistance = Math.Max(lDistanceX, lDistanceY);
                    int lMinDistance = Math.Min(lDistanceX, lDistanceY);

                    int lBaseWeight = 0;
                    if (lMinDistance > 0)
                        lBaseWeight++;
                    if (lMaxDistance > 1)
                    {
                        lBaseWeight++;
                        int lDelta = 2;
                        int lCount = lMaxDistance + lMinDistance - 1;
                        while (lCount-- > 2)
                            lBaseWeight += lDelta++;
                    }
                    int lWeight = lBaseWeight;
                    if (lMinDistance == 0 && lMaxDistance > 1)
                    {
                        lWeight += 5;
                    }
                    else
                    {
                        if (lMinDistance > 0)
                            lWeight += lMinDistance - 1;
                    }
                    _fMatrix[i, j] = lWeight;
                }
            });
        }
        //-------------------------------------------------------------------------------------------------------
        public int Length
        {
            get
            {
                return _fLength;
            }
        }
        //-------------------------------------------------------------------------------------------------------
        public int Width
        {
            get
            {
                return _fWidth;
            }
        }
        //-------------------------------------------------------------------------------------------------------
        public int Height
        {
            get
            {
                return _fHeight;
            }
        }
        //-------------------------------------------------------------------------------------------------------
        public int this[int x, int y]
        {
            get
            {
                return _fMatrix[x, y];
            }
        }
        //-------------------------------------------------------------------------------------------------------
    }
    //-------------------------------------------------------------------------------------------------------
}
