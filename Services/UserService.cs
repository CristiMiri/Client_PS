using Client.DTO;
using Client.Services.Interfaces;
using Client.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services
{
    internal class UserService : IUserService
    {
        private ClientHost clientHost;

        public UserService()
        {
            clientHost = ClientHost.Instance;
        }

        public async Task<bool> CreateUser(UserDTO userDTO)
        {
            try
            {
                var createUserResult = await ClientHost.Instance.RequestUserService("CreateUser", userDTO);                
                return Convert.ToBoolean(createUserResult.Result);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> UpdateUser(UserDTO userDTO)
        {
            try
            {
                var updateUserResult = await clientHost.RequestUserService("UpdateUser", userDTO);
                return Convert.ToBoolean(updateUserResult.Result);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> DeleteUser(UserDTO userDTO)
        {
            try
            {
                var deleteUserResult = await clientHost.RequestUserService("DeleteUser", userDTO);
                return Convert.ToBoolean(deleteUserResult.Result);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<UserDTO> GetUsersByEmailAndPassword(string email, string password)
        {
            try
            {
                var user = await clientHost.RequestUserService("GetUsersByEmailAndPassword", new { email, password });
                return Newtonsoft.Json.JsonConvert.DeserializeObject<UserDTO>(user.Data.ToString());
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<UserDTO>> GetAllUsers()
        {
            try
            {
                var getAllUsersResult = await clientHost.RequestUserService("GetAllUsers", new { });
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserDTO>>(getAllUsersResult.Data.ToString());
            }
            catch (Exception ex)
            {
                return new List<UserDTO>();
            }
        }
        public Task<UserDTO> GetUserbyId(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<List<UserDTO>> GetUsersByUserType(UserType userType)
        {
            try
            {
                var filterUsersResult = await clientHost.RequestUserService("GetUsersByUserType", new { userType });
                return await Task.FromResult(Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserDTO>>(filterUsersResult.Data.ToString()));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new List<UserDTO>());
            }
        }


    }
}
