using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OKHOSTING.Core.Data
{
	public class EncryptedDictionary<TKey, TValue> : ProxyDictionary<TKey, TValue>
	{
		public readonly bool EncryptKey = true;
		public readonly bool EncryptValue = true;
		public readonly string Password;

		public EncryptedDictionary(Dictionary<TKey, TValue> source, bool encryptKey, bool encryptValue, string password): base(source)
		{
			if (source == null) throw new ArgumentNullException("source");
			if (password == null) throw new ArgumentNullException("password");
			
			EncryptKey = encryptKey;
			EncryptValue = encryptValue;
			Password = password;
		}

		public override void Add(KeyValuePair<TKey, TValue> item)
		{

			//SharpAESCrypt.SharpAESCrypt.Encrypt(Password, localFullPath, encryptedFilePath);

			base.Add(item);
		}
	}
}