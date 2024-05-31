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
    public partial class AdminGui : Window
    {
        private AdminPresenter _adminPresenter;
        public AdminGui()
        {
            InitializeComponent();
            _adminPresenter = new AdminPresenter(this);
            UserTypeComboBox.ItemsSource = Enum.GetValues(typeof(UserType));
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

        public Button GetBackButton()
        { return this.BackButton; }

        
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

        private void CreateUserButton_Click(object sender, RoutedEventArgs e)
        {
            this._adminPresenter.CreateUser();
        }

        private void UserTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _adminPresenter.SelectUser();
        }
    }
}
