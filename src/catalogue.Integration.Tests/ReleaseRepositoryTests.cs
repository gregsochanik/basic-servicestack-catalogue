using NUnit.Framework;
using SevenDigital.Api.Wrapper.Exceptions;
using catalogue.Database.Repositories;
using catalogue.ServiceModel;

namespace catalogue.Integration.Tests
{
	[TestFixture]
	public class ReleaseRepositoryTests
	{
		[Test]
		public void Can_select_a_single_release() {
			
			var repository = new ReleaseWrapperRepository();
			try {
				Release release = repository.Get(12345);

				Assert.That(release.ReleaseId, Is.EqualTo(12345));
			} catch(ApiXmlException ex) {
				Assert.Fail(ex.Error.ErrorMessage);
			}
		}
	}
}