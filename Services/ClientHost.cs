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


        public async Task<PackageDTO> RequestUserService(string methodName, object parameters)
        {
            try
            {
                var request = new
                {
                    service = "UserService",
                    method = methodName,
                    parameters = parameters
                };
                //Console.WriteLine("Request: " + JsonConvert.SerializeObject(request));

                string requestJson = JsonConvert.SerializeObject(request);
                byte[] requestBytes = Encoding.UTF8.GetBytes(requestJson);
                await stream.WriteAsync(requestBytes, 0, requestBytes.Length);

                byte[] responseBuffer = new byte[4096];
                int byteCount = await stream.ReadAsync(responseBuffer, 0, responseBuffer.Length);
                string responseJson = Encoding.UTF8.GetString(responseBuffer, 0, byteCount);
                Console.WriteLine("Response: " + responseJson);

                var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseJson);
                bool success = Convert.ToBoolean(response["Result"]);
                string message = response["Message"].ToString();
                object data = null;

                if (success)
                {
                    // Attempt to deserialize the response data to a dynamic object
                    try
                    {
                        data = JsonConvert.DeserializeObject<object>(response["Data"].ToString());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Received data could not be deserialized to a dynamic object.");
                    }
                }
                else
                {
                    Console.WriteLine("Operation failed: " + message);
                }

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
        public async Task<PackageDTO> RequestPresentationService(string methodName, object parameters)
        {

            try
            {
                var request = new
                {
                    service = "PresentationService",
                    method = methodName,
                    parameters = parameters
                };
                string requestJson = JsonConvert.SerializeObject(request);
                byte[] requestBytes = Encoding.UTF8.GetBytes(requestJson);
                await stream.WriteAsync(requestBytes, 0, requestBytes.Length);

                byte[] responseBuffer = new byte[4096];
                int byteCount = await stream.ReadAsync(responseBuffer, 0, responseBuffer.Length);
                string responseJson = Encoding.UTF8.GetString(responseBuffer, 0, byteCount);
                Console.WriteLine("Response: " + responseJson);

                var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseJson);
                bool success = Convert.ToBoolean(response["Result"]);
                string message = response["Message"].ToString();
                object data = null;

                if (success)
                {
                    // Attempt to deserialize the response data to a dynamic object
                    try
                    {
                        data = JsonConvert.DeserializeObject<object>(response["Data"].ToString());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Received data could not be deserialized to a dynamic object.");
                    }
                }
                else
                {
                    Console.WriteLine("Operation failed: " + message);
                }

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
        public async Task<PackageDTO> RequestParticipantService(string methodName, object parameters)
        {
            try
            {
                var request = new
                {
                    service = "ParticipantService",
                    method = methodName,
                    parameters = parameters
                };
                string requestJson = JsonConvert.SerializeObject(request);
                byte[] requestBytes = Encoding.UTF8.GetBytes(requestJson);
                await stream.WriteAsync(requestBytes, 0, requestBytes.Length);

                byte[] responseBuffer = new byte[4096];
                int byteCount = await stream.ReadAsync(responseBuffer, 0, responseBuffer.Length);
                string responseJson = Encoding.UTF8.GetString(responseBuffer, 0, byteCount);
                Console.WriteLine("Response: " + responseJson);

                var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseJson);
                bool success = Convert.ToBoolean(response["Result"]);
                string message = response["Message"].ToString();
                object data = null;

                if (success)
                {
                    // Attempt to deserialize the response data to a dynamic object
                    try
                    {
                        data = JsonConvert.DeserializeObject<object>(response["Data"].ToString());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Received data could not be deserialized to a dynamic object.");
                    }
                }
                else
                {
                    Console.WriteLine("Operation failed: " + message);
                }

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
    }

}
