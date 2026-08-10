using DataLayer.Api;
using DataLayer.Api.Model;
using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cms.Controllers.Api
{
    public class AccountController : ApiController
    {
        UnitOfWork _context = new UnitOfWork();
        [HttpGet]
        public ApiResult Get()
   {
            ViewAccount currentAccount = _context.Account.GetCurrentAccount();
            if (currentAccount == null)
                return ApiResponse.CreateInvalidKeyResult();
            if (currentAccount.BirthDay != null)
            {
                currentAccount.FaBirthDay = currentAccount.BirthDay.ToPersian();
            }
            var temp = _context.Account.FirstOrDefault(s => s.ID == currentAccount.Id);
            currentAccount.NationalPictureId = temp.NationalPictureId;
            currentAccount.PictureID = temp.PictureId;
            currentAccount.Address = temp.Address;
            currentAccount.ZipCode = temp.ZipCode;
            currentAccount.FatherNameEn = temp.FatherNameEn;
            currentAccount.FatherName = temp.FatherName;
            currentAccount.FullNameEn = temp.FullNameEn;
            currentAccount.LatName = temp.LatName;
            currentAccount.LatNameEn = temp.LatNameEn;


            currentAccount.PassportPictureId = temp.PassportPictureId;
            currentAccount.InsurancePictureId = temp.InsurancePictureId;
            currentAccount.EduactionPictureId = temp.EduactionPictureId;
            currentAccount.ShenasnamehPictureId = temp.ShenasnamehPictureId;
            currentAccount.Coaching2PictureId = temp.Coaching2PictureId;
            currentAccount.Coaching3PictureId = temp.Coaching3PictureId;

            currentAccount.PictureUrl = temp.Picture != null ? temp.Picture.GetUrl() : temp.Image;
            currentAccount.NationalPictureUrl = temp.Picture1 != null ? temp.Picture1.GetUrl() : temp.ImageNational;
            currentAccount.EduactionPictureUrl = temp.Picture2 != null ? temp.Picture2.GetUrl() : temp.ImageEduaction;
            currentAccount.PassportPictureUrl = temp.Picture3 != null ? temp.Picture3.GetUrl() : temp.ImagePassport;
            currentAccount.ShenasnamehPictureUrl = temp.Picture4 != null ? temp.Picture4.GetUrl() : temp.ImageShenasnameh;
            currentAccount.InsurancePictureUrl = temp.Picture5 != null ? temp.Picture5.GetUrl() : temp.ImageInsurance;
            currentAccount.Coaching2PicturerUrl = temp.Picture6 != null ? temp.Picture6.GetUrl() : temp.ImageCoaching2;
            currentAccount.Coaching3PicturerUrl = temp.Picture7 != null ? temp.Picture7.GetUrl() : temp.ImageCoaching3;


            return ApiResponse.CreateSuccessResult(currentAccount);
        }

        [HttpPut]
        public ApiResult Put(ViewAccount account)
        {
            ApiResult result = new ApiResult();
            ViewAccount currentAccount = _context.Account.GetCurrentAccount();
            if (currentAccount == null)
                return ApiResponse.CreateInvalidKeyResult();
            account.Id = currentAccount.Id;
            result = ApiAccount.Put_Account(_context, account, account.Id);
            if (result.Code == ApiResult.ResponseCode.Success)
                _context.Account.SetCurrentAccount((ViewAccount)result.Value, currentAccount.Password, true);
            return result;
        }

        [HttpPut]
        public ApiResult Put(string oldPassword, string newPassword, string confirmPassword)
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            if (account == null)
                return ApiResponse.CreateInvalidKeyResult();
            ApiResult result = ApiAccount.Put_ChangePassword(_context, account.Id, oldPassword, newPassword, confirmPassword);
            if (result.Code == ApiResult.ResponseCode.Success)
                _context.Account.SetCurrentAccount((ViewAccount)result.Value, newPassword, false);
            return result;
        }

        [HttpPut]
        public ApiResult Put(string userName)
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            if (account == null)
                return ApiResponse.CreateInvalidKeyResult();
            ApiResult result = ApiAccount.Put_ChangeUserName(_context, account.Id, userName);
            //(_context, account.Id, oldPassword, newPassword, confirmPassword);
            if (result.Code == ApiResult.ResponseCode.Success)
                _context.Account.SetCurrentAccount((ViewAccount)result.Value, userName, false);
            return result;
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
