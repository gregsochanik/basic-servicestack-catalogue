using System;
using System.Net;
using System.Web.Routing;
using ServiceStack.CacheAccess;
using ServiceStack.Common;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using catalogue.Database.Repositories;
using catalogue.ServiceModel;
using SevenDigital.Api.Wrapper.Exceptions;

namespace catalogue.Service
{
	public class ReleaseDetailsService : ErrorHandlingRestServiceBase<Release>
	{
		private readonly IDbRepository<Release> _repository;
		private readonly ICacheClient _cacheClient;

		public ReleaseDetailsService(IDbRepository<Release> repository, ICacheClient cacheClient) {
			_repository = repository;
			_cacheClient = cacheClient;
		}

		public override object OnGet(Release request) {
			var cacheKey = UrnId.Create<Release>(request.ReleaseId.ToString());

			return ErrorCheck.NotFoundCheck(
					() => RequestContext.ToOptimizedResultUsingCache(_cacheClient, cacheKey, () => _repository.Get(request.ReleaseId))
				);
		}
	}

	public static class ErrorCheck
	{
		public static object NotFoundCheck(Func<object> action)
		{
			try
			{
				return action();
			}
			catch (ApiXmlException ex)
			{
				return new HttpResult(ex.Error) { StatusCode = HttpStatusCode.NotFound };
			}
		}
	}
}