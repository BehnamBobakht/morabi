using DataLayer.Entities;
using DataLayer.ViewModels;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Api.Model
{
    public class ApiShopResellerCollection : ApiResponse
    {
        public static ApiResult Post(UnitOfWork _context, ShopResellerCollection collection)
        {
            if (string.IsNullOrEmpty(collection.Name))
                return CreateErrorResult(Enum_Message.REQUIRED_SHOPRESELLER_COLLECTION_NAME);
            if (collection.PictureId == null)
                return CreateErrorResult(Enum_Message.REQUIRED_SHOPRESELLER_COLLECTION_PICTURE_ID);
            if (collection.ShopResellerCollectionProduct.Count == 0)
                return CreateErrorResult(Enum_Message.REQUIRED_SHOPRESELLER_COLLECTION_PRODUCT);

            List<ShopResellerCollectionProduct> listProduct = collection.ShopResellerCollectionProduct.ToList();
            collection.ShopResellerCollectionProduct.Clear();

            ShopResellerCollection entity = new ShopResellerCollection() {
                ResellerId = collection.ResellerId,
                Name = collection.Name,
                Description = collection.Description,
                ShowNumber = 0,
                PictureId = collection.PictureId,
                Picture2Id = collection.Picture2Id,
                Picture3Id = collection.Picture3Id
            };
            _context.ShopResellerCollection.Insert(entity);
            _context.Save();

            foreach (ShopResellerCollectionProduct product in listProduct)
            {
                product.CollectionId = entity.ID;
                _context.ShopResellerCollectionProduct.Insert(product);
            }
            _context.Save();

            _context = new UnitOfWork();
            _context.ShopResellerStory.InsertNewStory(Enum_StoryType.COLLECTION, collection.ResellerId, entity.ID, collection.PictureId, "کالکشن جدید! " + collection.Name);

            List<ShopResellerCollection> list = _context.ShopResellerCollection.Search(resellerId: collection.ResellerId);
            return CreateSuccessResult(list.ToApi());
        }

        public static ApiResult Get(
            UnitOfWork _context,
            int? index = null,
            int? notId = null,
            int? pageSize = null,
            string name = null,
            int? resellerId = null)
        {
            pageSize = pageSize == null ? 10 : pageSize;
            index = index == null ? 1 : index;
            List<ShopResellerCollection> list = _context.ShopResellerCollection.Search(
                index: index.Value,
                notId: notId,
                name: name,
                pageSize: pageSize.Value,
                resellerId: resellerId);
            return ApiResponse.CreateSuccessResult(list.ToApi());
        }
    }
}
