using Rvig.HaalCentraalApi.Historie.ApiModels.Historie;
using Rvig.HaalCentraalApi.Shared.Helpers;
using Rvig.Data.Base.Postgres.Mappers;
using Rvig.Data.Base.Postgres.Helpers;
using Rvig.HaalCentraalApi.Shared.ApiModels.Universal;
using Rvig.HaalCentraalApi.Shared.Exceptions;
using Rvig.Data.Historie.DatabaseModels;
using Rvig.Data.Base.Postgres.Mappers.Helpers;
using Rvig.Data.Base.Postgres.DatabaseModels;

namespace Rvig.Data.Historie.Mappers;
public interface IRvIGDataHistorieMapper
{
	/// <summary>
	/// Maps all attributes from the database model to the gba haalcentraal historic verblijfplaatsen.
	/// </summary>
	Task<IEnumerable<GbaVerblijfplaatsVoorkomen>> MapVerblijfplaatsHistorieFrom(DbVerblijfplaatsHistorieWrapper dbObjectWrapper);

	/// <summary>
	/// Map opschorting which is used in multiple APIs such as Personen, Reisdocumenten, Bewoningen and Historie.
	/// </summary>
	/// <param name="inschrijving"></param>
	/// <returns></returns>
	GbaOpschortingBijhouding? MapOpschortingBijhouding(lo3_pl inschrijving);
	//IEnumerable<GbaPartnerHistorie> MapPartnerHistorieFrom(DbPersoonHistorieWrapper dbPersoonWrapper);
	//IEnumerable<GbaNationaliteitHistorie> MapNationaliteitHistorieFrom(DbPersoonHistorieWrapper dbPersoonWrapper);
	//IEnumerable<GbaVerblijfstitel> MapVerblijfstitelHistorieFrom(DbPersoonHistorieWrapper dbPersoonWrapper);
}

public class RvIGDataHistorieMapper : RvIGDataMapperBase, IRvIGDataHistorieMapper
{
	public RvIGDataHistorieMapper(IDomeinTabellenHelper domeinTabellenHelper) : base(domeinTabellenHelper)
	{
	}
    /// <summary>
    /// Maps all attributes from the database model to the gba haalcentraal historic verblijfplaatsen.
    /// </summary>
    public async Task<IEnumerable<GbaVerblijfplaatsVoorkomen>> MapVerblijfplaatsHistorieFrom(DbVerblijfplaatsHistorieWrapper dbObjectWrapper)
	{
		IEnumerable<GbaVerblijfplaatsVoorkomen>? verblijfplaatsen = await Task.WhenAll(dbObjectWrapper.VerblijfplaatsVoorkomens.Select(MapVerblijfplaatsHistorie));

		if (verblijfplaatsen?.Any() != true)
		{
			return new List<GbaVerblijfplaatsVoorkomen>();
		}

		return verblijfplaatsen.Where(x => !ObjectHelper.AllPropertiesDefault(x));
	}

	/// <summary>
	/// Map opschorting which is used in multiple APIs such as Personen, Reisdocumenten, Bewoningen and Historie.
	/// </summary>
	/// <param name="inschrijving"></param>
	/// <returns></returns>
	GbaOpschortingBijhouding? IRvIGDataHistorieMapper.MapOpschortingBijhouding(lo3_pl inschrijving) => MapOpschortingBijhouding(inschrijving);

