using System;
using OKHOSTING.Core.Net4;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OKHOSTING.Core.Test
{
	[TestClass]
	public class Net4Test
	{
		[TestMethod]
		public void AppConfigTest()
		{
			AppConfig config = new AppConfig();
			string connectrionString = "my connection string";

			config.SetAppSetting("connectionString", connectrionString);
			config.Save();

			Assert.Equals((string) config.GetValue("connectionString", typeof(string)), connectrionString);
		}

		[TestMethod]
		public void AutoStartTest()
		{
			string location = System.Reflection.Assembly.GetExecutingAssembly().Location;

			AutoStart.SetAutoStart(location);
			Assert.IsTrue(AutoStart.IsAutoStartEnabled(location));

			AutoStart.UnSetAutoStart(location);
			Assert.IsFalse(AutoStart.IsAutoStartEnabled(location));
		}

		[TestMethod]
		public void ConfigurationTest()
		{
			var config = new MyConfiguration();
			config.Value1 = "hello";
			config.Value2 = DateTime.Today;
			config.Value3 = 100;
			config.Value4 = 12.5M;
			
			//save to disk
			config.Save();

			//then load it from disk
			config = (MyConfiguration) ConfigurationBase.Load(typeof(MyConfiguration));

			Assert.Equals(config.Value1, "hello");
			Assert.Equals(config.Value2, DateTime.Today);
			Assert.Equals(config.Value3, 100);
			Assert.Equals(config.Value4, 12.5M);
		}
	}
}