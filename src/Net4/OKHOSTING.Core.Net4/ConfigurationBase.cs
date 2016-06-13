using System;
using System.IO;
using System.Xml.Serialization;
using System.Reflection;

namespace OKHOSTING.Core.Net4
{
    /// <summary>
    /// Allows to create configuration sub-classes that are serialized 
    /// as read-write XML files in a "Config" folder at the target 
    /// Application
    /// <para xml:lang="es">
    /// Permite la creación de subclases de configuración que son 
    /// serializados como archivos XML de lectura-escritura en una
    /// carpeta "config" en la aplicación de destino
    /// </para>
    /// </summary>
    [Serializable]
	public abstract class ConfigurationBase
	{
		/// <summary>
		/// Indicates if the configuration must be saved
		/// when the object are destroyed
        /// <para xml:lang="es">
        /// Indica si la configuracion puede ser guardada
        /// cuando el objeto se ha destruido
        /// </para>
		/// </summary>
		private bool autoSave = false;

		/// <summary>
		/// Destroy the configuration
        /// <para xml:lang="es">
        /// Destruye la configuracion
        /// </para>
		/// </summary>
		~ConfigurationBase()
		{
			//Saving the configuration if apply
			if (AutoSave) Save();
		}

        /// <summary>
        /// Indicates if the configuration must be saved
        /// when the object are destroyed
        /// <para xml:lang="es">
        /// Indica si la configuracion puede ser guardada
        /// cuando el objeto se ha destruido
        /// </para>
        /// </summary>
        [XmlIgnoreAttribute]
		public bool AutoSave
		{
			get 
			{ 
				return autoSave; 
			}
			set 
			{ 
				autoSave = value; 
			}
		}

        /// <summary>
        /// Retrieve the path of the xml file for this configuration
        /// <para xml:lang="es">
        /// Recuperar la ruta del archivo XML para esta configuración
        /// </para>
        /// </summary>
        [XmlIgnoreAttribute]
		public string XmlPath
		{ 
			get 
			{ 
				return GetXmlPath(this.GetType()); 
			} 
		}

        /// <summary>
        /// Saves the configuration to his xml file
        /// <para xml:lang="es">
        /// Guarda la configuración de su archivo xml
        /// </para>
		/// </summary>
		public void Save()
		{
			//Local vars
			StreamWriter writer = null;

			try
			{
				//Creating the writer to the xml file for this configuration
				writer = new StreamWriter(XmlPath);

				//Creating the serializer to serialize this serializable class
				XmlSerializer serializer = new XmlSerializer(this.GetType());

				//Serializing the class and writing to disk
				serializer.Serialize(writer, this);
			}
			finally
			{
				//Closing the writer (if apply)
				if (writer != null) writer.Close();
			}
		}

        /// <summary>
        /// Retrieve the path of the xml file for the specified type
        /// <para xml:lang="es">
        /// Recuperar la ruta del archivo XML para el tipo especificado
        /// </para>
        /// </summary>
        /// <param name="type">
        /// Type that you desires to get his xml path
        /// <para xml:lang="es">
        /// Tipo que se desea obtener en su directorio xml
        /// </para>
        /// </param>
        /// <returns>
        /// Path of the xml file for the specified type
        /// <para xml:lang="es">
        /// Ruta del archivo XML para el tipo especificado
        /// </para>
        /// </returns>
        public static string GetXmlPath(Type type)
		{ 
			//Validating if the specified type is subclass of ConfiurationBase
			if (!type.IsSubclassOf(typeof(ConfigurationBase)))
				throw new ArgumentException(
					"The argument 'type' must be a sub class of ConfigurationBase",
					"type");

			//Returning the corresponding path
			return AppDomain.CurrentDomain.BaseDirectory + @"Config\" + type.FullName + @".config"; 
		}

