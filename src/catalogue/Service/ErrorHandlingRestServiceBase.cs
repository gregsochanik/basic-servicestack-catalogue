using System;
using System.Net;
using ServiceStack.Common.Web;
using ServiceStack.ServiceInterface;

namespace catalogue.Service
{
	public abstract class ErrorHandlingRestServiceBase<T> : RestServiceBase<T>
	{
		protected override object HandleException(T request, Exception ex) {
			if (ex is NotImplementedException)
				return new HttpResult(ex) { StatusCode = HttpStatusCode.MethodNotAllowed };

			return base.HandleException(request, ex);
		}
	}
}