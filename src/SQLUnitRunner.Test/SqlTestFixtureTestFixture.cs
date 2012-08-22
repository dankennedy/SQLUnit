using System;
using System.Linq;
using NUnit.Framework;

namespace SQLUnitRunner.Test
{
	[TestFixture]
	public class SqlTestFixtureTestFixture : BaseSqlTestFixture
	{
		[Test]
		public void Test_Fails_When_Rows_Returned()
		{
			var fixture = new SqlTestFixture("Test1", "SELECT 'StringValue' AS Field1, " +
			                                          "12345 AS Field2, " +
			                                          "NULL AS Field3");

			ISqlTestResult result;
			using (var con = GetDbConnection())
				result = fixture.Execute(con);

			Assert.That(result.Passed, Is.False);
			Assert.That(result.ErrorTable.Rows.Count, Is.EqualTo(1));
		}

		[Test]
		public void Correct_Values_Returned_From_Failing_Test()
		{
			var fixture = new SqlTestFixture("Test1", "SELECT 'StringValue' AS Field1, " +
																								"12345 AS Field2, " +
																								"NULL AS Field3");

			ISqlTestResult result;
			using (var con = GetDbConnection())
				result = fixture.Execute(con);

			Assert.That(result.ErrorTable.Rows.Count, Is.EqualTo(1));

			var errorRow = result.ErrorTable.Rows[0];

			Assert.That(result.ErrorTable.Columns.Count, Is.EqualTo(3));

			Assert.That(errorRow[0].ToString(), Is.EqualTo("StringValue"));
			Assert.That(errorRow[1].ToString(), Is.EqualTo("12345"));
			Assert.That(errorRow[2], Is.EqualTo(DBNull.Value));
		}

		[Test]
		public void Exception_Thrown_When_Null_Connection_Passed()
		{
			var fixture = new SqlTestFixture(string.Empty, string.Empty);

			try
			{
				fixture.Execute(null);
			}
			catch (ArgumentNullException)
			{
				return;
			}
			Assert.Fail("Expected exception not thrown");
		}

		[Test]
		public void False_Result_Returned_When_Invalid_SQL_Passed()
		{
			var fixture = new SqlTestFixture("TestName", "some invalid sql");

			ISqlTestResult result;
			using (var con = GetDbConnection())
				result = fixture.Execute(con);

			Assert.That(result.Passed, Is.False);
			Assert.That(result.Message.Contains("Exception"));
		}

		[Test]
		public void SqlTestComplete_Event_Raised_When_Test_Complete()
		{
			var fixture = new SqlTestFixture(string.Empty, string.Empty);
			bool eventCalled = false;

			fixture.SqlTestComplete += ((sender, e) => { eventCalled = true; });

			using (var con = GetDbConnection())
				fixture.Execute(con);

			Assert.That(eventCalled, Is.True);
		}
	}
}