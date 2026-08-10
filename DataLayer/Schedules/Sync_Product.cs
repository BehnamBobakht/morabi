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
    public class Sync_Product : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            if (Sync_Base.Is_Product_Syncing == false)
            {
                Sync_Base.Is_Product_Syncing = true;
                try
                {
                    UnitOfWork _context = new UnitOfWork();
                    SyncLog log = new SyncLog()
                    {
                        Name = "Product",
                        StartDatetime = DateTime.Now
                    };
                    string local_type = Enum_Code.SYSTEM_TYPE_LOCAL.ToString();
                    List<Website> listWebsite = _context.Website.Where(p => p.Code.Label == local_type).ToList();
                    foreach (Website website in listWebsite)
                    {
                        string datetime = _context.Product.Max(p => p.SyncDatetime).GetDateString();
                        // string datetime = "2019-01-30 11:55:59.000";
                        List<ApiUrlParameter> parameters = new List<ApiUrlParameter>() {
                                new ApiUrlParameter() { Name = "pageSize", Value="10" },
                                new ApiUrlParameter() { Name = "datetime", Value=datetime }
                            };

                        foreach (WebsiteDomain domain in website.WebsiteDomain)
                        {
                            // domain.Domain = "http://192.168.10.10/api/";
                            List<ViewProduct> list = new List<ViewProduct>();
                            try
                            {
                                ApiResult result = ApiRequest.CreateApiRequest<List<ViewProduct>>(domain, Enum_RequestMethod.GET, Enum_Api.PRODUCT, parameters);
                                list = (List<ViewProduct>)result.Value;
                            }
                            catch (Exception) {
                                continue;
                            }
                            if (list != null && list.Count > 0)
                            {
                                int syncCount = 0;
                                List<Code> listProductStatus = _context.Code.GetAllByCodeGroup(Enum_CodeGroup.PRODUCT_STATUS);
                                List<Shop> listShop = _context.Shop.GetAll();
                                List<ProductType> listTypes = _context.ProductType.GetAllHasSyncId();
                                List<ProductBrand> listBrands = _context.ProductBrand.GetAllHasSyncId();
                                List<ProductCategory> listCategories = _context.ProductCategory.GetAllHasSyncId();
                                List<ProductCustomField> AllFields = _context.ProductCustomField.GetAllForSync();
                                List<ProductSubCategory> listSubCategories = _context.ProductSubCategory.GetAllHasSyncId();
                                
                                foreach (ViewProduct item in list)
                                {
                                    string idString = item.Id.ToString();
                                    Product product = _context.Product.FirstOrDefault(p => p.SyncId == idString);
                                    Code status = listProductStatus.FirstOrDefault(p => p.Label == item.Status.Label);
                                    Shop shop = item.Shop != null ? _context.Shop.GetByLabel(item.Shop.Label) : null;
                                    ProductType type = item.ProductType != null ? listTypes.FirstOrDefault(p => p.SyncId == item.ProductType.Id.ToString()) : null;
                                    ProductBrand brand = item.ProductBrand != null ? listBrands.FirstOrDefault(p => p.SyncId == item.ProductBrand.Id.ToString()) : null;
                                    ProductCategory category = item.ProductCategory != null ? listCategories.FirstOrDefault(p => p.SyncId == item.ProductCategory.Id.ToString()) : null;
                                    ProductSubCategory subCategory = item.ProductSubCategory != null ? listSubCategories.FirstOrDefault(p => p.SyncId == item.ProductSubCategory.Id.ToString()) : null;
                                    Picture picture = null;
                                    
                                    if ((item.ProductType != null && type == null) ||
                                        (item.ProductBrand != null && brand == null) ||
                                        (item.ProductCategory != null && category == null) ||
                                        (item.ProductSubCategory != null && subCategory == null) ||
                                        (item.Shop != null && shop == null))
                                        break;

                                    bool IsInsert = true;
                                    try
                                    {
                                        if (product != null)
                                        {
                                            string lastStatus = product.Code.Label;
                                            if (item.Picture != null)
                                            {
                                                if (product.PictureId == null)
                                                {
                                                    picture = new Picture()
                                                    {
                                                        Url = Enum_Code.SYSTEM_TYPE_PANEL + item.Picture.Url
                                                    };
                                                    _context.Picture.Insert(picture);
                                                    _context.Save();
                                                    product.PictureId = picture.ID;
                                                }
                                            }
                                            else
                                                product.PictureId = null;

                                            product.Summary = item.Summary;
                                            product.SyncId = item.Id.ToString();
                                            product.ShopId = shop != null ? shop.ID : default(int?);
                                            product.ProductTypeId = type != null ? type.ID : default(int?);
                                            product.BrandId = brand != null ? brand.ID : default(int?);
                                            product.ProductCategoryId = category != null ? category.ID : default(int?);
                                            product.ProductSubCategoryId = subCategory != null ? subCategory.ID : default(int?);
                                            product.SyncDatetime = item.SyncDatetime;
                                            product.Name = item.Name;
                                            product.Description = item.Description;
                                            product.Price = item.Price;
                                            product.LastPrice = item.Price;
                                            product.MinOrder = item.MinOrder;
                                            product.StatusId = status.ID;
                                            product.UpdateDatetime = DateTime.Now;
                                            product.Active = item.Active;
                                            product.Quantity = item.Quantity;
                                            product.CodeValue = item.CodeValue;
                                            _context.Product.Update(product);
                                            bool productResult = _context.Save();
                                            
                                            if (lastStatus != Enum_Code.PRODUCT_STATUS_AVAILABLE.ToString() && status.Label == Enum_Code.PRODUCT_STATUS_AVAILABLE.ToString())
                                            {
                                                string token_1 = product.Name;
                                                string token_2 = BaseWebsite.ShopUrl + "/pr/" + product.ID + "/1";
                                                string token_3 = "";

                                                StringBuilder sms = new StringBuilder();
                                                sms.AppendLine("مشتری گرامی، محصول مورد نظر شما " + token_1 + " در سایت " + BaseWebsite.ShopWebsite.Title + " موجود شد");
                                                sms.Append(token_2);
                                                foreach (ProductNotify notify in product.ProductNotify)
                                                {
                                                    _context.Sms.SaveNewSms(notify.Account.Mobile, Enum_SmsType.PRODUCT_NOTIFY, sms.ToString(), token_1, token_2, token_3);
                                                    _context.Email.SaveNewEmail(notify.Account.Email, Enum_EmailType.PRODUCT_NOTIFY, sms.ToString(), "موجود شدن محصول " + notify.Product.Name);
                                                }
                                                _context.Save();
                                            }
                                            
                                            foreach (ViewProductCustomValue custom in item.Items)
                                            {
                                                ProductCustomField field = AllFields.FirstOrDefault(p => p.SyncName == custom.Field.Type.Label);
                                                if (field != null)
                                                {
                                                    List<ProductCustomValue> listValue = product.ProductCustomValue.ToList();
                                                    ProductCustomValue tempValue = listValue.FirstOrDefault(p => p.FieldId == field.ID);
                                                    if (tempValue != null)
                                                    {
                                                        if (field.Code.Label == Enum_Code.FIELD_TYPE_DROPDOWN.ToString())
                                                        {
                                                            string ItemId = custom.Value;
                                                            ProductCustomItem thisItem = field.ProductCustomItem.FirstOrDefault(s => s.SyncId == ItemId);
                                                            if (thisItem != null && tempValue.ItemId != thisItem.ID)
                                                            {
                                                                tempValue.ItemId = thisItem.ID;
                                                                _context.ProductCustomValue.Update(tempValue);
                                                            }
                                                            else
                                                                continue;
                                                        }
                                                        else
                                                        {
                                                            if (tempValue.Value != custom.Value)
                                                            {
                                                                tempValue.Value = custom.Value;
                                                                _context.ProductCustomValue.Update(tempValue);
                                                            }
                                                            else
                                                                continue;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        ProductCustomValue tempItem = new ProductCustomValue()
                                                        {
                                                            FieldId = field.ID,
                                                            ProductId = product.ID,
                                                            SyncDatetime = item.SyncDatetime,
                                                            SyncId = item.Id.ToString(),
                                                            Value = ""
                                                        };
                                                        if (field.Code.Label == Enum_Code.FIELD_TYPE_DROPDOWN.ToString() && custom.Value != null)
                                                        {
                                                            string ItemId = custom.Value;
                                                            ProductCustomItem thisItem = field.ProductCustomItem.FirstOrDefault(s => s.SyncId == ItemId);
                                                            if (thisItem != null)
                                                                tempItem.ItemId = thisItem.ID;
                                                            else
                                                                continue;
                                                        }
                                                        else
                                                            tempItem.Value = custom.Value;
                                                        _context.ProductCustomValue.Insert(tempItem);
                                                    }
                                                }
                                            }
                                            _context.Save();

                                            IsInsert = false;
                                        }
                                        else
                                        {
                                            if (item.Picture != null)
                                            {
                                                picture = new Picture()
                                                {
                                                    Url = Enum_Code.SYSTEM_TYPE_PANEL + item.Picture.Url
                                                };
                                                _context.Picture.Insert(picture);
                                                _context.Save();
                                            }
                                            product = new Product()
                                            {
                                                SyncId = item.Id.ToString(),
                                                Name = item.Name,
                                                ShopId = shop != null ? shop.ID : default(int?),
                                                ProductTypeId = type != null ? type.ID : default(int?),
                                                BrandId = brand != null ? brand.ID : default(int?),
                                                ProductCategoryId = category != null ? category.ID : default(int?),
                                                ProductSubCategoryId = subCategory != null ? subCategory.ID : default(int?),
                                                PictureId = picture != null ? picture.ID : default(int?),
                                                SyncDatetime = item.SyncDatetime,
                                                VisitCount = 0,
                                                ShowNumber = 0,
                                                Summary = item.Summary,
                                                UpdateDatetime = DateTime.Now,
                                                ShowHomePage = false,
                                                Description = item.Description,
                                                Price = item.Price,
                                                LastPrice = item.LastPrice,
                                                MinOrder = item.MinOrder,
                                                StatusId = status.ID,
                                                Active = item.Active,
                                                Quantity = item.Quantity,
                                                CodeValue = item.CodeValue
                                            };
                                            _context.Product.Insert(product);
                                            bool resultSave = _context.Save();
                                            foreach (ViewProductCustomValue custom in item.Items)
                                            {
                                                ProductCustomField field = AllFields.FirstOrDefault(p => p.SyncName == custom.Field.Type.Label);
                                                if (field != null)
                                                {
                                                    ProductCustomValue tempItem = new ProductCustomValue()
                                                    {
                                                        FieldId = field.ID,
                                                        ProductId = product.ID,
                                                        SyncDatetime = item.SyncDatetime,
                                                        SyncId = item.Id.ToString(),
                                                        Value = ""
                                                    };
                                                    if (field.Code.Label == Enum_Code.FIELD_TYPE_DROPDOWN.ToString() && custom.Value != null)
                                                    {
                                                        string ItemId = custom.Value;
                                                        ProductCustomItem thisItem = field.ProductCustomItem.FirstOrDefault(s => s.SyncId == ItemId);
                                                        if (thisItem != null)
                                                            tempItem.ItemId = thisItem.ID;
                                                        else
                                                            continue;
                                                    }
                                                    else
                                                        tempItem.Value = custom.Value;
                                                    _context.ProductCustomValue.Insert(tempItem);
                                                }
                                                else
                                                {
                                                    if (IsInsert && product.ID != 0)
                                                    {
                                                        _context.ProductCustomValue.DeleteByProductId(product.ID);
                                                        _context.Save();
                                                        _context.Product.Delete(product);
                                                        _context.Save();
                                                        break;
                                                    }
                                                }
                                                // SAVE WAS HERE
                                            }
                                            _context.Save();
                                        }

                                        syncCount++;
                                    }
                                    catch (Exception ex)
                                    {
                                        if (IsInsert && product.ID != 0)
                                        {
                                            _context.ProductCustomValue.DeleteByProductId(product.ID);
                                            _context.Save();
                                            _context.Product.Delete(product);
                                            _context.Save();
                                        }

                                        SyncLog errorLog = new SyncLog()
                                        {
                                            Description = "PRODUCT-" + item.Id + "-" + ex.Message,
                                            StartDatetime = log.StartDatetime,
                                            EndDatetime = DateTime.Now,
                                            Name = "Product",
                                            Status = "ERROR"
                                        };
                                        _context.SyncLog.Insert(errorLog);
                                        _context.Save();
                                        break;
                                    }
                                }

                                if (syncCount > 0)
                                {
                                    log.Description = syncCount.ToString();
                                    log.EndDatetime = DateTime.Now;
                                    log.Status = "OK";
                                    _context.SyncLog.Insert(log);
                                    var logResult = _context.Save();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex) {
                    string msg = ex.Message;
                }
                Sync_Base.Is_Product_Syncing = false;
            }
        }
    }
}