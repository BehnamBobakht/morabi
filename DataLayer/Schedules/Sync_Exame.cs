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
    public class Sync_Exame : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            
            if (Sync_Base.Is_Exame_Syncing == false)
            {
                Sync_Base.Is_Exame_Syncing = true;
                UnitOfWork _context = new UnitOfWork();
                //_context.Exame.CheckActive();
                _context.ExameAccountAnswer.AddDetails();

                //_context.Exame.CheckTotalLevel();

                _context.Dispose();
                Sync_Base.Is_Exame_Syncing = false;
            }
        }
    }
}
