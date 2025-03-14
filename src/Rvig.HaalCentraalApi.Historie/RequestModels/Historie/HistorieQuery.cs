using System.Runtime.Serialization;
using Newtonsoft.Json;
using NJsonSchema.Converters;
using Rvig.HaalCentraalApi.Historie.Util;

namespace Rvig.HaalCentraalApi.Historie.RequestModels.Historie
{
	[DataContract]
    [JsonConverter(typeof(HistorieQueryJsonInheritanceConverter), "type")]
    [JsonInheritance("RaadpleegMetPeildatum", typeof(RaadpleegMetPeildatum))]
    [JsonInheritance("RaadpleegMetPeriode", typeof(RaadpleegMetPeriode))]
    public class HistorieQuery
    {
        /// <summary>
        /// Gets or Sets Type
        /// </summary>
        [DataMember(Name = "type", EmitDefaultValue = false)]
        public string? type { get; set; }

		/// <summary>
		/// Gets or Sets Burgerservicenummer
		/// </summary>
		[DataMember(Name = "burgerservicenummer", EmitDefaultValue = false)]
		public string? burgerservicenummer { get; set; }
	}
}
