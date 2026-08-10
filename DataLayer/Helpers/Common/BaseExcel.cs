using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DataLayer.Helpers.Common
{
    public class BaseExcel
    {
        public static void ExportExcel(DataTable datatable)
        {
            var Response = System.Web.HttpContext.Current.Response;
            var Request = System.Web.HttpContext.Current.Request;
            string filename = Request.Url.Host + "_excel_" + Persia.Calendar.ConvertToPersian(DateTime.Now).Simple.GetEnglish().Replace("/", "") + "_" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second;
            var grid = new GridView();
            grid.DataSource = datatable;
            grid.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=" + filename + ".xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            grid.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
    }
}
