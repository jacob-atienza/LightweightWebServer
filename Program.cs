/*
 * Project: A05-WebServer
 * File: Program.cs
 * Programmers: Luke Alkema & Jacob Atienza
 * Date: 11/25/2024
 */

using System;
using System.Threading.Tasks;
namespace A05_WebServer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            if (!Arguments.parseArguments(args))
            {
                return;
            }

            try
            {
                Logger.LogServerStart(Arguments.WebRoot, Arguments.WebIP, Arguments.WebPort);
                Listener listener = new Listener();
                await listener.StartAsync();
            }
            catch (Exception ex)
            {
                Logger.Log($"Fatal Error: {ex.Message}", "ERROR");
                Console.WriteLine($"Fatal Error: {ex.Message}");
            }
        }
    }
}