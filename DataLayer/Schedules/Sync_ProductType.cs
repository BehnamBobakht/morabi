using DataLayer.Api;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using DataLayer.Base;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Schedules
{
    public class Sync_ProductType : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            if (Sync_Base.Is_ProductType_Syncing == false)
            {
                Sync_Base.Is_ProductType_Syncing = true;
                try
                {
                    UnitOfWork _context = new UnitOfWork();
                    SyncLog log = new SyncLog()
                    {
                        Name = "ProductType",
                        StartDatetime = DateTime.Now
                    };

                    string local_type = Enum_Code.SYSTEM_TYPE_LOCAL.ToString();
                    List<Website> listWebsite = _context.Website.Where(p => p.Code.Label == local_type).ToList();

                    foreach (Website website in listWebsite)
                    {
                        foreach (WebsiteDomain domain in website.WebsiteDomain)
                        {
                            string datetime = _context.ProductType.GetAll().Max(s => s.SyncDatetime).GetDateString();
                            List<ApiUrlParameter> parameters = new List<ApiUrlParameter>() {
                                new ApiUrlParameter() { Name = "pageSize", Value="10" },
                                new ApiUrlParameter() { Name = "datetime", Value=datetime }
                            };
                            ApiResult result = ApiRequest.CreateApiRequest<List<ViewProductType>>(domain, Enum_RequestMethod.GET, Enum_Api.PRODUCTTYPE, parameters);
                            if (result.Value != null)
                            {
                                List<ViewProductType> list = (List<ViewProductType>)result.Value;
                                if (list.Count > 0)
                                {
                                    int syncCount = 0;
                                    foreach (ViewProductType item in list)
                                    {
                                        string idString = item.Id.ToString();
                                        ProductType temp = _context.ProductType.FirstOrDefault(p => p.SyncId == idString);
                                        if (temp != null)
                                        {
                                            temp.Name = item.Name;
                                            temp.SyncId = item.Id.ToString();
                                            temp.SyncDatetime = item.SyncDatetime;
                                            _context.ProductType.Update(temp);
                                        }
                                        else
                                        {
                                            temp = new ProductType()
                                            {
                                                SyncId = item.Id.ToString(),
                                                Name = item.Name,
                                                SyncDatetime = item.SyncDatetime
                                            };
                                            _context.ProductType.Insert(temp);
                                        }
                                        syncCount++;
                                    }
                                    _context.Save();
                                    
                                    if (syncCount > 0)
                                    {
                                        log.Description = syncCount.ToString();
                                        log.EndDatetime = DateTime.Now;
                                        log.Status = "OK";
                                        _context.SyncLog.Insert(log);
                                        _context.Save();
                                    }
                                }
                            }
                        }
                    }
                }
                catch(Exception ex) {
                    string a = ex.Message;
                }
                Sync_Base.Is_ProductType_Syncing = false;                
            }
        }
    }
}
