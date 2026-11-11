using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CaminLab.Furnizori
{
    // Fabrica: la fiecare cerere adaugă furnizorul în lista furnizorilor.
    // Se înregistrează în Program.cs.
    public class CookieValueProviderFactory : IValueProviderFactory
    {
        public Task CreateValueProviderAsync(ValueProviderFactoryContext context)
        {
            IRequestCookieCollection cookies = context.ActionContext.HttpContext.Request.Cookies;
            context.ValueProviders.Add(new CookieValueProvider(cookies));
            return Task.CompletedTask;
        }
    }
}
