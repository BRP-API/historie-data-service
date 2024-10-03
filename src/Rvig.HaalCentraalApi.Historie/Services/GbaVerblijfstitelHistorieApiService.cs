using Rvig.HaalCentraalApi.Shared.Interfaces;
using Rvig.HaalCentraalApi.Historie.Interfaces;

namespace Rvig.HaalCentraalApi.Historie.Services;

public interface IGbaVerblijfstitelHistorieApiService
{
	//Task<VerblijfstitelhistorieQueryResponse> GetVerblijftitelHistorie(HistorieQuery model);
}
public class GbaVerblijfstitelHistorieApiService : GbaHistorieApiServiceBase, IGbaVerblijfstitelHistorieApiService
{
	public GbaVerblijfstitelHistorieApiService(IGetAndMapGbaHistorieService getAndMapHistorieService, IDomeinTabellenRepo domeinTabellenRepo)
		: base(getAndMapHistorieService, domeinTabellenRepo)
	{
	}

	///// <summary>
	///// Get verblijfstitel history via search params (child of HistorieQuery)
	///// </summary>
	///// <param name="model"></param>
	///// <returns>Response object with list of verblijfstitel history data</returns>
	//public async Task<VerblijfstitelhistorieQueryResponse> GetVerblijftitelHistorie(HistorieQuery model)
	//{
	//	// Validate + Get history
	//	var verblijfstitelHistorieResponse = await GetHistoryBase(model.burgerservicenummer, model.fields, _fieldsSettings.GbaFieldsSettings, _getAndMapHistorieService.GetVerblijfstitelHistorieMapByBsn);

	//	if (verblijfstitelHistorieResponse.Verblijfstitelhistorie?.Any() != true)
	//	{
	//		return new VerblijfstitelhistorieQueryResponse();
	//	}

	//	// Filter response by date and fields
	//	verblijfstitelHistorieResponse.Verblijfstitelhistorie = FilterResponseByDates(verblijfstitelHistorieResponse.Verblijfstitelhistorie, model, nameof(GbaVerblijfstitel.DatumIngang), nameof(GbaVerblijfstitel.DatumEinde));

	//	return _fieldsExpandFilterService.ApplyScope(verblijfstitelHistorieResponse, string.Join(",", model.fields), _fieldsSettings.GbaFieldsSettings);
	//}
}