using Rvig.HaalCentraalApi.Historie.ApiModels.Historie;
using Rvig.HaalCentraalApi.Shared.Interfaces;
using Rvig.HaalCentraalApi.Historie.Interfaces;
using Rvig.HaalCentraalApi.Historie.RequestModels.Historie;
using Rvig.HaalCentraalApi.Historie.ResponseModels.Historie;
using Rvig.HaalCentraalApi.Shared.Exceptions;
using Rvig.HaalCentraalApi.Shared.ApiModels.Universal;
using System.Globalization;

namespace Rvig.HaalCentraalApi.Historie.Services;

public interface IGbaVerblijfplaatsHistorieApiService
{
	Task<(VerblijfplaatshistorieQueryResponse verblijfplaatshistorieResponse, List<long>? plIds)> GetVerblijfplaatsHistorie(HistorieQuery model);
}
public class GbaVerblijfplaatsHistorieApiService : GbaHistorieApiServiceBase, IGbaVerblijfplaatsHistorieApiService
{
	public GbaVerblijfplaatsHistorieApiService(IGetAndMapGbaHistorieService getAndMapHistorieService, IDomeinTabellenRepo domeinTabellenRepo)
		: base(getAndMapHistorieService, domeinTabellenRepo)
	{
	}

	/// <summary>
	/// Get verblijfplaats history via search params (child of HistorieQuery)
	/// </summary>
	/// <param name="model"></param>
	/// <returns>Response object with list of verblijfplaats history data</returns>
	public async Task<(VerblijfplaatshistorieQueryResponse verblijfplaatshistorieResponse, List<long>? plIds)> GetVerblijfplaatsHistorie(HistorieQuery model)
	{
		// Validate + Get history
		ValidateDateParams(model);

		var (dateFrom, dateTo) = GetDateParamsFromModel(model);
		var (HistoryResponse, AfnemerCode) = await GetHistoryBase(model.burgerservicenummer, dateFrom, dateTo, _getAndMapHistorieService.GetVerblijfplaatsHistorieMapByBsn);

		var plIds = new List<long>();
		if (	HistoryResponse!._PlId.HasValue)
		{
			plIds.Add(HistoryResponse!._PlId.Value);
		}

		if (HistoryResponse.OpschortingBijhouding?.Reden?.Code?.ToLower()?.Equals("f") == true)
		{
			HistoryResponse.Verblijfplaatsen = Enumerable.Empty<GbaVerblijfplaatsVoorkomen>().ToList();
		}

		VerblijfplaatshistorieQueryResponse verblijfplaatshistorieQueryResponse = HistoryResponse.Verblijfplaatsen?.Any() == true
			? HistoryResponse
			: new VerblijfplaatshistorieQueryResponse { Verblijfplaatsen = new List<GbaVerblijfplaatsVoorkomen>(), GeheimhoudingPersoonsgegevens = HistoryResponse.GeheimhoudingPersoonsgegevens, OpschortingBijhouding = HistoryResponse.OpschortingBijhouding };

		return (verblijfplaatshistorieQueryResponse, plIds);
	}

	/// <summary>
	/// Validate given dates. Especially if datumVan and datumTot are filled then datumVan may not be equal or after datumTot.
	/// </summary>
	/// <param name="model"></param>
	/// <exception cref="InvalidParamsException"></exception>
	private static void ValidateDateParams(HistorieQuery model)
	{
		var invalidParams = new List<InvalidParams>();
		var periodeModel = model as RaadpleegMetPeriode;
		if (model is RaadpleegMetPeildatum peildatumModel && !DateTime.TryParseExact(peildatumModel.peildatum, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime _))
		{
			invalidParams.Add(new InvalidParams { Code = "date", Name = nameof(peildatumModel.peildatum), Reason = "Waarde is geen geldige datum." });
		}
		else if (periodeModel != null && !DateTime.TryParseExact(periodeModel.datumVan, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime _))
		{
			invalidParams.Add(new InvalidParams { Code = "date", Name = nameof(periodeModel.datumVan), Reason = "Waarde is geen geldige datum." });
		}
		else if (periodeModel != null && !DateTime.TryParseExact(periodeModel.datumTot, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime _))
		{
			invalidParams.Add(new InvalidParams { Code = "date", Name = nameof(periodeModel.datumTot), Reason = "Waarde is geen geldige datum." });
		}
		else if (periodeModel != null && DateTime.ParseExact(periodeModel.datumVan!, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(periodeModel.datumTot!, "yyyy-MM-dd", CultureInfo.InvariantCulture))
		{
			invalidParams.Add(new InvalidParams { Code = "date", Name = "datumTot", Reason = "datumTot moet na datumVan liggen." });
		}

		if (invalidParams.Any())
		{
			throw new InvalidParamsException(invalidParams);
		}
	}

	/// <summary>
	/// Get dates from model to create a period. Peildatum only received one date so the dateTo value will be peildatum + 1 day.
	/// </summary>
	/// <param name="model"></param>
	/// <returns></returns>
	/// <exception cref="CustomInvalidOperationException"></exception>
	private static (DateTime? dateFrom, DateTime? dateTo) GetDateParamsFromModel(HistorieQuery model)
	{
		return model switch
		{
			RaadpleegMetPeildatum peildatumModel => GetDatesFromPeildatumModel(peildatumModel),
			RaadpleegMetPeriode periodeModel => (DateTime.ParseExact(periodeModel?.datumVan!, "yyyy-MM-dd", CultureInfo.InvariantCulture), DateTime.ParseExact(periodeModel?.datumTot!, "yyyy-MM-dd", CultureInfo.InvariantCulture)),
			_ => throw new CustomInvalidOperationException($"Onbekend type query: {model}"),
		};

		static (DateTime, DateTime) GetDatesFromPeildatumModel(RaadpleegMetPeildatum peildatumModel)
		{
			var peildatum = DateTime.ParseExact(peildatumModel?.peildatum!, "yyyy-MM-dd", CultureInfo.InvariantCulture);
			return (peildatum, peildatum.AddDays(1));
		}
	}
}
