using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Api.Model
{
    public class ApiContactUs : ApiResponse
    {
        public static ApiResult Post(UnitOfWork _context, ContactUs contactUs)
        {
            contactUs.CreateDate = DateTime.Now;
            ApiResult result = new ApiResult()
            {
                Code = ApiResult.ResponseCode.Success,
                Message = BaseMessage.GetMessage(Enum_Message.SUCCESSFULL_CONTACTUS).Body
            };
            if (contactUs.Traceable == true)
            {
                contactUs.TrackingCode = BaseRandom.GetRandomUnique();
                result.Message = Resource.Notify.OperationSuccessful + "  &nbsp;&nbsp; " + Resource.Lang.TrackingCode + " : " + contactUs.TrackingCode;
            }
            _context.ContactUs.Insert(contactUs);
            _context.Save();
            EmailAddress address = _context.EmailAddress.FirstOrDefault();
            if (address != null)
            {
                StringBuilder body = new StringBuilder();
                body.AppendLine(Resource.Lang.Name + " : " + contactUs.Name + "<br />");
                body.AppendLine(Resource.Lang.Email + " : " + contactUs.Email + "<br />");
                body.AppendLine(Resource.Lang.Subject + " : " + contactUs.Title + "<br />");
                body.AppendLine(Resource.Lang.Phone + " : " + contactUs.Phone + "<br />");
                body.AppendLine(Resource.Lang.DateTime + " : " + contactUs.CreateDate + "<br />");
                foreach (var item in contactUs.ContactUsField.ToList())
                {
                    if (item.Value != "-")
                    {
                        body.AppendLine(item.Name + ": " + item.Value + "<br />");
                    }
                }
                body.AppendLine(Resource.Lang.Text + " : " + contactUs.Body + "<br />");

                string email = address.Email;
                if (!string.IsNullOrEmpty(contactUs.SendEmail))
                {
                    //message.Attachments.Add(new Attachment(@"c:\x.txt"));
                    //message.Attachments.Add(new Attachment(@"c:\x.txt"));
                    email = contactUs.SendEmail;
                }
                int id = 0;
                _context.Email.SaveNewEmailByAttach(email, Enum_EmailType.CONTACTFORM, body.ToString(), contactUs.Title, ref id);
                _context.Save();

                if (contactUs.PictureId != null)
                {
                    Picture pic = _context.Picture.GetById(contactUs.PictureId);

                    string url = pic.PathUrl;
                    url = url.Replace(Enum_Code.SYSTEM_TYPE_PANEL.ToString(), BaseWebsite.PanelUrl);
                    url = url.Replace(Enum_Code.SYSTEM_TYPE_SHOP.ToString(), BaseWebsite.ShopUrl);
                    url = url.Replace(Enum_Code.SYSTEM_TYPE_SYNCSERVER.ToString(), BaseWebsite.SyncUrl);
                    url = url.Replace(Enum_Code.SYSTEM_TYPE_CMS.ToString(), BaseWebsite.WildCardDomain);
                    EmailAttachment att = new EmailAttachment()
                    {
                        Url = url,
                        Name = contactUs.Title,
                        EmailId = id
                    };
                    _context.EmailAttachment.Insert(att);
                }
                if (contactUs.DocumentId != null)
                {
                    WebsiteDocument doc = _context.WebsiteDocument.GetById(contactUs.DocumentId);
                    string url = doc.PathUrl;
                    url = url.Replace(Enum_Code.SYSTEM_TYPE_PANEL.ToString(), BaseWebsite.PanelUrl);
                    url = url.Replace(Enum_Code.SYSTEM_TYPE_SHOP.ToString(), BaseWebsite.ShopUrl);
                    url = url.Replace(Enum_Code.SYSTEM_TYPE_SYNCSERVER.ToString(), BaseWebsite.SyncUrl);
                    url = url.Replace(Enum_Code.SYSTEM_TYPE_CMS.ToString(), BaseWebsite.WildCardDomain);
                    EmailAttachment att = new EmailAttachment()
                    {
                        Url = url,
                        Name = contactUs.Title,
                        EmailId = id,
                    };
                    _context.EmailAttachment.Insert(att);


                }
                _context.Save();



                if (BaseWebsite.WebsiteLabel == "FlySofia")
                {
                    StringBuilder body2 = new StringBuilder();
                    body2.AppendLine(contactUs.Name + "<br />");
                    body2.AppendLine(contactUs.Title + "<br />");
                    body2.AppendLine(" کد پیگیری شما " + contactUs.Phone + " می باشد. " + "<br /><br />");

                    body2.AppendLine(contactUs.CreateDate.ToPersianWithTime() + "<br />");

                    _context.Email.SaveNewEmail(contactUs.Email, Enum_EmailType.CONTACTFORM, body2.ToString(), "ثبت نام");
                    _context.Save();
                }
            }
            StringBuilder str = new StringBuilder();
            ViewWebsite website = BaseWebsite.ShopWebsite;

            string smstoken_1 = contactUs.ID.ToString();
            string smstoken_2 = "";
            string smstoken_3 = "";
            string smsbody = "";
            smsbody = string.Format(Resource.Pattern.SmsAdminContactUs, contactUs.ID, BaseContent.WebsiteDetail.Title);
            BaseSms.SaveNewSms(_context, BaseContent.WebsiteDetail.Mobile, Enum_SmsType.CONTACTUS_REGISTER, smsbody, smstoken_1, smstoken_2, smstoken_3);
            _context.Save();
            return result;
            /* CreateSuccessResult(Enum_Message.SUCCESSFULL_CONTACTUS, contactUs);*/
        }

        public static ApiResult GetTrackingCode(UnitOfWork _context, string TrackingCode)
        {
            ApiResult result = new ApiResult();
            ContactUs contactus = _context.ContactUs.FirstOrDefault(x => x.TrackingCode == TrackingCode);
            if (contactus == null)
            {
                result.Code = ApiResult.ResponseCode.Error;
                result.Message = Resource.Notify.TeackingCodeInvalid;
            }
            else if (string.IsNullOrEmpty(contactus.Answer))
            {
                result.Code = ApiResult.ResponseCode.Error;
                result.Message = Resource.Notify.MessageVerified;
            }
            else
            {
                result.Code = ApiResult.ResponseCode.Success;
                result.Message = contactus.Answer;
            }
            return result;
        }
    }
}
