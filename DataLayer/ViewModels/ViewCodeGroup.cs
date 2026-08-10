using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewCodeGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }

        public ViewCodeGroup() { }

        public ViewCodeGroup(CodeGroup group)
        {
            this.Id = group.ID;
            this.Label = group.Label;
            this.Name = group.Name;
        }
    }
}
