using DataLayer.Entities;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Schedules
{
    public class Sync_OrderToArchive : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            if (Sync_Base.Is_OrderToArchive_Syncing == false)
            {
                Sync_Base.Is_OrderToArchive_Syncing = true;
                UnitOfWork _context = new UnitOfWork();
                _context.AccountOrder.ChangeToArchive(_context);
                _context.Dispose();
                Sync_Base.Is_OrderToArchive_Syncing = false;
            }
        }
    }
}
