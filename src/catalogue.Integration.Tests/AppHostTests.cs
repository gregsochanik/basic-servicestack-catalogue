using System;
using System.IO;
using System.Net;
using System.Xml;
using Funq;
using NUnit.Framework;
using ServiceStack.ServiceHost;
using ServiceStack.WebHost.Endpoints;

namespace catalogue.Integration.Tests {

	[TestFixture]
	public class AppHostTests {
		private AppHost _appHost;

		[TestFixtureSetUp]
		public void SetUp() {
			_appHost = new AppHost();
			_appHost.Init();
			_appHost.Start("http://localhost:2212/");

		}
		
		[Test]
		public void Should_set_up_apphost() {
			IServiceRoutes serviceRoutes = _appHost.Routes;
			Assert.That(serviceRoutes, Is.Not.Null);
		}

		[Test]
		public void Should_set_up_apphost_globalResponseheaders() {
			EndpointHostConfig endpointHostConfig = _appHost.Config;

			Assert.That(endpointHostConfig.GlobalResponseHeaders["Access-Control-Allow-Origin"], Is.EqualTo("*"));
			Assert.That(endpointHostConfig.GlobalResponseHeaders["Access-Control-Allow-Method"], Is.EqualTo("GET, OPTIONS"));
		}

		[Test]
		public void Should_get_me_an_artist_xml() {
			var xmlDocument = new XmlDocument();
			xmlDocument.Load("http://localhost:2212/artist/details/1?format=xml");
			Console.WriteLine(xmlDocument.InnerXml);
			Assert.That(xmlDocument.InnerXml, Is.Not.Null);
		}

		[Test]
		public void Should_get_me_a_release_xml() {
			var xmlDocument = new XmlDocument();
			xmlDocument.Load("http://localhost:2212/release/details/12345?format=xml");
			Console.WriteLine(xmlDocument.InnerXml);
			Assert.That(xmlDocument.InnerXml, Is.Not.Null);
		}

		[Test]
		public void Should_get_me_an_artist_with_json() {
			var webRequest = (HttpWebRequest)WebRequest.Create("http://localhost:2212/artist/details/1?format=json");
			WebResponse webResponse = webRequest.GetResponse();
			using (var stream = webResponse.GetResponseStream()) {
				using (var sr = new StreamReader(stream)) {
					string readToEnd = sr.ReadToEnd();
					Console.WriteLine(readToEnd);
					Assert.That(readToEnd, Is.Not.Null);
				}
			}
		}

		[TestFixtureTearDown]
		public void TearDown() {
			_appHost.Stop();
			_appHost.Dispose();
		}
	}
}
