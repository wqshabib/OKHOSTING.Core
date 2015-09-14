using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OKHOSTING.Core.Data
{
	public class EncryptedDictionary<TKey, TValue> : ProxyDictionary<TKey, TValue>
	{
		public bool EncryptKey { get; set; }
		public bool EncryptValue { get; set; }
		public string Password { get; set; }

		public override void Add(KeyValuePair<TKey, TValue> item)
		{
			//SharpAESCrypt.SharpAESCrypt.Encrypt(Password, localFullPath, encryptedFilePath);
			base.Add(item);
		}
	}
}