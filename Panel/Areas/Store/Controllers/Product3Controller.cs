using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Store.Controllers
{
    [ValidateInput(false)]
    public class Product3Controller : Controller
    {

        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(
            string shopId = null,
            string typeId = null,
            string categoryId = null,
            string subcategoryId = null,
            int? index = null,
            int? pagesize = null,
            string name = null,
            string active = "true")
        {
            index = index == null ? 1 : index;
            pagesize = pagesize == null ? 10 : pagesize;

            SiteUser CurrentUser = _context.SiteUser.GetCurrentUser();
            _context.Permission.CheckPagePermission(CurrentUser, Enum_Permission.Product);
            FillDropDowns(null);

            int? userId = (_context.UserRole.HasUserRole(_context.SiteUserUserGroup.GetAllBySiteUserId(CurrentUser.ID), CurrentUser, Enum_UserRole.ADMIN)) ? default(int?) : _context.SiteUser.GetCurrentUser().ID;
            List<Product> list = _context.Product.Search(pageSize: pagesize.GetValueOrDefault(), index: index.GetValueOrDefault(), typeId: typeId, shopId: null, categoryId: categoryId, subCategoryId: subcategoryId, name: name, active: active, userId: userId, order: Enum_ProductOrder.NEW, activeLocation: false);
            ViewBag.TotalCount = _context.Product.SearchDetail(typeId: typeId,  categoryId: categoryId, subCategoryId: subcategoryId, name: name).TotalCount;
            return View(list.ToView());
        }

        public ActionResult SearchAjax()
        {
            try
            {
                if (_context.Permission.HasPermission(Enum_Permission.Product))
                {
                    return new JsonResult()
                    {
                        Data = _context.Product.GetAll().Select(row => new { Id = row.ID, Name = row.Name }),
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = int.MaxValue
                    };
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return new JsonResult()
                {
                    Data = ex.Message,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_Details);
            Product product = _context.Product.GetById(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        public ActionResult Create(int? shopId)
        {
            SiteUser CurrentUser = _context.SiteUser.GetCurrentUser();
            _context.Permission.CheckPagePermission(CurrentUser, Enum_Permission.Product_New);

            List<Shop> listShops = _context.Shop.GetAllShopForUserId(CurrentUser.ID);

            shopId = shopId == null && listShops.Count > 0 ? listShops[0].ID : shopId;

            Product product = new Product()
            {
                ID = 0,
                Active = true,
                MinOrder = 1,
                LastPrice = 0,
                Price = 0,
                VisitCount = 0,
                ShowNumber = 0,
                Summary = "-",
                ShopId = shopId,
                ProductTypeId = 0,
                ProductCategoryId = 0,
                ProductSubCategoryId = 0,
                Latitude = "",
                Longitude = "",
                IsService = false,
                CreateDateTime = DateTime.Now


            };

            FillDropDowns(null);
            return View(product);
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_New);
            ViewMessage error = new ViewMessage() { Type = Enum_MessageType.SUCCESS };

            List<ProductCustomValue> CustomValues = product.ProductCustomValue.ToList();
            List<ProductPicture> ListPictures = product.ProductPicture.ToList();
            List<ProductColor> ListColors = product.ProductColor.ToList();
            List<ProductSize> ListSizes = product.ProductSize.ToList();
            List<ProductLanguage> listLanguage = product.ProductLanguage.ToList();
            List<Review> listReviw = product.Review.ToList();

            product.ProductCustomValue.Clear();
            product.ProductPicture.Clear();
            product.ProductColor.Clear();
            product.ProductSize.Clear();
            product.ProductLanguage.Clear();
            product.Review.Clear();
            if (IsModelValid(product, out error))
            {
                product.VisitCount = 0;
                product.UpdateDatetime = DateTime.Now;
                product.CreateDateTime = DateTime.Now;
                product.UserId = _context.SiteUser.GetCurrentUser().ID;
                product.BrandId = product.BrandId == 0 ? null : product.BrandId;
                product.ProductOptionId = product.ProductOptionId == 0 ? null : product.ProductOptionId;
                product.UnitId = product.UnitId == 0 ? null : product.UnitId;
                product.CompanyId = product.CompanyId == 0 ? null : product.CompanyId;
                _context.Product.Insert(product);
                bool result = _context.Save();

                if (result == true)
                {
                    foreach (Review item in listReviw)
                    {
                        if (item.PictureId != null && item.Contect != "")
                        {
                            Review entity = new Review()
                            {
                                PictureId = item.PictureId,
                                Contect = item.Contect,
                                ProductId = product.ID
                            };
                            _context.Review.Insert(entity);
                        }

                    }
                    foreach (ProductCustomValue item in CustomValues)
                    {
                        ProductCustomValue entity = new ProductCustomValue()
                        {
                            FieldId = item.FieldId,
                            ItemId = item.ItemId == 0 ? null : item.ItemId,
                            ProductId = product.ID,
                            Value = item.Value
                        };
                        _context.ProductCustomValue.Insert(entity);
                    }
                    foreach (ProductPicture item in ListPictures)
                    {
                        ProductPicture entity = new ProductPicture()
                        {
                            PictureId = item.PictureId,
                            ProductId = product.ID
                        };
                        _context.ProductPicture.Insert(entity);
                    }


                    foreach (ProductColor color in ListColors)
                    {
                        ProductColor entity = new ProductColor()
                        {
                            ProductId = product.ID,
                            ColorId = color.ColorId
                        };
                        _context.ProductColor.Insert(entity);
                    }
                    foreach (ProductSize size in ListSizes)
                    {
                        ProductSize entity = new ProductSize()
                        {
                            ProductId = product.ID,
                            SizeId = size.SizeId
                        };
                        _context.ProductSize.Insert(entity);
                    }
                    foreach (ProductLanguage item in listLanguage)
                    {
                        item.ProductId = product.ID;
                        _context.ProductLanguage.Insert(item);
                        _context.Save();
                    }
                    _context.Save();
                    if (product.IsCopy == true)
                    {
                        Product productCopy = new Product();
                        List<ProductCustomValue> CustomValuesCopy = CustomValues;
                        List<ProductPicture> ListPicturesCopy = ListPictures;
                        List<ProductColor> ListColorsCopy = ListColors;
                        List<ProductSize> ListSizesCopy = ListSizes;
                        List<ProductLanguage> listLanguageCopy = listLanguage;
                        List<Review> listReviwCopy = listReviw;
                        //productCopy = product;
                        //productCopy.ID = 0;
                        productCopy.Name = product.Name;
                        productCopy.UpdateDatetime = DateTime.Now;
                        productCopy.CreateDateTime = DateTime.Now;
                        productCopy.UserId = _context.SiteUser.GetCurrentUser().ID;
                        productCopy.BrandId = product.BrandId == 0 ? null : product.BrandId;
                        productCopy.ProductOptionId = product.ProductOptionId == 0 ? null : product.ProductOptionId;
                        productCopy.UnitId = product.UnitId == 0 ? null : product.UnitId;
                        productCopy.CompanyId = product.CompanyId == 0 ? null : product.CompanyId;
                        productCopy.Name = product.Name;
                        productCopy.ShopId = product.ShopId;
                        productCopy.ProductTypeId = product.ProductTypeId;
                        productCopy.ProductCategoryId = product.ProductCategoryId;
                        productCopy.ProductSubCategoryId = product.ProductSubCategoryId;
                        productCopy.StatusId = product.StatusId;
                        productCopy.Summary = product.Summary;
                        productCopy.CodeValue = product.CodeValue;
                        productCopy.ShowNumber = product.ShowNumber;
                        productCopy.Description = product.Description;
                        productCopy.Price = product.Price;
                        productCopy.ShowHomePage = product.ShowHomePage;
                        productCopy.PictureId = product.PictureId;
                        productCopy.DocId = product.DocId;
                        productCopy.Active = product.Active;
                        productCopy.SyncDatetime = product.SyncDatetime;
                        productCopy.VisitCount = product.VisitCount;
                        productCopy.IsService = product.IsService;
                        productCopy.H1 = product.H1;
                        productCopy.Title = product.Title;
                        productCopy.Url = product.Url;
                        productCopy.Img = product.Img;
                        productCopy.Latitude = product.Latitude;
                        productCopy.Longitude = product.Longitude;
                        productCopy.ProductOptionId = product.ProductOptionId;
                        productCopy.Weight = product.Weight;

                        _context.Product.Insert(productCopy);
                        bool result2 = _context.Save();

                        if (result2 == true)
                        {
                            foreach (Review item in listReviwCopy)
                            {
                                if (item.PictureId != null && item.Contect != "")
                                {
                                    Review entity = new Review()
                                    {
                                        PictureId = item.PictureId,
                                        Contect = item.Contect,
                                        ProductId = productCopy.ID
                                    };
                                    _context.Review.Insert(entity);
                                }

                            }
                            foreach (ProductCustomValue item in CustomValuesCopy)
                            {
                                ProductCustomValue entity = new ProductCustomValue()
                                {
                                    FieldId = item.FieldId,
                                    ItemId = item.ItemId == 0 ? null : item.ItemId,
                                    ProductId = productCopy.ID,
                                    Value = item.Value
                                };
                                _context.ProductCustomValue.Insert(entity);
                            }
                            foreach (ProductPicture item in ListPicturesCopy)
                            {
                                ProductPicture entity = new ProductPicture()
                                {
                                    PictureId = item.PictureId,
                                    ProductId = productCopy.ID
                                };
                                _context.ProductPicture.Insert(entity);
                            }


                            foreach (ProductColor color in ListColorsCopy)
                            {
                                ProductColor entity = new ProductColor()
                                {
                                    ProductId = productCopy.ID,
                                    ColorId = color.ColorId
                                };
                                _context.ProductColor.Insert(entity);
                            }
                            foreach (ProductSize size in ListSizesCopy)
                            {
                                ProductSize entity = new ProductSize()
                                {
                                    ProductId = productCopy.ID,
                                    SizeId = size.SizeId
                                };
                                _context.ProductSize.Insert(entity);
                            }
                            foreach (ProductLanguage item in listLanguageCopy)
                            {
                                item.ProductId = productCopy.ID;
                                _context.ProductLanguage.Insert(item);
                                _context.Save();
                            }
                            _context.Save();
                        }
                        _context.ProductQuantity.UpsertProductQuantiy(productCopy.ID, null, true, _context);
                    }
                    _context.ProductQuantity.UpsertProductQuantiy(product.ID, null, true, _context);

                    if (BaseWebsite.WebsiteSetting.HasUnitPrice)
                    {
                        double computeprice = _context.Product.ComputeFee();
                        _context.Product.UpdateProductPriceBaseUnitPrice(product.ID, computeprice, _context);
                    }



                }
            }
            return new JsonResult() { Data = error };
        }


        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Product product = _context.Product.GetById(id);
            if (product == null)
                return HttpNotFound();

            SiteUser CurrentUser = _context.SiteUser.GetCurrentUser();
            List<Shop> listShops = _context.Shop.GetAllShopForUserId(CurrentUser.ID);

            product.ShopId = product.ShopId == null && listShops.Count > 0 ? listShops[0].ID : product.ShopId;

            FillDropDowns(product);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(product);
        }
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            SiteUser currentUser = _context.SiteUser.GetCurrentUser();
            _context.Permission.CheckPagePermission(currentUser, Enum_Permission.Product_Edit);

            List<ProductCustomValue> CustomValues = product.ProductCustomValue.ToList();
            List<ProductPicture> ListPictures = product.ProductPicture.ToList();
            List<ProductColor> ListColors = product.ProductColor.ToList();
            List<ProductSize> ListSizes = product.ProductSize.ToList();
            List<ProductLanguage> listLanguage = product.ProductLanguage.ToList();
            List<Review> listReview = product.Review.ToList();
            product.ProductCustomValue.Clear();
            product.ProductPicture.Clear();
            var LastListSizes = _context.ProductSize.Where(s => s.ProductId == product.ID).ToList();
            var LastListColors = _context.ProductColor.Where(s => s.ProductId == product.ID).ToList();

            product.ProductColor.Clear();
            product.ProductSize.Clear();
            product.ProductLanguage.Clear();
            product.Review.Clear();
            ViewMessage error = new ViewMessage() { Type = Enum_MessageType.SUCCESS };

            if (IsModelValid(product, out error))
            {
                product.UpdateDatetime = DateTime.Now;
                product.BrandId = product.BrandId == 0 ? null : product.BrandId;
                product.UnitId = product.UnitId == 0 ? null : product.UnitId;
                product.CompanyId = product.CompanyId == 0 ? null : product.CompanyId;
                product.UserId = currentUser.ID;
                product.ProductOptionId = product.ProductOptionId == 0 ? null : product.ProductOptionId;
                _context.Product.Update(product);
                _context.Save();
                _context.ProductCustomValue.DeleteByProductId(product.ID);
                _context.ProductPicture.DeleteByProductId(product.ID, true);
                _context.ProductColor.DeleteByProductId(product.ID);
                _context.ProductSize.DeleteByProductId(product.ID);
                _context.ProductLanguage.DeleteByProductId(product.ID);
                _context.Review.DeleteByProductId(product.ID);
                bool result = _context.Save();
                foreach (ProductCustomValue item in CustomValues)
                {
                    ProductCustomValue entity = new ProductCustomValue()
                    {
                        FieldId = item.FieldId,
                        ItemId = item.ItemId == 0 ? null : item.ItemId,
                        ProductId = product.ID,
                        Value = item.Value
                    };
                    _context.ProductCustomValue.Insert(entity);
                }
                result = _context.Save();
                foreach (ProductPicture item in ListPictures)
                {
                    ProductPicture entity = new ProductPicture()
                    {
                        PictureId = item.PictureId,
                        ProductId = product.ID
                    };
                    _context.ProductPicture.Insert(entity);
                }
                result = _context.Save();
                foreach (ProductColor color in ListColors)
                {
                    ProductColor entity = new ProductColor()
                    {
                        ProductId = product.ID,
                        ColorId = color.ColorId
                    };
                    _context.ProductColor.Insert(entity);
                }

                foreach (ProductSize size in ListSizes)
                {
                    ProductSize entity = new ProductSize()
                    {
                        ProductId = product.ID,
                        SizeId = size.SizeId
                    };
                    _context.ProductSize.Insert(entity);
                }
                result = _context.Save();
                foreach (ProductLanguage item in listLanguage)
                {
                    item.ProductId = product.ID;
                    _context.ProductLanguage.Insert(item);
                    _context.Save();
                }
                result = _context.Save();
                foreach (Review review in listReview)
                {
                    if (review.PictureId != null && review.Contect != "")
                    {
                        Review entity = new Review()
                        {
                            PictureId = review.PictureId,
                            Contect = review.Contect,
                            ProductId = product.ID
                        };
                        _context.Review.Insert(entity);
                    }
                }
                result = _context.Save();
                if (BaseWebsite.WebsiteSetting.HasUnitPrice)
                {
                    double computeprice = _context.Product.ComputeFee();

                    _context.Product.UpdateProductPriceBaseUnitPrice(product.ID, computeprice, _context);

                }
                else
                {
                    _context.ProductQuantity.UpsertProductQuantiy(product.ID, null, false, _context);
                }
            }
            return new JsonResult() { Data = error };
        }
        public ActionResult Quantity(int? id)
        {

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ViewBag.Product = _context.Product.GetById(id.GetValueOrDefault());

            List<ProductQuantity> listQuantity = _context.ProductQuantity.GetAllByProductId(id.GetValueOrDefault());
            return View(listQuantity);
        }

        [HttpPost]
        public ActionResult Quantity(List<ProductQuantity> listQuantiy, int productId)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_Edit);
            ViewMessage error = new ViewMessage() { Type = Enum_MessageType.SUCCESS };
            _context.ProductQuantity.UpdateAll(listQuantiy, productId);


            return new JsonResult() { Data = error };
        }

        private bool IsModelValid(Product product, out ViewMessage msg)
        {
            bool result = false;

            product.DocId = product.DocId != 0 ? product.DocId : null;
            product.PictureId = product.PictureId != 0 ? product.PictureId : null;
            product.ProductTypeId = product.ProductTypeId != 0 ? product.ProductTypeId : null;
            product.ProductCategoryId = product.ProductCategoryId != 0 ? product.ProductCategoryId : null;
            product.ProductSubCategoryId = product.ProductSubCategoryId != 0 ? product.ProductSubCategoryId : null;

            if (product.ShopId == 0)
                msg = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_SHOP);
            else if (product.Name == null)
                msg = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_NAME);
            else if (product.StatusId == 0)
                msg = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_STATUS);
            //else if (product.Name.Length > 40)
            //    msg = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_STATUS);
            else
            {
                msg = BaseMessage.GetMessage(Enum_MessageType.SUCCESS);
                result = true;
            }

            return result;
        }

        private void FillDropDowns(Product product)
        {
            SiteUser CurrentUser = _context.SiteUser.GetCurrentUser();
            ViewBag.BrandId = new SelectList(new List<ProductBrand>(), "ID", "Name");
            ViewBag.ProductOptionId = new SelectList(new List<ProductOption>(), "ID", "Name");
            ViewBag.ProductTypeId = new SelectList(new List<ProductType>(), "ID", "Name");
            ViewBag.ShopId = new SelectList(_context.Shop.GetAllShopForUserId(CurrentUser.ID), "ID", "Name", product != null ? product.ShopId : 0);
            ViewBag.ProductCategoryId = new SelectList(new List<ProductCategory>(), "ID", "Name", 0);
            ViewBag.ProductSubCategoryId = new SelectList(new List<ProductSubCategory>(), "ID", "Name", product != null ? product.ProductSubCategoryId : 0);
            ViewBag.StatusId = new SelectList(_context.Code.GetAllByCodeGroup(Enum_CodeGroup.PRODUCT_STATUS), "ID", "Name", product != null ? product.StatusId : 0);
            ViewBag.UnitId = new SelectList(_context.Unit.GetAll(), "ID", "Name", product != null ? product.UnitId : 0);
            ViewBag.CompanyId = new SelectList(_context.ProductCompany.GetAll(), "ID", "Name", product != null ? product.CompanyId : 0);

        }

        [HttpGet]
        public ActionResult FillProductType(int? ShopId)
        {
            return new JsonResult()
            {
                Data = _context.ProductType.Search(shopId: ShopId, pageSize: 100),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult FillProductCategory(int TypeId)
        {

            return new JsonResult()
            {
                Data = _context.ProductCategory.Search(typeId: TypeId, pageSize: 100),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult FillProductSubCategory(string CategoryId = null)
        {
            List<ProductSubCategoryVm> listSubCategory = (CategoryId == null) ? new List<ProductSubCategoryVm>() : _context.ProductSubCategory.Search(categoryId: CategoryId, pageSize: 100);
            return new JsonResult()
            {
                Data = listSubCategory,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult FillProductBrand(int TypeId)
        {
            SiteUser user = _context.SiteUser.GetCurrentUser();
            return new JsonResult()
            {
                Data = _context.ProductBrand.Search(typeId: TypeId, pageSize: 2000, userId: user.ID),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult FillProductCustomFields(int TypeId = 0, int CategoryId = 0, int SubCategoryId = 0, int BrandId = 0, int ProductId = 0)
        {
            List<ProductCustomField> list = _context.ProductCustomField.Search(typeId: TypeId, categoryId: CategoryId, subcategoryId: SubCategoryId, brandId: BrandId, pageSize: 1000);
            List<ViewProductCustomField> listFields = list.ToView();
            if (ProductId != 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    ProductCustomField item = list[i];
                    ProductCustomValue value = item.ProductCustomValue.FirstOrDefault(p => p.ProductId == ProductId && p.FieldId == item.ID);
                    if (value != null)
                    {
                        if (item.Code.Label == Enum_Code.FIELD_TYPE_DROPDOWN.ToString())
                            listFields[i].ProductFieldItem = value.ItemId;
                        else
                            listFields[i].ProductFieldValue = value.Value;
                    }
                }
            }

            return new JsonResult()
            {
                Data = listFields,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult FillColors(int TypeId, int ProductId)
        {
            List<ViewColor> listColor = _context.Color.GetAllProductTypeId(TypeId).ToView();
            List<ProductColor> listSelectedColor = _context.ProductColor.GetAllByProductId(ProductId);
            foreach (ViewColor color in listColor)
            {
                color.IsSelected = listSelectedColor.Any(p => p.ColorId == color.Id);
            }

            return new JsonResult()
            {
                Data = listColor,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult FillSize(int TypeId, int ProductId)
        {
            List<ViewSize> listSize = _context.Size.GetAllProductTypeId(TypeId).ToView();
            List<ProductSize> listSelectedSize = _context.ProductSize.GetAllByProductId(ProductId);
            foreach (ViewSize size in listSize)
            {
                size.IsSelected = listSelectedSize.Any(p => p.SizeId == size.Id);
            }

            return new JsonResult()
            {
                Data = listSize,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public ActionResult FillProductOption(int? typeId)
        {

            var listOption = _context.ProductOption.Search(typeId, 200, 1).ToView();
            return new JsonResult()
            {
                Data = listOption,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Product product = _context.Product.GetById(id);
            if (product == null)
                return HttpNotFound();

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_Delete);
            //Product product = _context.Product.GetById(id);

            //_context.ProductPicture.DeleteByProductId(product.ID);
            //_context.ProductLanguage.DeleteByProductId(product.ID);
            //_context.ProductLike.DeleteByProductId(product.ID);
            //_context.Discount.DeleteByProductId(product.ID);
            //_context.ProductCustomValue.DeleteByProductId(product.ID);
            //_context.ProductColor.DeleteByProductId(product.ID);
            //_context.ProductSize.DeleteByProductId(product.ID);
            //_context.ProductQuantity.DeleteByProductId(product.ID);
            //_context.Save();

            _context.Product.Hide(id);
            _context.Save();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Picture(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Product product = _context.Product.GetById(id);
            if (product == null)
                return HttpNotFound();
            return View(product);
        }

        [HttpPost]
        public ActionResult Picture([Bind(Include = "ID")] Product product, HttpPostedFileBase file, int? ColorId = null)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_Edit);

            if (file != null)
            {
                Picture picture = _context.Picture.CreateAndUpload(Enum_Code.SYSTEM_TYPE_PANEL, file);
                ProductPicture pic = new ProductPicture()
                {
                    PictureId = picture.ID,
                    ProductId = product.ID,
                    ColorId = ColorId
                };
                _context.ProductPicture.Insert(pic);
                _context.Save();
            }
            //product = _context.Product.GetById(product.ID);
            return RedirectToAction("picture", new { id = product.ID });
        }

        [HttpPost]
        public ActionResult PictureDelete(int ProductId, int PictureId)
        {
            _context.ProductPicture.Delete(PictureId);
            _context.Save();
            return RedirectToAction("Picture", "Product", new { id = ProductId });
        }
        public ActionResult AddToStore(int productId, int? storeId)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_Edit);
            var model = new ProductToStoreVm();
            model.ProductId = productId;
            model.StoreId = storeId;
            model.ProductQuantities = _context.ProductQuantity.GetForStore(productId: productId, storeId: storeId);
            ViewBag.ProductId = new SelectList(_context.Product.Search(pageSize:9000000), "ID", "Name", productId);
            ViewBag.StoreId = new SelectList(_context.Store.GetAll(), "ID", "Name", storeId);

            return View(model);
        }
        [HttpPost]
        public ActionResult AddToStore(ProductToStoreVm productToStoreVm)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_Edit);
            if (productToStoreVm.StoreId.HasValue )
            {
                _context.ProductQuantity.AddProductToStore(unitOfWork: _context,productToStore: productToStoreVm);
            }
            else
            {
                RedirectToAction("AddToStore", new { productId= productToStoreVm.ProductId, storeId= productToStoreVm.StoreId });
            }
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}