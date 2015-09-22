using System.Net.NetworkInformation;

namespace OKHOSTING.Net.Net4
{
	public class InternetConnection
	{
		[System.Runtime.InteropServices.DllImport("wininet.dll", SetLastError = true)]
		private static extern bool InternetGetConnectedState(out int lpdwFlags, int dwReserved);

		/// <summary>
		/// Checks if the internet connection is active in the local computer
		/// </summary>
		public static bool IsConnectedToInternet()
		{
			int flags;
			return InternetGetConnectedState(out flags, 0);
		}

		/// <summary>
		/// Returns the MAC Address of the local network card
		/// </summary>
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