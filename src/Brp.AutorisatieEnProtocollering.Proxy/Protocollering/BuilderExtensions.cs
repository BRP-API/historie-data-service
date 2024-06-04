using Brp.AutorisatieEnProtocollering.Proxy.Protocollering.Personen;
using Brp.AutorisatieEnProtocollering.Proxy.Protocollering.Reisdocumenten;
using Brp.Shared.Infrastructure.Protocollering;

namespace Brp.AutorisatieEnProtocollering.Proxy.Protocollering;

public static class BuilderExtensions
{
    public static void SetupRequestValidation(this WebApplicationBuilder builder)
    {
        builder.Services.AddKeyedTransient<IProtocollering, PersonenProtocolleringService>("personen");
        builder.Services.AddKeyedTransient<IProtocollering, ReisdocumentenProtocolleringService>("reisdocumenten");
    }
}
