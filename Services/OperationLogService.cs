using Admin.Extensions;
using Admin.IService;
using Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Services
{
    public class OperationLogService : IOperationLogService
    {
        private readonly AdminContext _context;
        public OperationLogService()
        {
            _context = XWFHttpContext.ServiceProvider.GetService(typeof(AdminContext)) as AdminContext;
        }
        public void Record(OperationLog log)
        {
            _context.OperationLog.Add(log);
            _context.SaveChangesAsync();
        }
    }
}
