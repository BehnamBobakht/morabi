using DataLayer.Api;
using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Schedules
{
    public class Sync_ProductBrand : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            if (Sync_Base.Is_ProductBrand_Syncing == false)
            {
                Sync_Base.Is_ProductBrand_Syncing = true;
                try
                {
                    UnitOfWork _context = new UnitOfWork();
                    SyncLog log = new SyncLog()
                    {
                        Name = "ProductBrand",
                        StartDatetime = DateTime.Now
                    };
                    string local_type = Enum_Code.SYSTEM_TYPE_LOCAL.ToString();
                    List<Website> listWebsite = _context.Website.Where(p => p.Code.Label == local_type).ToList();

                    foreach (Website website in listWebsite)
                    {
                        foreach (WebsiteDomain domain in website.WebsiteDomain)
                        {
                            string datetime = _context.ProductBrand.GetAll().Max(s => s.SyncDatetime).GetDateString();
                            List<string> listSubmit = new List<string>();
                            List<ApiUrlParameter> parameters = new List<ApiUrlParameter>() {
                                new ApiUrlParameter() { Name = "pageSize", Value="10" },
                                new ApiUrlParameter() { Name = "datetime", Value=datetime }
                            };
                            ApiResult result = ApiRequest.CreateApiRequest<List<ViewProductBrand>>(domain, Enum_RequestMethod.GET, Enum_Api.PRODUCTBRAND, parameters);
                            if (result.Value != null)
                            {
                                List<ViewProductBrand> list = (List<ViewProductBrand>)result.Value;
                                int syncCount = 0;
                                foreach (ViewProductBrand item in list)
                                {
                                    string idString = item.Id.ToString();
                                    ProductBrand temp = _context.ProductBrand.FirstOrDefault(p => p.SyncId == idString);
                                    ProductType type = _context.ProductType.GetBySyncId(item.ProductType.Id.ToString());
                                    if (type == null)
                                        break;

                                    listSubmit.Add(item.Id);

                                    bool IsUpdate = false;
                                    if (temp != null)
                                        IsUpdate = true;
                                    else
                                        temp = new ProductBrand();

                                    temp.ProductTypeId = type.ID;
                                    temp.Name = item.Name;
                                    temp.SyncId = item.Id.ToString();
                                    temp.SyncDatetime = item.SyncDatetime;
                                    temp.Active = item.Active;
                                    temp.Label = item.Label;
                                    temp.ShowNumber = item.ShowNumber;

                                    if (IsUpdate)
                                        _context.ProductBrand.Update(temp);
                                    else
                                        _context.ProductBrand.Insert(temp);

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
                catch (Exception) { }
                
                Sync_Base.Is_ProductBrand_Syncing = false;
            }
        }
    }
}
