/*
 * Project: A05-WebServer
 * File: Listener.cs
 * Programmers: Luke Alkema & Jacob Atienza
 * Date: 11/25/2024
 */

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace A05_WebServer
{
    /// <summary>
    /// Represents a TCP listener that uses the specified IP and port.
    /// </summary>
    public class Listener
    {
        private TcpListener _myListener;
        private bool _isRunning = true;

        /// <summary>
        /// Starts the listener asynchronously and processes incoming requests.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task StartAsync()
        {
            try
            {
                while (_isRunning)
                {
                    Socket mySocket = await _myListener.AcceptSocketAsync();

                    try
                    {
                        Request request = new Request(mySocket);
                        Response response = new Response(mySocket, request);
                        await response.ProcessAsync();
                    }
                    catch (SocketException)
                    {
                        // Ignore socket-related errors
                    }
                    catch (Exception ex)
                    {
                        Logger.Log($"Error handling request: {ex.Message}", "ERROR");
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Log($"Fatal error in listener: {e.Message}", "ERROR");
                throw;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Listener"/> class and starts the TCP listener.
        /// </summary>
        public Listener()
        {
            try
            {
                IPAddress localAddress = IPAddress.Parse(Arguments.WebIP);
                _myListener = new TcpListener(localAddress, Arguments.WebPort);
                _myListener.Start();
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to start listener: {e.Message}", "ERROR");
                throw;
            }
        }

        /// <summary>
        /// Stops the listener.
        /// </summary>
        public void Stop()
        {
            _isRunning = false;
            _myListener?.Stop();
        }
    }
}