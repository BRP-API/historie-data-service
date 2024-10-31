using Rvig.HaalCentraalApi.Shared.Interfaces;
using Rvig.HaalCentraalApi.Historie.Interfaces;

namespace Rvig.HaalCentraalApi.Historie.Services;

public interface IGbaPartnerHistorieApiService
{
	//Task<PartnerhistorieQueryResponse> GetPartnerHistorie(HistorieQuery model);
}
public class GbaPartnerHistorieApiService : GbaHistorieApiServiceBase, IGbaPartnerHistorieApiService
{
	public GbaPartnerHistorieApiService(IGetAndMapGbaHistorieService getAndMapHistorieService, IDomeinTabellenRepo domeinTabellenRepo)
		: base(getAndMapHistorieService, domeinTabellenRepo)
	{
	}

	///// <summary>
	///// Get partner history via search params (child of HistorieQuery)
	///// </summary>
	///// <param name="model"></param>
	///// <returns>Response object with list of partner history data</returns>
	//public async Task<PartnerhistorieQueryResponse> GetPartnerHistorie(HistorieQuery model)
	//{
	//	// Validate + Get history
	//	var partnerHistorieResponse = await GetHistoryBase(model.burgerservicenummer, model.fields, _fieldsSettings.GbaFieldsSettings, _getAndMapHistorieService.GetPartnerHistorieMapByBsn);

	//	if (partnerHistorieResponse.Partnerhistorie?.Any() != true)
	//	{
	//		return new PartnerhistorieQueryResponse();
	//	}

	//	// Filter response by date and fields
	//	partnerHistorieResponse.Partnerhistorie = FilterResponseByDates(partnerHistorieResponse.Partnerhistorie, model, nameof(GbaPartnerHistorie._datumAangaanHuwelijkPartnerschap), nameof(GbaPartnerHistorie._datumOntbindingHuwelijkPartnerschap));

	//	return _fieldsExpandFilterService.ApplyScope(partnerHistorieResponse, string.Join(",", model.fields), _fieldsSettings.GbaFieldsSettings);
	//}
}
