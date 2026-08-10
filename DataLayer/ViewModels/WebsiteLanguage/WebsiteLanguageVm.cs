using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class WebsiteLanguageVm
    {

        public int ID { get; set; }
        public int WebsiteId { get; set; }
        public int LanguageId { get; set; }
        public string LanguageName { get; set; }
        public string LanguageLabel { get; set; }

    }
}
