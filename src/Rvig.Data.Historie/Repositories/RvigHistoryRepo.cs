using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using Rvig.Data.Base.Postgres.Repositories;
using Rvig.Data.Historie.DatabaseModels;
using Rvig.Data.Historie.Repositories.Queries;
using Rvig.Data.Repositories.Queries;
using Rvig.HaalCentraalApi.Shared.Helpers;
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
	//Task<DbPersoonHistorieWrapper?> GetPartnerHistorieByBsn(string bsn);
	//Task<DbPersoonHistorieWrapper?> GetNationaliteitHistorieByBsn(string bsn);
	//Task<DbPersoonHistorieWrapper?> GetVerblijfstitelHistorieByBsn(string bsn);
}

public class DbHistorieRepo : RvigRepoPostgresBase<verblijfplaats_voorkomen>, IRvigHistorieRepo
{
	public DbHistorieRepo(IOptions<DatabaseOptions> databaseOptions, ILoggingHelper loggingHelper)
		: base(databaseOptions, loggingHelper)
	{
	}

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

	//public async Task<DbPersoonHistorieWrapper?> GetPartnerHistorieByBsn(string bsn)
	//{
	//	(DbPersoonHistorieWrapper dbPersoonWrapper, List<long> pl_ids) = await GetHistorieByBsnBase(bsn);

	//	if (pl_ids.Count > 0)
	//	{
	//		var additionalMappings = RvIGHistorieWhereMappingsHelper.GetAdditionalPlRelationMappings();
	//		var plRelationMappings = WhereMappings.Concat(additionalMappings).Select(o => o.Key).Aggregate((i, j) => i + "," + j);
	//		var persoonslijsten = (await GetFilterResultAsync(CreateFilterCommand(HistorieQueryHelper.PersoonslijstByPlIds, plRelationMappings, GetPlIdsWhere(pl_ids, "pers")), CreateMappingsFromWhereMappings(additionalMappings)))
	//			.Select(persoonWrapper => persoonWrapper.Persoon);
	//		dbPersoonWrapper.Partners = persoonslijsten.Where(x => x.persoon_type == "R").ToList();
	//	}

	//	return dbPersoonWrapper;
	//}

	//public async Task<DbPersoonHistorieWrapper?> GetNationaliteitHistorieByBsn(string bsn)
	//{
	//	(DbPersoonHistorieWrapper dbPersoonWrapper, List<long> pl_ids) = await GetHistorieByBsnBase(bsn);

	//	if (pl_ids.Count > 0)
	//	{
	//		dbPersoonWrapper.Nationaliteiten = await GetPersoonDataByPlIds<lo3_pl_nationaliteit>(pl_ids, HistorieQueryHelper.PersoonNationaliteitenByPlIds, "natnltt");
	//	}

	//	return dbPersoonWrapper;
	//}

	//public async Task<DbPersoonHistorieWrapper?> GetVerblijfstitelHistorieByBsn(string bsn)
	//{
	//	(DbPersoonHistorieWrapper dbPersoonWrapper, List<long> pl_ids) = await GetHistorieByBsnBase(bsn);

	//	if (pl_ids.Count > 0)
	//	{
	//		dbPersoonWrapper.Verblijfstitels = await GetPersoonDataByPlIds<lo3_pl_verblijfstitel>(pl_ids, HistorieQueryHelper.PersoonVerblijfstitelHistorieByPlIds, "verblftl");
	//	}

	//	return dbPersoonWrapper;
	//}

	//private async Task<(DbPersoonHistorieWrapper, List<long>)> GetHistorieByBsnBase(string bsn)
	//{
	//	(string where, NpgsqlParameter parameter) = HistorieQueryHelper.CreateBurgerservicenummerWhere(bsn);
	//	var query = string.Format(HistorieQueryHelper.HistoryQuery, WhereMappings.Select(o => o.Key).Aggregate((i, j) => i + "," + j), where);
	//	var command = new NpgsqlCommand(query);
	//	command.Parameters.Add(parameter);

	//	var dbPersonen = await GetFilterResultAsync(command);
	//	if (dbPersonen.Count() > 1)
	//	{
	//		var invalidParam = new InvalidParams
	//		{
	//			Code = "unique",
	//			Reason = "De opgegeven persoonidentificatie is niet uniek.",
	//			Name = nameof(HistorieQuery.burgerservicenummer)
	//		};

	//		throw new InvalidParamsException(new List<InvalidParams> { invalidParam });
	//	}
	//	var dbPersoonWrapper = dbPersonen.SingleOrDefault();

	//	if (dbPersoonWrapper == null)
	//	{
	//		return default;
	//	}
	//	var pl_ids = new List<long> { dbPersoonWrapper.Persoon.pl_id };

	//	return (dbPersoonWrapper, pl_ids);
	//}

	//private static (DynamicParameters, string) GetAdresIdConditions(List<long?> verblijfplaatsAdresIds)
	//{
	//	var dynamicParameters = new DynamicParameters();
	//	var adresIdIndex = 0;
	//	var adresIdConditions = verblijfplaatsAdresIds.Select(x =>
	//	{
	//		adresIdIndex++;
	//		dynamicParameters.Add($"ADRES_ID{adresIdIndex}", x);
	//		return $"adr.adres_id = @ADRES_ID{adresIdIndex}";
	//	});
	//	return (dynamicParameters, $"({string.Join(" or ", adresIdConditions)})");
	//}
}