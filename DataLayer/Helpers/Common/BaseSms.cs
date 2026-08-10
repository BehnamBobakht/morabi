using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.ViewModels;
using DataLayer.Entities;
using DataLayer.Enumarables;
using ExpressMapper.Extensions;

namespace DataLayer.Base
{
    public static class BaseSms
    {
        public  static void SaveNewSms(UnitOfWork _context, string receiver, Enum_SmsType type, string body, string token1 = "", string token2 = "", string token3 = "")
        {
            try
            {
                string typeString = type.ToString().ToUpper();
                SmsType smsType = _context.SmsType.FirstOrDefault(p => p.Label == typeString);
                SmsNumber number = _context.SmsNumber.FirstOrDefault();
                int? smsTypeId = smsType == null ? default(int?) : smsType.ID;
                Sms message = new Sms()
                {
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
               _context.Sms. Insert(message);
                _context.Save();
            }
            catch (Exception) { }
        }

    }
}
