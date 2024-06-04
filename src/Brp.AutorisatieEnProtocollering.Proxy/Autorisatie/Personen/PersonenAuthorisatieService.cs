using Brp.AutorisatieEnProtocollering.Proxy.Helpers;
using Brp.Shared.Infrastructure.Autorisatie;
using Newtonsoft.Json.Linq;
using Serilog;
using System.Text.RegularExpressions;

namespace Brp.AutorisatieEnProtocollering.Proxy.Autorisatie.Personen;

public class PersonenAuthorisatieService : AbstractAutorisatieService
{
    private readonly IDiagnosticContext _diagnosticContext;

    public PersonenAuthorisatieService(IServiceProvider serviceProvider, IDiagnosticContext diagnosticContext)
        : base(serviceProvider)
    {
        _diagnosticContext = diagnosticContext;
    }

    public override AuthorisationResult Authorize(int afnemerCode, int? gemeenteCode, string requestBody)
    {
        var autorisatie = GetActueleAutorisatieFor(afnemerCode);
        if (autorisatie != null)
        {
            _diagnosticContext.Set("Autorisatie", autorisatie, true);
        }

        if (GeenAutorisatieOfNietGeautoriseerdVoorAdHocGegevensverstrekking(autorisatie))
        {
            return NietGeautoriseerdVoorAdhocGegevensverstrekking(autorisatie, afnemerCode);
        }

        if (gemeenteCode.HasValue)
        {
            _diagnosticContext.Set("Authorized", "Afnemer is gemeente");
            return Authorized();
        }

        var input = JObject.Parse(requestBody);

        var zoekElementNrs = input.BepaalElementNrVanZoekParameters(Constanten.FieldElementNrDictionary);

        var geautoriseerdeElementNrs = autorisatie!.RubrieknummerAdHoc!.Split(' ');

        var nietGeautoriseerdQueryElementNrs = BepaalNietGeautoriseerdeElementNamen(geautoriseerdeElementNrs, zoekElementNrs);
        if (nietGeautoriseerdQueryElementNrs.Any())
        {
            return NietGeautoriseerdVoorParameters(nietGeautoriseerdQueryElementNrs);
        }

        var fieldElementNrs = BepaalElementNrVanFields(input);

        var nietGeautoriseerdFieldNames = BepaalNietGeautoriseerdeElementNamen(geautoriseerdeElementNrs, fieldElementNrs);
        if (nietGeautoriseerdFieldNames.Any())
        {
            return NietGeautoriseerdVoorFields(nietGeautoriseerdFieldNames, afnemerCode);
        }

        return Authorized();
    }

    private static AuthorisationResult NietGeautoriseerdVoorAdhocGegevensverstrekking(Data.Autorisatie? autorisatie, int afnemerCode) =>
        NotAuthorized(
            title: "U bent niet geautoriseerd voor het gebruik van deze API.",
            detail: "Niet geautoriseerd voor ad hoc bevragingen.",
            code: "unauthorized",
            reason: autorisatie != null
                ? $"Vereiste ad_hoc_medium: A of N. Werkelijk: {autorisatie.AdHocMedium}. Afnemer code: {autorisatie.AfnemerCode}"
                : $"Geen\\Verlopen autorisatie gevonden voor Afnemer code {afnemerCode}");

    private static AuthorisationResult NietGeautoriseerdVoorParameters(IEnumerable<string> nietGeautoriseerdQueryElementNrs) =>
        NotAuthorized(title: "U bent niet geautoriseerd voor de gebruikte parameter(s).",
                      detail: $"U bent niet geautoriseerd voor het gebruik van parameter(s): {string.Join(", ", nietGeautoriseerdQueryElementNrs.OrderBy(x => x))}.",
                      code: "unauthorizedParameter");

    private static AuthorisationResult NietGeautoriseerdVoorFields(IEnumerable<string> nietGeautoriseerdFieldNames, int afnemerCode) =>
        NotAuthorized(title: "U bent niet geautoriseerd voor één of meerdere opgegeven field waarden.",
                      code: "unauthorizedField",
                      reason: $"afnemer '{afnemerCode}' is niet geautoriseerd voor fields {string.Join(", ", nietGeautoriseerdFieldNames.OrderBy(x => x))}");

    private static bool GeenAutorisatieOfNietGeautoriseerdVoorAdHocGegevensverstrekking(Data.Autorisatie? autorisatie)
    {
        return autorisatie == null || !new[] { 'A', 'N' }.Contains(autorisatie.AdHocMedium);
    }

    private static IEnumerable<(string Name, string[] Value)> BepaalElementNrVanFields(JObject input)
    {
        var retval = new List<(string Name, string[] Value)>();

        var zoekType = input.Value<string>("type");
        var gevraagdeFields = from field in input.Value<JArray>("fields")!
                              let v = field.Value<string>()
                              where v != null
                              select v;

        foreach (var gevraagdField in gevraagdeFields)
        {
            var key = zoekType != "RaadpleegMetBurgerservicenummer"
                ? $"{gevraagdField}-beperkt"
                : gevraagdField;

            var fieldElementNrs = Constanten.FieldElementNrDictionary.ContainsKey(key)
                ? Constanten.FieldElementNrDictionary[key]
                : Constanten.FieldElementNrDictionary[gevraagdField];

            retval.Add(new(gevraagdField, fieldElementNrs.Split(' ')));
        }
        return retval;
    }

    private static IEnumerable<string> BepaalNietGeautoriseerdeElementNamen(IEnumerable<string> geautoriseerdeElementen,
                                                                            IEnumerable<(string Name, string[] Value)> gevraagdeElementen)
    {
        var retval = new List<string>();

        foreach (var (Name, Value) in gevraagdeElementen)
        {
            foreach (var gevraagdElementNr in Value)
            {
                if (gevraagdElementNr == string.Empty && Name == "ouders.ouderAanduiding")
                {
                    if (!IsGeautoriseerdVoorOuderAanduidingVraag(geautoriseerdeElementen))
                    {
                        retval.Add(Name);
                    }
                }
                else if (!geautoriseerdeElementen.Any(x => gevraagdElementNr == x.PrefixWithZero()))
                {
                    retval.Add(Name);
                }
            }
        }

        return retval.Distinct();
    }

    private static bool IsGeautoriseerdVoorOuderAanduidingVraag(IEnumerable<string> geautoriseerdeElementen)
    {
        var ouder1Regex = new Regex(@"^(02(01|02|03|04|62)\d{2}|PAOU01)$");
        var ouder2Regex = new Regex(@"^(03(01|02|03|04|62)\d{2}|PAOU01)$");

        var isGeautoriseerdVoorOuder1 = false;
        var isGeautoriseerdVoorOuder2 = false;

        foreach (var elementNr in geautoriseerdeElementen)
        {
            var prefixedElementNr = elementNr.PrefixWithZero();
            if (ouder1Regex.IsMatch(prefixedElementNr))
            {
                isGeautoriseerdVoorOuder1 = true;
            }
            if (ouder2Regex.IsMatch(prefixedElementNr))
            {
                isGeautoriseerdVoorOuder2 = true;
            }
        }

        return isGeautoriseerdVoorOuder1 && isGeautoriseerdVoorOuder2;
    }
}

internal static class AutorisationServiceHelpers
{
    public static string PrefixWithZero(this string str)
    {
        return str.Length == 6
            ? str
            : $"0{str}";
    }
}
