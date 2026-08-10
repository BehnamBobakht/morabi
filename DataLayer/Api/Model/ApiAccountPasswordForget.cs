using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.ViewModels;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Api.Model
{
    public class ApiAccountPasswordForget : ApiResponse
    {
        public static ApiResult Post(UnitOfWork _context, Account account)
        {
            if (account == null ||
                (
                    string.IsNullOrEmpty(account.Mobile) &&
                    string.IsNullOrEmpty(account.Email))
                )
                return CreateErrorResult(Enum_Message.REQUIRED_ACCOUNT);
          
            Account entity = null;
            account.Mobile = account.Mobile.Trim().ToEnglishDigit();
            account.Email = account.Email.Trim().ToEnglishDigit();
            if (string.IsNullOrEmpty(account.Mobile) == false)
                entity = _context.Account.GetByMobile(account.Mobile);
            else if (string.IsNullOrEmpty(account.Email) == false)
                entity = _context.Account.GetByEmail(account.Email);

            if (entity == null)
                return CreateErrorResult(Enum_Message.INVALID_ACCOUNT_DATA);
            string pass = BaseRandom.GetRandomNumber(6);
            entity.Password = BaseSecurity.HashMd5(pass);
            _context.Account.Update(entity);
            _context.Save();
            AccountPasswordForget forget = new AccountPasswordForget()
            {
                AccountId = entity.ID,
                Datetime = DateTime.Now,
                EmailKey = Guid.NewGuid().ToString(),
                MobileKey = BaseRandom.GetRandomNumber(),
                //MobileKey="1234",
                IsUsed = false
            };
            _context.AccountPasswordForget.Insert(forget);
            try
            {
                string url = BaseWebsite.ShopUrl;
                if (string.IsNullOrEmpty(url) == false)
                {
                    StringBuilder str = new StringBuilder();
                    //str.Append("لینک بازیابی رمز عبور(وبسایت): ");
                    //str.Append("<br />");
                    //str.Append("<a href='" + url + "recover?k=" + forget.EmailKey + "&a=" + entity.UniqueId + "'>"+ url + "recover?k=" + forget.EmailKey + "&a=" + entity.UniqueId+"</a>");
                    //str.Append("<hr />");
                    //str.Append("کد بازیابی رمز عبور(اپلیکیشن): ");
                    //str.Append("<br />");
                    str.Append("رمز عبور جدید شما در سایت");
                    str.Append(pass);
                    _context.Email.SaveNewEmail(entity.Email, Enum_EmailType.FORGETPASSWORD, str.ToString(), "بازیابی رمز عبور");
                }
                StringBuilder str_1 = new StringBuilder();
                string token_1 = pass;
                string token_2 = "";
                string token_3 = "";
                str_1.AppendLine("رمز عبور جدید شما در سایت");
                str_1.AppendLine(token_1);
                _context.Sms.SaveNewSms(entity.Mobile.ToEnglishDigit(), Enum_SmsType.RECOVER_PASSWORD, str_1.ToString(), token_1, token_2, token_3);
            }
            catch (Exception) { }

            _context.Save();
            return CreateSuccessResult(Enum_Message.SUCCESSFULL_FORGET_PASSWORD);
        }
        public static ApiResult SendEmail(UnitOfWork _context, Account account)
        {
            if (account == null ||
                (
                    string.IsNullOrEmpty(account.Mobile) &&
                    string.IsNullOrEmpty(account.Email))
                )
                return CreateErrorResult(Enum_Message.REQUIRED_ACCOUNT);

            Account entity = null;
            if (string.IsNullOrEmpty(account.Mobile) == false)
                entity = _context.Account.GetByMobile(account.Mobile.Trim().GetEnglish());
            else if (string.IsNullOrEmpty(account.Email) == false)
                entity = _context.Account.GetByEmail(account.Email);

            if (entity == null)
                return CreateErrorResult(Enum_Message.INVALID_ACCOUNT_DATA);
            string pass = BaseRandom.GetRandomNumber(6);
            Account temp = _context.Account.GetByMobile(account.Mobile.Trim().GetEnglish());

            temp.Password = BaseSecurity.HashMd5(pass);
            _context.Account.Update(temp);
            _context.Save();
            AccountPasswordForget forget = new AccountPasswordForget()
            {
                AccountId = entity.ID,
                Datetime = DateTime.Now,
                EmailKey = Guid.NewGuid().ToString(),
                MobileKey = BaseRandom.GetRandomNumber(),
                //MobileKey="1234",
                IsUsed = false
            };
            _context.AccountPasswordForget.Insert(forget);
            try
            {
                EmailAddress address = _context.EmailAddress.FirstOrDefault();
                if (address != null)
                {
                    StringBuilder body = new StringBuilder();
                    body.AppendLine("رمز عبور کاربری شما ویرایش شده است" + "<br />");
                    body.AppendLine( pass + "<br />");
                    string title = "فراموشی رمز عبور";
                    _context.Email.SaveNewEmail(entity.Email, Enum_EmailType.FORGETPASSWORD, body.ToString(), title);
                    _context.Save();
                }
            }
            catch (Exception) { }

            _context.Save();
            return CreateSuccessResult(Enum_Message.SUCCESSFULL_FORGET_PASSWORD_SENDEMAIL);
        }

        public static ApiResult Post_ChangePassword(UnitOfWork _context, AccountPasswordForget forget, string password, string confirm, bool returnAcount = true)
        {
            if (forget == null)
                return CreateErrorResult(Enum_Message.INVALID_DATA);
            if (string.IsNullOrEmpty(password) == true)
                return CreateErrorResult(Enum_Message.REQUIRED_PASSWORD);
            if (string.IsNullOrEmpty(confirm) == true)
                return CreateErrorResult(Enum_Message.REQUIRED_PASSWORD_CONFIRM);
            if (password != confirm)
                return CreateErrorResult(Enum_Message.INVALID_PASSWORD_CONFIRM);
            else if (BaseSecurity.IsValidInput(password, Enum_Validation.PASSWORD) == false)
                return CreateErrorResult(Enum_Message.Validate_ACCOUNT_PASSWORD);

            forget = _context.AccountPasswordForget.GetById(forget.ID);
            forget.IsUsed = true;

            Account account = forget.Account;
            account.Password = BaseSecurity.HashMd5(password);
            _context.Account.Update(account);
            _context.Save();

            return CreateSuccessResult(Enum_Message.SUCCESSFULL_SUBMIT, (returnAcount ? account : null));
        }
    }
}
