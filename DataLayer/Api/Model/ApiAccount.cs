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
    public class ApiAccount : ApiResponse
    {
        public static ApiResult Post_DoLogin(UnitOfWork _context, Account account, string password, bool isWeb, bool returnLog = true, AccountAddress address = null, string password2 = null)
        {
            account.Mobile = account.Mobile != null ? account.Mobile.ToEnglishDigit() : account.Mobile;
            account.Email = account.Email != null ? account.Email.ToEnglishDigit() : account.Email;
            account.UserName = account.UserName != null ? account.UserName.ToEnglishDigit() : account.UserName;
            Account temp = new Account();
            if (!string.IsNullOrEmpty(account.UserName))
            {
                temp = _context.Account.GetByMobile(account.UserName);
                if (temp == null)
                {
                    temp = _context.Account.GetByEmail(account.UserName);
                }
            }
            else if (!string.IsNullOrEmpty(account.Mobile) && !string.IsNullOrEmpty(account.Email))
            {
                temp = _context.Account.FirstOrDefault(s => s.Mobile == account.Mobile || s.Email == account.Email);
            }
            else if (!string.IsNullOrEmpty(account.Mobile))
            {
                temp = _context.Account.GetByMobile(account.Mobile);
            }
            else if (!string.IsNullOrEmpty(account.Email))
            {
                temp = _context.Account.GetByEmail(account.Email);
            }
            if (temp == null)
            {
                return CreateErrorResult(Enum_Message.INVALID_USERNAME_PASSWORD);
            }
            Guid uniqueId = new Guid();
            if (temp != null)
            {
                if (temp.UniqueId != null)
                {
                    uniqueId = temp.UniqueId.Value;
                }
            }

            //password = password.ToEnglishDigit();
            string first_password = BaseSecurity.HashMd5(password);
            string new_password = BaseSecurity.ComputeSHA256(password2);

            string second_password = BaseSecurity.HashPassword(uniqueId, password);

            Account user = _context.Account.GetFromUsernameAndPassword(temp, first_password, second_password);
            if (user == null)
            {
                //if (1 == 1)
                //{

                //    string url = "http://lms.psbf.ir/webservice/rest/server.php";
                //    List<ApiHeaderParameter> header = new List<ApiHeaderParameter>() {
                //                    new ApiHeaderParameter() { Name="String-Token",Value=""},
                //                     new ApiHeaderParameter() { Name="Ip-Token",Value=""},
                //    };
                //    ApiResult result = ApiRequest.CreateApiRequest<ViewAccount>(url, Enum_RequestMethod.POST, Enum_Api.PRODUCTCATEGORY, headerList:header);
                //    ViewAccount res = (ViewAccount)signIn.Value;
                //    return CreateErrorResult(Enum_Message.INVALID_USERNAME_PASSWORD);

                //}
                //else
                //{

                return CreateErrorResult(Enum_Message.INVALID_USERNAME_PASSWORD);
                //}

            }

            else if (user.Deleted)
            {
                return CreateErrorResult(Enum_Message.INVALID_USERNAME_PASSWORD);
            }
            else if (returnLog)
            {
                if (isWeb)
                {
                    //_context.AccountBasket.MergeAccountBasket(user.ID);
                    //_context.AccountBasket.DeleteNotAvailableItems(_context, user.ID);
                }
                AccountLog log = _context.AccountLog.NewLog(user.ID, isWeb);
                _context.AccountLog.Insert(log);
                _context.Save();

                ViewAccount entity = user.ToApi(address);
                entity.Key = log.AccountKey;

                StringBuilder str = new StringBuilder();
                //string token1 = user.FullName;
                //string token2 = "";
                //string token3 = "";
                //str.AppendLine(token1);
                //str.AppendLine(" عزیز خوش آمدید");

                //_context.Sms.SaveNewSms(user.Mobile, Enum_SmsType.ACCOUNT_LOGIN, str.ToString(), token1, token2, token3);
                //_context.Email.SaveNewEmail(user.Email, Enum_EmailType.ACCOUNT_LOGIN, str.ToString(), "خوش آمدگویی");
                _context.Save();
                return CreateSuccessResult(Enum_Message.SUCCESSFULL_LOGIN, entity);
            }
            else
            {
                StringBuilder str = new StringBuilder();
                string token1 = account.FullName;
                string token2 = "";
                string token3 = "";
                str.AppendLine(token1);
                str.AppendLine(" عزیز خوش آمدید");

                _context.Sms.SaveNewSms(account.Mobile, Enum_SmsType.ACCOUNT_LOGIN, str.ToString(), token1, token2, token3);
                _context.Email.SaveNewEmail(account.Email, Enum_EmailType.ACCOUNT_LOGIN, str.ToString(), "خوش آمدگویی");
                _context.Save();
                return CreateSuccessResult(Enum_Message.SUCCESSFULL_LOGIN, user.ID);
            }
        }


        public static ApiResult Post_DoRegister(UnitOfWork _context, Account account, bool isWeb, string ConfirmPassword, string bothEmailMobile = "false", Boolean iswebsiteSetting = false)
        {
            if (bothEmailMobile == "null")
            {
                bothEmailMobile = "false";
            }

            if (account != null)
            {
                account.Mobile = string.IsNullOrEmpty(account.Mobile) == false ? account.Mobile.Trim().GetEnglish().ToString() : account.Mobile;
                account.Email = string.IsNullOrEmpty(account.Email) == false ? account.Email.Trim().GetEnglish().ToString() : account.Email;
            }

            if (account.Mobile == null && BaseSecurity.IsValidInput(account.Mobile, Enum_Validation.MOBILE))
            {
                account.Mobile = account.Mobile;
                // account.Email = null;
            }

            if ((account.Email == null || account.Email == "") && BaseSecurity.IsValidInput(account.Email, Enum_Validation.EMAIL))
            {
                account.Email = account.Email;
                // account.Mobile = null;
            }
            account.Mobile = account.Mobile.ToEnglishDigit();
            Account reagentUser = _context.Account.GetFromReagentCode(account.ReagentCode);

            if (account == null)
                return CreateErrorResult(Enum_Message.REQUIRED_ACCOUNT);

            //if (account.Email == null || account.Email == "")
            //    return CreateErrorResult(Enum_Message.REQUIRED_ACCOUNT_EMAIL);
            else if (string.IsNullOrEmpty(account.FullName))
                return CreateErrorResult(Enum_Message.REQUIRED_ACCOUNT_FULLNAME);
            else if (string.IsNullOrEmpty(account.LatName))
                return CreateErrorResult(Enum_Message.REQUIRED_ACCOUNT_FULLNAME);
            else if (string.IsNullOrEmpty(account.LatNameEn))
                return CreateErrorResult(Enum_Message.REQUIRED_ACCOUNT_FULLNAME);
            else if (account.StateId <= 0)
                return CreateErrorResult(Enum_Message.REQUIRED_ACCOUNT_State_VALUE);
            else if (string.IsNullOrEmpty(account.FullNameEn))
                return CreateErrorResult(Enum_Message.REQUIRED_ACCOUNT_FULLNAMEEN);
            else if (!Convert.ToBoolean(bothEmailMobile) && (string.IsNullOrEmpty(account.Email) && string.IsNullOrEmpty(account.Mobile)))
                return CreateErrorResult(Enum_Message.REQUIRED_ACCOUNT_EMAIL_OR_MOBILE);

            else if (Convert.ToBoolean(bothEmailMobile) && (string.IsNullOrEmpty(account.Email) || string.IsNullOrEmpty(account.Mobile)))
                return CreateErrorResult(Enum_Message.REQUIRED_ACCOUNT_EMAIL_AND_MOBILE);
            else if (string.IsNullOrEmpty(account.NationalCode))
                return CreateErrorResult(Enum_Message.REQUIRED_ACCOUNT_NationalCode);
            else if (BaseSecurity.IsValidInput(account.NationalCode, Enum_Validation.NationalCode) == false)
                return CreateErrorResult(Enum_Message.INVALID_REAGENT_NationalCode);

            else if (BaseSecurity.IsValidInput(account.Mobile, Enum_Validation.MOBILE) == false)
                return CreateErrorResult(Enum_Message.INVALID_ACCOUNT_MOBILE);
            else if (string.IsNullOrEmpty(account.Mobile) == false && _context.Account.GetByMobile(account.Mobile) != null)
                return CreateErrorResult(Enum_Message.DUPLICATED_ACCOUNT_MOBILE);
            else if (BaseSecurity.IsValidInput(account.Email, Enum_Validation.EMAIL) == false)
                return CreateErrorResult(Enum_Message.INVALID_ACCOUNT_EMAIL);
            else if (string.IsNullOrEmpty(account.Email) == false && _context.Account.GetByEmail(account.Email) != null)
                return CreateErrorResult(Enum_Message.DUPLICATED_ACCOUNT_EMAIL);
            else if (string.IsNullOrEmpty(account.NationalCode) == false && _context.Account.FirstOrDefault(s => s.NationalCode == account.NationalCode && s.Deleted != true) != null)
                return CreateErrorResult(Enum_Message.DUPLICATED_ACCOUNT_NationalCode);
            else if (account.ReagentCode != null && string.IsNullOrEmpty(account.ReagentCode.Trim()) == false && reagentUser == null)
                return CreateErrorResult(Enum_Message.INVALID_REAGENT_CODE);
            else if (string.IsNullOrEmpty(account.Password))
                return CreateErrorResult(Enum_Message.REQUIRED_ACCOUNT_PASSWORD);
            else if (account.Password != ConfirmPassword)
                return CreateErrorResult(Enum_Message.INVALID_PASSWORD_CONFIRM);
            else if (BaseSecurity.IsValidInput(account.Password, Enum_Validation.PASSWORD) == false)
                return CreateErrorResult(Enum_Message.Validate_ACCOUNT_PASSWORD);
            account.Password = account.Password.ToEnglishDigit();
            account.Mobile = account.Mobile.ToEnglishDigit();
            account.SpacialAccount = account.SpacialAccount;
            account.ReagentId = reagentUser != null ? reagentUser.ID : default(int?);
            account.ReagentCode = _context.Account.GenerateNewReagentCode();
            account.Password = BaseSecurity.HashMd5(account.Password);
            account.UpdateDatetime = DateTime.Now;
            account.CreateDatetime = DateTime.Now;
            account.CountryId = 118;
            account.UniqueId = Guid.NewGuid();
            _context.Account.Insert(account);
            _context.Save();

            ViewAccount entity = account.ToApi();
            AccountLog log = _context.AccountLog.NewLog(account.ID, isWeb);
            _context.AccountLog.Insert(log);
            _context.Save();
            entity.Key = log.AccountKey;
            if (isWeb)
            {
                _context.AccountBasket.MergeAccountBasket(account.ID);
                _context.AccountBasket.DeleteNotAvailableItems(_context, account.ID);

            }
            StringBuilder str = new StringBuilder();
            ViewWebsite website = BaseWebsite.ShopWebsite;
            string token1 = account.Mobile;
            string token2 = ConfirmPassword;
            string token3 = "";
            str.AppendLine("ثبت نام شما در سایت (" + website.Domain + ") با موفقیت انجام شد");
            str.AppendLine("نام کاربری: " + token1);
            str.AppendLine("رمز عبور: " + token2);
            _context.Sms.SaveNewSms(account.Mobile, Enum_SmsType.ACCOUNT_REGISTER, str.ToString(), token1, token2, token3);
            _context.Email.SaveNewEmail(account.Email, Enum_EmailType.ACCOUNT_REGISTER, str.ToString(), "ثبت نام در سایت");
            _context.Save();

            return CreateSuccessResult(Enum_Message.SUCCESSFULL_REGISTER, entity);
        }

        public static ApiResult Get_Account(UnitOfWork _context, int accountId)
        {
            Account account = _context.Account.GetById(accountId);
            return CreateSuccessResult(account.ToApi());
        }

        public static ApiResult Put_Account(UnitOfWork _context, ViewAccount account, int accountId)
        {
            if (account.QuestionRegisterValue != null)
            {
                if (account.QuestionRegisterValue.Any())
                {
                    foreach (var item in account.QuestionRegisterValue)
                    {
                        var listold = _context.QuestionRegisterValue.Where(s => s.FieldId == item.FieldId && s.AccountId == account.Id).ToList();
                        foreach (var old in listold)
                        {
                            _context.QuestionRegisterValue.Delete(old);
                        }
                        _context.Save();
                    }
                    foreach (var item in account.QuestionRegisterValue)
                    {
                        
                        QuestionRegisterValue temp = new QuestionRegisterValue();
                        if (item.FileId > 0)
                        {
                            temp.FileId = item.FileId;
                        }
                        if (item.ItemId > 0)
                        {
                            temp.ItemId = item.ItemId;
                        }
                        temp.FieldId = item.FieldId;
                        temp.AccountId = account.Id;
                        temp.Value = item.Value;

                        _context.QuestionRegisterValue.Insert(temp);
                        _context.Save();

                    }
                   
                }
            }
            if (account.Email != null && BaseSecurity.IsValidInput(account.Email, Enum_Validation.EMAIL) == false)
            {
                return CreateErrorResult(Enum_Message.INVALID_EMAIL_FORMAT);
            }
            if (account.FullName == null || account.FullName == "")
            {
                return CreateErrorResult(Enum_Message.REQUIRED_ACCOUNT_NAME);
            }
            if (account.FullNameEn == null || account.FullNameEn == "")
            {
                return CreateErrorResult(Enum_Message.REQUIRED_ACCOUNT_FULLNAMEEN);
            }
            else if (account.CardNumber != null && account.CardNumber.Trim() != string.Empty)
            {
                if (account.CardNumber.Trim().Length != 16)
                    return CreateErrorResult(Enum_Message.INVALID_ACCOUNT_CardNumber);
                if (BaseSecurity.IsValidInput(account.CardNumber, Enum_Validation.TEXT))
                    return CreateErrorResult(Enum_Message.INVALID_ACCOUNT_CardNumber);
                if (BaseSecurity.IsValidInput(account.CardNumber, Enum_Validation.PERSIAN))
                    return CreateErrorResult(Enum_Message.INVALID_ACCOUNT_CardNumber);
            }
            Account entity = _context.Account.GetById(accountId);
            entity.PictureId = account.PictureID;
            entity.NationalPictureId = account.NationalPictureId;
            entity.InsurancePictureId = account.InsurancePictureId;
            entity.EduactionPictureId = account.EduactionPictureId;
            entity.InsurancePictureId = account.InsurancePictureId;
            entity.Coaching2PictureId = account.Coaching2PictureId;
            entity.ShenasnamehPictureId = account.ShenasnamehPictureId;
            entity.Coaching3PictureId = account.Coaching3PictureId;
            entity.PassportPictureId = account.PassportPictureId;
            entity.QuestionAnswer = account.QuestionAnswer;

            entity.FullNameEn = account.FullNameEn;
            entity.FatherName = account.FatherName;
            entity.FatherNameEn = account.FatherNameEn;
            entity.ZipCode = account.ZipCode;

            entity.Address = account.Address;
            entity.Agent = account.Agent;
            entity.AgentPhone = account.AgentPhone;
            entity.Company = account.Company;
            entity.CompanyNo = account.CompanyNo;
            entity.Email = account.Email;
            entity.FullName = account.FullName;
            entity.LatName = account.LatName;
            entity.LatNameEn = account.LatNameEn;

            entity.IsMale = account.IsMale;
            entity.Job = account.Job;
            entity.NationalCode = account.NationalCode;
            entity.Phone = account.Phone;
            entity.ReagentName = account.ReagentName;
            entity.BirthDay = DataLayer.Base.BaseDate.GetGregorian(account.FaBirthDay);
            entity.Description = account.Description;
            entity.PictureId = account.PictureID;
            entity.Sheba = account.Sheba;
            entity.CardNumber = account.CardNumber;
            entity.StateId = account.StateId != null ? account.StateId : 0;
            entity.CityId = account.CityId != null ? account.CityId : 0;
            entity.CountryId = account.CountryId != null ? account.CountryId : 0;

            _context.Account.Update(entity);
            _context.Save();

            Account tempEntity = _context.Account.GetById(accountId);
            return CreateSuccessResult(tempEntity.ToApi());
        }

        public static ApiResult Put_ChangePassword(UnitOfWork _context, int accountId, string oldPassword, string newPassword, string confirmPassword)
        {
            Account entity = _context.Account.GetById(accountId);

            if (string.IsNullOrEmpty(oldPassword))
                return CreateErrorResult(Enum_Message.REQUIRED_PASSWORD_PREVIOUS);
            else if (string.IsNullOrEmpty(newPassword))
                return CreateErrorResult(Enum_Message.REQUIRED_PASSWORD);
            else if (string.IsNullOrEmpty(confirmPassword))
                return CreateErrorResult(Enum_Message.REQUIRED_PASSWORD_CONFIRM);
            else if (newPassword != confirmPassword)
                return CreateErrorResult(Enum_Message.INVALID_PASSWORD_CONFIRM);
            else if (BaseSecurity.IsValidInput(newPassword, Enum_Validation.PASSWORD) == false)
                return CreateErrorResult(Enum_Message.Validate_ACCOUNT_PASSWORD);
            else if (entity.Password != BaseSecurity.HashMd5(oldPassword))
                return CreateErrorResult(Enum_Message.INVALID_PASSWORD_PREVIOUS);

            entity.Password = BaseSecurity.HashMd5(newPassword);
            _context.Save();

            return CreateSuccessResult(entity.ToApi());
        }

        public static ApiResult Put_ChangeUserName(UnitOfWork _context, int accountId, string username)
        {
            //Account entity = _context.Account.GetById(accountId);

            if (string.IsNullOrEmpty(username))
                return CreateErrorResult(Enum_Message.REQUIRED_USERNAME);
            username = username.Trim().Replace(" ", "");
            if (BaseSecurity.IsValidInput(username, Enum_Validation.USERNAME) == false)
                return CreateErrorResult(Enum_Message.INVALID_USERNAME);
            else if (_context.Account.ChangeUserName(accountId, username) == false)
                return CreateErrorResult(Enum_Message.DUPLICATED_ACCOUNT_USERNAME);
            return CreateSuccessResult();
        }

        public static SignInVm Post_DoSignIn(UnitOfWork _context, SignInVm signIn)
        {
            signIn.BackUrl = signIn.BackUrl;
            signIn.Mobile = signIn.Mobile;
            if (string.IsNullOrEmpty(signIn.Mobile))
            {
                signIn.Status = ResultStatus.Error;
                signIn.Message = Resource.Notify.PleaseInsertTheMobile;
                return signIn;
            }
            if (BaseSecurity.IsValidInput(signIn.Mobile, Enum_Validation.MOBILE) == false)
            {
                signIn.Status = ResultStatus.Error;
                signIn.Message = Resource.Notify.InvalidMobile;
                return signIn;
            }
            Account user = _context.Account.GetByMobile(signIn.Mobile);
            if (user == null)
            {
                signIn.Status = ResultStatus.Error;
                signIn.Message = Resource.Notify.NotRegister;
                return signIn;
            }

            else if (user.Deleted)
            {
                signIn.Status = ResultStatus.Error;
                signIn.Message = Resource.Notify.NotRegister;
                return signIn;
            }
            else
            {
                signIn.Status = ResultStatus.Success;
                signIn.Message = Resource.Notify.success;
                return signIn;
            }
        }
        public static SignInVm Post_DoSendVerifyCode(UnitOfWork _context, SignInVm signIn)
        {
            signIn.BackUrl = signIn.BackUrl;
            signIn.Mobile = signIn.Mobile;
            Account user = _context.Account.GetByMobile(signIn.Mobile);
            if (user == null)
            {
                signIn.Status = ResultStatus.Error;
                signIn.Message = Resource.Notify.NotRegister;
                return signIn;
            }
            else if (user.Deleted)
            {
                signIn.Status = ResultStatus.Error;
                signIn.Message = Resource.Notify.NotRegister;
                return signIn;
            }
            if (user.VerifyTryCount > 4 && user.LastVerifyTryDateTime != null && user.LastVerifyTryDateTime.Value.AddMinutes(10) > DateTime.Now)
            {
                signIn.Status = ResultStatus.Error;
                signIn.Message = Resource.Notify.YourWrongNumberIsMoreThanAllowed;
                return signIn;
            }
            if (user.LastVerifyTryDateTime != null && user.LastVerifyTryDateTime.Value.AddMinutes(10) < DateTime.Now.AddMinutes(10))
            {
                user.VerifyTryCount = 0;
            }
            user.VerifyTryCount++;
            user.LastVerifyTryDateTime = DateTime.Now;
            user.Password = user.VerifyCode = BaseRandom.GetRandomNumber(length: 6);
            _context.Account.Update(user);
            _context.Save();
            StringBuilder str_1 = new StringBuilder();
            string token_1 = user.VerifyCode;
            string token_2 = "";
            string token_3 = "";
            str_1.AppendLine(" کد ورود شما به سایت ");
            str_1.AppendLine(BaseContent.WebsiteDetail.Title);
            str_1.AppendLine(token_1);
            str_1.AppendLine(" می باشد ");
            _context.Sms.SaveNewSms(user.Mobile, Enum_SmsType.ACCOUNT_SendCodeVerify, str_1.ToString(), token_1, token_2, token_3);

            signIn.Status = ResultStatus.Success;
            signIn.Message = Resource.Notify.TheCodeWasSentSuccessfully;
            return signIn;
        }
        public static SignInVm Post_VerifyCode(UnitOfWork _context, SignInVm signIn)
        {
            var result = new SignInVm();
            signIn.BackUrl = signIn.BackUrl;
            signIn.Mobile = signIn.Mobile;
            Account user = _context.Account.GetByMobile(signIn.Mobile);
            if (user == null)
            {
                signIn.Status = ResultStatus.Error;
                signIn.Message = Resource.Notify.NotRegister;
                return signIn;
            }

            else if (user.Deleted)
            {
                signIn.Status = ResultStatus.Error;
                signIn.Message = Resource.Notify.NotRegister;
                return signIn;
            }
            else
            {
                if ((!string.IsNullOrEmpty(signIn.Password)) && user.VerifyCode == signIn.Password)
                {
                    user.VerifyTryCount = 0;
                    user.LastVerifyTryDateTime = null;
                    user.MobileVerified = true;
                    _context.Account.Update(user);
                    _context.Save();

                    StringBuilder str = new StringBuilder();
                    ViewWebsite website = BaseWebsite.ShopWebsite;
                    if (user.CreateDatetime.HasValue && user.CreateDatetime.Value > DateTime.Now.AddMinutes(-10))
                    {
                        string token1 = user.Mobile;
                        string token2 = "";
                        string token3 = "";
                        str.AppendLine("ثبت نام شما در سایت (" + website.Domain + ") با موفقیت انجام شد");
                        _context.Sms.SaveNewSms(user.Mobile, Enum_SmsType.ACCOUNT_REGISTER, str.ToString(), token1, token2, token3);
                        _context.Save();
                    }
                    signIn.Status = ResultStatus.Success;
                    signIn.Message = Resource.Notify.success;
                    return signIn;
                }
                else
                {
                    signIn.Status = ResultStatus.Error;
                    signIn.Message = Resource.Notify.TheCodeEnteredIsIncorrect;
                    return signIn;
                }
            }
        }
        public static SignUpVm Post_DoSignUp(UnitOfWork _context, SignUpVm signUp)
        {

            signUp.Mobile = string.IsNullOrEmpty(signUp.Mobile) == false ? signUp.Mobile.Trim().GetEnglish().ToString() : signUp.Mobile;
            if (string.IsNullOrEmpty(signUp.Mobile))
            {
                signUp.Status = ResultStatus.Error;
                signUp.Message = Resource.Notify.PleaseInsertTheMobile;
                return signUp;
            }
            if (string.IsNullOrEmpty(signUp.FullName))
            {
                signUp.Status = ResultStatus.Error;
                signUp.Message = Resource.Notify.PleaseInsertTheFullName;
                return signUp;
            }
            else if (BaseSecurity.IsValidInput(signUp.Mobile, Enum_Validation.MOBILE) == false)
            {
                signUp.Status = ResultStatus.Error;
                signUp.Message = Resource.Notify.InvalidMobile;
                return signUp;
            }
            else if (string.IsNullOrEmpty(signUp.Mobile) == false && _context.Account.GetByMobile(signUp.Mobile) != null)
            {
                signUp.Status = ResultStatus.Error;
                signUp.Message = Resource.Notify.TheMobileNumberIsDuplicate;
                return signUp;
            }
            var account = new Account();
            account.FullName = signUp.FullName;
            account.Password = "";
            account.Mobile = signUp.Mobile;
            account.UpdateDatetime = DateTime.Now;
            account.CreateDatetime = DateTime.Now;
            account.CountryId = 118;
            account.UniqueId = Guid.NewGuid();
            account.MobileVerified = false;
            _context.Account.Insert(account);
            _context.Save();
            AccountLog log = _context.AccountLog.NewLog(account.ID, false);
            _context.AccountLog.Insert(log);
            _context.Save();
            signUp.Status = ResultStatus.Success;
            signUp.Message = Resource.Notify.success;
            return signUp;
        }

    }
}
