using System;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using System.Collections.Generic;

namespace SQLUnitRunner.Test
{
	[TestFixture]
	public class SqlTestRunnerFixture : BaseSqlFixture
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

		[Test]
		public void SqlTestRunComplete_Event_Raised_With_Summary_Results_When_Run_Complete()
		{
			var mockedLoader = Mock<ISqlTestLoader>();
			var runner = new SqlTestRunner(mockedLoader);
			var testFixtures = new List<ISqlTestFixture>
			                   	{
			                   		Mock<ISqlTestFixture>(),
			                   		Mock<ISqlTestFixture>(),
			                   		Mock<ISqlTestFixture>(),
			                   	};

			// expect the collection to be returned once
			mockedLoader.Expect(x => x.Fixtures).Return(testFixtures).Repeat.Once();

			var testRunEventCalledCount = 0;
			int fixtureExecutedCount = 0;

			// expect each fixture to be executed once
			foreach (var fixture in testFixtures)
			{
				fixture.Stub(x => x.Execute(null))
					.IgnoreArguments()
					.WhenCalled(x =>
					            	{
					            		// execute with a delay to check we get a TimeTaken result
					            		System.Threading.Thread.Sleep(100);
					            		// fail one of the tests to check the counts
													x.ReturnValue = new SqlTestResult(++fixtureExecutedCount % 2 == 1); 
					            	});
			}

			Mocks.ReplayAll();

			SqlTestRunCompleteEventArgs args = null;
			runner.SqlTestRunComplete += ((sender, e) =>
			                              	{
			                              		testRunEventCalledCount++;
			                              		args = e;
			                              	});
			using (var con = GetDbConnection())
				runner.ExecuteAll(con).ToList();

			Assert.That(testRunEventCalledCount, Is.EqualTo(1));
			Assert.That(args, Is.Not.Null);
			Assert.That(args.TestsTaken, Is.EqualTo(testFixtures.Count));
			Assert.That(args.TestsPassed, Is.EqualTo(2));
			Assert.That(args.TestsFailed, Is.EqualTo(1));
			Assert.That(args.TimeTaken.Ticks, Is.GreaterThan(0));
		}
	}
}