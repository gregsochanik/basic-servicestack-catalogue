using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using catalogue.ServiceModel.Solr;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using SolrNet;
using SolrNet.Attributes;
using SolrNet.Impl;
using SolrNet.Impl.FieldParsers;

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
			var trackSearch = new TrackSearchNoSolrNet(new SolrJsonResponseParser());
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
		private readonly IStringResponseParser _responseParser;

		public TrackSearchNoSolrNet(IStringResponseParser responseParser)
		{
			_responseParser = responseParser;
		}

		public ISolrQueryResults<SolrTrack> Search(string searchterm)
		{
			WebRequest webRequest = WebRequest.Create("http://solr.sochanik.co.uk/core0/select/?q=" + searchterm + "&start=0&rows=20&wt=json");
			var solrQueryResults = new SolrQueryResults<SolrTrack>();
			string responseAsString = webRequest.GetResponseAsString();
			foreach (var solrTrack in _responseParser.EnumerateResponse(responseAsString))
			{
				solrQueryResults.Add(solrTrack);
			}

			return solrQueryResults;
		}
	}

	public interface IStringResponseParser
	{
		IEnumerable<SolrTrack> EnumerateResponse(string outputAsString);
	}

	public class SolrJsonResponseParser : IStringResponseParser
	{
		public IEnumerable<SolrTrack> EnumerateResponse(string outputAsString)
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

	[TestFixture]
	public class SolrFieldParserTests
	{
		[Test]
		public void Should_return_correct_instance_type()
		{
			var solrFieldParser = new SolrFieldParser<SolrTrack>();
			Assert.That(solrFieldParser.ParseFields(new NameValueCollection()), Is.TypeOf(typeof(SolrTrack)));
		}
		
		[Test]
		public void Should_read_field_name_attributes()
		{
			var solrFieldParser = new SolrFieldParser<SolrTrack>();
			solrFieldParser.ParseFields(null);
		}

		[Test]
		public void SHould_fall_back_to_property_name()
		{
			
		}

		[Test]
		public void SHould_not_fail_if_does_not_exist(){}
	}


	public class SolrFieldParser<T>
	{
		public T ParseFields(NameValueCollection doc)
		{
			var type = typeof (T);

			Type solrFieldAttribute = typeof(SolrFieldAttribute);
			Type solrUniqueKeyAttribute = typeof(SolrUniqueKeyAttribute);

			return (T)Activator.CreateInstance(type);
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
