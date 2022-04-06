using Domain.Common;

namespace Domain.Entities
{
    public class CourseSubscription : AuditableEntity
    {
        public int Id { get; set; }

        public int StudentId { get; set; }
        public virtual Student Student { get; set; }

        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

        public double Price { get; set; }

        public int? Rating { get; set; }
        public string Comment { get; set; }

        public string BuyTx { get; set; } //100% Student to Platform

        public string CashoutPaymentTx { get; set; } //88% Platform to Dev Marketing
        public double? AmountCashout { get; set; }

        public string SendToBurnTx { get; set; }  //4% Platform to Burn
        public double? PriceBurn { get; set; }

        public string SendToDevTx { get; set; } //4% Platform to Dev Marketing
        public double? PriceDev { get; set; }
    }
}
