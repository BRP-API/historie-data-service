using Rvig.Data.Base.Postgres.Repositories.Queries;

namespace Rvig.Data.Historie.Repositories.Queries
{
	public class HistorieQueryHelper : QueryBaseHelper
	{
		public static string VerblijfplaatsHistorieByBsn => @"select distinct
	{0}
from lo3_pl_persoon pers
	left join lo3_pl pl on pl.pl_id = pers.pl_id
	left join lo3_pl_verblijfplaats vb on vb.pl_id = pers.pl_id
		left join lo3_adres adres on adres.adres_id = vb.adres_id
{1}";
	}
}
