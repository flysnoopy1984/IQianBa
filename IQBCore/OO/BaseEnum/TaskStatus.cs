﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.OO.BaseEnum
{
    public enum UserTaskStatus
    {
        /// <summary>
        /// 一接单就开始了
        /// </summary>
        Created = 0,

        Process = 2,

        Stop = 4,

        GetError = -1,

        Complete = 100,

        ALL = 999,

    }
}
