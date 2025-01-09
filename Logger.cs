/*
 * Project: A05-WebServer
 * File: Logger.cs
 * Programmers: Luke Alkema & Jacob Atienza
 * Date: 11/25/2024
 */

using System;
using System.IO;


namespace A05_WebServer
{
	/// <summary>
	/// Provides logging functionality for the web server.
	/// </summary>
	public static class Logger
	{
		private static readonly string LogFilePath = "../../../WebServer.log";

		/// <summary>
		/// Logs a message to the log file.
		/// </summary>
		/// <param name="logMessage">The message to log.</param>
		/// <param name="eventType">The type of event to log (e.g., SERVER STARTED, REQUEST, RESPONSE).</param>
		public static void Log(string logMessage, string eventType)
		{
			try
			{
				// Write the log entry
				using (StreamWriter writer = File.AppendText(LogFilePath))
				{
					string timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
					writer.WriteLine($"{timeStamp} [{eventType}] - {logMessage}");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error: Could not write to log file.");
			}
		}

		/// <summary>
		/// Logs the server start event.
		/// </summary>
		/// <param name="webRoot">The root folder for the website data.</param>
		/// <param name="webIP">The IP address the server is listening on.</param>
		/// <param name="webPort">The port number the server is listening on.</param>
		public static void LogServerStart(string webRoot, string webIP, int webPort)
		{
			try
			{
				// Create or overwrite the log file
				File.WriteAllText(LogFilePath, string.Empty);

				// Log the server start message
				string logMessage = $"Server started with WebRoot: {webRoot}, IP: {webIP}, Port: {webPort}";
				Log(logMessage, "SERVER STARTED");
			}
			catch (Exception)
			{
				Console.WriteLine("Error: Could not write to log file.");
			}
		}
	}
}