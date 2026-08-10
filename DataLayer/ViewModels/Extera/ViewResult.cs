using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Enumarables;

namespace DataLayer.ViewModels
{

    public class ResultVm<TView> 
    {
        public ResultVm()
        {
            Status = ResultStatus.None;
        }
        public TView Model { get; set; }
        public ResultStatus Status { get; set; }
        public string Message { get; set; }
    }
}
