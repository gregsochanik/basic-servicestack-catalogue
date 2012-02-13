using SevenDigital.Api.Wrapper;
using catalogue.ServiceModel;

namespace catalogue.Database.Repositories
{
	public class ReleaseWrapperRepository : IDbRepository<Release>
	{
		public Release Get(int id) {
				var apiRelease =
					Api<SevenDigital.Api.Schema.ReleaseEndpoint.Release>
						.Get.WithParameter("releaseId", id.ToString())
						.Please();

				var release = new Release
				              	{
				              		ReleaseId = apiRelease.Id,
				              		Barcode = apiRelease.Barcode,
				              		DateAdded = apiRelease.AddedDate.ToString(),
				              		ExplicitContent = apiRelease.ExplicitContent,
				              		ImageUrl = apiRelease.Image,
				              		Popularity = 1,
				              		ReleaseDate = apiRelease.ReleaseDate.ToString(),
				              		ReleaseType = apiRelease.Type.ToString(),
				              		Title = apiRelease.Title,
				              		Url = apiRelease.Url,
				              		Version = apiRelease.Version,
				              		Year = apiRelease.Year.ToString(),
				              		ReleaseArtist =
				              			new Artist
				              				{
				              					Id = apiRelease.Artist.Id,
				              					Name = apiRelease.Artist.Name,
				              					SortName = apiRelease.Artist.SortName,
				              					Url = apiRelease.Artist.Url
				              				}
				              	};
				return release;
		}
	}
}