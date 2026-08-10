using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewUserStoreComment
    {
        public int ID { get; set; }
        public string RowId { get; set; }
        public string NameStore { get; set; }
        public int StoreId { get; set; }
        public string NameFamily { get; set; }
        public string Body { get; set; }
        public string EmailAddress { get; set; }
        public string AnswerString { get; set; }
        public bool? Approved { get; set; }
        public DateTime Datetime { get; set; }
        public DateTime? AnswerDatetime { get; set; }
        public double? Rate { get; set; }

        public ViewUserStoreComment() { }
        
       
    }
}
