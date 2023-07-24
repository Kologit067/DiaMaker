﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Interfaces
{
    //-------------------------------------------------------------------------------------------------------
    // Interface IVertex
    //-------------------------------------------------------------------------------------------------------
    public interface IVertex
    {
        string Name { get;}
        List<int> EndPoints
        {
            get;
        }
    }
    //-------------------------------------------------------------------------------------------------------
}
