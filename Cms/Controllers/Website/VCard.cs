using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Text;

namespace Cms.Controllers
{
    public class VCard
    {
        public string FullName { get; set; }

        public string Addrress { get; set; }

        public string JobTitle { get; set; }

        public string Phone { get; set; }

        public string Mobile { get; set; }

        public string Email { get; set; }

        public string Fax { get; set; }

        public string Instagram { get; set; }
        public string Telegram { get; set; }
        public string WhatsApp { get; set; }
        public string Website { get; set; }
        public string HomePage { get; set; }


        public byte[] Image { get; set; }

        public string ToIos()
        {
            var builder = new StringBuilder();
            builder.AppendLine("BEGIN:VCARD");
            builder.AppendLine("VERSION:3.0");
            builder.AppendLine("N:;" + FullName + ";;;");
            builder.AppendLine("FN:" + FullName);
            builder.AppendLine("ORG:" + JobTitle + ";");
            builder.AppendLine("EMAIL;type=INTERNET;type=WORK;type=pref:" + Email);
            builder.AppendLine("TEL;type=CELL;type=VOICE;type=pref:" + Mobile);
            builder.AppendLine("item1.ADR;type=WORK;type=pref:;;" + Addrress + ";;;;");
            builder.AppendLine("TEL;type=WORK;type=VOICE:" + Phone);
            builder.AppendLine("TEL;type=WORK;type=FAX:" + Fax);
            builder.AppendLine("X-SOCIALPROFILE;type=Telegram;x-user=" + Telegram + ":x-apple:" + Telegram);
            builder.AppendLine("X-SOCIALPROFILE;type=‎WhatsApp;x-user=" + WhatsApp + ":x-apple:" + WhatsApp);
            builder.AppendLine("X-SOCIALPROFILE;type=Instagram;x-user=" + Instagram + ":x-apple:" + Instagram);
            builder.AppendLine("item1.URL;type=pref:" + HomePage);
            builder.AppendLine("item1.X-ABLabel:_$!<HomePage>!$_");
            builder.AppendLine("URL;type=WORK:" + Website);
            if (Image != null)
            {
                builder.AppendLine("PHOTO;ENCODING=b:" + Convert.ToBase64String(Image));
            }
            builder.AppendLine("END:VCARD");
            return builder.ToString();
        }
        public string ToAndroid()
        {
            var builder = new StringBuilder();

            builder.AppendLine("BEGIN:VCARD");

            builder.AppendLine("VERSION:2.1");

            builder.AppendLine("FN:" + FullName);

            builder.Append("ADR;WORK;POSTAL:" + Addrress);
            builder.Append("\n");

            builder.AppendLine("TITLE:" + JobTitle);

            builder.AppendLine("TEL;TYPE=Work:" + Phone);

            builder.AppendLine("TEL;TYPE=Mobile:" + Mobile);

            builder.AppendLine("TEL;TYPE=Fax:" + Fax);

            builder.AppendLine("URL:" + HomePage);

            builder.AppendLine("URL:" + Website);

            builder.AppendLine("EMAIL;PREF;INTERNET:" + Email);

            builder.AppendLine("X-WHATSAPP:" + WhatsApp);

            builder.AppendLine("X-Telegram:" + Telegram);

            builder.AppendLine("URL;TYPE=Instagram:" + Instagram);

            if (Image != null)
            {
                builder.AppendLine("PHOTO;ENCODING=b:" + Convert.ToBase64String(Image));
            }

            builder.AppendLine(string.Empty);


            builder.AppendLine("END:VCARD");


            return builder.ToString();

        }

    }
}