	private async Task<GbaVerblijfplaatsVoorkomen> MapVerblijfplaatsHistorie(verblijfplaats_voorkomen dbVerblijfplaatsVoorkomen)
	{
		var historieVerblijfplaats = new GbaVerblijfplaatsVoorkomen();

		foreach (var propertyName in ObjectHelper.GetPropertyNames<GbaVerblijfplaatsVoorkomen>())
		{
			switch (propertyName)
			{
				case nameof(GbaVerblijfplaatsVoorkomen.AdresseerbaarObjectIdentificatie):
					if (dbVerblijfplaatsVoorkomen?.adres_verblijf_plaats_ident_code?.Length == 16 && long.TryParse(dbVerblijfplaatsVoorkomen?.adres_verblijf_plaats_ident_code, out _))
					{
						historieVerblijfplaats.AdresseerbaarObjectIdentificatie = dbVerblijfplaatsVoorkomen?.adres_verblijf_plaats_ident_code;
					}
					break;
				case nameof(GbaVerblijfplaatsVoorkomen.NummeraanduidingIdentificatie):
					if (dbVerblijfplaatsVoorkomen?.adres_nummer_aand_ident_code?.Length == 16 && long.TryParse(dbVerblijfplaatsVoorkomen?.adres_nummer_aand_ident_code, out _))
					{
						historieVerblijfplaats.NummeraanduidingIdentificatie = dbVerblijfplaatsVoorkomen?.adres_nummer_aand_ident_code;
					}
					break;
				case nameof(GbaVerblijfplaatsVoorkomen.DatumAanvangAdresBuitenland):
					historieVerblijfplaats.DatumAanvangAdresBuitenland = GbaMappingHelper.ParseToDatumOnvolledig(dbVerblijfplaatsVoorkomen?.vb_vertrek_datum);
					break;
				case nameof(GbaVerblijfplaatsVoorkomen.DatumAanvangVolgendeAdresBuitenland):
					historieVerblijfplaats.DatumAanvangVolgendeAdresBuitenland = GbaMappingHelper.ParseToDatumOnvolledig(dbVerblijfplaatsVoorkomen?.volgende_vertrek_datum);
					break;
				case nameof(GbaVerblijfplaatsVoorkomen.DatumAanvangAdreshouding):
					historieVerblijfplaats.DatumAanvangAdreshouding = GbaMappingHelper.ParseToDatumOnvolledig(dbVerblijfplaatsVoorkomen?.vb_adreshouding_start_datum);
					break;
				case nameof(GbaVerblijfplaatsVoorkomen.DatumAanvangVolgendeAdreshouding):
					historieVerblijfplaats.DatumAanvangVolgendeAdreshouding = GbaMappingHelper.ParseToDatumOnvolledig(dbVerblijfplaatsVoorkomen?.volgende_start_adres_datum);
					break;
				case nameof(GbaVerblijfplaatsVoorkomen.FunctieAdres):
					historieVerblijfplaats.FunctieAdres = GbaMappingHelper.ParseToSoortAdresEnum(dbVerblijfplaatsVoorkomen!.vb_adres_functie);
					break;
				case nameof(GbaVerblijfplaatsVoorkomen.NaamOpenbareRuimte):
					historieVerblijfplaats.NaamOpenbareRuimte = dbVerblijfplaatsVoorkomen?.adres_diak_open_ruimte_naam ?? dbVerblijfplaatsVoorkomen?.adres_open_ruimte_naam;
					break;
				case nameof(GbaVerblijfplaatsVoorkomen.GemeenteVanInschrijving):
					if (dbVerblijfplaatsVoorkomen?.vb_inschrijving_gemeente_code != null)
					{
						historieVerblijfplaats.GemeenteVanInschrijving = new Waardetabel
						{
							Code = dbVerblijfplaatsVoorkomen.vb_inschrijving_gemeente_code?.ToString().PadLeft(4, '0'),
							Omschrijving = dbVerblijfplaatsVoorkomen.vb_inschrijving_gemeente_naam ?? await _domeinTabellenHelper.GetGemeenteOmschrijving(dbVerblijfplaatsVoorkomen.vb_inschrijving_gemeente_code)
						};
					}
					break;
				case nameof(GbaVerblijfplaatsVoorkomen.Rni):
					if (dbVerblijfplaatsVoorkomen?.vb_rni_deelnemer.HasValue == true)
					{
						historieVerblijfplaats.Rni = new RniDeelnemer
						{
							// Category not mapped because unlike with the Personen API, it is always the same category.
							//Categorie = "Verblijfplaats",
							Deelnemer = new Waardetabel
							{
								Code = dbVerblijfplaatsVoorkomen.vb_rni_deelnemer.Value.ToString()?.PadLeft(4, '0'),
								Omschrijving = dbVerblijfplaatsVoorkomen.vb_rni_deelnemer_omschrijving ?? await _domeinTabellenHelper.GetRniDeelnemerOmschrijving(dbVerblijfplaatsVoorkomen.vb_rni_deelnemer)
							},
							OmschrijvingVerdrag = dbVerblijfplaatsVoorkomen.vb_verdrag_oms
						};
					}
					break;
				case nameof(GbaVerblijfplaatsVoorkomen.InOnderzoekVolgendeVerblijfplaats):
					historieVerblijfplaats.InOnderzoekVolgendeVerblijfplaats = MapGbaInOnderzoekVolgendeVerblijfplaats(dbVerblijfplaatsVoorkomen?.volgende_onderzoek_gegevens_aand, dbVerblijfplaatsVoorkomen?.volgende_onderzoek_start_datum, dbVerblijfplaatsVoorkomen?.volgende_onderzoek_eind_datum);
					break;
				// GbaVerblijfplaatsBeperkt
				case nameof(GbaVerblijfplaatsVoorkomen.Locatiebeschrijving):
					historieVerblijfplaats.Locatiebeschrijving = dbVerblijfplaatsVoorkomen?.adres_diak_locatie_beschrijving ?? dbVerblijfplaatsVoorkomen?.adres_locatie_beschrijving;
					break;
				case nameof(GbaVerblijfplaatsVoorkomen.Straat):
					historieVerblijfplaats.Straat = dbVerblijfplaatsVoorkomen?.adres_diak_straat_naam ?? dbVerblijfplaatsVoorkomen?.adres_straat_naam;
					break;
				case nameof(GbaVerblijfplaatsVoorkomen.AanduidingBijHuisnummer):
					historieVerblijfplaats.AanduidingBijHuisnummer = GbaMappingHelper.ParseToAanduidingBijHuisnummerEnum(dbVerblijfplaatsVoorkomen?.adres_huis_nr_aand);
					break;
				case nameof(GbaVerblijfplaatsVoorkomen.Regel1):
					historieVerblijfplaats.Regel1 = dbVerblijfplaatsVoorkomen?.vb_vertrek_land_adres_1;
					break;
				case nameof(GbaVerblijfplaatsVoorkomen.Regel2):
					historieVerblijfplaats.Regel2 = dbVerblijfplaatsVoorkomen?.vb_vertrek_land_adres_2;
					break;
				case nameof(GbaVerblijfplaatsVoorkomen.Regel3):
					historieVerblijfplaats.Regel3 = dbVerblijfplaatsVoorkomen?.vb_vertrek_land_adres_3;
					break;
				case nameof(GbaVerblijfplaatsVoorkomen.Land):
					if (dbVerblijfplaatsVoorkomen?.vb_vertrek_land_code != null && dbVerblijfplaatsVoorkomen.vb_vertrek_land_code != 6030)
					{
						historieVerblijfplaats.Land = new Waardetabel
						{
							Code = dbVerblijfplaatsVoorkomen.vb_vertrek_land_code?.ToString().PadLeft(4, '0'),
							Omschrijving = dbVerblijfplaatsVoorkomen.vb_vertrek_land_naam ?? await _domeinTabellenHelper.GetLandOmschrijving(dbVerblijfplaatsVoorkomen?.vb_vertrek_land_code)
						};
					}
					break;
				case nameof(GbaVerblijfplaatsVoorkomen.Huisnummer):
					historieVerblijfplaats.Huisnummer = dbVerblijfplaatsVoorkomen?.adres_huis_nr;
					break;
				case nameof(GbaVerblijfplaatsVoorkomen.Huisletter):
					historieVerblijfplaats.Huisletter = dbVerblijfplaatsVoorkomen?.adres_huis_letter;
					break;
				case nameof(GbaVerblijfplaatsVoorkomen.Huisnummertoevoeging):
					historieVerblijfplaats.Huisnummertoevoeging = dbVerblijfplaatsVoorkomen?.adres_huis_nr_toevoeging;
					break;
				case nameof(GbaVerblijfplaatsVoorkomen.Postcode):
					historieVerblijfplaats.Postcode = dbVerblijfplaatsVoorkomen?.adres_postcode;
					break;
				case nameof(GbaVerblijfplaatsVoorkomen.Woonplaats):
					historieVerblijfplaats.Woonplaats = !string.IsNullOrEmpty(dbVerblijfplaatsVoorkomen?.adres_diak_woon_plaats_naam) ? dbVerblijfplaatsVoorkomen?.adres_diak_woon_plaats_naam : dbVerblijfplaatsVoorkomen?.adres_woon_plaats_naam;
					break;
				case nameof(GbaVerblijfplaatsVoorkomen.InOnderzoek):
					historieVerblijfplaats.InOnderzoek = MapGbaInOnderzoek(dbVerblijfplaatsVoorkomen?.vb_onderzoek_gegevens_aand, dbVerblijfplaatsVoorkomen?.vb_onderzoek_start_datum, dbVerblijfplaatsVoorkomen?.vb_onderzoek_eind_datum);
					break;
				case nameof(GbaVerblijfplaatsVoorkomen.LatestStartAddressDate):
					historieVerblijfplaats.LatestStartAddressDate = dbVerblijfplaatsVoorkomen?.vb_adreshouding_start_datum.HasValue == true
																			? GbaMappingHelper.GetNullableDateTimeFromIncompleteDateString(GbaMappingHelper.ParseToDatumOnvolledig(dbVerblijfplaatsVoorkomen?.vb_adreshouding_start_datum))
																			: GbaMappingHelper.GetNullableDateTimeFromIncompleteDateString(GbaMappingHelper.ParseToDatumOnvolledig(dbVerblijfplaatsVoorkomen?.vb_vertrek_datum));
					break;
				case nameof(GbaVerblijfplaatsVoorkomen.NextLatestStartAddressDate):
					historieVerblijfplaats.NextLatestStartAddressDate = dbVerblijfplaatsVoorkomen?.volgende_start_adres_datum.HasValue == true
																			? GbaMappingHelper.GetNullableDateTimeFromIncompleteDateString(GbaMappingHelper.ParseToDatumOnvolledig(dbVerblijfplaatsVoorkomen?.volgende_start_adres_datum))
																			: GbaMappingHelper.GetNullableDateTimeFromIncompleteDateString(GbaMappingHelper.ParseToDatumOnvolledig(dbVerblijfplaatsVoorkomen?.volgende_vertrek_datum));
					break;
				case nameof(GbaVerblijfplaatsVoorkomen.PreviousLatestStartAddressDate):
					historieVerblijfplaats.PreviousLatestStartAddressDate = dbVerblijfplaatsVoorkomen?.vorige_start_adres_datum.HasValue == true
																			? GbaMappingHelper.GetNullableDateTimeFromIncompleteDateString(GbaMappingHelper.ParseToDatumOnvolledig(dbVerblijfplaatsVoorkomen?.vorige_start_adres_datum))
																			: GbaMappingHelper.GetNullableDateTimeFromIncompleteDateString(GbaMappingHelper.ParseToDatumOnvolledig(dbVerblijfplaatsVoorkomen?.vorige_vertrek_datum));
					break;
				default:
					throw new CustomNotImplementedException($"Mapping not implemented for {nameof(GbaVerblijfplaatsVoorkomen)} property {propertyName}.");
			}
		}
		return historieVerblijfplaats;
	}

