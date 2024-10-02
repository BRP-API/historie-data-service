using Rvig.HaalCentraalApi.Shared.Interfaces;
using Rvig.HaalCentraalApi.Historie.Interfaces;

namespace Rvig.HaalCentraalApi.Historie.Services;
public interface IGbaNationaliteitHistorieApiService
{
	//Task<NationalisatiehistorieQueryResponse> GetNationaliteitHistorie(HistorieQuery model);
}
public class GbaNationaliteitHistorieApiService : GbaHistorieApiServiceBase, IGbaNationaliteitHistorieApiService
{
	public GbaNationaliteitHistorieApiService(IGetAndMapGbaHistorieService getAndMapHistorieService, IDomeinTabellenRepo domeinTabellenRepo)
		: base(getAndMapHistorieService, domeinTabellenRepo)
	{
	}

	/// <summary>
	/// Get nationaliteit history via search params (child of HistorieQuery)
	/// </summary>
	/// <returns>Response object with list of nationaliteit history data</returns>
	//public async Task<NationalisatiehistorieQueryResponse> GetNationaliteitHistorie(HistorieQuery model)
	//{
	//	// Validate + Get history
	//	var nationaliteitHistorieResponse = await GetHistoryBase(model.burgerservicenummer, model.fields, _fieldsSettings.GbaFieldsSettings, _getAndMapHistorieService.GetNationaliteitHistorieMapByBsn);

	//	if (nationaliteitHistorieResponse.Nationaliteithistorie?.Any() != true)
	//	{
	//		return new NationalisatiehistorieQueryResponse();
	//	}

	//	// Filter response by date and fields
	//	nationaliteitHistorieResponse.Nationaliteithistorie = FilterResponseByDates(nationaliteitHistorieResponse.Nationaliteithistorie, model, nameof(GbaNationaliteitHistorie._datumOpneming), null);

	//	return _fieldsExpandFilterService.ApplyScope(nationaliteitHistorieResponse, string.Join(",", model.fields), _fieldsSettings.GbaFieldsSettings);
	//}
}
