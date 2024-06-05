using Client.DTO;
using Client.Presenter;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;


namespace Client.View
{
    /// <summary>
    /// Interaction logic for HomeGUI.xaml
    /// </summary>
    public partial class HomeGUI : Window
    {
        private HomePresenter homeController;
        public HomeGUI()
        {
            InitializeComponent();
            homeController = new HomePresenter(this);
            FillComboBox();
        }

        private void FillComboBox()
        {
            this.FilterPresentationComboBox.ItemsSource = Enum.GetValues(typeof(Section));
            this.FilterPresentationComboBox.SelectedIndex = 0;
        }



        //Filter Section
        public ComboBox GetFilterPresentationComboBox()
        { return this.FilterPresentationComboBox; }
        public Button GetFilterPresentationButton()
        { return this.FilterPresentationButton; }

        //Table Section
        public DataGrid GetConferenceTable()
        { return this.TabelConferinte; }

        //Sign up section
        public TextBox GetNameTextBox()
        { return this.NameTextBox; }
        public TextBox GetEmailTextBox()
        { return this.EmailTextBox; }
        public TextBox GetPhoneTextBox()
        { return this.PhoneTextBox; }
        public TextBox GetPinTextBox()
        { return this.PinTextBox; }
        public TextBox GetPhotoPathTexBox()
        { return this.PhotoPathTextBox; }
        public TextBox GetDocumentPathTexBox()
        { return this.DocumentPathTextBox; }
        public ComboBox GetTypeComboBox()
        { return this.AuthorComboBox; }

        //Participant fields
        public Label GetPresentationLabel()
        { return this.PresentationLabel; }
        public ComboBox GetAttendPresentationComboBox()
        { return this.AttendPresentationComboBox; }


        //Autor fields                
        public TextBox GetTitleTextBox()
        {return this.TitleTextBox;}
        public TextBox GetDescriptionTextBox()
        {return this.DescriptionTextBox;}
        public TextBox GetTimeTextBox()
        {return this.TimeTextBox;}      
        public DatePicker GetDataDatePicker()
        {return this.PresentationDatePicker;}
        public ComboBox GetSectionComboBox()
        {return this.SectionComboBox;}

        public void ShowAuthorFields(bool isSelected)
        {
            this.TitleTextBox.Visibility = isSelected ? Visibility.Visible : Visibility.Collapsed;
            this.GetDescriptionTextBox().Visibility = isSelected ? Visibility.Visible : Visibility.Collapsed;
            this.TimeTextBox.Visibility = isSelected ? Visibility.Visible : Visibility.Collapsed;
            this.PresentationDatePicker.Visibility = isSelected ? Visibility.Visible : Visibility.Collapsed;
            this.SectionComboBox.Visibility = isSelected ? Visibility.Visible : Visibility.Collapsed;           

            this.TitleLabel.Visibility = isSelected ? Visibility.Visible : Visibility.Collapsed;
            this.DescriptionLabel.Visibility = isSelected ? Visibility.Visible : Visibility.Collapsed;
            this.TimeLabel.Visibility = isSelected ? Visibility.Visible : Visibility.Collapsed;
            this.DateLabel.Visibility = isSelected ? Visibility.Visible : Visibility.Collapsed;
            this.SectionLabel.Visibility = isSelected ? Visibility.Visible : Visibility.Collapsed;            
        }
        public void ShowParticipantFields(bool isSelected)
        {
            this.PresentationLabel.Visibility = isSelected ? Visibility.Visible :Visibility.Collapsed;
            this.AttendPresentationComboBox.Visibility = isSelected ? Visibility.Visible : Visibility.Collapsed;
        }


        public async void DownloadLink_Click(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            string pdfFilePath = e.Uri.ToString();
            //also get email of the participant row
            homeController.DownloadLink_Click(pdfFilePath);
            e.Handled = true;

        }

        private async void FilterPresentationButton_Click(object sender, RoutedEventArgs e)
        {
            await homeController.FilterPresentations();
        }

        private string BrowseFile(string filter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog { Filter = filter };
            return openFileDialog.ShowDialog() == true ? openFileDialog.FileName : string.Empty;
        }

        private void BrowsePhoto_Click(object sender, RoutedEventArgs e)
        {
            string photoPath = BrowseFile("Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*");
            if (!string.IsNullOrEmpty(photoPath))
            {
                this.PhotoPathTextBox.Text = photoPath;
            }
        }

        private void BrowseDocument_Click(object sender, RoutedEventArgs e)
        {
            string documentPath = BrowseFile("Document files (*.pdf;*.docx)|*.pdf;*.docx|All files (*.*)|*.*");
            if (!string.IsNullOrEmpty(documentPath))
            {
                this.DocumentPathTextBox.Text = documentPath;
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginGUI loginGUI = new LoginGUI();
            loginGUI.Show();
        }

        private async void SingUpButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.AuthorComboBox.SelectedIndex == 0)
                {
                    await homeController.CreateAuthor();
                }
                else
                {
                    await homeController.CreateParticipant();
                }
                await homeController.LoadConferenceTable();
            }
            catch (Exception ex)
            {
                ShowMessage($"Error uploading files: {ex.Message}");
            }
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        private void AuthorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = (sender as ComboBox).SelectedIndex;
            if(selectedIndex==0)
            {
                ShowAuthorFields(true);
                ShowParticipantFields(false);
            }
            else
            {
                ShowAuthorFields(false);
                ShowParticipantFields(true);
            }
        }
    }
}
