using System.IO;

namespace OKHOSTING.Core.Net4
{
    /// <summary>
    /// Extension methods for System.IO.DirectoryInfo
    /// <para xml:lang="es">
    /// Los métodos de extensión para System.IO.DirectoryInfo
    /// </para>
    /// </summary>
    public static class DirectoryInfoExtensions
	{
        /// <summary>
        /// This method extends the DirectoryInfo class to return the size in bytes of the directory represented by the DirectoryInfo instance.
        /// <para xml:lang="es">
        /// Este método extiende la clase DirectoryInfo para devolver el tamaño en bytes del directorio representado por la instancia DirectoryInfo.
        /// </para>
        /// </summary>
        public static long GetSize(this DirectoryInfo dir)
		{
			long length = 0;

			// Loop through files and keep adding their size
			foreach (FileInfo nextfile in dir.GetFiles())
				length += nextfile.Exists ? nextfile.Length : 0;

			// Loop through subdirectories and keep adding their size
			foreach (DirectoryInfo nextdir in dir.GetDirectories())
				length += nextdir.Exists ? nextdir.GetSize() : 0;

			return length;
		}
	}
}
