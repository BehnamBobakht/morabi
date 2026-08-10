using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;

namespace DataLayer.Repositories
{
    public class Entity_SliderLanguage : BaseRepository<SliderLanguage>
    {

        private DatabaseEntities _context;
        public Entity_SliderLanguage(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
               
        public void DeleteBySliderId(int sliderID)
        {
            List<SliderLanguage> list = GetAllBySliderId(sliderID);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }

        public List<SliderLanguage> GetAllBySliderId(int sliderID)
        {
            return _context.SliderLanguage.Where(p =>p.SliderId == sliderID).ToList();
        }

    }
}