	protected override GbaInOnderzoek? MapGbaInOnderzoek(int? inOnderzoekAanduiding, int? inOnderzoekBeginDatum, int? inOnderzoekEindDatum)
	{
		if ((!inOnderzoekAanduiding.HasValue && !inOnderzoekBeginDatum.HasValue) || (inOnderzoekEindDatum.HasValue && inOnderzoekAanduiding != 89999 && inOnderzoekAanduiding != 589999))
		{
			return null;
		}

		return new GbaInOnderzoek
		{
			AanduidingGegevensInOnderzoek = (inOnderzoekAanduiding?.ToString().PadLeft(6, '0')),
			DatumIngangOnderzoek = GbaMappingHelper.ParseToDatumOnvolledig(inOnderzoekBeginDatum),
			DatumEindeOnderzoek = GbaMappingHelper.ParseToDatumOnvolledig(inOnderzoekEindDatum)
		};
	}

	private GbaInOnderzoek? MapGbaInOnderzoekVolgendeVerblijfplaats(int? inOnderzoekAanduiding, int? inOnderzoekBeginDatum, int? inOnderzoekEindDatum)
	{
		return base.MapGbaInOnderzoek(inOnderzoekAanduiding, inOnderzoekBeginDatum, inOnderzoekEindDatum);
	}

