using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewColor
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public string SyncId { get; set; }
        public ViewProductType ProductType { get; set; }
        public bool IsSelected { get; set; }
        public string Name { get; set; }
        public string Hex { get; set; }

        public ViewColor() { }
        public ViewColor(Color color, bool extra)
        {
            if (color != null)
            {
                this.Id = color.ID;
                this.Name = color.Name;
                this.Hex = color.HexValue;
                if (color.ProductTypeId != null && extra == true)
                {
                    this.SyncId = color.SyncId;
                    this.ProductType = new ViewProductType()
                    {
                        Id = color.ProductTypeId.ToString(),
                        Name = color.ProductType.Name
                    };
                }
            }
        }
        public ViewColor(Color color, int index, string MaxZero)
        {
            this.Id = color.ID;
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.Name = color.Name;
            this.Hex = color.HexValue;
            this.SyncId = color.SyncId;
            if (color.ProductTypeId != null)
            {
                this.ProductType = new ViewProductType()
                {
                    Id = color.ProductTypeId.ToString(),
                    Name = color.ProductType.Name
                };
            }
        }
    }
}
