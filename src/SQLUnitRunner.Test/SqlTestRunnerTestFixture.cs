using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using System.Collections.Generic;

namespace SQLUnitRunner.Test
{
	[TestFixture]
	public class SqlTestRunnerTestFixture : BaseSqlTestFixture
	{
		[Test]
		public void Can_Execute_All_Test_Fixtures_From_Runner()
		{
			var mockedLoader = Mock<ISqlTestLoader>();
			var testFixtures = new List<ISqlTestFixture>
			                   	{
			                   		Mock<ISqlTestFixture>(),
			                   		Mock<ISqlTestFixture>(),
			                   		Mock<ISqlTestFixture>(),
			                   	};
			// expect the collection to be returned once
			mockedLoader.Expect(x => x.Fixtures).Return(testFixtures).Repeat.Once();

			// expect each fixture to be executed once
			foreach (var fixture in testFixtures)
				fixture.Expect(x => x.Execute(null)).IgnoreArguments().Repeat.Once();

			Mocks.ReplayAll();

			var runner = new SqlTestRunner(mockedLoader);
			var results = runner.ExecuteAll(null).ToList();
			Assert.That(results.Count, Is.EqualTo(testFixtures.Count));

			Mocks.VerifyAll();
		}

		[Test]
		public void SqlTestComplete_Events_Raised_When_Test_Complete()
		{
			var mockedLoader = Mock<ISqlTestLoader>();
			var testFixtures = new List<ISqlTestFixture>
			                   	{
			                   		Mock<ISqlTestFixture>(),
			                   		Mock<ISqlTestFixture>(),
			                   		Mock<ISqlTestFixture>(),
			                   	};
			var runner = new SqlTestRunner(mockedLoader);
			var eventCalledCount = 0;

			// expect the collection to be returned once
			mockedLoader.Expect(x => x.Fixtures).Return(testFixtures).Repeat.Once();

			// expect each fixture to be executed once
			foreach (var fixture in testFixtures)
				fixture.Expect(x => x.Execute(null)).IgnoreArguments().Repeat.Once();

			Mocks.ReplayAll();

			runner.SqlTestComplete += ((sender, e) => { eventCalledCount++; });
			using (var con = GetDbConnection())
				runner.ExecuteAll(con).ToList();

			Assert.That(eventCalledCount, Is.EqualTo(3));
		}
	}
}