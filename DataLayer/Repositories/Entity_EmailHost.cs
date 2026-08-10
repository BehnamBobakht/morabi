using DataLayer.Entities;
using DataLayer.ViewModels;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_EmailHost : BaseRepository<EmailHost>
    {
        DatabaseEntities _context;
        public Entity_EmailHost(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<ViewEmailHost> GetAllView()
        {
            List<EmailHost> list = _context.EmailHost.ToList();
            List<ViewEmailHost> Output = new List<ViewEmailHost>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewEmailHost(list[i], i, MaxZero));
            }
            return Output;
        }
    }
}
