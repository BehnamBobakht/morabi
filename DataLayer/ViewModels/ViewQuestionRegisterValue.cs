using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewQuestionRegisterValue
    {
        public int Id { get; set; }
        public int FieldId { get; set; }
        public int ? AccountId { get; set; }
        public int ? ItemId { get; set; }
        public int ? FileId { get; set; }
        public string Value { get; set; }


        public ViewQuestionRegisterValue() { }

        public ViewQuestionRegisterValue(QuestionRegisterValue address)
        {
            if (address != null)
            {
                this.Id = address.ID;
                this.FieldId = address.FieldId;
                this.AccountId = address.AccountId;
                this.ItemId = address.ItemId;
                this.FileId = address.FileId;
                this.Value = address.Value;

            }
        }
    }
}
