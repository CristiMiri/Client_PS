using Client.Presenter;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for UtilizatorGUI.xaml
    /// </summary>
    public partial class UtilizatorGUI : Window, IObserver
    {
        private UtilizatorPresenter _utilizatorPresenter;
        public UtilizatorGUI()
        {
            InitializeComponent();
            _utilizatorPresenter = new UtilizatorPresenter(this);
            
        }

        public DataGrid GetConferenceTable()
        {
            return this.TabelConferinte;
        }

        public async void Update()
        {
            await _utilizatorPresenter.LoadConferenceTable();
        }

        private void DownloadLink_Click(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            _utilizatorPresenter.DownloadLink_Click(e.Uri.ToString());
            MessageBox.Show("File downloaded");
        }
    }
}
