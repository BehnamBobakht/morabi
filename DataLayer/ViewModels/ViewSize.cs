using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewSize
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public string SyncId { get; set; }
        public ViewProductType ProductType { get; set; }
        public bool IsSelected { get; set; }
        public string Name { get; set; }

        public ViewSize() { }
        public ViewSize(Size size, bool extra)
        {
            if (size != null)
            {
                this.Id = size.ID;
                this.Name = size.Name;
                if (size.ProductTypeId != null && extra == true)
                {
                    this.SyncId = size.SyncId;
                    this.ProductType = new ViewProductType()
                    {
                        Id = size.ProductTypeId.ToString(),
                        Name = size.ProductType.Name
                    };
                }
            }
        }
        public ViewSize(Size size, int index, string MaxZero)
        {
            this.Id = size.ID;
            this.SyncId = size.SyncId;
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.Name = size.Name;
            if (size.ProductTypeId != null)
            {
                this.ProductType = new ViewProductType()
                {
                    Id = size.ProductTypeId.ToString(),
                    Name = size.ProductType.Name
                };
            }
        }
    }
}
