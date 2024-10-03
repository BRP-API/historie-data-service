using Rvig.Data.Base.Authorisation;

namespace Rvig.Data.Historie.DatabaseModels;

public record verblijfplaats_voorkomen
{
	// lo3_pl_persoon
	public long? pl_id { get; set; }

	// lo3_pl
	[RubriekCategory(7), RubriekElement("67.10")] public int? pl_bijhouding_opschort_datum { get; set; }
	[RubriekCategory(7), RubriekElement("67.20")] public string? pl_bijhouding_opschort_reden { get; set; }
	[RubriekCategory(7),RubriekElement("70.10")] public short? pl_geheim_ind { get; set; }

	// lo3_pl_verblijfplaats
	[AlwaysAuthorized] public short vb_volg_nr { get; set; }
	[RubriekElement("09.10")] public short? vb_inschrijving_gemeente_code { get; set; }
	public long? adres_id { get; set; }
	[RubriekElement("09.20")] public int? vb_inschrijving_datum { get; set; }
	[RubriekElement("10.10")] public string? vb_adres_functie { get; set; }
	[RubriekElement("10.20")] public string? vb_gemeente_deel { get; set; }
	[RubriekElement("10.30")] public int? vb_adreshouding_start_datum { get; set; }
	[RubriekElement("13.10")] public short? vb_vertrek_land_code { get; set; }
	[RubriekElement("13.20")] public int? vb_vertrek_datum { get; set; }
	[RubriekElement("13.30")] public string? vb_vertrek_land_adres_1 { get; set; }
	[RubriekElement("13.40")] public string? vb_vertrek_land_adres_2 { get; set; }
	[RubriekElement("13.50")] public string? vb_vertrek_land_adres_3 { get; set; }
	[RubriekElement("14.10")] public short? vb_vestiging_land_code { get; set; }
	[RubriekElement("14.20")] public int? vb_vestiging_datum { get; set; }
	[RubriekElement("72.10")] public string? vb_aangifte_adreshouding_oms { get; set; }
	[RubriekElement("75.10")] public short? vb_doc_ind { get; set; }
	[RubriekElement("83.10")] public int? vb_onderzoek_gegevens_aand { get; set; }
	[RubriekElement("83.20")] public int? vb_onderzoek_start_datum { get; set; }
	[RubriekElement("83.30")] public int? vb_onderzoek_eind_datum { get; set; }
	[RubriekElement("84.10")] public string? vb_onjuist_ind { get; set; }
	[RubriekElement("85.10")] public int? vb_geldigheid_start_datum { get; set; }
	[RubriekElement("86.10")] public int? vb_opneming_datum { get; set; }
	[RubriekElement("88.10")] public short? vb_rni_deelnemer { get; set; }
	[RubriekElement("88.20")] public string? vb_verdrag_oms { get; set; }

	// joined verblijfplaats omschrijvingen
	[RubriekElement("09.10")] public string? vb_inschrijving_gemeente_naam { get; set; }
	[RubriekElement("14.10")] public string? vb_vestiging_land_naam { get; set; }
	[RubriekElement("13.10")] public string? vb_vertrek_land_naam { get; set; }
	[RubriekElement("88.10")] public string? vb_rni_deelnemer_omschrijving { get; set; }

	// lo3_adres
	public short adres_gemeente_code { get; init; }
	[RubriekElement("11.10")] public string? adres_straat_naam { get; init; }
	[RubriekElement("11.10")] public string? adres_diak_straat_naam { get; init; }
	[RubriekElement("11.20")] public int? adres_huis_nr { get; init; }
	[RubriekElement("11.30")] public string? adres_huis_letter { get; init; }
	[RubriekElement("11.40")] public string? adres_huis_nr_toevoeging { get; init; }
	[RubriekElement("11.50")] public string? adres_huis_nr_aand { get; init; }
	[RubriekElement("11.60")] public string? adres_postcode { get; init; }
	[RubriekElement("12.10")] public string? adres_locatie_beschrijving { get; init; }
	[RubriekElement("12.10")] public string? adres_diak_locatie_beschrijving { get; init; }
	[RubriekElement("11.15")] public string? adres_open_ruimte_naam { get; init; }
	[RubriekElement("11.15")] public string? adres_diak_open_ruimte_naam { get; init; }
	[RubriekElement("11.70")] public string? adres_woon_plaats_naam { get; init; }
	[RubriekElement("11.70")] public string? adres_diak_woon_plaats_naam { get; init; }
	[RubriekElement("11.80")] public string? adres_verblijf_plaats_ident_code { get; init; }
	[RubriekElement("11.90")] public string? adres_nummer_aand_ident_code { get; init; }

	// Custom historie
	[RubriekCategory(8), RubriekElement("10.30")] public int? vorige_start_adres_datum { get; set; }
	[RubriekCategory(8), RubriekElement("10.30")] public int? volgende_start_adres_datum { get; set; }
	[RubriekCategory(8), RubriekElement("11.80")] public string? vorige_adres_verblijf_plaats_ident_code { get; set; }
	[RubriekElement("13.20")] public int? vorige_vertrek_datum { get; set; }
	[RubriekElement("13.20")] public int? volgende_vertrek_datum { get; set; }
	[RubriekElement("83.10")] public int? volgende_onderzoek_gegevens_aand { get; set; }
	[RubriekElement("83.20")] public int? volgende_onderzoek_start_datum { get; set; }
	[RubriekElement("83.30")] public int? volgende_onderzoek_eind_datum { get; set; }
}