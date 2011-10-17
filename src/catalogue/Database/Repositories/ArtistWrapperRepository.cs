using SevenDigital.Api.Wrapper;
using catalogue.ServiceModel;

namespace catalogue.Database.Repositories
{
	public class ArtistWrapperRepository : IDbRepository<Artist>
	{
		public Artist Get(int id) {
			var apiRelease =
				Api<SevenDigital.Api.Schema.ArtistEndpoint.Artist>
					.Get
					.WithParameter("artistId", id.ToString())
					.Please();
			var artist = new Artist()
			{
				Bio = apiRelease.Name,
				Id = apiRelease.Id,
				Image = apiRelease.Image,
				Name = apiRelease.Name,
				SortName = apiRelease.SortName,
				Url = apiRelease.Url
			};
			return artist;
		}
	}
}