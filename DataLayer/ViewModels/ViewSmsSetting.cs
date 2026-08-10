using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewSmsSetting
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public ViewSmsNumber SmsNumber { get; set; }
        public ViewSmsType SmsType { get; set; }

        public ViewSmsSetting() { }

        public ViewSmsSetting(SmsSetting setting, int index, string MaxZero)
        {
            this.Id = setting.ID;
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.SmsNumber = new ViewSmsNumber(setting.SmsNumber);
            this.SmsType = new ViewSmsType(setting.SmsType);
        }
    }
}
