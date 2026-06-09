namespace _3dShop.Api.Models.Interfaces
{
    public interface IValidateCategoryWithId : IValidateCategory
    {
        public Guid Id { get; set; }
    }
}
