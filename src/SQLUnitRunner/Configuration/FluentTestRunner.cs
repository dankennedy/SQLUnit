namespace SQLUnitRunner.Configuration
{
	/// <summary>
	/// Provides a basic fluent interface to configure and
	/// run a set of tests
	/// </summary>
	public static class FluentTestRunner
	{
		public static FluentConfiguration ConfigureAndRun()
		{
			return new FluentConfiguration();
		}
	}
}