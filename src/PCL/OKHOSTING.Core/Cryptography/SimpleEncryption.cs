using PCLCrypto;
using System;
using System.Linq;
using System.Text;

namespace OKHOSTING.Core.Cryptography
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
			// Get the MD5 key hash (you can as well use the binary of the key string)
			var keyHash = GetMD5Hash(password);

			// Open a symmetric algorithm provider for the specified algorithm.
			var aes = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesEcbPkcs7);

			// Create a symmetric key.
			var symetricKey = aes.CreateSymmetricKey(keyHash);

			// The input key must be securely shared between the sender of the cryptic message
			// and the recipient. The initialization vector must also be shared but does not
			// need to be shared in a secure manner. If the sender encodes a message string
			// to a buffer, the binary encoding method must also be shared with the recipient.
			return WinRTCrypto.CryptographicEngine.Encrypt(symetricKey, unencrypted, null);
		}

		/// <summary>
		/// Decrypts a string a returns the result
		/// </summary>
		/// <param name="encrypted">Encrypted value that will be decrypted</param>
		/// <param name="password">Password used to decrypt the value</param>
		/// <returns>A decrypted string</returns>
		public static byte[] Decrypt(byte[] encrypted, string password)
		{
			// Get the MD5 key hash (you can as well use the binary of the key string)
			var keyHash = GetMD5Hash(password);

			// Open a symmetric algorithm provider for the specified algorithm.
			var aes = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesEcbPkcs7);

			// Create a symmetric key.
			var symetricKey = aes.CreateSymmetricKey(keyHash);

			return WinRTCrypto.CryptographicEngine.Decrypt(symetricKey, encrypted, null);
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
			byte[] buffer = WinRTCrypto.CryptographicBuffer.ConvertStringToBinary(strToEncrypt, Encoding.UTF8);
			byte[] encrypted = Encrypt(buffer, password);
			
			// Convert the encrypted buffer to a string (for display).
			// We are using Base64 to convert bytes to string since you might get unmatched characters
			// in the encrypted buffer that we cannot convert to string with UTF8.
			return WinRTCrypto.CryptographicBuffer.EncodeToBase64String(encrypted);
		}

		/// <summary>
		/// Decrypts a string a returns the result
		/// </summary>
		/// <param name="strEncrypted">Encrypted value that will be decrypted</param>
		/// <param name="password">Password used to decrypt the value</param>
		/// <returns>A decrypted string</returns>
		public static string Decrypt(string strEncrypted, string password)
		{
			// Create a buffer that contains the encoded message to be decrypted.
			var toDecryptBuffer = WinRTCrypto.CryptographicBuffer.DecodeFromBase64String(strEncrypted);
			var decryptedBuffer = Decrypt(toDecryptBuffer, password);
			
			return WinRTCrypto.CryptographicBuffer.ConvertBinaryToString(Encoding.UTF8, decryptedBuffer);
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
			return WinRTCrypto.CryptographicBuffer.GenerateRandom((int) length);
		}

		private static byte[] GetMD5Hash(string key)
		{
			// Convert the message string to binary data.
			var buffUtf8Msg = WinRTCrypto.CryptographicBuffer.ConvertStringToBinary(key, Encoding.UTF8);

			// Create a HashAlgorithmProvider object.
			var objAlgProv = WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(HashAlgorithm.Md5);

			// Hash the message.
			var buffHash = objAlgProv.HashData(buffUtf8Msg);

			// Verify that the hash length equals the length specified for the algorithm.
			if (buffHash.Length != objAlgProv.HashLength)
			{
				throw new Exception("There was an error creating the hash");
			}

			return buffHash;
		}

		#endregion
	}
}