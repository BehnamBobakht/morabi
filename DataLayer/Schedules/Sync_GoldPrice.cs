using DataLayer.Entities;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Schedules
{
    public class Sync_GoldPrice : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            if (Sync_Base.Is_GoldPrice_Syncing == false)
            {
                Sync_Base.Is_GoldPrice_Syncing = true;
                UnitOfWork _context = new UnitOfWork();
                _context.Product.UpdateAllProductPrice(_context);
                _context.Dispose();
                Sync_Base.Is_GoldPrice_Syncing = false;
            }
        }
    }
}
