using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using catalogue.ServiceModel.Solr;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using SolrNet;
using SolrNet.Impl;

namespace catalogue.Integration.Tests
{
	[TestFixture]
	public class TrackSearchTests
	{
		[Test]
		public void Should_be_able_to_retrieve_a_track_from_solr()
		{
			Startup.Init<SolrTrack>("http://solr.sochanik.co.uk/core0");
			var solr = ServiceLocator.Current.GetInstance<ISolrOperations<SolrTrack>>();
			var trackSearch = new TrackSearch(solr);
			ISolrQueryResults<SolrTrack> solrQueryResults = trackSearch.Search("muse");
		}

		[Test]
		public void Should_be_able_to_retrieve_a_track_from_solr_wothout_solrnet()
		{
			var trackSearch = new TrackSearchNoSolrNet();
			ISolrQueryResults<SolrTrack> solrTracks = trackSearch.Search("muse");
		}
	}

	[TestFixture]
	public class WebRequestExtensionTests
	{
		[Test]
		public void Should_return_me_a_request_as_a_string()
		{
			WebRequest webRequest = WebRequest.Create("http://www.sochanik.com/hello.html");
			string responseAsString = webRequest.GetResponseAsString();
			Assert.That(responseAsString, Is.EqualTo("hello world"));
		}
	}

	public class TrackSearch
	{
		private readonly ISolrOperations<SolrTrack> _solrOperations;

		public TrackSearch(ISolrOperations<SolrTrack> solrOperations)
		{
			_solrOperations = solrOperations;
		}

		public ISolrQueryResults<SolrTrack> Search(string searchterm)
		{
			var solrQuery = new SolrQuery(searchterm);
			return _solrOperations.Query(solrQuery);
		}
	}

	public class TrackSearchNoSolrNet
	{
		public ISolrQueryResults<SolrTrack> Search(string searchterm)
		{
			WebRequest webRequest = WebRequest.Create("http://solr.sochanik.co.uk/core0/select/?q=" + searchterm + "&start=0&rows=20&wt=json");
			var solrQueryResults = new SolrQueryResults<SolrTrack>();
			foreach (var solrTrack in EnumerateResponse(webRequest.GetResponseAsString()))
			{
				solrQueryResults.Add(solrTrack);
			}

			return solrQueryResults;
		}

		private static IEnumerable<SolrTrack> EnumerateResponse(string outputAsString)
		{
			var jss = new JavaScriptSerializer();
			var output = jss.Deserialize<dynamic>(outputAsString);
			foreach(var doc in output["response"]["docs"])
			{
				yield return new SolrTrack
				             	{
									Id = doc["id"],
				             		Album = doc["Album"],
									Artist = doc["Artist"],
									BitRate = doc["BitRate"],
									Genre = doc["Genre"],
									Kind = doc["Kind"],
									Location = doc["Location"],
									Name = doc["Name"],
									TrackNumber = doc["TrackNumber"]
				             	};
			}
		}
	}

	public static class WebRequestExtensions
	{
		public static string GetResponseAsString(this WebRequest request)
		{
			using (var webResponse = request.GetResponse())
			{
				using (var sr = new StreamReader(webResponse.GetResponseStream()))
				{
					return sr.ReadToEnd();
				}
			}
		}
	}
}
