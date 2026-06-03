using Rvig.HaalCentraalApi.Shared.Interfaces;
using Rvig.HaalCentraalApi.Historie.Interfaces;
using Rvig.HaalCentraalApi.Shared.Validation;
using Rvig.HaalCentraalApi.Shared.Services;
using Rvig.HaalCentraalApi.Shared.Fields;

namespace Rvig.HaalCentraalApi.Historie.Services;

public abstract class GbaHistorieApiServiceBase : BaseApiService
{
	protected override FieldsSettings _fieldsSettings => throw new NotImplementedException();
	protected IGetAndMapGbaHistorieService _getAndMapHistorieService;

	protected GbaHistorieApiServiceBase(IGetAndMapGbaHistorieService getAndMapHistorieService, IDomeinTabellenRepo domeinTabellenRepo)
		: base(domeinTabellenRepo)
	{
		_getAndMapHistorieService = getAndMapHistorieService;
	}

	/// <summary>
	/// Get history data based on given types and bsn.
	/// Uses getHistoryFunc as given method unique for each given type to retrieve the history data.
	/// </summary>
	/// <typeparam name="TResponseObject">Expected response object type</typeparam>
	/// <param name="getHistoryFunc"></param>
	protected static Task<TResponseObject> GetHistoryBase<TResponseObject>(string? burgerservicenummer, DateTime? dateFrom, DateTime? dateTo, Func<string, DateTime?, DateTime?, Task<TResponseObject>> getHistoryFunc)
	{
		// Validation
		if (string.IsNullOrEmpty(burgerservicenummer))
		{
			return Task.FromResult(Activator.CreateInstance<TResponseObject>());
		}
		ValidationHelperBase.ValidateBurgerservicenummers(new List<string> { burgerservicenummer });

		// Get history
		return getHistoryFunc(burgerservicenummer, dateFrom, dateTo);
	}
}
