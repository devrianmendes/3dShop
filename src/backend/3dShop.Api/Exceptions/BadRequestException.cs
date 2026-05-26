namespace _3dShop.Api.Exceptions
{
    public class BadRequestException : Exception
    {
        public int StatusCode { get; set; }

        public BadRequestException(string message) : base(message)
        {
            StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}
