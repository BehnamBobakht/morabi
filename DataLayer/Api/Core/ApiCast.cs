using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.ViewModels;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Net;

namespace DataLayer.Api
{
    public static class ApiCast
    {
        public static List<ViewShopResellerProduct> ToApi(this List<ShopResellerProduct> list)
        {
            List<ViewShopResellerProduct> Output = new List<ViewShopResellerProduct>();
            foreach (ShopResellerProduct item in list)
            {
                Output.Add(new ViewShopResellerProduct()
                {
                    Id = item.ID,
                    Product = item.Product.ToApi(),
                    Status = item.Code.ToApi()
                });
            }
            return Output;
        }

        public static List<ViewShopResellerGallery> ToApi(this List<ShopResellerGallery> list)
        {
            List<ViewShopResellerGallery> Output = new List<ViewShopResellerGallery>();
            foreach (ShopResellerGallery item in list)
            {
                Output.Add(new ViewShopResellerGallery()
                {
                    Id = item.ID,
                    Name = item.Name,
                    Description = item.Description,
                    Picture = new ViewPicture(item.Picture),
                    Video = new ViewWebsiteDocument(item.WebsiteDocument)
                });
            }
            return Output;
        }

        public static List<ViewProductType> ToApi(this IEnumerable<ProductType> list, bool extra = true)
        {
            List<ViewProductType> Output = new List<ViewProductType>();
            foreach (ProductType item in list)
            {
                Output.Add(item.ToApi(extra));
            }
            return Output;
        }

        public static ViewProductType ToApi(this ProductType productType, bool extra = true)
        {
            ViewProductType entity = new ViewProductType()
            {
                CreateDatetime = null,
                UpdateDatetime = null,
                SyncDatetime = productType.SyncDatetime,
                Id = productType.ID.ToString(),
                Name = productType.Name,
                //CustomFields = item.ProductCustomField.ToList().ToApi(),

            };
            if (productType.PictureId != null)
                entity.Picture = new ViewPicture(productType.Picture);
            if (extra == true)
                entity.ProductCategories = productType.ProductCategory.ToApi();
            return entity;
        }

        public static List<ViewAccountOrder> ToApi(this List<AccountOrder> list, AccountAddress address = null)
        {
            List<ViewAccountOrder> Output = new List<ViewAccountOrder>();
            foreach (AccountOrder item in list)
            {
                Output.Add(new ViewAccountOrder(item, address));
            }
            return Output;
        }

        public static ViewAccountOrder ToApi(this AccountOrder order)
        {
            ViewAccountOrder entity = new ViewAccountOrder()
            {
                Id = order.ID,
                Status = new ViewCode()
                {
                    Label = order.Code.Label,
                    Name = order.Code.Name
                },
                Account = new ViewAccount()
                {
                    FullName = order.Account.FullName,
                    Email = order.Account.Email,
                    Mobile = order.Account.Mobile
                },
                Datetime = order.Datetime,
                Price = order.Price,
                PaymentType = new ViewPaymentType()
                {
                    Name = order.PaymentType.Name,
                    Label = order.PaymentType.Label
                }
            };
            return entity;
        }

        public static List<ViewProductCustomField> ToApi(this List<ProductCustomField> list)
        {
            List<ViewProductCustomField> Output = new List<ViewProductCustomField>();
            foreach (ProductCustomField item in list)
            {
                ViewProductCustomField entity = new ViewProductCustomField()
                {
                    Id = item.ID,
                    IsRequired = item.IsRequired,
                    Name = item.Name,
                    Type = new ViewCode(item.Code),
                    Items = item.ProductCustomItem.ToApi()
                };
                Output.Add(entity);
            }
            return Output;
        }

        public static List<ViewProductCustomItem> ToApi(this IEnumerable<ProductCustomItem> list)
        {
            List<ViewProductCustomItem> Output = new List<ViewProductCustomItem>();
            foreach (ProductCustomItem item in list)
            {
                ViewProductCustomItem entity = new ViewProductCustomItem()
                {
                    Id = item.ID,
                    Value = item.Value
                };
                Output.Add(entity);
            }
            return Output;
        }

        public static ViewProductCategory ToApi(this ProductCategory productCategory, bool hasChilds = true)
        {
            ViewProductCategory entity = new ViewProductCategory()
            {
                Id = productCategory.ID.ToString(),
                Name = productCategory.Name
            };
            if (productCategory.PictureId != null)
                entity.Picture = new ViewPicture(productCategory.Picture, Enum_Code.SYSTEM_TYPE_PANEL);
            if (hasChilds)
                entity.SubCategories = productCategory.ProductSubCategory.ToApi();
            return entity;
        }

        public static List<ViewProductCategory> ToApi(this IEnumerable<ProductCategory> list, bool hasChilds = true)
        {
            List<ViewProductCategory> Output = new List<ViewProductCategory>();
            foreach (ProductCategory item in list)
            {
                Output.Add(item.ToApi(hasChilds));
            }
            return Output;
        }

        public static List<ViewProductSubCategory> ToApi(this IEnumerable<ProductSubCategory> list, bool? extra = true)
        {
            List<ViewProductSubCategory> Output = new List<ViewProductSubCategory>();
            foreach (ProductSubCategory item in list)
            {
                Output.Add(new ViewProductSubCategory(item, extra));
            }
            return Output;
        }
        public static List<ViewStore> ToApi(this IEnumerable<Store> list, AccountAddress address = null)
        {
            List<ViewStore> Output = new List<ViewStore>();
            foreach (Store item in list)
            {
                Output.Add(new ViewStore(item, address: address));
            }
            return Output;
        }
        public static ViewProductSubCategory ToApi(this ProductSubCategory productSubCategory)
        {
            ViewProductSubCategory entity = new ViewProductSubCategory()
            {
                Id = productSubCategory.ID.ToString(),
                Name = productSubCategory.Name
            };
            if (productSubCategory.PictureId != null)
                entity.Picture = new ViewPicture(productSubCategory.Picture, Enum_Code.SYSTEM_TYPE_PANEL);
            return entity;
        }

