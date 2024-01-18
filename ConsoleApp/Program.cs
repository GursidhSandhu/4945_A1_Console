using System;
using System.Net.Sockets;
using System.Text;

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
                        // Read the content of the file (tst.txt) from the same directory
                        string filePath = "../../../tst.txt";
                        string fileContent = File.ReadAllText(filePath);

                        // Construct a POST request with form data and file content
                        string boundary = "----WebKitFormBoundaryABC123";
                        string formData = $"--{boundary}\r\n" +
                                          "Content-Disposition: form-data; name=\"myText\"\r\n\r\n" +
                                          "YourName\r\n" +
                                          $"--{boundary}\r\n" +
                                          $"Content-Disposition: form-data; name=\"filename\"; filename=\"{Path.GetFileName(filePath)}\"\r\n" +
                                          "Content-Type: text/plain\r\n\r\n" +
                                          $"{fileContent}\r\n" +
                                          $"--{boundary}--\r\n";

                        string userAgent = "MyCustomClientApp/1.0"; // Set our custom User-Agent here that the server will recognize

                        string postRequest = $"POST /form HTTP/1.1\r\n" +
                                             $"Host: {serverIp}\r\n" +
                                             $"Content-Type: multipart/form-data; boundary={boundary}\r\n" +
                                             $"Content-Length: {formData.Length}\r\n" +
                                             $"User-Agent: {userAgent}\r\n\r\n" +
                                             $"{formData}";

                        byte[] requestBytes = Encoding.UTF8.GetBytes(postRequest);

                        // Send the POST request to the server
                        clientSocket.Send(requestBytes);

                        // Read the response headers from the server
                        byte[] buffer = new byte[1024];
                        int bytesRead = clientSocket.Receive(buffer);
                        string responseHeaders = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                        Console.WriteLine("Response Headers:\n" + responseHeaders);

                        // Read and print the content of the response body (assuming JSON)
                        StringBuilder responseBody = new StringBuilder();
                        while (clientSocket.Available > 0)
                        {
                            bytesRead = clientSocket.Receive(buffer);
                            responseBody.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
                        }

                        Console.WriteLine("Response Body (JSON):\n" + responseBody.ToString());
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