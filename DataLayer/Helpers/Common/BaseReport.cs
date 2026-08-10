using DataLayer.Entities;
using DataLayer.ViewModels.Report;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataLayer.Base
{
    public class BaseReport
    {
        public static object AccountOrder(UnitOfWork unitOfWork,
            int? accountId = null,
            int? storeId = null,
            int[] stores = null,
            int? merchantId = null,
            string refId = null,
            int[] statusId = null,
            List<string> statusStr = null,
            string customer = null,
            string product = null,
            int? fromorderid = null,
            int? toorderid = null,
            DateTime? fromDatetime = null,
            DateTime? toDatetime = null,
            string rebate = null,
            bool? clear = null,
            Enum_Report reportType = Enum_Report.ORDER_STORE_SELL
            )
        {
            var MyQuery = unitOfWork.AccountOrder.Report(
                accountId: accountId,
                storeId: storeId,
                stores: stores,
                merchantId: merchantId,
                customer: customer,
                fromDatetime: fromDatetime,
                fromorderid: fromorderid,
                product: product,
                rebate: rebate,
                refId: refId,
                statusId: statusId,
                statusStr: statusStr,
                toDatetime: toDatetime,
                toorderid: toorderid,
                clear: clear
                );
            if (MyQuery.Any())
            {
                switch (reportType)
                {
                    case Enum_Report.ORDER_STORE_SELL:
                        {
                            var model = new ColumnDrilldownChart();
                            model.ColumnName = Resource.Enum.ORDER_STORE_SELL;
                            model.yTitle = $"{Resource.Lang.Amount } -  {Resource.Lang.Toman} ";
                            var subModel = MyQuery.GroupBy(x => x.Store).Select(s => new SubColumnDrilldownChart
                            {
                                name = s.Key != null ? s.Key.Name : Resource.Lang.Shop,
                                y = s.Sum(u => u.Price)
                            }).ToArray();
                            model.List = subModel;
                            return model;
                        }
                    case Enum_Report.ORDER_STORE_SELL_DAY:
                        {
                            var model = new ColumnBasicChart();
                            model.ColumnName = Resource.Enum.ORDER_STORE_SELL_DAY;
                            model.yTitle = $"{Resource.Lang.Amount } - {Resource.Lang.Toman}";

                            var dates = new List<DateTime>();
                            var minDate = MyQuery.Min(s => s.Datetime);
                            var maxDate = MyQuery.Max(s => s.Datetime);
                            maxDate = maxDate.AddDays(1);
                            model.Categories = new List<string>();
                            for (var dt = minDate; dt < maxDate; dt = dt.AddDays(1))
                            {
                                var date = dt.ToShortDateString();
                                model.Categories.Add(date);
                            }
                            var subModel2 = MyQuery.GroupBy(x => x.Store);
                            model.List = new List<SubColumnBasicChart>();
                            foreach (var item in subModel2)
                            {
                                var subm = new SubColumnBasicChart();
                                subm.name = item.Key != null ? item.Key.Name : Resource.Lang.Shop;
                                subm.data = new List<double>();
                                for (var dt = minDate; dt <= maxDate; dt = dt.AddDays(1))
                                {
                                    var sum = item.Where(a => a.Datetime.Date == dt.Date).Sum(s => s.Price);
                                    subm.data.Add(sum);
                                }
                                model.List.Add(subm);
                            }


                            return model;
                        }
                    case Enum_Report.ORDER_STORE_SELL_MONTH:
                        {
                            var model = new ColumnBasicChart();
                            model.ColumnName = model.ColumnName = Resource.Enum.ORDER_STORE_SELL_MONTH;
                            model.yTitle = $"{Resource.Lang.Amount } - {Resource.Lang.Toman}";
                            var dates = new List<DateTime>();
                            var minDate = MyQuery.Min(s => s.Datetime);
                            var maxDate = MyQuery.Max(s => s.Datetime);
                            maxDate = maxDate.AddDays(1);
                            model.Categories = new List<string>();
                            for (var dt = minDate; dt < maxDate; dt = dt.AddMonths(1))
                            {
                                var date = dt.ToString("yyyy/MM");
                                model.Categories.Add(date);
                            }
                            var subModel2 = MyQuery.GroupBy(x => x.Store);
                            model.List = new List<SubColumnBasicChart>();
                            foreach (var item in subModel2)
                            {
                                var subm = new SubColumnBasicChart();
                                subm.name = item.Key != null ? item.Key.Name : Resource.Lang.Shop;
                                subm.data = new List<double>();
                                for (var dt = minDate; dt <= maxDate; dt = dt.AddMonths(1))
                                {
                                    var sum = item.Where(a => a.Datetime.ToString("yyyy/MM") == dt.ToString("yyyy/MM")).Sum(s => s.Price);
                                    subm.data.Add(sum);
                                }
                                model.List.Add(subm);
                            }


                            return model;
                        }
                    case Enum_Report.ORDER_STORE_SELL_YEAR:
                        {
                            var model = new ColumnBasicChart();
                            model.yTitle = $"{Resource.Lang.Amount } - {Resource.Lang.Toman}";
                            model.ColumnName = Resource.Enum.ORDER_STORE_SELL_YEAR;
                            var dates = new List<DateTime>();
                            var minDate = MyQuery.Min(s => s.Datetime);
                            var maxDate = MyQuery.Max(s => s.Datetime);
                            maxDate = maxDate.AddDays(1);
                            model.Categories = new List<string>();
                            for (var dt = minDate; dt < maxDate; dt = dt.AddYears(1))
                            {
                                var date = dt.ToString("yyyy");
                                model.Categories.Add(date);
                            }
                            var subModel2 = MyQuery.GroupBy(x => x.Store);
                            model.List = new List<SubColumnBasicChart>();
                            foreach (var item in subModel2)
                            {
                                var subm = new SubColumnBasicChart();
                                subm.name = item.Key != null ? item.Key.Name : Resource.Lang.Shop;
                                subm.data = new List<double>();
                                for (var dt = minDate; dt <= maxDate; dt = dt.AddYears(1))
                                {
                                    var sum = item.Where(a => a.Datetime.Year == dt.Year).Sum(s => s.Price);
                                    subm.data.Add(sum);
                                }
                                model.List.Add(subm);
                            }


                            return model;
                        }
                }

            }
            return null;
        }



        public static object AccountOrderProduct(UnitOfWork unitOfWork,
              int? accountId = null,

         Enum_Code status = Enum_Code.ORDER_STATUS_NONE,
         int[] statusId = null,
         string product = null,
         DateTime? from = null,
         DateTime? to = null,
          string pageIndex = null,
            string pageSize = null,
           int? typeId = null,
           int ? optionId=null,
           Enum_Report reportType = Enum_Report.ORDER_STORE_SELL
              )
        {
            var MyQuery = unitOfWork.AccountOrderProduct.Report(
                 product: product,
                    typeId: typeId,
                    optionId: optionId,
                    statusId: statusId,
                    fromDatetime: from,
                    toDatetime: to
                );
            if (MyQuery.Any())
            {
                switch (reportType)
                {
                    case Enum_Report.ORDER_STORE_SELL:
                        {
                            var model = new ColumnDrilldownChart();
                            model.ColumnName = Resource.Enum.ORDER_STORE_SELL;
                            model.yTitle = $"{Resource.Lang.Amount } -  {Resource.Lang.Toman} ";
                            var subModel = MyQuery.GroupBy(x => x.AccountOrder.Store).Select(s => new SubColumnDrilldownChart
                            {
                                name = s.Key != null ? s.Key.Name : Resource.Lang.Shop,
                                y = s.Sum(u => u.Price)
                            }).ToArray();
                            model.List = subModel;
                            return model;
                        }
                    case Enum_Report.ORDER_STORE_SELL_DAY:
                        {
                            var model = new ColumnBasicChart();
                            model.ColumnName = Resource.Enum.ORDER_STORE_SELL_DAY;
                            model.yTitle = $"{Resource.Lang.Amount } - {Resource.Lang.Toman}";

                            var dates = new List<DateTime>();
                            var minDate = MyQuery.Min(s => s.Datetime);
                            var maxDate = MyQuery.Max(s => s.Datetime);
                            maxDate = maxDate.AddDays(1);
                            model.Categories = new List<string>();
                            for (var dt = minDate; dt < maxDate; dt = dt.AddDays(1))
                            {
                                var date = dt.ToShortDateString();
                                model.Categories.Add(date);
                            }
                            var subModel2 = MyQuery.GroupBy(x => x.AccountOrder.Store);
                            model.List = new List<SubColumnBasicChart>();
                            foreach (var item in subModel2)
                            {
                                var subm = new SubColumnBasicChart();
                                subm.name = item.Key != null ? item.Key.Name : Resource.Lang.Shop;
                                subm.data = new List<double>();
                                for (var dt = minDate; dt <= maxDate; dt = dt.AddDays(1))
                                {
                                    var sum = item.Where(a => a.Datetime.Date == dt.Date).Sum(s => s.Price);
                                    subm.data.Add(sum);
                                }
                                model.List.Add(subm);
                            }


                            return model;
                        }
                    case Enum_Report.ORDER_STORE_SELL_MONTH:
                        {
                            var model = new ColumnBasicChart();
                            model.ColumnName = model.ColumnName = Resource.Enum.ORDER_STORE_SELL_MONTH;
                            model.yTitle = $"{Resource.Lang.Amount } - {Resource.Lang.Toman}";
                            var dates = new List<DateTime>();
                            var minDate = MyQuery.Min(s => s.Datetime);
                            var maxDate = MyQuery.Max(s => s.Datetime);
                            maxDate = maxDate.AddDays(1);
                            model.Categories = new List<string>();
                            for (var dt = minDate; dt < maxDate; dt = dt.AddMonths(1))
                            {
                                var date = dt.ToString("yyyy/MM");
                                model.Categories.Add(date);
                            }
                            var subModel2 = MyQuery.GroupBy(x => x.AccountOrder.Store);
                            model.List = new List<SubColumnBasicChart>();
                            foreach (var item in subModel2)
                            {
                                var subm = new SubColumnBasicChart();
                                subm.name = item.Key != null ? item.Key.Name : Resource.Lang.Shop;
                                subm.data = new List<double>();
                                for (var dt = minDate; dt <= maxDate; dt = dt.AddMonths(1))
                                {
                                    var sum = item.Where(a => a.Datetime.ToString("yyyy/MM") == dt.ToString("yyyy/MM")).Sum(s => s.Price);
                                    subm.data.Add(sum);
                                }
                                model.List.Add(subm);
                            }


                            return model;
                        }
                    case Enum_Report.ORDER_STORE_SELL_YEAR:
                        {
                            var model = new ColumnBasicChart();
                            model.yTitle = $"{Resource.Lang.Amount } - {Resource.Lang.Toman}";
                            model.ColumnName = Resource.Enum.ORDER_STORE_SELL_YEAR;
                            var dates = new List<DateTime>();
                            var minDate = MyQuery.Min(s => s.Datetime);
                            var maxDate = MyQuery.Max(s => s.Datetime);
                            maxDate = maxDate.AddDays(1);
                            model.Categories = new List<string>();
                            for (var dt = minDate; dt < maxDate; dt = dt.AddYears(1))
                            {
                                var date = dt.ToString("yyyy");
                                model.Categories.Add(date);
                            }
                            var subModel2 = MyQuery.GroupBy(x => x.AccountOrder.Store);
                            model.List = new List<SubColumnBasicChart>();
                            foreach (var item in subModel2)
                            {
                                var subm = new SubColumnBasicChart();
                                subm.name = item.Key != null ? item.Key.Name : Resource.Lang.Shop;
                                subm.data = new List<double>();
                                for (var dt = minDate; dt <= maxDate; dt = dt.AddYears(1))
                                {
                                    var sum = item.Where(a => a.Datetime.Year == dt.Year).Sum(s => s.Price);
                                    subm.data.Add(sum);
                                }
                                model.List.Add(subm);
                            }


                            return model;
                        }
                }

            }
            return null;
        }

    }
}
