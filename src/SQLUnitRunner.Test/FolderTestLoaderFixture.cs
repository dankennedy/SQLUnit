using System.IO;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;

namespace SQLUnitRunner.Test
{
	[TestFixture]
	public class FolderTestLoaderFixture : BaseFixture
	{
		private string[] testFileNames = new[] { "TestFile1.sql", "TestFile2.sql", "TestFile3.sql" };

		[TestFixtureSetUp]
		void CreateDummyTestFiles()
		{
			foreach (var fileName in testFileNames)
			{
				using (var writer = File.CreateText(Path.Combine(Path.GetTempPath(), fileName)))
				{
					writer.WriteLine("-- Test name from file " + fileName);
					writer.WriteLine("Random gubbins");
					writer.WriteLine("Some more random gubbins");
				}
			}
		}

		[Test]
		public void Can_Read_Zero_Fixtures_From_Default_Folder()
		{
			var reader = Mock<ITestReader>();
			var loader = new FolderTestLoader(reader);
			
			Assert.That(loader.Fixtures.Count(), Is.EqualTo(0));
		}

		[Test]
		public void Can_Read_TestFile_Fixtures_From_Test_Folder()
		{
			CreateDummyTestFiles();
			var mockedReader = Mock<ITestReader>();

			mockedReader.Expect(x => x.Create(null))
				.Return(Mock<ISqlTestFixture>())
					.IgnoreArguments()
					.Repeat.Times(testFileNames.Count());

			mockedReader.Replay();
			var loader = new FolderTestLoader(mockedReader) {FolderPath = Path.GetTempPath()};
			Assert.That(loader.Fixtures.Count(), Is.EqualTo(testFileNames.Count()));
			DeleteDummyTestFiles();
		}


		[TestFixtureTearDown]
		void DeleteDummyTestFiles()
		{
			foreach (var fileName in testFileNames)
				File.Delete(Path.Combine(Path.GetTempPath(), fileName));
		}

	}
}