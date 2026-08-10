using DataLayer.Entities;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Api.Model
{
    public class ApiProductCompare : ApiResponse
    {
        public static ApiResult Get(UnitOfWork _context, int[] productId = null)
        {
            List<ViewProduct> list = new List<ViewProduct>();
            foreach (int item in productId)
            {
                ViewProduct product = list.FirstOrDefault(p => p.Id == item.ToString());
                if (product == null)
                {
                    Product entity = _context.Product.GetById(item);
                    if (entity != null)
                    {
                        list.Add(entity.ToApi());
                    }
                }
            }
            return CreateSuccessResult(list);
        }
    }
}
