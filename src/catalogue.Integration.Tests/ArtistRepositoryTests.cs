using System.Data;
using FakeItEasy;
using NUnit.Framework;
using catalogue.Database.Queries;
using catalogue.Database.Repositories;
using catalogue.ServiceModel;

namespace catalogue.Integration.Tests
{
	[TestFixture]
	public class ArtistRepositoryTests
	{
		[Test]
		public void Can_select_a_single_artist() {
			var databaseQuery = A.Fake<IDatabaseQuery>();
			var dataTable = new DataTable();
			dataTable.Columns.Add("masterartist_masterartistid");
			dataTable.Columns.Add("masterartist_masterartistname");
			dataTable.Columns.Add("masterartist_masterartistsortname");
			dataTable.Columns.Add("masterartist_masterartisturl");
			dataTable.Rows.Add(1, "Test", "test", "test");

			A.CallTo(() => databaseQuery.ExecuteQuery(null)).WithAnyArguments().Returns(new DataTableReader(dataTable));

			var artistRepository = new ArtistDbRepository(databaseQuery);
			Artist artist = artistRepository.Get(1);

			Assert.That(artist.Id, Is.EqualTo(1));
			Assert.That(artist.Name, Is.Not.Null);
			Assert.That(artist.SortName, Is.Not.Null);
			Assert.That(artist.Url, Is.Not.Null);
		}
	}
}