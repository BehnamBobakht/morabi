using DataLayer.Base;
using DataLayer.Helpers;
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
    public class ApiContactForm : ApiResponse
    {
        public static ApiResult Post_Register(UnitOfWork _context, WebsiteContactForm form)
        {
      
            
            if (form == null)
                return CreateErrorResult(Enum_Message.REQUIRED_CONTACT_FORM);
            //else if (string.IsNullOrEmpty(form.FullName))
            //    return CreateErrorResult(Enum_Message.REQUIRED_CONTACT_FORM_FULLNAME);
            else if (string.IsNullOrEmpty(form.Email))
                return CreateErrorResult(Enum_Message.REQUIRED_CONTACT_FORM_EMAIL);
            else if (string.IsNullOrEmpty(form.Body))
                return CreateErrorResult(Enum_Message.REQUIRED_CONTACT_FORM_BODY);
            //else if (string.IsNullOrEmpty(form.Mobile) == false && BaseSecurity.IsValidInput(form.Mobile, Enum_Validation.MOBILE) == false)
            //    return CreateErrorResult(Enum_Message.INVALID_MOBILE_FORMAT);
            else if (BaseSecurity.IsValidInput(form.Email, Enum_Validation.EMAIL) == false)
                return CreateErrorResult(Enum_Message.INVALID_EMAIL_FORMAT);

            form.AccountId = form.AccountId;
            form.Datetime = DateTime.Now;
            form.IsRead = false;
            _context.WebsiteContactForm.Insert(form);
            _context.Save();

            EmailAddress address = _context.EmailAddress.FirstOrDefault();
            if (address != null)
            {
                StringBuilder body = new StringBuilder();
                body.AppendLine("نام و نام خانوادگی: " + form.FullName + "<br />");
                body.AppendLine("ایمیل: " + form.Email + "<br />");
                body.AppendLine("موضوع: " + form.Subject + "<br />");
                body.AppendLine("موبایل: " + form.Mobile + "<br />");
                body.AppendLine("وبسایت: " + form.Website + "<br />");
                body.AppendLine("تاریخ و زمان ارسال: " + form.Datetime.ToPersianWithTime() + "<br />");
                body.AppendLine("متن: " + form.Body + "<br />");
                _context.Email.SaveNewEmail(address.Email, Enum_EmailType.CONTACTFORM, body.ToString(), "فرم تماس با ما");
                _context.Save();
            }

            return CreateSuccessResult();
        }
    }
}