        public static List<ViewProductBrand> ToApi(this IEnumerable<ProductBrand> list, bool extra = true)
        {
            List<ViewProductBrand> Output = new List<ViewProductBrand>();
            foreach (ProductBrand item in list)
            {
                Output.Add(new ViewProductBrand(item, extra));
            }
            return Output;
        }
        public static List<ViewProductOption> ToApi(this IEnumerable<ProductOption> list, bool extra = true)
        {
            List<ViewProductOption> Output = new List<ViewProductOption>();
            foreach (ProductOption item in list)
            {
                Output.Add(new ViewProductOption(item, extra));
            }
            return Output;
        }

        public static ViewProductBrand ToApi(this ProductBrand productBrand)
        {
            ViewProductBrand entity = new ViewProductBrand()
            {
                Id = productBrand.ID.ToString(),
                Name = productBrand.Name,
                Active = productBrand.Active,
                Description = productBrand.Description,
                IsPublic = productBrand.IsPublic,
                Label = productBrand.Label,
                ThemeColor = productBrand.ThemeColor,
                ShowNumber = productBrand.ShowNumber.GetValueOrDefault()
            };
            return entity;
        }

        public static List<ViewProductCustomValue> ToApi(this IEnumerable<ProductCustomValue> list)
        {
            List<ViewProductCustomValue> Output = new List<ViewProductCustomValue>();
            foreach (ProductCustomValue item in list)
            {
                ViewProductCustomValue entity = new ViewProductCustomValue()
                {
                    Id = item.ID,
                    Field = new ViewProductCustomField()
                    {
                        Type = new ViewCode()
                        {
                            Label = item.ProductCustomField != null ? item.ProductCustomField.Code.Label : ""
                        },
                        Name = item.ProductCustomField != null ? item.ProductCustomField.Name : "",
                        SyncName = item.ProductCustomField != null ? item.ProductCustomField.SyncName : ""
                    },
                    Value = item.Value
                };
                if (item.ItemId != null)
                {
                    entity.Item = new ViewProductCustomItem()
                    {
                        Value = item.ProductCustomItem.Value
                    };
                    entity.Value = entity.Value == null ? item.ProductCustomItem.Value : null;
                }
                Output.Add(entity);
            }
            return Output;
        }

        public static List<ViewProduct> ToApi(this IEnumerable<Product> list, bool? extra = true)
        {
            List<ViewProduct> Output = new List<ViewProduct>();
            foreach (Product item in list)
            {

                Discount discount = item.GetDiscountObject();
                ViewProduct entity = new ViewProduct()
                {
                    CreateDateTime = item.CreateDateTime,
                    UpdateDatetime = null,
                    Id = item.ID.ToString(),
                    LastPrice = item.LastPrice,
                    MinOrder = item.MinOrder,
                    Name = item.GetName(),
                    Price = item.Price.GetValueOrDefault(),
                    MinPrice = item.MinPrice,

                    Rate = item.GetRate(),
                    Status = new ViewCode(item.Code),
                    SyncDatetime = item.SyncDatetime,
                    Quantity = item.Quantity.GetValueOrDefault(),
                    IsService = item.IsService,
                    Latitude = item.Latitude,
                    Longitude = item.Longitude,
                    Summary = item.GetSummery(),
                    Url=item.GetLinkWithUrl(),
                };
                if (BaseWebsite.WebsiteSetting.HasStore)
                {
                    entity.DiscountPrice = 0;
                    entity.DiscountPercent = 0;
                    Discount discountByPrice = item.GetDiscountPriceObject();

                    //entity.DiscountPrice =/* discount != null ?*/ product.GetDiscountPrice() /*: product.Price.GetValueOrDefault()*/;
                    if (discount != null)
                    {
                        entity.DiscountPercent = discount.PercentValue != null ? discount.PercentValue : 0;
                        entity.DiscountPrice = 0;
                    }
                    else if (discountByPrice != null)
                    {
                        entity.DiscountPrice = discountByPrice.PriceValue != null ? discountByPrice.PriceValue.Value : 0;
                        entity.DiscountPercent = 0;
                    }

                    if (entity.DiscountPrice != 0)
                    {
                        if (item.MinPrice == 0)
                        {
                            entity.DiscountValue = item.Price.GetValueOrDefault();

                        }
                        else
                        {
                            entity.DiscountValue = item.MinPrice + entity.DiscountPrice;

                        }
                    }
                    if (entity.DiscountPercent != 0)
                    {
                        if (item.MinPrice == 0)
                        {
                            entity.DiscountValue = item.Price.GetValueOrDefault();

                        }
                        else
                        {
                            int percent = 100 - entity.DiscountPercent.Value;
                            entity.DiscountValue = (item.MinPrice * 100) / percent;

                        }
                    }
                    ////Discount discount = product.GetDiscountObject();
                    //entity.DiscountPrice = /*discount != null ?*/ item.GetDiscountPrice() /*: item.Price.GetValueOrDefault()*/;

                    ////entity.DiscountPrice = discount != null ? (item.Price.GetValueOrDefault() - item.GetDiscountPrice(discount)) : item.Price.GetValueOrDefault();
                    //entity.DiscountPercent = discount != null ? discount.PercentValue : 0;

                    ////entity.DiscountPrice = entity.Price - entity.MinPrice;
                    ////if (entity.DiscountPrice > 0)
                    ////    entity.DiscountPercent = Convert.ToInt32((entity.DiscountPrice * 100) / entity.Price);
                }
                else
                {
                    entity.DiscountPrice = item.Price.GetValueOrDefault() - item.GetDiscountPrice(discount);
                    entity.DiscountPercent = discount != null ? discount.PercentValue : 0;
                }
                if (item.PictureId != null)
                    entity.Picture = new ViewPicture(item.Picture);

                if (extra == true)
                {
                    string SimpleDescription = BaseHtml.ConvertToSimpleText(item.GetDescription());
                    entity.Description = SimpleDescription;
                    entity.ProductType = new ViewProductType(item.ProductType);
                    entity.ProductCategory = new ViewProductCategory(item.ProductCategory);
                    entity.ProductSubCategory = new ViewProductSubCategory(item.ProductSubCategory);
                    entity.Shop = new ViewShop(item.Shop);
                    entity.Items = item.ProductCustomValue.ToApi();
                    entity.Document = new ViewDocument(item.WebsiteDocument);
                }
                entity.Pictures = new List<ViewProductPicture>();
                Output.Add(entity);
            }
            return Output;
        }

