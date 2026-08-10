using DataLayer.Base;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewProductSubCategory
    {
        public string Id { get; set; }
        public string RowId { get; set; }
        public int? Priority { get; set; }
        public string SyncId { get; set; }
        public ViewProductCategory Category { get; set; }
        public string Name { get; set; }
        public ViewPicture Picture { get; set; }
        public DateTime? SyncDatetime { get; set; }
        public DateTime? CreateDatetime { get; set; }
        public DateTime? UpdateDatetime { get; set; }

        public ViewProductSubCategory() { }

        public ViewProductSubCategory(ProductSubCategory subcategory, bool? extra = true)
        {
            if (subcategory != null)
            {
                this.Id = subcategory.ID.ToString();
                this.Name = subcategory.Name;
                this.Priority= subcategory.Priority;
                this.Picture = subcategory.Picture!=null? new ViewPicture()
                {
                    Id = subcategory.Picture!=null ? subcategory.Picture.ID:0,
                    Url = subcategory.Picture != null ? subcategory.Picture.GetUrl() : null
                }:null;
                if (extra == true)
                {
                    this.Category = new ViewProductCategory(subcategory.ProductCategory);
                }
            }
        }

        public ViewProductSubCategory(ProductSubCategory subcategory, int index, string MaxZero)
        {
            this.Id = subcategory.ID.ToString();
            this.SyncId = subcategory.SyncId;
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.Name = subcategory.Name;
            this.Priority = subcategory.Priority;
            this.Picture = subcategory.Picture != null ? new ViewPicture()
            {
                Id = subcategory.Picture.ID,
                Url = subcategory.Picture.GetUrl()
            }:null;
            this.Category = new ViewProductCategory(subcategory.ProductCategory);
        }
    }
}
