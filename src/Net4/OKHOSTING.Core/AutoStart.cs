/*
 * Downloaded from http://simpcode.blogspot.com/2008/07/c-set-and-unset-auto-start-for-windows.html
 * Modified By Leopoldo Arenas
 * 
*/

using Microsoft.Win32;
using System.IO;

namespace OKHOSTING.Core
{

	/// <summary>
	/// Allows an EXE application to autostart on windows startup
	/// </summary>
	public class AutoStart
	{

		/// <summary>
		/// Registry location where autostart programs must be stored
		/// </summary>
		private const string RUN_LOCATION = @"Software\Microsoft\Windows\CurrentVersion\Run";

		/// <summary>
		/// Sets the autostart value for the specified assembly using the file name
		/// as Key name
		/// </summary>
		/// <param name="assemblyLocation">
		/// Assembly location (e.g. Assembly.GetExecutingAssembly().Location)
		/// </param>
		public static void SetAutoStart(string assemblyLocation)
		{ 
			//Getting the file name
			string KeyName = Path.GetFileNameWithoutExtension(assemblyLocation);

			//Establishing auto start 
			SetAutoStart(KeyName, assemblyLocation);
		}

		/// <summary>
		/// Sets the autostart value for the assembly with the specified Key name and
		/// assembly path
		/// </summary>
		/// <param name="keyName">
		/// Registry Key Name
		/// </param>
		/// <param name="assemblyLocation">
		/// Assembly location (e.g. Assembly.GetExecutingAssembly().Location)
		/// </param>
		public static void SetAutoStart(string keyName, string assemblyLocation)
		{
			//Local vars
			RegistryKey key = null;

			try
			{
				//Trying to open the corresponding key
				key = Registry.CurrentUser.CreateSubKey(RUN_LOCATION);

				//Establishing the specified value (if applies)
				if (key != null)
					key.SetValue(keyName, assemblyLocation);
				else
					throw new System.InvalidOperationException(
						"Can't set auto start for the specified assembly location because the registry key can't be opened");
			}
			finally
			{
				if (key != null)
				{
					key.Close();
					key = null;
				}
			}
		}

		/// <summary>
		/// Returns whether auto start is enabled based only on the assembly name
		/// </summary>
		/// <param name="assemblyLocation">
		/// Assembly location (e.g. Assembly.GetExecutingAssembly().Location)
		/// </param>
		public static bool IsAutoStartEnabled(string assemblyLocation)
		{return IsAutoStartEnabled(null, assemblyLocation); }

		/// <summary>
		/// Returns whether auto start is enabled based on assembly name and optionaly
		/// on assigned key name
		/// </summary>
		/// <param name="keyName">
		/// Registry Key Name (if null the param is ignored)
		/// </param>
		/// <param name="assemblyLocation">
		/// Assembly location (e.g. Assembly.GetExecutingAssembly().Location)
		/// </param>
		public static bool IsAutoStartEnabled(string keyName, string assemblyLocation)
		{
			//Local vars
			bool IsEnabled = false;
			RegistryKey key = null;

			try
			{
				//Trying to open the registry key
				key = Registry.CurrentUser.OpenSubKey(RUN_LOCATION);

				//Transforming assemblyLocation argument for comparisons
				assemblyLocation = assemblyLocation.ToLower().Trim();

				//Validating if the key could be opened
				if (key != null)
				{
					//Valiating if must search by key name
					if (keyName != null)
					{
						//Loading assenbly auto start key value
						string value = (string)key.GetValue(keyName);

						//Validating if the value could be loaded
						if (value != null) IsEnabled = (value.ToLower().Trim() == assemblyLocation);
					}
					else
					{
						//Loading the value names
						string[] valueNames = key.GetValueNames();

						//Crossing sub keys
						for (int counter = 0; counter < valueNames.GetLength(0); counter++)
						{
							//Loading assenbly auto start key value
							string value = (string)key.GetValue(valueNames[counter]);

							//Validating if the value could be loaded
							if (value != null && value.ToLower().Trim() == assemblyLocation)
							{
								IsEnabled = true;
								break;
							}
						}
					}
				}
			}
			finally 
			{
				//Clossing the key if applies
				if (key != null)
				{
					key.Close();
					key = null;
				}
			}


			//Returning the value
			return IsEnabled;
		}

		/// <summary>
		/// Unsets the autostart value for the assembly with the specified key name
		/// </summary>
		/// <param name="keyName">
		/// Registry Key Name
		/// </param>
		public static void UnSetAutoStart(string keyName)
		{
			//Local vars
			RegistryKey key = null;

			try
			{
				//Trying to open the corresponding key
				key = Registry.CurrentUser.CreateSubKey(RUN_LOCATION);

				//Deleting the specified value (if applies)
				if (key != null)
					key.DeleteValue(keyName);

				else
					throw new System.InvalidOperationException(
						"Can't quit auto start for the specified key name because the registry key can't be opened");
			}
			finally
			{
				//Clossing the key if applies
				if (key != null)
				{
					key.Close();
					key = null;
				}
			}
		}

		/// <summary>
		/// Unsets all the auto-start values for the specified assembly
		/// </summary>
		/// <param name="assemblyLocation">
		/// Assembly location (e.g. Assembly.GetExecutingAssembly().Location)
		/// </param>
		public static void UnSetAllAutoStartsForAssembly(string assemblyLocation)
		{
			//Local vars
			RegistryKey key = null;

			try
			{
				//Trying to open the corresponding key
				key = Registry.CurrentUser.CreateSubKey(RUN_LOCATION);

				//Transforming assemblyLocation argument for comparisons
				assemblyLocation = assemblyLocation.ToLower().Trim();

				//Crossing subkeys
				if (key != null)
				{
					//Loading the sub keys names
					string[] valueNames = key.GetValueNames();

					//Crossing sub keys
					for (int counter = 0; counter < valueNames.GetLength(0); counter++)
					{
						//Loading assenbly auto start key value
						string keyName = valueNames[counter];
						string value = ((string)key.GetValue(keyName)).Trim().ToLower();

						//Deleting autostart if is of the searched assembly
						if (value == assemblyLocation) key.DeleteValue(keyName);
					}
				}
			}
			finally
			{
				//Clossing the key if applies
				if (key != null)
				{
					key.Close();
					key = null;
				}
			}
		}

	}
}