/*
 * Project: A05-WebServer
 * File: Request.cs
 * Programmers: Luke Alkema & Jacob Atienza
 * Date: 11/25/2024
 */

using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using static System.Net.WebRequestMethods;

namespace A05_WebServer
{
    /// <summary>
	//  Represents an HTTP request received by the web server, 
    /// parses the request method, requested file, and HTTP version.
	/// </summary>
	public class Request
	{
		public string HttpMethod { get; private set; } = "GET";
		public string HttpVersion { get; private set; } = "HTTP/1.1";
		public string RequestedFile { get; private set; } = "";
		public string Directory { get; private set; } = "/";

		/// <summary>
		/// Initializes a new instance of the <see cref="Request"/> class by parsing the request from the client socket.
		/// </summary>
		/// <param name="client"></param>
		public Request(Socket client)
		{
			byte[] buffer = new byte[1024];
			int recievedBytes = client.Receive(buffer);
			string request = Encoding.ASCII.GetString(buffer, 0, recievedBytes);

			Parse(request);
		}
		/// <summary>
		/// Parses the HTTP request string and extracts the method, requested file, and HTTP version.
		/// </summary>
		/// <param name="request">The raw HTTP request string.</param>
		private void Parse(string request)
		{
			try
			{
				string[] lines = request.Split('\n');
				if (lines.Length > 0)
				{
					string[] requestLine = lines[0].Split(' ');
					if (requestLine.Length >= 2)
					{
						HttpMethod = requestLine[0].ToUpper();
						string fullPath = requestLine[1];

						Directory = Path.GetDirectoryName(fullPath) ?? "/";
						RequestedFile = fullPath.TrimStart('/');

						if (HttpMethod != "GET")
						{
							Logger.Log("Only Get Method is supported", HttpMethod);
						}
						if (string.IsNullOrEmpty(RequestedFile))
						{
							RequestedFile = "index.html";
						}

						if (requestLine.Length >= 3)
						{
							HttpVersion = requestLine[2].Trim();
						}

						// Log the HTTP verb and resource
						Logger.Log($"{HttpMethod} {fullPath}", "REQUEST");
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log($"Error parsing request: {ex.Message}", "ERROR");
				throw;
			}
		}
	}
}
