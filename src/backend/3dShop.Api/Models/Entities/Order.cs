using _3dShop.Api.Models.Enums;

namespace _3dShop.Api.Models.Entities
{
    public class Order : BaseEntity
    {
        public required string OrderNumber { get; set; }
        public required OrderStatus Status { get; set; }
        public required string ShippingStreet { get; set; }
        public required string ShippingNumber { get; set; }
        public string? ShippingComplement { get; set; }
        public required string ShippingNeighborhood { get; set; }
        public required string ShippingCity { get; set; }
        public required string ShippingState { get; set; }
        public required string ShippingZipCode { get; set; }
        public required string ShippingMethod { get; set; }
        public required decimal ShippingCost { get; set; }
        public required int ShippingEstimatedDays { get; set; }
        public required decimal Subtotal { get; set; }
        public required decimal DiscountTotal { get; set; }
        public required decimal Total { get; set; }
        public string? Notes { get; set; }

        //Relations
        public required User Customer { get; set; }
        public required Guid CustomerId { get; set; }
        public ICollection<OrderItem>? OrderItemList { get; set; }
        public Payment? Payment { get; set; }
        public ICollection<OrderStatusHistory>? OrderStatusHistory { get; set; }

    }
}

