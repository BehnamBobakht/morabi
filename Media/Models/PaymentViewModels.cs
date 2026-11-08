namespace Media.Models
{
    public class MediaPaymentResultViewModel
    {
        public bool Success { get; set; }
        public string ReferenceNumber { get; set; }
        public string Message { get; set; }
        public int? ProductId { get; set; }
        public string ProductTitle { get; set; }
    }
}