        public static List<ViewShopResellerCollection> ToApi(this IEnumerable<ShopResellerCollection> list)
        {
            List<ViewShopResellerCollection> Output = new List<ViewShopResellerCollection>();
            foreach (ShopResellerCollection item in list)
            {
                Output.Add(item.ToApi());
            }
            return Output;
        }

        public static ViewShopResellerCollection ToApi(this ShopResellerCollection collection)
        {
            ViewShopResellerCollection entity = null;
            if (collection != null)
            {
                entity = new ViewShopResellerCollection()
                {
                    Id = collection.ID,
                    Name = collection.Name,
                    Picture = new ViewPicture(collection.Picture)
                };
            }
            return entity;
        }

        public static ViewProductBarcode ToApi(this ProductBarcode barcode)
        {
            ViewProductBarcode entity = null;
            if (barcode != null)
            {
                entity = new ViewProductBarcode()
                {
                    Value = barcode.Value
                };
                if (barcode.ProductId != null)
                {
                    entity.Product = new ViewProduct()
                    {
                        Name = barcode.Product.Name,
                        Id = barcode.ProductId.ToString()
                    };
                }
                if (barcode.ColorId != null)
                {
                    entity.Color = new ViewColor()
                    {
                        Name = barcode.Color.Name,
                        Id = barcode.ColorId.GetValueOrDefault()
                    };
                }
                if (barcode.SizeId != null)
                {
                    entity.Size = new ViewSize()
                    {
                        Name = barcode.Size.Name,
                        Id = barcode.SizeId.GetValueOrDefault()
                    };
                }
            }
            return entity;
        }

        public static ViewProduct ToApi(this Product product, int? accountId = null)
        {

            string SimpleDescription = BaseHtml.ConvertToSimpleText(product.Description);
            //List<ViewStore> stores = new List<ViewStore>();
            //foreach(var item in product.StoreProduct)
            //{
            //    var store = _context.Store.GetById(item.StoreId);
            //    stores.Add(new ViewStore(store));
            //}
            ViewProduct entity = new ViewProduct()
            {
                CreateDateTime = product.CreateDateTime,
                UpdateDatetime = null,
                Description = SimpleDescription,
                Id = product.ID.ToString(),
                LastPrice = product.LastPrice,
                MinOrder = product.MinOrder,
                Name = product.GetName(),
                MinPrice = product.MinPrice,
                //Stores = stores,
                Latitude = product.Latitude,
                Longitude = product.Longitude,
                Rate = product.GetRate(),
                Price = product.Price.GetValueOrDefault(),
                Document = product.DocId != null ? new ViewDocument(product.WebsiteDocument) : null,
                ProductBrand = product.BrandId != null ? new ViewProductBrand(product.ProductBrand, true) : null,
                ProductType = product.ProductTypeId != null ? new ViewProductType(product.ProductType) : null,
                ProductCategory = product.ProductCategoryId != null ? new ViewProductCategory(product.ProductCategory) : null,
                ProductSubCategory = product.ProductSubCategoryId != null ? new ViewProductSubCategory(product.ProductSubCategory) : null,
                Picture = product.PictureId != null ? new ViewPicture(product.Picture) : null,
                Shop = new ViewShop(product.Shop),
                Status = new ViewCode(product.Code),
                SyncDatetime = product.SyncDatetime,
                Items = product.ProductCustomValue.ToApi(),
                Quantity = product.Quantity.GetValueOrDefault(),
                IsService = product.IsService,
                CodeValue = product.CodeValue,
                Url = product.GetLinkWithUrl(),
               
            };
            if ((Base.BaseWebsite.WebsiteSetting.HasStore))
            {
                entity.DiscountPrice = 0;
                entity.DiscountPercent = 0;
                Discount discount = product.GetDiscountObject();
                Discount discountByPrice = product.GetDiscountPriceObject();
                
                //entity.DiscountPrice =/* discount != null ?*/ product.GetDiscountPrice() /*: product.Price.GetValueOrDefault()*/;
                if (discount != null)
                {
                    entity.DiscountPercent = discount.PercentValue != null ? discount.PercentValue : 0;
                    entity.DiscountPrice = 0;
                }
                else if (discountByPrice != null)
                {
                    entity.DiscountPrice = discountByPrice.PriceValue != null ? discountByPrice.PriceValue.Value : 0;
                    entity.DiscountPercent = 0;
                }
               
                if (entity.DiscountPrice != 0)
                {
                    if (product.MinPrice == 0)
                    {
                        entity.DiscountValue = product.Price.GetValueOrDefault();

                    }
                    else
                    {
                        entity.DiscountValue = product.MinPrice + entity.DiscountPrice;

                    }
                }
                if (entity.DiscountPercent != 0)
                {
                    if (product.MinPrice == 0)
                    {
                        entity.DiscountValue = product.Price.GetValueOrDefault();

                    }
                    else
                    {
                        int percent = 100 - entity.DiscountPercent.Value;
                        entity.DiscountValue = (product.MinPrice *100)/percent;

                    }
                }
            }
            else
            {

                entity.DiscountPrice = product.DiscountValue;
                entity.MinPrice = product.MinPrice;
            }

            if (accountId != null)
            {
                entity.IsLiked = product.ProductLike.Any(p => p.AccountId == accountId);
            }

            List<ViewProductPicture> listPicture = new List<ViewProductPicture>();
            if (product.ProductPicture.Count > 0)
            {
                if (product.PictureId != null && product.ProductPicture.Count == 0)
                    listPicture.Add(new ViewProductPicture(product.Picture));

                foreach (ProductPicture pic in product.ProductPicture)
                {
                    listPicture.Add(new ViewProductPicture(pic.Picture, pic.ColorId));
                }
            }
            entity.Pictures = listPicture;

            List<ViewColor> listColor = new List<ViewColor>();
            foreach (ProductColor color in product.ProductColor)
            {
                listColor.Add(new ViewColor
                {
                    Hex = color.Color.HexValue,
                    Name = color.Color.Name,
                    Id = color.ColorId
                });
            }
            entity.Colors = listColor;

            List<ViewSize> listSize = new List<ViewSize>();
            foreach (ProductSize size in product.ProductSize)
            {
                listSize.Add(new ViewSize()
                {
                    Id = size.SizeId,
                    Name = size.Size.Name
                });
            }
            entity.Sizes = listSize;
            if (product.ProductOptionId != null)
            {
                entity.Option = new ViewProductOption(product.ProductOption, extera: true);
            }
            return entity;
        }

