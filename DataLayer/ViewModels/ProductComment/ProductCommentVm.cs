using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ProductCommentVm
    {
        public int ID { get; set; }
        public Nullable<int> UserId { get; set; }
        public int ProductId { get; set; }
        public Nullable<int> AccountId { get; set; }
        public string NameFamily { get; set; }
        public string EmailAddress { get; set; }
        public string Body { get; set; }
        public string AnswerString { get; set; }
        public System.DateTime Datetime { get; set; }
        public Nullable<System.DateTime> AnswerDatetime { get; set; }
        public Nullable<bool> Approved { get; set; }
        public Nullable<double> Rate { get; set; }

    }
}
