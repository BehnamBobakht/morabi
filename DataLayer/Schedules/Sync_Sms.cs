using DataLayer.Entities;
using DataLayer.Enumarables;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Schedules
{
    public class Sync_Sms : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            if (Sync_Base.Is_Sms_Syncing == false)
            {
                Sync_Base.Is_Sms_Syncing = true;
                UnitOfWork _context = new UnitOfWork();
                _context.Sms.SendCenter();
                _context.Dispose();
                Sync_Base.Is_Sms_Syncing = false;
            }
        }
    }
}
