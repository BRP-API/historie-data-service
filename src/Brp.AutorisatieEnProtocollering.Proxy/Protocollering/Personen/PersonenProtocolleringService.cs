using Brp.AutorisatieEnProtocollering.Proxy.Autorisatie.Personen;
using Brp.AutorisatieEnProtocollering.Proxy.Helpers;
using Brp.Shared.Infrastructure.Protocollering;
using Newtonsoft.Json.Linq;

namespace Brp.AutorisatieEnProtocollering.Proxy.Protocollering.Personen
{
    public class PersonenProtocolleringService : IProtocollering
    {
        private readonly IServiceProvider _serviceProvider;

        public PersonenProtocolleringService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public bool Protocolleer(int afnemerCode, string geleverdePersoonslijstIds, string requestBody)
        {
            using var scope = _serviceProvider.CreateScope();

            var appDbContext = scope.ServiceProvider.GetRequiredService<Data.AppDbContext>();

            var input = JObject.Parse(requestBody);

            var zoekElementNrs = input.BepaalElementNrVanZoekParameters(Constanten.FieldElementNrDictionary);
            var fieldElementNrs = BepaalElementNrVanFieldsVoorProtocollering(input);

            var zoekRubrieken = new List<string>();
            foreach (var (Name, Value) in zoekElementNrs)
            {
                zoekRubrieken.AddRange(Value);
            }
            var requestZoekRubrieken = string.Join(", ", zoekRubrieken.Distinct().OrderBy(x => x));

            var gevraagdeRubrieken = new List<string>();
            foreach (var (Name, Value) in fieldElementNrs)
            {
                gevraagdeRubrieken.AddRange(Value);
            }
            var requestGevraagdeRubrieken = string.Join(", ", gevraagdeRubrieken.Distinct().OrderBy(x => x));

            foreach (var plId in geleverdePersoonslijstIds.Split(','))
            {
                Data.Protocollering protocollering = new()
                {
                    RequestId = Guid.NewGuid().ToString(),
                    AfnemerCode = afnemerCode,
                    PersoonslijstId = long.Parse(plId),
                    RequestZoekRubrieken = requestZoekRubrieken,
                    RequestGevraagdeRubrieken = requestGevraagdeRubrieken
                };

                appDbContext.Add(protocollering);
            }
            appDbContext.SaveChanges();

            return true;
        }

        private static IEnumerable<(string Name, string[] Value)> BepaalElementNrVanFieldsVoorProtocollering(JObject input)
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
                    : $"{gevraagdField}-protocollering";

                var fieldElementNrs = Constanten.FieldElementNrDictionary.ContainsKey(key)
                    ? Constanten.FieldElementNrDictionary[key]
                    : Constanten.FieldElementNrDictionary[gevraagdField];

                retval.Add(new(gevraagdField, !string.IsNullOrWhiteSpace(fieldElementNrs) ? fieldElementNrs.Split(' ') : Array.Empty<string>()));
            }
            return retval;
        }
    }
}
