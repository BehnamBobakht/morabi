using Binbin.Linq;
using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataLayer.Repositories
{
    public class Entity_Account : BaseRepository<Account>
    {
        DatabaseEntities _context;
        public Entity_Account(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public List<Account> Search(
          int? index = 1,
          int? id = null,

          string NationalCode =null,
          string Mobile = null,
          int? pageSize = null,
          int? productId = null,
          string fullName = null, DateTime? fromDatetime = null, DateTime? toDatetime = null

          )
        {
            var MyQuery = GetSearchQuery(
                fullName: fullName,
                id:id,
                productId: productId,
                Mobile: Mobile,
                fromDatetime: fromDatetime,
            toDatetime: toDatetime,
            NationalCode: NationalCode
                );

            pageSize = pageSize == null ? 10 : pageSize;
            index = index == null ? 1 : index;
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;
            
                List<Account> results = _context
                .Account
                .OrderByDescending(p => p.ID)
                .Where(MyQuery)

                .Skip(skipValue)
                .Take(pageValue)
                .ToList();
                return results;
            
            

            
        }
        public List<Account> Search2(
        int? index = 1,
        int? id = null,

        string NationalCode = null,
        string Mobile = null,
        int? pageSize = null,
        int? productId = null,
        string fullName = null, DateTime? fromDatetime = null, DateTime? toDatetime = null

        )
        {
            var MyQuery = GetSearchQuery(
                fullName: fullName,
                id: id,
                productId: productId,
                Mobile: Mobile,
                fromDatetime: fromDatetime,
            toDatetime: toDatetime,
            NationalCode: NationalCode
                );

            pageSize = pageSize == null ? 10 : pageSize;
            index = index == null ? 1 : index;
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;

            List<Account> results = _context
            .Account
            .OrderByDescending(p => p.ID)
            .Where(MyQuery)

            .Skip(skipValue)
            .Take(pageValue).ToList().
            Select(s => new Account
            {
                ID = s.ID,
                FullName = s.FullName,
                LatName = s.LatName,
                LatNameEn = s.LatNameEn,
                FullNameEn = s.FullNameEn,
                Mobile = s.Mobile,
                Email = s.Email,
                BookPayment = s.BookPayment,
                Address = s.Address,
                QuestionAnswer = s.QuestionAnswer,
                Picture = s.Picture,
                NationalCode = s.NationalCode
            }).ToList();
           
            return results;




        }
        public int SearchCount(
         string fullName = null,
         string NationalCode=null,
         string Mobile = null,

         int? productId = null,
          DateTime? fromDatetime = null, DateTime? toDatetime = null
         )
        {
            var MyQuery = GetSearchQuery(
                fullName: fullName,
                productId: productId,
                Mobile: Mobile,
                NationalCode: NationalCode,
                fromDatetime: fromDatetime,
                toDatetime: toDatetime
             );
            return _context.Account.Count(MyQuery);
        }
        private Expression<Func<Account, bool>> GetSearchQuery(
         string fullName = null,
          int? id = null,
         DateTime? fromDatetime = null,
         DateTime? toDatetime = null,
         string NationalCode=null,
         string Mobile = null,
         int? productId=null
         )
        {
            List<Account> results = new List<Account>();
            var MyQuery = PredicateBuilder.True<Account>();
            fullName = fullName.ToPersianChar();
            if (id != null)
                MyQuery = MyQuery.And(p => p.ID== id);

            if (fullName != null)
                MyQuery = MyQuery.And(p => p.FullName.Contains(fullName) || p.LatName.Contains(fullName));
            if (productId != null)
                MyQuery = MyQuery.And(p => p.AccountOrder.Any(s=>s.AccountOrderProduct.Any(x=>x.ProductId== productId || x.Product.ProductsId.Contains(productId.ToString())) && (s.StatusId==2068 || s.StatusId== 2073)) /*||p.CoursePayment.Any(x=>x.CourseId==productId && x.PaidStatus==1) || p.QuizPayment.Any(x=>x.QuizId==productId && x.Status==1) || p.BookPayment.Any(x => x.BookId == productId) || p.Students.Any(x => x.CourseId == productId)*/);
            if (NationalCode != null && NationalCode!="")
            {
                NationalCode = NationalCode.ToEnglishDigit();
                MyQuery = MyQuery.And(p => p.NationalCode.Contains(NationalCode));

            }
            if (Mobile != null && Mobile != "")
            {
                var mobieEn = Mobile.ToEnglishDigit();
                MyQuery = MyQuery.And(p => p.Mobile.Contains(mobieEn) || p.Mobile.Contains(Mobile));
            }
            if (fromDatetime != null && fromDatetime != default(DateTime))
                MyQuery = MyQuery.And(p => p.CreateDatetime >= fromDatetime);

            if (toDatetime != null && toDatetime != default(DateTime))
            {
                DateTime toDatetimeTemp = toDatetime.Value.AddDays(1);
                MyQuery = MyQuery.And(p => p.CreateDatetime <= toDatetimeTemp);
            }

            return MyQuery;
        }
        public ViewAccount GetCurrentAccount()
        {
            var CurrentUser = HttpContext.Current.Session["ACCOUNT"];
            if (CurrentUser != null)
                return (ViewAccount)HttpContext.Current.Session["ACCOUNT"];
            HttpCookie loginCookie = HttpContext.Current.Request.Cookies["ACCOUNT"];
            if (loginCookie != null)
            {
                if (
                    (
                        !string.IsNullOrEmpty(loginCookie.Values["EMAIL"]) ||
                        !string.IsNullOrEmpty(loginCookie.Values["MOBILE"]) ||
                        !string.IsNullOrEmpty(loginCookie.Values["NATIONALCODE"])
                    ) &&
                    !string.IsNullOrEmpty(loginCookie.Values["PASSWORD"]))
                {
                    string Email = BaseSecurity.DecryptText(loginCookie.Values["EMAIL"]);
                    string Mobile = BaseSecurity.DecryptText(loginCookie.Values["MOBILE"]);
                    string NationalCode = BaseSecurity.DecryptText(loginCookie.Values["NATIONALCODE"]);
                    string Password = BaseSecurity.DecryptText(loginCookie.Values["PASSWORD"]);
                    string UserName = BaseSecurity.DecryptText(loginCookie.Values["USERNAME"]);
                    string tempPassword = BaseSecurity.HashMd5(Password);
                    string tempSecondPassword = BaseSecurity.HashPassword(new Guid(), Password);

                    Account tempAccount = new Account()
                    {
                        Email = Email,
                        Mobile = Mobile,
                        NationalCode = NationalCode
                    };
                    Account RememberUser = GetFromUsernameAndPassword(tempAccount, tempPassword, tempSecondPassword);
                    if (RememberUser != null)
                    {
                        ViewAccount entity = new ViewAccount(RememberUser);
                        SetCurrentAccount(entity, Password, true);
                        return entity;
                    }
                    else
                    {
                        Logout();
                        return null;
                    }
                }
            }
            return null;
        }

        public ViewLocation GetCurrentLocation()
        {
            //var CurrentLoction = HttpContext.Current.Session[".LOCATION"];
            //if (CurrentLoction != null)
            //    return (ViewLocation)HttpContext.Current.Session[".LOCATION"];
            try
            {
                HttpCookie locationCookie = HttpContext.Current.Request.Cookies[".LOCATION"];
                if (locationCookie != null)
                {
                    if (!string.IsNullOrEmpty(locationCookie.Values["CountyId"]) &&
                        !string.IsNullOrEmpty(locationCookie.Values["CityId"]) &&
                        !string.IsNullOrEmpty(locationCookie.Values["StateId"]))

                    {
                        int CountyId = int.Parse(locationCookie.Values["CountyId"]);
                        int CityId = int.Parse(locationCookie.Values["CityId"]);
                        int StateId = int.Parse(locationCookie.Values["StateId"]);
                        int AddressId = int.Parse(locationCookie.Values["AddressId"]);
                        //ViewLocation location = new ViewLocation()
                        //{
                        //    CountryId = 118, /*CountyId,*/
                        //    CityId = CityId,
                        //    AddressId = AddressId,
                        //    StateId = StateId,
                        //    CityName = _context.City.Where(s => s.ID == CityId).FirstOrDefault().Name,
                        //    StateName = _context.State.Where(s => s.ID == StateId).FirstOrDefault().Name,
                        //    CountryName = _context.Country.Where(s => s.ID == CountyId).FirstOrDefault().FaName,
                        //};

                        //return location;

                    }

                    return null;

                }
                else
                {

                    SetCurrentLocation(118, 87, 8, 0);
                    return GetCurrentLocation();
                }
            }
            catch
            {
                ViewLocation location = new ViewLocation()
                {
                    CityId = 87,
                    StateId = 8,
                    CountryId = 118,/*countyId,*/
                    AddressId = 0
                };
                return location;
            }


        }
        public int GetSendType(int storeId)
        {
            List<ViewSendTypeStores> listOutput = new List<ViewSendTypeStores>();
            HttpCookie cookie = HttpContext.Current.Request.Cookies[".SENDTYPE"];
            if (cookie != null)
            {
                string cookieSendType = HttpContext.Current.Server.UrlDecode(cookie.Value);
                List<ViewSendTypeStores> list = new List<ViewSendTypeStores>();
                list = JsonConvert.DeserializeObject<List<ViewSendTypeStores>>(cookieSendType);
                listOutput = CastCookieSendType(list);
                var storeSend = listOutput.Where(s => s.StoreId == storeId).FirstOrDefault();
                if (storeSend != null)
                {
                    return storeSend.Id;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 1;
            }


        }
        public void RemoveSendTypeStore()
        {
            HttpContext.Current.Response.Cookies[".SENDTYPE"].Expires = DateTime.Now.AddDays(-1);
        }
        public List<ViewSendTypeStores> GetSendTypeList()
        {
            List<ViewSendTypeStores> listOutput = new List<ViewSendTypeStores>();
            HttpCookie cookie = HttpContext.Current.Request.Cookies[".SENDTYPE"];
            if (cookie != null)
            {
                string cookieSendType = HttpContext.Current.Server.UrlDecode(cookie.Value);
                List<ViewSendTypeStores> list = new List<ViewSendTypeStores>();
                list = JsonConvert.DeserializeObject<List<ViewSendTypeStores>>(cookieSendType);
                listOutput = CastCookieSendType(list);

                return listOutput;
            }
            else
            {
                return new List<ViewSendTypeStores>();
            }


        }
        public void SetSendTypeStore(List<ViewSendTypeStores> list)
        {
            List<ViewSendTypeStores> finalList = new List<ViewSendTypeStores>();
            foreach (ViewSendTypeStores item in list)
            {
                finalList.Add(new ViewSendTypeStores()
                {
                    Id = item.Id,
                    StoreId = item.StoreId
                });
            }
            HttpCookie cookie = new HttpCookie(".SENDTYPE")
            {
                Value = JsonConvert.SerializeObject(finalList),
                Expires = DateTime.Now.AddDays(1)
            };
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public List<ViewSendTypeStores> CastCookieSendType(List<ViewSendTypeStores> list)
        {
            List<ViewSendTypeStores> listOutput = new List<ViewSendTypeStores>();
            foreach (ViewSendTypeStores item in list.ToList())
            {
                ViewSendTypeStores entity = new ViewSendTypeStores()
                {
                    Id = item.Id,
                    StoreId = item.StoreId,

                };

                listOutput.Add(entity);
            }
            return listOutput;
        }
        public void Logout()
        {
            HttpContext.Current.Session.Remove("ACCOUNT");
            HttpContext.Current.Response.Cookies["ACCOUNT"].Expires = DateTime.Now.AddDays(-1);
            HttpContext.Current.Session.Remove(".LOCATION");
            HttpContext.Current.Response.Cookies[".LOCATION"].Expires = DateTime.Now.AddDays(-1);
            HttpContext.Current.Response.Cookies[".intercode"].Expires = DateTime.Now.AddDays(-1);
            SetCurrentLocation(118, 87, 8, 0);
        }

        public string GenerateNewReagentCode()
        {
            bool IsRepeated = true;
            string code = "";
            while (IsRepeated)
            {
                code = new Random().Next(10000, 99999).ToString();
                if (_context.Account.Any(p => p.ReagentCode == code) == false)
                    IsRepeated = false;
                else
                    code = "";
            }
            return code;
        }

        public Account GetFromReagentCode(string ReagentCode)
        {
            return ReagentCode != null ? _context.Account.FirstOrDefault(p => p.ReagentCode == ReagentCode) : null;
        }

        public void SetCurrentAccount(ViewAccount account, string password, bool remember)
        {
            HttpContext.Current.Session["ACCOUNT"] = account;
            HttpCookie loginCookie = new HttpCookie("ACCOUNT");
            if (remember)
            {
                HttpContext.Current.Response.Cookies["ACCOUNT"].Expires = DateTime.Now.AddDays(-6);

                loginCookie.Values.Add("EMAIL", BaseSecurity.EncryptText(account.Email));
                loginCookie.Values.Add("MOBILE", BaseSecurity.EncryptText(account.Mobile));
                loginCookie.Values.Add("NATIONALCODE", BaseSecurity.EncryptText(account.NationalCode));
                loginCookie.Values.Add("PASSWORD", BaseSecurity.EncryptText(password));
                loginCookie.Values.Add("USERNAME", BaseSecurity.EncryptText(account.UserName));
                loginCookie.Expires = DateTime.Now.AddMonths(1);
                HttpContext.Current.Response.Cookies.Add(loginCookie);
            }
            else
            {
                loginCookie.Expires = DateTime.Now.AddMonths(-1);
            }
        }
        public void SetCurrentLocation(int countyId, int cityId, int stateId, int addressId)
        {
            ViewLocation location = new ViewLocation()
            {
                CityId = cityId,
                StateId = stateId,
                CountryId = 118,/*countyId,*/
                AddressId = addressId
            };
            HttpContext.Current.Session[".LOCATION"] = location;
            HttpCookie locationCookie = new HttpCookie(".LOCATION");
            //HttpContext.Current.Response.Cookies[".LOCATION"].Expires = DateTime.Now.AddDays(-1);
            locationCookie.Values.Add("CountyId", countyId.ToString());
            locationCookie.Values.Add("CityId", cityId.ToString());
            locationCookie.Values.Add("StateId", stateId.ToString());
            locationCookie.Values.Add("AddressId", addressId.ToString());
            locationCookie.Expires = DateTime.Now.AddMonths(1);
            HttpContext.Current.Response.Cookies.Add(locationCookie);

        }
        public Account GetFromUsernameAndPassword(Account account, string password, string second_password, string new_password = null)
        {
            string username = null;


            Boolean iswebsiteSetting = DataLayer.Base.BaseWebsite.WebsiteSetting.IsLoginWithNationalCode;
            if (iswebsiteSetting)
            {

                username = !string.IsNullOrEmpty(username) ? username : account.NationalCode.GetEnglish();


                return _context.Account.FirstOrDefault(p =>
            (
                (p.NationalCode != null && p.NationalCode == username)
            )
            &&
            (
                p.Password == password ||
                p.Password == second_password

            ||
            (p.Password2 != null && p.Password2 == new_password)
            )
              );
            }
            else
            {
                username = /*username != null ? username :*/ account.Mobile.GetEnglish();
                username = !string.IsNullOrEmpty(username) ? username : account.Email;
                return _context.Account.FirstOrDefault(p => p.Deleted == false &&
                  (
                    (p.Mobile != null && p.Mobile == username) ||
                      (p.Email != null && p.Email == username)
                  )
                  &&
                  (
                      p.Password == password ||
                      p.Password == second_password
                      ||
               (p.Password2 != null && p.Password2 == new_password)
                  )
              );
            }
        }

        public Account GetByEmail(string email)
        {
            var item = _context.Account.FirstOrDefault(p => p.Email == email && p.Deleted != true);
            return item;
        }
        public bool CheckEmailAndMobile(Account account, bool mobile = true)
        {
            var temp = new Account();
            if (mobile)
                temp = _context.Account.FirstOrDefault(p => p.Mobile == account.Mobile);
            else
                temp = _context.Account.FirstOrDefault(p => p.Email == account.Email);

            if (account.ID == 0)
            {
                if (temp != null)
                    return false;
                else
                    return true;
            }
            else
            {
                if (temp != null)
                {
                    if (temp.ID == account.ID)
                        return true;
                    else
                        return false;
                }
                return true;
            }
        }
        public bool CheckMobile(Account account, bool mobile = true)
        {
            var temp = new Account();
            if (mobile)
                temp = _context.Account.FirstOrDefault(p => p.Mobile == account.Mobile);
          

            if (account.ID == 0)
            {
                if (temp != null)
                    return false;
                else
                    return true;
            }
            else
            {
                if (temp != null)
                {
                    if (temp.ID == account.ID)
                        return true;
                    else
                        return false;
                }
                return true;
            }
        }
        public Account GetByMobile(string mobile)
        {
            mobile = mobile.GetEnglish();
            return _context.Account.FirstOrDefault(p =>
                p.Mobile == mobile && p.Deleted != true
            );
        }

        public Account GetByUserName(string username)
        {
            username = username.GetEnglish();
            return _context.Account.FirstOrDefault(p =>
                p.UserName == username
            );
        }

        public ViewAccount GetAccountbyUserName(string username)
        {
            var User = GetByUserName(username);
            ViewAccount account = new ViewAccount();
            if (User != null)
            {
                account.Address = User.Address;
                account.Agent = User.Agent;
                account.AgentPhone = User.AgentPhone;
                account.Company = User.Company;
                account.CompanyNo = User.CompanyNo;
                account.Description = User.Description;
                account.Email = User.Email;
                account.Fax = User.Fax;
                account.FullName = User.FullName;
                account.Id = User.ID;
                account.Instagram = User.Instagram;
                account.Job = User.Job;
                account.Mobile = User.Mobile;
                account.NationalCode = User.NationalCode;
                account.Password = User.Password;
                account.Phone = User.Phone;
                if (User.PictureId != null)
                {
                    account.PictureID = User.PictureId.Value;
                }
                account.ReagentCode = User.ReagentCode;
                account.ReagentName = User.ReagentName;
                account.Telegram = User.Telegram;
                account.UserName = User.UserName;
                account.Website = User.Website;
                account.WhatsApp = User.WhatsApp;
                account.ChangeUserName = User.ChangeUserName;
                account.BirthDay = User.BirthDay;
                account.CardNumber = User.CardNumber;
                account.Sheba = User.Sheba;
                account.StateId = User.StateId;
                account.CityId = User.CityId;
                account.CountryId = 118; /*User.CountryId;*/
                account.PictureID = User.PictureId;


            }
            return account;
        }
        public bool UserNameNotDuplicate(Account account, string newUserName)
        {

            var temp = _context.Account.FirstOrDefault(s => s.UserName == newUserName);
            if (temp == null)
            {
                return true;
            }
            else if (temp.ID == account.ID)
            {
                return true;
            }
            else
            {
                return false;
            }



        }
        public bool ChangeUserName(int accountId, string username)
        {
            var user = GetById(accountId);
            if (user != null)
            {
                if (user.ChangeUserName)
                    return false;
                if (UserNameNotDuplicate(user, username))
                {
                    user.UserName = username;
                    user.ChangeUserName = true;
                    Update(user);
                    Save();
                    return true;
                }
            }
            return false;

        }
    }
}
