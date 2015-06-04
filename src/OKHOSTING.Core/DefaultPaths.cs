using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OKHOSTING.Core
{
	/// <summary>
	/// Defines define folder paths for common uses in applications
	/// </summary>
	public class DefaultPaths
	{
		/// <summary>
		/// Gets the base path where the application is running
		/// </summary>
		public static string Base
		{
			get
			{
				return AppDomain.CurrentDomain.BaseDirectory;
			}
		}

		/// <summary>
		/// Gets the custom directory path. This is a read-write directory where all custom files should be placed
		/// </summary>
		/// <remarks>
		/// Administrator must manually set read-write permissions to this directory (and subdirectories) on system setup
		/// </remarks>
		public static string Custom
		{
			get
			{
				return DefaultPaths.Base + @"Custom\";
			}
		}
	}
}