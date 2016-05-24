using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OKHOSTING.Core
{
	/// <summary>
	/// Implements the bait and switch PCL trick which allows you to define a Type in a PCL, have different implementations for diferent platforms,
	/// and call the correct implementation in runtime by searching for intalled assemblies with predictable names and selecting the one (should be only one) installed
	/// <para xml:lang="es">
	/// Implementa el cebo y PCL truco que le permite definir un tipo en un PCL, tienen diferentes implementaciones para plataformas diferentes, y llamar a la correcta
	/// aplicación en tiempo de ejecución mediante la búsqueda de los conjuntos intalados con nombres predecibles y de seleccionar la (debe ser único) instalado
	/// </para>
	/// </summary>
	/// <remarks>
	/// Idea originally from http://log.paulbetts.org/the-bait-and-switch-pcl-trick/
	/// <para xml:lang="es">
	/// Idea originaria de http://log.paulbetts.org/the-bait-and-switch-pcl-trick/
	/// </para>
	/// </remarks>
	public static class BaitAndSwitch
	{
		/// <summary>
		/// Here we store the actual type (platform specific) that we must invoke when creating an object. You can just init this dictionary at app startup or use the Create
		/// method that takes possible platforms to search for the right imeplementation of the type you need
		/// <para xml:lang="es">
		/// Aquí almacenamos el tipo real (plataforma específica) que debemos invocar al crear un objeto. Puede simplemente init este diccionario en el inicio de aplicaciones
		/// o utilizar el método Create que se lleva a posibles plataformas para buscar la imeplementacion del tipo que necesitas
		/// </para>
		/// </summary>
		public readonly static Dictionary<Type, Type> PlatformSpecificTypes = new Dictionary<Type, Type>();

		/// <summary>
		/// Create a platform-specific object, for any feature you need to be platform-specific
		/// <para xml:lang="es">
		/// Crear un objeto específico de la plataforma, para cualquier función que debe ser específico para cada plataforma
		/// </para>
		/// </summary>
		/// <typeparam name="T">
		/// Type of the object. Can be any type of object
		/// <para xml:lang="es">
		/// Tipo del objeto. Puede ser cualquier tipo de objeto
		/// </para>
		/// </typeparam>
		/// <returns>
		/// An instance of type T
		/// <para xml:lang="es">
		/// Una instancia de tipo T
		/// </para>
		/// </returns>
		/// <remarks>
		/// Use this for platform specific features like storage, streaming, settings or whatever you need
		/// <para xml:lang="es">
		/// Utilice esta plataforma de características como el almacenamiento, streaming, ajustes o lo que usted necesita
		/// </para>
		/// </remarks>
		public static T Create<T>()
		{
			return Create<T>(null, null);
		}

		/// <summary>
		/// Create a platform-specific object, for any feature you need to be platform-specific
		/// <para xml:lang="es">
		/// Crea un objeto específico de la plataforma, para cualquier función que debe ser específico para cada plataforma
		/// </para>
		/// </summary>
		/// <typeparam name="T">
		/// Type of the object. Can be any type of object
		/// <para xml:lang="es">
		/// Tipo del objeto. Puede ser cualquier tipo de objeto
		/// </para>
		/// </typeparam>
		/// <param name="possiblePlatforms">
		/// Names of the platforms that a PCL library has as platform-specifc implementations. We will look in assemblies that are named the same
		/// as T's assembly with with a sufix for the platform name
		/// <para xml:lang="es">
		/// Los nombres de las plataformas que una biblioteca PCL tiene como implementaciones específicas de la plataforma. Vamos a ver en los
		/// ensamblados que se denominan igual que el montaje de T con un sufijo para el nombre de la plataforma
		/// </para>
		/// </param>
		/// <returns>
		/// An instance of type T
		/// <para xml:lang="es">
		/// Una instancia de tipo T
		/// </para>
		/// </returns>
		/// <remarks>
		/// Use this for platform specific features like storage, streaming, settings or whatever you need
		/// <para xml:lang="es">
		/// Utilice esta plataforma de características como el almacenamiento, streaming, ajustes o lo que usted necesita
		/// </para>
		/// </remarks>
		/// <example>
		/// If you are looking for an implementation of type OKHOSTING.Files.ILocalFileSystem and you provide possible platforms "Xamarin.Android" and "UWP"
		/// we will look into the assemblies named "OKHOSTING.Files.Xamarin.Android" and "OKHOSTING.Files.UWP" for one classes that imlements ILocalFileSystem and
		/// create an instance of that found type
		/// <para xml:lang="es">
		/// Si usted está buscando una aplicación de tipo OKHOSTING.Files.ILocalFileSystem y le proporcionará posibles plataformas "Xamarin.Android" y "uwp" vamos
		/// a ver en los conjuntos denominados "OKHOSTING.Files.Xamarin.Android" y "OKHOSTING.Files. UWP "para uno clases que imlementa ILocalFileSystem y crear
		/// una instancia de ese tipo que se encuentra
		/// </para>
		/// </example>
		public static T Create<T>(IEnumerable<string> possiblePlatforms, params object[] arguments)
		{
			Type baseType = typeof(T);

			//look for possible assemblies to see which one is installed, and save the corresponding type in PlatformSpecificTypes
			if (!PlatformSpecificTypes.ContainsKey(baseType))
			{
				if (possiblePlatforms == null || possiblePlatforms.Count() == 0)
				{
					throw new NotImplementedException("A platform-specific implementation was not found for type " + baseType);
				}

				Assembly platformSpecificAssembly = null;

				foreach (string platform in possiblePlatforms)
				{
					AssemblyName assemblyName = new AssemblyName(baseType.GetTypeInfo().Assembly.FullName);
					assemblyName.Name += "." + platform;

					//try using the patform as a sufix of the assembly name, using the base type T's assembly as name's prefix
					try
					{
						platformSpecificAssembly = Assembly.Load(assemblyName);
						break;
					}
					catch (System.IO.FileNotFoundException)
					{
					}

					//try again using the platform as the complete assembly name
					assemblyName = new AssemblyName(platform);

					try
					{
						platformSpecificAssembly = Assembly.Load(assemblyName);
						break;
					}
					catch (System.IO.FileNotFoundException)
					{
					}
				}

				if (platformSpecificAssembly == null)
				{
					throw new NotImplementedException("A platform-specific implementation was not found for type " + baseType);
				}

				IEnumerable<TypeInfo> platformSpecificType = null;

				if (baseType.GetTypeInfo().IsInterface)
				{
					platformSpecificType = platformSpecificAssembly.DefinedTypes.Where(dt => dt.ImplementedInterfaces.Contains(baseType) && !dt.IsAbstract).OrderBy(dt => dt.ImplementedInterfaces.Count());
				}
				else
				{
					platformSpecificType = platformSpecificAssembly.DefinedTypes.Where(dt => dt.IsSubclassOf(baseType) && !dt.IsAbstract).OrderBy(dt => Core.TypeExtensions.GetAllParents(dt.AsType()).Count());
				}

				if (platformSpecificType.Count() == 0)
				{
					throw new NotImplementedException("A platform-specific implementation was not found for type " + baseType);
				}
				else if (platformSpecificType.Count() > 1)
				{
					//throw new NotImplementedException("There is more than one platform-specific implementation for type " + baseType);
				}

				//get the one type that we must use
				PlatformSpecificTypes[baseType] = platformSpecificType.First().AsType();
			}

			//finally just create the instance
			return (T) Activator.CreateInstance(PlatformSpecificTypes[baseType], arguments);
		}
	}
}