using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using Rvig.Data.Base.Postgres.Repositories;
using Rvig.Data.Historie.DatabaseModels;
using Rvig.Data.Historie.Repositories.Queries;
using Rvig.Data.Repositories.Queries;
using Rvig.HaalCentraalApi.Shared.Options;

namespace Rvig.Data.Historie.Repositories;
public interface IRvigHistorieRepo
{
	/// <summary>
	/// Get historie of verblijfplaats by BSN.
	/// </summary>
	/// <param name="bsn"></param>
	/// <returns></returns>
	Task<DbVerblijfplaatsHistorieWrapper?> GetVerblijfplaatsHistorieByBsn(string bsn);
}

public class DbHistorieRepo(IOptions<DatabaseOptions> databaseOptions)
	: RvigRepoPostgresBase<verblijfplaats_voorkomen>(databaseOptions), IRvigHistorieRepo
{
    protected override void SetMappings() => CreateMappingsFromWhereMappings();
	protected override void SetWhereMappings() => WhereMappings = RvIGHistorieWhereMappingsHelper.GetVerblijfplaatsHistorieMappings();

	/// <summary>
	/// Get historie of verblijfplaats by BSN.
	/// </summary>
	/// <param name="bsn"></param>
	/// <returns></returns>
	public async Task<DbVerblijfplaatsHistorieWrapper?> GetVerblijfplaatsHistorieByBsn(string bsn)
	{
		(string where, NpgsqlParameter parameter) = HistorieQueryHelper.CreateBurgerservicenummerWhere(bsn);
		where += " and vb.onjuist_ind is null";
		var dynamicParameters = new DynamicParameters();
		dynamicParameters.Add(parameter.ParameterName, parameter.Value);

		var query = string.Format(HistorieQueryHelper.VerblijfplaatsHistorieByBsn, WhereMappings.Select(o => o.Key).Aggregate((i, j) => i + "," + j), where);
		var command = new NpgsqlCommand(query);
		command.Parameters.Add(parameter);

		var verblijfplaatsVoorkomens = (await GetFilterResultAsync(command)).ToList();

		if (verblijfplaatsVoorkomens?.Any() == false)
		{
			return new DbVerblijfplaatsHistorieWrapper();
		}

		var plId = verblijfplaatsVoorkomens!
						.Select(vb => vb.pl_id)
						.FirstOrDefault();
		var geheimInd = verblijfplaatsVoorkomens!
						.Where(vb => vb.pl_geheim_ind.HasValue)
						.Select(vb => vb.pl_geheim_ind)
						.FirstOrDefault();
		var opschortingDatum = verblijfplaatsVoorkomens!
						.Where(vb => vb.pl_bijhouding_opschort_datum.HasValue)
						.Select(vb => vb.pl_bijhouding_opschort_datum)
						.FirstOrDefault();
		var opschortingReden = verblijfplaatsVoorkomens!
						.Where(vb => !string.IsNullOrWhiteSpace(vb.pl_bijhouding_opschort_reden))
						.Select(vb => vb.pl_bijhouding_opschort_reden)
						.FirstOrDefault();

		return new DbVerblijfplaatsHistorieWrapper
		{
			VerblijfplaatsVoorkomens = verblijfplaatsVoorkomens!,
			bijhouding_opschort_reden = opschortingReden,
			bijhouding_opschort_datum = opschortingDatum,
			pl_id = plId,
			geheim_ind = geheimInd
		};
	}
}