        public static ViewProduct ToProductApi(this ProductQuantity quantity)
        {
            Product product = quantity.Product;
            string SimpleDescription = BaseHtml.ConvertToSimpleText(product.Description);
            Discount discount = product.GetDiscountObject();
            ViewProduct entity = new ViewProduct()
            {
                CreateDateTime = product.CreateDateTime,
                UpdateDatetime = null,
                Description = SimpleDescription,
                Id = product.ID.ToString(),
                LastPrice = product.LastPrice,
                MinOrder = product.MinOrder,
                Name = product.Name,
                Price = quantity.Price.GetValueOrDefault(),
                Rate = product.GetRate(),
                DiscountPrice = discount != null ? (quantity.Price.GetValueOrDefault() - quantity.GetDiscountPrice(discount)) : quantity.Price.GetValueOrDefault(),
                DiscountPercent = discount != null ? discount.PercentValue : 0,
                Document = product.DocId != null ? new ViewDocument(product.WebsiteDocument) : null,
                ProductBrand = product.BrandId != null ? new ViewProductBrand(product.ProductBrand, true) : null,
                ProductType = product.ProductTypeId != null ? new ViewProductType(product.ProductType) : null,
                ProductCategory = product.ProductCategoryId != null ? new ViewProductCategory(product.ProductCategory) : null,
                ProductSubCategory = product.ProductSubCategoryId != null ? new ViewProductSubCategory(product.ProductSubCategory) : null,
                Picture = product.PictureId != null ? new ViewPicture(product.Picture) : null,
                Shop = new ViewShop(product.Shop),
                Status = new ViewCode(product.Code),
                SyncDatetime = product.SyncDatetime,
                Items = product.ProductCustomValue.ToApi(),
                Quantity = product.Quantity.GetValueOrDefault()
            };
            List<ViewProductPicture> listPicture = new List<ViewProductPicture>();
            if (product.ProductPicture.Count > 0)
            {
                if (product.PictureId != null && product.ProductPicture.Count == 0)
                    listPicture.Add(new ViewProductPicture(product.Picture));

                foreach (ProductPicture pic in product.ProductPicture)
                {
                    listPicture.Add(new ViewProductPicture(pic.Picture, pic.ColorId));
                }
            }
            entity.Pictures = listPicture;
            List<ViewColor> listColor = new List<ViewColor>();
            foreach (ProductColor color in product.ProductColor)
            {
                listColor.Add(new ViewColor
                {
                    Hex = color.Color.HexValue,
                    Name = color.Color.Name,
                    Id = color.ColorId
                });
            }
            entity.Colors = listColor;

            List<ViewSize> listSize = new List<ViewSize>();
            foreach (ProductSize size in product.ProductSize)
            {
                listSize.Add(new ViewSize()
                {
                    Id = size.SizeId,
                    Name = size.Size.Name
                });
            }
            entity.Sizes = listSize;
            return entity;
        }

        public static List<ViewCategory> ToApi(this IEnumerable<Category> list)
        {
            List<ViewCategory> Output = new List<ViewCategory>();
            foreach (Category item in list)
            {
                ViewCategory entity = new ViewCategory()
                {
                    Id = item.ID,
                    Label = item.Label,
                    Name = item.Name
                };
                Output.Add(entity);
            }
            return Output;
        }

        public static List<ViewShop> ToApi(this IEnumerable<Shop> list)
        {
            List<ViewShop> Output = new List<ViewShop>();
            foreach (Shop item in list)
            {
                ViewShop entity = new ViewShop()
                {
                    Id = item.ID,
                    Name = item.Name,
                    Website = item.Website.Title,
                    Picture = new ViewPicture(item.Picture, Enum_Code.SYSTEM_TYPE_PANEL),
                    Description = item.Description
                };

                List<ViewShopContact> listContact = new List<ViewShopContact>();
                foreach (ShopContact contact in item.ShopContact)
                {
                    listContact.Add(new ViewShopContact()
                    {
                        Id = contact.ID,
                        Type = new ViewCode(contact.Code),
                        Value = contact.Value
                    });
                }
                entity.ShopContacts = listContact;

                Output.Add(entity);
            }
            return Output;
        }

        public static ViewShop ToApi(this Shop shop)
        {
            ViewShop entity = new ViewShop()
            {
                Id = shop.ID,
                Name = shop.Name,
                Website = shop.Website.Title,
                Picture = new ViewPicture(shop.Picture),
                Description = shop.Description
            };

            List<ViewShopContact> listContact = new List<ViewShopContact>();
            foreach (ShopContact contact in shop.ShopContact)
            {
                listContact.Add(new ViewShopContact()
                {
                    Id = contact.ID,
                    Type = new ViewCode(contact.Code),
                    Value = contact.Value
                });
            }
            entity.ShopContacts = listContact;
            return entity;
        }

        public static ViewShopReseller ToApi(this ShopReseller reseller)
        {
            ViewShopReseller entity = new ViewShopReseller()
            {
                Id = reseller.ID,
                AddressValue = reseller.AddressValue,
                City = reseller.CityId != null ? new ViewCity() { Name = reseller.City.Name } : null,
                Name = reseller.Name,
                Website = reseller.Website,
                Phone1 = reseller.Phone1,
                Phone2 = reseller.Phone2,
                Shop = reseller.ShopId != null ? new ViewShop() { Name = reseller.Shop.Name } : null,
                Picture = reseller.PictureId != null ? new ViewPicture(reseller.Picture) : null,
                CoverPicture = reseller.CoverPictureId != null ? new ViewPicture(reseller.Picture1) : null,
                PersonalPicture = reseller.PersonalPictureId != null ? new ViewPicture(reseller.Picture2) : null
            };
            return entity;
        }

        public static List<ViewShopReseller> ToApi(this IEnumerable<ShopReseller> list)
        {
            List<ViewShopReseller> Output = new List<ViewShopReseller>();
            foreach (ShopReseller item in list)
            {
                Output.Add(item.ToApi());
            }
            return Output;
        }

        public static ViewPost ToApi(this Post post)
        {
            string SimpleDescription = BaseHtml.ConvertToSimpleText(post.Body);
            ViewPost entity = new ViewPost()
            {
                Active = post.Active,
                Body = SimpleDescription,
                Category = new ViewCategory(post.Category),
                Id = post.ID,
                Keywords = post.Keywords,
                Name = post.Name,
                Picture = new ViewPicture(post.Picture, Enum_Code.SYSTEM_TYPE_PANEL),
                Summary = post.Summary
            };
            return entity;
        }



