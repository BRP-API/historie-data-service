using System.Runtime.Serialization;

namespace Rvig.HaalCentraalApi.Historie.RequestModels.Historie
{
	[DataContract]
    public class RaadpleegMetPeildatum : HistorieQuery
    {
		/// <summary>
		/// De datum waarop de resource wordt opgevraagd.
		/// </summary>
		/// <value>De datum waarop de resource wordt opgevraagd. </value>
		[DataMember(Name="peildatum", EmitDefaultValue=false)]
        public string? peildatum { get; set; }
    }
}
