using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataLayer.Base
{
    public class BasePayment
    {
        public static string GetHost()
        {
            return (HttpContext.Current.Request.Url.Host == "localhost") ?
                    HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port :
                    HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host;
        }

        public NameValueCollection Inputs = new NameValueCollection();
        public string m_Url = "";
        private string m_Method = "post";
        private string m_FormName = "form1";
        
        public string Post()
        {
            StringBuilder html = new StringBuilder();
            html.AppendLine("<html><head></head>");
            html.AppendLine(string.Format("<body onload=\"document.{0}.submit()\">", m_FormName));
            html.AppendLine(string.Format("<form name=\"{0}\" method=\"{1}\" action=\"{2}\" >", m_FormName, m_Method, Url));
            for (int i = 0; i < Inputs.Keys.Count; i++)
            {
                html.AppendLine(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", Inputs.Keys[i], Inputs[Inputs.Keys[i]]));
            }
            html.AppendLine("</form>");
            html.AppendLine("</body></html>");

            return html.ToString();
        }

        public void AddKey(string name, string value)
        {
            Inputs.Add(name, value);
        }

        public string Method
        {
            get { return m_Method; }
            set { m_Method = value; }
        }

        public string FormName
        {
            get { return m_FormName; }
            set { m_FormName = value; }
        }

        public string Url
        {
            get { return m_Url; }
            set { m_Url = value; }
        }
    }
}
