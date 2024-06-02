using Client.DTO;
using Client.Presenter;
using Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
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
    /// Interaction logic for LoginGUI.xaml
    /// </summary>
    public partial class LoginGUI : Window
    {
        private LoginPresenter _loginPresenter;
        public LoginGUI()
        {
            InitializeComponent();
            _loginPresenter = new LoginPresenter(this);
        }

        //Data access methods
        public TextBox GetEmailTextBox()
        { return EmailTextBox; }
        public TextBox GetPasswordTextBox()
        { return PasswordTextBox; }
        public Button GetLoginButton()
        { return LoginButton; }

        
        public void ShowError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private async void Login(object sender, RoutedEventArgs e)
        {
            await _loginPresenter.Login();
        }
    }
}
