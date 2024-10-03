using Microsoft.AspNetCore.Mvc;
using Rvig.HaalCentraalApi.Historie.RequestModels.Historie;
using Rvig.HaalCentraalApi.Historie.ResponseModels.Historie;
using Rvig.HaalCentraalApi.Shared.Validation;
using Rvig.HaalCentraalApi.Historie.Services;
using Rvig.HaalCentraalApi.Shared.Controllers;

namespace Rvig.HaalCentraalApi.Historie.Controllers;

[ApiController, Route("haalcentraal/api/brphistorie"), ValidateContentTypeHeader]
public class GbaApiHistorieController : GbaApiBaseController
{
	private readonly IGbaVerblijfplaatsHistorieApiService _gbaVerblijfplaatsHistorieApiService;

	public GbaApiHistorieController(IGbaVerblijfplaatsHistorieApiService gbaVerblijfplaatsHistorieApiService)
	{
		_gbaVerblijfplaatsHistorieApiService = gbaVerblijfplaatsHistorieApiService;
	}

	[HttpPost]
	[Route("verblijfplaatshistorie")]
	[ValidateUnusableQueryParams]
	public async Task<VerblijfplaatshistorieQueryResponse> GetVerblijfplaatsHistorie([FromBody] HistorieQuery model)
	{
		await ValidateUnusableQueryParams(model);
		(VerblijfplaatshistorieQueryResponse verblijfplaatshistorieResponse, List<long>? plIds) = await _gbaVerblijfplaatsHistorieApiService.GetVerblijfplaatsHistorie(model);
		AddPlIdsToResponseHeaders(plIds);

		return verblijfplaatshistorieResponse;
	}
}