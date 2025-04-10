using System.Runtime.Serialization;

namespace Rvig.HaalCentraalApi.Historie.RequestModels.Historie
{
	[DataContract]
    public class RaadpleegMetPeriode : HistorieQuery
    {
		/// <summary>
		/// De begindatum van de periode waarover de resource wordt opgevraagd.
		/// </summary>
		/// <value>De begindatum van de periode waarover de resource wordt opgevraagd. </value>
		[DataMember(Name="datumVan", EmitDefaultValue=false)]
        public string? datumVan { get; set; }

		/// <summary>
		/// De einddatum van de periode waarover de resource wordt opgevraagd.
		/// </summary>
		/// <value>De einddatum van de periode waarover de resource wordt opgevraagd. </value>
		[DataMember(Name="datumTot", EmitDefaultValue=false)]
        public string? datumTot { get; set; }
    }
}
