using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;
using DataLayer.ViewModels;
using ExpressMapper;
using ExpressMapper.Extensions;

namespace DataLayer.Base
{
    public class MapAll
    {
        public static void Maping()
        {
            Mapper.Register<Cart, CartVm>()

                .Member(dest => dest.CartStoreList, src => src.CartStore)
                 .Function(dest => dest.AddressVm, src => src.AccountAddress)
                .Function(dest => dest.DeliveryList, src =>
                                {
                                    if (BaseWebsite.WebsiteSetting.HasCentralizedDelivery)
                                    {
                                        var accountAddress = src.AccountAddress;
                                        if (accountAddress != null)
                                        {
                                            var deliveries = BaseDelivery.GetPossibleDeliveries(storeId: null, price: src.TotalPrice - src.RebatePrice, cityId: accountAddress.CityId.Value, stateId: accountAddress.StateId.Value);
                                            return deliveries.Model;
                                        }
                                        else
                                        {
                                            return new List<DeliveryVm>();

                                        }
                                    }
                                    else
                                    {
                                        return new List<DeliveryVm>();

                                    }

                                })
                .Function(dest => dest.MerchantList, src =>
                   {
                       return BaseMerchant.GetActives();
                   })

                ;

            Mapper.Register<CartStore, CartStoreVm>()
                .Member(dest => dest.CartStoreItemList, src => src.CartStoreItem)
                .Member(dest => dest.StoreName, src => src.Store.Name)
                .Function(dest => dest.DeliveryList, src =>
                {
                    if (BaseWebsite.WebsiteSetting.HasCentralizedDelivery == false)
                    {
                        var accountAddress = src.Cart.AccountAddress;
                        if (accountAddress != null)
                        {
                            var deliveries = BaseDelivery.GetPossibleDeliveries(storeId: src.StoreId, price: src.TotalPrice - src.RebatePrice, cityId: accountAddress.CityId.Value, stateId: accountAddress.StateId.Value);
                            return deliveries.Model;
                        }
                    }

                    return new List<DeliveryVm>();
                });
                          //.After((src, dest) =>
                          //{
                          //    if (dest.DeliveryList != null && dest.DeliveryList.Any())
                          //    {
                          //        dest.DeliveryId = dest.DeliveryList.First().ID;
                          //        dest.DeliveryPrice = dest.DeliveryList.First().PayementPrice;
                          //    }

                          //});
            Mapper.Register<Delivery, DeliveryVm>()
                .Function(dest => dest.CashOnDelivery, src =>
                {
                    if (src.CashOnDeliveryMaxPrice.HasValue && src.CashOnDeliveryMaxPrice > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                })
                .Function(dest => dest.IsUnknownPrice, src =>
                {
                    if (src.BasePrice == null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                })
                ;
            Mapper.Register<CartStoreItem, CartStoreItemVm>()
            .Function(dest => dest.ProductName, src => src.Product.GetName())
            .Member(dest => dest.Color, src => src.StoreProductQuantity.ProductQuantity.Color)             
            .Member(dest => dest.Size, src => src.StoreProductQuantity.ProductQuantity.Size)
            .Member(dest => dest.ProductOptionItem, src => src.StoreProductQuantity.ProductQuantity.ProductOptionItem)
             .After((src, dest) =>
             {
                 if (dest.ProductOptionItem != null)
                 {
                     dest.ProductOptionItem.Value=src.StoreProductQuantity.ProductQuantity.ProductOptionItem.GetValue();
                 }
                 if (dest.Size != null)
                 {
                     dest.Size.Name = src.StoreProductQuantity.ProductQuantity.Size.GetName();
                 }
                 if (dest.Color != null)
                 {
                     dest.Color.Name = src.StoreProductQuantity.ProductQuantity.Color.GetName();
                 }
             })

                 ;
            Mapper.Register<ProductOptionItem, ProductOptionItemVm>()
             .Member(dest => dest.OptionName, src => src.ProductOption.Name)
             .Function(dest => dest.OptionName, src =>
             {

                 return src.ProductOption.GetName();

             });
            Mapper.Register<Picture, PictureVm>()
                .Ignore(dest => dest.PathUrl)
                .Before((src, dest) =>
                {
                    if (src == null)
                    {
                        dest = new PictureVm();
                    }
                })
                .After((src, dest) =>
                {
                    if (src == null)
                    {
                        dest = new PictureVm();
                    }
                })
                .Member(dest => dest.Url, src => src.GetUrl());
            Mapper.Register<WebsiteDocument, WebsiteDocumentVm>()
                .Before((src, dest) =>
                {
                    if (src == null)
                    {
                        dest = new WebsiteDocumentVm();
                    }
                })
                .After((src, dest) =>
                {
                    if (src == null)
                    {
                        dest = new WebsiteDocumentVm();
                    }
                })
                .Member(dest => dest.Url, src => src.GetUrl());
            Mapper.Register<Product, ProductVm>()
             .Member(dest => dest.ProductUrl, src => src.GetLinkWithUrl())

             ;

            Mapper.Register<StoreProductQuantity, StoreProductQuantityVm>()

                .Member(dest => dest.ProductId, src => src.StoreProduct.ProductId)
                .Member(dest => dest.Store, src => src.StoreProduct.Store)
                .Member(dest => dest.Color, src => src.ProductQuantity.Color)
                .Member(dest => dest.Size, src => src.ProductQuantity.Size)
                .Member(dest => dest.OptionItem, src => src.ProductQuantity.ProductOptionItem)
                .Member(dest => dest.ColorId, src => src.ProductQuantity.ColorId)
                .Member(dest => dest.SizeId, src => src.ProductQuantity.SizeId)
                .Member(dest => dest.OptionItemId, src => src.ProductQuantity.OptionItemId)

                .Member(dest => dest.BasePrice, src => src.Price)
                .Member(dest => dest.PriceRange, src => src.PriceRange.OrderBy(s => s.StartCount).ToList())

                .After((src, dest) =>
                {

                    dest.FullName = src.ProductQuantity.Product.GetName();
                    if (dest.Color != null)
                    {
                        dest.FullName += $"-{Resource.Lang.Color} {dest.Color.Name}";
                        dest.Color.Name = src.ProductQuantity.Color.GetName();
                    }
                    if (dest.Size != null)
                    {
                        dest.FullName += $"-{Resource.Lang.Size} {dest.Size.Name}";
                        dest.Size.Name=src.ProductQuantity.Size.GetName();
                    }
                    if (dest.OptionItem != null)
                    {
                        dest.FullName += $"-{dest.OptionItem.OptionName} {dest.OptionItem.Value}";
                        dest.OptionItem.Value=src.ProductQuantity.ProductOptionItem.GetValue();
                    }

                    var discount = BaseDiscount.GetDiscount(src);
                    if (discount != null)
                    {
                        dest.DiscountValue = discount.DiscountValue;
                        dest.DiscountPercent = discount.DiscountPercent;
                        dest.DiscountExpire = discount.DiscountExpire;
                        dest.BasePrice = discount.BasePrice;
                        dest.Price = discount.Price;
                    }
                    if (BaseStore.StoreProductItemIsEarnest(src))
                    {
                        dest.EarnestPrice = Math.Round(dest.Price * ((float)BaseWebsite.WebsiteSetting.Earnest / 100), 0);
                    }


                });
            ;
            Mapper.Register<Merchant, MerchantVm>()
                .Member(dest => dest.BankName, src => src.Bank.Name)
                .Member(dest => dest.Picture, src => $"{BaseWebsite.ShopUrl}/assets/Default/image/payment/Merchant/{src.Bank.Label}.PNG");
            Mapper.Register<Rebate, RebateVm>();
            Mapper.Register<WebsiteDocument, WebsiteDocumentVm>()
                .Member(dest => dest.Url, src => src.GetUrl());
            Mapper.Register<ProductBrand, ProductBrandVm>()
                 .Function(dest => dest.Name, src => src.GetName())
                 .Function(dest => dest.Summary, src => src.GetSummary())
                 .Function(dest => dest.Description, src => src.GetDescription())
                 .Function(dest => dest.Title, src => src.GetTitle())

            .Function(dest => dest.Href, src => src.GetLinkNew())
                .After((src, dest) =>
                {
                    if (dest.Picture == null)
                    {
                        dest.Picture = new PictureVm();
                    }
                });


            ;
            Mapper.Register<ProductSubCategory, ProductSubCategoryVm>()
                .Function(dest => dest.Name, src => src.GetName())
                .Function(dest => dest.LandingText, src => src.GetLandingText())
                .Function(dest => dest.Href, src => src.GetUrl())
                .Member(dest => dest.Icon, src => src.Picture1)
                .Function(dest => dest.ProductCategoryName, src => src.ProductCategory.GetName())
                .Function(dest => dest.ProductTypeName, src => src.ProductCategory.ProductType.GetName())

                .After((src, dest) =>
          {
              if (src.Picture == null)
              {
                  dest.Picture = new PictureVm();
              }
              if (src.IconId == null)
              {
                  dest.Icon = new PictureVm();
              }

          });
            Mapper.Register<ProductCategory, ProductCategoryVm>()
                .Function(dest => dest.Name, src => src.GetName())
                .Function(dest => dest.LandingText, src => src.GetLandingText())
                .Function(dest => dest.Href, src => src.GetUrlNew())
                .Function(dest => dest.ProductTypeName, src => src.ProductType.GetName())
                .Member(dest => dest.Icon, src => src.Picture1)
                                .After((src, dest) =>
                                {
                                    if (src.Picture == null)
                                    {
                                        dest.Picture = new PictureVm();
                                    }
                                    if (src.Picture1 == null)
                                    {
                                        dest.Icon = new PictureVm();
                                    }
                                });
            ;
            Mapper.Register<ProductType, ProductTypeVm>()
                .Function(dest => dest.Name, src => src.GetName())
                .Function(dest => dest.Description, src => src.GetDescription())
                .Function(dest => dest.title, src => src.GetTitle())
                .Function(dest => dest.H1, src => src.GetH1())
                .Function(dest => dest.Href, src => src.GetUrlNew())
                                .Function(dest => dest.LandingText, src => src.GetLandingText())

                                .Member(dest => dest.Icon, src => src.Picture1)

                .After((src, dest) =>
                {
                    if (src.Picture == null)
                    {
                        dest.Picture = new PictureVm();
                    }
                    if (src.Picture1 == null)
                    {
                        dest.Icon = new PictureVm();
                    }
                });

            Mapper.Register<Post, PostVm>()
            .Function(dest => dest.Name, src => src.GetName())
            .Function(dest => dest.Summary, src => src.GetSummery())
            .Function(dest => dest.H1, src => src.GetH1())

            .Function(dest => dest.Body, src => src.GetBody())
            .Function(dest => dest.CategoryName, src => src.Category.GetName())
            .Function(dest => dest.CategoryBody, src => src.Category.GetBody())

            .Function(dest => dest.Href, src => src.GetLinkWithUrl())
                            .After((src, dest) =>
                            {
                                if (src.Picture == null)
                                {
                                    dest.Picture = new PictureVm();
                                }
                                if (src.Picture1 == null)
                                {
                                    dest.Picture1 = new PictureVm();
                                }
                                if (src.WebsiteDocument == null)
                                {
                                    dest.WebsiteDocument = new WebsiteDocumentVm();
                                }

                            });
            ;

            Mapper.Register<City, CityVm>()
            .Function(dest => dest.StateName, src => src.State!=null? src.State.Name :"")
            ;
            Mapper.Register<Product, ProductShowVm>()
                .Function(dest => dest.ProductTypeName, src =>
                {
                    if (src.ProductType != null)
                    {
                        return src.ProductType.GetName();
                    }
                    else
                    {
                        return "";
                    }
                })
                .Function(dest => dest.HasExpire, src =>
                {
                    DateTime now = DateTime.Now;
                    if ((src.StartDatetime == null || (src.StartDatetime != null && src.StartDatetime <= now)) &&
                        (src.EndDatetime == null || (src.EndDatetime != null && src.EndDatetime >= now)))
                    {
                        return false;
                    }
                    else { return true; }
                })
                .Function(dest => dest.ProductCategoryName, src =>
                {
                    if (src.ProductCategory != null)
                    {
                        return src.ProductCategory.GetName();
                    }
                    else
                    {
                        return "";
                    }
                })

                .Function(dest => dest.ProductSubCategoryName, src =>
                {
                    if (src.ProductSubCategory != null)
                    {
                        return src.ProductSubCategory.GetName();
                    }
                    else
                    {
                        return "";
                    }
                })
                .Function(dest => dest.ProductTypeUrl, src =>
                  {
                      if (src.ProductType != null)
                      {
                          return src.ProductType.GetUrlNew();
                      }
                      else
                      {
                          return "";
                      }
                  })
                .Function(dest => dest.ProductTypePictureUrl, src =>
                {
                    if (src.ProductType != null)
                    {
                        if (src.ProductType.Picture != null)
                        {
                            return src.ProductType.Picture.GetUrl();
                        }
                        else
                        {
                            return "";
                        }
                       
                    }
                    else
                    {
                        return "";
                    }
                })
                  .Function(dest => dest.ProductCategoryUrl, src =>
                   {
                       if (src.ProductCategory != null)
                       {
                           return src.ProductCategory.GetUrlNew();
                       }
                       else
                       {
                           return "";
                       }
                   })
                 .Function(dest => dest.ProductSubCategoryUrl, src =>
                   {
                       if (src.ProductSubCategory != null)
                       {
                           return src.ProductSubCategory.GetUrl();
                       }
                       else
                       {
                           return "";
                       }
                   })
                .Function(dest => dest.Name, src => src.GetName())
                .Function(dest => dest.Summary, src => src.GetSummery())
                .Function(dest => dest.H1, src => src.GetH1())
                .Function(dest => dest.Title, src => src.GetTitle())

                .Function(dest => dest.Description, src => src.GetDescription())
                .Function(dest => dest.MetaTagDescription, src => src.GetMetaDescription())
                .Function(dest => dest.ProductUrl, src => src.GetLinkWithUrl())
                .Function(dest => dest.Canonical, src =>
                {
                    if (string.IsNullOrEmpty(src.Canonical) != false)
                        return src.Canonical;
                    return src.GetLinkWithUrl();

                }
                )
                .Function(dest => dest.ProductComment, src => src.ProductComment.Where(s => s.Approved == true))
                .Function(dest => dest.Pictures, src => src.ProductPicture.Select(s => s.Picture))
                .Member(dest => dest.ProductBrand, src => src.ProductBrand)

                .After((src, dest) =>
                {
                    if (src.Picture == null)
                    {
                        dest.Picture = new PictureVm();
                    }

                    if (dest.ProductComment.Any())
                    {
                        var rates = dest.ProductComment.Where(s => s.Rate.HasValue);
                        if (rates.Any())
                        {
                            dest.RateAvarage = rates.Average(s => s.Rate.Value);
                            dest.RateMin = rates.Min(s => s.Rate.Value);
                            dest.RateMax = rates.Max(s => s.Rate.Value);
                            dest.RateCount = rates.Count(s => s.Rate.HasValue);
                            dest.RateCount1 = rates.Count(s => s.Rate == 1);
                            dest.RateCount2 = rates.Count(s => s.Rate == 2);
                            dest.RateCount3 = rates.Count(s => s.Rate == 3);
                            dest.RateCount4 = rates.Count(s => s.Rate == 4);
                            dest.RateCount5 = rates.Count(s => s.Rate == 5);
                        }
                    }
                    dest.NameId = $"({src.ID}){src.Name}";
                });
            ;
            Mapper.Register<Product, ProductMinVm>()

                .Function(dest => dest.Name, src => src.GetName())
                .Function(dest => dest.Summary, src => src.GetSummery())
                .Function(dest => dest.Description, src => src.GetDescription())
                .Function(dest => dest.ProductUrl, src => src.GetLinkWithUrl())
                .Function(dest => dest.Pictures, src => src.ProductPicture.Select(s => s.Picture))
                .Member(dest => dest.ProductBrand, src => src.ProductBrand)
                .Member(dest => dest.UpdateDatetime, src => src.UpdateDatetime)
                .Function(dest => dest.ProductComment, src => src.ProductComment.Where(s => s.Approved == true))
                .Function(dest => dest.ProductTypeName, src =>
                {
                    if (src.ProductType != null)
                    {
                        return src.ProductType.GetName();
                    }
                    else { return null; }
                })
                .Function(dest => dest.HasExpire, src =>
                {
                    DateTime now = DateTime.Now;
                    if ((src.StartDatetime == null || (src.StartDatetime != null && src.StartDatetime <= now)) &&
                        (src.EndDatetime == null || (src.EndDatetime != null && src.EndDatetime >= now)))
                    {
                        return false;
                    }
                    else { return true; }
                })
            .Function(dest => dest.ProductCategoryName, src =>
            {
                if (src.ProductCategory != null)
                {
                    return src.ProductCategory.GetName();
                }
                else { return null; }
            })
            .Function(dest => dest.ProductSubCategoryName, src =>
            {
                if (src.ProductSubCategory != null)
                {
                    return src.ProductSubCategory.GetName();
                }
                else { return null; }
            })
                .After((src, dest) =>
                {
                    if (src.Picture == null)
                    {
                        dest.Picture = new PictureVm();
                    }
                    if (dest.ProductComment.Any())
                    {
                        var rates = dest.ProductComment.Where(s => s.Rate.HasValue);
                        if (rates.Any())
                        {
                            dest.RateAvarage = rates.Average(s => s.Rate.Value);
                            dest.RateMin = rates.Min(s => s.Rate.Value);
                            dest.RateMax = rates.Min(s => s.Rate.Value);
                            dest.RateCount = rates.Count(s => s.Rate.HasValue);
                        }

                    }
                    dest.NameId = $"({src.ID}){src.Name}";

                });
            ;
            Mapper.Register<ProductComment, ProductCommentVm>()
                .Function(dest => dest.NameFamily, src =>
                {
                    return src.AccountId != null ? src.Account.FullName : Resource.Lang.User;
                });
            Mapper.Register<Store, StoreShowVm>()
                .Function(dest => dest.CompetitiveFeature, src =>
                {
                    return src.StoreCompetitiveFeature.Select(s => s.CompetitiveFeature);
                });


            Mapper.Register<Payment, PaymentVm>()
                .Member(dest => dest.Status, src => src.Code)
                .Function(dest => dest.PaymentAddress, src =>
                {
                    var domain = BaseWebsite.ShopUrl;
                    return $"{domain}/payment/start/{src.ID}";
                });
            Mapper.Register<PaymentCertificate, PaymentVm2>()
               .Member(dest => dest.Status, src => src.Code)
               .Function(dest => dest.PaymentAddress, src =>
               {
                   var domain = BaseWebsite.ShopUrl;
                   return $"{domain}/payment/start/{src.ID}";
               });
            Mapper.Register<PaymentExame, PaymentVm3>()
              .Member(dest => dest.Status, src => src.Code)
              .Function(dest => dest.PaymentAddress, src =>
              {
                  var domain = BaseWebsite.ShopUrl;
                  return $"{domain}/payment/start/{src.ID}";
              });
            Mapper.Register<ProductQuantity, ProductQuantityVm>()
                .Member(dest => dest.ProductName, src => src.Product.Name);
            Mapper.Register<Slider, SliderVm>()
    .Function(dest => dest.Name, src => src.GetName())
    .Function(dest => dest.SecondName, src => src.GetSecondName())
 .After((src, dest) =>
 {
     if (src.Picture == null)
     {
         dest.Picture = new PictureVm();
     }
 });
            Mapper.Register<Banner, BannerVm>()
    .Function(dest => dest.Name, src => src.GetName())
    .Function(dest => dest.Summary, src => src.GetSummery());
            Mapper.Register<ProductCustomField, ProductCustomFieldVm>()
                    .Function(dest => dest.Name, src => src.GetName())
                    .Function(dest => dest.ProductCustomItemList, src => src.ProductCustomItem)
                    .Member(dest => dest.TypeLabel, src => src.Code.Label)
                    ;
            Mapper.Register<QuestionRegister, QuestionRegisterVm>()
                   .Function(dest => dest.Name, src => src.Name)
                   .Function(dest => dest.QuestionRegisterItem, src => src.QuestionRegisterItem)
                   .Member(dest => dest.TypeLabel, src => src.Code.Label)
                   ;
            Mapper.Register<ProductCustomValue, ProductCustomValueVm>();
            Mapper.Register<QuestionRegisterProduct, QuestionRegisterProductVm>();
            Mapper.Register<QuestionRegisterItem, QuestionRegisterItemVm>();

            Mapper.Register<ProductCustomItem, ProductCustomItemVm>()
                                    .Function(dest => dest.Value, src => src.GetValue());
            Mapper.Register<WebsiteLanguage, WebsiteLanguageVm>()
                    .Function(dest => dest.LanguageLabel, src => src.Language.Label)
                    .Function(dest => dest.LanguageName, src => src.Language.Name);
            Mapper.Register<ProductCreateVm, Product>()
                .Ignore(dest => dest.Picture)
                .Ignore(dest => dest.Code)
                .Ignore(dest => dest.CreateDateTime)
                .Ignore(dest => dest.UpdateDatetime)
                .Ignore(dest => dest.SellCount)
                .Ignore(dest => dest.Price)
                .Ignore(dest => dest.MinPrice)
                .Ignore(dest => dest.InexpensiveItemBasePrice)
                .Ignore(dest => dest.InexpensiveItemPrice)
                .Ignore(dest => dest.LastPrice)
                .Ignore(dest => dest.File)
                .Ignore(dest => dest.WebsiteDocument)
                .Ignore(dest => dest.ProductBrand)
                .Ignore(dest => dest.ProductCategory)
                .Ignore(dest => dest.ProductOption)
                .Ignore(dest => dest.ProductColor)
                .Ignore(dest => dest.ProductSize)
                .Ignore(dest => dest.ProductProductOptionItem)
                .Ignore(dest => dest.ProductSubCategory)
                .Ignore(dest => dest.ProductType)
                .Ignore(dest => dest.Shop)
                .Ignore(dest => dest.SiteUser)
                .Ignore(dest => dest.Unit)
                .Ignore(dest => dest.File1)
                .Ignore(dest => dest.File2)
                .Ignore(dest => dest.File2)
              

                ;
            Mapper.Register<Product, ProductCreateVm>()
                .Function(dest => dest.PictureUrl, src => src.Picture.GetUrl())
                .Function(dest => dest.ProductOptionName, src =>
                {
                    if (src.ProductOptionId.HasValue)
                    {
                        return src.ProductOption.GetName();
                    }
                    return "";
                })
                .Function(dest => dest.HasColor, src =>
                {
                    if (src.ProductColor != null && src.ProductColor.Any())
                    {
                        return true;
                    }
                    return false;
                })
                .Function(dest => dest.HasSize, src =>
                {
                    if (src.ProductSize != null && src.ProductSize.Any())
                    {
                        return true;
                    }
                    return false;
                })
                .Function(dest => dest.HasOption, src =>
                {
                    if (src.ProductProductOptionItem != null && src.ProductProductOptionItem.Any())
                    {
                        return true;
                    }
                    return false;
                })

                .Member(dest => dest.ProductPictures, src => src.ProductPicture.Select(s => s.Picture))
                .Member(dest => dest.Reviews, src => src.Review)
                .Member(dest => dest.ProductLanguages, src => src.ProductLanguage)
                ;
            Mapper.Register<ProductLanguage, ProductLanguageVm>()
              .Member(dest => dest.LanguageName, src => src.Language.Name);
            Mapper.Register<Review, ReviewVm>()
              .Function(dest => dest.PictureUrl, src => src.Picture.GetUrl());
            Mapper.Register<Discount, CreateDiscountVm>()
              .Function(dest => dest.GroupName, src =>
            {
                if (src.DiscountGroup != null)
                {
                    return src.DiscountGroup.Name;
                }
                else { return null; }
            })
            .Function(dest => dest.StoreName, src =>
            {
                if (src.Store != null)
                {
                    return src.Store.Name;
                }
                else { return null; }
            })
            .Function(dest => dest.ProductTypeName, src =>
            {
                if (src.ProductType != null)
                {
                    return src.ProductType.GetName();
                }
                else { return null; }
            })
            .Function(dest => dest.ProductCategoryName, src =>
            {
                if (src.ProductCategory != null)
                {
                    return src.ProductCategory.GetName();
                }
                else { return null; }
            })
            .Function(dest => dest.ProductSubCategoryName, src =>
            {
                if (src.ProductSubCategory != null)
                {
                    return src.ProductSubCategory.GetName();
                }
                else { return null; }
            })
            .Function(dest => dest.ProductBrandName, src =>
            {
                if (src.ProductBrand != null)
                {
                    return src.ProductBrand.Name;
                }
                else { return null; }
            })
            .Function(dest => dest.ProductName, src =>
            {
                if (src.Product != null)
                {
                    return src.Product.GetName();
                }
                else { return null; }
            })
            .Function(dest => dest.VarityName, src =>
            {
                if (src.StoreProductQuantity != null)
                {
                    return src.StoreProductQuantity.Map<StoreProductQuantity, StoreProductQuantityVm>().FullName;
                }
                else { return null; }
            })
            ;
            Mapper.Register<Discount, DiscountVm>()
              .Function(dest => dest.TypeName, src =>
            {
                if (src.Code != null)
                {
                    return src.Code.Name;
                }
                else { return null; }
            })
              .Function(dest => dest.GroupName, src =>
            {
                if (src.DiscountGroup != null)
                {
                    return src.DiscountGroup.Name;
                }
                else { return null; }
            })
            .Function(dest => dest.StoreName, src =>
            {
                if (src.Store != null)
                {
                    return src.Store.Name;
                }
                else { return null; }
            })
            .Function(dest => dest.ProductTypeName, src =>
            {
                if (src.ProductType != null)
                {
                    return src.ProductType.GetName();
                }
                else { return null; }
            })
            .Function(dest => dest.ProductCategoryName, src =>
            {
                if (src.ProductCategory != null)
                {
                    return src.ProductCategory.GetName();
                }
                else { return null; }
            })
            .Function(dest => dest.ProductSubCategoryName, src =>
            {
                if (src.ProductSubCategory != null)
                {
                    return src.ProductSubCategory.GetName();
                }
                else { return null; }
            })
            .Function(dest => dest.ProductBrandName, src =>
            {
                if (src.ProductBrand != null)
                {
                    return src.ProductBrand.Name;
                }
                else { return null; }
            })
            .Function(dest => dest.ProductName, src =>
            {
                if (src.Product != null)
                {
                    return src.Product.GetName();
                }
                else { return null; }
            })
            .Function(dest => dest.VarityName, src =>
            {
                if (src.StoreProductQuantity != null)
                {
                    return src.StoreProductQuantity.Map<StoreProductQuantity, StoreProductQuantityVm>().FullName;
                }
                else { return null; }
            })
            ;
            Mapper.Register<Survey, SurveyVm>()
                .Member(dest => dest.SurveyQuestionsVm, src => src.SurveyQuestion);
            Mapper.Register<SurveyQuestionItem, SurveyQuestionItemVm>()
                .Function(dest => dest.Name, src => src.GetName());
            Mapper.Register<SurveyQuestion, SurveyQuestionVm>()
                .Function(dest => dest.Question, src => src.GetQuestion())
                .Member(dest => dest.SurveyQuestionItemsVm, src => src.SurveyQuestionItem);
            Mapper.Register<SurveyAnswerDetail, SurveyAnswerDetailVm>()
                .Member(dest => dest.ItemIdsVm, src => src.SurveyAnswerDetailItem);
            Mapper.Register<SurveyQuestionVm, SurveyAnswerDetailVm>()
                .Member(dest => dest.QuestionId, src => src.ID)
                .Member(dest => dest.QuestionText, src => src.Question)
                .Member(dest => dest.QuestionCodeLabel, src => src.Code.Label)
                .Member(dest => dest.ItemIdsVm, src => src.SurveyQuestionItemsVm.Where(s => s.Active != false).OrderBy(o => o.ShowNumber).ToList());
            Mapper.Register<ProductType, CategoriesSearchVm>()
                .Function(dest => dest.ProductTypeName, src => src.GetName())
                .Function(dest => dest.Href, src => src.GetUrlNew())
.After((src, dest) =>
                                {
                                    if (src.Picture == null)
                                    {
                                        dest.Picture = new PictureVm();
                                    }
                                    if (!string.IsNullOrEmpty(dest.ProductTypeName))
                                    {
                                        dest.Fullname = dest.ProductTypeName;
                                    }


                                });
            Mapper.Register<ProductCategory, CategoriesSearchVm>()
                .Function(dest => dest.ProductTypeName, src => src.ProductType.GetName())
                .Function(dest => dest.ProductCategoryName, src => src.GetName())
                .Function(dest => dest.Href, src => src.GetUrlNew())
.After((src, dest) =>
                                {
                                    if (src.Picture == null)
                                    {
                                        dest.Picture = new PictureVm();
                                    }

                                    if (!string.IsNullOrEmpty(dest.ProductTypeName))
                                    {
                                        dest.Fullname = dest.ProductTypeName;
                                    }
                                    if (!string.IsNullOrEmpty(dest.ProductCategoryName))
                                    {
                                        dest.Fullname += " / " + dest.ProductCategoryName;
                                    }


                                });
            Mapper.Register<ProductSubCategory, CategoriesSearchVm>()
                .Function(dest => dest.ProductTypeName, src => src.ProductCategory.ProductType.GetName())
                .Function(dest => dest.ProductCategoryName, src => src.ProductCategory.GetName())
                .Function(dest => dest.ProductSubCategoryName, src => src.GetName())
                .Function(dest => dest.Href, src => src.GetUrl())
.After((src, dest) =>
                                {
                                    if (src.Picture == null)
                                    {
                                        dest.Picture = new PictureVm();
                                    }

                                    if (!string.IsNullOrEmpty(dest.ProductTypeName))
                                    {
                                        dest.Fullname = dest.ProductTypeName;
                                    }
                                    if (!string.IsNullOrEmpty(dest.ProductCategoryName))
                                    {
                                        dest.Fullname += " / " + dest.ProductCategoryName;
                                    }
                                    if (!string.IsNullOrEmpty(dest.ProductSubCategoryName))
                                    {
                                        dest.Fullname += " / " + dest.ProductSubCategoryName;
                                    }

                                });
            Mapper.Register<ProductBrand, BrandsSearchVm>()
    .Function(dest => dest.Name, src => src.GetName())
    .Function(dest => dest.Href, src => src.GetLinkNew())
.After((src, dest) =>
{
    if (src.Picture == null)
    {
        dest.Picture = new PictureVm();
    }
});
            Mapper.Register<Product, ProductsSearchVm>()
.Function(dest => dest.Name, src => src.GetName())
                .Function(dest => dest.ProductTypeName, src =>
                {
                    if (src.ProductType != null)
                    {
                        return src.ProductType.GetName();
                    }
                    else
                    {
                        return "";
                    }
                })
                .Function(dest => dest.ProductCategoryName, src =>
                {
                    if (src.ProductCategory != null)
                    {
                        return src.ProductCategory.GetName();
                    }
                    else
                    {
                        return "";
                    }
                })

                .Function(dest => dest.ProductSubCategoryName, src =>
                {
                    if (src.ProductSubCategory != null)
                    {
                        return src.ProductSubCategory.GetName();
                    }
                    else
                    {
                        return "";
                    }
                })
.Function(dest => dest.Href, src => src.GetLinkWithUrl())
.After((src, dest) =>
{
    if (src.Picture == null)
    {
        dest.Picture = new PictureVm();
    }
    if (!string.IsNullOrEmpty(dest.ProductTypeName))
    {
        dest.Fullname = dest.ProductTypeName;
    }
    if (!string.IsNullOrEmpty(dest.ProductCategoryName))
    {
        dest.Fullname += " / " + dest.ProductCategoryName;
    }
    if (!string.IsNullOrEmpty(dest.ProductSubCategoryName))
    {
        dest.Fullname += " / " + dest.ProductSubCategoryName;
    }
    dest.Fullname += " / " + dest.Name;

});

            Mapper.Register<Post, PostsSearchVm>()
.Function(dest => dest.Name, src => src.GetName())
.Function(dest => dest.CategoryName, src => src.Category.GetName())
.Function(dest => dest.CategoryBody, src => src.Category.GetBody())
.Function(dest => dest.Href, src => src.GetLinkWithUrl())
.After((src, dest) =>
{
    if (src.Picture == null)
    {
        dest.Picture = new PictureVm();
    }
});

            Mapper.Compile();
            
        }
    }
}
