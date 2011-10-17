using System;
using System.Collections.Generic;
using catalogue.Database.Queries;
using catalogue.ServiceModel;

namespace catalogue.Database.Repositories
{
	public class ArtistDbRepository : IDbRepository<Artist>
	{
		private readonly IDatabaseQuery _databaseQuery;

		public ArtistDbRepository(IDatabaseQuery databaseQuery) {
			_databaseQuery = databaseQuery;
		}

		public Artist Get(int id) {
			var query = new ArtistDetailsQuery(new KeyValuePair<string, object>("@Id", id));
			var executeQuery = _databaseQuery.ExecuteQuery(query);
			var artist = new Artist();
			while(executeQuery.Read()) {
				artist = new Artist
				{
					Id = Convert.ToInt32(executeQuery["masterartist_masterartistid"]),
					Name = (string) executeQuery["masterartist_masterartistname"],
					SortName = (string) executeQuery["masterartist_masterartistsortname"],
					Url = (string) executeQuery["masterartist_masterartisturl"]
				};
			}
			return artist;
		}
	}
}