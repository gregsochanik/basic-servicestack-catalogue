using System.Runtime.Serialization;
using System.Xml.Serialization;
using ServiceStack.ServiceHost;

namespace catalogue.ServiceModel {
	
	[XmlRoot(ElementName = "artist")]
	[DataContract(Name="artist")]
	[RestService("/artist/details/{Id}")]
	public class Artist {
		[DataMember(Name = "id")]
		public int Id { get; set; }

		[DataMember(Name="name")]
		public string Name { get; set; }

		[DataMember(Name = "sortName")]
		public string SortName { get; set; }

		[DataMember(Name = "url")]
		public string Url { get; set; }

		public string Image { get; set; }
		
		public string Bio { get; set; }
	}
}
