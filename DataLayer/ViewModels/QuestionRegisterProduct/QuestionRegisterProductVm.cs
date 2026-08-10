using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class QuestionRegisterProductVm
    {
        public int ID { get; set; }
        public int FieldId { get; set; }
        public int ProductId { get; set; }
        public QuestionRegisterVm QuestionRegister { get; set; }

    }
}
