using Client.DTO;
using Client.Presenter;
using Client.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Client.View
{
    /// <summary>
    /// Interaction logic for AdminGui.xaml
    /// </summary>
    public partial class AdminGui : Window, IObserver
    {
        private AdminPresenter _adminPresenter;
        public AdminGui()
        {
            InitializeComponent();
            _adminPresenter = new AdminPresenter(this);
            UserTypeComboBox.ItemsSource = Enum.GetValues(typeof(UserType));
            var items = Enum.GetValues(typeof(UserType)).Cast<UserType>().Select(x => x.ToString()).ToList();
            items.Insert(3,"All");
            ComboBoxUserType.ItemsSource = items;
        }


        //Form Data        
        public TextBox GetIdTextBox()
        { return this.IdTextBox; }

        public TextBox GetNameTextBox()
        { return this.NameTextBox; }

        public TextBox GetEmailTextBox()
        { return this.EmailTextBox; }

        public TextBox GetPasswordTextBox()
        { return this.PasswordTextBox; }

        public TextBox GetPhoneTextBox()
        { return this.PhoneTextBox; }

        public ComboBox GetUserTypeComboBox()
        { return this.UserTypeComboBox; }

        //Form Buttons        
        public Button GetCreateUserButton()
        { return this.CreateUserButton; }

        public Button GetDeleteUserButton()
        { return this.DeleteUserButton; }

        public Button GetUpdateUserButton()
        { return this.UpdateUserButton; }

        
        //Filter Section
        public ComboBox GetFilterUsersComboBox()
        { return this.ComboBoxUserType; }

        public Button GetFilterUsersButton()
        { return this.FilterUsersButton; }


        //Data Table
        public DataGrid GetUserTable()
        { return this.UserTable; }


        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyDescriptor is PropertyDescriptor descriptor)
            {
                e.Column.Header = descriptor.DisplayName ?? descriptor.Name;
            }
        }

        public void ClearFormFields()
        {
            this.IdTextBox.Clear();
            this.NameTextBox.Clear();
            this.EmailTextBox.Clear();
            this.PasswordTextBox.Clear();
            this.PhoneTextBox.Clear();
        }

        

        public void ShowMessage(string message)
        {
            MessageBox.Show(message, "Info");
        }
        private void UserTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _adminPresenter.SelectUser();
        }


        //Button actions 
        private async void CreateUserButton_Click(object sender, RoutedEventArgs e)
        {
            await this._adminPresenter.CreateUser();
            Subject.GetInstance().Notify();
        }
        private async void UpdateUserButton_Click(object sender, RoutedEventArgs e)
        {
            await this._adminPresenter.UpdateUser();
            Subject.GetInstance().Notify();
        }
        private async void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            await this._adminPresenter.DeleteUser();
            Subject.GetInstance().Notify();
        }

        private async void FilterUsersButton_Click(object sender, RoutedEventArgs e)
        {
            await _adminPresenter.FilterUsers();
        }

        public async void Update()
        {
            this.ClearFormFields();
            await _adminPresenter.LoadUserTable();
        }
    }
}
