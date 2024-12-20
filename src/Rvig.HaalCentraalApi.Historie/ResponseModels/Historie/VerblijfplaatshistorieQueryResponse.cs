using Rvig.HaalCentraalApi.Historie.ApiModels.Historie;
using Rvig.HaalCentraalApi.Shared.ApiModels.Universal;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Rvig.HaalCentraalApi.Historie.ResponseModels.Historie
{
    [DataContract]
    public class VerblijfplaatshistorieQueryResponse
	{
		/// <summary>
		/// Gets or Sets Verblijfplaatsen
		/// </summary>
		[DataMember(Name = "verblijfplaatsen", EmitDefaultValue = false)]
		public List<GbaVerblijfplaatsVoorkomen>? Verblijfplaatsen { get; set; }
		/// <summary>
		/// Gets or Sets Verblijfplaatsen
		/// </summary>
		[DataMember(Name = "geheimhoudingPersoonsgegevens", EmitDefaultValue = false)]
		public int? GeheimhoudingPersoonsgegevens { get; set; }

		/// <summary>
		/// Gets or Sets OpschortingBijhouding
		/// </summary>
		[DataMember(Name = "opschortingBijhouding", EmitDefaultValue = false)]
		public GbaOpschortingBijhouding? OpschortingBijhouding { get; set; }

		// Used for protocollering. DO NOT SERIALIZE AS PART OF THE API RESPONSE.
		[XmlIgnore, JsonIgnore]
		public long? _PlId { get; set; }
	}
}