        /// <summary>
        /// Load the configuration file for the specified type
        /// <para xml:lang="es">
        /// Cargar el archivo de configuración para el tipo especificado
        /// </para>
        /// </summary>
        /// <param name="type">
        /// Type that you desire to load his configuration file
        /// <para xml:lang="es">
        /// Tipo que se desea cargar del archivo de configuración
        /// </para>
        /// </param>
        /// <returns>
        /// Configuration for the specified type
        /// <para xml:lang="es">
        /// Configuracion para el tipo especificado
        /// </para>
        /// </returns>
        public static ConfigurationBase Load(Type type)
		{
			//Local Vars
			StreamReader reader = null;
			ConfigurationBase configuration = null;

			//Validating if the type is an ConfigurationBase of inherits of him
			if (!typeof(ConfigurationBase).IsAssignableFrom(type))
				throw new ArgumentException("Type '" + type.FullName + "' does not inherits from OKHOSTING.Core.ConfigurationBase");

			//Initializes the configuration file (if apply)
			InitializeConfigFile(type);

			try
			{
				//Reading the configuration file for the specified type
				reader = new StreamReader(GetXmlPath(type));

				//Creating serializer for the specified type
				XmlSerializer serializer = new XmlSerializer(type);

				//Getting the configuration instance
				configuration = (ConfigurationBase)serializer.Deserialize(reader);
			}
			finally
			{
				//Closing the reader if apply
				if (reader != null) reader.Close();
			}
			
			//Returning the configuration
			return configuration;
		}

        /// <summary>
        /// Initializes the config file for the specified type
        /// <para xml:lang="es">
        /// Inicia la configuracion del archivo para el tipo especificado
        /// </para>
        /// </summary>
        /// <param name="type">
        /// Type of subclass of ConfigurationBase used for initialization
        /// <para xml:lang="es">
        /// Tipo de subclase de Configuracion Base usada para la inicializacion
        /// </para>
        /// </param>
        private static void InitializeConfigFile(Type type)
		{
			//if configuration file does not exist, create empty configuration file
			if (!File.Exists(GetXmlPath(type)))
			{
				//Create Config folder if doesn't exist
				if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"Config\"))
					Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"Config\");

				//Create a new config and set default values so the config file is fully created
				ConfigurationBase conf = (ConfigurationBase) Activator.CreateInstance(type);
				
				//Set default values on fields
				foreach (FieldInfo field in conf.GetType().GetFields())
				{
					//Validating if the current field is not static
					if (!field.IsStatic)
					{
						try
						{
							//Validating if is a string field
							if (field.FieldType == typeof(string))
							{
								//Establishing default string value (if apply)
								if (string.IsNullOrWhiteSpace((string)field.GetValue(conf))) 
									field.SetValue(conf, "sample");
							}
							else
							{
								//Establishing default value to field
								if (field.GetValue(conf) == null)
									field.SetValue(conf, Activator.CreateInstance(field.FieldType));
							}
						}
						catch { /* The errors will be ignored */ }
					}
				}

				//Set default values on properties
				foreach (PropertyInfo property in conf.GetType().GetProperties())
				{
					try
					{
						//Validating if the current field is not static
						if (!property.GetGetMethod().IsStatic)
						{
							//Validating if is a string property
							if (property.PropertyType == typeof(string))
							{
								//Establishing default string value (if apply)
								if (string.IsNullOrWhiteSpace((string)property.GetValue(conf, null)))
									property.SetValue(conf, "sample", null);
							}
							else
							{
								//Establishing default value to property
								if (property.GetValue(conf, null) == null)
									property.SetValue(conf, Activator.CreateInstance(property.PropertyType), null);
							}
						}
					}
					catch { /* The errors will be ignored */ }
				}

				//Saving the file
				conf.Save();
			}
		}

        /// <summary>
        /// Loads and return the current configuration
        /// <para xml:lang="es">
        /// Carga y devuelve la actual configuracion
        /// </para>
        /// </summary>
        /// <typeparam name="ConfigurationClassType">
        /// TypeConfiguration
        /// <para xml:lang="es">
        /// Tipo de configuracion
        /// </para>
        /// </typeparam>
        /// <returns>
        /// return the current configuration
        /// <para xml:lang="es">
        /// Devuelve la actual configuracion
        /// </para>
        /// </returns>
		protected static ConfigurationClassType Current<ConfigurationClassType>() where ConfigurationClassType: ConfigurationBase
		{
			return (ConfigurationClassType) ConfigurationBase.Load(typeof(ConfigurationClassType));
		}
	}
}