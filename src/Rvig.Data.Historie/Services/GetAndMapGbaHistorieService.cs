using Microsoft.AspNetCore.Http;
using Rvig.Data.Base.Postgres.Repositories;
using Rvig.Data.Base.Postgres.Services;
using Rvig.Data.Base.Postgres.DatabaseModels;
using Rvig.Data.Historie.Mappers;
using Rvig.Data.Historie.Repositories;
using Rvig.HaalCentraalApi.Historie.Interfaces;
using Rvig.HaalCentraalApi.Historie.ResponseModels.Historie;
using Rvig.HaalCentraalApi.Shared.Interfaces;
using Rvig.HaalCentraalApi.Shared.Exceptions;
using Rvig.Data.Historie.DatabaseModels;
using Rvig.HaalCentraalApi.Shared.Util;
using Rvig.HaalCentraalApi.Shared.ApiModels.Universal;
using Rvig.Data.Base.Postgres.Mappers.Helpers;

namespace Rvig.Data.Historie.Services;
public class GetAndMapGbaHistorieService : GetAndMapGbaServiceBase, IGetAndMapGbaHistorieService
{
	private readonly IRvigHistorieRepo _dbHistorieRepo;
    private readonly IRvIGDataHistorieMapper _historieMapper;

    public GetAndMapGbaHistorieService(IAutorisationRepo autorisationRepo, IRvigHistorieRepo dbPersoonRepo, IRvIGDataHistorieMapper historieMapper,
        IHttpContextAccessor httpContextAccessor, IProtocolleringService protocolleringService)
		: base(httpContextAccessor, autorisationRepo, protocolleringService)
    {
        _dbHistorieRepo = dbPersoonRepo;
        _historieMapper = historieMapper;
    }

	/// <summary>
	/// Get verblijfplaatshistorie via burgerservicenummer and given period.
	/// </summary>
	/// <param name="burgerservicenummer"></param>
	/// <param name="dateFrom"></param>
	/// <param name="dateTo"></param>
	/// <returns></returns>
	public async Task<(VerblijfplaatshistorieQueryResponse HistoryResponse, int AfnemerCode)> GetVerblijfplaatsHistorieMapByBsn(string burgerservicenummer, DateTime? dateFrom, DateTime? dateTo)
	{
		var historyData = await GetHistorieMapByBsn(
			burgerservicenummer,
			dateFrom,
			dateTo,
			_dbHistorieRepo.GetVerblijfplaatsHistorieByBsn,
			_historieMapper.MapVerblijfplaatsHistorieFrom,
			_historieMapper.MapOpschortingBijhouding);

		var response = new VerblijfplaatshistorieQueryResponse
		{
			Verblijfplaatsen = historyData.MappedHistoryObjects,
			_PlId = historyData.PlId,
			OpschortingBijhouding = historyData.OpschortingBijhouding,
			GeheimhoudingPersoonsgegevens = historyData.GeheimhoudingPersoonsgegevens
		};

		return (response, historyData.AfnemerCode);
	}

