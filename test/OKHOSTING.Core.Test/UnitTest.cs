using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OKHOSTING.Core.Test
{
	[TestClass]
	public class UnitTest
	{
		[TestMethod]
		public void CryptographyTest()
		{
			string unencrypted = "hola k ase";
            string password = "mypassword";

            string encrypted = Cryptography.SimpleEncryption.Encrypt(unencrypted, password);
			string decrypted = Cryptography.SimpleEncryption.Decrypt(unencrypted, password);

			Assert.AreEqual(unencrypted, decrypted);
		}

		[TestMethod]
		public void ConversionTest()
		{
			string original  = "hola k ase";

			byte[] bytes = Cryptography.SimpleEncryption.StringToBytes(original);
			string result = Cryptography.SimpleEncryption.BytesToString(bytes);

			Assert.AreEqual(original, result);
		}
	}
}
