using System.Runtime.Serialization;

namespace catalogue.ServiceModel
{
	[DataContract(Name = "error")]
	public class Error
	{
		[DataMember(Name = "errorCode")]
		public int ErrorCode { get; set; }
		[DataMember(Name = "errorMessage")]
		public string ErrorMessage { get; set; }
	}
}
