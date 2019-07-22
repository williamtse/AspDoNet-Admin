using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Admin.Models;

namespace Admin.Controllers
{
    public class OperationLogsController : Controller
    {
        private readonly AdminContext _context;

        public OperationLogsController(AdminContext context)
        {
            _context = context;
        }

        // GET: OperationLogs
        public async Task<IActionResult> Index()
        {
            return View(await _context.OperationLog.ToListAsync());
        }

        // GET: OperationLogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var operationLog = await _context.OperationLog
                .FirstOrDefaultAsync(m => m.ID == id);
            if (operationLog == null)
            {
                return NotFound();
            }

            return View(operationLog);
        }

        // POST: OperationLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var operationLog = await _context.OperationLog.FindAsync(id);
            _context.OperationLog.Remove(operationLog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OperationLogExists(int id)
        {
            return _context.OperationLog.Any(e => e.ID == id);
        }
    }
}
