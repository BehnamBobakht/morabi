using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewProductVisitModel
    {
        public ViewAccount Account { get; set; }
        public ViewProduct Product { get; set; }



        public ViewProductVisitModel(ProductVisit productVisit)
        {
            if (productVisit != null)
            {
              
                this.Account = new ViewAccount(productVisit.Account);
                this.Product = new ViewProduct(productVisit.Product);

            }
        }
        public ViewProductVisitModel()
        {

        }
    }
}
