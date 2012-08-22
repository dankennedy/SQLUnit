using System;
using System.Data;

namespace SQLUnitRunner
{
	/// <summary>
	/// Basic implementation of <see cref="ISqlTestFixture"/> which executes a test
	/// against an <see cref="IDbConnection"/> and returns the result
	/// </summary>
	public class SqlTestFixture : ISqlTestFixture
	{
		public event EventHandler<SqlTestCompleteEventArgs> SqlTestComplete;

		public SqlTestFixture(string name, string sql)
		{
			Name = name;
			Sql = sql;
		}

		public string Name { get; private set; }
		public string Sql { get; private set; }

		public ISqlTestResult Execute(IDbConnection connection)
		{
			if (connection == null)
				throw new ArgumentNullException("connection", "Valid database connection required to execute test");

			ISqlTestResult result;
			using (var cmd = connection.CreateCommand())
			{
				cmd.CommandText = Sql;
				try
				{
					if (connection.State != ConnectionState.Open)
						connection.Open();

					using (var dr = cmd.ExecuteReader())
					{
						var errorTable = new DataTable();

						if (dr != null && !dr.IsClosed)
							errorTable.Load(dr);

						if (errorTable.Rows.Count > 0)
							result = new SqlTestResult(false,
							                           string.Format("{0} row{1} produced by test", errorTable.Rows.Count,
							                                         errorTable.Rows.Count == 1 ? string.Empty : "s"),
																				 errorTable);
						else
							result = new SqlTestResult(true);
					}
				}
				catch (Exception ex)
				{
					result = new SqlTestResult(false, ex.GetType().Name + ". " + ex.Message);
				}
			}
			OnSqlTestComplete(result);
			return result;
		}

		protected void OnSqlTestComplete(ISqlTestResult result)
		{
			if (result == null)
				throw new ArgumentNullException("result", "Test result required");

			if (SqlTestComplete != null)
				SqlTestComplete(this, new SqlTestCompleteEventArgs(this, result));
		}
	}
}