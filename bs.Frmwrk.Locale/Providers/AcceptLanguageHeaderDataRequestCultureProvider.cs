using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

namespace bs.Frmwrk.Locale.Providers
{
    public class AcceptLanguageHeaderDataRequestCultureProvider : RequestCultureProvider
    {
        /// <summary>
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">httpContext</exception>
        /// <inheritdoc />
        public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));

            var acceptedLanguageRequest = httpContext.Request.GetTypedHeaders().AcceptLanguage;
            var mainPriorityLanguage = acceptedLanguageRequest.OrderByDescending(x => x.Quality ?? 1).FirstOrDefault()?.Value ?? "it-IT";

            var providerResultCulture = new ProviderCultureResult(mainPriorityLanguage, mainPriorityLanguage);

            return Task.FromResult(providerResultCulture);
        }
    }
}