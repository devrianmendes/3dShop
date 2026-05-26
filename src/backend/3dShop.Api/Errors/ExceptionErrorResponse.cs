namespace _3dShop.Api.Errors
{
    public class ExceptionErrorResponse
    {
        public required int StatusCode { get; set; }
        public required string Message { get; set; }
    }
}
