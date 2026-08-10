using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewExameAccountAnswerDetails
    {
        public string CategoryName { get; set; }
        public string AnswerDetails { get; set; }

        public int TrueId { get; set; }
        public int ID { get; set; }

        public int? QuestionId { get; set; }

        public bool? IsTrue { get; set; }


        public ViewExameAccountAnswerDetails() { }


    }
}
