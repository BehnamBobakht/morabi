using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.OpenSsl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using ZarinPal;

namespace DataLayer.Helpers.Merchant
{
    public class BaseTejart : BasePayment
    {
        public static PayingVm StartPayment(UnitOfWork _context, Payment payment)
        {
            var paying = new PayingVm();
            paying.Status = ResultStatus.Error;
            paying.Message = Resource.Notify.PaymentWasProblematic;
            string host = string.IsNullOrEmpty(payment.Merchant.ExternalInfo2) ? BaseWebsite.ShopUrl : payment.Merchant.ExternalInfo2;
            string redirectAddress = host + "payment/callback?id=" + payment.ID;
            try
            {
                IPGData iPGData = new IPGData();
                iPGData.TreminalId = payment.Merchant.TerminalKey;
                iPGData.AcceptorId = payment.Merchant.MerchantId;

                iPGData.RevertURL = redirectAddress;
                iPGData.Amount = (long)(payment.Amount * 10);
                iPGData.PaymentId = payment.ID.ToString();
                iPGData.RequestId = payment.ID.ToString();
                iPGData.TransactionType = TransactionType.Purchase;
                iPGData.BillInfo = null;
                iPGData.PassPhrase = payment.Merchant.Password;
                iPGData.RsaPublicKey = payment.Merchant.PrivateKey;
                string request = string.Empty;
                request = CreateJsonRequest.CreateJasonRequest(iPGData);

                Uri url = new Uri("https://ikc.shaparak.ir/api/v3/tokenization/make");
                WebHelper webHelper = new WebHelper();

                string jresponse = webHelper.Post(url, request);
                if (jresponse != null)
                {
                    TokenResult jResult = JsonConvert.DeserializeObject<TokenResult>(jresponse);
                    //Handle your reponse here 

                    if (jResult.status)
                    {
                        var urlpayment = "https://ikc.shaparak.ir/iuiv3/IPG/Index";
                        var sb = new System.Text.StringBuilder();
                        sb.Append("<html>");
                        sb.AppendFormat("<body onload='document.forms[0].submit()'>");
                        sb.AppendFormat("<form action='{0}' method='post'>", urlpayment);

                        sb.AppendFormat("<input type='hidden' name='tokenIdentity' value='{0}'>", jResult.result?.token);
                        sb.Append("</form>");
                        sb.Append("</body>");
                        sb.Append("</html>");
                        paying.Status = ResultStatus.Success;
                        paying.Link = sb.ToString();
                    }
                    else
                    {
                        paying.Message = string.Format("result:{0} desc:{1}", jResult.responseCode, jResult.description);
                    }
                }

            }
            catch (Exception ex)
            {
                paying.Status = ResultStatus.Exception;
                paying.Message = ex.Message;
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
                string responseCode = form["responseCode"];
                string token = form["token"];
                string amount = form["amount"];
                string retrievalReferenceNumber = form["retrievalReferenceNumber"];
                string systemTraceAuditNumber = form["systemTraceAuditNumber"];
                string acceptorId = form["acceptorId"].Substring(7, 8);

                if (!string.IsNullOrEmpty(responseCode) && responseCode == "00")
                {
                    WebHelper webHelper = new WebHelper();
                    RequestVerify requestVerify = new RequestVerify();
                    requestVerify.terminalId = acceptorId;
                    requestVerify.systemTraceAuditNumber = systemTraceAuditNumber;
                    requestVerify.retrievalReferenceNumber = retrievalReferenceNumber;
                    requestVerify.tokenIdentity = token;
                    string request = JsonConvert.SerializeObject(requestVerify);
                    XmlDocument doc = new XmlDocument();
                    var urlVerify = "https://ikc.shaparak.ir/api/v3/confirmation/purchase";

                    Uri url = new Uri(urlVerify);
                    string jresponse = webHelper.Post(url, request);


                    if (jresponse != null)
                    {
                        VerifyResult jResult = JsonConvert.DeserializeObject<VerifyResult>(jresponse);
                        //Handle your reponse here


                        if (jResult.status)
                        {
                            payment.ExternalInfo1 = "تراکنش موفقیت آمیز انجام شد";
                            payment.RefNumber = jResult.description;
                            payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_SUCCESSFUL);
                            BaseCart.DoPaymentCallBack(_context, payment);
                            _context.Payment.Save();

                            //  Verification succed , your statements Goes here

                        }
                        else
                        {
                            payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY_ERROR);
                            _context.Payment.Save();

                        }
                    }
                }
                else
                {
                    payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY_ERROR);
                    _context.Payment.Save();
                }
            }
            catch (Exception e)
            {
                payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY_ERROR);               
                payment.ExternalInfo1 = "مشکلی در پرداخت به وجود آمده است ، در صورتیکه وجه پرداختی از حساب بانکی شما کسر شده است آن مبلغ به صورت خودکار برگشت داده خواهد شد";
                payment.ExternalInfo3 =e.Message;
            }
            _context.Save();
            payment = _context.Payment.GetById(payment.ID);
            return payment;
        }
        public static byte[] RSAEncription(byte[] aesCodingResult, string publicKey)
        {
            //var bytesToEncrypt = Encoding.UTF8.GetBytes(clearText);

            var encryptEngine = new Pkcs1Encoding(new RsaEngine());

            using (var txtreader = new StringReader(publicKey))
            {
                var keyParameter = (AsymmetricKeyParameter)new PemReader(txtreader).ReadObject();

                encryptEngine.Init(true, keyParameter);
            }

            return encryptEngine.ProcessBlock(aesCodingResult, 0, aesCodingResult.Length);
        }
        public static byte[] CombinArray(byte[] first, byte[] second)
        {
            byte[] bytes = new byte[first.Length + second.Length];
            Array.Copy(first, 0, bytes, 0, first.Length);
            Array.Copy(second, 0, bytes, first.Length, second.Length);
            return bytes;
        }
        public static byte[] HashHSA256(byte[] date)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(date);

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return bytes;
            }
        }
        public static byte[] EncryptStringToBytes_Aes(byte[] plainText, byte[] Key, byte[] IV)
        {
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.KeySize = 128;
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                return encryptor.TransformFinalBlock(plainText, 0, plainText.Length);
                // Create the streams used for encryption.

            }

        }
    }

    public static class CreateJsonRequest
    {
        public static string CreateJasonRequest(IPGData iPGData)
        {
            RequestClass requestClass = new RequestClass();
            requestClass.Request.CmsPreservationId = iPGData.CmsPreservationId;
            //requestClass.Request.additionalParameters = new List<KeyValuePair<string, string>>
            //{
            //    new KeyValuePair<string, string>("nationalId","1045879569")
            //};
            requestClass.Request.AcceptorId = iPGData.AcceptorId;
            requestClass.Request.amount = iPGData.Amount;
            requestClass.Request.BillInfo = iPGData.BillInfo;
            requestClass.Request.multiplexParameters = iPGData.MultiplexParameters;
            requestClass.Request.PaymentId = iPGData.PaymentId;
            requestClass.Request.RequestId = iPGData.RequestId;
            requestClass.Request.RevertUri = iPGData.RevertURL;
            requestClass.Request.terminalId = iPGData.TreminalId;
            requestClass.Request.RequestTimestamp = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            requestClass.Request.transactionType = iPGData.TransactionType;

            CreateAESCoding(requestClass, iPGData.RsaPublicKey, iPGData.PassPhrase, requestClass.Request.multiplexParameters == null ? false : true);
            try
            {

            }
            catch (Exception ex)
            {


            }
            return JsonConvert.SerializeObject(requestClass); ;
        }
        private static void CreateAESCoding(this RequestClass requestClass, string rsaPublicKey, string passPhrase, bool isMultiplex)
        {
            try
            {
                string baseString =
              requestClass.Request.terminalId +
               passPhrase +
                requestClass.Request.amount.ToString().PadLeft(12, '0') +
               (isMultiplex ? "01" : "00") +
               (isMultiplex ?
                requestClass.Request.multiplexParameters.Select(t => $"{t.Iban.Replace("IR", "2718")}{t.Amount.ToString().PadLeft(12, '0')}")
               .Aggregate((a, b) => $"{a}{b}")
               : string.Empty);
                using (Aes myAes = Aes.Create())
                {
                    myAes.KeySize = 128;
                    myAes.GenerateKey();
                    myAes.GenerateIV();
                    byte[] keyAes = myAes.Key;
                    byte[] ivAes = myAes.IV;

                    byte[] resultCoding = new byte[48];
                    byte[] baseStringbyte = baseString.HexStringToByteArray();

                    byte[] encrypted = EncryptStringToBytes_Aes(baseStringbyte, myAes.Key, myAes.IV);
                    byte[] hsahash = new SHA256CryptoServiceProvider().ComputeHash(encrypted);

                    resultCoding = CombinArray(keyAes, hsahash);

                    requestClass.AuthenticationEnvelope.Data = RSAEncription(resultCoding, rsaPublicKey).ByteArrayToHexString();
                    //  string decripte = DecryptStringFromBytes_Aes(encrypted, myAes.Key, myAes.IV);
                    requestClass.AuthenticationEnvelope.Iv = ivAes.ByteArrayToHexString();

                }
            }
            catch (Exception ex)
            {


            }

        }


        private static byte[] RSAEncription(byte[] aesCodingResult, string publicKey)
        {
            //var bytesToEncrypt = Encoding.UTF8.GetBytes(clearText);

            var encryptEngine = new Pkcs1Encoding(new RsaEngine());

            using (var txtreader = new StringReader(publicKey))
            {
                var keyParameter = (AsymmetricKeyParameter)new PemReader(txtreader).ReadObject();

                encryptEngine.Init(true, keyParameter);
            }

            return encryptEngine.ProcessBlock(aesCodingResult, 0, aesCodingResult.Length);
        }
        private static byte[] CombinArray(byte[] first, byte[] second)
        {
            byte[] bytes = new byte[first.Length + second.Length];
            Array.Copy(first, 0, bytes, 0, first.Length);
            Array.Copy(second, 0, bytes, first.Length, second.Length);
            return bytes;
        }
        private static byte[] HashHSA256(byte[] date)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(date);

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return bytes;
            }
        }
        private static byte[] EncryptStringToBytes_Aes(byte[] plainText, byte[] Key, byte[] IV)
        {
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.KeySize = 128;
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                return encryptor.TransformFinalBlock(plainText, 0, plainText.Length);
                // Create the streams used for encryption.

            }

        }
        private static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }
    }

    public class VerifyResult
    {
        public string responseCode { get; set; }
        public string description { get; set; }
        public bool status { get; set; }
        public SubResult result { get; set; }
    }

    public class SubResult
    {
        public string responseCode { get; set; }
        public string systemTraceAuditNumber { get; set; }
        public string retrievalReferenceNumber { get; set; }
        public DateTime transactionDateTime { get; set; }
        public int transactionDate { get; set; }
        public int transactionTime { get; set; }
        public string processCode { get; set; }
        public object requestId { get; set; }
        public object additional { get; set; }
        public object billType { get; set; }
        public object billId { get; set; }
        public string paymentId { get; set; }
        public string amount { get; set; }
        public object revertUri { get; set; }
        public object acceptorId { get; set; }
        public object terminalId { get; set; }
        public object tokenIdentity { get; set; }
    }
    public class RequestClass
    {
        public AuthenticationEnvelope AuthenticationEnvelope = new AuthenticationEnvelope();
        public Request Request = new Request();
    }
    public class AuthenticationEnvelope
    {
        public string Data { get; set; }
        public string Iv { get; set; }
    }
    public class Request
    {
        public string AcceptorId { get; set; }
        public long amount { get; set; }
        public BillInfo BillInfo { get; set; }
        public string CmsPreservationId { get; set; }
        public List<MultiplexParameter> multiplexParameters { get; set; }
        public string PaymentId { get; set; }
        public string RequestId { get; set; }
        public long RequestTimestamp { get; set; }
        public string RevertUri { get; set; }
        public string terminalId { get; set; }
        public string transactionType { get; set; }
        public List<KeyValuePair<string, string>> additionalParameters { get; set; }
    }
    public struct TransactionType
    {
        public const string Purchase = "Purchase";
        public const string Bill = "Bill";

    }
    public class BillInfo
    {
        public string BillId { get; set; }
        public string billPaymentId { get; set; }
    }
    public class MultiplexParameter
    {
        public string Iban { get; set; }
        public int Amount { get; set; }
    }
    
    public partial class IPGData
    {

        public string treminalId;

        public string acceptorId;

        public string passPhrase;

        public string revertURL;

        public string rsaPublicKey;

        public long amount;

        public string paymentId;

        public string requestId;

        public string cmsPreservationId;

        public BillInfo billInfo;

        public List<MultiplexParameter> multiplexParameters;

        public string transactionType;
        public string RsaPublicKey { get; set; }//{ get => rsaPublicKey; set => rsaPublicKey = value; }
        public string TreminalId { get; set; }//{ get => treminalId; set => treminalId = value; }
        public string AcceptorId { get; set; }//{ get => acceptorId; set => acceptorId = value; }
        public string PassPhrase { get; set; }//{ get => passPhrase; set => passPhrase = value; }
        public string RevertURL { get; set; }//{ get => revertURL; set => revertURL = value; }
        public long Amount { get; set; }//{ get => amount; set => amount = value; }
        public string PaymentId { get; set; }//{ get => paymentId; set => paymentId = value; }
        public string CmsPreservationId { get; set; }//{ get => cmsPreservationId; set => cmsPreservationId = value; }
        public string TransactionType { get; set; }//{ get => transactionType; set => transactionType = value; }
        public BillInfo BillInfo { get; set; }//{ get => billInfo; set => billInfo = value; }
        public List<MultiplexParameter> MultiplexParameters { get; set; }//{ get => multiplexParameters; set => multiplexParameters = value; }
        public string RequestId { get; set; }//{ get => requestId; set => requestId = value; }
    }

    public static class Extension
    {
        public static byte[] HexStringToByteArray(this string hexString)
            => Enumerable.Range(0, hexString.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(value: hexString.Substring(startIndex: x, length: 2), fromBase: 16))
                .ToArray();

        public static string ByteArrayToHexString(this byte[] bytes)
            => bytes.Select(t => t.ToString(format: "X2")).Aggregate((a, b) => $"{a}{b}");
    }

    public class WebHelper
    {
        public string Post(Uri url, string value)
        {
            var request = HttpWebRequest.Create(url);
            var byteData = Encoding.ASCII.GetBytes(value);
            request.ContentType = "application/json";
            request.Method = "POST";

            try
            {
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(byteData, 0, byteData.Length);
                }
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                return responseString;
            }
            catch (WebException e)
            {
                return null;
            }
        }

        public string Get(Uri url)
        {
            var request = HttpWebRequest.Create(url);
            request.ContentType = "application/json";
            request.Method = "GET";

            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                return responseString;
            }
            catch (WebException e)
            {
                return null;
            }
        }
    }
    public class TokenResult
    {
        TokenResult()
        {
            result = new Result();
        }
        public string responseCode { get; set; }
        public object description { get; set; }
        public bool status { get; set; }
        public Result result { get; set; }
    }

    public class Result
    {
        public Result()
        {
            billInfo = new Billinfo();
        }
        public string token { get; set; }
        public int initiateTimeStamp { get; set; }
        public int expiryTimeStamp { get; set; }
        public string transactionType { get; set; }
        public Billinfo billInfo { get; set; }
    }

    public class Billinfo
    {
        public object billId { get; set; }
        public object billPaymentId { get; set; }
    }
    public class RequestVerify
    {
        [DataMember]
        public string terminalId { get; set; }

        [DataMember]
        public string retrievalReferenceNumber { get; set; }

        [DataMember]
        public string systemTraceAuditNumber { get; set; }

        [DataMember]
        public string tokenIdentity { get; set; }
    }
}

