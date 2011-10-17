using System;
using catalogue.ServiceModel;

namespace catalogue.Service
{
	public class StatusService : ErrorHandlingRestServiceBase<Status>
	{
		public override object OnGet(Status request) {
			request.CurrentTime = DateTime.Now;
			return request;
		}
	}
}