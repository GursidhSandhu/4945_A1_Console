using System;
using System.Net.Sockets;

namespace ConsoleApp
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // Use localhost and port 8080  to connect
            string serverIp = "localhost";
            int port = 8080;

            try
            {
                using (Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    clientSocket.Connect(serverIp, port);

                    if (clientSocket.Connected)
                    {
                        Console.WriteLine("Connected to the server!");
                        // Additional logic for data transmission and receiving JSON response will go here
                    }
                    else
                    {
                        Console.WriteLine("Unable to connect to the server.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            Console.ReadLine(); // Keep console open for viewing the result
        }
    }
}