using DataLayer.Api;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.Base;
using DataLayer.ViewModels;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Schedules
{
    public class Sync_ProductCategory : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            if (Sync_Base.Is_ProductCategory_Syncing == false)
            {
                Sync_Base.Is_ProductCategory_Syncing = true;
                UnitOfWork _context = new UnitOfWork();
                SyncLog log = new SyncLog()
                {
                    Name = "ProductCategory",
                    StartDatetime = DateTime.Now
                };
                string local_type = Enum_Code.SYSTEM_TYPE_LOCAL.ToString();
                List<Website> listWebsite = _context.Website.Where(p => p.Code.Label == local_type).ToList();

                foreach (Website website in listWebsite)
                {
                    foreach (WebsiteDomain domain in website.WebsiteDomain)
                    {
                        string datetime = _context.ProductCategory.GetAll().Max(s => s.SyncDatetime).GetDateString();
                        List<string> listSubmit = new List<string>();
                        List<ApiUrlParameter> parameters = new List<ApiUrlParameter>() {
                            new ApiUrlParameter() { Name = "pageSize", Value="10" },
                            new ApiUrlParameter() { Name = "datetime", Value=datetime }
                        };
                        ApiResult result = ApiRequest.CreateApiRequest<List<ViewProductCategory>>(domain, Enum_RequestMethod.GET, Enum_Api.PRODUCTCATEGORY, parameters);
                        if (result.Value != null)
                        {
                            List<ViewProductCategory> list = (List<ViewProductCategory>)result.Value;
                            int syncCount = 0;
                            foreach (ViewProductCategory item in list)
                            {
                                string idString = item.Id.ToString();
                                ProductCategory temp = _context.ProductCategory.FirstOrDefault(p => p.SyncId == idString);
                                ProductType type = _context.ProductType.GetBySyncId(item.ProductType.Id.ToString());
                                if (type == null)
                                    break;

                                listSubmit.Add(item.Id);

                                bool IsUpdate = false;
                                if (temp != null)
                                    IsUpdate = true;
                                else
                                    temp = new ProductCategory();

                                temp.TypeId = type.ID;
                                temp.Name = item.Name;
                                temp.SyncId = item.Id.ToString();
                                temp.SyncDatetime = item.SyncDatetime;
                                temp.UpdateDatetime = DateTime.Now;

                                if (IsUpdate)
                                    _context.ProductCategory.Update(temp);
                                else
                                    _context.ProductCategory.Insert(temp);

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

                Sync_Base.Is_ProductCategory_Syncing = false;
            }
            
        }
    }
}
