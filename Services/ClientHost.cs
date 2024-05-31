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
        private TcpClient? client;
        private NetworkStream? stream;
        //private SslStream? s;
        public ClientHost() { }

        public async 
        Task
ConnectToServer()
        {
            try
            {
                string serverIp = "127.0.0.1";
                int port = 8001;

                client = new TcpClient();
                await client.ConnectAsync(serverIp, port);
                stream = client.GetStream();
                //StatusTextBlock.Text = $"Connected to server on {serverIp}:{port}.";
                Console.WriteLine($"Connected to server on {serverIp}:{port}.");
            }
            catch (Exception ex)
            {
                //StatusTextBlock.Text = $"Failed to connect to server: {ex.Message}";
                Console.WriteLine($"Failed to connect to server: {ex.Message}");
            }
        }


        public async Task<List<UserDTO>> RequestGetAllUsers()
        {
            var request = new
            {
                service = "UserService",
                method = "GetAllUsers",
                parameters = new { }
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

            if (success)
            {
                var users = JsonConvert.DeserializeObject<List<UserDTO>>(message); // Assuming users are in the message
                foreach (var user in users)
                {
                    Console.WriteLine($"User: {user.Id}, {user.Name}, {user.Email}");
                }
                return users;
            }
            else
            {
                Console.WriteLine("Operation failed: " + message);
                return new List<UserDTO>(); // Return an empty list or handle as needed
            }
        }

        public async Task<dynamic> RequestUserService(string methodName, object parameters)
        {
            var request = new
            {
                service = "UserService",
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

            if (success)
            {
                // Attempt to deserialize the response data based on the expected return type
                try
                {
                    var data = JsonConvert.DeserializeObject<dynamic>(message);
                    return data;
                }
                catch (JsonSerializationException)
                {
                    Console.WriteLine("Received data could not be deserialized to a dynamic object.");
                    return null;
                }
            }
            else
            {
                Console.WriteLine("Operation failed: " + message);
                return null;
            }
        }

    }
}
