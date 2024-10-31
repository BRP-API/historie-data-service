﻿using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using Rvig.HaalCentraalApi.Shared.Helpers;
using Rvig.HaalCentraalApi.Shared.Options;

namespace Rvig.Data.Base.Postgres.Repositories;

public abstract class RvigRepoPostgresBase<T> : PostgresSqlQueryRepoBase<T> where T : class, new()
{
	protected RvigRepoPostgresBase(IOptions<DatabaseOptions> databaseOptions, ILoggingHelper loggingHelper) : base(databaseOptions, loggingHelper)
	{
		SetWhereMappings();
		SetMappings();
	}

	/// <summary>
	/// Get data based on pl id(s) via Dapper.
	/// </summary>
	/// <param name="connection"></param>
	/// <param name="pl_ids"></param>
	protected async Task<List<TDataObject>> GetPersoonDataByPlIds<TDataObject>(IEnumerable<long> pl_ids, string queryBase, string alias)
	{
		var whereStringAndParams = GetPlIdsWhere(pl_ids, alias);
		var dynamicParameters = new DynamicParameters();
		whereStringAndParams.parameters.ForEach(param => dynamicParameters.Add(param.ParameterName, param.NpgsqlValue));

		return await GetDataViaDapper<TDataObject>(queryBase, dynamicParameters, whereStringAndParams.where);
	}

	/// <summary>
	/// Create a where clause based on plIds.
	/// </summary>
	/// <param name="pl_ids"></param>
	/// <param name="alias"></param>
	/// <returns></returns>
	protected static (string where, List<NpgsqlParameter> parameters) GetPlIdsWhere(IEnumerable<long> pl_ids, string alias)
	{
		if (pl_ids.Count() == 1)
		{
			return ($"{alias}.pl_id = @PL_ID", new List<NpgsqlParameter>
			{
				new NpgsqlParameter($"PL_ID", pl_ids.Single())
			});
		}
		else
		{
			(string where, List<NpgsqlParameter> parameters) whereStringAndParams = ("", new List<NpgsqlParameter>());
			var plIdIndex = 0;
			var pldIdConditions = pl_ids.Select(x =>
			{
				plIdIndex++;
				whereStringAndParams.parameters.Add(new NpgsqlParameter($"PL_ID{plIdIndex}", x));
				return $"@PL_ID{plIdIndex}";
			});

			whereStringAndParams.where = $"{alias}.pl_id in ({string.Join(", ", pldIdConditions)})";
			return whereStringAndParams;
		}
	}
}