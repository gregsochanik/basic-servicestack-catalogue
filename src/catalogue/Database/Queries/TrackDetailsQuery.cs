using System.Collections.Generic;

namespace catalogue.Database.Queries
{
	public class TrackDetailsQuery : IQuery
	{
		
		private readonly KeyValuePair<string, object> _id;

		public TrackDetailsQuery(KeyValuePair<string, object> id) {
			_id = id;
		}

		public IDictionary<string, object> Parameters {
			get {
				return new Dictionary<string, object> { { "@Id", _id.Value } };
			}
		}

		public string Command {
			get {
				return "SELECT [track_trackid]"
				       + ",[track_trackname]"
				       + ",[track_trackversionname]"
				       + ",[track_tracksortname]"
				       + ",[track_artistid]"
				       + ",[track_labelid]"
				       + ",[track_publisherid]"
				       + ",[track_duration]"
				       + ",[track_dateadded]"
				       + ",[track_isrc]"
				       + ",[track_labelline]"
				       + ",[track_publisherline]"
				       + ",[track_parentaladvisory]"
				       + ",[track_tracktypeid]"
				       + "FROM [vw_Track] WHERE track_trackid=@Id";
			}
		}
	}
}