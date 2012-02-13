using ServiceStack.CacheAccess;
using ServiceStack.Common;
using ServiceStack.ServiceHost;
using catalogue.Database.Repositories;
using catalogue.ServiceModel;

namespace catalogue.Service
{
	public class ArtistDetailsService : ErrorHandlingRestServiceBase<Artist>
	{
		private readonly IDbRepository<Artist> _repository;
		private readonly ICacheClient _cacheClient;

		public ArtistDetailsService(IDbRepository<Artist> repository, ICacheClient cacheClient) {
			_repository = repository;
			_cacheClient = cacheClient;
		}

		public override object OnGet(Artist request) {
			var cacheKey = UrnId.Create<Artist>(request.Id.ToString());

			return ErrorCheck.NotFoundCheck(
					() => RequestContext.ToOptimizedResultUsingCache(_cacheClient, cacheKey, () => _repository.Get(request.Id))
				);
		}
	}
}