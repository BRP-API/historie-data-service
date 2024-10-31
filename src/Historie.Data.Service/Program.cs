using Rvig.Base.App;
using System.Collections.Generic;
using System;
using Rvig.HaalCentraalApi.Historie.Services;
using Rvig.Data.Historie.Mappers;
using Rvig.Data.Historie.Repositories;
using Rvig.Data.Historie.Services;
using Rvig.HaalCentraalApi.Historie.Interfaces;
using Rvig.HaalCentraalApi.Historie.Validation.RequestModelValidators;
using Microsoft.AspNetCore.Builder;

var servicesDictionary = new Dictionary<Type, Type>
{
	// Data
	{ typeof(IRvigHistorieRepo), typeof(DbHistorieRepo) },
	{ typeof(IRvIGDataHistorieMapper), typeof(RvIGDataHistorieMapper) },
	{ typeof(IGetAndMapGbaHistorieService), typeof(GetAndMapGbaHistorieService) },

	// API
	//{ typeof(IGbaNationaliteitHistorieApiService), typeof(GbaNationaliteitHistorieApiService) },
	//{ typeof(IGbaPartnerHistorieApiService), typeof(GbaPartnerHistorieApiService) },
	{ typeof(IGbaVerblijfplaatsHistorieApiService), typeof(GbaVerblijfplaatsHistorieApiService) },
	//{ typeof(IGbaVerblijfstitelHistorieApiService), typeof(GbaVerblijfstitelHistorieApiService) }
};

var validatorList = new List<Type>
{
	typeof(RaadpleegMetPeildatumValidator),
	typeof(RaadpleegMetPeriodeValidator)
};

RvigBaseApp.Init(servicesDictionary, validatorList, (WebApplicationBuilder _) => false, "BRP Historie API");