/*
 * Project: A05-WebServer
 * File: Response.cs
 * Programmers: Luke Alkema & Jacob Atienza
 * Date: 11/25/2024
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace A05_WebServer
{
    /// <summary>
    /// Represents an HTTP response sent by the web server.
    /// </summary>
    public class Response
    {
        private Socket _client;
        private Request _request;

        private Dictionary<string, string> mimeTypes = new Dictionary<string, string>()
                        {
                            { ".html", "text/html" },
                            { ".htm", "text/html" },
                            { ".jpg", "image/jpeg" },
                            { ".jpeg", "image/jpeg" },
                            { ".txt", "text/plain" },
                            { ".gif", "image/gif" }
                        };

        /// <summary>
        /// Initializes a new instance of the <see cref="Response"/> class.
        /// </summary>
        /// <param name="client">The client socket to send the response to.</param>
        /// <param name="request">The HTTP request to respond to.</param>
        public Response(Socket client, Request request)
        {
            _client = client;
            _request = request;
        }

        /// <summary>
        /// Processes the HTTP request and sends the appropriate response.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task ProcessAsync()
        {
            string filePath = Path.Combine(Arguments.WebRoot, _request.RequestedFile);

            if (!File.Exists(filePath))
            {
                string originalExtension = Path.GetExtension(filePath).ToLower();
                switch (originalExtension)
                {
                    case ".html":
                        filePath = Path.ChangeExtension(filePath, ".htm");
                        break;
                    case ".htm":
                        filePath = Path.ChangeExtension(filePath, ".html");
                        break;
                    case ".jpg":
                        filePath = Path.ChangeExtension(filePath, ".jpeg");
                        break;
                    case ".jpeg":
                        filePath = Path.ChangeExtension(filePath, ".jpg");
                        break;
                }
                if (!File.Exists(filePath))
                {
                    await SendErrorAsync(404, "File Not Found");
                    return;
                }
            }

            try
            {
                byte[] fileBytes = File.ReadAllBytes(filePath);
                string mimeType = GetMimeType(Path.GetExtension(filePath));
                await SendHeaderAsync("200 OK", mimeType, fileBytes.Length);
                await SendToClientAsync(fileBytes);
            }
            catch (Exception ex)
            {
                await SendErrorAsync(500, "Internal Server Error");
            }
            finally
            {
                await CloseSocketAsync();
            }
        }

        /// <summary>
        /// Sends the HTTP header to the client.
        /// </summary>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <param name="contentType">The content type of the response.</param>
        /// <param name="contentLength">The length of the response content.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task SendHeaderAsync(string statusCode, string contentType, int contentLength)
        {
            string header = $"HTTP/1.1 {statusCode}\r\n" +
                            $"Content-Type: {contentType}\r\n" +
                            $"Content-Length: {contentLength}\r\n" +
                            $"Server: MyOwnWebServer/1.0\r\n" +
                            $"Date: {DateTime.UtcNow:R}\r\n\r\n";
            await SendToClientAsync(Encoding.ASCII.GetBytes(header));
        }

        /// <summary>
        /// Sends data to the client.
        /// </summary>
        /// <param name="data">The data to send.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task SendToClientAsync(byte[] data)
        {
            await _client.SendAsync(new ArraySegment<byte>(data), SocketFlags.None);
        }

        /// <summary>
        /// Sends an error response to the client.
        /// </summary>
        /// <param name="code">The HTTP status code.</param>
        /// <param name="message">The error message.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task SendErrorAsync(int code, string message)
        {
            string status = $"{code} {message}";
            string body = $"<h1>{status}</h1>";

            string header = $"HTTP/1.1 {status}\r\n" +
                            $"Content-Type: text/html\r\n" +
                            $"Content-Length: {body.Length}\r\n" +
                            $"Server: MyOwnWebServer\r\n" +
                            $"Date: {DateTime.UtcNow:R}\r\n\r\n";

            Logger.Log(status, "RESPONSE");

            await SendToClientAsync(Encoding.ASCII.GetBytes(header));
            await SendToClientAsync(Encoding.ASCII.GetBytes(body));
        }

        /// <summary>
        /// Gets the MIME type for the given file extension.
        /// </summary>
        /// <param name="extension">The file extension.</param>
        /// <returns>The MIME type.</returns>
        private string GetMimeType(string extension)
        {
            return mimeTypes.TryGetValue(extension.ToLower(), out var mimeType)
                ? mimeType
                : "application/octet-stream";
        }

        /// <summary>
        /// Closes the client socket.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task CloseSocketAsync()
        {
            if (_client != null)
            {
                try
                {
                    await Task.Run(() => _client.Shutdown(SocketShutdown.Both));
                }
                catch (SocketException ex)
                {
                    Logger.Log("Socket shutdown error: " + ex.Message, "ERROR");
                }
                finally
                {
                    _client.Close();
                }
            }
        }
    }
}
