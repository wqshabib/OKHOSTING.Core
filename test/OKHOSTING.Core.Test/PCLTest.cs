using System;
using OKHOSTING.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OKHOSTING.Core.Test
{
	[TestClass]
	public class PCLTest
	{
		[TestMethod]
		public void CharExtenstionTest()
		{
			Assert.Equals('.'.Category(), CharExtensions.CharCategory.Punctuation);
			Assert.Equals(' '.Category(), CharExtensions.CharCategory.Whitespace);
			Assert.Equals('0'.Category(), CharExtensions.CharCategory.Digit);
			Assert.Equals('f'.Category(), CharExtensions.CharCategory.Letter);
			Assert.Equals('%'.Category(), CharExtensions.CharCategory.Symbol);
		}

		[TestMethod]
		public void DateTimeExtenstionTest()
		{
			DateTime date = new DateTime(2016, 2, 27);

			Assert.IsTrue(date.IsWeekend());
			Assert.Equals(date.GetLastDayOfMonth(), new DateTime(2016, 2, 29));
		}
	}
}