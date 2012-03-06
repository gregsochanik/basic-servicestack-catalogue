using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web.Script.Serialization;
using catalogue.ServiceModel.Solr;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using SolrNet;
using SolrNet.Attributes;
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
			var trackSearch = new TrackSearchNoSolrNet(new SolrJsonResponseParser(new SolrFieldParser<SolrTrack>(new SolrFieldMapper<SolrTrack>())));
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
		private readonly SolrFieldParser<SolrTrack> _solrFieldParser;

		public SolrJsonResponseParser(SolrFieldParser<SolrTrack> solrFieldParser)
		{
			_solrFieldParser = solrFieldParser;
		}

		public IEnumerable<SolrTrack> EnumerateResponse(string outputAsString)
		{
			var jss = new JavaScriptSerializer();
			var output = jss.Deserialize<dynamic>(outputAsString);

			var docs = output["response"]["docs"];

			foreach(var doc in docs)
			{
				yield return _solrFieldParser.ParseFields(doc);
			}
		}
	}

	[TestFixture]
	public class SolrFieldParserTests
	{
		private static dynamic StubTrack()
		{
			const string json = "{\"id\":\"19223\",\"Album\":\"The Hits/The B-Sides [Disc 2]\",\"Artist\":\"Prince\", " +
			                    "\"BitRate\":192,\"Composer\":\"Prince\",\"Genre\":\"Rock\",\"Kind\":\"MPEG audio file\", " +
			                    "\"Location\":\"file://localhost/G:/ServerFolders/Music/Prince/The%20Hits_The%20B-Sides%20%5BDisc%202%5D/2-11%20Kiss.mp3\", " +
			                    "\"Name\":\"Kiss\",\"PlayCount\":0,\"SampleRate\":44100,\"Size\":5435292,\"TotalTime\":226377,\"TrackNumber\":11}";

			var jss = new JavaScriptSerializer();
			return jss.Deserialize<dynamic>(json);
		}

		[Test]
		public void Should_return_correct_instance_type()
		{
			var solrFieldParser = new SolrFieldParser<SolrTrack>(new SolrFieldMapper<SolrTrack>());
			var condition = solrFieldParser.ParseFields(StubTrack());
			Assert.That(condition, Is.TypeOf(typeof(SolrTrack)));
		}
	}

	[TestFixture]
	public class SolrFieldMapperTests
	{
		[Test]
		public void Should_map_fields_to_properties()
		{
			var solrFieldMapper = new SolrFieldMapper<SolrTrack>();
			var propertyToFieldMapping = solrFieldMapper.GetPropertyToFieldMapping();
			Assert.That(propertyToFieldMapping.Count(), Is.EqualTo(9));
		}
	}

	public class SolrFieldMapper<T> : IFieldMapper
	{
		public IDictionary<PropertyInfo, SolrFieldAttribute> GetPropertyToFieldMapping()
		{
			var type = typeof(T);

			var propertyToFieldMapping = new Dictionary<PropertyInfo, SolrFieldAttribute>();

			foreach (var property in type.GetProperties())
			{
				var fieldAttribute = (SolrFieldAttribute)property.GetCustomAttributes(typeof(SolrFieldAttribute), false).First();
				if (fieldAttribute != null)
					propertyToFieldMapping.Add(property, fieldAttribute);
			}
			return propertyToFieldMapping;
		}
	}

	public interface IFieldMapper
	{
		IDictionary<PropertyInfo, SolrFieldAttribute> GetPropertyToFieldMapping();
	}

	public class SolrFieldParser<T>
	{
		private readonly IFieldMapper _fieldMapper;

		public SolrFieldParser(IFieldMapper fieldMapper)
		{
			_fieldMapper = fieldMapper;
		}

		public T ParseFields(dynamic doc)
		{
			var type = typeof (T);

			var propertyToFieldMapping = _fieldMapper.GetPropertyToFieldMapping();

			var instance = (T)Activator.CreateInstance(type);

			foreach (var attribute in propertyToFieldMapping)
			{
				PropertyInfo propertyInfo = attribute.Key;
				MethodInfo methodInfo = propertyInfo.GetSetMethod();
				string fieldName = attribute.Value.FieldName;
				var s = doc[fieldName];
				methodInfo.Invoke(instance, new object[]{s});
			}

			return instance;
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
