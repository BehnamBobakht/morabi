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
    public class Sync_ProductSubCategory : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            if (Sync_Base.Is_ProductSubCategory_Syncing == false)
            {
                Sync_Base.Is_ProductSubCategory_Syncing = true;
                try
                {
                    UnitOfWork _context = new UnitOfWork();
                    SyncLog log = new SyncLog()
                    {
                        Name = "ProductSubCategory",
                        StartDatetime = DateTime.Now
                    };
                    string local_type = Enum_Code.SYSTEM_TYPE_LOCAL.ToString();
                    List<Website> listWebsite = _context.Website.Where(p => p.Code.Label == local_type).ToList();

                    foreach (Website website in listWebsite)
                    {
                        foreach (WebsiteDomain domain in website.WebsiteDomain)
                        {
                            string datetime = _context.ProductSubCategory.GetAll().Max(s => s.SyncDatetime).GetDateString();
                            List<ApiUrlParameter> parameters = new List<ApiUrlParameter>() {
                                new ApiUrlParameter() { Name = "pageSize", Value="10" },
                                new ApiUrlParameter() { Name = "datetime", Value=datetime }
                            };
                            ApiResult result = ApiRequest.CreateApiRequest<List<ViewProductSubCategory>>(domain, Enum_RequestMethod.GET, Enum_Api.PRODUCTSUBCATEGORY, parameters);
                            if (result.Value != null)
                            {
                                List<ViewProductSubCategory> list = (List<ViewProductSubCategory>)result.Value;
                                if (list.Count > 0)
                                {
                                    int syncCount = 0;
                                    foreach (ViewProductSubCategory item in list)
                                    {
                                        string idString = item.Id.ToString();
                                        ProductSubCategory temp = _context.ProductSubCategory.FirstOrDefault(p => p.SyncId == idString);
                                        ProductCategory category = _context.ProductCategory.GetBySyncId(item.Category.Id.ToString());
                                        if (category == null)
                                            break;

                                        bool IsUpdate = false;
                                        if (temp != null)
                                            IsUpdate = true;
                                        else
                                            temp = new ProductSubCategory();

                                        temp.CategoryId = category.ID;
                                        temp.Name = item.Name;
                                        temp.SyncId = item.Id.ToString();
                                        temp.SyncDatetime = item.SyncDatetime;
                                        temp.UpdateDatetime = DateTime.Now;

                                        if (IsUpdate)
                                            _context.ProductSubCategory.Update(temp);
                                        else
                                            _context.ProductSubCategory.Insert(temp);

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
                catch (Exception) { }
                Sync_Base.Is_ProductSubCategory_Syncing = false;
            }
        }
    }
}
