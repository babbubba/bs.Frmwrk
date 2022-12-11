using bs.Frmwrk.Core.Services.Locale;
using Microsoft.Extensions.Localization;

namespace bs.Frmwrk.Locale.Services
{
    public class TranslateService : ITranslateService
    {
        private readonly IStringLocalizer<Locale> stringLocalizer;

        public TranslateService(IStringLocalizer<Locale> stringLocalizer)
        {
            this.stringLocalizer = stringLocalizer;
        }

        public string Translate(string text)
        {
            return stringLocalizer[text].Value;
        }

        public string Translate(string text, params object[] objs)
        {
            return stringLocalizer[text, objs].Value;
        }
    }
}