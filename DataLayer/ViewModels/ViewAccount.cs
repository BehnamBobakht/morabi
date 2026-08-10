using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Base;

namespace DataLayer.ViewModels
{
    public class ViewAccount
    {
        public int Id { get; set; }
        public DateTime ? Datetime { get; set; }
        public int? PictureID { get; set; }
        public int? NationalPictureId { get; set; }
        public int? EduactionPictureId { get; set; }
        public int? PassportPictureId { get; set; }
        public string Price { get; set; }
        public string QuestionAnswer { get; set; }

        public string LatName { get; set; }
        public string LatNameEn { get; set; }

        public int? ShenasnamehPictureId { get; set; }
        public int? InsurancePictureId { get; set; }
        public int? Coaching2PictureId { get; set; }
        public int? Coaching3PictureId { get; set; }

        public string RowId { get; set; }
        public string FullName { get; set; }
        public string FullNameEn { get; set; }
        public string FatherName { get; set; }
        public string FatherNameEn { get; set; }

        public string Password { get; set; }
        public string Email { get; set; }

        public DateTime? BirthDay { get; set; }
        public string FaBirthDay { get; set; }

        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string NationalCode { get; set; }
        public string Job { get; set; }
        public string Company { get; set; }
        public ViewCode Type { get; set; }
        public string Address { get; set; }
        public string CompanyNo { get; set; }
        public string ReagentName { get; set; }
        public string Agent { get; set; }
        public string AgentPhone { get; set; }
        public string Description { get; set; }
        public string ZipCode { get; set; }

        public Guid ? UniqueId { get; set; }
        public Guid Key { get; set; }
        public bool IsMale { get; set; }
        public int StoreId { get; set; }
        public ViewStore Store { get; set; }
        public string ReagentCode { get; set; }
        public string Website { get; set; }
        public List<ViewAccountAddress> AddressList { get; set; }

        public List<ViewQuestionRegisterValue> QuestionRegisterValue { get; set; }

        public string Fax { get; set; }
        public string Instagram { get; set; }
        public string Telegram { get; set; }
        public string WhatsApp { get; set; }
        public string Location { get; set; }
        public string UserName { get; set; }
        public bool ChangeUserName { get; set; }
        public string Linkedin { get; set; }
        public string Facebook { get; set; }
        public string Profession { get; set; }
        public string AboutMe { get; set; }
        public string FavoritTitle { get; set; }
        public string FavoritLink { get; set; }

        public int PercentDiscount { get; set; }
        public Boolean? bothEmailMobile { get; set; }
        public string CardNumber { get; set; }
        public string Sheba { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }
        public int? CountryId { get; set; }
        public string PictureUrl { get; set; }
        public string NationalPictureUrl { get; set; }
        public string EduactionPictureUrl { get; set; }
        public string PassportPictureUrl { get; set; }
        public string ShenasnamehPictureUrl { get; set; }
        public string InsurancePictureUrl { get; set; }
        public string Coaching2PicturerUrl { get; set; }
        public string Coaching3PicturerUrl { get; set; }

        public string CreateDate { get; set; }
        public bool SpacialAccount { get; set; }

        public ViewAccount() { }

