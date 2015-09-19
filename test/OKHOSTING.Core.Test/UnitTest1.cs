using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OKHOSTING.Core.Data.Validation;

namespace OKHOSTING.Core.Test
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestMethod1()
		{
			string member = MemberExpression<StringLengthValidator>.GetMemberString(m => m.Member.ReturnType.Name);
		}
	}
}
