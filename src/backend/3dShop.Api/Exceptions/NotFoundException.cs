namespace _3dShop.Api.Exceptions
{
    public class NotFoundException : Exception
    {
        public int StatusCode { get; set; }

        public NotFoundException(string message) : base(message)
        {
            StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}
