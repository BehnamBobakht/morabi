using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ExpressMapper;
using ExpressMapper.Extensions;
using Binbin.Linq;
using System.Linq.Expressions;

namespace DataLayer.Repositories
{
    public class Entity_Delivery : BaseRepository<Delivery>
    {
        DatabaseEntities _context;
        public Entity_Delivery(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public DeliveryVm GetByIdVm(int id)
        {
            return GetById(id).Map<Delivery, DeliveryVm>();
        }
        public List<DeliveryVm> GetForStore(int? storeId, double price, int cityId, int stateId)
        {
            var result = new List<DeliveryVm>();
            var deliverys = Search(pageSize: 1000, storeId: storeId, active: true);
            var deliveryTypes = deliverys.Select(s => s.DeliveryTypeId).Distinct();
            foreach (var typeId in deliveryTypes)
            {
                var dis = deliverys.Where(s => s.DeliveryTypeId == typeId && s.CityId == cityId && s.StateId == stateId);
                if (!dis.Any())
                {
                    dis = deliverys.Where(s => s.DeliveryTypeId == typeId && s.CityId == (int)StateCity.AllCityId && s.StateId == stateId);
                }
                if (!dis.Any())
                {
                    dis = deliverys.Where(s => s.DeliveryTypeId == typeId && s.CityId == (int)StateCity.AllCityId && s.StateId == (int)StateCity.AllStateId);
                }
                if (dis.Any())
                {
                    foreach (var deliveryItem in dis)
                    {
                        var paymentPrice = GetDeliveryPrice(deliveryItem: deliveryItem, cartPrice: price);
                        deliveryItem.PayementPrice = paymentPrice;
                        var PayementPriceText = "";

                        switch (deliveryItem.DeliveryType.Name)
                        {

                            case "پیک":
                                {
                                    deliveryItem.DeliveryType.Name = Resource.Lang.Peyk;
                                }
                                break;
                            case "تی پاکس":
                                {
                                    deliveryItem.DeliveryType.Name = Resource.Lang.Tipax;
                                }
                                break;
                            case "باربری":
                                {
                                    deliveryItem.DeliveryType.Name = Resource.Lang.Barbari;
                                }
                                break;
                            case "پست":
                                {
                                    deliveryItem.DeliveryType.Name = Resource.Lang.PostDelivery;
                                }
                                break;
                        }
                        if (deliveryItem.BasePrice == null)
                        {
                            PayementPriceText = Resource.Lang.AtTheCustomerExpense;
                        }
                        else
                        {
                            PayementPriceText = deliveryItem.PayementPrice > 0 ? deliveryItem.PayementPrice.GetCurrencyFormat() : Resource.Lang.Free;
                        }
                        if (deliveryItem.MaxDays > 0)
                        {
                            deliveryItem.Title = string.Format(Resource.Pattern.DeliveryTitle, deliveryItem.DeliveryType.Name, PayementPriceText, deliveryItem.MaxDays);
                        }
                        else
                        {
                            deliveryItem.Title = string.Format(Resource.Pattern.DeliveryTitleImmediate, deliveryItem.DeliveryType.Name, PayementPriceText);
                        }
                        if (deliveryItem.MinPriceForFree != null && deliveryItem.MinPriceForFree >0)
                        {
                            deliveryItem.Title += " - " + string.Format(Resource.Pattern.DeliveryMinPriceFree, deliveryItem.MinPriceForFree.Value.GetCurrencyFormat());
                        }
                        if (deliveryItem.CashOnDeliveryMaxPrice != null && deliveryItem.CashOnDeliveryMaxPrice > 0)
                        {
                            if (deliveryItem.CashOnDeliveryMaxPrice.Value > price)
                            {
                                deliveryItem.CashOnDelivery = true;
                                deliveryItem.Title += " - " + Resource.Lang.CashOnDelivery;
                                result.Add(deliveryItem);
                            }
                        }
                        else
                        {
                            result.Add(deliveryItem);
                        }
                    }
                }

            }
            return result;
        }
        public double GetDeliveryPrice(int deliveryId, int cartPrice)
        {
            var item = GetById(deliveryId).Map<Delivery, DeliveryVm>();
            return GetDeliveryPrice(item, cartPrice);
        }
        public double GetDeliveryPrice(DeliveryVm deliveryItem, double cartPrice)
        {
            if (deliveryItem.MinPriceForFree > 0 && deliveryItem.MinPriceForFree <= cartPrice)
            {
                return 0;
            }
            else
            {
                return deliveryItem.BasePrice.HasValue ? deliveryItem.BasePrice.Value : 0;
            }
        }

        public List<DeliveryVm> Search(int index = 1, int pageSize = 10, int? storeId = null, bool? active = null)
        {
            List<DeliveryVm> results = new List<DeliveryVm>();
            int skipValue = pageSize * (index - 1);
            int pageValue = pageSize;

            var MyQuery = GetSearchQuery(
                storeId: storeId,
                active: active

                );
            results = _context.Delivery.Where(MyQuery)
                    .OrderBy(p => p.DeliveryTypeId).ThenByDescending(s => s.BasePrice)
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList().Map<List<Delivery>, List<DeliveryVm>>();
            return results;
        }

        public int SearchCount(int? storeId = null, bool? active = null)
        {
            var MyQuery = GetSearchQuery(
                storeId: storeId,
                active: active
                );
            return _context.Delivery.Count(MyQuery);
        }

        private Expression<Func<Delivery, bool>> GetSearchQuery(
            int? storeId = null, bool? active = null)
        {
            var MyQuery = PredicateBuilder.True<Delivery>();
            MyQuery = MyQuery.And(p => p.Deleted == false);
            MyQuery = MyQuery.And(p => p.StoreId == storeId);
            if (active != null)
                MyQuery = MyQuery.And(p => p.Active == active);
            return MyQuery;
        }

        public ResultVm<DeliveryVm> Insert(DeliveryVm delivery)
        {
            var result = new ResultVm<DeliveryVm>();

            var model = delivery.Map<DeliveryVm, Delivery>();
            var exist = FirstOrDefault(s =>
              s.DeliveryTypeId == delivery.DeliveryTypeId
              && s.StoreId == delivery.StoreId
              && s.CityId == delivery.CityId
              && s.StateId == delivery.StoreId
              && s.Deleted == false
            );
            if (exist != null)
            {
                result.Status = ResultStatus.Error;
                result.Message = Resource.Notify.TheInformationEnteredIsDuplicate;
                return result;
            }
            if (delivery.IsUnknownPrice)
            {
                model.BasePrice = null;
            }
            else
            {
                model.BasePrice = model.BasePrice == null ? 0 : model.BasePrice;
            }
            Insert(model);
            Save();
            result.Status = ResultStatus.Success;
            return result;
        }
        public void Delete(int id)
        {
            var model = FirstOrDefault(s => s.ID == id);
            if (model != null)
            {
                model.Deleted = true;
                Update(model);
                Save();
            }
        }
        public void Active(int id)
        {
            var model = FirstOrDefault(s => s.ID == id);
            if (model != null)
            {
                if (model.Active == true)
                    model.Active = false;
                else
                    model.Active = true;

                Update(model);
                Save();
            }
        }

    }
}
