namespace Media.Models
{
    public class QuickCheckoutViewModel
    {
        public int ProductId { get; set; }
        public int QuantityId { get; set; }
        public string ProductTitle { get; set; }
        public string ImageUrl { get; set; }
        public double Price { get; set; }
        public string Error { get; set; }
    }
}