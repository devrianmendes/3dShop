using _3dShop.Api.Models.Enums;

namespace _3dShop.Api.Models.Entities
{
    public class OrderStatusHistory : BaseEntity
    {

        public OrderStatus? FromStatus { get; set; }
        public required OrderStatus ToStatus { get; set; }
        public string? Notes { get; set; }

        //Relations
        public required User User { get; set; }
        public required Guid ChangedByUserId { get; set; }
        public required Order Order { get; set; }
        public required Guid OrderId { get; set; }
    }
}
