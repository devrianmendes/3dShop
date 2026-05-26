namespace _3dShop.Api.Errors
{
    public class ValidationErrorResponse : ExceptionErrorResponse
    {
        public IEnumerable<ValidationError> Errors { get; set; } = [];
    }
}
