using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer.Entities;
using DataLayer.Base;
using Cms.Areas.Store.Security;
//using OfficeOpenXml;


namespace Cms.Areas.Store.Controllers
{
    public class ProductsController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        #region Utilites

        public List<ProductType> GetProductTypes()
        {
            var currentAccount = BaseWebsite.CurrentAccount;
            return _context.ProductType.GetStoreProductTypes(currentAccount.StoreId);
        }
        public JsonResult LoadProductTypes()
        {
            var result = GetProductTypes().Select(productType => new SelectListItem
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

        public List<ProductCategory> GetProductCategories()
        {
            var currentAccount = BaseWebsite.CurrentAccount;
            return _context.ProductCategory.GetStoreProductCategories(currentAccount.StoreId);
        }
        public JsonResult LoadProductCategories(int productTypeId)
        {
            var pcs = GetProductCategories().Where(pc => pc.TypeId == productTypeId);
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

        public List<ProductSubCategory> GetProductSubCategories()
        {
            var currentAccount = BaseWebsite.CurrentAccount;
            return _context.ProductSubCategory.GetStoreProductSubCategories(currentAccount.StoreId);
        }
        public JsonResult LoadProductSubCategories(int productCategoryId)
        {
            var pscS = GetProductSubCategories().Where(psc => psc.CategoryId == productCategoryId);
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

        public List<StoreProduct> GetStoreProducts()
        {
            var currentAccount = BaseWebsite.CurrentAccount;
            return _context.StoreProduct.Search(storeId: currentAccount.StoreId, pageSize: int.MaxValue);
        }
        public JsonResult LoadProducts(int productSubCategoryId)
        {
            var products = GetStoreProducts().Where(x => x.Product.ProductSubCategoryId == productSubCategoryId);
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

        public List<StoreProductQuantity> GetStoreProductQuantities()
        {
            var currentAccount = BaseWebsite.CurrentAccount;
            return _context.StoreProductQuantity.GetForStore(currentAccount.StoreId);
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

        public JsonResult LoadProductQuantities(int productId)
        {
            var quantities = GetStoreProductQuantities().Where(x => x.StoreProductId == productId).ToList();
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

        #endregion

        // GET: Store/Products 3222
        [AuthorizeFilter(Enum_StorePanel = DataLayer.Enumarables.Enum_StorePanel.StoreState)]

        public ActionResult Index(int index = 1,int pageSize = 10,string name = null)
        {

            var currentAccount = DataLayer.Base.BaseWebsite.CurrentAccount;
            var storeProduct = _context.StoreProduct.Search(statusId: null, index: index, pageSize: pageSize, name: name, accountId: currentAccount.Id);
            // ViewBag.TotalCount = _context.StoreProduct.Search(statusId: null, index: null, pageSize: null, name: name, accountId: currentAccount.Id).Count();
            ViewBag.TotalCount = _context.StoreProduct.SearchCount(statusId : null, name: name, accountId: currentAccount.Id);
            ViewBag.pageSize = pageSize.ToString();
            return View(storeProduct.ToView());
        }

        [HttpPost]
        public ActionResult UpdatePrice(bool percent, bool increase, double changeAmount, int groupPricingType, int? productTypeId = null, int? productCategoryId = null, int? productSubCategoryId = null, int? productId = null, int? quantityId = null)
        {
            if (changeAmount <= 0 || groupPricingType < 0 || groupPricingType > 5)
                return RedirectToAction("Index");

            double GetFinalPrice(StoreProductQuantity spq)
            {
                var changePrice = percent ? spq.Price * changeAmount / 100 : changeAmount;
                var price = increase ? spq.Price + changePrice : spq.Price - changePrice;
                return price > 0 ? price : 0;
            }

            var storeProductQuantities = GetStoreProductQuantities();
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
                var products = targetList.Select(s => s.ProductQuantity.Product).Distinct().ToList();
                BaseStore.UpdateProductQuantity(products:products,unitOfWork:_context);
            }

            return RedirectToAction("Index");
        }

        public void DownloadExcel()
        {
            var dataResult = new List<StoreProductQuantity>();
            var currentAccount = BaseWebsite.CurrentAccount;
            var storeProducts = _context.StoreProduct.Search(statusId: null, index: 1, pageSize: int.MaxValue, name: null, accountId: currentAccount.Id);
            foreach (var storeProductQuantities in storeProducts
                .Select(storeProduct => storeProduct.StoreProductQuantity)
                .Where(storeProductQuantities => storeProductQuantities.Any()))
                dataResult.AddRange(storeProductQuantities);

            if (!dataResult.Any()) return;

            //var excelPackage = new ExcelPackage();
            //var sheet = excelPackage.Workbook.Worksheets.Add(Resource.Lang.StoreProducts);

            //var row = 1;

            //sheet.Cells[$"A{row}"].Value = Resource.Lang.ID;
            //sheet.Cells[$"B{row}"].Value = Resource.Lang.ProductName;
            //sheet.Cells[$"C{row}"].Value = Resource.Lang.Color;
            //sheet.Cells[$"D{row}"].Value = Resource.Lang.Size;
            //sheet.Cells[$"E{row}"].Value = Resource.Lang.ProductOption;
            //sheet.Cells[$"F{row}"].Value = $"{Resource.Lang.Price} ({Resource.Lang.Toman})";
            //sheet.Cells[$"G{row}"].Value = Resource.Lang.Count;

            //foreach (var spq in dataResult)
            //{
            //    row++;
            //    sheet.Cells[$"A{row}"].Value = spq.ID;
            //    sheet.Cells[$"B{row}"].Value = spq.ProductQuantity.Product.GetName();
            //    sheet.Cells[$"C{row}"].Value = spq.ProductQuantity.Color?.GetName();
            //    sheet.Cells[$"D{row}"].Value = spq.ProductQuantity.Size?.GetName();
            //    sheet.Cells[$"E{row}"].Value = spq.ProductQuantity.OptionItemId.HasValue
            //        ? $"{spq.ProductQuantity.ProductOptionItem.ProductOption.Name} : {spq.ProductQuantity.ProductOptionItem.Value}"
            //        : null;
            //    sheet.Cells[$"F{row}"].Value = spq.Price;
            //    sheet.Cells[$"G{row}"].Value = null;
            //}

            //sheet.Cells["A:AZ"].AutoFitColumns();
            //Response.Clear();
            //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //Response.AppendHeader("Content-Disposition", "attachment; filename=StoreProducts.xlsx");
            //Response.BinaryWrite(excelPackage.GetAsByteArray());
            //Response.End();
        }

        [HttpPost]
        public ActionResult UploadExcel(HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength <= 0) return RedirectToAction("Index");

            var allowedQuantities = new List<StoreProductQuantity>();
            var currentAccount = BaseWebsite.CurrentAccount;
            var storeProducts = _context.StoreProduct.Search(statusId: null, index: 1, pageSize: int.MaxValue, name: null, accountId: currentAccount.Id);
            foreach (var storeProductQuantities in storeProducts
                .Select(storeProduct => storeProduct.StoreProductQuantity)
                .Where(storeProductQuantities => storeProductQuantities.Any()))
                allowedQuantities.AddRange(storeProductQuantities);

            if (!allowedQuantities.Any()) return RedirectToAction("Index");

            //var fileContentType = file.ContentType;
            //var fileBytes = new byte[file.ContentLength];
            //var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
            var products = new List<Product>();
            //using (var package = new ExcelPackage(file.InputStream))
            //{
            //    var currentSheet = package.Workbook.Worksheets;
            //    var workSheet = currentSheet.First();
            //    var noOfCol = workSheet.Dimension.End.Column;
            //    var noOfRow = workSheet.Dimension.End.Row;
            //    for (var rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
            //    {
            //        var spqId = (workSheet.Cells[rowIterator, 1].Value ?? "").ToString();
            //        var trueId = int.TryParse(spqId, out var id);
            //        if (!trueId) continue;
            //        if (allowedQuantities.All(storeProductQuantity => storeProductQuantity.ID != id)) continue;

            //        var spq = _context.StoreProductQuantity.GetById(id);

            //        var spqPrice = (workSheet.Cells[rowIterator, 6].Value ?? 0).ToString();
            //        if (string.IsNullOrWhiteSpace(spqPrice)) spq.Price = 0;
            //        else
            //        {
            //            var truePrice = double.TryParse(spqPrice, out var price);
            //            if (!truePrice) continue;
            //            if (price < 0) continue;
            //            spq.Price = price;
            //        }

            //        var spqCount = (workSheet.Cells[rowIterator, 7].Value ?? "").ToString();
            //        if (!string.IsNullOrWhiteSpace(spqCount))
            //        {
            //            var trueCount = int.TryParse(spqCount, out var count);
            //            if (trueCount && count > 0)
            //                spq.Count += count;
            //        }

            //        _context.StoreProductQuantity.Update(spq);
            //        if(!products.Any(x=>x.ID==spq.ProductQuantity.ProductId))
            //        {
            //            products.Add(spq.ProductQuantity.Product);
            //        }
            //    }

            //    _context.Save();
            //    BaseStore.UpdateProductQuantity(products: products, unitOfWork: _context);
            //}
            return RedirectToAction("Index");
        }

        // GET: Store/Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!BaseStore.StoreProductId_IsValid(id.Value, _context))
          return View("Invalid");

            StoreProduct storeProduct = _context.StoreProduct.FirstOrDefault(s => s.ID == id);
            if (storeProduct == null)
            {
                return HttpNotFound();
            }
            return View(storeProduct);
        }

        // GET: Store/Products/Create
        [AuthorizeFilter(Enum_StorePanel = DataLayer.Enumarables.Enum_StorePanel.StoreState)]

        public ActionResult Create()
        {

            return View();
        }

        // POST: Store/Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeFilter(Enum_StorePanel = DataLayer.Enumarables.Enum_StorePanel.StoreState)]
        public ActionResult Create(List<int> products)
        {

            var currentAccount = DataLayer.Base.BaseWebsite.CurrentAccount;

            var list = new List<StoreProduct>();
            foreach(var productId in products)
            {
                var item = new StoreProduct()
                {
                    ProductId = productId,
                    StoreId = currentAccount.StoreId,
                    CreateDate = DateTime.Now,
                    StatusId = _context.Code.GetIdByLabel(DataLayer.Enumarables.Enum_Code.STOREPRODUCT_REQUEST_NOT_CHECKED)
                };
                list.Add(item);
            }
            _context.StoreProduct.MultipleInsert(list);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!BaseStore.StoreProductId_IsValid(id.Value,_context))
          return View("Invalid");
            StoreProduct storeProduct = _context.StoreProduct.FirstOrDefault(s => s.ID == id);
            if (storeProduct == null)
            {
                return HttpNotFound();
            }
            return View(storeProduct);
        }

        // POST: Store/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!BaseStore.StoreProductId_IsValid(id, _context))
          return View("Invalid");
            _context.StoreProduct.Delete(id);
            _context.Save();
            return RedirectToAction("Index");
        }

        public ActionResult ProductQuantity(int id)
        {
            if (!BaseStore.StoreProductId_IsValid(id, _context))
          return View("Invalid");
            var model = _context.StoreProduct.FirstOrDefault(s => s.ID == id);
            return View(model);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult ProductQuantity(StoreProductQuantity storeProductQuantity)
        {
            if (!BaseStore.StoreProductId_IsValid(storeProductQuantity.StoreProductId, _context))
          return View("Invalid");
            _context.StoreProductQuantity.AddOrEdit(storeProductQuantity, _context);
            //var model = _context.StoreProduct.FirstOrDefault(s => s.ID == storeProductQuantity.StoreProduct.ID);
            return RedirectToAction("ProductQuantity",storeProductQuantity.StoreProductId);
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
