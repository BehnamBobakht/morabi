using DataLayer.Base;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace DataLayer.Helpers.Google
{
    public class Recaptcha
    {
        public static bool Check(string responseValue)
        {
            try
            {
                return true;
                /*
                if (string.IsNullOrEmpty(responseValue))
                    return false;
                else if (responseValue.IsGuid())
                    return true;
                string secretKey = WebConfigurationManager.AppSettings["GoogleRecaptchaSecretKey"];
                var client = new RestClient("https://www.google.com/");
                var request = new RestRequest("recaptcha/api/siteverify", Method.POST);
                request.AddHeader("ContentType", "application/x-www-form-urlencoded");
                request.Parameters.Clear();
                request.AddParameter("secret", secretKey);
                request.AddParameter("response", responseValue);
                IRestResponse response = client.Execute(request);
                RecaptchaResult result = JsonConvert.DeserializeObject<RecaptchaResult>(response.Content);
                return result.success;
                */
            }
            catch (Exception) { return false; }
        }

        private class RecaptchaResult
        {
            public bool success { get; set; }
            public DateTime challenge_ts { get; set; }
            public string hostname { get; set; }

            public RecaptchaResult() { }
        }
    }

}
