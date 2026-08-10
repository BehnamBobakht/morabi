using DataLayer.Base;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewStoreComment
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public ViewStore Store { get; set; }
        public ViewPicture Picture { get; set; }
        public ViewAccount Account { get; set; }
        public ViewStoreComment Parent { get; set; }
        public string NameFamily { get; set; }
        public string Body { get; set; }
        public string EmailAddress { get; set; }
        public string AnswerString { get; set; }
        public bool? Approved { get; set; }
        public DateTime Datetime { get; set; }
        public DateTime? AnswerDatetime { get; set; }
        public double? Rate { get; set; }

        public ViewStoreComment() { }
        
        public ViewStoreComment(StoreComment comment, int index, string MaxZero)
        {
            this.Id = comment.ID;
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.Store = new ViewStore() { Name = comment.Store.Name };
            this.Datetime = comment.Datetime;
            this.Body = comment.Body;
            this.Approved = comment.Approved;
            this.Rate = comment.Rate;
            this.Picture= this.Picture != null
                ? new ViewPicture()
                {
                    Id = comment.Store.Picture.ID,
                    Url = comment.Store.Picture.GetUrl()
                }
                : null;
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
