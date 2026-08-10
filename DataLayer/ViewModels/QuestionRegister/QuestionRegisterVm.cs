using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class QuestionRegisterVm
    {
        public int ID { get; set; }
        public int TypeId { get; set; }
        public string TypeLabel { get; set; }
        public string Name { get; set; }

        public List<QuestionRegisterItemVm> QuestionRegisterItem { get; set; }
        public QuestionRegisterVm()
        {
            QuestionRegisterItem = new List<QuestionRegisterItemVm>();
        }
    }
}
