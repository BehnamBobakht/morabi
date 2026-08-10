using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewStoreProduct
    {

        public int ID { get; set; }
        public int StoreId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public int StatusId { get; set; }
        public System.DateTime CreateDate { get; set; }
        public ViewStore Store { get; set; }
        public ViewProduct Product { get; set; }
        public ViewCode Status { get; set; }

        public ViewStoreProduct() { }

        public ViewStoreProduct(StoreProduct storeProduct,bool complate=true,AccountAddress address=null)
        {
            if (storeProduct != null)
            {
                this.ID = storeProduct.ID;
                this.StoreId = storeProduct.StoreId;
                this.ProductId = storeProduct.ProductId;
                this.ProductName = storeProduct.Product.Name;
                this.StatusId = storeProduct.StatusId;
                this.CreateDate = storeProduct.CreateDate;
                if(complate)
                {
                    this.Store = new ViewStore(storeProduct.Store,complate, address);  
   
                    this.Status = new ViewCode(storeProduct.Code);
                this.Product = new ViewProduct(storeProduct.Product);

                }

            }
        }      
    }
}
