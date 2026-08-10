using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Api.Model
{
    public class ApiNewsletter : ApiResponse
    {
        public static ApiResult Post(UnitOfWork _context, Newsletter newsletter)
        {
            if (string.IsNullOrEmpty(newsletter.Email) && string.IsNullOrEmpty(newsletter.Mobile))
                return CreateErrorResult(Enum_Message.REQUIRED_NEWSLETTER_VALUE);

            if (string.IsNullOrEmpty(newsletter.Mobile) == false && BaseSecurity.IsValidInput(newsletter.Mobile, Enum_Validation.MOBILE) == false)
                return CreateErrorResult(Enum_Message.INVALID_MOBILE_FORMAT);

            if (string.IsNullOrEmpty(newsletter.Email) == false && BaseSecurity.IsValidInput(newsletter.Email, Enum_Validation.EMAIL) == false)
                return CreateErrorResult(Enum_Message.INVALID_EMAIL_FORMAT);

            if (_context.NewsLetter.IsRepeated(newsletter))
                return CreateErrorResult(Enum_Message.DUPLICATED_NEWSLETTER);
            
            newsletter.Datetime = DateTime.Now;
            newsletter.Active = true;
            _context.NewsLetter.Insert(newsletter);
            _context.Save();
            return CreateSuccessResult(Enum_Message.SUCCESSFULL_NEWSLETTER, newsletter);
        }
    }
}
