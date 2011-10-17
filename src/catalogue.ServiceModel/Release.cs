using System.Runtime.Serialization;
using System.Xml.Serialization;
using ServiceStack.ServiceHost;

namespace catalogue.ServiceModel
{
	[XmlRoot(ElementName = "release")]
	[DataContract(Name = "release")]
	[RestService("/release/details/{ReleaseId}")]
	public class Release
	{
		public const string TYPE = "release";

		[XmlAttribute(AttributeName = "id")]
		[DataMember(Name = "id")]
		public int ReleaseId { get; set; }

		[XmlElement(ElementName = "title")]
		[DataMember(Name = "title")]
		public string Title { get; set; }

		[XmlElement(ElementName = "version", IsNullable = true)]
		[DataMember(Name = "version")]
		public string Version { get; set; }

		[XmlElement(ElementName = "type")]
		[DataMember(Name = "type")]
		public string ReleaseType { get; set; }

		[XmlElement(ElementName = "barcode", IsNullable = true)]
		[DataMember(Name = "barcode")]
		public string Barcode { get; set; }

		[XmlElement(ElementName = "year")]
		[DataMember(Name = "year")]
		public string Year { get; set; }

		[XmlElement(ElementName = "explicitContent")]
		[DataMember(Name = "explicitContent")]
		public bool ExplicitContent { get; set; }

		[XmlElement(ElementName = "artist")]
		[DataMember(Name = "artist")]
		public Artist ReleaseArtist { get; set; }

		[XmlElement(ElementName = "url", IsNullable = true)]
		[DataMember(Name = "url")]
		public string Url { get; set; }

		[XmlElement(ElementName = "image", IsNullable = true)]
		[DataMember(Name = "image")]
		public string ImageUrl { get; set; }

		[XmlElement(ElementName = "releaseDate", IsNullable = true)]
		[DataMember(Name = "releaseDate")]
		public string ReleaseDate { get; set; }

		[XmlElement(ElementName = "addedDate")]
		[DataMember(Name = "addedDate")]
		public string DateAdded { get; set; }

		[XmlElement(ElementName = "popularity")]
		[DataMember(Name = "popularity")]
		public float Popularity { get; set; }
	}
}