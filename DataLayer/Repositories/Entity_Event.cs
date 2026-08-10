using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_Event : BaseRepository<Event>
    {
        DatabaseEntities _context;
        public Entity_Event(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<Event> GetAllForAccount(int UserId, DateTime StartDate, DateTime EndDate)
        {
            return _context.Event.Where(p => p.UserId == UserId && p.Date >= StartDate && p.Date <= EndDate).ToList();
        }

        public List<Event> GetAllForAccount(int UserId, DateTime Date)
        {
            DateTime StartDate = Date.AddDays(-1);
            DateTime EndDate = Date.AddDays(1);
            List<Event> list = GetAllForAccount(UserId, StartDate, EndDate);
            return list.Where(p => p.Date.DayOfYear == Date.DayOfYear).OrderBy(p => p.StartTime).ToList();
        }
    }
}
