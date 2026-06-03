using Microsoft.Extensions.Options;
using Npgsql;
using Rvig.HaalCentraalApi.Shared.Options;

namespace Rvig.Data.Base.Postgres.Repositories;
public abstract class PostgresSqlQueryRepoBase<T>(IOptions<DatabaseOptions> databaseOptions)
	: PostgresRepoBase(databaseOptions) where T : class, new()
{

    /// <summary>
    /// Datasourcename (columnname), propertyname (in csharp class)
    /// </summary>
    protected IDictionary<string, string> Mappings { get; set; } = new Dictionary<string, string>();
	protected IDictionary<string, string> WhereMappings { get; set; } = new Dictionary<string, string>();

	/// <summary>
	/// Override and set mappings from database columnnames to propertynames of table class.
	/// </summary>
	protected abstract void SetMappings();
	protected abstract void SetWhereMappings();

	protected static NpgsqlCommand CreateDbCommand(string query) => new(query);

	protected static IDictionary<string, string> CreateMappingsFromWhereMappings(IDictionary<string, string> whereMappings)
	{
		return whereMappings.ToDictionary(whereMapping =>
		{
			if (whereMapping.Key.Contains(" AS ", StringComparison.OrdinalIgnoreCase))
			{
				return whereMapping.Key.Substring(whereMapping.Key.LastIndexOf(" AS ", StringComparison.OrdinalIgnoreCase) + 4);
			}
			return whereMapping.Key.Substring(whereMapping.Key.IndexOf('.') + 1);
		}, whereMapping => whereMapping.Value);
	}

	protected void CreateMappingsFromWhereMappings()
	{
		Mappings = CreateMappingsFromWhereMappings(WhereMappings);
	}

	protected virtual async Task<IEnumerable<T>> GetFilterResultAsync(NpgsqlCommand command, IDictionary<string, string>? additionalMappings = null)
	{
		var mergedMappings = additionalMappings != null ? Mappings.Concat(additionalMappings) : Mappings;
		var records = new List<T>();

		using var connection = GetConnection();

		await OpenConnectionAndLog(connection);

		command.Connection = connection;

		var reader = await command.ExecuteReaderAsync();
		while (await reader.ReadAsync())
		{
			var record = Activator.CreateInstance<T>();

			foreach (var mapping in mergedMappings)
			{
				if (!ResultContainsColumn(reader, mapping.Key))
				{
					continue;
				}
				var columnValue = reader[mapping.Key];
				if (columnValue != null && columnValue is not DBNull)
				{
					SetValue(mapping.Value, record, columnValue);
				}
			}

			records.Add(record);
		}

        return records;
	}

	/// <summary>
	/// Map value of column from query result to C# object property.
	/// </summary>
	/// <param name="propPath"></param>
	/// <param name="parent"></param>
	/// <param name="value"></param>
	private static void SetValue(string propPath, object? parent, object? value)
	{
		if (parent != null)
		{
			if (!propPath.Contains('.'))
			{
				parent?.GetType()?.GetProperty(propPath)?.SetValue(parent, value);
			}
			else
			{
				var propertyParts = propPath.Split(".");
				foreach (var part in propertyParts)
				{
					if (propertyParts.Count().Equals(part))
					{
						parent?.GetType()?.GetProperty(part)?.SetValue(parent, value);
					}
					else
					{
						parent = parent?.GetType()?.GetProperty(part)?.GetValue(parent);
					}
				}
			}
		}
	}

	/// <summary>
	/// Used for mapping query result to C# object.
	/// </summary>
	/// <param name="reader"></param>
	/// <param name="columnName"></param>
	/// <returns></returns>
	private static bool ResultContainsColumn(NpgsqlDataReader reader, string columnName)
	{
		for (var i = 0; i < reader.FieldCount; i++)
		{
			var paramName = reader.GetName(i);

			if (string.Equals(columnName, paramName, StringComparison.CurrentCultureIgnoreCase))
			{
				return true;
			}
		}

		return false;
	}
}