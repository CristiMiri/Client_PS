
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.DTO
{
    public enum UserType
    {
        PARTICIPANT,
        ORGANIZER,
        ADMINISTRATOR
    }
    internal class UserDTO
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserType UserType { get; set; }
        public string Phone { get; set; }

        // Default constructor
        public UserDTO() { }
        public UserDTO(int id, string name, string email, string password, UserType userType, string phone)
        {
            Id = id;
            Name = name;
            Email = email;
            Password = password;
            UserType = userType;
            Phone = phone;
        }

        public override string ToString()
        {
            return $"UserDTO with id {Id} and name {Name} has email {Email} and is of type {UserType} and has phone number {Phone}.";
        }

        
    }
}