        public static List<ViewContactUs> ToApi(this IEnumerable<WebsiteContactForm> contactForm)
        {
            List<ViewContactUs> outPut = new List<ViewContactUs>();

            foreach (var item in contactForm)
            {
                string SimpleDescription = BaseHtml.ConvertToSimpleText(item.Body);
                ViewContactUs entity = new ViewContactUs()
                {
                    Body = SimpleDescription,
                    Email = item.Email,
                    AccountId = item != null ? item.AccountId : null,
                    FullName = item.FullName,
                    Mobile = item.Mobile,
                    Subject = item.Subject,
                    Datetime = item.Datetime
                };
                outPut.Add(entity);
            }
            return outPut;
        }

        public static ViewPayment ToApi(this Payment payment)
        {
            if (payment != null)
            {
                ViewPayment entity = new ViewPayment()
                {
                    Id = payment.ID,
                    Url = BaseWebsite.ShopUrl + "payment/start?id=" + payment.ID+ "&isMobile=true",

                };
                return entity;
            }
            else
                return null;
        }

        public static ViewAccount ToApi(this Account account, AccountAddress address = null)
        {
            if (account != null)
            {
                List<ViewAccountAddress> listAddress = new List<ViewAccountAddress>();
                foreach(var item in account.AccountAddress)
                {
                    listAddress.Add(new ViewAccountAddress(item));
                }
                ViewAccount entity = new ViewAccount()
                {
                    Id = account.ID,
                    Address = account.Address,
                    Agent = account.Agent,
                    AgentPhone = account.AgentPhone,
                    Company = account.Company,
                    CompanyNo = account.CompanyNo,
                    Email = account.Email,
                    Password = account.Mobile,
                    FullName = account.FullName,
                    IsMale = account.IsMale.GetValueOrDefault(),
                    Job = account.Job,
                    Mobile = account.Mobile,
                    NationalCode = account.NationalCode,
                    Phone = account.Phone,
                    ReagentName = account.ReagentName,
                    Type = new ViewCode(account.Code),
                    UniqueId = account.UniqueId,
                    Description = account.Description,
                    ReagentCode = account.ReagentCode,
                    StoreId = account.Store.Any() ? account.Store.First().ID : 0,
                    Store = account.Store.Any() ? new ViewStore(account.Store.First(), true, address) : null,
                    BirthDay = account.BirthDay.HasValue ? account.BirthDay.Value : (DateTime?)null,
                    FaBirthDay = account.BirthDay.HasValue ? account.BirthDay.Value.ToPersian() : null,
                    CardNumber = account.CardNumber,
                    Sheba = account.Sheba,
                    PictureID = account.PictureId,
                    AddressList=listAddress,
                    StateId = account.StateId != null ? account.StateId : 0,
                    CityId = account.CityId != null ? account.CityId : 0,
                    CountryId = 118, /* account.CountryId != null ? account.CountryId : 118*/
                   SpacialAccount=account.SpacialAccount

                };
                return entity;
            }
            else
                return null;
        }

        public static List<ViewPost> ToApi(this IEnumerable<Post> list)
        {
            List<ViewPost> Output = new List<ViewPost>();
            foreach (Post post in list)
            {
                string SimpleDescription = BaseHtml.ConvertToSimpleText(post.GetBody());
                ViewPost entity = new ViewPost()
                {
                    Active = post.Active,
                    Body = SimpleDescription,
                    Category = new ViewCategory(post.Category),
                    Id = post.ID,
                    Keywords = post.Keywords,
                    Name = post.GetName(),
                    Picture = new ViewPicture(post.Picture, Enum_Code.SYSTEM_TYPE_PANEL),
                    Summary = post.GetSummery(),
                    Url = post.GetLinkWithUrl(),
                    Datetime = post.CreateDateTime,
                    DatetimePersian = post.CreateDateTime.ToPersian()
                };
                Output.Add(entity);
            }
            return Output;
        }

        public static List<ViewAccountBasket> ToApi(this IEnumerable<AccountBasket> list)
        {
            List<ViewAccountBasket> Output = new List<ViewAccountBasket>();

            foreach (AccountBasket basket in list)
            {
                ViewAccountBasket entity = new ViewAccountBasket()
                {
                    Id = basket.ID,
                    Account = new ViewAccount()
                    {
                        Id = basket.AccountId.GetValueOrDefault()
                    },
                    Count = basket.Count,
                    Datetime = basket.Datetime,
                    Price = basket.Price,
                    PercentDiscount = basket.ProductDiscount != 0 ? (Convert.ToDouble(basket.ProductDiscount) / Convert.ToDouble(basket.ProductPrice)) * 100 : 0,
                    ProductPrice = basket.ProductPrice,
                    Discount = basket.ProductDiscount,
                    ColorId = basket.ProductColorId,
                    SizeId = basket.ProductSizeId,
                    OptionItemId = basket.ProductOptionItemId,
                    Product = new ViewProduct(basket.Product),
                };

                if (basket.Product != null && basket.Product.PictureId != null)
                    entity.Product.Picture = new ViewPicture(basket.Product.Picture, Enum_Code.SYSTEM_TYPE_PANEL);
                if (basket.Color != null && basket.ProductColorId != null)
                {
                    entity.Color = new ViewColor()
                    {
                        Id = basket.Color.ID,
                        Name = basket.Color.Name,
                        Hex = basket.Color.HexValue
                    };
                }
                if (basket.Size != null && basket.ProductSizeId != null)
                {
                    entity.Size = new ViewSize()
                    {
                        Id = basket.Size.ID,
                        Name = basket.Size.Name
                    };
                }
                if (basket.ProductOptionItemId != null && basket.ProductOptionItem != null)
                {
                    entity.ProductOptionItem = new ViewProductOptionItem()
                    {
                        Id = basket.ProductOptionItem.ID,
                        Value = basket.ProductOptionItem.Value,

                        ProductOption = new ViewProductOption()
                        {
                            Name = basket.ProductOptionItem.ProductOption != null ? basket.ProductOptionItem.ProductOption.Name : ""
                        }
                    };
                }
                if (basket.ShopReseller != null && basket.ResellerId != null)
                {
                    entity.ShopReseller = new ViewShopReseller()
                    {
                        Id = basket.ShopReseller.ID,
                        Name = basket.ShopReseller.Name
                    };
                }
                if (basket.Store != null && basket.StoreId != null)
                {
                    entity.Store = new ViewStore()
                    {
                        ID = basket.Store.ID,
                        Name = basket.Store.Name
                    };
                    entity.StoreId = basket.Store.ID;
                    entity.StoreName = basket.Store.Name;

                }
                Output.Add(entity);
            }


            return Output;
        }

