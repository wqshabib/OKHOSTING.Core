using System;
using System.IO;
using System.Linq;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.DataProtection;
using Windows.Storage.Streams;

namespace OKHOSTING.Cryptography.UWP
{
	/// <summary>
	/// Encrypts and decrypts strings, streams and files in a very simple, yet secure way
	/// </summary>
	public class SimpleEncryption: OKHOSTING.Cryptography.SimpleEncryption
	{
		protected const string LocalUser = "LOCAL=user";
		protected const string LocalMachine = "LOCAL=machine";

		/// <summary>
		/// Encrypts data using local windows user security. 
		/// </summary>
		/// <param name="unencrypted">Data to encrypt</param>
		/// <returns>Encrypted data</returns>
		/// <remarks>
		/// Pretty safe, but the encrypted data will only be possible to decrypt on this same windows machine because we are using the windows account
		/// </remarks>
		public byte[] Encrypt(byte[] unencrypted)
		{
			DataProtectionProvider provider = new DataProtectionProvider(LocalUser);
			IBuffer buffer = CryptographicBuffer.CreateFromByteArray(unencrypted);

			var encryptedBuffer = provider.UnprotectAsync(buffer).GetResults();
			byte[] encrypted = new byte[encryptedBuffer.Length];
			CryptographicBuffer.CopyToByteArray(encryptedBuffer, out encrypted);

			return encrypted;
		}

		/// <summary>
		/// Decrypts data using local windows user security. 
		/// </summary>
		/// <param name="encrypted">Data to decrypt</param>
		/// <returns>Decrypted data</returns>
		/// <remarks>
		/// Pretty safe, but the encrypted data will only be possible to decrypt on this same windows machine because we are using the windows account
		/// </remarks>
		public byte[] Decrypt(byte[] encrypted)
		{
			DataProtectionProvider provider = new DataProtectionProvider(LocalUser);
			IBuffer buffer = CryptographicBuffer.CreateFromByteArray(encrypted);

			var decryptedBuffer = provider.UnprotectAsync(buffer).GetResults();
			byte[] decrypted = new byte[decryptedBuffer.Length];
			CryptographicBuffer.CopyToByteArray(decryptedBuffer, out decrypted);

			return decrypted;
        }

		public async void Encrypt(Stream input, Stream output)
		{
			var outputStream = output.AsOutputStream();
			DataProtectionProvider provider = new DataProtectionProvider(LocalUser);

			await provider.ProtectStreamAsync(input.AsInputStream(), outputStream);
		}

		public async void Decrypt(Stream input, Stream output)
		{
			var outputStream = output.AsOutputStream();
			DataProtectionProvider provider = new DataProtectionProvider(LocalUser);

			await provider.UnprotectStreamAsync(input.AsInputStream(), outputStream);
		}

		#region Support methods

		/// <summary>
		/// Encrypts a file
		/// </summary>
		public void EncryptFile(string inputFilePath, string outputFilePath)
		{
			Encrypt(File.OpenRead(inputFilePath), File.OpenWrite(outputFilePath));
		}

		/// <summary>
		/// Decrypts a file
		/// </summary>
		public void DecryptFile(string inputFilePath, string outputFilePath)
		{
			Decrypt(File.OpenRead(inputFilePath), File.OpenWrite(outputFilePath));
		}

		#endregion
	}
}