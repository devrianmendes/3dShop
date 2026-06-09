namespace _3dShop.Api.Helpers
{
    public class StringNormalizer
    {
        public string Normalize(string a)
        {
            return a.ToLower().Trim();
        }
    }
}