	///// <summary>
	///// Maps all attributes from the database model to the gba haalcentraal historic partners.
	///// </summary>
	//public IEnumerable<GbaPartnerHistorie> MapPartnerHistorieFrom(DbPersoonHistorieWrapper dbPersoonWrapper)
	//   {
	//	IEnumerable<GbaPartnerHistorie>? partners = dbPersoonWrapper.Partners.Select(partner => MapPartnerHistorie(partner))
	//																				.Where(x => x != null);

	//       if (partners?.Any() != true)
	//       {
	//           return new List<GbaPartnerHistorie>();
	//       }

	//       return partners.Where(x => !ObjectHelper.AllPropertiesDefault(x));
	//}

	//private async Task<GbaOntbindingHuwelijkPartnerschapHistorie?> MapOntbindingHuwelijkPartnerschapHistorie(lo3_pl_persoon dbPartner)
	//{
	//	var ontbindingHuwelijkPartnerschapHistorie = new GbaOntbindingHuwelijkPartnerschapHistorie();

	//	foreach (var propertyName in ObjectHelper.GetPropertyNames<GbaOntbindingHuwelijkPartnerschapHistorie>())
	//	{
	//		switch (propertyName)
	//		{
	//			case nameof(GbaOntbindingHuwelijkPartnerschapHistorie.Datum):
	//				ontbindingHuwelijkPartnerschapHistorie.Datum = GbaMappingHelper.ParseToDatumOnvolledig(dbPartner.relatie_eind_datum);
	//				break;
	//			case nameof(GbaOntbindingHuwelijkPartnerschapHistorie.Land):
	//				ontbindingHuwelijkPartnerschapHistorie.Land = dbPartner.relatie_eind_land_code.HasValue ?
	//					 new Waardetabel { Code = dbPartner.relatie_eind_land_code?.ToString().PadLeft(4, '0'), Omschrijving = dbPartner.relatie_eind_land_naam } : null;
	//				break;
	//			case nameof(GbaOntbindingHuwelijkPartnerschapHistorie.Plaats):
	//				ontbindingHuwelijkPartnerschapHistorie.Plaats = await MapPlaats(dbPartner.relatie_eind_land_code, dbPartner.relatie_eind_plaats, dbPartner.relatie_eind_plaats_naam);
	//				break;
	//			case nameof(GbaOntbindingHuwelijkPartnerschapHistorie.Reden): // TODOd
	//				ontbindingHuwelijkPartnerschapHistorie.Reden = !string.IsNullOrWhiteSpace(dbPartner.relatie_eind_reden_oms)
	//					? new Waardetabel { Code = dbPartner.relatie_eind_reden, Omschrijving = dbPartner.relatie_eind_reden_oms }
	//					: null;
	//				break;
	//			default:
	//				throw new CustomNotImplementedException($"Mapping not implemented for {nameof(GbaOntbindingHuwelijkPartnerschap)} property {propertyName}");
	//		}
	//	}

