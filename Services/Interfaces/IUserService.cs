using Client.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services.Interfaces
{
    internal interface IUserService
    {
        Task<bool> CreateUser(UserDTO userDTO);
        Task<bool> UpdateUser(UserDTO userDTO);
        Task<bool> DeleteUser(UserDTO userDTO);
        
        Task<UserDTO> GetUserbyId(int id);
        Task<List<UserDTO>> GetAllUsers();
        Task<UserDTO> GetUsersByEmailAndPassword(string email, string password);
        Task<List<UserDTO>> GetUsersByUserType(UserType userType);

    }
}
