/*
 * Project: A05-WebServer
 * File: Arguments.cs
 * Programmers: Luke Alkema & Jacob Atienza
 * Date: 11/25/2024
 */

using System;

namespace A05_WebServer
{
    /// <summary>
    /// Provides methods and properties to handle command line arguments for the web server.
    /// </summary>
    public static class Arguments
    {
        public static string WebRoot { get; set; }
        public static string WebIP { get; set; }
        public static int WebPort { get; set; }

        /// <summary>
        /// Parses the command line arguments and sets the corresponding properties.
        /// </summary>
        /// <param name="args">An array of command line arguments.</param>
        /// <returns>True if arguments are parsed successfully if not, returns false.</returns>
        public static bool parseArguments(string[] args)
        {
            try
            {
                if (args.Length != 3 ||
                    args[0].Split('=')[0] != "-webRoot" ||
                    args[1].Split('=')[0] != "-webIP" ||
                    args[2].Split('=')[0] != "-webPort")
                {
                    SetDefaultValues();
                    return true;
                }

                WebRoot = args[0].Split('=')[1];
                WebIP = args[1].Split('=')[1];
                int.TryParse(args[2].Split('=')[1], out int tempPort);
                WebPort = tempPort;

                return true;
            }
            catch (Exception ex)
            {
                Logger.Log("Command line arguments are in an invalid format", "SERVER STARTED");
                SetDefaultValues();
                return true;
            }

        }
        private static void SetDefaultValues()
        {
            WebRoot = "../../../DefaultServerFiles";
            WebIP = "127.0.0.1";
            WebPort = 8080;
        }
    }
}