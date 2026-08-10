using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Store.Controllers
{
    public class StoreProductController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        #region Utilites

        public JsonResult LoadProductTypes(int storeId)
        {
            var result = _context.ProductType.GetStoreProductTypes(storeId).Select(productType => new SelectListItem
            {
                Text = productType.GetName(),
                Value = productType.ID.ToString(),
                Disabled = !productType.Active
            }).ToList();
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = result
            };
        }

        public JsonResult LoadProductCategories(int storeId, int productTypeId)
        {
            var pcs = _context.ProductCategory.GetStoreProductCategories(storeId).Where(pc => pc.TypeId == productTypeId);
            var result = pcs.Select(pc => new SelectListItem
            {
                Text = pc.Name,
                Value = pc.ID.ToString(),
                Disabled = !pc.Active
            });
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = result
            };
        }

        public JsonResult LoadProductSubCategories(int storeId, int productCategoryId)
        {
            var pscS = _context.ProductSubCategory.GetStoreProductSubCategories(storeId).Where(psc => psc.CategoryId == productCategoryId);
            var result = pscS.Select(pc => new SelectListItem
            {
                Text = pc.Name,
                Value = pc.ID.ToString(),
                Disabled = !pc.Active
            });
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = result
            };
        }

        public JsonResult LoadProducts(int storeId, int productSubCategoryId)
        {
            var products = _context.StoreProduct.Search(storeId: storeId, pageSize: int.MaxValue)
                .Where(x => x.Product.ProductSubCategoryId == productSubCategoryId);
            var result = products.Select(pc => new SelectListItem
            {
                Text = pc.Product.Name,
                Value = pc.ID.ToString(),
                Disabled = !pc.Product.Active
            });
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = result
            };
        }

        public JsonResult LoadProductQuantities(int storeId, int productId)
        {
            var quantities = GetStoreProductQuantities(storeId).Where(x => x.StoreProductId == productId).ToList();
            var result = quantities.Select(x => new SelectListItem
            {
                Text = GenerateVarietyText(x),
                Value = x.ID.ToString(),
                Disabled = !x.ProductQuantity.Product.Active
            });

            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = result
            };
        }

        public string GenerateVarietyText(StoreProductQuantity spq)
        {
            if (!spq.ProductQuantity.ColorId.HasValue && !spq.ProductQuantity.SizeId.HasValue && !spq.ProductQuantity.OptionItemId.HasValue)
                return Resource.Lang.NoVariety;

            var result = string.Empty;
            result += spq.ProductQuantity.ColorId.HasValue
                ? $"{Resource.Lang.Color} : {spq.ProductQuantity.Color.GetName()}"
                : string.Empty;

            if (spq.ProductQuantity.SizeId.HasValue)
            {
                result += string.IsNullOrWhiteSpace(result) ? string.Empty : " | ";
                result += $"{Resource.Lang.Size} : {spq.ProductQuantity.Size.GetName()}";
            }

            if (spq.ProductQuantity.OptionItemId.HasValue)
            {
                result += string.IsNullOrWhiteSpace(result) ? string.Empty : " | ";
                result += $"{spq.ProductQuantity.ProductOptionItem.Value} : {spq.ProductQuantity.ProductOptionItem.ProductOption.Name}";
            }

            return result;
        }

        public List<StoreProductQuantity> GetStoreProductQuantities(int storeId)
        {
            return _context.StoreProductQuantity.GetForStore(storeId);
        }

        [HttpPost]
        public ActionResult UpdatePrice(bool percent, bool increase, double changeAmount, int groupPricingType, int storeId, int? productTypeId = null, int? productCategoryId = null, int? productSubCategoryId = null, int? productId = null, int? quantityId = null)
        {
            if (changeAmount <= 0 || groupPricingType < 0 || groupPricingType > 5)
                return RedirectToAction("Index");

            double GetFinalPrice(StoreProductQuantity spq)
            {
                var changePrice = percent ? spq.Price * changeAmount / 100 : changeAmount;
                var price = increase ? spq.Price + changePrice : spq.Price - changePrice;
                return price > 0 ? price : 0;
            }

            var storeProductQuantities = GetStoreProductQuantities(storeId);
            var targetList = new List<StoreProductQuantity>();

            switch (groupPricingType)
            {
                case 0:// all
                    targetList = storeProductQuantities;
                    break;

                case 1:
                    targetList = storeProductQuantities.Where(x => x.StoreProduct.Product.ProductTypeId == productTypeId).ToList();
                    break;

                case 2:
                    targetList = storeProductQuantities.Where(x => x.StoreProduct.Product.ProductCategoryId == productCategoryId).ToList();
                    break;

                case 3:
                    targetList = storeProductQuantities.Where(x => x.StoreProduct.Product.ProductSubCategoryId == productSubCategoryId).ToList();
                    break;

                case 4:
                    targetList = storeProductQuantities.Where(x => x.StoreProductId == productId).ToList();
                    break;

                case 5:
                    targetList = storeProductQuantities.Where(x => x.ID == quantityId).ToList();
                    break;
            }

            if (targetList.Any())
            {
                foreach (var spq in targetList)
                {
                    spq.Price = GetFinalPrice(spq);
                    _context.StoreProductQuantity.Update(spq);
                }
                _context.Save();
            }

            return RedirectToAction("Index", new { storeId = storeId });
        }

        #endregion

        public ActionResult Index(int? index = null, int? pagesize = null, int? statusId = null, int? storeId = null, string name = null)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.StoreProduct);

            index = index == null ? 1 : index;
            pagesize = pagesize == null ? 10 : pagesize;
            var model = _context.StoreProduct.Search(statusId: statusId, index: index, pageSize: pagesize, name: name, storeId: storeId);
            ViewBag.TotalCount = _context.StoreProduct.SearchCount(statusId: statusId, name: name, storeId: storeId);
            FillDropDowns();
            return View(model);
        }
        public void ChangeStatus(int storeProductId, int statusId)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.StoreProduct);

            var model = _context.StoreProduct.GetById(storeProductId);
            if (model != null)
            {
                model.StatusId = statusId;
                _context.Save();
            }
        }
        private void FillDropDowns()
        {
            ViewBag.Statuses = new SelectList(_context.Code.GetAllByCodeGroup(Enum_CodeGroup.STORE_REQUEST), "ID", "Name");
            ViewBag.Stores = new SelectList(_context.Store.GetAll(), "ID", "Name");
        }
    }
}