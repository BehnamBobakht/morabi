using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataLayer.Repositories
{
    public class Entity_ShippingCity : BaseRepository<ShippingCity>
    {
        DatabaseEntities _context;
        UnitOfWork _contextOf=new UnitOfWork();
        public Entity_ShippingCity(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public ViewShippingCity GetShippingPrice(int? storeId, AccountAddress address = null)
        {
            var listModel = new List<ShippingCity>();
            var model = new ShippingCity();
            listModel = _context.ShippingCity.Where(s => s.StoreId == storeId).ToList();
            if (address == null)
            {
                var location = Base.BaseWebsite.CurrentLocation;
                if (listModel != null)
                {

                    model = listModel.Where(s => s.StateId == location.StateId).FirstOrDefault();
                    if (model == null)
                    {
                        model = listModel.Where(s => s.StateId == 0).FirstOrDefault();
                    }
                    else
                    {
                        model = listModel.Where(s => (s.CityId == location.CityId && s.StateId == location.StateId) || (s.CityId == 0 && s.StateId == location.StateId)).FirstOrDefault();
                        if (model == null)
                        {
                            model = listModel.Where(s => s.CityId == 0 && s.StateId == 0).FirstOrDefault();
                        }
                    }
                }
            }
            else
            {
                if (listModel != null)
                {

                    model = listModel.Where(s => s.StateId == address.StateId).FirstOrDefault();
                    if (model == null)
                    {
                        model = listModel.Where(s => s.StateId == 0).FirstOrDefault();
                    }
                    else
                    {
                        model = listModel.Where(s => (s.CityId == address.CityId && s.StateId == address.StateId) || (s.CityId == 0 && s.StateId == address.StateId)).FirstOrDefault();
                        if (model == null)
                        {
                            model = listModel.Where(s => s.CityId == 0 && s.StateId == 0).FirstOrDefault();
                        }
                    }
                }
            }
           
            if (model != null)
            {
               
                return new ViewShippingCity(model);
            }
               
            else
                return new ViewShippingCity();
        }

    }
}