        public static List<ViewAccountBasketStore> ToApiStore(this IEnumerable<AccountBasket> list, UnitOfWork _context, AccountAddress address = null)
        {
            var listByStore = list.GroupBy(p => p.StoreId);
            List<ViewAccountBasketStore> listStore = new List<ViewAccountBasketStore>();
            foreach (var item in listByStore)
            {
                ViewSendTypeStores sendStore = new ViewSendTypeStores();
                if (Base.BaseWebsite.WebsiteSetting.HasStore)
                {
                    sendStore = _context.Account.GetSendTypeList().FirstOrDefault();
                }
                listStore.Add(new ViewAccountBasketStore()
                {

                    Store = new ViewStore()
                    {
                        ID = item.Key.GetValueOrDefault(),
                        Name = item.First().Store != null ? item.First().Store.Name : "",
                        SendTypeStores = sendStore
                    },
                    Products = item.ToApi(),
                    Count = item.Count(),
                    Discount = item.Sum(s => s.ProductDiscount * s.Count),
                    Price = item.Sum(s => s.Price),

                    ShippingCity = _context.ShippingCity.GetShippingPrice(storeId: item.First().StoreId, address),
                    //SendType = new ViewSendType() { CurrentPrice = _context.SendType.GetShippingPrice(item.Key, item.Sum(s => s.Price)) }

                });
            }
            return listStore;
        }


        public static List<ViewCity> ToApi(this IEnumerable<City> list)
        {
            List<ViewCity> Output = new List<ViewCity>();
            foreach (City city in list)
            {
                ViewCity entity = new ViewCity()
                {
                    Id = city.ID,
                    Name = city.GetName()
                };
                Output.Add(entity);
            }
            return Output;
        }
        public static List<ViewShippingCity> ToApi(this IEnumerable<ShippingCity> list)
        {
            List<ViewShippingCity> Output = new List<ViewShippingCity>();
            foreach (ShippingCity shipping in list)
            {
                ViewShippingCity entity = new ViewShippingCity()
                {
                    Id = shipping.ShippingCityId,
                    CityName = shipping.City.Name,
                    StateName = shipping.State.Name,
                    CountyName = shipping.Country.FaName,
                    SendTime = shipping.SendTime,
                    SendPrice = shipping.SendPrice,
                    CountryId = shipping.CountryId.Value,
                    StateId = shipping.StateId.Value,
                    StoreId = shipping.StoreId,
                    CityId = shipping.CityId.Value,
                    SendTimePostP = shipping.SendTimePostP,
                    SendTimePost = shipping.SendTimePost,
                    SendPricePost = shipping.SendPricePost,
                    SendPricePostP = shipping.SendPricePostP


                };
                Output.Add(entity);
            }
            return Output;
        }
        public static ViewStore ToApi(this Store store)
        {


            ViewStore entity = new ViewStore()
            {
                ID = store.ID,
                Name = store.Name,
                Description = store.Description,
                Phone1 = store.Phone1,
                Picture = store.Picture != null ? new ViewPicture(store.Picture) : null,
                //Comments = store.StoreComment,
                Active = store.Active,
                AddressValue = store.AddressValue,
            };


            return entity;
        }
        public static List<ViewState> ToApi(this IEnumerable<State> list)
        {
            List<ViewState> Output = new List<ViewState>();
            foreach (State state in list)
            {
                ViewState entity = new ViewState()
                {
                    Id = state.ID,
                    Name = state.GetName()
                };
                Output.Add(entity);
            }
            return Output;
        }
        public static List<ViewCurrency> ToApi(this IEnumerable<Currency> list)
        {
            List<ViewCurrency> Output = new List<ViewCurrency>();
            foreach (Currency currency in list)
            {
                ViewCurrency entity = new ViewCurrency()
                {
                    CurrencyId = currency.CurrencyId,
                    CurrencyName = currency.CurrencyName,
                    CurrencySign = currency.CurrencySign,
                    UnitPrice = currency.UnitPrice
                };
                Output.Add(entity);
            }
            return Output;
        }

        public static List<ViewCountry> ToApi(this IEnumerable<Country> list)
        {
            List<ViewCountry> Output = new List<ViewCountry>();
            foreach (Country country in list)
            {
                ViewCountry entity = new ViewCountry()
                {
                    Id = country.ID,
                    FaName = country.FaName
                };
                Output.Add(entity);
            }
            return Output;
        }

        public static List<ViewAccountAddress> ToApi(this IEnumerable<AccountAddress> list)
        {
            List<ViewAccountAddress> Output = new List<ViewAccountAddress>();
            foreach (AccountAddress address in list)
            {
                ViewAccountAddress entity = new ViewAccountAddress()
                {
                    Id = address.ID,
                    AddressValue = address.AddressValue,
                    Latitude = address.Latitude,
                    Longitude = address.Longitude,
                    NameFamily = address.NameFamily,
                    Mobile = address.Mobile,
                    PostalCode = address.PostalCode,
                    NationalCode = address.NationalCode,
                    Phone = address.Phone,
                    LegalUser=address.LegalUser,
                    City = new ViewCity(address.City),
                    State = new ViewState(address.State),
                    Country = new ViewCountry(address.Country)

                };
                Output.Add(entity);
            }
            return Output;
        }

        public static List<ViewMerchant> ToApi(this IEnumerable<Merchant> list)
        {
            string panel_url = BaseWebsite.PanelUrl;
            List<ViewMerchant> Output = new List<ViewMerchant>();
            foreach (Merchant merchant in list)
            {
                ViewMerchant entity = new ViewMerchant()
                {
                    Id = merchant.ID,
                    Active = merchant.Active,
                    Bank = new ViewBank()
                    {
                        Id = merchant.Bank.ID,
                        Name = merchant.Bank.Name,
                        Label = merchant.Bank.Label,
                        PictureUrl = BaseWebsite.ShopUrl + merchant.Bank.PictureUrl
                    }
                };
                Output.Add(entity);
            }
            return Output;
        }