        public ViewAccount(Account account, AccountAddress address = null)
        {
            if (account != null)
            {
                this.Id = account.ID;
                this.Address = account.Address;
                this.Agent = account.Agent;
                this.AgentPhone = account.AgentPhone;
                this.Company = account.Company;
                this.CompanyNo = account.CompanyNo;
                this.Email = account.Email;
                this.FullName = account.FullName;
                this.IsMale = account.IsMale.GetValueOrDefault();
                this.Job = account.Job;
                this.Mobile = account.Mobile;
                this.NationalCode = account.NationalCode;
                this.Phone = account.Phone;
                this.ReagentName = account.ReagentName;
                this.Type = new ViewCode(account.Code);
                this.UniqueId = account.UniqueId;
                this.ZipCode = account.ZipCode;
                this.QuestionAnswer = account.QuestionAnswer;

                this.Description = account.Description;
                this.ReagentCode = account.ReagentCode;
                this.StoreId = account.Store.Any() ? account.Store.First().ID : 0;
                this.Store = account.Store.Any() ? new ViewStore(account.Store.First(), false, address) : null;
            
                //foreach(var item in account.AccountAddress)
                //{
                //    this.AddressList.Add(new ViewAccountAddress(item));
                //}
                this.Website = account.Website;
                this.WhatsApp = account.WhatsApp;
                this.Fax = account.Fax;
                this.Instagram = account.Instagram;
                this.UserName = account.UserName;
                this.Telegram = account.Telegram;
                this.UserName = account.UserName;
                this.ChangeUserName = account.ChangeUserName;
                this.Linkedin = account.Linkedin;
                this.CardNumber = account.CardNumber;
                this.Sheba = account.Sheba;
                this.PictureID = account.PictureId;
                this.BirthDay = account.BirthDay;
                this.StateId = account.StateId != null ? account.StateId : 0;
                this.CityId = account.CityId != null ? account.CityId : 0;
                this.CountryId = account.CountryId != null ? account.CountryId : 0;
                this.PictureUrl = account.Picture.GetUrl();
                this.NationalPictureUrl = account.Picture1.GetUrl();
                this.PassportPictureUrl = account.Picture3.GetUrl();
                this.PassportPictureId = account.PassportPictureId;
                this.ShenasnamehPictureId = account.ShenasnamehPictureId;
                this.Coaching2PictureId = account.Coaching2PictureId;
                this.Coaching3PictureId = account.Coaching3PictureId;
                this.EduactionPictureId = account.EduactionPictureId;
                this.BirthDay = account.BirthDay;
                this.PictureID = account.PictureId;
                this.NationalPictureId = account.NationalPictureId;
                this.FullNameEn = account.FullNameEn;
                this.FatherName = account.FatherName;
                this.FatherNameEn = account.FatherNameEn;
                this.LatName = account.LatName;
                this.LatNameEn = account.LatNameEn;
                this.CreateDate = account.CreateDatetime.ToPersian();
                this.SpacialAccount = account.SpacialAccount;
                List<ViewQuestionRegisterValue> model = new List<ViewQuestionRegisterValue>();
                foreach (var item in account.QuestionRegisterValue)
                {
                    model.Add(new ViewQuestionRegisterValue(item));
                  
                }
                this.QuestionRegisterValue= model;
            }
        }
        public ViewAccount(Account account, int index, string MaxZero)
        {
            if (account != null)
            {
                
                this.CreateDate = account.CreateDatetime.ToPersian();
                this.Id = account.ID;
                this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
                this.Email = account.Email;
                this.FullName = account.FullName;
                this.LatName = account.LatName;
                this.LatNameEn = account.LatNameEn;
                this.QuestionAnswer = account.QuestionAnswer;
                this.Mobile = account.Mobile;
                this.UserName = account.UserName;
                this.CardNumber = account.CardNumber;
                this.BirthDay = account.BirthDay;
                this.Sheba = account.Sheba;
                this.PictureID = account.PictureId;
                this.StateId = account.StateId != null ? account.StateId : 0;
                this.CityId = account.CityId != null ? account.CityId : 0;
                this.CountryId = account.CountryId != null ? account.CountryId : 0;
                this.PictureUrl = account.Picture.GetUrl();
                this.SpacialAccount = account.SpacialAccount;
                this.NationalPictureUrl = account.Picture1.GetUrl();
                this.NationalPictureId = account.NationalPictureId;
                this.FullNameEn = account.FullNameEn;
                this.FatherName = account.FatherName;
                this.FatherNameEn = account.FatherNameEn;
                this.ZipCode = account.ZipCode;
                this.Address = account.Address;
                this.NationalCode = account.NationalCode;



            }
        }
    }
}
