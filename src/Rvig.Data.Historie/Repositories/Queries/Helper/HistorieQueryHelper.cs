using Rvig.Data.Base.Postgres.Repositories.Queries;

namespace Rvig.Data.Historie.Repositories.Queries
{
	public class HistorieQueryHelper : QueryBaseHelper
	{
		public static string HistoryQuery => @"select {0}
	from lo3_pl_persoon pers
	left join lo3_pl pl
		on pers.pl_id = pl.pl_id
{1};";
		public static string PersoonAdresHistorieByPlIds => "select adr.* from lo3_adres adr where {0}";
		public static string PersoonVerblijfplaatsHistorieByPlIds => @"select verblfpls.*, inschrvng_plts.gemeente_naam as inschrijving_gemeente_naam, vestgng_land.land_naam as vestiging_land_naam, vertrk_land.land_naam as vertrek_land_naam
from lo3_pl_verblijfplaats verblfpls
	left join lo3_gemeente inschrvng_plts
		on verblfpls.inschrijving_gemeente_code = inschrvng_plts.gemeente_code
	left join lo3_land vestgng_land
		on verblfpls.vestiging_land_code = vestgng_land.land_code
	left join lo3_land vertrk_land
		on verblfpls.vertrek_land_code = vertrk_land.land_code
where {0}";
		public static string PersoonVerblijfstitelHistorieByPlIds => @"select 	verblftl.*, verblftl_oms.verblijfstitel_aand_oms
from lo3_pl_verblijfstitel verblftl
	left join lo3_verblijfstitel_aand verblftl_oms
		on verblftl.verblijfstitel_aand = verblftl_oms.verblijfstitel_aand
where {0}";

		public static string VerblijfplaatsHistorieByBsn => @"select distinct
	{0}
from lo3_pl_persoon pers
	left join lo3_pl pl on pl.pl_id = pers.pl_id
	left join lo3_pl_verblijfplaats vb on vb.pl_id = pers.pl_id
		left join lo3_adres adres on adres.adres_id = vb.adres_id
{1}";
	}
}
