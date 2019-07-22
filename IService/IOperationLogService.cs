using Admin.Interface;
using Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.IService
{
    public interface IOperationLogService:IDependency
    {
        void Record(OperationLog log);
    }
}
