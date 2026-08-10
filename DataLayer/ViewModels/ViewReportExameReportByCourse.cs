using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewReportExameReportByCourse
    {
        public int ? ProductId { get; set; }
        public string  Number { get; set; }

        public ExameReportByCourse Course { get; set; }
       


        public ViewReportExameReportByCourse() { }

       
    }
}
