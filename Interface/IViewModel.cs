﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Interface
{
    public interface IViewModel<T>:IDependency
    {
        T GetEntity();
    }
}
