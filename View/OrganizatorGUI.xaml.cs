using Client.DTO;
using Client.Presenter;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Client.View
{
    /// <summary>
    /// Interaction logic for OrganizatorGUI.xaml
    /// </summary>
    public partial class OrganizatorGUI : Window, IObserver
    {
        private OrganizatorPresenter _organizatorPresenter;

        public OrganizatorGUI()
        {
            InitializeComponent();
            FillComboBoxes();
            _organizatorPresenter = new OrganizatorPresenter(this);
        }

        // DataGrid Accessors
        public DataGrid GetTabelPrezentari() => this.PresentationTable;
        public DataGrid GetParticipantsTable() => this.ParticipantsTable;

        // Presentation Form Accessors
        public TextBox GetIdPrezentareTextBox() => this.IdPrezentareTextBox;
        public TextBox GetTitleTextBox() => this.TitleTextBox;
        public TextBox GetDescriptionTextBox() => this.DescriptionTextBox;
        public DatePicker GetDataDatePicker() => this.DataDatePicker;
        public TextBox GetTimeTextBox() => this.TimeTextBox;
        public ComboBox GetPresentationFormSectionComboBox() => this.PresentationFormSectionComboBox;
        public TextBox GetAuthorTextBox() => this.AuthorTextBox;

        // Presentation Form Buttons Accessors
        public Button GetCreatePresentationButton() => this.CreatePresentationButton;
        public Button GetUpdatePresentationButton() => this.UpdatePresentationButton;
        public Button GetDeletePresentationButton() => this.DeletePresentationButton;

        // Presentation Form Button Actions
        private async void CreatePresentationButton_Click(object sender, RoutedEventArgs e) => await this._organizatorPresenter.CreatePresentation();
        private async void UpdatePresentationButton_Click(object sender, RoutedEventArgs e) => await this._organizatorPresenter.UpdatePresentation();
        private async void DeletePresentationButton_Click(object sender, RoutedEventArgs e) => await this._organizatorPresenter.DeletePresentation();

        // Filter Presentations Accessors
        public ComboBox GetFilterPresentationsComboBox() => this.FilterPresentationsComboBox;
        public ComboBox GetSelectFormatComboBox() => this.SelectFormatComboBox;

        // Filter Presentations Buttons Accessors
        public Button GetFilterPresentationsButton() => this.FilterPresentationsButton;
        public Button GetDownloadListButton() => this.DownloadListButton;

        // Filter Presentations Button Actions
        private async void FilterPresentationsButton_Click(object sender, RoutedEventArgs e) => await this._organizatorPresenter.FilterPresentations();
        private async void DownloadListButton_Click(object sender, RoutedEventArgs e) => await this._organizatorPresenter.DownloadPresentationList();

        // Participant Form Accessors
        public TextBox GetIdParticipantTextBox() => this.IdParticipantTextBox;
        public TextBox GetNameTextBox() => this.NameTextBox;
        public TextBox GetEmailTextBox() => this.EmailTextBox;
        public TextBox GetPhoneTextBox() => this.PhoneTextBox;
        public TextBox GetPinTextBox() => this.PinTextBox;
        public TextBox GetPhotoPathTextBox() => this.PhotoPathTextBox;
        public TextBox GetDocumentPathTextBox() => this.DocumentPathTextBox;

        // Participant Form Buttons Accessors
        public Button GetCreateParticipantButton() => this.CreateParticipantButton;
        public Button GetDeleteParticipantButton() => this.DeleteParticipantButton;
        public Button GetUpdateParticipantButton() => this.UpdateParticipantButton;
        public Button GetAcceptParticipantButton() => this.AcceptParticipantButton;
        public Button GetRejectParticipantButton() => this.RejectParticipantButton;

        // Filter Participants Accessors
        public ComboBox GetFilterParticipantsComboBox() => this.FilterParticipantsComboBox;
        public Button GetFilterParticipantButton() => this.FilterParticipantButton;

        // Filter Participants Button Actions
        private void FilterParticipantButton_Click(object sender, RoutedEventArgs e) => _organizatorPresenter.FilterParticipants();

        // Utility Methods
        private void FillComboBoxes()
        {
            this.FilterPresentationsComboBox.ItemsSource = Enum.GetValues(typeof(Section));
            this.FilterPresentationsComboBox.SelectedIndex = 0;
            List<string> formats = new List<string> { "Csv", "Doc", "Json", "Xml" };
            this.SelectFormatComboBox.ItemsSource = formats;
            this.SelectFormatComboBox.SelectedIndex = 0;
            this.PresentationFormSectionComboBox.ItemsSource = Enum.GetValues(typeof(Section));
            this.PresentationFormSectionComboBox.SelectedIndex = 0;
            this.FilterParticipantsComboBox.ItemsSource = Enum.GetValues(typeof(Section));
            this.FilterParticipantsComboBox.SelectedIndex = 0;
        }

        // DataGrid Event Handlers
        private void PresentationTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PresentationDTO presentationDTO = (PresentationDTO)PresentationTable.SelectedItem;
            if (presentationDTO != null)
            {
                this.IdPrezentareTextBox.Text = presentationDTO.Id.ToString();
                this.TitleTextBox.Text = presentationDTO.Title;
                this.DescriptionTextBox.Text = presentationDTO.Description;
                this.DataDatePicker.SelectedDate = presentationDTO.Date;
                this.TimeTextBox.Text = presentationDTO.Hour.ToString();
                this.PresentationFormSectionComboBox.SelectedItem = presentationDTO.Section;
                this.AuthorTextBox.Text = presentationDTO.IdAuthor.ToString();
            }
        }

        private void PresentationTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "Author")
            {
                var templateColumn = new DataGridTemplateColumn { Header = "Author" };
                var itemsControlFactory = new FrameworkElementFactory(typeof(ItemsControl));
                itemsControlFactory.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("Author"));

                var itemTemplate = new DataTemplate();
                var stackPanelFactory = new FrameworkElementFactory(typeof(StackPanel));
                stackPanelFactory.SetValue(StackPanel.OrientationProperty, Orientation.Vertical);

                var textBlockFactory = new FrameworkElementFactory(typeof(TextBlock));
                textBlockFactory.SetBinding(TextBlock.TextProperty, new Binding("Email"));
                stackPanelFactory.AppendChild(textBlockFactory);

                itemTemplate.VisualTree = stackPanelFactory;
                itemsControlFactory.SetValue(ItemsControl.ItemTemplateProperty, itemTemplate);

                var dataTemplate = new DataTemplate { VisualTree = itemsControlFactory };
                templateColumn.CellTemplate = dataTemplate;
                e.Column = templateColumn;
            }
            else if (e.PropertyName == "Participants")
            {
                var templateColumn = new DataGridTemplateColumn { Header = "Participants" };
                var itemsControlFactory = new FrameworkElementFactory(typeof(ItemsControl));
                itemsControlFactory.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("Participants"));

                var itemTemplate = new DataTemplate();
                var stackPanelFactory = new FrameworkElementFactory(typeof(StackPanel));
                stackPanelFactory.SetValue(StackPanel.OrientationProperty, Orientation.Vertical);

                var textBlockFactory = new FrameworkElementFactory(typeof(TextBlock));
                textBlockFactory.SetBinding(TextBlock.TextProperty, new Binding("Email"));
                stackPanelFactory.AppendChild(textBlockFactory);

                itemTemplate.VisualTree = stackPanelFactory;
                itemsControlFactory.SetValue(ItemsControl.ItemTemplateProperty, itemTemplate);

                var dataTemplate = new DataTemplate { VisualTree = itemsControlFactory };
                templateColumn.CellTemplate = dataTemplate;
                e.Column = templateColumn;
            }
        }

        private void TabelParticipanti_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "PdfFilePath")
            {
                e.Cancel = true;
            }
            else if (e.PropertyName == "PhotoFilePath")
            {
                var templateColumn = new DataGridTemplateColumn
                {
                    Header = "Photo",
                    CellTemplate = (DataTemplate)FindResource("PhotoFilePathTemplate")
                };
                e.Column = templateColumn;
            }
            else if (e.PropertyName == "PhotoImage")
            {
                e.Cancel = true;
            }
        }

        private void SelectParticipant(object sender, SelectionChangedEventArgs e)
        {
            ParticipantDTO participant = (ParticipantDTO)this.ParticipantsTable.SelectedItem;
            if (participant != null)
            {
                GetIdParticipantTextBox().Text = participant.Id.ToString();
                GetNameTextBox().Text = participant.Name;
                GetEmailTextBox().Text = participant.Email;
                GetPhoneTextBox().Text = participant.Phone;
                GetPinTextBox().Text = participant.CNP;
                GetPhotoPathTextBox().Text = participant.PhotoFilePath;
                GetDocumentPathTextBox().Text = participant.PdfFilePath;
            }
        }

        public void ClearFormFields()
        {
            this.IdPrezentareTextBox.Clear();
            this.TitleTextBox.Clear();
            this.DescriptionTextBox.Clear();
            this.DataDatePicker.SelectedDate = null;
            this.TimeTextBox.Clear();
            this.PresentationFormSectionComboBox.SelectedIndex = 0;
            this.AuthorTextBox.Clear();
        }

        public void ShowMessage(string message) => MessageBox.Show(message);

        private void ShowStatisticsButton_Click(object sender, RoutedEventArgs e)
        {
            ChartView chartView = new ChartView();
            chartView.Show();
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

        private async void CreateParticipantButton_Click(object sender, RoutedEventArgs e)
        {
            await _organizatorPresenter.CreateParticipant();
        }

        private async void UpdateParticipantButton_Click(object sender, RoutedEventArgs e)
        {
            await _organizatorPresenter.UpdateParticipant();
        }

        private async void DeleteParticipantButton_Click(object sender, RoutedEventArgs e)
        {
            await _organizatorPresenter.DeleteParticipant();
        }

        private async void AcceptParticipantButton_Click(object sender, RoutedEventArgs e)
        {
            await _organizatorPresenter.AcceptParticipant();
        }

        private async void RejectParticipantButton_Click(object sender, RoutedEventArgs e)
        {
            await _organizatorPresenter.RejectParticipant();
        }

        public async void Update()
        {
            ClearFormFields();
            await _organizatorPresenter.loadParticipantTable();
            await _organizatorPresenter.loadPresentationTable();
        }
    }
}
