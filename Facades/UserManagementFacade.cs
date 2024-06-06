using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Client.DTO;
using Client.Services;

namespace Client.Facades
{
    internal class UserManagementFacade
    {
        private readonly UserService _userService;

        public UserManagementFacade()
        {
            _userService = new UserService();
        }

        public async Task<bool> CreateUser(UserDTO userDTO)
        {
            try
            {
                return await _userService.CreateUser(userDTO);
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
                bool result = await _userService.UpdateUser(userDTO);
                await NotifyUpdates(userDTO);
                return result;
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
                return await _userService.DeleteUser(userDTO);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<UserDTO> GetUserByEmailAndPassword(string email, string password)
        {
            try
            {
                return await _userService.GetUsersByEmailAndPassword(email, password);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<UserDTO> GetUserById(int id)
        {
            try
            {
                return await _userService.GetUserbyId(id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<UserDTO> NotifyUpdates(UserDTO userDTO)
        {
            try
            {
                await _userService.NotifyEmailUpdate(userDTO);
                await _userService.NotifyWhatsAppUpdate(userDTO);
                return userDTO;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<UserDTO> GetUserByEmail(string email)
        {
            try
            {
                var users = await _userService.GetAllUsers();
                return users.FirstOrDefault(u => u.Email == email);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<List<UserDTO>> GetAllUserDTOs()
        {
            try
            {
                List<UserDTO> users = await _userService.GetAllUsers();
                return users;
            }
            catch (Exception ex)
            {
                return new List<UserDTO>();


            }
        }

        public async Task<List<UserDTO>> GetUsersByUserType(UserType userType)
        {
            try
            {
                List<UserDTO> users = await _userService.GetUsersByUserType(userType);
                return users;
            }
            catch (Exception ex)
            {
                return new List<UserDTO>();


            }
        }
    }
}
