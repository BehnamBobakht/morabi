using DataLayer.Entities;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Schedules
{
    public class Sync_Alexa : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            if (Sync_Base.Is_Alexa_Syncing == false)
            {
                Sync_Base.Is_Alexa_Syncing = true;
                UnitOfWork _context = new UnitOfWork();
                _context.AlexaRank.CheckAlexaRank(_context);
                _context.Dispose();
                Sync_Base.Is_Alexa_Syncing = false;
            }
        }
    }
}
