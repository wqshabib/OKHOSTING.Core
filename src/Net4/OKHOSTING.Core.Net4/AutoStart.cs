/*
 * Downloaded from http://simpcode.blogspot.com/2008/07/c-set-and-unset-auto-start-for-windows.html
 * Modified By Leopoldo Arenas
 * 
*/

using Microsoft.Win32;
using System.IO;

namespace OKHOSTING.Core.Net4
{
    /// <summary>
    /// Allows an EXE application to autostart on windows startup
    /// <para xml:lang="es">
    /// Permite que una aplicación EXE para iniciar automáticamente al iniciar Windows
    /// </para>
    /// </summary>
    public class AutoStart
	{

        /// <summary>
        /// Registry location where autostart programs must be stored
        /// <para xml:lang="es">
        /// ubicación del registro donde se deben almacenar los programas de inicio automático
        /// </para>
        /// </summary>
        private const string RUN_LOCATION = @"Software\Microsoft\Windows\CurrentVersion\Run";

        /// <summary>
        /// Sets the autostart value for the specified assembly using the file name
        /// as Key name
        /// <para xml:lang="es">
        /// Establece el valor de inicio automático para el ensamblado especificado utilizando el nombre de archivo como nombre clave
        /// </para>
        /// </summary>
        /// <param name="assemblyLocation">
        /// Assembly location (e.g. Assembly.GetExecutingAssembly().Location)
        /// <para xml:lang="es">
        /// lugar de montaje (por ejemplo Assembly.GetExecutingAssembly (). Lugar)
        /// </para>
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
        /// <para xml:lang="es">
        /// Establece el valor de inicio automático para el montaje con el nombre
        /// clave especificada y la ruta de montaje
        /// </para>
        /// </summary>
        /// <param name="keyName">
        /// Registry Key Name
        /// <para xml:lang="es">
        /// Registro de Nombre de clave
        /// </para>
        /// </param>
        /// <param name="assemblyLocation">
        /// Assembly location (e.g. Assembly.GetExecutingAssembly().Location)
        /// <para xml:lang="es">
        /// lugar de montaje (por ejemplo Assembly.GetExecutingAssembly (). Lugar)
        /// </para>
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
        /// Returns true whether auto start is enabled based only on the assembly name
        /// <para xml:lang="es">
        /// Devuelve verdadero si arranque automático está activado basado sólo en el nombre de ensamblado
        /// </para>
        /// </summary>
        /// <param name="assemblyLocation">
        /// Assembly location (e.g. Assembly.GetExecutingAssembly().Location)
        /// <para xml:lang="es">
        /// lugar de montaje (por ejemplo Assembly.GetExecutingAssembly (). Lugar)
        /// </para>
        /// </param>
        public static bool IsAutoStartEnabled(string assemblyLocation)
		{return IsAutoStartEnabled(null, assemblyLocation); }

        /// <summary>
        /// Returns whether auto start is enabled based on assembly name and optionaly
        /// on assigned key name
        /// <para xml:lang="es">
        /// Devuelve si el inicio automático está activado basado en nombre de ensamblado
        /// y Opcionalmente el nombre de la clave asignada
        /// </para>
        /// </summary>
        /// <param name="keyName">
        /// Registry Key Name (if null the param is ignored)
        /// <para xml:lang="es">
        /// Registro de Nombre de clave (si es nulo el parámetro es ignorado)
        /// </para>
        /// </param>
        /// <param name="assemblyLocation">
        /// Assembly location (e.g. Assembly.GetExecutingAssembly().Location)
        /// <para xml:lang="es">
        /// lugar de montaje(por ejemplo Assembly.GetExecutingAssembly (). Lugar)
        /// </para>
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
        /// <para xml:lang="es">
        /// Desactiva el valor de inicio automático para el montaje con el nombre clave especificada
        /// </para>
        /// </summary>
        /// <param name="keyName">
        /// Registry Key Name
        /// <para xml:lang="es">
        /// Registro de Nombre de clave
        /// </para>
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
        /// <para xml:lang="es">
        /// Desactiva todos los valores de inicio automático para el conjunto especificado
        /// </para>
        /// </summary>
        /// <param name="assemblyLocation">
        /// Assembly location (e.g. Assembly.GetExecutingAssembly().Location)
        /// <para xml:lang="es">
        /// lugar de montaje(por ejemplo Assembly.GetExecutingAssembly (). Lugar)
        /// </para>
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