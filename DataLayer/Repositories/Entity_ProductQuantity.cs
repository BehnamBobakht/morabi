using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.ViewModels;
using ExpressMapper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ProductQuantity : BaseRepository<ProductQuantity>
    {
        private DatabaseEntities _context;
        public Entity_ProductQuantity(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public void DeleteByProductId(int productId)
        {
            List<ProductQuantity> list = GetAllByProductId(productId);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }


        public void UpdateProductQuentity(int productId, double? price)
        {
            //List<ProductColor> listColor = _context.ProductColor.Where(x => x.ProductId == productId).ToList();
            //if (!listColor.Any())
            //    listColor.Add(null);
            //List<ProductSize> listSize = _context.ProductSize.Where(x => x.ProductId == productId).ToList();
            //if (!listSize.Any())
            //    listSize.Add(null);
            //List<ProductOptionItem> listOptionItem = _context.ProductOptionItem.Where(x => x.ProductOption.Product.Any(p => p.ID == productId)).ToList();
            //if (!listOptionItem.Any())
            //    listOptionItem.Add(null);
            //List<ProductQuantity> listEntity = new List<ProductQuantity>();

            //foreach (ProductColor color in listColor)
            //{
            //    foreach (ProductSize size in listSize)
            //    {
            //        foreach (ProductOptionItem optionItem in listOptionItem)
            //        {
            //            ProductQuantity quantity = new ProductQuantity()
            //            {
            //                ColorId = color?.ColorId,
            //                ProductId = productId,
            //                SizeId = size?.SizeId,
            //                OptionItemId = optionItem?.ID,
            //                Price = price,
            //            };
            //            listEntity.Add(quantity);
            //        }
            //    }
            //}

            //List<ProductQuantity> listQuantity = _context.ProductQuantity.Where(x => x.ProductId == productId).ToList();
            //var deleteList = listQuantity.Where(s => ((listColor[0] != null) && !listColor.Any(a => a.ColorId.Equals(s.ColorId))) || ((listSize[0] != null) && !listSize.Any(a => a.SizeId.Equals(s.SizeId))) || ((s.OptionItemId != null) && !listOptionItem.Any(a => a.ID.Equals(s.OptionItemId))));
            //foreach (var item in deleteList)
            //{
            //    if (item.StoreProductQuantity.Any())
            //    {
            //        foreach (StoreProductQuantity item2 in item.StoreProductQuantity)
            //        {
            //            _context.StoreProductQuantity.Remove(item2);
            //            _context.SaveChanges();
            //        }
            //    }
            //    Delete(item);
            //    Save();
            //}
            List<ProductQuantity> listQuantity = _context.ProductQuantity.Where(x => x.ProductId == productId).ToList();

            foreach (ProductQuantity item in listQuantity)
            {
                item.Price = price;
                Update(item);
                Save();
            }
        }

        public List<ProductQuantity> GetAllByProductId(int productId)
        {
            return _context.ProductQuantity.Where(p => p.ProductId == productId).ToList();
        }
        public List<ProductQuantity> GetAllActives(int productId)
        {
            return _context.ProductQuantity.Where(p => p.ProductId == productId && p.Price.HasValue && p.Price > 0 && p.Count > 0 && p.Product.Active).ToList();
        }

        public ProductQuantity GetByProductIdAndColorAndSize(int productId, int? colorId = null, int? sizeId = null, int? optionItemId = null)
        {
            return _context.ProductQuantity.FirstOrDefault(p => p.ProductId == productId &&
                p.ColorId == colorId &&
                p.SizeId == sizeId &&
                p.OptionItemId == optionItemId
            );
        }
        public void UpdateAll(List<ProductQuantity> listQuantiy, int productId)
        {
            foreach (ProductQuantity item in listQuantiy)
            {
                ProductQuantity entity = GetById(item.ID);
                entity.Price = item.Price;
                entity.Count = item.Count;
                Save();
            }
            Base.BaseStore.UpdateProductQuantity(productId: productId);
        }

        public void UpsertProductQuantiy(int productId, double? price, bool isInsert, UnitOfWork _context)
        {
            List<ProductColor> listColor = _context.ProductColor.GetAllByProductId(productId);
            if (!listColor.Any())
                listColor.Add(null);
            List<ProductSize> listSize = _context.ProductSize.GetAllByProductId(productId);
            if (!listSize.Any())
                listSize.Add(null);
            List<ProductProductOptionItem> listOptionItem = _context.ProductProductOptionItem.GetAllByProductId(productId);
            if (!listOptionItem.Any())
                listOptionItem.Add(null);
            List<ProductQuantity> listEntity = new List<ProductQuantity>();
            var temp = listOptionItem[0];
            if (temp!=null && price > 0)
            {
                var count = listOptionItem.Count();
                var pricEuro = price.Value / count;
                double temPrice = price.Value;
                foreach (ProductColor color in listColor)
                {
                    foreach (ProductSize size in listSize)
                    {
                        foreach (ProductProductOptionItem optionItem in listOptionItem.OrderByDescending(s => s.ID))
                        {

                            double computeprice = GetEuroPrice(temPrice);
                            ProductQuantity quantity = new ProductQuantity()
                            {
                                ColorId = color?.ColorId,
                                ProductId = productId,
                                SizeId = size?.SizeId,
                                OptionItemId = optionItem?.ProductOptionItemId,
                                Price = computeprice,
                                EuroPrice= temPrice
                            };
                            listEntity.Add(quantity);
                            temPrice -= pricEuro;
                        }
                    }
                }
            }
            else if(price > 0)
            {
                foreach (ProductColor color in listColor)
                {
                    foreach (ProductSize size in listSize)
                    {
                        foreach (ProductProductOptionItem optionItem in listOptionItem)
                        {
                            ProductQuantity quantity = new ProductQuantity()
                            {
                                ColorId = color?.ColorId,
                                ProductId = productId,
                                SizeId = size?.SizeId,
                                OptionItemId = optionItem?.ProductOptionItemId,
                                Price = GetEuroPrice(price.Value) ,
                                EuroPrice= price.Value
                            };
                            listEntity.Add(quantity);
                        }
                    }
                }
            }
            else
            {
                foreach (ProductColor color in listColor)
                {
                    foreach (ProductSize size in listSize)
                    {
                        foreach (ProductProductOptionItem optionItem in listOptionItem)
                        {
                            ProductQuantity quantity = new ProductQuantity()
                            {
                                ColorId = color?.ColorId,
                                ProductId = productId,
                                SizeId = size?.SizeId,
                                OptionItemId = optionItem?.ProductOptionItemId,
                                Price = price,
                            };
                            listEntity.Add(quantity);
                        }
                    }
                }

            }

            if (isInsert)
            {
                foreach (ProductQuantity item in listEntity)
                {
                    ProductQuantity quantity = new ProductQuantity()
                    {
                        ColorId = item.ColorId,
                        ProductId = item.ProductId,
                        SizeId = item.SizeId,
                        OptionItemId = item.OptionItemId,
                        Count = item.Count,
                        Price = item.Price,
                        EuroPrice=item.EuroPrice
                        
                    };
                    _context.ProductQuantity.Insert(quantity);
                }
                _context.Save();
            }
            else
            {
                List<ProductQuantity> listQuantity = _context.ProductQuantity.GetAllByProductId(productId);
                List<ProductQuantity> deleteList = new List<ProductQuantity>();
                if (listColor[0] != null || listSize[0] != null || listOptionItem[0] != null)
                {
                    deleteList = listQuantity.Where(s => ((listColor[0] != null) && !listColor.Any(a => a.ColorId.Equals(s.ColorId))) || ((listSize[0] != null) && !listSize.Any(a => a.SizeId.Equals(s.SizeId))) || ((s.OptionItemId != null) && !listOptionItem.Any(a => a.ID.Equals(s.OptionItemId))) && s.StoreProductQuantity.Count() == 0).ToList();
                }
                else
                {
                    deleteList = listQuantity.Where(s => s.ColorId.HasValue || s.SizeId.HasValue || s.OptionItemId.HasValue).ToList();

                }

                listQuantity = _context.ProductQuantity.GetAllByProductId(productId);
                List<ProductQuantity> ProcessList = new List<ProductQuantity>();
                foreach (ProductQuantity item in listEntity)
                {
                    ProductQuantity quantity = listQuantity.FirstOrDefault(p =>
                            p.ProductId == item.ProductId &&
                            p.ColorId == item.ColorId &&
                            p.SizeId == item.SizeId &&
                            p.OptionItemId == item.OptionItemId
                        );

                    if (quantity == null)
                    {
                        quantity = new ProductQuantity
                        {
                            ColorId = item.ColorId,
                            Count = item.Count,
                            Price = item.Price,
                            EuroPrice=item.EuroPrice,
                            SizeId = item.SizeId,
                            ProductId = productId,
                            OptionItemId = item.OptionItemId
                        };
                        _context.ProductQuantity.Insert(quantity);
                        _context.Save();
                    }
                    else
                    {
                        if (price != null)
                        {
                            var updateModel = _context.ProductQuantity.GetById(quantity.ID);
                            updateModel.Price = item.Price.Value ;
                            updateModel.EuroPrice = item.EuroPrice;

                            _context.ProductQuantity.Update(updateModel);
                            _context.Save();
                        }
                       
                    }
                }
            }
            Base.BaseStore.UpdateProductQuantity(productId, _context);
        }
        public void UpsertProductQuantiy2(int productId, double? price, bool isInsert, UnitOfWork _context)
        {
            List<ProductColor> listColor = _context.ProductColor.GetAllByProductId(productId);
            if (!listColor.Any())
                listColor.Add(null);
            List<ProductSize> listSize = _context.ProductSize.GetAllByProductId(productId);
            if (!listSize.Any())
                listSize.Add(null);
            List<ProductProductOptionItem> listOptionItem = _context.ProductProductOptionItem.GetAllByProductId(productId);
            if (!listOptionItem.Any())
                listOptionItem.Add(null);
            List<ProductQuantity> listEntity = new List<ProductQuantity>();

            foreach (ProductColor color in listColor)
            {
                foreach (ProductSize size in listSize)
                {
                    foreach (ProductProductOptionItem optionItem in listOptionItem)
                    {
                        ProductQuantity quantity = new ProductQuantity()
                        {
                            ColorId = color?.ColorId,
                            ProductId = productId,
                            SizeId = size?.SizeId,
                            OptionItemId = optionItem?.ProductOptionItemId,
                            Price = price,
                        };
                        listEntity.Add(quantity);
                    }
                }
            }

            if (isInsert)
            {
                foreach (ProductQuantity item in listEntity)
                {
                    ProductQuantity quantity = new ProductQuantity()
                    {
                        ColorId = item.ColorId,
                        ProductId = item.ProductId,
                        SizeId = item.SizeId,
                        OptionItemId = item.OptionItemId,
                        Count = 0,
                        Price = price
                    };
                    _context.ProductQuantity.Insert(quantity);
                }
                _context.Save();
            }
            else
            {
                List<ProductQuantity> listQuantity = _context.ProductQuantity.GetAllByProductId(productId);
                bool deleteExist = false;
                if (listColor[0] == null && listQuantity.Any(s => s.ColorId.HasValue))
                {
                    deleteExist = true;
                }
                else if (listColor[0] != null && listQuantity.Any(s => s.ColorId == null))
                {
                    deleteExist = true;
                }
                if (listSize[0] == null && listQuantity.Any(s => s.SizeId.HasValue))
                {
                    deleteExist = true;
                }
                else if (listSize[0] != null && listQuantity.Any(s => s.Size == null))
                {
                    deleteExist = true;
                }
                if (listOptionItem[0] == null && listQuantity.Any(s => s.OptionItemId.HasValue))
                {
                    deleteExist = true;
                }
                else if (listOptionItem[0] != null && listQuantity.Any(s => s.OptionItemId == null))
                {
                    deleteExist = true;
                }
                if (deleteExist)
                {
                    foreach (var productQuantity in listQuantity)
                    {
                        if (productQuantity.StoreProductQuantity != null && productQuantity.StoreProductQuantity.Any())
                        {
                            foreach (var storeProductQ in productQuantity.StoreProductQuantity)
                            {

                            }
                        }
                    }
                }
                List<ProductQuantity> deleteList = new List<ProductQuantity>();

                listQuantity = _context.ProductQuantity.GetAllByProductId(productId);
                List<ProductQuantity> ProcessList = new List<ProductQuantity>();
                foreach (ProductQuantity item in listEntity)
                {
                    ProductQuantity quantity = listQuantity.FirstOrDefault(p =>
                            p.ProductId == item.ProductId &&
                            p.ColorId == item.ColorId &&
                            p.SizeId == item.SizeId &&
                            p.OptionItemId == item.OptionItemId
                        );

                    if (quantity == null)
                    {
                        quantity = new ProductQuantity
                        {
                            ColorId = item.ColorId,
                            Count = 0,
                            Price = price,
                            SizeId = item.SizeId,
                            ProductId = productId,
                            OptionItemId = item.OptionItemId
                        };
                        _context.ProductQuantity.Insert(quantity);
                        _context.Save();
                    }
                }
            }
            Base.BaseStore.UpdateProductQuantity(productId, _context);
        }

        public List<ProductQuantityVm> GetForStore(int productId, int? storeId)
        {
            var list = new List<ProductQuantityVm>();
            if (storeId.HasValue)
            {
                var productQuantities = Where(s => s.ProductId == productId).ToList();
                list = productQuantities.Map<List<ProductQuantity>, List<ProductQuantityVm>>();
                for (int i = 0; i < productQuantities.Count(); i++)
                {
                    var productQuantity = productQuantities[i].StoreProductQuantity?.FirstOrDefault(s => s.StoreProduct.StoreId == storeId);
                    if (productQuantity != null)
                    {
                        list[i].StoreProductQuantityId = productQuantity.ID;
                        list[i].Price = productQuantity.Price;
                        list[i].EuroPrice = productQuantity.EuroPrice;

                        list[i].Count = productQuantity.Count;
                    }
                }
            }

            return list;
        }
        public double GetEuroPrice(double price)
        {
            double? unitPrice = _context.WebsiteDetail.FirstOrDefault().EuroPrice;
            return unitPrice != null ? unitPrice.Value * price : 1 * price;
        }
        public ResultVm<ProductToStoreVm> AddProductToStore(UnitOfWork unitOfWork, ProductToStoreVm productToStore)
        {
            if (productToStore.StoreId.HasValue)
            {
                var quantities = productToStore.ProductQuantities;
                if (quantities.Any())
                {
                    var storeProduct = _context.StoreProduct.FirstOrDefault(s => s.StoreId == productToStore.StoreId.Value && s.ProductId == productToStore.ProductId);
                    if (storeProduct == null)
                    {
                        storeProduct = new StoreProduct() { StatusId = 3094, CreateDate = DateTime.Now, ProductId = productToStore.ProductId, StoreId = productToStore.StoreId.Value };
                        unitOfWork.StoreProduct.Insert(storeProduct);
                        unitOfWork.Save();
                    }

                    foreach (var item in quantities)
                    {
                        var spq = unitOfWork.StoreProductQuantity.FirstOrDefault(s => s.StoreProductId == storeProduct.ID && s.ProductQuantityId == item.ID);
                        if (spq != null)
                        {
                            spq.Count = item.Count.HasValue ? item.Count.Value : 0;
                            spq.Price = item.Price.HasValue ? item.Price.Value : 0;
                            unitOfWork.StoreProductQuantity.Update(spq);
                            unitOfWork.Save();
                        }
                        else
                        {
                            spq = new StoreProductQuantity()
                            {
                                Count = item.Count.HasValue ? item.Count.Value : 0,
                                Price = item.Price.HasValue ? item.Price.Value : 0,
                                StoreProductId = storeProduct.ID,
                                ProductQuantityId = item.ID,
                            };
                            unitOfWork.StoreProductQuantity.Insert(spq);
                            unitOfWork.Save();
                        }
                    }

                    if (BaseWebsite.WebsiteSetting.HasUnitPrice)
                    {
                        double computeprice = unitOfWork.Product.ComputeFee();
                        unitOfWork.Product.UpdateProductPriceBaseUnitPrice(productToStore.ProductId, computeprice, unitOfWork);
                    }
                    else
                    {
                        Base.BaseStore.UpdateProductQuantity(productId: productToStore.ProductId, unitOfWork: unitOfWork);
                    }
                }
            }
            return new ResultVm<ProductToStoreVm>() { Status = Enumarables.ResultStatus.Success, Message = Resource.Notify.success };
        }
    }
}
