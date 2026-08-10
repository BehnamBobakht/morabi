using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataLayer.Repositories
{
    public class Entity_AccountBasket : BaseRepository<AccountBasket>
    {
        private DatabaseEntities _context;

        public Entity_AccountBasket(DatabaseEntities context) : base(context)
        {
            _context = context;
        }


        public List<AccountBasket> GetAllByAccount(int accountId)
        {
            return _context.AccountBasket.Where(p =>
                p.AccountId == accountId
            ).ToList();
        }

        public List<AccountBasket> GetAllFromCookie(bool isComplete = true)
        {
            List<AccountBasket> listOutput = new List<AccountBasket>();
            try
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies["BASKET"];
                if (cookie != null)
                {
                    string cookieBasket = HttpContext.Current.Server.UrlDecode(cookie.Value);
                    List<AccountBasket> list = new List<AccountBasket>();
                    list = JsonConvert.DeserializeObject<List<AccountBasket>>(cookieBasket);
                    listOutput = CastCookieBasket(list, isComplete);
                }
            }
            catch { }
            return listOutput;
        }

        public List<AccountBasket> CastCookieBasket(List<AccountBasket> list, bool isComplete = true)
        {
            List<AccountBasket> listOutput = new List<AccountBasket>();
            foreach (AccountBasket item in list.ToList())
            {
                AccountBasket entity = new AccountBasket()
                {
                    Count = item.Count,
                    Datetime = item.Datetime,
                    Price = item.Price,
                    ProductId = item.ProductId,
                    ProductColorId = item.ProductColorId,
                    ProductSizeId = item.ProductSizeId,
                    ProductOptionItemId = item.ProductOptionItemId,
                    ProductPrice = item.ProductPrice,
                    AccountId = item.AccountId,
                    ProductDiscount = item.ProductDiscount,
                    StoreId = item.StoreId
                };
                if (isComplete)
                {
                    entity.Product = _context.Product.FirstOrDefault(p => p.ID == item.ProductId);
                    entity.Color = item.ProductColorId != null ? _context.Color.FirstOrDefault(p => p.ID == item.ProductColorId) : null;
                    entity.Size = item.ProductSizeId != null ? _context.Size.FirstOrDefault(p => p.ID == item.ProductSizeId) : null;
                    entity.ProductOptionItem = item.ProductOptionItemId != null ? _context.ProductOptionItem.FirstOrDefault(p => p.ID == item.ProductOptionItemId) : null;

                    entity.Store = item.StoreId != null ? _context.Store.Find(item.StoreId) : null;
                }
                listOutput.Add(entity);
            }
            return listOutput;
        }
        public void RemoveOutOfSendStore()
        {
            HttpContext.Current.Response.Cookies["OutOfSendStore"].Expires = DateTime.Now.AddDays(-1);
        }
        public void SetOutOfSendStore(List<ViewProduct> list)
        {

           
            List<ViewProduct> finalList = new List<ViewProduct>();
            foreach (ViewProduct item in list)
            {
                finalList.Add(new ViewProduct()
                {
                    Name = item.Name,
                });
            }
            HttpCookie cookie = new HttpCookie("OutOfSendStore")
            {
                Value = JsonConvert.SerializeObject(finalList),
                Expires = DateTime.Now.AddDays(1)
            };
            HttpContext.Current.Response.Cookies.Add(cookie);
            //HttpContext.Current.Session[".OutOfSendStore"] = true;
            //HttpCookie locationCookie = new HttpCookie(".OutOfSendStore");
            ////HttpContext.Current.Response.Cookies[".LOCATION"].Expires = DateTime.Now.AddDays(-1);
            //locationCookie.Values.Add("Products", products);            
            //locationCookie.Expires = DateTime.Now.AddMonths(1);
            //HttpContext.Current.Response.Cookies.Add(locationCookie);

        }
        public List<ViewProduct> GetAllSetOutOfSendStoreFromCookie(bool isComplete = true)
        {
            List<ViewProduct> listOutput = new List<ViewProduct>();
            try
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies["OutOfSendStore"];
                if (cookie != null)
                {
                    string cookieBasket = HttpContext.Current.Server.UrlDecode(cookie.Value);
                    List<ViewProduct> list = new List<ViewProduct>();
                    list = JsonConvert.DeserializeObject<List<ViewProduct>>(cookieBasket);
                    listOutput = CastCookieOutOf(list);
                }
            }
            catch {
            
            }
            return listOutput;
        }
        public List<ViewProduct> CastCookieOutOf(List<ViewProduct> list)
        {
            List<ViewProduct> listOutput = new List<ViewProduct>();
            foreach (ViewProduct item in list.ToList())
            {
                ViewProduct entity = new ViewProduct()
                {
                    Name = item.Name,
                };
                listOutput.Add(entity);
            }
            return listOutput;
        }
        public double GetFinalBasketPrice(List<AccountBasket> list)
        {
            double sum = 0;
            foreach (AccountBasket item in list)
            {
                sum = sum + item.Price;
            }
            return sum;
        }

        public double GetFinalDiscountPrice(List<AccountBasket> list)
        {
            double sum = 0;
            foreach (AccountBasket item in list)
            {
                sum = sum + (item.ProductDiscount * item.Count);
            }
            return sum;
        }

        public void ClearBasket(int? accountId)
        {
            if (accountId != null)
            {
                List<AccountBasket> list = GetAllByAccount(accountId.GetValueOrDefault());
                foreach (AccountBasket item in list)
                {
                    Delete(item);
                }
                Save();
            }

            if (HttpContext.Current != null)
            {
                HttpCookie myCookie = new HttpCookie("BASKET");
                myCookie.Expires = DateTime.Now.AddDays(-1d);
                HttpContext.Current.Response.Cookies.Add(myCookie);
            }
        }

        public void ClearAccountBaskets()
        {
            try
            {
                DateTime yesterday = DateTime.Now.AddDays(-1);
                List<AccountBasket> listAccountBasket = _context.AccountBasket.Where(p => p.Datetime < yesterday).ToList();
                for (int i = 0; i < listAccountBasket.Count; i++)
                {
                    Delete(listAccountBasket[i]);
                }
                Save();
            }
            catch (Exception) { }
        }

        public bool AddToBasket(out List<AccountBasket> listOutput, Product product, int? accountId, int? colorId = null, int? sizeId = null, int? optionItemId = null, int? resellerId = null, int? storeId = null, int count = 1)
        {
            bool isAvailable = false;
            List<AccountBasket> ListAccountBasket = new List<AccountBasket>();
            ProductQuantity productQuantity = new ProductQuantity();
            StoreProductQuantity storeProductQuantity = new StoreProductQuantity();
            
            if (product != null && product.Code.Label == Enum_Code.PRODUCT_STATUS_AVAILABLE.ToString())
            {
                if (storeId == null)
                {

                    if (colorId != null || sizeId != null || optionItemId != null)
                    {
                        productQuantity = _context.ProductQuantity.FirstOrDefault(p =>
                                        p.ProductId == product.ID &&
                                        p.ColorId == colorId &&
                                        p.SizeId == sizeId &&
                                        p.OptionItemId == optionItemId
                                    );
                        if (productQuantity != null)
                        {
                            if (productQuantity.Count > 0 && productQuantity.Price != null && productQuantity.Price > 0)
                                isAvailable = true;
                        }
                    }
                    else
                    {
                        productQuantity = _context.ProductQuantity.FirstOrDefault(p => p.ProductId == product.ID);
                        if (productQuantity != null && product.Code.Label == Enum_Code.PRODUCT_STATUS_AVAILABLE.ToString())
                            isAvailable = true;
                    }
                }
                else
                {
                    storeProductQuantity = _context.StoreProductQuantity.FirstOrDefault(p =>
                    p.ProductQuantity.ProductId == product.ID &&
                    p.ProductQuantity.ColorId == colorId &&
                    p.ProductQuantity.SizeId == sizeId &&
                    p.ProductQuantity.OptionItemId == optionItemId &&
                    p.StoreProduct.StoreId == storeId);

                    if (storeProductQuantity != null)
                    {
                        if (storeProductQuantity.Count > 0)
                            isAvailable = true;
                    }
                }
            }

            if (isAvailable == true)
            {
                if (accountId != null)
                    ListAccountBasket = GetAllByAccount(accountId.GetValueOrDefault());
                else
                    ListAccountBasket = GetAllFromCookie();

                AccountBasket basket = ListAccountBasket.FirstOrDefault(p =>
                    p.StoreId == storeId && p.ProductId == product.ID && p.ProductColorId == colorId && p.ProductSizeId == sizeId && p.ProductOptionItemId == optionItemId);

                double price = 0;
                double product_price = 0;
                double discount_price = 0;
                if (storeId != null)
                {
                    if (storeProductQuantity != null)
                    {
                        var previousCount = 0;
                        if (basket != null)
                        {
                            previousCount = basket.Count;
                        }
                        product_price = storeProductQuantity.GetFromPriceRange(previousCount + count);
                        discount_price = storeProductQuantity.GetDiscountPrice(previousCount + count);
                        price = product_price - discount_price;

                    }
                }
                else
                {
                    if (colorId != null || sizeId != null || optionItemId != null)
                    {
                        ProductQuantity quantity = _context.ProductQuantity.FirstOrDefault(p =>
                                            p.ProductId == product.ID &&
                                            p.ColorId == colorId &&
                                            p.SizeId == sizeId &&
                                            p.OptionItemId == optionItemId
                                        );
                        if (quantity != null)
                        {
                            price = quantity.GetDiscountPrice();
                            product_price = quantity.Price.GetValueOrDefault();
                            discount_price = product_price - price;
                        }
                        else
                        {
                            price = product.GetDiscountPrice();
                            product_price = product.Price.GetValueOrDefault();
                            discount_price = product_price - price;
                        }
                    }
                    else
                    {
                        price = product.GetDiscountPrice();
                        product_price = product.Price.GetValueOrDefault();
                        discount_price = product_price - price;
                    }
                }



                if (basket != null)
                {
                    if (BaseWebsite.WebsiteSetting.HasStore && storeProductQuantity != null)
                    {
                        if ((basket.Count + count) > storeProductQuantity.Count)
                        {
                            listOutput = ListAccountBasket;
                            return false;
                        }
                    }

                    if (Base.BaseWebsite.WebsiteSetting.ProductMaxOrderCount && product.MaxOrderCount.HasValue)
                    {
                        if ((basket.Count + count) > product.MaxOrderCount.Value)
                        {
                            listOutput = ListAccountBasket;
                            return false;
                        }
                    }
                    basket.ResellerId = resellerId != null ? resellerId : basket.ResellerId;
                    basket.Count = basket.Count + count;
                    basket.Price =  price * (basket.Count/* +*/ /*count*/);
                    basket.ProductPrice = product_price;
                    basket.ProductDiscount = discount_price;
                    basket.Datetime = DateTime.Now;
                    //basket.InstallationPrice = serivePrice * count;
                    if (accountId != null)
                    {
                        Update(basket);
                        Save();
                    }
                    else
                    {
                        int basketIndex = ListAccountBasket.FindIndex(p =>
                            p.ProductId == basket.ProductId &&
                            p.ProductColorId == basket.ProductColorId &&
                            p.ProductSizeId == basket.ProductSizeId &&
                            p.ProductOptionItemId == basket.ProductOptionItemId &&
                            p.StoreId == basket.StoreId

                            );

                        ListAccountBasket[basketIndex] = basket;
                        SetCookieBasket(ListAccountBasket);
                    }
                }
                else
                {

                    if (BaseWebsite.WebsiteSetting.HasStore)
                    {
                        if ((count) > storeProductQuantity.Count)
                        {
                            listOutput = ListAccountBasket;
                            return false;
                        }
                    }

                    if (BaseWebsite.WebsiteSetting.ProductMaxOrderCount && product.MaxOrderCount.HasValue)
                    {
                        if ((count) > product.MaxOrderCount.Value)
                        {
                            listOutput = ListAccountBasket;
                            return false;
                        }
                    }
                    basket = new AccountBasket
                    {
                        AccountId = accountId,
                        ProductSizeId = sizeId,
                        ProductColorId = colorId,
                        ProductOptionItemId = optionItemId,
                        ResellerId = resellerId,
                        Count = count,
                        Datetime = DateTime.Now,
                        Price = price * count,
                        ProductPrice = product_price,
                        ProductDiscount = discount_price,
                        ProductId = product.ID,
                        StoreId = storeId,
                        //    InstallationPrice = serivePrice * count

                    };

                    if (accountId != null)
                    {
                        Insert(basket);
                        Save();
                    }

                    ListAccountBasket.Add(basket);
                    SetCookieBasket(ListAccountBasket);

                }
            }

            listOutput = ListAccountBasket;
            return isAvailable;
        }

        public void SetCookieBasket(List<AccountBasket> list)
        {
            List<AccountBasket> finalList = new List<AccountBasket>();
            foreach (AccountBasket item in list)
            {
                finalList.Add(new AccountBasket()
                {
                    Count = item.Count,
                    Datetime = DateTime.Parse(item.Datetime.ToShortDateString()),
                    Price = item.Price,
                    ID = item.ID,
                    ProductColorId = item.ProductColorId,
                    ProductSizeId = item.ProductSizeId,
                    ProductOptionItemId = item.ProductOptionItemId,
                    ResellerId = item.ResellerId,
                    ProductId = item.ProductId,
                    ProductPrice = item.ProductPrice,
                    ProductDiscount = item.ProductDiscount,
                    StoreId = item.StoreId

                });
            }
            HttpCookie cookie = new HttpCookie("BASKET")
            {
                Value = JsonConvert.SerializeObject(finalList),
                Expires = DateTime.Now.AddDays(1)
            };
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public void MergeAccountBasket(int accountId)
        {
            List<AccountBasket> listBasketCookie = GetAllFromCookie();
            List<AccountBasket> listAccountBasket = GetAllByAccount(accountId);
            List<AccountBasket> listFinal = new List<AccountBasket>();
            foreach (AccountBasket item in listBasketCookie)
            {
                AccountBasket entity = listAccountBasket.FirstOrDefault(p =>
                    p.ProductId == item.ProductId &&
                    p.ProductColorId == item.ProductColorId &&
                    p.ProductSizeId == item.ProductSizeId &&
                    p.ProductOptionItemId == item.ProductOptionItemId &&
                    p.StoreId == item.StoreId);
                if (entity == null)
                {
                    AddToBasket(out listFinal, item.Product, accountId, item.ProductColorId, item.ProductSizeId, item.ProductOptionItemId, null, storeId: item.StoreId, count: item.Count);
                }


            }
        }
        public void DeleteNotAvailableItems(UnitOfWork unitOfWork, int? accountId = null)
        {
            var basketList = new List<AccountBasket>();
            if (accountId.HasValue)
            {
                basketList = GetAllByAccount(accountId.Value);
            }
            else
            {
                basketList = GetAllFromCookie();
            }

            foreach (var item in basketList)
            {
                var product = unitOfWork.Product.GetById(item.ProductId);
                if (product != null)
                {
                    if (product.Code.Label != Enum_Code.PRODUCT_STATUS_AVAILABLE.ToString())
                    {
                        Delete(item);
                        Save();
                        continue;
                    }
                    else
                    {
                        if (BaseWebsite.WebsiteSetting.HasStore)
                        {
                            var activeStoreProductQuantities = unitOfWork.StoreProductQuantity.GetActives(item.ProductId);
                            if (!activeStoreProductQuantities.Any(a => a.StoreProduct.StoreId == item.StoreId && a.ProductQuantity.ColorId == item.ProductColorId && a.ProductQuantity.SizeId == item.ProductSizeId && a.ProductQuantity.OptionItemId == item.ProductOptionItemId && a.Count >= item.Count))
                            {
                                Delete(item);
                                Save();
                                continue;
                            }
                        }
                        else
                        {
                            if (!product.ProductQuantity.Any(a => a.ColorId == item.ProductColorId && a.SizeId == item.ProductSizeId && a.OptionItemId == item.ProductOptionItemId))
                            {
                                Delete(item);
                                Save();
                                continue;
                            }
                        }
                    }
                }
            }
        }
    }
}
