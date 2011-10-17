using System.Collections.Generic;

namespace catalogue.Database.Queries
{
	public class ArtistDetailsQuery : IQuery
	{
		private readonly KeyValuePair<string, object> _id;

		public ArtistDetailsQuery(KeyValuePair<string, object> id) {
			_id = id;
		}

		public IDictionary<string, object> Parameters {
			get {
				return new Dictionary<string, object> { { "@Id", _id.Value } };
			}
		}

		public string Command {
			get {
				return "SELECT masterartist_masterartistid, masterartist_masterartistsortname, masterartist_masterartistname, "
						+ "masterartist_masterartisturl FROM vw_MasterArtist WHERE masterartist_masterartistid=@Id";
			}
		}
	}
}