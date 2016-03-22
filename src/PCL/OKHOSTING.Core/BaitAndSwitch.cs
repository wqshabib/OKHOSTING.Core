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
	/// </summary>
	/// <remarks>
	/// Idea originally from http://log.paulbetts.org/the-bait-and-switch-pcl-trick/
	/// </remarks>
	public static class BaitAndSwitch
	{
		/// <summary>
		/// Here we store the actual type (platform specific) that we must invoke when creating an object. You can just init this dictionary at app startup or use the Create
        /// method that takes possible platforms to search for the right imeplementation of the type you need
		/// </summary>
		public readonly static Dictionary<Type, Type> PlatformSpecificTypes = new Dictionary<Type, Type>();

		/// <summary>
		/// Create a platform-specific object, for any feature you need to be platform-specific
		/// </summary>
		/// <typeparam name="T">Type of the object. Can be any type of object</typeparam>
		/// <returns>
		/// An instance of type T
		/// </returns>
		/// <remarks>
		/// Use this for platform specific features like storage, streaming, settings or whatever you need
		/// </remarks>
		public static T Create<T>()
		{
			return Create<T>(null, null);
		}

        /// <summary>
		/// Create a platform-specific object, for any feature you need to be platform-specific
		/// </summary>
		/// <typeparam name="T">Type of the object. Can be any type of object</typeparam>
		/// <returns>
		/// An instance of type T
		/// </returns>
		/// <remarks>
		/// Use this for platform specific features like storage, streaming, settings or whatever you need
		/// </remarks>
		public static T Create<T>(params object[] arguments)
        {
            return Create<T>(null, arguments);
        }

        /// <summary>
        /// Create a platform-specific object, for any feature you need to be platform-specific
        /// </summary>
        /// <typeparam name="T">Type of the object. Can be any type of object</typeparam>
        /// <param name="possiblePlatforms">
        /// Names of the platforms that a PCL library has as platform-specifc implementations. We will look in assemblies that are named the same
        /// as T's assembly with with a sufix for the platform name
        /// </param>
        /// <returns>
        /// An instance of type T
        /// </returns>
        /// <remarks>
        /// Use this for platform specific features like storage, streaming, settings or whatever you need
        /// </remarks>
        /// <example>
        /// If you are looking for an implementation of type OKHOSTING.Files.ILocalFileSystem and you provide possible platforms "Xamarin.Android" and "UWP"
        /// we will look into the assemblies named "OKHOSTING.Files.Xamarin.Android" and "OKHOSTING.Files.UWP" for one classes that imlements ILocalFileSystem and
        /// create an instance of that found type
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