	//	return ObjectHelper.InstanceOrNullWhenDefault(ontbindingHuwelijkPartnerschapHistorie);
	//}

	///// <summary>
	///// Maps all attributes from the database model to the gba haalcentraal historic partners.
	///// </summary>
	//public IEnumerable<GbaNationaliteitHistorie> MapNationaliteitHistorieFrom(DbPersoonHistorieWrapper dbPersoonWrapper)
	//{
	//	throw new CustomNotImplementedException("Reimplement nationaliteiten history mapping.");
	//	//IEnumerable<GbaNationaliteitHistorie>? nationaliteiten = MapNationaliteiten<GbaNationaliteitHistorie>(dbPersoonWrapper.Nationaliteiten, true);

	// //      if (nationaliteiten?.Any() != true)
	// //      {
	// //          return new List<GbaNationaliteitHistorie>();
	// //      }

	// //      return nationaliteiten.Where(x => !ObjectHelper.AllPropertiesDefault(x));
	//   }

	///// <summary>
	///// Maps all attributes from the database model to the gba haalcentraal historic partners.
	///// </summary>
	//public IEnumerable<GbaVerblijfstitel> MapVerblijfstitelHistorieFrom(DbPersoonHistorieWrapper dbPersoonWrapper)
	//   {
	//	IEnumerable<GbaVerblijfstitel> verblijfstitels = dbPersoonWrapper.Verblijfstitels.Select(verblijfstitel => MapVerblijfstitel(verblijfstitel))
	//																							.Where(x => x != null)
	//																							.Cast<GbaVerblijfstitel>();

	//       if (verblijfstitels?.Any() != true)
	//       {
	//           return new List<GbaVerblijfstitel>();
	//       }

	//       return verblijfstitels.Where(x => !ObjectHelper.AllPropertiesDefault(x));
	//}

	//private GbaPartnerHistorie MapPartnerHistorie(lo3_pl_persoon partner)
	//{
	//	throw new CustomNotImplementedException("Reimplement partner history mapping.");
	//	//var excludePropNames = new List<string> { nameof(GbaPartnerHistorie.OntbindingHuwelijkPartnerschapHistorie), nameof(GbaPartnerHistorie._datumAangaanHuwelijkPartnerschap), nameof(GbaPartnerHistorie._datumOntbindingHuwelijkPartnerschap) };
	//	//var historiePartner = MapPartner<GbaPartnerHistorie>(partner, excludePropNames).gbaPartner;

	//	//if (historiePartner == null)
	//	//{
	//	//	return new GbaPartnerHistorie();
	//	//}
	//	//historiePartner.OntbindingHuwelijkPartnerschapHistorie = MapOntbindingHuwelijkPartnerschapHistorie(partner);

	//	//return historiePartner;
	//}
}