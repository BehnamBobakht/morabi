using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewBank
    {
        public int Id { get; set; }
        public int RowId { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public string PictureUrl { get; set; }

        public ViewBank() { }

        public ViewBank(Bank bank)
        {
            this.Id = bank.ID;
            this.Name = bank.Name;
            this.Label = bank.Label;
            this.PictureUrl = bank.PictureUrl;
        }
    }
}
