using NUnit.Framework;
using Rhino.Mocks;

namespace SQLUnitRunner.Test
{
	[TestFixture]
	public class BaseFixture
	{
		protected readonly MockRepository Mocks;

		public BaseFixture()
		{
			Mocks = new MockRepository();	
		}

		protected T Mock<T>(params object[] constructorParams) where T : class
		{
			return Mocks.StrictMock<T>(constructorParams);
		}

		protected T Stub<T>(params object[] constructorParams) where T : class
		{
			return Mocks.Stub<T>(constructorParams);
		}
	}
}