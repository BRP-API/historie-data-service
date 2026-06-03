using Microsoft.Extensions.Options;
using Rvig.HaalCentraalApi.Shared.Options;

namespace Rvig.Data.Base.Postgres.Repositories;

public abstract class RvigRepoPostgresBase<T> : PostgresSqlQueryRepoBase<T> where T : class, new()
{
	protected RvigRepoPostgresBase(IOptions<DatabaseOptions> databaseOptions) : base(databaseOptions)
	{
		SetWhereMappings();
		SetMappings();
	}
}