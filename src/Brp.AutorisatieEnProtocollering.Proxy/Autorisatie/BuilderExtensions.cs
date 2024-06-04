using Brp.AutorisatieEnProtocollering.Proxy.Autorisatie.Personen;
using Brp.AutorisatieEnProtocollering.Proxy.Autorisatie.Reisdocumenten;
using Brp.Shared.Infrastructure.Autorisatie;

namespace Brp.AutorisatieEnProtocollering.Proxy.Autorisatie;

public static class BuilderExtensions
{
    public static void SetupAuthorisation(this WebApplicationBuilder builder)
    {
        builder.Services.AddKeyedTransient<IAuthorisation, PersonenAuthorisatieService>("personen");
        builder.Services.AddKeyedTransient<IAuthorisation, ReisdocumentenAutorisatieService>("reisdocumenten");
    }
}
