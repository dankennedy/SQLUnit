using System;
using System.IO;
using NUnit.Framework;

namespace SQLUnitRunner.Test
{
	[TestFixture]
	public class FileTestReaderFixture : BaseFixture
	{

		[Test]
		public void Can_Read_TestName_From_Incoming_Stream()
		{
			var testReader = new FileTestReader();
			const string TEST_NAME = "This is my test name";
			using (var dummyFileStream = CreateDummyTestFixtureFile(TEST_NAME, ""))
			{
				var testFixture = testReader.Create(dummyFileStream);
				Assert.That(testFixture.Name, Is.EqualTo(TEST_NAME));
			}
		}

		[Test]
		public void Default_TestName_Provided_From_Empty_Incoming_Stream()
		{
			var testReader = new FileTestReader();
			using (var dummyFileStream = CreateDummyTestFixtureFile("", ""))
			{
				var testFixture = testReader.Create(dummyFileStream);
				Assert.That(testFixture.Name, Is.Not.Empty);
			}
		}

		[Test]
		public void Read_Only_First_TestName_From_Multiple_Options()
		{
			var testReader = new FileTestReader();
			const string TEST_NAME = "This is my test name\r\n-- Another potential test name\r\nAnd another";
			using (var dummyFileStream = CreateDummyTestFixtureFile(TEST_NAME, ""))
			{
				var testFixture = testReader.Create(dummyFileStream);
				Assert.That(testFixture.Name, Is.EqualTo(TEST_NAME.Split(Environment.NewLine.ToCharArray())[0]));
			}
		}

		[Test]
		public void Can_Read_FixtureContents_From_Incoming_Stream()
		{
			var testReader = new FileTestReader();
			using (var dummyFileStream = CreateDummyTestFixtureFile())
			{
				var testFixture = testReader.Create(dummyFileStream);
				dummyFileStream.BaseStream.Seek(0, SeekOrigin.Begin);
				Assert.That(testFixture.Sql, Is.EqualTo(dummyFileStream.ReadToEnd()));
			}
		}

		StreamReader CreateDummyTestFixtureFile()
		{
			return CreateDummyTestFixtureFile("This is my test name", "These are some test contents");
		}

		StreamReader CreateDummyTestFixtureFile(string testName, string fixtureContents)
		{
			var dummyFileStream = new MemoryStream();
			var dummyFileContents = new StreamWriter(dummyFileStream);
			dummyFileContents.Write("-- " + testName + "\r\n" + fixtureContents);
			dummyFileStream.Position = 0;
			dummyFileContents.Flush();
			return new StreamReader(dummyFileStream);
		}
	}
}