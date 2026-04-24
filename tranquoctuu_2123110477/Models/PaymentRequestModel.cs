namespace ToyUniverse.API.Models
{
    public class PaymentRequestModel
    {
        public int OrderId { get; set; }
        public double Amount { get; set; }
        public string OrderDescription { get; set; }
    }
}