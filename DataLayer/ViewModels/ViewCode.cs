using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewCode
    {
        public int Id { get; set; }
        public ViewCodeGroup CodeGroup { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }

        public ViewCode() { }
        public ViewCode(Code code, bool hasGroup = false)
        {
            if (code != null)
            {
                this.Id = code.ID;
                this.Label = code.Label;
                this.Name = code.Name;
                if (hasGroup)
                    this.CodeGroup = new ViewCodeGroup(code.CodeGroup);
            }
        }
    }
}
