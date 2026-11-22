using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;

namespace TheTouchdownBarberShop.Localization
{
    public static class LocalizationExtensions
    {
        public const string DefaultCulture = "es-CR";

        public static readonly CultureInfo[] SupportedCultures =
        {
            new CultureInfo("es-CR"),
            new CultureInfo("en-US")
        };

        public static IMvcBuilder AddAppLocalization(this IMvcBuilder mvc)
        {
            // Registra IStringLocalizer<T>; los .resx se buscan en la carpeta "Resources"
            mvc.Services.AddLocalization(options => options.ResourcesPath = "Resources");

            // Localización en Razor
            mvc.AddViewLocalization();

            // Mensajes de DataAnnotations: todos salen de ValidationResource
            mvc.AddDataAnnotationsLocalization(options =>
            {
                options.DataAnnotationLocalizerProvider = (type, factory) =>
                    factory.Create(typeof(ValidationResource));
            });

            mvc.Services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(DefaultCulture);
                options.SupportedCultures = SupportedCultures;     // formato de fechas y números
                options.SupportedUICultures = SupportedCultures;   // selección del .resx

                // 1) cookie con la preferencia guardada, 2) idioma del navegador (primera visita)
                options.RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new CookieRequestCultureProvider(),
                    new AcceptLanguageHeaderRequestCultureProvider()
                };
            });

            return mvc;
        }

        public static IApplicationBuilder UseAppLocalization(this IApplicationBuilder app)
        {
            return app.UseRequestLocalization();
        }
    }
}