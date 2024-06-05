using Client.DTO;
using Client.Presenter;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Client.View
{
    /// <summary>
    /// Interaction logic for OrganizatorGUI.xaml
    /// </summary>
    public partial class OrganizatorGUI : Window
    {
        private OrganizatorPresenter _organizatorPresenter;
        public OrganizatorGUI()
        {
            InitializeComponent();
            FillComboBoxes();
            _organizatorPresenter = new OrganizatorPresenter(this);            

        }

        public DataGrid GetTabelPrezentari()
        { return this.PresentationTable; }
        public DataGrid GetParticipantsTable()
        { return this.ParticipantsTable; }
        //Presentation Form
        public TextBox GetIdPrezentareTextBox()
        { return this.IdPrezentareTextBox; }
        public TextBox GetTitleTextBox()
        { return this.TitleTextBox; }
        public TextBox GetDescriptionTextBox()
        { return this.DescriptionTextBox; }
        public DatePicker GetDataDatePicker()
        { return this.DataDatePicker; }
        public TextBox GetTimeTextBox()
        { return this.TimeTextBox; }
        public ComboBox GetPresentationFormSectionComboBox()
        { return this.PresentationFormSectionComboBox; }
        public TextBox GetAuthorTextBox()
        { return this.AuthorTextBox; }

        //Presentaion Form Buttons
        public Button GetCreatePresentationButton()
        { return this.CreatePresentationButton; }
        public Button GetUpdatePresentationButton()
        { return this.UpdatePresentationButton; }
        public Button GetDeletePresentationButton()
        { return this.DeletePresentationButton; }

        //Presentation Form Button actions
        private async void CreatePresentationButton_Click(object sender, RoutedEventArgs e)
        {
            await this._organizatorPresenter.CreatePresentation();
        }
        private async void UpdatePresentationButton_Click(object sender, RoutedEventArgs e)
        {
            await this._organizatorPresenter.UpdatePresentation();
        }
        private async void DeletePresentationButton_Click(object sender, RoutedEventArgs e)
        {
            await this._organizatorPresenter.DeletePresentation();
        }

        //Filter Presentations 
        public ComboBox GetFilterPresentationsComboBox()
        { return this.FilterPresentationsComboBox; }
        public ComboBox GetSelectFormatComboBox()
        { return this.SelectFormatComboBox; }

        //Filter Presentations Buttons
        public Button GetFilterPresentationsButton()
        { return this.FilterPresentationsButton; }
        public Button GetDownloadListButton()
        { return this.DownloadListButton; }

        //Filter Presentations Button actions
        private async void FilterPresentationsButton_Click(object sender, RoutedEventArgs e)
        {
            await this._organizatorPresenter.FilterPresentations();
        }
        private async void DownloadListButton_Click(object sender, RoutedEventArgs e)
        {
            await this._organizatorPresenter.DownloadPresentationList();
        }



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
        private void PresentationTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "Author")
            {
                var templateColumn = new DataGridTemplateColumn
                {
                    Header = "Author"
                };

                var itemsControlFactory = new FrameworkElementFactory(typeof(ItemsControl));
                itemsControlFactory.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("Author"));

                // Define the ItemTemplate for the ItemsControl to display the Email property
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
                var templateColumn = new DataGridTemplateColumn
                {
                    Header = "Participants"
                };

                var itemsControlFactory = new FrameworkElementFactory(typeof(ItemsControl));
                itemsControlFactory.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("Participants"));

                // Define the ItemTemplate for the ItemsControl to display the Email property
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
        public void ClearFormFields()
        {
            this.IdPrezentareTextBox.Clear();
            this.TitleTextBox.Clear();
            this.DescriptionTextBox.Clear();
            this.DataDatePicker.SetValue(UidProperty, null);
            this.TimeTextBox.Clear();
            this.PresentationFormSectionComboBox.SelectedIndex = 0;
            this.AuthorTextBox.Clear();
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }


        private void TabelParticipanti_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "PdfFilePath")
            {
                var templateColumn = new DataGridTemplateColumn
                {
                    Header = "CV",
                    CellTemplate = (DataTemplate)FindResource("PdfFilePathTemplate")
                };
                e.Column = templateColumn;
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
        }

        private void ShowStatisticsButton_Click(object sender, RoutedEventArgs e)
        {
            ChartView chartView = new ChartView();
            chartView.Show();
        }
    }
}
