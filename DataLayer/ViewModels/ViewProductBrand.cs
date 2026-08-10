using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewProductBrand
    {
        public string Id { get; set; }
        public string RowId { get; set; }
        public string SyncId { get; set; }
        public ViewProductType ProductType { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public bool IsPublic { get; set; }
        public DateTime? SyncDatetime { get; set; }
        public string ThemeColor { get; set; }
        public string Description { get; set; }
        public int? ShowNumber { get; set; }
        public bool Active { get; set; }

        public ViewProductBrand() { }

        public ViewProductBrand(ProductBrand productBrand, bool extra)
        {
            if (productBrand != null)
            {
                this.Id = productBrand.ID.ToString();
                this.Name = productBrand.Name;
                this.IsPublic = productBrand.IsPublic;
            }
        }

        public ViewProductBrand(ProductBrand productBrand, int index, string MaxZero)
        {
            this.Id = productBrand.ID.ToString();
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.SyncId = productBrand.SyncId;
            this.Name = productBrand.Name;
            this.ShowNumber = productBrand.ShowNumber;
            this.IsPublic = productBrand.IsPublic;
            if (productBrand.ProductTypeId != null)
                this.ProductType = new ViewProductType(productBrand.ProductType);
        }
    }
}
