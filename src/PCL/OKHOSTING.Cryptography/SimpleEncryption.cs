using PCLCrypto;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace OKHOSTING.Cryptography
{
	/// <summary>
	/// Allows very simple encription of string values using Rijndael mechanism
	/// </summary>
	public static class SimpleEncryption
	{
		/// <summary>
		/// Encrypts a string a returns the result
		/// </summary>
		/// <param name="unencrypted">Value that will be encrypted</param>
		/// <param name="password">Password used to encrypt the value</param>
		/// <returns>A encrypted string</returns>
		public static byte[] Encrypt(byte[] unencrypted, string password)
		{
			byte[] keyMaterial = WinRTCrypto.CryptographicBuffer.ConvertStringToBinary(password, Encoding.Unicode);
			var provider = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesCbcPkcs7);
			var key = provider.CreateSymmetricKey(keyMaterial);

			return WinRTCrypto.CryptographicEngine.Encrypt(key, unencrypted);
		}

		/// <summary>
		/// Decrypts a string a returns the result
		/// </summary>
		/// <param name="encrypted">Encrypted value that will be decrypted</param>
		/// <param name="password">Password used to decrypt the value</param>
		/// <returns>A decrypted string</returns>
		public static byte[] Decrypt(byte[] encrypted, string password)
		{
			byte[] keyMaterial = WinRTCrypto.CryptographicBuffer.ConvertStringToBinary(password, Encoding.Unicode);
			var provider = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesCbcPkcs7);
			var key = provider.CreateSymmetricKey(keyMaterial);

			return WinRTCrypto.CryptographicEngine.Decrypt(key, encrypted);
		}

		#region Support methods

		/// <summary>
		/// Encrypts a string a returns the result
		/// </summary>
		/// <param name="strToEncrypt">Value that will be encrypted</param>
		/// <param name="password">Password used to encrypt the value</param>
		/// <returns>A encrypted string</returns>
		public static string Encrypt(string strToEncrypt, string password)
		{
			return BytesToString(Encrypt(StringToBytes(strToEncrypt), password));
		}

		/// <summary>
		/// Decrypts a string a returns the result
		/// </summary>
		/// <param name="strEncrypted">Encrypted value that will be decrypted</param>
		/// <param name="password">Password used to decrypt the value</param>
		/// <returns>A decrypted string</returns>
		public static string Decrypt(string strEncrypted, string password)
		{
			return BytesToString(Decrypt(StringToBytes(strEncrypted), password));
		}

		/// <summary>
		/// Creates a random number
		/// </summary>
		public static uint CreateRandomKey()
		{
			return WinRTCrypto.CryptographicBuffer.GenerateRandomNumber();
		}

		public static string CreateRandomPassword(int lenght, bool useNumbers, bool useUpperCase, bool useLowerCase, bool useSymbols)
		{
			var upperCase = new char[]
				{
				'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U',
				'V', 'W', 'X', 'Y', 'Z'
				};

			var lowerCase = new char[]
				{
				'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u',
				'v', 'w', 'x', 'y', 'z'
				};

			var numerals = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

			var symbols = new char[]
				{
				'~', '`', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '{', '[', '}', ']', '-', '_', '=', '+', ':',
				';', '|', '/', '?', ',', '<', '.', '>'
				};

			char[] total = (new char[0])
				.Concat(useLowerCase ? upperCase : new char[0])
				.Concat(useUpperCase ? lowerCase : new char[0])
				.Concat(useNumbers ? numerals : new char[0])
				.Concat(useSymbols ? symbols : new char[0])
				.ToArray();

			var rnd = new Random();

			var chars = Enumerable
				.Repeat<int>(0, lenght)
				.Select(i => total[rnd.Next(total.Length)])
				.ToArray();

			return new string(chars);
		}

		/// <summary>
		/// Creates an array of bytes with random numbers
		/// </summary>
		public static byte[] CreateRandomKey(uint length)
		{
			return WinRTCrypto.CryptographicBuffer.GenerateRandom(length);
		}

		public static string BytesToString(byte[] data)
		{
			return WinRTCrypto.CryptographicBuffer.EncodeToBase64String(data);
		}

		public static byte[] StringToBytes(string data)
		{
			return WinRTCrypto.CryptographicBuffer.DecodeFromBase64String(data);
		}

		#endregion
	}
}