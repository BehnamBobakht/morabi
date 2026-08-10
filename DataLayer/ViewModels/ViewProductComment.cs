using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewProductComment
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public ViewProduct Product { get; set; }
        public ViewAccount Account { get; set; }
        public ViewProductComment Parent { get; set; }
        public string NameFamily { get; set; }
        public string Body { get; set; }
        public string EmailAddress { get; set; }
        public string AnswerString { get; set; }
        public bool? Approved { get; set; }
        public DateTime Datetime { get; set; }
        public DateTime? AnswerDatetime { get; set; }
        public double? Rate { get; set; }

        public ViewProductComment() { }
        
        public ViewProductComment(ProductComment comment, int index, string MaxZero)
        {
            this.Id = comment.ID;
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.Product = new ViewProduct() { Name = comment.NameFamily };
            this.Datetime = comment.Datetime;
            this.Body = comment.Body;
            this.Approved = comment.Approved;
            this.Rate = comment.Rate;
            if (comment.AccountId != null)
            {
                this.Account = new ViewAccount(comment.Account);
                this.NameFamily = comment.Account.FullName;
                this.EmailAddress = comment.Account.Email;
            }
            else
            {
                this.NameFamily = comment.NameFamily;
                this.EmailAddress = comment.EmailAddress;
            }
        }
    }
}
