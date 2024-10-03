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
using Rvig.HaalCentraalApi.Historie.ApiModels.Historie;

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
        var mappedHistoryObjectsAndGeheimhouding = await GetHistorieMapByBsn(burgerservicenummer, dateFrom, dateTo, _dbHistorieRepo.GetVerblijfplaatsHistorieByBsn, _historieMapper.MapVerblijfplaatsHistorieFrom, _historieMapper.MapOpschortingBijhouding);
        var verblijfplaatsen = OrderVerblijfplaatsVoorkomens(mappedHistoryObjectsAndGeheimhouding.MappedHistoryObjects)
									?.ToList();

		return (new VerblijfplaatshistorieQueryResponse { Verblijfplaatsen = verblijfplaatsen, _PlId = mappedHistoryObjectsAndGeheimhouding.PlId, OpschortingBijhouding = mappedHistoryObjectsAndGeheimhouding.OpschortingBijhouding, GeheimhoudingPersoonsgegevens = mappedHistoryObjectsAndGeheimhouding.GeheimhoudingPersoonsgegevens }, mappedHistoryObjectsAndGeheimhouding.AfnemerCode);
	}

	/// <summary>
	/// If you have three verblijfplaatsVoorkomen items with the following data:
	/// A1:
	///		PreviousLatestStartAddressDate: 2021-05-26
	///		LatestStartAddressDate: 0000-00-00
	///		NextLatestStartAddressDate: 2023-10-14
	/// A2:
	///		PreviousLatestStartAddressDate: null
	///		LatestStartAddressDate: 2021-05-26
	///		NextLatestStartAddressDate: 0000-00-00
	/// A3:
	///		PreviousLatestStartAddressDate: 0000-00-00
	///		LatestStartAddressDate: 2023-10-14
	///		NextLatestStartAddressDate: null
	///
	/// then we expect the order to be A3 -> A1 -> A2. Because having a Next with value NULL means it is the most recent one and having a Previous with value NULL means that is the first item and therefore the oldest.
	/// Everything in between must be ordered based on the Next value.
	/// </summary>
	/// <param name="verblijfplaatsVoorkomens"></param>
	/// <returns></returns>
	private static List<GbaVerblijfplaatsVoorkomen>? OrderVerblijfplaatsVoorkomens(List<GbaVerblijfplaatsVoorkomen>? verblijfplaatsVoorkomens)
	{
		if (verblijfplaatsVoorkomens?.Any() == false)
		{
			return verblijfplaatsVoorkomens;
		}

		// By verblijfplaatsVoorkomens logic one must have a record that has no next date (if regarding their current address) and a record has no previous date (if regarding their first address).
		// It is possible that the current is the same as the first. For that reason we perform a .Distinct at the end of this method.
		var firstInList = verblijfplaatsVoorkomens!.SingleOrDefault(t => t.NextLatestStartAddressDate == null);
		var lastInList = verblijfplaatsVoorkomens!.SingleOrDefault(t => t.PreviousLatestStartAddressDate == null);
		List<GbaVerblijfplaatsVoorkomen>? orderedList = new() {firstInList!};

		orderedList.AddRange(verblijfplaatsVoorkomens!
							  .Except(new List<GbaVerblijfplaatsVoorkomen> { firstInList!, lastInList! })
							  .OrderByDescending(t => t.NextLatestStartAddressDate.HasValue ? (t.NextLatestStartAddressDate != DateTime.MinValue ? t.NextLatestStartAddressDate : DateTime.MaxValue.AddMilliseconds(-1)) : DateTime.MaxValue)
							  .ToList());

		if (lastInList != null)
		{
			orderedList.Add(lastInList!);
		}

		// firstInList can be the same as lastInList if there is only one item in verblijfplaatsVoorkomens.
		orderedList = orderedList.Distinct()
									 .Where(x => x != null)
									 .ToList();

		//if (orderedList.Count > 0 && firstInList == null && lastInList == null)
		//{
		//	orderedList.AddRange(verblijfplaatsVoorkomens!);
		//}

		return orderedList;
	}

	//public async Task<PartnerhistorieQueryResponse> GetPartnerHistorieMapByBsn(string burgerservicenummer)
	//{
	//	var mappedHistoryObjectsAndGeheimhouding = await GetHistorieMapByBsn(burgerservicenummer, _dbHistorieRepo.GetPartnerHistorieByBsn, _historieMapper.MapPartnerHistorieFrom);

	//	return new PartnerhistorieQueryResponse { Partnerhistorie = mappedHistoryObjectsAndGeheimhouding.MappedHistoryObjects, GeheimhoudingPersoonsgegevens = mappedHistoryObjectsAndGeheimhouding.GeheimhoudingPersoonsgegevens };
	//}

	//public async Task<NationalisatiehistorieQueryResponse> GetNationaliteitHistorieMapByBsn(string burgerservicenummer)
	//{
	//	var mappedHistoryObjectsAndGeheimhouding = await GetHistorieMapByBsn(burgerservicenummer, _dbHistorieRepo.GetNationaliteitHistorieByBsn, _historieMapper.MapNationaliteitHistorieFrom);

	//	return new NationalisatiehistorieQueryResponse { Nationaliteithistorie = mappedHistoryObjectsAndGeheimhouding.MappedHistoryObjects, GeheimhoudingPersoonsgegevens = mappedHistoryObjectsAndGeheimhouding.GeheimhoudingPersoonsgegevens };
	//}

	//public async Task<VerblijfstitelhistorieQueryResponse> GetVerblijfstitelHistorieMapByBsn(string burgerservicenummer)
	//{
	//	var mappedHistoryObjectsAndGeheimhouding = await GetHistorieMapByBsn(burgerservicenummer, _dbHistorieRepo.GetVerblijfstitelHistorieByBsn, _historieMapper.MapVerblijfstitelHistorieFrom);

	//	return new VerblijfstitelhistorieQueryResponse { Verblijfstitelhistorie = mappedHistoryObjectsAndGeheimhouding.MappedHistoryObjects, GeheimhoudingPersoonsgegevens = mappedHistoryObjectsAndGeheimhouding.GeheimhoudingPersoonsgegevens };
	//}

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