        public static List<ViewSlider> ToApi(this IEnumerable<Slider> list)
        {
            string panel_url = BaseWebsite.PanelUrl;
            List<ViewSlider> Output = new List<ViewSlider>();
            foreach (Slider slider in list)
            {
                ViewSlider entity = new ViewSlider()
                {
                    Id = slider.ID,
                    Name = slider.Name,
                    Picture = new ViewPicture(slider.Picture, Enum_Code.SYSTEM_TYPE_PANEL)
                };
                Output.Add(entity);
            }
            return Output;
        }

        public static ViewMenu ToApi(this Menu menu)
        {
            ViewMenu entity = new ViewMenu()
            {
                Id = menu.ID,
                Name = menu.Name,
                Link = menu.Link,
                Picture = menu.PictureId == null ? null : new ViewPicture(menu.Picture, Enum_Code.SYSTEM_TYPE_PANEL),
                Type = new ViewCode()
                {
                    Label = menu.Code.Label,
                    Name = menu.Name
                }
            };
            return entity;
        }

        public static List<ViewMenu> ToApi(this IEnumerable<Menu> list, bool hasChilds)
        {
            List<ViewMenu> Output = new List<ViewMenu>();
            foreach (Menu menu in list)
            {
                ViewMenu entity = menu.ToApi();
                if (hasChilds == true)
                {
                    entity.Childs = menu.Menu1.ToApi(hasChilds);
                }
                Output.Add(entity);
            }
            return Output;
        }

        public static List<ViewBanner> ToApi(this IEnumerable<Banner> list)
        {
            string panel_url = BaseWebsite.PanelUrl;
            List<ViewBanner> Output = new List<ViewBanner>();
            foreach (Banner banner in list)
            {
                ViewBanner entity = new ViewBanner()
                {
                    Id = banner.ID,
                    Name = banner.Name,
                    Picture = new ViewPicture(banner.Picture, Enum_Code.SYSTEM_TYPE_PANEL),
                    Category = new ViewCategory(banner.Category),
                    ShowNumber = banner.ShowNumber.GetValueOrDefault(),
                    Summary = banner.Summary,
                    Url = banner.Url
                };
                Output.Add(entity);
            }
            return Output;
        }

        public static List<ViewProductComment> ToApi(this IEnumerable<ProductComment> list)
        {
            string panel_url = BaseWebsite.PanelUrl;
            List<ViewProductComment> Output = new List<ViewProductComment>();
            foreach (ProductComment comment in list)
            {
                ViewProductComment entity = new ViewProductComment()
                {
                    Id = comment.ID,
                    Approved = comment.Approved,
                    Account = new ViewAccount()
                    {
                        FullName = comment.Account.FullName
                    },
                    Body = comment.Body,
                    
                    Datetime = comment.Datetime,
                    EmailAddress = comment.EmailAddress,
                    NameFamily = comment.NameFamily,
                    AnswerDatetime = comment.AnswerDatetime,
                    AnswerString = comment.AnswerString,
                    Product = new ViewProduct()
                    {
                        Id = comment.ProductId.ToString(),
                        Name = comment.Product.Name,
                        ProductCategory=comment.Product.ProductCategory!=null? new ViewProductCategory()
                        {
                            Id=comment.Product.ProductCategory.ID.ToString()
                        }:null,
                        ProductSubCategory = comment.Product.ProductSubCategory != null ? new ViewProductSubCategory()
                        {
                            Id = comment.Product.ProductSubCategory.ID.ToString()
                        }:null,
                        ProductType = comment.Product.ProductType!=null ?new ViewProductType()
                        {
                            Id = comment.Product.ProductType.ID.ToString()
                        }:null,
                        Picture =new ViewPicture()
                        {
                            Id=comment.Product.Picture.ID,
                            Url=comment.Product.Picture.GetUrl()
                        }
                    },
                    Rate = comment.Rate
                };
                Output.Add(entity);
            }
            return Output;
        }
        public static List<ViewStoreComment> ToApi(this IEnumerable<StoreComment> list)
        {
            string panel_url = BaseWebsite.PanelUrl;
            List<ViewStoreComment> Output = new List<ViewStoreComment>();
            foreach (StoreComment comment in list)
            {
                ViewStoreComment entity = new ViewStoreComment()
                {
                    Id = comment.ID,
                    Approved = comment.Approved,
                    Account = new ViewAccount()
                    {
                        FullName = comment.Account.FullName
                    },
                    Picture=comment.Store.Picture!=null? new ViewPicture()
                    {
                        Id = comment.Store.Picture.ID,
                        Url = comment.Store.Picture.GetUrl()
                    }:null,
                    Body = comment.Body,
                    Datetime = comment.Datetime,
                    EmailAddress = comment.EmailAddress,
                    NameFamily = comment.NameFamily,
                    AnswerDatetime = comment.AnswerDatetime,
                    AnswerString = comment.AnswerString,
                    Store = new ViewStore()
                    {
                        ID = comment.StoreId,
                        Name = comment.Store.Name
                    },
                    Rate = comment.Rate
                };
                Output.Add(entity);
            }
            return Output;
        }
        public static List<ViewProductVisitModel> ToApi(this IEnumerable<ProductVisit> list)
        {
            string panel_url = BaseWebsite.PanelUrl;
            List<ViewProductVisitModel> Output = new List<ViewProductVisitModel>();
            foreach (ProductVisit visit in list)
            {
                Discount discount = visit.Product.GetDiscountObject();
                double? DiscountPrice = 0, DiscountPercent = 0;double ?  DiscountValue=0;
                if (BaseWebsite.WebsiteSetting.HasStore)
                {

                    Discount discountByPrice = visit.Product.GetDiscountPriceObject();

                    //entity.DiscountPrice =/* discount != null ?*/ product.GetDiscountPrice() /*: product.Price.GetValueOrDefault()*/;
                    if (discount != null)
                    {
                        DiscountPercent = discount.PercentValue != null ? discount.PercentValue : 0;
                        DiscountPrice = 0;
                    }
                    else if (discountByPrice != null)
                    {
                        DiscountPrice = discountByPrice.PriceValue != null ? discountByPrice.PriceValue.Value : 0;
                        DiscountPercent = 0;
                    }

                    if (DiscountPrice != 0)
                    {
                        if (visit.Product.MinPrice == 0)
                        {
                            DiscountValue = visit.Product.Price.GetValueOrDefault();

                        }
                        else
                        {
                            DiscountValue = visit.Product.MinPrice + DiscountPrice;

                        }
                    }

                    if (DiscountPercent != 0)
                    {
                        if (visit.Product.MinPrice == 0)
                        {
                            DiscountValue = visit.Product.Price.GetValueOrDefault();

                        }
                        else
                        {
                            double percent = 100 - DiscountPercent.Value;
                            DiscountValue = (visit.Product.MinPrice * 100) / percent;

                        }
                    }
                }
                else
                {
                    DiscountPrice = visit.Product.Price.GetValueOrDefault() - visit.Product.GetDiscountPrice(discount);
                    DiscountPercent = discount != null ? discount.PercentValue : 0;

                }

                ViewProductVisitModel entity = new ViewProductVisitModel()
                {

                    Account = new ViewAccount()
                    {
                        FullName = visit.Account.FullName,
                        Id = visit.Account.ID
                    },
                     
                    Product = new ViewProduct()
                    {
                        CreateDateTime = visit.Product.CreateDateTime,
                        UpdateDatetime = null,
                        Description = visit.Product.Description,
                        Id = visit.Product.ID.ToString(),
                        LastPrice = visit.Product.LastPrice,
                        MinPrice=visit.Product.MinPrice,
                        MinOrder = visit.Product.MinOrder,
                        Name = visit.Product.GetName(),
                        //Stores = stores,
                        Rate = visit.Product.GetRate(),
                        Price = visit.Product.Price.GetValueOrDefault(),
                        Document = visit.Product.DocId != null ? new ViewDocument(visit.Product.WebsiteDocument) : null,
                        ProductBrand = visit.Product.BrandId != null ? new ViewProductBrand(visit.Product.ProductBrand, true) : null,
                        ProductType = visit.Product.ProductTypeId != null ? new ViewProductType(visit.Product.ProductType) : null,
                        ProductCategory = visit.Product.ProductCategoryId != null ? new ViewProductCategory(visit.Product.ProductCategory) : null,
                        ProductSubCategory = visit.Product.ProductSubCategoryId != null ? new ViewProductSubCategory(visit.Product.ProductSubCategory) : null,
                        Picture = visit.Product.PictureId != null ? new ViewPicture(visit.Product.Picture) : null,
                        Shop = new ViewShop(visit.Product.Shop),
                        Status = new ViewCode(visit.Product.Code),
                        SyncDatetime = visit.Product.SyncDatetime,
                        Items = visit.Product.ProductCustomValue.ToApi(),
                        Quantity = visit.Product.Quantity.GetValueOrDefault(),
                        IsService = visit.Product.IsService,
                        CodeValue = visit.Product.CodeValue,
                        Url = visit.Product.GetLinkWithUrl(),
                        DiscountPercent = DiscountPercent !=null ? (int)DiscountPercent : 0,
                        DiscountValue = DiscountValue!=null? DiscountValue.Value: 0,
                        DiscountPrice= DiscountPrice != null ? DiscountPrice.Value : 0,
                    },
                };

                Output.Add(entity);
            }
            return Output;
        }

