using System.Runtime.Serialization;
using System.Xml.Serialization;
using ServiceStack.ServiceHost;

namespace catalogue.ServiceModel
{
	[XmlRoot(ElementName = "track")]
	[DataContract(Name = "track")]
	[RestService("/track/details/{Id}")]
	public class Track
	{
		public const string TYPE = "track";

		[XmlElement(ElementName = "url", IsNullable = true)]
		[DataMember(Name = "url")]
		public string Url { get; set; }

		[XmlElement(ElementName = "trackNumber", IsNullable = true)]
		[DataMember(Name = "trackNumber")]
		public int? TrackNumber { get; set; }

		[XmlElement(ElementName = "discNumber", IsNullable = true)]
		[DataMember(Name = "discNumber")]
		public int? DiskNumber { get; set; }

		[XmlAttribute(AttributeName = "id")]
		[DataMember(Name = "id")]
		public int TrackId { get; set; }

		[XmlElement(ElementName = "title")]
		[DataMember(Name = "title")]
		public string Title { get; set; }

		[XmlElement(ElementName = "version")]
		[DataMember(Name = "version")]
		public string VersionName { get; set; }

		[XmlElement(ElementName = "release", IsNullable = true)]
		[DataMember(Name = "release")]
		public Release Release { get; set; }

		[XmlElement(ElementName = "artist")]
		[DataMember(Name = "artist")]
		public Artist Artist { get; set; }

	}
}