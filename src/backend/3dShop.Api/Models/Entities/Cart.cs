namespace _3dShop.Api.Models.Entities
{
    public class Cart : BaseEntity
    {
        //References
        public required User Customer { get; set; }
        public required Guid CustomerId { get; set; }

        public ICollection<CartItem>? CartItemList { get; set; }

    }
}
