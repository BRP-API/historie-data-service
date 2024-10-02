using Rvig.HaalCentraalApi.Historie.RequestModels.Historie;
using Rvig.HaalCentraalApi.Shared.Util;

namespace Rvig.HaalCentraalApi.Historie.Util
{
	/// <summary>
	/// Used to convert HistorieQuery request body into one of its subTypes when deserializing.
	/// </summary>
	public class HistorieQueryJsonInheritanceConverter : QueryBaseJsonInheritanceConverter
	{
		public HistorieQueryJsonInheritanceConverter()
		{
		}

		public HistorieQueryJsonInheritanceConverter(string discriminatorName) : base(discriminatorName)
		{
		}

		public HistorieQueryJsonInheritanceConverter(Type baseType) : base(baseType)
		{
		}

		public HistorieQueryJsonInheritanceConverter(string discriminatorName, bool readTypeProperty) : base(discriminatorName, readTypeProperty)
		{
		}

		public HistorieQueryJsonInheritanceConverter(Type baseType, string discriminatorName) : base(baseType, discriminatorName)
		{
		}

		public HistorieQueryJsonInheritanceConverter(Type baseType, string discriminatorName, bool readTypeProperty) : base(baseType, discriminatorName, readTypeProperty)
		{
		}

		protected override List<string> _subTypes => new()
		{
			nameof(RaadpleegMetPeildatum),
			nameof(RaadpleegMetPeriode)
		};
		protected override string _discriminator => nameof(HistorieQuery.type);
	}
}