using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewSearchCompany
    {

        public ViewSearchProduct SearchProduct { get; set; }
        public ProductCompany Company { get; set; }
        public ViewSearchCompany() { }


    }
}
