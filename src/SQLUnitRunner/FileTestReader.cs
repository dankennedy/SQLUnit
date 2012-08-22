using System;
using System.IO;
using System.Text.RegularExpressions;

namespace SQLUnitRunner
{
	/// <summary>
	/// Implementation of <see cref="ITestReader"/> which parses a 
	/// text file, reads the first comment as the test name
	/// and the remaining content as the test body
	/// </summary>
	public class FileTestReader : ITestReader
	{
		public FileTestReader()
		{
			TestNameRegexPattern = @"^\s*--(?<TestName>.+?)\s*$";
		}

		#region ITestReader Members

		public ISqlTestFixture Create(StreamReader input)
		{
			if (input == null)
				throw new ArgumentNullException("input", "Test content required");

			if (input.EndOfStream)
				input.BaseStream.Seek(0, SeekOrigin.Begin);

			var testContent = input.ReadToEnd();

			return new SqlTestFixture(GetTestName(testContent), testContent);
		}

		#endregion

		public string TestNameRegexPattern { get; set; }

		/// <summary>
		/// Parses the test name out from the entire test definition
		/// </summary>
		/// <param name="testContent">The test content including name</param>
		/// <returns>The test name from the definition or <c>Not Specified</c> if not found</returns>
		string GetTestName(string testContent)
		{
			var testNameMatch = Regex.Match(testContent, TestNameRegexPattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
			var testName = testNameMatch.Success ? testNameMatch.Groups["TestName"].Value.Trim() : string.Empty;
			return string.IsNullOrEmpty(testName) ? "Not specified" : testName;
		}
	}
}