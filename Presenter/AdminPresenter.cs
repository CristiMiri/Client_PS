using Client.DTO;
using Client.Services;
using Client.View;
using Newtonsoft.Json;

namespace Client.Presenter
{
    internal class AdminPresenter
    {
        private AdminGui _adminGui;
        private ClientHost clientHost;
        private UserService _userService;

        public AdminPresenter(AdminGui adminGui)
        {
            _adminGui = adminGui;
            _userService = new UserService();
            StartSetup();
        }

        private async void StartSetup()
        {
            await setup();
        }

        private async Task setup()
        {
            clientHost = ClientHost.Instance;            
            await LoadUserTable();
        }

        private async Task LoadUserTable()
        {
            var users = await _userService.GetAllUsers();
            _adminGui.GetUserTable().ItemsSource = users;
        }

        private UserDTO ValidData()
        {
            string Id = _adminGui.GetIdTextBox().Text;
            if (String.IsNullOrEmpty(Id))
            {
                Id = "0";
            }
            string Name = _adminGui.GetNameTextBox().Text;
            string Email = _adminGui.GetEmailTextBox().Text;
            string Password = _adminGui.GetPasswordTextBox().Text;
            string Phone = _adminGui.GetPhoneTextBox().Text;
            UserType userType = (UserType)_adminGui.GetUserTypeComboBox().SelectedItem;
            bool anyEmpty = new[] {  Name, Email, Password, Phone }.Any(string.IsNullOrEmpty);
            if (anyEmpty)
            {
                return null;
            }
            return new UserDTO(int.Parse(Id), Name, Email, Password, userType, Phone);
        }


        public async Task CreateUser()
        {
            UserDTO user = ValidData();
            if (user == null)
                _adminGui.ShowMessage("All fields are mandatory");
            bool result = await _userService.CreateUser(user);
            if (result)
            {
                _adminGui.ClearFormFields();
                LoadUserTable();
                _adminGui.ShowMessage("Operation Successful");
            }
            else
            {
                _adminGui.ShowMessage("Operation Failed");
            }
        }

        public async Task UpdateUser()
        {
            UserDTO user = ValidData();
            if (user == null)
                _adminGui.ShowMessage("All fields are mandatory");
            bool result = await _userService.UpdateUser(user);
            if (result)
            {
                _adminGui.ClearFormFields();
                LoadUserTable();
                _adminGui.ShowMessage("Operation Successful");
            }
            else
            {
                _adminGui.ShowMessage("Operation Failed");
            }
        }

        public async Task DeleteUser()
        {
            UserDTO user = ValidData();
            if (user == null)
                _adminGui.ShowMessage("All fields are mandatory");
            bool deleteUserResult = await _userService.DeleteUser(user);
            if (deleteUserResult)
            {
                _adminGui.ClearFormFields();
                LoadUserTable();
                _adminGui.ShowMessage("Operation Successful");
            }
            else
            {
                _adminGui.ShowMessage("Operation Failed");
            }
        }



        public async Task FilterUsers()
        {

            if (_adminGui.GetFilterUsersComboBox().SelectedIndex == 3)
            {
                await LoadUserTable();    
                return;
            }
            else
            {
                UserType userType = (UserType)_adminGui.GetFilterUsersComboBox().SelectedIndex;                
                var users = await _userService.GetUsersByUserType(userType);
                _adminGui.GetUserTable().ItemsSource = users;
                return;
            }
        }


        public void SelectUser()
        {
            UserDTO user = (UserDTO)_adminGui.GetUserTable().SelectedItem;
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
