using Rvig.Base.App;
using System.Collections.Generic;
using System;
using Rvig.HaalCentraalApi.Historie.Services;
using Rvig.Data.Historie.Mappers;
using Rvig.Data.Historie.Repositories;
using Rvig.Data.Historie.Services;
using Rvig.HaalCentraalApi.Historie.Interfaces;

var servicesDictionary = new Dictionary<Type, Type>
{
	// Data
	{ typeof(IRvigHistorieRepo), typeof(DbHistorieRepo) },
	{ typeof(IRvIGDataHistorieMapper), typeof(RvIGDataHistorieMapper) },
	{ typeof(IGetAndMapGbaHistorieService), typeof(GetAndMapGbaHistorieService) },

	// API
	{ typeof(IGbaVerblijfplaatsHistorieApiService), typeof(GbaVerblijfplaatsHistorieApiService) },
};

RvigBaseApp.Init(servicesDictionary, "BRP Historie API");