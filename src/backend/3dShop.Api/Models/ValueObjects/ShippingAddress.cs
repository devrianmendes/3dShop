namespace _3dShop.Api.Models.ValueObjects
{
    public class ShippingAddress
    {
        public required string ShippingStreet { get; set; }
        public required string ShippingNumber { get; set; }
        public string? ShippingComplement { get; set; }
        public required string ShippingNeighborhood { get; set; }
        public required string ShippingCity { get; set; }
        public required string ShippingState { get; set; }
        public required string ShippingZipCode { get; set; }
    }
}
