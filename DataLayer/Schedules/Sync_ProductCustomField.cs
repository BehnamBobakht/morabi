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
    public class Sync_ProductCustomField : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            if (Sync_Base.Is_ProductCustomField_Syncing == false)
            {
                Sync_Base.Is_ProductCustomField_Syncing = true;
                try
                {
                    UnitOfWork _context = new UnitOfWork();
                    SyncLog log = new SyncLog()
                    {
                        Name = "ProductCustomField",
                        StartDatetime = DateTime.Now
                    };
                    string local_type = Enum_Code.SYSTEM_TYPE_LOCAL.ToString();
                    List<Website> listWebsite = _context.Website.Where(p => p.Code.Label == local_type).ToList();

                    foreach (Website website in listWebsite)
                    {
                        foreach (WebsiteDomain domain in website.WebsiteDomain)
                        {
                            foreach (ProductCustomField field in _context.ProductCustomField.GetAllForSync())
                            {
                                if (field.SyncName.StartsWith("-") == false)
                                {
                                    string datetime = field.ProductCustomItem.Max(s => s.SyncDatetime).GetDateString();
                                    List<string> listSubmit = new List<string>();
                                    List<ApiUrlParameter> parameters = new List<ApiUrlParameter>() {
                                    new ApiUrlParameter() { Name = "pageSize", Value="10" },
                                    new ApiUrlParameter() { Name = "label", Value=field.SyncName },
                                    new ApiUrlParameter() { Name = "datetime", Value=datetime }
                                };
                                    ApiResult result = ApiRequest.CreateApiRequest<List<ViewSimpleApiValue>>(domain, Enum_RequestMethod.GET, Enum_Api.PRODUCTCUSTOMFIELD, parameters);
                                    if (result.Value != null)
                                    {
                                        List<ViewSimpleApiValue> list = (List<ViewSimpleApiValue>)result.Value;
                                        int syncCount = 0;
                                        foreach (ViewSimpleApiValue item in list)
                                        {
                                            string idString = item.Id.ToString();
                                            ProductCustomItem temp = _context.ProductCustomItem.FirstOrDefault(p => p.SyncId == idString && p.FieldId == field.ID);
                                            listSubmit.Add(item.Id);
                                            if (temp != null)
                                            {
                                                temp.SyncId = item.Id.ToString();
                                                temp.SyncDatetime = item.Datetime;
                                                temp.Value = item.Name;
                                                _context.ProductCustomItem.Update(temp);
                                            }
                                            else
                                            {
                                                temp = new ProductCustomItem();
                                                temp.FieldId = field.ID;
                                                temp.SyncId = item.Id.ToString();
                                                temp.Value = item.Name;
                                                temp.SyncDatetime = item.Datetime;
                                                _context.ProductCustomItem.Insert(temp);
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
                }
                catch (Exception) { }
                Sync_Base.Is_ProductCustomField_Syncing = false;
            }
        }
    }
}
