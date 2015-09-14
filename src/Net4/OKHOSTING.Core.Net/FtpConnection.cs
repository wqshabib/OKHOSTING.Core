using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Sockets;
using EnterpriseDT.Net.Ftp;

namespace OKHOSTING.Core.Net
{
	/// <summary>
	/// Provides FTP client functionality
	/// </summary>
	/// <remarks>
	/// Based on EnterpriseDT.Net.Ftp.FTPConnection class, just 
	/// adds some aditional methods
	/// </remarks>
	public class FtpConnection: FTPConnection
	{
		/// <summary>
		/// Gets a value indicating whether a directory with the specified name, exists
		/// on the FTP server, inside the current conecction's directory
		/// </summary>
		/// <param name="directory">
		/// Name of the directory to look for
		/// </param>
		/// <returns>
		/// True if the directory exists, otherwise false
		/// </returns>
		public bool ExistsDirectory(string directory)
		{
			//Local Vars
			bool existsDirectory = false;

			//Validating argument
			if (string.IsNullOrWhiteSpace(directory)) throw new ArgumentNullException("directory");
			
			//Transforming to lower
			directory = directory.ToLower();
			
			//Crossing all files on current directory
			foreach (FTPFile file in base.GetFileInfos())
			{
				//Validating if the current file is the searched directory
				if (file.Name.ToLower() == directory && file.Dir)
				{
					existsDirectory = true;
					break;
				}
			}

			//Returning result
			return existsDirectory;
		}

		/// <summary>
		/// List the given directory's sub-directories
		/// </summary>
		/// <returns>
		/// A list of the sub-directories names
		/// </returns>
		public string[] GetDirectories()
		{ return GetDirectories(""); }

		/// <summary>
		/// List the given directory's sub-directories
		/// </summary>
		/// <param name="directory">
		/// Name of the directory (empty string = current directory)
		/// </param>
		/// <returns>
		/// A list of the sub-directories names
		/// </returns>
		public string[] GetDirectories(string directory)
		{
			//Local Vars
			List<string> directoriesList = new List<string>();

			//Crossing all ftp items searching directories
			foreach (FTPFile element in GetFileInfos(directory))
			{
				//Validating if the current element is a directory
				if (element.Dir) directoriesList.Add(element.Name);
			}

			//Returning fixed-size array
			return directoriesList.ToArray();
		}

		/// <summary>
		/// Creates all directories and subdirectories as specified by path, also changes the working directory to the new one
		/// </summary>
		/// <param name="path">
		/// The directory path to create
		/// </param>
		public override void CreateDirectory(string path)
		{
			//Validating argument
			if (string.IsNullOrWhiteSpace(path)) throw new ArgumentNullException("path");

			//Splitting path on directories
			string[] directories = path.Split('\\', '/');

			//Crossing directories
			foreach (string directory in directories)
			{
				//Validating that is valir
				if (!string.IsNullOrWhiteSpace(directory))
				{
					//Validating if diretory exists
					if (!ExistsDirectory(directory))
					{
						//Creating directory
						base.CreateDirectory(directory);
					}

					//Changing current diretory
					base.ChangeWorkingDirectory(directory);
				}
			}
		}

		/// <summary>
		/// Downloads a full directory with all 
		/// it's files and subdirectories
		/// </summary>
		/// <param name="directory">
		/// Remote server directory to be downloaded
		/// </param>
		public void DownloadDirectory(string directory)
		{
			//Validating if exists directory to download
			if (!ExistsDirectory(directory))
				throw new ArgumentException("Directory '" + directory + "' does not exists");

			//Backup original paths
			string localDirectoryOriginal = this.LocalDirectory;
			string serverDirectoryOriginal = this.ServerDirectory;

			//Changing the remote directory
			this.ChangeWorkingDirectory(directory);

			//Creating the directory locally (if dont exists)
			if (!Directory.Exists(this.LocalDirectory + '\\' + directory))
			{
				//Creating directory
				Directory.CreateDirectory(this.LocalDirectory + '\\' + directory);
			}

			//Changing local directory
			this.LocalDirectory = this.LocalDirectory + @"\" + directory;

			//Downloading all files
			foreach (FTPFile file in GetFileInfos())
			{
				//Ignoring directories
				if (!file.Dir)
				{
					//Validating if the local file must be deleted
					if (File.Exists(this.LocalDirectory + '\\' + file.Name))
						File.Delete(this.LocalDirectory + '\\' + file.Name);

					//Downloading file
					DownloadFile(file.Name, file.Name);
				}
			}

			//Downloading all subdirectories recursively
			foreach (string subdir in GetDirectories())
				DownloadDirectory(subdir);

			//Restoring original paths
			this.LocalDirectory = localDirectoryOriginal;
			this.ServerDirectory = serverDirectoryOriginal;
		}

		/// <summary>
		/// Download a file from the FTP server and save it locally
		/// </summary>
		/// <remarks>
		/// Transfers in the current <see cref="TransferType"/>
		/// </remarks>
		/// <param name="localPath">
		/// Local file to put data in
		/// </param>
		/// <param name="remoteFile">
		/// Name of remote file in current working directory
		/// </param>
		public override void DownloadFile(string localPath, string remoteFile)
		{
			//Transforming path to absolute path
			localPath = base.RelativePathToAbsolute(this.LocalDirectory, localPath);

			//Downloading...
			base.DownloadFile(localPath, remoteFile);

			//Set the right last modified date for the local file
			File.SetLastWriteTime(localPath, base.GetLastWriteTime(remoteFile));
		}
	}
}