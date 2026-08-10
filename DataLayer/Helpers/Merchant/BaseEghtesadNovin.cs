using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.Services.OnlinePayments.EghtesadNovin;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataLayer.Helpers.Merchant
{
    public class BaseEghtesadNovin : BasePayment
    {
        public static PayingVm StartPayment(UnitOfWork _context, Payment payment)
        {
            var paying = new PayingVm();
            paying.Status = ResultStatus.Error;
            paying.Message = Resource.Notify.PaymentWasProblematic;
            try
            {
                string host = string.IsNullOrEmpty(payment.Merchant.ExternalInfo2) ? BaseWebsite.ShopUrl : payment.Merchant.ExternalInfo2;
                string redirectAddress = host + "payment/callback?id=" + payment.ID;
                string userName = payment.Merchant.Username;        // نام کاربری اینترنتی که به پذیرنده اختصاص داده می شود 
                string password = payment.Merchant.Password;        // کلمه عبور مربوط به نام کاربری که به پذیرنده اختصاص داده می شود
                string terminalId = payment.Merchant.TerminalKey;   // شماره ترمینال پذیرنده مورد نظر 
                string merchantId = payment.Merchant.PrivateKey;    // شماره پذیرندگی
                string goodsReferenceID = "";                       // شناسه خرید دارنده کارت 
                string merchatGoodReferenceID = "";                 // شناسه خرید پذیرنده 
                string productId = "";                              // کد محصول که برای خرید وچر اجباری می باشد
                string sessionId, Resultlogin;
                string resNum = payment.ID.ToString();
                string certificatePath = HttpContext.Current.Server.MapPath(payment.Merchant.ExternalInfo1);

                TokenResult result = null;
                TechnoPaymentWebServiceService ipgTest = new TechnoPaymentWebServiceService();
                LoginParam loginParam = new LoginParam();
                LoginResult loginResult = new LoginResult();

                loginParam.UserName = userName;
                loginParam.Password = password;

                ipgTest.Timeout = 150000;

                loginResult = ipgTest.MerchantLogin(loginParam);
                if (loginResult != null)
                {
                    if (loginResult.Result == "erSucceed")
                    {
                        //Get SessionId from Step1
                        sessionId = loginResult.SessionId;

                        //Step2----> GenerateTransactionDataToSign
                        RequestParam requestParam = new RequestParam();

                        //Make Required Params to Generate Transaction Data To Sign
                        WSContext wsContext = new WSContext();
                        wsContext.SessionId = sessionId;
                        wsContext.UserId = userName;
                        wsContext.Password = password;

                        requestParam.WSContext = wsContext;
                        requestParam.TransType = enTransType.enGoods;
                        requestParam.TransTypeSpecified = true;
                        requestParam.ReserveNum = resNum;
                        requestParam.TerminalId = terminalId;

                        requestParam.Amount = (decimal)(payment.Amount * 10);
                        requestParam.RedirectUrl = redirectAddress;
                        requestParam.MerchantId = merchantId;
                        requestParam.AmountSpecified = true;
                        requestParam.GoodsReferenceID = goodsReferenceID;
                        requestParam.MerchatGoodReferenceID = merchatGoodReferenceID;
                        requestParam.ProductId = productId;

                        //Send Request to Generate Transaction Data to Sign 
                        var generateTransactionDataToSignResult = ipgTest.GenerateTransactionDataToSign(requestParam);

                        //Get Generate Transaction Data to Sign Result from Step2
                        string ResultgenerateTransactionDataToSign = generateTransactionDataToSignResult.Result;
                        if (ResultgenerateTransactionDataToSign == "erSucceed")
                        {
                            string DataToSign = generateTransactionDataToSignResult.DataToSign;
                            string UniqueId = generateTransactionDataToSignResult.UniqueId;
                            try
                            {
                                //Step3----> Sign
                                /*
                                Sign sign = new Sign(
                                    subject: payment.Merchant.MerchantId,
                                    password: payment.Merchant.ExternalInfo2,
                                    path: certificatePath);

                                SyncLog log = new SyncLog() {
                                    Description = certificatePath
                                };
                                _context.SyncLog.Insert(log);
                                _context.SyncLog.Save();

                                byte[] signedBytes = sign.SignSomeText(DataToSign);
                                string signedText = Convert.ToBase64String(signedBytes);
                                */

                                //Step4----> GenerateSignedDataTokenIpgTest.GenerateSignedDataTokenParam generateSignedDataTokenParam = new GenerateSignedDataTokenParam();
                                GenerateSignedDataTokenParam tokenParams = new GenerateSignedDataTokenParam();
                                //tokenParams.Signature = signedText;
                                tokenParams.Signature = DataToSign;
                                tokenParams.UniqueId = UniqueId;
                                tokenParams.WSContext = wsContext;
                                var tokenResult = ipgTest.GenerateSignedDataToken(tokenParams);
                                result = new TokenResult()
                                {
                                    token = tokenResult.Token
                                };
                                paying.Link = GetForm(payment, requestParam, result);
                                paying.Status = ResultStatus.Success;
                                return paying;
                            }
                            catch (Exception ex)
                            {
                                String s;
                                s = ex.ToString();
                                s += ex.StackTrace;

                                paying.Status = ResultStatus.Error;
                                paying.Message = "خطا در مرحله ساین" + "\n" + s;
                            }
                        }
                        else
                        {

                            paying.Status = ResultStatus.Error;
                            paying.Message = "خطا در ارسال پارامترها جهت ساین و ساخت توکن" + "\n" + ResultgenerateTransactionDataToSign;
                        }
                    }
                    else
                    {
                        Resultlogin = loginResult.Result;
                        paying.Status = ResultStatus.Error;
                        paying.Message = "خطا در لاگین پذیرنده" + "\n" + Resultlogin;


                    }
                }
            }
            catch (Exception e)
            {
                paying.Status = ResultStatus.Exception;
                paying.Message = e.Message;

            }
            if (paying.Status != ResultStatus.Success)
            {
                payment.ExternalInfo7 = paying.Message;
                _context.Payment.Update(payment);
                _context.Save();
                paying.Message = Resource.Notify.PaymentWasProblematic;
            }

            return paying;
        }

        public static Payment DoCallBack(UnitOfWork _context, Payment payment)
        {
            try
            {
                var form = HttpContext.Current.Request.Form;
                TechnoPaymentWebServiceService ipgTest = new TechnoPaymentWebServiceService();
                WSContext wsContext = new WSContext();

                wsContext.UserId = payment.Merchant.Username;
                wsContext.Password = payment.Merchant.Password;



                VerifyMerchantTransParam verifyParam = new VerifyMerchantTransParam();
                verifyParam.WSContext = wsContext;
                verifyParam.Token = form["token"];
                verifyParam.RefNum = form["RefNum"];

                VerifyMerchantTransResult result = null;
                SyncLog log = new SyncLog();
                log.Name = form["RefNum"] + form["token"];
                try
                {
                    result = ipgTest.VerifyMerchantTrans(verifyParam);
                    log.Description = result.Result;
                }
                catch (Exception ex)
                {
                    log.Description = ex.Message;
                }
                _context.SyncLog.Insert(log);
                _context.Save();
                if (result.Result.Equals("erSucceed"))
                {

                    try
                    {
                        payment.RefNumber = form["CustomerRefNum"];
                        payment.ExternalInfo1 = form["MID"];
                        payment.ExternalInfo2 = form["ResNum"];
                        payment.ExternalInfo3 = form["goodReferenceId"];
                        payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_SUCCESSFUL);
                        BaseCart.DoPaymentCallBack(_context, payment);
                        _context.Payment.Save();
                    }
                    catch (Exception)
                    {
                        payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_SERVICE_FAILED);
                    }
                }
                else
                {
                    payment.ExternalInfo1 = form["State"];
                    payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY_ERROR);
                }

                _context.Payment.Save();
                return payment;
            }
            catch
            {
                return null;
            }
        }

        private static string GetForm(Payment payment, RequestParam requestParam, TokenResult token)
        {
            StringBuilder htmlForm = new StringBuilder();
            htmlForm.AppendLine("<html>");
            //htmlForm.AppendLine("<body onload='document.getElementById(\"goodsSubmitForm\").submit()'>");
            htmlForm.AppendLine("<body>");
            htmlForm.AppendLine("<form id='goodsSubmitForm' action='" + payment.Merchant.Bank.PaymentUrl + "' method='post' >");
            htmlForm.AppendLine("<input type='hidden' name='amount' value='" + (payment.Amount * 10) + "' />");
            htmlForm.AppendLine("<input type='hidden' name='resNum' value='" + payment.ID + "' />");
            htmlForm.AppendLine("<input type='hidden' name='MID' value='" + requestParam.MerchantId + "' />");
            htmlForm.AppendLine("<input type='hidden' name='goodRefrenceId' value='" + requestParam.GoodsReferenceID + "' />");
            htmlForm.AppendLine("<input type='hidden' name='merchantData' value='" + requestParam.MerchatGoodReferenceID + "' />");
            htmlForm.AppendLine("<input type='hidden' name='redirectURL' value='" + requestParam.RedirectUrl + "' />");
            htmlForm.AppendLine("<input type='hidden' name='language' value='fa' />");
            htmlForm.AppendLine("<input type='hidden' name='token' value='" + token.token + "' />");
            htmlForm.AppendLine("</form>");
            htmlForm.AppendLine("</body>");
            htmlForm.AppendLine("<html>");

            return htmlForm.ToString();
        }

        private class TokenResult
        {
            public string token { get; set; }
            public string message { get; set; }
        }

        private class Sign
        {
            private string password, subject, path;

            public Sign(string subject, string password, string path)
            {
                this.subject = subject;
                this.password = password;
                this.path = path;
            }

            public byte[] SignSomeText(string message)
            {
                Encoding unicode = Encoding.UTF8;
                byte[] msgBytes = Encoding.Default.GetBytes(message);
                X509Certificate2 signerCert = GetSignerCert();
                byte[] encodedSignedCms = SignMsg(msgBytes, signerCert);
                return encodedSignedCms;
            }

            public X509Certificate2 GetSignerCert()
            {
                string certPath = path; //مسیری که فایل پی  12 سرتیفیکیت پذیرنده آنجا قرار دارد
                string certPass = password; //کلمه عبور مختصص سرتیفیکت پذیرنده
                X509Certificate2Collection collection = new X509Certificate2Collection();
                collection.Import(certPath, certPass, X509KeyStorageFlags.PersistKeySet);
                foreach (X509Certificate2 cert in collection)
                {
                    if (cert.Subject.ToLower().Contains(subject.ToLower()))
                        return cert;
                }
                return null;
            }

            //  Sign the message by the using the private key of the signer.
            //  Note that signer's public key certificate is input here 
            //  because it is used to locate the corresponding private key.
            public byte[] SignMsg(Byte[] msg, X509Certificate2 signerCert)
            {
                ContentInfo contentInfo = new ContentInfo(msg);
                SignedCms signedCms = new SignedCms(contentInfo);
                CmsSigner cmsSigner = new CmsSigner(signerCert);
                cmsSigner.SignedAttributes.Add(new Pkcs9SigningTime());
                signedCms.ComputeSignature(cmsSigner);
                return signedCms.Encode();
            }
        }
    }
}
