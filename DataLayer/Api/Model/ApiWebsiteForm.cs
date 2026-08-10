using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Api.Model
{
    public class ApiWebsiteForm : ApiResponse
    {
        public static ApiResult Post(UnitOfWork _context, ViewWebsiteForm form)
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            int? accountId = account != null ? account.Id : default(int?);
            WebsiteForm entity = _context.WebsiteForm.GetByLabel(form.Label);

            WebsiteFormAccount formAccount = new WebsiteFormAccount()
            {
                AccountId = accountId,
                Datetime = DateTime.Now,
                FormId = entity.ID
            };
            _context.WebsiteFormAccount.Insert(formAccount);
            _context.Save();
            
            foreach (ViewWebsiteFormValue item in form.FormValues)
            {
                int ID = item.ColumnId.GetValueOrDefault();
                var mdoel = entity.WebsiteFormField.First(x => x.ID == ID);
                Boolean? requier = mdoel.Requier;

                if (mdoel.Code.Label == DataLayer.Enumarables.Enum_Code.FIELD_TYPE_BOOLEAN.ToString())
                {
                    if (!(requier == true && item.Value == "true"))
                    {
                        return CreateErrorResult(Enum_Message.REQUIRED_FORM);
                    }
                    else
                    {
                        _context.WebsiteFormValue.Insert(new WebsiteFormValue()
                        {
                            AccountFormId = formAccount.ID,
                            AccountId = accountId,
                            FieldId = item.ColumnId.GetValueOrDefault(),
                            Value = item.Value
                        });
                    }
                }
                else if (!(requier == true && string.IsNullOrEmpty(item.Value)))
                {
                    if (mdoel.ColumnName == "Email")
                    {

                        if (BaseSecurity.IsValidInput(item.Value, Enum_Validation.EMAIL) == false)
                        {
                            return CreateErrorResult(Enum_Message.INVALID_EMAIL_FORMAT);
                        }
                    }
                    else if (mdoel.ColumnName == "Mobile")
                    {

                        if (BaseSecurity.IsValidInput(item.Value, Enum_Validation.MOBILE) == false)
                        {
                            return CreateErrorResult(Enum_Message.INVALID_ACCOUNT_MOBILE);
                        }
                    }
                    else if (mdoel.ColumnName == "capcha")
                    {
                        if (System.Web.HttpContext.Current.Session["Captcha"] != null && item.Value != System.Web.HttpContext.Current.Session["Captcha"].ToString())
                            return ApiResponse.CreateErrorResult(Enum_Message.CHECK_ACCOUNT_CAPTCHA);
                    }
                   
                    _context.WebsiteFormValue.Insert(new WebsiteFormValue()
                    {
                        AccountFormId = formAccount.ID,
                        AccountId = accountId,
                        FieldId = item.ColumnId.GetValueOrDefault(),
                        Value = item.Value
                    });
                }
                else
                {

                    return CreateErrorResult(Enumarables.Enum_Message.REQUIRED_FORM);
                }
            }
            _context.Save();

            return CreateSuccessResult();
        }

        private static string RemoveJson(dynamic obj)
        {
            string value = obj.ToString();
            value = value.Replace("{", "");
            value = value.Replace("}", "");
            return value;
        }
    }
}
