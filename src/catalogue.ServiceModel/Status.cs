using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using ServiceStack.ServiceHost;

namespace catalogue.ServiceModel
{
	[XmlRoot(ElementName = "status")]
	[DataContract(Name = "status")]
	[RestService("/status")]
	public class Status
	{
		[DataMember(Name = "time")]
		public DateTime CurrentTime { get; set; }
	}
}