using System.Net.NetworkInformation;

namespace OKHOSTING.Core.Net4.Net
{
    /// <summary>
    /// It represents the internet
    /// <para xml:lang="es">
    /// Representa la conexión a internet
    /// </para>
    /// </summary>
    public class InternetConnection
	{
        /// <summary>
        /// Determines whether the status of the Internet connection is active
        /// <para xml:lang="es">
        /// Determina si el estado de la conexion a internet esta activa
        /// </para>
        /// </summary>
        /// <param name="lpdwFlags">
        /// 
        /// <para xml:lang="es">
        /// 
        /// </para>
        /// </param>
        /// <param name="dwReserved">
        /// 
        /// <para xml:lang="es">
        /// 
        /// </para>
        /// </param>
        /// <returns>
        /// 
        /// <para xml:lang="es">
        /// 
        /// </para>
        /// </returns>
		[System.Runtime.InteropServices.DllImport("wininet.dll", SetLastError = true)]
		private static extern bool InternetGetConnectedState(out int lpdwFlags, int dwReserved);

        /// <summary>
        /// Determines whether he is connected to internet
        /// <para xml:lang="es">
        /// Determina si se esta conectado a internet
        /// </para>
        /// </summary>
        /// <returns>
        /// Returns true if there is internet connection, otherwise it returns false
        /// <para xml:lang="es">
        /// Devuelve true si hay conexion a internet, de lo contrario devuelve false
        /// </para>
        /// </returns>
        public static bool IsConnectedToInternet()
		{
			int flags;
			return InternetGetConnectedState(out flags, 0);
		}

        /// <summary>
        /// Returns the MAC Address of the local network card
        /// <para xml:lang="es">
        /// Devuelve la dirección MAC de la tarjeta de red local
        /// </para>
        /// </summary>
        /// <returns>
        /// Returns the MAC Address
        /// <para xml:lang="es">
        /// Devuelve la dirección MAC
        /// </para>
        /// </returns>
        public static PhysicalAddress GetMacAddress()
		{
			foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
			{
				// Only consider Ethernet network interfaces
				if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet && nic.OperationalStatus == OperationalStatus.Up)
				{
					return nic.GetPhysicalAddress();
				}
			}

			return null;
		}
	}
}