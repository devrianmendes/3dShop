using _3dShop.Api.Models.Enums;
using _3dShop.Api.Models.ValueObjects;

namespace _3dShop.Api.Models.Entities
{
    public class Order : BaseEntity
    {
        public required string OrderNumber { get; set; }
        public required OrderStatus Status { get; set; }
        public required ShippingAddress Address { get; set; }
        public required string ShippingMethod { get; set; }
        public required decimal ShippingCost { get; set; }
        public required int ShippingEstimatedDays { get; set; }
        public required decimal Subtotal { get; set; }
        public required decimal DiscountTotal { get; set; }
        public required decimal Total { get; set; }
        public string? Notes { get; set; }

        //Relations
        public required Guid CustomerId { get; set; } //FK
        public required User Customer { get; set; } //Navegação - Facilita acesso a entidade relacionada no código
        public ICollection<OrderItem>? OrderItemList { get; set; } //Navegação - Facilita acesso a entidade relacionada no código
        public Payment? Payment { get; set; } //Navegação - Facilita acesso a entidade relacionada no código
        public ICollection<OrderStatusHistory>? OrderStatusHistory { get; set; } //Navegação - Facilita acesso a entidade relacionada no código

    }
}

