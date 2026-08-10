using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewProductCategory
    {
        public string Id { get; set; }
        public int? Priority { get; set; }
        public string RowId { get; set; }
        public string SyncId { get; set; }
        public ViewProductType ProductType { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public DateTime? SyncDatetime { get; set; }
        public DateTime? CreateDatetime { get; set; }
        public DateTime? UpdateDatetime { get; set; }
        public ViewPicture Picture { get; set; }
        public List<ViewProductSubCategory> SubCategories { get; set; }

        public ViewProductCategory() { }

        public ViewProductCategory(ProductCategory category)
        {
            if (category != null)
            {
                this.Id = category.ID.ToString();
                this.Name = category.Name;
                this.FullName = category.ProductType.Name + "-" + category.Name;

                this.ProductType = new ViewProductType(category.ProductType);
                this.Priority = category.Priority;
            }
        }

        public ViewProductCategory(ProductCategory category, int index, string MaxZero)
        {
            this.Id = category.ID.ToString();
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.Name = category.Name;
            this.FullName = category.ProductType.Name + "-" + category.Name;

            this.SyncId = category.SyncId;
            this.Priority = category.Priority;
            this.ProductType = new ViewProductType(category.ProductType);
        }
    }
}
