using Rvig.HaalCentraalApi.Shared.ApiModels.Universal;

namespace Rvig.Data.Historie.Services;

public class HistoryResult<T>
{
	public List<T>? MappedHistoryObjects { get; set; }
	public long? PlId { get; set; }
	public int AfnemerCode { get; set; }
	public GbaOpschortingBijhouding? OpschortingBijhouding { get; set; }
	public int? GeheimhoudingPersoonsgegevens { get; set; }
}