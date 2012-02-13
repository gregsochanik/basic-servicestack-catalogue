using Funq;
using ServiceStack.CacheAccess;
using ServiceStack.CacheAccess.Providers;
using ServiceStack.Common;
using ServiceStack.Redis;
using ServiceStack.ServiceHost;
using ServiceStack.WebHost.Endpoints;
using SevenDigital.Api.Wrapper;
using catalogue.Database.Queries;
using catalogue.Database.Repositories;
using catalogue.Service;
using catalogue.ServiceModel;

namespace catalogue {
	public class AppHost : AppHostHttpListenerBase {

		public AppHost() : base("Test Catalogue service", typeof(ArtistDetailsService).Assembly) {}

		public override void Configure(Container container) {
			
			// Set global headers
			SetConfig(
				new EndpointHostConfig
				{
					GlobalResponseHeaders =
					{
						{"Access-Control-Allow-Origin", "*"},
						{"Access-Control-Allow-Method", "GET, OPTIONS"}
					},
					EnableFeatures = Feature.All.Remove(Feature.All).Add(Feature.Xml | Feature.Json),
				}
			);

			// -- Example where every request is intercepted to return HttpStatusCode.Gone
			//ResponseFilters.Add((request, response, item) =>{
			//    response.StatusCode = (int)HttpStatusCode.Gone;
			//});

			// Register Dependencies
			// Example where I have set up an instance of a repository that reads from a db here
			// container.Register<IDbRepository<Artist>>(new ArtistDbRepository(new DatabaseQuery()));
			
			container.Register<IDbRepository<Artist>>(new ArtistWrapperRepository());
			container.Register<IDbRepository<Release>>(new ReleaseWrapperRepository());

			// -- Example setup of a redis cache client here
			// var cacheClient = new RedisClient("localhost", 6379);
			var cacheClient = new MemoryCacheClient();
			container.Register<ICacheClient>(cacheClient);

			// Register "routes"
			Routes
				.Add<Artist>("/artist/details")
				.Add<Artist>("/artist/details/{Id}")
				.Add<Release>("/release/details")
				.Add<Release>("/release/details/{ReleaseId}");
		}
	}

	public class ApiUri : IApiUri {
		public string Uri {
			get { return "http://api.7digital.com/1.2"; }
		}
	}

	public class AppSettingsCredentials : IOAuthCredentials {
		public AppSettingsCredentials() {
			ConsumerKey = "YOUR_KEY_HERE";
			ConsumerSecret = "";
		}

		public string ConsumerKey { get; set; }
		public string ConsumerSecret { get; set; }
	}

}
