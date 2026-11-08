using System;

namespace DataLayer.ViewModels
{
    // Purpose-built, read-only cards for the public home page.
    // Keep these small so a homepage request never materializes full Product/Post graphs.
    public class HomeProductCardVm
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }
        public string PictureUrl { get; set; }
        public string ProductUrl { get; set; }
        public double CurrentPrice { get; set; }
        public double BasePrice { get; set; }
        public int DiscountPercent { get; set; }
        public bool HasDiscount { get; set; }
        public bool RegistrationExpired { get; set; }
    }

    public class HomePostCardVm
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }
        public string PostUrl { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
