using Rvig.Data.Base.Authorisation;

namespace Rvig.Data.Historie.DatabaseModels;

/// <summary>
/// Combine all different database persoon parts that represent one haalcentraal ingeschreven persoon into one class.
/// </summary>
public class DbVerblijfplaatsHistorieWrapper
{
	public long? pl_id { get; set; }
	[RubriekCategory(7), RubriekElement("67.10")] public int? bijhouding_opschort_datum { get; set; }
	[RubriekCategory(7), RubriekElement("67.20")] public string? bijhouding_opschort_reden { get; set; }
	[RubriekCategory(7), RubriekElement("70.10")] public short? geheim_ind { get; set; }
	[RubriekCategory(1, 51)] public IEnumerable<verblijfplaats_voorkomen> VerblijfplaatsVoorkomens { get; set; }

	public DbVerblijfplaatsHistorieWrapper()
	{
		VerblijfplaatsVoorkomens = new List<verblijfplaats_voorkomen>();
	}
}