        public static List<ViewSendType> ToApi(this IEnumerable<SendType> list)
        {
            List<ViewSendType> Output = new List<ViewSendType>();
            foreach (SendType sendType in list)
            {
                Output.Add(new ViewSendType(sendType));
            }
            return Output;
        }

        public static List<ViewProductLike> ToApi(this IEnumerable<ProductLike> list)
        {
            List<ViewProductLike> listOutput = new List<ViewProductLike>();
            foreach (ProductLike item in list)
            {
                Product product = item.Product;
                listOutput.Add(new ViewProductLike()
                {
                    Id = item.ID,
                    Product = item.Product.ToApi()
                });
            }
            return listOutput;
        }
        public static List<ViewProductQuantity> ToApi(this IEnumerable<ProductQuantity> list)
        {
            List<ViewProductQuantity> Output = new List<ViewProductQuantity>();
            foreach (ProductQuantity productQuantity in list)
            {
                var item = new ViewProductQuantity(productQuantity);
                Output.Add(item);
            }
            return Output;
        }
        public static List<ViewStoreProductQuantity> ToApi(this IEnumerable<StoreProductQuantity> list, int count = 1, AccountAddress address = null)
        {
            List<ViewStoreProductQuantity> Output = new List<ViewStoreProductQuantity>();
            foreach (StoreProductQuantity storeProductQuantity in list)
            {
                var item = new ViewStoreProductQuantity(storeProductQuantity, count, address);
                item.Discount = DataLayer.Base.BaseStore.GetDiscountPrice(storeProductQuantity, count);
                item.BasePrice = item.Price;
                item.Price = item.BasePrice - item.Discount;
                item.PriceRanges = new List<ViewPriceRange>();
                foreach (var range in storeProductQuantity.PriceRange)
                {
                    item.PriceRanges.Add(new ViewPriceRange(range));
                }
                Output.Add(item);

            }
            return Output;
        }

        public static List<ViewColor> ToApi(this IEnumerable<Color> list, bool extra = true)
        {
            List<ViewColor> Output = new List<ViewColor>();
            foreach (Color color in list)
            {
                Output.Add(new ViewColor(color, extra));
            }
            return Output;
        }

        public static List<ViewSize> ToApi(this IEnumerable<Size> list, bool extra = true)
        {
            List<ViewSize> Output = new List<ViewSize>();
            foreach (Size size in list)
            {
                Output.Add(new ViewSize(size, extra));
            }
            return Output;
        }
        public static List<ViewProductOptionItem> ToApi(this IEnumerable<ProductOptionItem> list, bool extra = true)
        {
            List<ViewProductOptionItem> Output = new List<ViewProductOptionItem>();
            foreach (ProductOptionItem productOptionItem in list)
            {
                Output.Add(new ViewProductOptionItem(productOptionItem, extra));
            }
            return Output;
        }
        public static ViewCode ToApi(this Code code)
        {
            ViewCode entity = new ViewCode()
            {
                Id = code.ID,
                Name = code.Name,
                Label = code.Label
            };
            return entity;
        }

    }
}
