using DataLayer.Api;
using DataLayer.Api.Model;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using DataLayer.Repositories;
using DataLayer.ViewModels;
using System.Web;
using System.Linq.Expressions;

namespace Cms.Controllers.Api
{
    public class AccountLogVisitController : ApiController
    {

        UnitOfWork _context = new UnitOfWork();

        [HttpGet]
        public Boolean Post(int? sourceId, int pageName)
        {
            try
            {
                AccountLogVisit objVisit = new AccountLogVisit();
                ViewAccount currentAccount = _context.Account.GetCurrentAccount();
                int? accid = null;
                if (currentAccount != null)
                {
                    accid = currentAccount.Id;
                }
                string ip = GetIPAddress();
                AccountLogVisit AccSearch = null;
                AccountLogVisit IpSearch = null;
                if (accid > 0)
                {
                    AccSearch = _context.AccountLogVisit.FirstOrDefault(x => x.AccountId == accid &&
                                                                                x.PageName == pageName &&
                                                                                x.SourceId == sourceId);
                }
                else
                {
                    IpSearch = _context.AccountLogVisit.FirstOrDefault(x => x.IP == ip &&
                                                                     x.PageName == pageName &&
                                                                     x.SourceId == sourceId);
                }
                if (AccSearch == null && IpSearch == null)
                {
                    objVisit.AccountId = accid;
                    objVisit.Datetime = DateTime.Now;
                    objVisit.SourceId = sourceId;
                    objVisit.PageName = pageName;
                    objVisit.IP = ip;
                    _context.AccountLogVisit.Insert(objVisit);
                    _context.Save();
                }
                return true;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch(Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                 throw;
            }
        }

        [HttpGet]
        public Boolean CheckUserVisit(int? sId, int pName)
        {
            ViewAccount tempAccount = _context.Account.GetCurrentAccount();
            Account account = null;
            if (tempAccount != null)
            {
                account = _context.Account.GetById(tempAccount.Id);
            }
            List<AccountLogVisit> results = null;
            if (account != null)
            {
                results = _context.AccountLogVisit.Where(x => x.AccountId == account.ID && x.SourceId == sId && x.PageName == pName).ToList();
            }
            if (results == null)
            {
                string ip = GetIPAddress();
                results = _context.AccountLogVisit.Where(x => x.IP == ip && x.SourceId == sId && x.PageName == pName).ToList();
            }

            if (results != null && results.Count() > 0)
                return true;
            else return false;
        }
        public static string GetIPAddress()
        {
            try
            {
                HttpRequest currentRequest = HttpContext.Current.Request;
                string ip = (currentRequest.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? currentRequest.ServerVariables["REMOTE_ADDR"]).Split(',')[0].Trim();
                return ip;
            }
            catch
            {
                return "";
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
