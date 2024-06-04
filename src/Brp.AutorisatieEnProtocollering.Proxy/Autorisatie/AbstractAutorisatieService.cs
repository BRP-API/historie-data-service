using Brp.AutorisatieEnProtocollering.Proxy.Data;
using Brp.Shared.Infrastructure.Autorisatie;
using Microsoft.FeatureManagement;

namespace Brp.AutorisatieEnProtocollering.Proxy.Autorisatie;

public abstract class AbstractAutorisatieService : IAuthorisation
{
    private readonly IServiceProvider _serviceProvider;

    protected AbstractAutorisatieService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public virtual AuthorisationResult Authorize(int afnemerCode, int? gemeenteCode, string requestBody) => throw new NotImplementedException();

    protected Data.Autorisatie? GetActueleAutorisatieFor(int afnemerCode)
    {
        var featureManager = _serviceProvider.GetRequiredService<IFeatureManager>();

        var gebruikMeestRecenteAutorisatie = featureManager.IsEnabledAsync("gebruikMeestRecenteAutorisatie").Result;

        using var scope = _serviceProvider.CreateScope();

        var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        return gebruikMeestRecenteAutorisatie
            ? appDbContext.Autorisaties
                          .Where(a => a.AfnemerCode == afnemerCode)
                          .OrderByDescending(a => a.TabelRegelStartDatum)
                          .FirstOrDefault()
            : appDbContext.Autorisaties
                          .FirstOrDefault(a => a.AfnemerCode == afnemerCode &&
                                               a.TabelRegelStartDatum <= Vandaag() &&
                                               (a.TabelRegelEindDatum == null || a.TabelRegelEindDatum > Vandaag()));
    }
    private static long Vandaag()
    {
        return int.Parse(DateTime.Today.ToString("yyyyMMdd"));
    }

    protected static AuthorisationResult NotAuthorized(string? title = null, string? detail = null, string? code = null, string? reason = null)
    {
        return new AuthorisationResult(
            false,
            new List<AuthorisationFailure>
            {
                new()
                {
                    Title = title,
                    Detail = detail,
                    Code = code,
                    Reason = reason
                }
            }
        );
    }

    protected static AuthorisationResult Authorized()
    {
        return new AuthorisationResult(
            true,
            new List<AuthorisationFailure>()
        );
    }
}
