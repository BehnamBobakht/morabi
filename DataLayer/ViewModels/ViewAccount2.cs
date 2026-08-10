using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Base;

namespace DataLayer.ViewModels
{
    public class ViewAccount2
    {
        public int Id { get; set; }
       
        public string Price { get; set; }
        public string QuestionAnswer { get; set; }

        public string FullName { get; set; }
        public string FullNameEn { get; set; }

     

        public string Mobile { get; set; }
        public string Email { get; set; }

        public string NationalCode { get; set; }
        public string EnterCode { get; set; }
        public string Address { get; set; }

        public string PictureUrl { get; set; }
        public DateTime ? Date { get; set; }




        public ViewAccount2() { }


    }
}
