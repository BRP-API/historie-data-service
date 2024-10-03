using Rvig.Data.Base.Postgres.Repositories.Queries;
using Rvig.Data.Historie.DatabaseModels;

namespace Rvig.Data.Repositories.Queries;

public class RvIGHistorieWhereMappingsHelper : RvIGBaseWhereMappingsHelper
{
	public static IDictionary<string, string> GetPersoonHistorieMappings() => GetPersoonBaseMappings();

	public static IDictionary<string, string> GetVerblijfplaatsHistorieMappings() => new Dictionary<string, string>()
	{
		// lo3_pl_persoon
		["pers.pl_id"] = nameof(verblijfplaats_voorkomen.pl_id),

		// lo3_pl
		["pl.bijhouding_opschort_datum as pl_bijhouding_opschort_datum"] = nameof(verblijfplaats_voorkomen.pl_bijhouding_opschort_datum),
		["pl.bijhouding_opschort_reden as pl_bijhouding_opschort_reden"] = nameof(verblijfplaats_voorkomen.pl_bijhouding_opschort_reden),
		["pl.geheim_ind as pl_geheim_ind"] = nameof(verblijfplaats_voorkomen.pl_geheim_ind),

		// lo3_pl_verblijfplaats
		["vb.volg_nr as vb_volg_nr"] = nameof(verblijfplaats_voorkomen.vb_volg_nr),
		["vb.inschrijving_gemeente_code as vb_inschrijving_gemeente_code"] = nameof(verblijfplaats_voorkomen.vb_inschrijving_gemeente_code),
		["vb.inschrijving_datum as vb_inschrijving_datum"] = nameof(verblijfplaats_voorkomen.vb_inschrijving_datum),
		["vb.adres_functie as vb_adres_functie"] = nameof(verblijfplaats_voorkomen.vb_adres_functie),
		["vb.gemeente_deel as vb_gemeente_deel"] = nameof(verblijfplaats_voorkomen.vb_gemeente_deel),
		["vb.vertrek_land_code as vb_vertrek_land_code"] = nameof(verblijfplaats_voorkomen.vb_vertrek_land_code),
		["vb.vertrek_datum as huidig_vertrek_datum"] = nameof(verblijfplaats_voorkomen.vb_vertrek_datum),
		["vb.vertrek_land_adres_1 as vb_vertrek_land_adres_1"] = nameof(verblijfplaats_voorkomen.vb_vertrek_land_adres_1),
		["vb.vertrek_land_adres_2 as vb_vertrek_land_adres_2"] = nameof(verblijfplaats_voorkomen.vb_vertrek_land_adres_2),
		["vb.vertrek_land_adres_3 as vb_vertrek_land_adres_3"] = nameof(verblijfplaats_voorkomen.vb_vertrek_land_adres_3),
		["vb.vestiging_land_code as vb_vestiging_land_code"] = nameof(verblijfplaats_voorkomen.vb_vestiging_land_code),
		["vb.vestiging_datum as vb_vestiging_datum"] = nameof(verblijfplaats_voorkomen.vb_vestiging_datum),
		["vb.aangifte_adreshouding_oms as vb_aangifte_adreshouding_oms"] = nameof(verblijfplaats_voorkomen.vb_aangifte_adreshouding_oms),
		["vb.doc_ind as vb_doc_ind"] = nameof(verblijfplaats_voorkomen.vb_doc_ind),
		["vb.onderzoek_gegevens_aand as vb_onderzoek_gegevens_aand"] = nameof(verblijfplaats_voorkomen.vb_onderzoek_gegevens_aand),
		["vb.onderzoek_start_datum as vb_onderzoek_start_datum"] = nameof(verblijfplaats_voorkomen.vb_onderzoek_start_datum),
		["vb.onderzoek_eind_datum as vb_onderzoek_eind_datum"] = nameof(verblijfplaats_voorkomen.vb_onderzoek_eind_datum),
		["vb.onjuist_ind as vb_onjuist_ind"] = nameof(verblijfplaats_voorkomen.vb_onjuist_ind),
		["vb.geldigheid_start_datum as vb_geldigheid_start_datum"] = nameof(verblijfplaats_voorkomen.vb_geldigheid_start_datum),
		["vb.opneming_datum as vb_opneming_datum"] = nameof(verblijfplaats_voorkomen.vb_opneming_datum),
		["vb.rni_deelnemer as vb_rni_deelnemer"] = nameof(verblijfplaats_voorkomen.vb_rni_deelnemer),
		["vb.verdrag_oms as vb_verdrag_oms"] = nameof(verblijfplaats_voorkomen.vb_verdrag_oms),

		// lo3_adres
		["adres.gemeente_code as adres_gemeente_code"] = nameof(verblijfplaats_voorkomen.adres_gemeente_code),
		["adres.straat_naam as adres_straat_naam"] = nameof(verblijfplaats_voorkomen.adres_straat_naam),
		["adres.diak_straat_naam as adres_diak_straat_naam"] = nameof(verblijfplaats_voorkomen.adres_diak_straat_naam),
		["adres.huis_nr as adres_huis_nr"] = nameof(verblijfplaats_voorkomen.adres_huis_nr),
		["adres.huis_letter as adres_huis_letter"] = nameof(verblijfplaats_voorkomen.adres_huis_letter),
		["adres.huis_nr_toevoeging as adres_huis_nr_toevoeging"] = nameof(verblijfplaats_voorkomen.adres_huis_nr_toevoeging),
		["adres.huis_nr_aand as adres_huis_nr_aand"] = nameof(verblijfplaats_voorkomen.adres_huis_nr_aand),
		["adres.postcode as adres_postcode"] = nameof(verblijfplaats_voorkomen.adres_postcode),
		["adres.locatie_beschrijving as adres_locatie_beschrijving"] = nameof(verblijfplaats_voorkomen.adres_locatie_beschrijving),
		["adres.diak_locatie_beschrijving as adres_diak_locatie_beschrijving"] = nameof(verblijfplaats_voorkomen.adres_diak_locatie_beschrijving),
		["adres.open_ruimte_naam as adres_open_ruimte_naam"] = nameof(verblijfplaats_voorkomen.adres_open_ruimte_naam),
		["adres.diak_open_ruimte_naam as adres_diak_open_ruimte_naam"] = nameof(verblijfplaats_voorkomen.adres_diak_open_ruimte_naam),
		["adres.woon_plaats_naam as adres_woon_plaats_naam"] = nameof(verblijfplaats_voorkomen.adres_woon_plaats_naam),
		["adres.diak_woon_plaats_naam as adres_diak_woon_plaats_naam"] = nameof(verblijfplaats_voorkomen.adres_diak_woon_plaats_naam),
		["adres.verblijf_plaats_ident_code as adres_verblijf_plaats_ident_code"] = nameof(verblijfplaats_voorkomen.adres_verblijf_plaats_ident_code),
		["adres.nummer_aand_ident_code as adres_nummer_aand_ident_code"] = nameof(verblijfplaats_voorkomen.adres_nummer_aand_ident_code),

		["vb.adreshouding_start_datum as huidig_start_adres_datum"] = nameof(verblijfplaats_voorkomen.vb_adreshouding_start_datum),
		["lead(vb.adreshouding_start_datum) over (PARTITION BY vb.pl_id order by vb.pl_id, vb.volg_nr) as vorige_start_adres_datum"] = nameof(verblijfplaats_voorkomen.vorige_start_adres_datum),
		["lag(vb.adreshouding_start_datum) over (PARTITION BY vb.pl_id order by vb.pl_id, vb.volg_nr) as volgende_start_adres_datum"] = nameof(verblijfplaats_voorkomen.volgende_start_adres_datum),
		["lead(adres.verblijf_plaats_ident_code) over (PARTITION BY vb.pl_id order by vb.pl_id, vb.volg_nr) as vorige_adres_verblijf_plaats_ident_code"] = nameof(verblijfplaats_voorkomen.vorige_adres_verblijf_plaats_ident_code),
		["lead(vb.vertrek_datum) over (PARTITION BY vb.pl_id order by vb.pl_id, vb.volg_nr) as vorige_vertrek_datum"] = nameof(verblijfplaats_voorkomen.vorige_vertrek_datum),
		["lag(vb.vertrek_datum) over (PARTITION BY vb.pl_id order by vb.pl_id, vb.volg_nr) as volgende_vertrek_datum"] = nameof(verblijfplaats_voorkomen.volgende_vertrek_datum),
		["lag(vb.onderzoek_gegevens_aand) over (PARTITION BY vb.pl_id order by vb.pl_id, vb.volg_nr) as volgende_onderzoek_gegevens_aand"] = nameof(verblijfplaats_voorkomen.volgende_onderzoek_gegevens_aand),
		["lag(vb.onderzoek_start_datum) over (PARTITION BY vb.pl_id order by vb.pl_id, vb.volg_nr) as volgende_onderzoek_start_datum"] = nameof(verblijfplaats_voorkomen.volgende_onderzoek_start_datum),
		["lag(vb.onderzoek_eind_datum) over (PARTITION BY vb.pl_id order by vb.pl_id, vb.volg_nr) as volgende_onderzoek_eind_datum"] = nameof(verblijfplaats_voorkomen.volgende_onderzoek_eind_datum),
	};

}