using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Security.Cryptography;
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
		/// <param name="strToEncrypt">Value that will be encrypted</param>
		/// <param name="password">Password used to encrypt the value</param>
		/// <returns>A encrypted string</returns>
		public static string Encrypt(string strToEncrypt, SecureString password)
		{
			return BytesToBase64String(Encrypt(StringToBytes(strToEncrypt), password));
		}

		/// <summary>
		/// Decrypts a string a returns the result
		/// </summary>
		/// <param name="strEncrypted">Encrypted value that will be decrypted</param>
		/// <param name="password">Password used to decrypt the value</param>
		/// <returns>A decrypted string</returns>
		public static string Decrypt(string strEncrypted, SecureString password)
		{
			return BytesToString(Decrypt(Base64StringToBytes(strEncrypted), password));
		}

		/// <summary>
		/// Encrypts a string a returns the result
		/// </summary>
		/// <param name="unencrypted">Value that will be encrypted</param>
		/// <param name="password">Password used to encrypt the value</param>
		/// <returns>A encrypted string</returns>
		public static byte[] Encrypt(byte[] unencrypted, SecureString password)
		{
			TripleDESCryptoServiceProvider objDESCrypto = new TripleDESCryptoServiceProvider();
			MD5CryptoServiceProvider objHashMD5 = new MD5CryptoServiceProvider();
			byte[] byteHash;

			byteHash = objHashMD5.ComputeHash(StringToBytes(ToUnsecureString(password)));
			objHashMD5 = null;
			objDESCrypto.Key = byteHash;
			objDESCrypto.Mode = CipherMode.ECB; //CBC, CFB

			return objDESCrypto.CreateEncryptor().TransformFinalBlock(unencrypted, 0, unencrypted.Length);
		}

		/// <summary>
		/// Decrypts a string a returns the result
		/// </summary>
		/// <param name="encrypted">Encrypted value that will be decrypted</param>
		/// <param name="password">Password used to decrypt the value</param>
		/// <returns>A decrypted string</returns>
		public static byte[] Decrypt(byte[] encrypted, SecureString password)
		{
			TripleDESCryptoServiceProvider objDESCrypto = new TripleDESCryptoServiceProvider();
			MD5CryptoServiceProvider objHashMD5 = new MD5CryptoServiceProvider();
			byte[] byteHash;

			byteHash = objHashMD5.ComputeHash(StringToBytes(ToUnsecureString(password)));
			objHashMD5 = null;
			objDESCrypto.Key = byteHash;
			objDESCrypto.Mode = CipherMode.ECB; //CBC, CFB

			var decrypted = objDESCrypto.CreateDecryptor().TransformFinalBlock(encrypted, 0, encrypted.Length);
			objDESCrypto = null;
			
			return decrypted;
		}

		public static void Encrypt(Stream input, Stream output, SecureString password)
		{
			SharpAESCrypt.SharpAESCrypt.Encrypt(ToUnsecureString(password), input, output);
		}

		public static void Decrypt(Stream input, Stream output, SecureString password)
		{
			SharpAESCrypt.SharpAESCrypt.Decrypt(ToUnsecureString(password), input, output);
		}

		/// <summary>
		/// Encrypts a file
		/// </summary>
		public static void Encrypt(string inputFilePath, string outputFilePath, SecureString password)
		{
			SharpAESCrypt.SharpAESCrypt.Encrypt(ToUnsecureString(password), inputFilePath, outputFilePath);
		}

		/// <summary>
		/// Decrypts a file
		/// </summary>
		public static void Decrypt(string inputFilePath, string outputFilePath, SecureString password)
		{
			SharpAESCrypt.SharpAESCrypt.Decrypt(ToUnsecureString(password), inputFilePath, outputFilePath);
		}

		/// <summary>
		/// Encrypts a string a returns the result
		/// </summary>
		/// <param name="strToEncrypt">Value that will be encrypted</param>
		/// <param name="password">Password used to encrypt the value</param>
		/// <returns>A encrypted string</returns>
		/// <remarks>
		/// Pretty safe, but the encrypted data will only be possible to decrypt on this same windows machine because we are using the windows account
		/// </remarks>
		public static string Encrypt(string strToEncrypt)
		{
			return BytesToBase64String(Encrypt(StringToBytes(strToEncrypt)));
		}

		/// <summary>
		/// Decrypts a string a returns the result
		/// </summary>
		/// <param name="strEncrypted">Encrypted value that will be decrypted</param>
		/// <param name="password">Password used to decrypt the value</param>
		/// <returns>A decrypted string</returns>
		/// <remarks>
		/// Pretty safe, but the encrypted data will only be possible to decrypt on this same windows machine because we are using the windows account
		/// </remarks>
		public static string Decrypt(string strEncrypted)
		{
			return BytesToString(Decrypt(Base64StringToBytes(strEncrypted)));
		}

		/// <summary>
		/// Encrypts data using local windows user security. 
		/// </summary>
		/// <param name="unencrypted">Data to encrypt</param>
		/// <returns>Encrypted data</returns>
		/// <remarks>
		/// Pretty safe, but the encrypted data will only be possible to decrypt on this same windows machine because we are using the windows account
		/// </remarks>
		public static byte[] Encrypt(byte[] unencrypted)
		{
			return ProtectedData.Protect(unencrypted, null, DataProtectionScope.CurrentUser);
		}

		/// <summary>
		/// Decrypts data using local windows user security. 
		/// </summary>
		/// <param name="encrypted">Data to decrypt</param>
		/// <returns>Decrypted data</returns>
		/// <remarks>
		/// Pretty safe, but the encrypted data will only be possible to decrypt on this same windows machine because we are using the windows account
		/// </remarks>
		public static byte[] Decrypt(byte[] encrypted)
		{
			return ProtectedData.Unprotect(encrypted, null, DataProtectionScope.CurrentUser);
		}

		/// <summary>
		/// Creates a random key for using in encryption
		/// </summary>
		/// <remarks>
		/// Taken from https://msdn.microsoft.com/en-us/library/ms229741%28v=vs.100%29.aspx
		/// </remarks>
		public static byte[] CreateRandomKey()
		{
			// Create a byte array to hold the random value.
			byte[] entropy = new byte[16];

			// Create a new instance of the RNGCryptoServiceProvider.
			// Fill the array with a random value.
			new RNGCryptoServiceProvider().GetBytes(entropy);

			// Return the array.
			return entropy;
		}

		/// <remarks>
		/// Taken from http://blogs.msdn.com/b/fpintos/archive/2009/06/12/how-to-properly-convert-securestring-to-string.aspx
		/// </remarks>
		public static string ToUnsecureString(SecureString securePassword)
		{
			if (securePassword == null)
				throw new ArgumentNullException("securePassword");

			IntPtr unmanagedString = IntPtr.Zero;

			try
			{
				unmanagedString = System.Runtime.InteropServices.Marshal.SecureStringToGlobalAllocUnicode(securePassword);
				return System.Runtime.InteropServices.Marshal.PtrToStringUni(unmanagedString);
			}
			finally
			{
				System.Runtime.InteropServices.Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
			}
		}

		/// <remarks>
		/// Taken from http://blogs.msdn.com/b/fpintos/archive/2009/06/12/how-to-properly-convert-securestring-to-string.aspx
		/// </remarks>
		public static SecureString ToSecureString(string password)
		{
			if (password == null)
			{
				throw new ArgumentNullException("password");
			}

			unsafe
			{
				fixed (char* passwordChars = password)
				{
					var securePassword = new SecureString(passwordChars, password.Length);
					securePassword.MakeReadOnly();
					return securePassword;
				}
			}
		}

		public static string BytesToString(byte[] data)
		{
			return Encoding.Unicode.GetString(data);
		}

		public static byte[] StringToBytes(string data)
		{
			return Encoding.Unicode.GetBytes(data);
		}

		public static string BytesToBase64String(byte[] data)
		{
			return Convert.ToBase64String(data);
		}

		public static byte[] Base64StringToBytes(string data)
		{
			return Convert.FromBase64String(data);
		}
	}
}