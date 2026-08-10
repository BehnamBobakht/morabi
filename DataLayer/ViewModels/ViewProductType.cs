using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    /// <summary>
    /// نوع محصول
    /// </summary>
    public class ViewProductType
    {
        public string Id { get; set; }
        public int? Priority { get; set; }
        public string SyncId { get; set; }
        public string RowId { get; set; }
        public string Name { get; set; }
        public bool IsService { get; set; }
        public DateTime? SyncDatetime { get; set; }
        public DateTime? CreateDatetime { get; set; }
        public DateTime? UpdateDatetime { get; set; }
        public ViewPicture Picture { get; set; }
        public List<ViewProductCustomField> CustomFields { get; set; }
        public List<ViewProductCategory> ProductCategories { get; set; }

        public ViewProductType() { }

        public ViewProductType(ProductType productType, int index, string MaxZero)
        {
            this.Id = productType.ID.ToString();
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.Name = productType.Name;
            this.IsService = productType.IsService;
            this.Priority = productType.Priority;

            this.SyncId = productType.SyncId;
        }

        public ViewProductType(ProductType productType)
        {
            if (productType != null)
            {
                this.Id = productType.ID.ToString();
                this.Name = productType.Name;
                this.SyncId = productType.SyncId;
                this.IsService = productType.IsService;
                this.Priority = productType.Priority;

            }
        }
    }
}
