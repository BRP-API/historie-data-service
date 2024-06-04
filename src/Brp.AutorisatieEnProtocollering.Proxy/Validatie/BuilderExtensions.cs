using Brp.AutorisatieEnProtocollering.Proxy.Validatie.Personen;
using Brp.AutorisatieEnProtocollering.Proxy.Validatie.Reisdocumenten;
using Brp.Shared.Infrastructure.Validatie;

namespace Brp.AutorisatieEnProtocollering.Proxy.Validatie;

public static class BuilderExtensions
{
    public static void SetupProtocollering(this WebApplicationBuilder builder)
    {
        builder.Services.AddKeyedTransient<IRequestBodyValidator, PersonenRequestBodyValidatieService>("personen");
        builder.Services.AddKeyedTransient<IRequestBodyValidator, ReisdocumentenRequestBodyValidatieService>("reisdocumenten");
    }
}
