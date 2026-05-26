namespace _3dShop.Api.Exceptions
{
    public class BusinessException : Exception
    {
        public int StatusCode { get; set; }

        public BusinessException(string message) : base(message)
        {
            StatusCode = StatusCodes.Status417ExpectationFailed;
        }
    }
}
