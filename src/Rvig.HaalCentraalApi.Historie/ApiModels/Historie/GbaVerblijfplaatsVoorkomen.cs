using Rvig.HaalCentraalApi.Shared.ApiModels.Universal;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Rvig.HaalCentraalApi.Historie.ApiModels.Historie
{
	[DataContract]
    public class GbaVerblijfplaatsVoorkomen : GbaVerblijfplaatsBeperkt
	{
        /// <summary>
        /// De verblijfplaats van de persoon kan een ligplaats, een standplaats of een verblijfsobject zijn.
        /// </summary>
        /// <value>De verblijfplaats van de persoon kan een ligplaats, een standplaats of een verblijfsobject zijn. </value>
        [RegularExpression("^[0-9]{16}$")]
        [DataMember(Name = "adresseerbaarObjectIdentificatie", EmitDefaultValue = false)]
        public string? AdresseerbaarObjectIdentificatie { get; set; }

        /// <summary>
        /// Unieke identificatie van een nummeraanduiding (en het bijbehorende adres) in de BAG.
        /// </summary>
        /// <value>Unieke identificatie van een nummeraanduiding (en het bijbehorende adres) in de BAG. </value>
        [RegularExpression("^[0-9]{16}$")]
        [DataMember(Name = "nummeraanduidingIdentificatie", EmitDefaultValue = false)]
        public string? NummeraanduidingIdentificatie { get; set; }

        /// <summary>
        /// Gets or Sets DatumAanvangAdresBuitenland
        /// </summary>
        [RegularExpression("^[0-9]{8}$")]
        [DataMember(Name = "datumAanvangAdresBuitenland", EmitDefaultValue = false)]
        public string? DatumAanvangAdresBuitenland { get; set; }

        /// <summary>
        /// Gets or Sets DatumAanvangVolgendeAdresBuitenland
        /// </summary>
        [RegularExpression("^[0-9]{8}$")]
        [DataMember(Name = "datumAanvangVolgendeAdresBuitenland", EmitDefaultValue = false)]
        public string? DatumAanvangVolgendeAdresBuitenland { get; set; }

        /// <summary>
        /// Gets or Sets DatumAanvangAdreshouding
        /// </summary>
        [RegularExpression("^[0-9]{8}$")]
        [DataMember(Name = "datumAanvangAdreshouding", EmitDefaultValue = false)]
        public string? DatumAanvangAdreshouding { get; set; }

        /// <summary>
        /// Gets or Sets DatumAanvangVolgendeAdreshouding
        /// </summary>
        [RegularExpression("^[0-9]{8}$")]
        [DataMember(Name = "datumAanvangVolgendeAdreshouding", EmitDefaultValue = false)]
        public string? DatumAanvangVolgendeAdreshouding { get; set; }

        /// <summary>
        /// Gets or Sets FunctieAdres
        /// </summary>
        [DataMember(Name = "functieAdres", EmitDefaultValue = false)]
        public Waardetabel? FunctieAdres { get; set; }

        /// <summary>
        /// Gets or Sets NaamOpenbareRuimte
        /// </summary>
        [MaxLength(80)]
        [DataMember(Name = "naamOpenbareRuimte", EmitDefaultValue = false)]
        public string? NaamOpenbareRuimte { get; set; }

        /// <summary>
        /// Gets or Sets GemeenteVanInschrijving
        /// </summary>
        [DataMember(Name = "gemeenteVanInschrijving", EmitDefaultValue = false)]
        public Waardetabel? GemeenteVanInschrijving { get; set; }

        /// <summary>
        /// Gets or Sets Rni
        /// </summary>
        [DataMember(Name = "rni", EmitDefaultValue = false)]
        public RniDeelnemer? Rni { get; set; }

        /// <summary>
        /// Gets or Sets InOnderzoekVolgendeVerblijfplaats
        /// </summary>
        [DataMember(Name = "inOnderzoekVolgendeVerblijfplaats", EmitDefaultValue = false)]
        public GbaInOnderzoek? InOnderzoekVolgendeVerblijfplaats { get; set; }

		/* Used for ordering list of GbaVerblijfplaatsVoorkomen in method OrderVerblijfplaatsVoorkomens in the GetAndMapGbaHistorieService class.*/
		[XmlIgnore, JsonIgnore]
		public DateTime? LatestStartAddressDate { get; set; }

		[XmlIgnore, JsonIgnore]
		public DateTime? NextLatestStartAddressDate { get; set; }

		[XmlIgnore, JsonIgnore]
		public DateTime? PreviousLatestStartAddressDate { get; set; }
	}
}
