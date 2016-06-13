using System;
using System.Diagnostics;

namespace OKHOSTING.Core.Net4
{
    /// <summary>
    /// Allows to execute shell (command line) commands very easily and
    /// keep track of the current working directory
    /// <para xml:lang="es">
    /// Permite ejecutar el entorno(línea de comandos) comandos muy fácilmente
    /// y no perder de vista el directorio de trabajo actual
    /// </para>
	/// </summary>
	public class ShellProxy
	{
        /// <summary>
        /// Current working directory
        /// <para xml:lamg="es">
        /// Directorio de trabajo actual
        /// </para>
        /// </summary>
        public string WorkingDirectory = Environment.CurrentDirectory;

        /// <summary>
        /// Executes a command on the shell
        /// <para xml:lang="es">
        /// Ejecuta un comando en el shell(Linea de comando)
        /// </para>
        /// </summary>
        /// <param name="command">
        /// Command (and optional arguments) to execute
        /// <para xml:lang="es">
        /// Comando(y argumentos opcionales) para ejecutar
        /// </para>
        /// </param>
        /// <example>dir</example>
        /// <example>cd c:\ && dir</example>
        public string Execute(string command)
		{
			//Defining process and its settings
			Process process = new Process();
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardInput = true;
			process.StartInfo.RedirectStandardError = true;
			process.StartInfo.ErrorDialog = false;
			process.StartInfo.WorkingDirectory = WorkingDirectory;
			process.StartInfo.FileName = "cmd.exe";

			//Starting command line (cmd.exe)
			process.Start();

			//Writting command and arguments
			process.StandardInput.WriteLine(command);
			process.StandardInput.Close();

			//Get output and error
			string output = process.StandardOutput.ReadToEnd().Trim();
			string error = process.StandardError.ReadToEnd().Trim();

			//Clossing the process
			process.Close();

			//Checking for errors
			if (!string.IsNullOrWhiteSpace(error)) throw new Exception(error);

			//get working directory
			WorkingDirectory = output.Substring(output.LastIndexOf(Environment.NewLine)).Trim().TrimEnd('>');
			
			/*
			 * Removing header from output (first 4 lines)
			 * 
			 * example:
			 * Microsoft Windows [Version 6.1.7600]
			 * Copyright (c) 2009 Microsoft Corporation. All rights reserved.
			 * 
			 * c:\>dir
			 */
			output = output.Substring(output.IndexOf(Environment.NewLine)).Trim();
			output = output.Substring(output.IndexOf(Environment.NewLine)).Trim();
			output = output.Substring(output.IndexOf(Environment.NewLine)).Trim();

			/*
			 * remove footer from output (last 2 lines)
			 * 
			 * example:
			 * 
			 * c:\>
			 */
			output = output.Substring(0, output.Length - WorkingDirectory.Length - 1).Trim();

			//Returning output
			return output;
		}
	}
}