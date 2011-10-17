using ServiceStack.CacheAccess;
using ServiceStack.Common;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using catalogue.Database.Repositories;
using catalogue.ServiceModel;

namespace catalogue.Service
{
	public class ReleaseDetailsService : RestServiceBase<Release>
	{
		private readonly IDbRepository<Release> _repository;
		private readonly ICacheClient _cacheClient;

		public ReleaseDetailsService(IDbRepository<Release> repository, ICacheClient cacheClient) {
			_repository = repository;
			_cacheClient = cacheClient;
		}

		public override object OnGet(Release request) {
			var cacheKey = UrnId.Create<Release>(request.ReleaseId.ToString());

			return RequestContext.ToOptimizedResultUsingCache(_cacheClient, cacheKey,
			                                                  () => _repository.Get(request.ReleaseId)
															);
		}
	}
}