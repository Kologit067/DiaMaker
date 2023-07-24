﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalPositionLib.Matrix
{
    //-------------------------------------------------------------------------------------------------------
    // interface IMatrixOfDistance
    //-------------------------------------------------------------------------------------------------------
    public interface IMatrixOfDistance
    {
        //-------------------------------------------------------------------------------------------------------
        int Length
        {
            get;
        }
        //-------------------------------------------------------------------------------------------------------
        int this[int x, int y]
        {
            get;
        }
        //-------------------------------------------------------------------------------------------------------
    }
    //-------------------------------------------------------------------------------------------------------
}