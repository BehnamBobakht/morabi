using DataLayer.Base;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewPaymentExame
    {
        public int Id { get; set; }

        public ViewAccount Account { get; set; }
        public double Price { get; set; }

        public DateTime Datetime { get; set; }


        public List<ViewExameOrder> ExameOrder { get; set; }

        public ViewPaymentExame() { }

        public ViewPaymentExame(PaymentExame order)
        {
            this.Id = order.ID;
            this.Account = new ViewAccount(order.Account);

            this.Price = order.Amount;
            this.Datetime = order.Datetime;
            this.ExameOrder = new List<ViewExameOrder>();
            foreach (var item in order.PaymentExameCourseId)
            {
                ExameOrder.Add(new ViewExameOrder(item.ExameCourse));
            }
            

        }

        public ViewPaymentExame(PaymentExame order, int index, string MaxZero)
        {
            this.Id = order.ID;
            this.Id = order.ID;
            this.Account = new ViewAccount(order.Account);

            this.Price = order.Amount;
            this.Datetime = order.Datetime;
            this.ExameOrder = new List<ViewExameOrder>();
            foreach (var item in order.PaymentExameCourseId)
            {
                ExameOrder.Add(new ViewExameOrder(item.ExameCourse));
            }

        }
    }
}
