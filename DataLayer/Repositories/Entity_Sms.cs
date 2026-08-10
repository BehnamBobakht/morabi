using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Binbin.Linq;

namespace DataLayer.Repositories
{
    public class Entity_Sms : BaseRepository<Sms>
    {
        private DatabaseEntities _context;

        public Entity_Sms(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public void SaveNewSms(string receiver, Enum_SmsType type, string body, string token1 = "", string token2 = "", string token3 = "")
        {
            try
            {
                string typeString = type.ToString().ToUpper();
                SmsType smsType = _context.SmsType.FirstOrDefault(p => p.Label == typeString);
                SmsNumber number = _context.SmsNumber.FirstOrDefault();
                int? smsTypeId = smsType == null ? default(int?) : smsType.ID;
                Sms message = new Sms() {
                    Body = body,
                    TypeId = smsTypeId,
                    CreateDatetime = DateTime.Now,
                    LastSendDatetime = DateTime.Now,
                    Receive = receiver,
                    Sender = number?.Number,
                    Token1 = token1.Replace(" ", "_"),
                    Token2 = token2.Replace(" ", "_"),
                    Token3 = token3.Replace(" ", "_"),
                    IsSend = false,
                };
                Insert(message);
                Save();
            }
            catch (Exception) { }
        }

        public void SendCenter()
        {
            List<Sms> list = _context.Sms.Where(p => p.IsSend == false).Take(100).ToList();
            if (list.Count > 0)
            {
                foreach (Sms item in list)
                {
                    item.IsSend = true;
                }
                _context.SaveChanges();
                
                var smsTypes = list.GroupBy(p => p.TypeId);
                foreach (var item in smsTypes)
                {
                    SmsSetting setting = _context.SmsSetting.FirstOrDefault(p => p.SmsTypeId == item.Key);
                    if (setting != null)
                    {
                        List<Sms> listSmsType = list.Where(p => p.TypeId == item.Key).ToList();
                        SmsProvider provider = setting.SmsNumber.SmsProvider;
                        SmsNumber number = setting.SmsNumber;
                        if (setting.SmsNumber.SmsProvider.Label == Enum_SmsProvider.KAVENEGAR.ToString())
                        {
                            foreach (Sms sms in listSmsType)
                            {
                                SendSingleKavenegar(provider, number, setting, sms);
                            }
                        }
                        else if (setting.SmsNumber.SmsProvider.Label == Enum_SmsProvider.KAVENEGAR_LOOKUP.ToString())
                        {
                            foreach (Sms sms in listSmsType)
                            {
                                SendSingleKavenegarLookup(provider, number, setting, sms);
                            }
                        }
                        else if (setting.SmsNumber.SmsProvider.Label == Enum_SmsProvider.IPPANEL.ToString())
                        {
                            foreach (Sms sms in listSmsType)
                            {
                                SendSingleIpPanel(provider, number, setting, sms);
                            }
                        }
                        else if (setting.SmsNumber.SmsProvider.Label == Enum_SmsProvider.RAHYAB.ToString())
                        {
                            foreach (Sms sms in listSmsType)
                            {
                                SendSingleRahavard(provider, number, setting, sms);
                            }
                        }
                        else if (setting.SmsNumber.SmsProvider.Label == Enum_SmsProvider.JAHAN_PAYAMAK.ToString())
                        {
                            foreach (Sms sms in listSmsType)
                            {
                                SendSingleJahanPayamak(provider, number, setting, sms);
                            }
                        }
                        else if (setting.SmsNumber.SmsProvider.Label == Enum_SmsProvider.BEHPARDAZAN.ToString())
                        {
                            foreach (Sms sms in listSmsType)
                            {
                                SendSingleBehpardazanPayamak(provider, number, setting, sms);
                            }
                        }
                    }
                    _context.SaveChanges();
                }
            }
        }

        private void SendSingleRahavard(SmsProvider provider, SmsNumber number, SmsSetting setting, Sms sms)
        {
            try
            {
                Services.Sms.Rahavard.SingleStatusSMSRequest request = new Services.Sms.Rahavard.SingleStatusSMSRequest();
                string strServiceID = "RahyabSMS";
                string strServiceToken = "R@hy@bSoap_V1";
                string strNumberCompany = "RAHAVARD";
                string strIP = "http://193.104.22.14:2055/CPSMSService/Access";
                string resultMessage = "";
                Services.Sms.Rahavard.SendWebServiceSoapClient service = new Services.Sms.Rahavard.SendWebServiceSoapClient();
                service.SendSMS_Single(
                    strServiceID,
                    strServiceToken,
                    sms.Body,
                    sms.Receive,
                    sms.Sender,
                    setting.SmsNumber.Username,
                    setting.SmsNumber.Password,
                    strNumberCompany,
                    strIP,
                    ref resultMessage);
            }
            catch (Exception) { }
        }

        private void SendSingleJahanPayamak(SmsProvider provider, SmsNumber number, SmsSetting setting, Sms sms)
        {
            try
            {
                var client = new RestClient(provider.ServiceUrl);
                string[] rcpts = new string[] { sms.Receive };
                string json = JsonConvert.SerializeObject(rcpts);
                var request = new RestRequest("", Method.GET);
                request.Parameters.Clear();
                request.AddParameter("action", "SMS_SEND");
                request.AddParameter("username", setting.SmsNumber.Username);
                request.AddParameter("password", setting.SmsNumber.Password);
                request.AddParameter("api", "9");
                request.AddParameter("from", setting.SmsNumber.Number.GetEnglish());
                request.AddParameter("to", sms.Receive.GetEnglish());
                request.AddParameter("FLASH", "0");
                request.AddParameter("text", sms.Body);
                IRestResponse response = client.Execute(request);
            }
            catch (Exception) { }
        }

        private void SendSingleIpPanel(SmsProvider provider, SmsNumber number, SmsSetting setting, Sms sms)
        {
            try
            {
                var client = new RestClient(provider.ServiceUrl);
                string[] rcpts = new string[] { sms.Receive };
                string json = JsonConvert.SerializeObject(rcpts);
                var request = new RestRequest("", Method.POST);
                request.Parameters.Clear();
                request.AddParameter("op", "send");
                request.AddParameter("uname", setting.SmsNumber.Username);
                request.AddParameter("pass", setting.SmsNumber.Password);
                request.AddParameter("message", sms.Body);
                request.AddParameter("to", json);
                request.AddParameter("from", setting.SmsNumber.Number.GetEnglish());
                request.AddHeader("ContentType", "application/x-www-form-urlencoded");
                IRestResponse response = client.Execute(request);
            }
            catch (Exception) { }
        }

        private void SendSingleKavenegar(SmsProvider provider, SmsNumber number, SmsSetting setting, Sms sms)
        {
            try
            {
                var client = new RestClient(provider.ServiceUrl + number.ApiKey + "/sms/send.json");
                var request = new RestRequest("", Method.GET);
                request.Parameters.Clear();
                request.AddQueryParameter("receptor", sms.Receive.GetEnglish());
                request.AddQueryParameter("message", sms.Body);
                request.AddQueryParameter("sender", sms.Sender);
                request.AddQueryParameter("localid", sms.ID.ToString());
                IRestResponse response = client.Execute(request);
                dynamic result = JsonConvert.DeserializeObject(response.Content);
                sms.StatusText = result.statustext;
                sms.Cost = result.cost;
                sms.MessageId = result.messageid;
            }
            catch (Exception) { }
        }

        private void SendSingleKavenegarLookup(SmsProvider provider, SmsNumber number, SmsSetting setting, Sms sms)
        {
            try
            {
                string restClient = provider.ServiceUrl + number.ApiKey + "/verify/lookup.json";
                var client = new RestClient(restClient);
                string template = setting.SmsType.Label.Replace("_", "");
                var request = new RestRequest("", Method.GET);
                request.Parameters.Clear();
                request.AddQueryParameter("receptor", sms.Receive.GetEnglish());
                request.AddQueryParameter("template", template);
                request.AddQueryParameter("token", sms.Token1);
                request.AddQueryParameter("token2", sms.Token2);
                request.AddQueryParameter("token3", sms.Token3);
                IRestResponse response = client.Execute(request);
                dynamic result = JsonConvert.DeserializeObject(response.Content);
                sms.StatusText = result.statustext;
                sms.Cost = result.cost;
                sms.MessageId = result.messageid;
                sms.Token3 = "true";
                _context.SaveChanges();
            }
            catch (Exception) {
                sms.Token3 = "false";
                _context.SaveChanges();
            }
        }

        private void SendSingleBehpardazanPayamak(SmsProvider provider, SmsNumber number, SmsSetting setting, Sms sms)
        {
            try
            {
                object input = new
                {
                    Date = DateTime.Now.ToShortDateString(),
                    Time = DateTime.Now.ToShortTimeString(),
                    Number = sms.Receive,
                    Body = sms.Body,
                    Project = BaseWebsite.WebsiteLabel.ToLower(),
                };

                var client = new RestClient(provider.ServiceUrl);
                client.AddDefaultHeader("Content-type", "application/json");
                var request = new RestRequest("", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddJsonBody(input);
                var response = client.Execute(request);
            }
            catch (Exception) { }
        }     
        public List<Sms> Search(int index = 1, int pageSize = 10, string name = null)
        {
            List<Sms> results = new List<Sms>();
            int skipValue = pageSize * (index - 1);
            int pageValue = pageSize;

            var MyQuery = GetSearchQuery(name: name);
            return _context.Sms.Where(MyQuery)
                .OrderByDescending(p => p.ID)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();
        }
        public int SearchCount(string name = null)
        {
            var MyQuery = GetSearchQuery(
                name: name);
            return _context.Sms.Count(MyQuery);
        }

        private Expression<Func<Sms, bool>> GetSearchQuery(
            string name = null)
        {
            var MyQuery = PredicateBuilder.True<Sms>();
            if (IsNullOrEmptyId(name) == false)
            {
                MyQuery = MyQuery.And(p => p.Body.Contains(name));
            }

            return MyQuery;
        }

    }
}
