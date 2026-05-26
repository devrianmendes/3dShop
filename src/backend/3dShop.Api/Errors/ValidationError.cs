namespace _3dShop.Api.Errors
{
    public class ValidationError
    {
        public required string Field { get; set; }
        public required string Message { get; set; }
    }
}
