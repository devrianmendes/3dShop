using _3dShop.Api.Models.Enums;

namespace _3dShop.Api.Models.Entities
{
    public class Payment : BaseEntity
    {
        public required PaymentMethod Method { get; set; }
        public required PaymentStatus Status { get; set; }
        public string? GatewayPaymentId { get; set; }
        public string? GatewayPreferenceId { get; set; }
        public required decimal Amount { get; set; }
        public DateTime? PaidAt { get; set; }

        //Relations
        public required Guid OrderId { get; set; }//FK
        public required Order Order { get; set; }//Navegação - Facilita acesso a entidade relacionada no código
    }
}
