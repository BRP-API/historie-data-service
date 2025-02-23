using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Rvig.HaalCentraalApi.Shared.ApiModels.Universal
{
	[DataContract]
	public class GbaVerblijfplaatsBeperkt
	{
		/// <summary>
		/// Gets or Sets Straat
		/// </summary>
		[MaxLength(80)]
		[DataMember(Name = "straat", EmitDefaultValue = false)]
		public string? Straat { get; set; }

		/// <summary>
		/// Een nummer dat door de gemeente aan een adresseerbaar object is gegeven.
		/// </summary>
		/// <value>Een nummer dat door de gemeente aan een adresseerbaar object is gegeven. </value>
		[Range(1, 99999)]
		[DataMember(Name = "huisnummer", EmitDefaultValue = false)]
		public int? Huisnummer { get; set; }

		/// <summary>
		/// Een toevoeging aan een huisnummer in de vorm van een letter die door de gemeente aan een adresseerbaar object is gegeven.
		/// </summary>
		/// <value>Een toevoeging aan een huisnummer in de vorm van een letter die door de gemeente aan een adresseerbaar object is gegeven. </value>
		[RegularExpression("^[a-zA-Z]{1}$")]
		[DataMember(Name = "huisletter", EmitDefaultValue = false)]
		public string? Huisletter { get; set; }

		/// <summary>
		/// Een toevoeging aan een huisnummer of een combinatie van huisnummer en huisletter die door de gemeente aan een adresseerbaar object is gegeven.
		/// </summary>
		/// <value>Een toevoeging aan een huisnummer of een combinatie van huisnummer en huisletter die door de gemeente aan een adresseerbaar object is gegeven. </value>
		[RegularExpression(@"^[a-zA-Z0-9 \-]{1,4}$")]
		[DataMember(Name = "huisnummertoevoeging", EmitDefaultValue = false)]
		public string? Huisnummertoevoeging { get; set; }

		/// <summary>
		/// Gets or Sets AanduidingBijHuisnummer
		/// </summary>
		[DataMember(Name = "aanduidingBijHuisnummer", EmitDefaultValue = false)]
		public Waardetabel? AanduidingBijHuisnummer { get; set; }

		/// <summary>
		/// De door PostNL vastgestelde code die bij een bepaalde combinatie van een straatnaam en een huisnummer hoort.
		/// </summary>
		/// <value>De door PostNL vastgestelde code die bij een bepaalde combinatie van een straatnaam en een huisnummer hoort. </value>
		[RegularExpression("^[1-9]{1}[0-9]{3}[ ]?[A-Za-z]{2}$")]
		[DataMember(Name = "postcode", EmitDefaultValue = false)]
		public string? Postcode { get; set; }

		/// <summary>
		/// Een woonplaats is een gedeelte van het grondgebied van de gemeente met een naam.
		/// </summary>
		/// <value>Een woonplaats is een gedeelte van het grondgebied van de gemeente met een naam. </value>
		[RegularExpression(@"^[a-zA-Z0-9À-ž \(\)\,\.\-\']{1,80}$")]
		[DataMember(Name = "woonplaats", EmitDefaultValue = false)]
		public string? Woonplaats { get; set; }

		/// <summary>
		/// Omschrijving van de ligging van een verblijfsobject, standplaats of ligplaats.
		/// </summary>
		/// <value>Omschrijving van de ligging van een verblijfsobject, standplaats of ligplaats. </value>
		[MaxLength(35)]
		[DataMember(Name = "locatiebeschrijving", EmitDefaultValue = false)]
		public string? Locatiebeschrijving { get; set; }

		/// <summary>
		/// Gets or Sets Land
		/// </summary>
		[DataMember(Name = "land", EmitDefaultValue = false)]
		public Waardetabel? Land { get; set; }

		/// <summary>
		/// Het eerste deel van een buitenlands adres. Vaak is dit een combinatie van de straat en huisnummer.
		/// </summary>
		/// <value>Het eerste deel van een buitenlands adres. Vaak is dit een combinatie van de straat en huisnummer. </value>
		[MaxLength(40)]
		[DataMember(Name = "regel1", EmitDefaultValue = false)]
		public string? Regel1 { get; set; }

		/// <summary>
		/// Het tweede deel van een buitenlands adres. Vaak is dit een combinatie van woonplaats eventueel in combinatie met de postcode.
		/// </summary>
		/// <value>Het tweede deel van een buitenlands adres. Vaak is dit een combinatie van woonplaats eventueel in combinatie met de postcode. </value>
		[MaxLength(50)]
		[DataMember(Name = "regel2", EmitDefaultValue = false)]
		public string? Regel2 { get; set; }

		/// <summary>
		/// Het derde deel van een buitenlands adres is optioneel. Het gaat om een of meer geografische gebieden van het adres in het buitenland.
		/// </summary>
		/// <value>Het derde deel van een buitenlands adres is optioneel. Het gaat om een of meer geografische gebieden van het adres in het buitenland. </value>
		[MaxLength(35)]
		[DataMember(Name = "regel3", EmitDefaultValue = false)]
		public string? Regel3 { get; set; }

		/// <summary>
		/// Gets or Sets InOnderzoek
		/// </summary>
		[DataMember(Name = "inOnderzoek", EmitDefaultValue = false)]
		public GbaInOnderzoek? InOnderzoek { get; set; }
	}
}
