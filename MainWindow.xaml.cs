using Client.View;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Hide();
            //AdminGui adminGui = new AdminGui();
            //adminGui.Show();
            //OrganizatorGUI organizatorGUI = new OrganizatorGUI();
            //organizatorGUI.Show();
            //HomeGUI homeGUI = new HomeGUI();
            //homeGUI.Show();
            LoginGUI loginGUI = new LoginGUI();
            loginGUI.Show();
        }
    }
}