using System;

namespace OKHOSTING.Core.Test
{
	public class MyConfiguration: OKHOSTING.Core.Net4.ConfigurationBase
	{
		public string Value1 { get; set; }
		public DateTime Value2 { get; set; }
		public int Value3 { get; set; }
		public decimal Value4 { get; set; }
	}
}