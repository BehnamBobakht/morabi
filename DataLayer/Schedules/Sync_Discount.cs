using DataLayer.Api;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using DataLayer.Base;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Schedules
{
    public class Sync_Discount : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            if (Sync_Base.Is_Discount_Syncing == false)
            {
                Sync_Base.Is_Discount_Syncing = true;
                try
                {
                    SyncLog log = new SyncLog()
                    {
                        Name = "Discount",
                        StartDatetime = DateTime.Now
                    };
                    UnitOfWork _context = new UnitOfWork();
                    _context.Discount.SyncDiscount(_context);
                }
                catch (Exception ex)
                {
                    string a = ex.Message;
                }
                Sync_Base.Is_Discount_Syncing = false;
            }
        }
    }
}
