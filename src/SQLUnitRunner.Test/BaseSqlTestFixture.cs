using System.Data;
using System.Data.SQLite;

namespace SQLUnitRunner.Test
{
	public class BaseSqlTestFixture : BaseTestFixture
	{
		private const string CONNECTION_STRING = "Data Source=:memory:;Version=3;New=True;";

		protected IDbConnection GetDbConnection()
		{
			var con = new SQLiteConnection(CONNECTION_STRING);
			con.Open();

			return con;
		}

	}
}