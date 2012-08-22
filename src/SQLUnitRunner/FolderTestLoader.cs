using System;
using System.Collections.Generic;
using System.IO;

namespace SQLUnitRunner
{
	/// <summary>
	/// Implementation of <see cref="ISqlTestLoader"/> which loads tests from 
	/// files found in a specified folder
	/// </summary>
	public class FolderTestLoader : ISqlTestLoader
	{
		private string folderPath = Environment.CurrentDirectory;
		private string filePattern = "*test*.sql";

		public FolderTestLoader(ITestReader reader, string folderPath)
		{
			TestReader = reader;
			FolderPath = folderPath;
		}

		public FolderTestLoader(ITestReader reader) : this(reader, Environment.CurrentDirectory) { }

		public string FolderPath
		{
			get { return folderPath; }
			set { folderPath = string.IsNullOrEmpty(value) ? Environment.CurrentDirectory : value.Trim(); }
		}

		public string FilePattern
		{
			get { return filePattern; }
			set
			{
				filePattern = string.IsNullOrEmpty(value) ? "*.*" : value.Trim();
			}
		}

		#region ISqlTestLoader Members

		public ITestReader TestReader { get; private set; }

		public IEnumerable<ISqlTestFixture> Fixtures
		{
			get 
			{
				if (TestReader == null)
					throw new ArgumentNullException("TestFileReader", "Implementation of ITestFileReader required");

				var testFolder = GetTestFolder();
				foreach (var testFile in testFolder.GetFiles(FilePattern))
				{
					using (var testFileInput = File.OpenText(testFile.FullName))
						yield return TestReader.Create(testFileInput);
				}
			}
		}

		DirectoryInfo GetTestFolder()
		{
			if (string.IsNullOrEmpty(FolderPath))
				throw new ArgumentException("TestFolderPath required", "TestFolderPath");

			var testFolder = new DirectoryInfo(FolderPath);

			if (!testFolder.Exists)
				throw new FileNotFoundException("Cannot find test folder " + testFolder.FullName);

			return testFolder;
		}
		#endregion
	}
}