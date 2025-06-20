namespace MovieTicketsBookings.Models
{
    public class PaymentInformation
    {
        public decimal Amount { get; set; }
        public string OrderType { get; set; } = string.Empty;
        public string OrderDescription { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string OrderId { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
    }
}
