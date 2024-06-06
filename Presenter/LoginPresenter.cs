using Client.DTO;
using Client.Services;
using Client.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Client.Presenter
{
    internal class LoginPresenter
    {
        private LoginGUI _loginGui;        
        private UserService _userService;

        public LoginPresenter(LoginGUI loginGui)
        {
            _loginGui = loginGui;
            _userService = new UserService();         
        }

        //Validation methods
        private UserDTO validData()
        {
            string email = _loginGui.GetEmailTextBox().Text;
            string password = _loginGui.GetPasswordTextBox().Text;
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                _loginGui.ShowError("Email and password are required!");
                return null;
            }

            return new UserDTO { Email = email, Password = password };
        }

        //Navigation methods
        private void NavigateToUserPage(UserDTO user)
        {
            switch (user.UserType)
            {
                case UserType.ADMINISTRATOR:
                    AdminGui adminGUI = new AdminGui();
                    Subject.GetInstance().Attach(adminGUI);
                    adminGUI.Show();
                    break;
                case UserType.PARTICIPANT:
                    UtilizatorGUI utilizatorGUI = new UtilizatorGUI();
                    Subject.GetInstance().Attach(utilizatorGUI);
                    utilizatorGUI.Show();
                    break;
                case UserType.ORGANIZER:
                    OrganizatorGUI organizatorGUI = new OrganizatorGUI();
                    Subject.GetInstance().Attach(organizatorGUI);
                    organizatorGUI.Show();
                    break;
                default:
                    return;
                    break;
            }
        }
        public async Task Login()
        {
            try
            {
                UserDTO user = validData();
                if (user != null)
                {
                    UserDTO authenticatedUser = await _userService.GetUsersByEmailAndPassword(user.Email, user.Password);
                    if (authenticatedUser != null)
                    {
                        NavigateToUserPage(authenticatedUser);
                    }
                    else
                    {
                        _loginGui.ShowError("Invalid email or password!");
                    }
                }
            }
            catch (Exception ex)
            {
                _loginGui.ShowError("An error occurred during login. Please try again.");
            }
        }

    }
}
