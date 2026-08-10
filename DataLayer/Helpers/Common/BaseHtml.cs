using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Base
{
    public class BaseHtml
    {
        public static string ConvertToSimpleText(string htmlBody)
        {
            if (htmlBody != null)
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(htmlBody);
                var root = doc.DocumentNode;
                var sb = new StringBuilder();
                foreach (HtmlNode node in doc.DocumentNode.DescendantsAndSelf())
                {
                    if (!node.HasChildNodes)
                    {
                        string text = node.InnerText;
                        if (!string.IsNullOrEmpty(text))
                        {
                            string tempText = text.Trim();
                            tempText = tempText.Replace("&nbsp;", "");

                            sb.AppendLine(tempText);
                        }
                    }
                }
                return sb.ToString();
            }
            else
                return null;
        }
    }
}