	/// <summary>
	/// Returns a result object containing the mapped history objects, geheimhoudingpersoonsgegevens value from bsns, used persoonlijst Ids (plid) from bsns, opschorting from bsns and
	/// afnemer code using a function to get history from the database and a function to map history from the database result.
	/// </summary>
	/// <typeparam name="TMappedHistoryType">The history API has multiple topics so to make sure you do not repeat lines of those, this method doesn't discriminate against history type.</typeparam>
	/// <param name="burgerservicenummer"></param>
	/// <param name="getHistoryDataObjectFunc"></param>
	/// <param name="getMappedHistoryObjectFunc"></param>
	/// <returns>mapped history objects, geheimhoudingpersoonsgegevens value from bsns, used persoonlijst Ids (plid) from bsns, opschorting from bsns and afnemer code. </returns>
	private static async Task<HistoryResult<TMappedHistoryType>> GetHistorieMapByBsn<TMappedHistoryType>(string burgerservicenummer, DateTime? dateFrom, DateTime? dateTo, Func<string, Task<DbVerblijfplaatsHistorieWrapper?>> getHistoryDataObjectFunc, Func<DbVerblijfplaatsHistorieWrapper, Task<IEnumerable<TMappedHistoryType>>> getMappedHistoryObjectFunc, Func<lo3_pl, GbaOpschortingBijhouding?> mapOpschortingBijHouding)
	{
		(List<TMappedHistoryType> historyObjects, int? geheimhoudingPersoonsgegevens) = (new List<TMappedHistoryType>(), null);
		var dbObject = await getHistoryDataObjectFunc(burgerservicenummer);
		GbaOpschortingBijhouding? opschortingBijhouding = null;
		if (dbObject != null)
		{
			var verblijfplaatsVoorkomens = dbObject.VerblijfplaatsVoorkomens.OrderByDescending(vbo => vbo.vb_volg_nr);
			geheimhoudingPersoonsgegevens = dbObject.geheim_ind.HasValue && dbObject.geheim_ind != 0 ? dbObject.geheim_ind : null;

			if (dateFrom.HasValue && dateTo.HasValue)
			{
				dbObject.VerblijfplaatsVoorkomens = FilterObjectsByDateRange(verblijfplaatsVoorkomens, dateFrom.Value, dateTo.Value);
				dbObject.VerblijfplaatsVoorkomens = dbObject.VerblijfplaatsVoorkomens.OrderBy(vbo => vbo.vb_volg_nr);
			}
			else
			{
				throw new InvalidParamsException("Geen peildatum of datumVan/datumTot geleverd.");
			}

			historyObjects = (await getMappedHistoryObjectFunc(dbObject)).ToList();
			opschortingBijhouding = mapOpschortingBijHouding(new lo3_pl { bijhouding_opschort_reden = dbObject.bijhouding_opschort_reden, bijhouding_opschort_datum = dbObject.bijhouding_opschort_datum });
		}
		// It is impossible to have an empty or null array of bsns because the API request models already validate this and reject all non valid values.
		historyObjects = historyObjects
			.Where(x => x != null)
			.ToList();
		return new HistoryResult<TMappedHistoryType> { MappedHistoryObjects = historyObjects, PlId = dbObject?.pl_id, OpschortingBijhouding = opschortingBijhouding, GeheimhoudingPersoonsgegevens = geheimhoudingPersoonsgegevens };
	}

	/// <summary>
	/// Filter objects based on given period.
	/// </summary>
	/// <param name="objects"></param>
	/// <param name="dateFrom"></param>
	/// <param name="dateTo"></param>
	/// <returns></returns>
	public static List<verblijfplaats_voorkomen> FilterObjectsByDateRange(IEnumerable<verblijfplaats_voorkomen> objects, DateTime dateFrom, DateTime dateTo)
	{
		List<verblijfplaats_voorkomen> filteredObjects = new();

		for (int i = 0; i < objects.Count(); i++)
		{
			var obj = objects.ElementAt(i);

			DateTime objectDateStart, objectDateEnd;
			var objStartDate = obj.vb_adreshouding_start_datum?.ToString() ?? obj.vb_vertrek_datum?.ToString();
			objectDateStart = GbaMappingHelper.GetDateTimeFromIncompleteDateString(objStartDate);

			DateTime previousObjectDateStart = DateTime.MinValue;
			if (objects.Count() > 1 && objects.FirstOrDefault() != obj)
			{
				var previousObjStartDate = obj.vorige_start_adres_datum?.ToString() ?? obj.vorige_vertrek_datum?.ToString();
				previousObjectDateStart = GbaMappingHelper.GetDateTimeFromIncompleteDateString(previousObjStartDate);
			}

			if (objectDateStart <= previousObjectDateStart && new DatumOnvolledig(objStartDate).IsOnvolledig())
			{
				objectDateStart = previousObjectDateStart.AddDays(1);
			}

			if (objects.Count() > 1 && objects.LastOrDefault() != obj)
			{
				var nextObjStartDate = obj.volgende_start_adres_datum?.ToString() ?? obj.volgende_vertrek_datum?.ToString();
				// Determine the end date of the current object by looking at the start date of the next object
				DateTime nextObjectDateStart = GbaMappingHelper.GetDateTimeFromIncompleteDateString(nextObjStartDate);

				if (nextObjectDateStart <= objectDateStart && !new DatumOnvolledig(nextObjStartDate).IsOnvolledig())
				{
					continue;
				}

				if (nextObjectDateStart <= objectDateStart)
				{
					objectDateEnd = objectDateStart;
				}
				else
				{
					objectDateEnd = nextObjectDateStart.AddDays(-1);
				}
			}
			else
			{
				objectDateEnd = DateTime.MaxValue;
			}

			// Check if the date range of the object overlaps with the [dateFrom, dateTo) range
			if (objectDateStart < dateTo && objectDateEnd >= dateFrom)
			{
				filteredObjects.Add(obj);
			}
		}

		return filteredObjects;
	}
}
