using Client.DTO;
using Client.Services;
using Client.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;

namespace Client.Presenter
{
    internal class AdminPresenter
    {
        private AdminGui _adminGui;
        private ClientHost clientHost;

        public AdminPresenter(AdminGui adminGui)
        {
            _adminGui = adminGui;
            StartSetup();
        }

        private async void StartSetup()
        {
            await setup();
        }

        private async Task setup()
        {
            clientHost = new ClientHost();
            await clientHost.ConnectToServer();
            await LoadUserTable();
        }

        private async Task LoadUserTable()
        {
            var response = await clientHost.RequestUserService("GetAllUsers", new { });            
            Response responseObject = JsonConvert.DeserializeObject<Response>(response);

            if (responseObject.Result)
            {
                // Deserialize the Message property into a list of UserDTO
                var users = JsonConvert.DeserializeObject<List<UserDTO>>(responseObject.Message);

                // Update the GUI with the user list (you need to implement UpdateUserTable in AdminGui)
                _adminGui.GetUserTable().ItemsSource = users;
            }
            Console.WriteLine(responseObject.Message);
        }

        private UserDTO ValidData()
        {
            string Id= _adminGui.GetIdTextBox().Text;
            string Name= _adminGui.GetNameTextBox().Text;
            string Email = _adminGui.GetEmailTextBox().Text;
            string Password = _adminGui.GetPasswordTextBox().Text;
            string Phone = _adminGui.GetPhoneTextBox().Text;
            UserType userType = (UserType)_adminGui.GetUserTypeComboBox().SelectedItem;
            bool anyEmpty = new[] { Id, Name, Email, Password, Phone }.Any(string.IsNullOrEmpty);
            if (anyEmpty)
            {                
                return null;
            }
            return new UserDTO(int.Parse(Id), Name, Email, Password,userType, Phone);            
        }


        public async void CreateUser()
        {
            try
            {
                //UserDTO user = ValidData();
                //if (user == null)
                //    _adminGui.ShowMessage("All fields are mandatory");
                UserDTO userDummy = new UserDTO(0, "Name", "Email", "Password", UserType.PARTICIPANT, "Phone");
                var createUserResult = await clientHost.RequestUserService("CreateUser", userDummy);
                _adminGui.ClearFormFields();
                LoadUserTable();
            }
            catch (Exception ex)
            {
                _adminGui.ShowMessage("Operation Failed");
            }
        }

        public void SelectUser()
        {
            UserDTO user = (UserDTO) _adminGui.GetUserTable().SelectedItem;
            if (user == null)
            {
                return;
            }
            _adminGui.GetIdTextBox().Text = user.Id.ToString();
            _adminGui.GetNameTextBox().Text = user.Name.ToString();
            _adminGui.GetEmailTextBox().Text = user.Email.ToString();
            _adminGui.GetPasswordTextBox().Text = user.Password.ToString();
            _adminGui.GetPhoneTextBox().Text = user.Phone.ToString();
            _adminGui.GetUserTypeComboBox().SelectedIndex = (int)user.UserType;
        }
    }
}
