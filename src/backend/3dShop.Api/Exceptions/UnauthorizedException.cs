namespace _3dShop.Api.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public int StatusCode { get; set; }

        public UnauthorizedException(string message) : base(message)
        {
            StatusCode = StatusCodes.Status401Unauthorized;
        }
    }
}
