using SolrNet.Attributes;

namespace catalogue.ServiceModel.Solr
{
	public class SolrTrack
	{
		[SolrUniqueKey("id")]
		public string Id { get; set; }
		[SolrField("Album")]
		public string Album { get; set; }
		[SolrField("Artist")]
		public string Artist { get; set; }
		[SolrField("BitRate")]
		public int BitRate { get; set; }
		[SolrField("Genre")]
		public string Genre { get; set; }
		[SolrField("Kind")]
		public string Kind { get; set; }
		[SolrField("Location")]
		public string Location { get; set; }
		[SolrField("Name")]
		public string Name { get; set; }
		[SolrField("TrackNumber")]
		public int TrackNumber { get; set; }
	}
}
