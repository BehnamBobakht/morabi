using DataLayer.Entities;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Schedules
{
    public class Sync_ShrinkDatabase : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            if (Sync_Base.Is_ShrinkDatabase_Syncing == false)
            {
                Sync_Base.Is_ShrinkDatabase_Syncing = true;
                try
                {
                    DatabaseEntities _context = new DatabaseEntities();
                    string DataBaseName = _context.Database.Connection.Database;
                    int result = 0;

                    StringBuilder Query = new StringBuilder();
                    Query.AppendLine("USE " + DataBaseName + " ");
                    Query.AppendLine("ALTER DATABASE [" + DataBaseName + "] ");
                    Query.AppendLine("SET RECOVERY SIMPLE ");
                    result = _context.Database.ExecuteSqlCommand(Query.ToString(), new object[0]);
                    
                    Query.Clear();
                    Query.AppendLine("USE " + DataBaseName + " ");
                    Query.AppendLine("ALTER DATABASE [" + DataBaseName + "] ");
                    Query.AppendLine("SET RECOVERY FULL");
                    result = _context.Database.ExecuteSqlCommand(Query.ToString(), new object[0]);

                    _context.Dispose();
                }
                catch (Exception ex) { string a = ex.Message;  }
                Sync_Base.Is_ShrinkDatabase_Syncing = false;
            }
        }
    }
}
