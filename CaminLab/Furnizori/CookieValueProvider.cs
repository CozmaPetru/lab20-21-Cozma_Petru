using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CaminLab.Furnizori
{
    // Furnizorul de valori: răspunde la întrebarea „ai o valoare cu acest nume?”.
    // Citește cookie-urile cererii.
    public class CookieValueProvider : IValueProvider
    {
        private IRequestCookieCollection cookies;

        public CookieValueProvider(IRequestCookieCollection cookies)
        {
            this.cookies = cookies;
        }

        // Legătorul întreabă întâi dacă există ceva cu acest nume.
        public bool ContainsPrefix(string prefix)
        {
            return cookies.ContainsKey(prefix);
        }

        // Apoi cere valoarea.
        public ValueProviderResult GetValue(string key)
        {
            string valoare;
            if (cookies.TryGetValue(key, out valoare))
            {
                return new ValueProviderResult(valoare);
            }
            return ValueProviderResult.None;
        }
    }
}
