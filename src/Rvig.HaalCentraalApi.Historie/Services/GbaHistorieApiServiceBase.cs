using Rvig.HaalCentraalApi.Shared.Interfaces;
using Rvig.HaalCentraalApi.Historie.Interfaces;
using Rvig.HaalCentraalApi.Historie.RequestModels.Historie;
using Rvig.HaalCentraalApi.Shared.Validation;
using Rvig.HaalCentraalApi.Shared.Services;
using Rvig.HaalCentraalApi.Shared.Exceptions;
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

	/// <summary>
	/// Filter response by model given dates.
	/// </summary>
	/// <typeparam name="TListItem"></typeparam>
	/// <param name="resultList"></param>
	/// <param name="model"></param>
	/// <param name="beginDatePropName"></param>
	/// <param name="endDatePropName"></param>
	/// <exception cref="InvalidOperationException"></exception>
	protected static List<TListItem>? FilterResponseByDates<TListItem>(List<TListItem> resultList, HistorieQuery model, string? beginDatePropName, string? endDatePropName) where TListItem : class
	{
		// Filter response by date and fields
		if (model is RaadpleegMetPeildatum raadpleegMetPeildatum)
		{
			_ = DateTime.TryParse(raadpleegMetPeildatum.peildatum, out var peildatum);
			return FilterByPeildatum(peildatum == DateTime.MinValue ? null : peildatum, resultList, beginDatePropName, endDatePropName);
		}
		else if (model is RaadpleegMetPeriode raadpleegMetPeriode)
		{
			_ = DateTime.TryParse(raadpleegMetPeriode.datumVan, out var datumVan);
			_ = DateTime.TryParse(raadpleegMetPeriode.datumTot, out var datumTot);
			return FilterByDatumVanDatumTot(datumVan == DateTime.MinValue ? null : datumVan, datumTot == DateTime.MinValue ? null : datumTot, resultList, beginDatePropName, endDatePropName);
		}
		else
		{
			throw new CustomInvalidOperationException($"Onbekend type query: {model}");
		}
	}
}
