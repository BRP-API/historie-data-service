using Rvig.HaalCentraalApi.Historie.ResponseModels.Historie;

namespace Rvig.HaalCentraalApi.Historie.Interfaces;

public interface IGetAndMapGbaHistorieService
{
    Task<(VerblijfplaatshistorieQueryResponse HistoryResponse, int AfnemerCode)> GetVerblijfplaatsHistorieMapByBsn(string burgerservicenummer, DateTime? dateFrom, DateTime? dateTo);
}
