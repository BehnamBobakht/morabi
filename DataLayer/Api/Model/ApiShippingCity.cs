//using DataLayer.Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace DataLayer.Api.Model
//{
//    public class ApiShippingCity : ApiResponse
//    {
//        public static ApiResult Post(UnitOfWork _context, int storeId, int stateId, int cityId, int countryId, int sendTime, double sendPrice)
//        {
//            if (stateId == (int)DataLayer.Enumarables.ShippingCity.All)
//            {
//                var listShipping=_context.ShippingCity.Where(s => s.StoreId == storeId).ToList();
//                foreach(var item in listShipping)
//                {
//                    _context.Shipping.Delete(item);
//                }
//                _context.Save();
//            }
//            if (cityId == (int)DataLayer.Enumarables.ShippingCity.All)
//            {
//                var listShipping = _context.ShippingCity.Where(s => s.StoreId == storeId && s.StateId==stateId).ToList();
//                foreach (var item in listShipping)
//                {
//                    _context.Shipping.Delete(item);
//                }
//                _context.Save();
//            }
//            return Get(_context, accountId);
//        }
//    }
//}
