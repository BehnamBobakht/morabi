using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_Website : BaseRepository<Website>
    {
        DatabaseEntities _context;
        public Entity_Website(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public Website GetFirstByType(Enum_Code type)
        {
            string typeString = type.ToString();
            return _context.Website.FirstOrDefault(p =>
                p.Code.Label == typeString
            );
        }

        public List<Website> GetAllByType(params Enum_Code[] types)
        {
            string[] typeString = new string[5];
            for (int i = 0; i < 5; i++)
            {
                if (types.Length > i)
                    typeString[i] = types[i].ToString();
                else
                    typeString[i] = "";
            }

            string type_0 = typeString[0];
            string type_1 = typeString[1];
            string type_2 = typeString[2];
            string type_3 = typeString[3];
            string type_4 = typeString[4];

            return _context.Website.Where(p =>
                p.Code.Label == type_0 ||
                p.Code.Label == type_1 ||
                p.Code.Label == type_2 ||
                p.Code.Label == type_3 ||
                p.Code.Label == type_4
            ).ToList();
        }
    }
}
