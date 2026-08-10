using DataLayer.Entities;
using DataLayer.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.ViewModels;
using DataLayer.Api.Model;

namespace DataLayer.Base
{
    public class BaseDelivery
    {
        public static ResultVm<List<DeliveryVm>> GetPossibleDeliveries(int? storeId, double price, int cityId, int stateId,UnitOfWork _context=null)
        {
            var result = new ResultVm<List<DeliveryVm>>();
            if (_context == null)
                _context = new UnitOfWork();
            result.Model = _context.Delivery.GetForStore(storeId: storeId, price: price, cityId: cityId, stateId: stateId);
            if (result.Model.Any())
            {
                result.Status = Enumarables.ResultStatus.Success;
            }
            else
            {
                result.Status = Enumarables.ResultStatus.Error;
            }
            _context.Dispose();
            return result;
        }
     


    }
}
