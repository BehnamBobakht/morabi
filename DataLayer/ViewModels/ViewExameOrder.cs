using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewExameOrder
    {

        public string Name { get; set; }
       
        public ViewExameOrder() { }

        public ViewExameOrder(ExameCourse orderProduct)
        {
            this.Name = orderProduct.Name;
            
        }

    }
}
