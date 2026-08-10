using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.ViewModels;

namespace DataLayer.Repositories
{
    public class Entity_StoreCompetitiveFeature : BaseRepository<StoreCompetitiveFeature>
    {
        private DatabaseEntities _context;
        public Entity_StoreCompetitiveFeature(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<ViewStoreCompetitiveFeature> GetByStoreId(int? storeId)
        {
            if (storeId == null)
            {
                return null;
            }
            var liststore = _context.StoreCompetitiveFeature.Where(s => s.StoreId == storeId).ToList();
            var listCompetitive = _context.CompetitiveFeature.ToList();
            List<ViewStoreCompetitiveFeature> result = new List<ViewStoreCompetitiveFeature>();

            foreach (var item in listCompetitive)
            {
                bool status = false;
                if (liststore.FirstOrDefault(s => s.CompetitiveFeatureId == item.ID) != null)
                {
                    status = true;
                }
                result.Add(new ViewStoreCompetitiveFeature()
                {
                    Status = status,
                    Text = item.TexCompetitiveFeature,
                    Id = item.ID

                }
                );
            }

            return result;
        }

        public bool UpdateStoreCompetitive(List<ViewStoreCompetitiveFeature> competitive, int storeId)
        {
            var list = _context.StoreCompetitiveFeature.Where(s => s.StoreId == storeId).ToList();
            try
            {
                for (int i = 0; i < list.Count; i++)
                {
                    Delete(list[i]);
                }
                foreach (ViewStoreCompetitiveFeature item in competitive)
                {
                    if (item.Status == true)
                    {
                        StoreCompetitiveFeature entity = new StoreCompetitiveFeature()
                        {
                            StoreId = storeId,
                            CompetitiveFeatureId = item.Id,
                        };
                        Insert(entity);
                    }

                }
                Save();
                return true;
            }
            catch
            {
                return false;
            }
           
           
           
        }
    }
}
