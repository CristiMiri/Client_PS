using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Client.DTO;


namespace Client.Services
{
    internal class ClientHost
    {
        private static readonly Lazy<ClientHost> instance = new Lazy<ClientHost>(() => new ClientHost());
        private TcpClient? client;
        private NetworkStream? stream;


        private ClientHost() { ConnectToServer(); }
        // Public property to provide access to the singleton instance
        public static ClientHost Instance => instance.Value;

        public async Task ConnectToServer()
        {
            try
            {
                string serverIp = "127.0.0.1";
                int port = 8001;

                client = new TcpClient();
                await client.ConnectAsync(serverIp, port);
                stream = client.GetStream();
                Console.WriteLine($"Connected to server on {serverIp}:{port}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to connect to server: {ex.Message}");
            }
        }


        public async Task<PackageDTO> RequestService(string serviceName, string methodName, object parameters)
        {
            try
            {
                // Create the request object
                var request = new
                {
                    service = serviceName,
                    method = methodName,
                    parameters = parameters
                };

                // Send the request to the server
                string requestJson = JsonConvert.SerializeObject(request);
                byte[] requestBytes = Encoding.UTF8.GetBytes(requestJson);
                await stream.WriteAsync(requestBytes, 0, requestBytes.Length);

                // Read the length of the response first
                byte[] lengthBuffer = new byte[4];
                await stream.ReadAsync(lengthBuffer, 0, lengthBuffer.Length);
                int responseLength = BitConverter.ToInt32(lengthBuffer, 0);

                // Read the actual response
                byte[] responseBuffer = new byte[responseLength];
                int totalBytesRead = 0;
                while (totalBytesRead < responseLength)
                {
                    int bytesRead = await stream.ReadAsync(responseBuffer, totalBytesRead, responseLength - totalBytesRead);
                    if (bytesRead == 0)
                    {
                        // Handle connection closed prematurely
                        throw new Exception("Connection closed prematurely");
                    }
                    totalBytesRead += bytesRead;
                }
                string responseJson = Encoding.UTF8.GetString(responseBuffer, 0, responseLength);
                //Console.WriteLine("Response: " + responseJson);

                // Deserialize the response
                var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseJson);
                bool success = Convert.ToBoolean(response["Result"]);
                string message = response["Message"].ToString();
                object data = null;

                if (success)
                {
                    // Attempt to deserialize the response data to a dynamic object
                    data = JsonConvert.DeserializeObject<object>(response["Data"].ToString());
                }
                else
                {
                    Console.WriteLine("Operation failed: " + message);
                }

                // Return the response
                return new PackageDTO
                {
                    Result = success,
                    Message = message,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred during request: " + ex.Message);
                return new PackageDTO { Result = false, Message = "Error occurred during request", Data = null };
            }
        }


        public Task<PackageDTO> RequestUserService(string methodName, object parameters)
        {
            return RequestService("UserService", methodName, parameters);
        }
        public Task<PackageDTO> RequestPresentationService(string methodName, object parameters)
        {
            return RequestService("PresentationService", methodName, parameters);
        }
        public Task<PackageDTO> RequestParticipantService(string methodName, object parameters)
        {
            return RequestService("ParticipantService", methodName, parameters);
        }
        public Task<PackageDTO> RequestStatisticsService(string methodName, object parameters)
        {
            return RequestService("StatisticsService", methodName, parameters);
        }

    }

}
