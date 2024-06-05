using Client.DTO;
using Client.Services;
using Client.View;
using System.Windows;

namespace Client.Presenter
{
    internal class OrganizatorPresenter
    {
        private OrganizatorGUI _organizatorGUI;
        private PresentationService _presentationService;
        private ParticipantService _participantService;
        private FileWriter _fileWriter;
        public OrganizatorPresenter(OrganizatorGUI organizatorGUI)
        {
            _organizatorGUI = organizatorGUI;
            _presentationService = new PresentationService();
            _participantService = new ParticipantService();
            _fileWriter = new FileWriter();
            StartSetup();
        }

        private async void StartSetup()
        {
            await setup();
        }

        private async Task setup()
        {            
            await loadPresentationTable();
            await loadParticipantTable();
        }



        public async Task loadPresentationTable()
        {
            var presentations = await _presentationService.GetAllPresentation();
            _organizatorGUI.GetTabelPrezentari().ItemsSource = presentations;
        }
        public async Task loadParticipantTable()
        {

            var participants = await _participantService.GetAll();
            _organizatorGUI.GetParticipantsTable().ItemsSource = participants;
        }

        private PresentationDTO ValidPresentationData()
        {
            string Id = _organizatorGUI.GetIdPrezentareTextBox().Text;
            string Title = _organizatorGUI.GetTitleTextBox().Text;
            string Description = _organizatorGUI.GetDescriptionTextBox().Text;
            string Data = _organizatorGUI.GetDataDatePicker().Text;
            string Time = _organizatorGUI.GetTimeTextBox().Text;
            string Author = _organizatorGUI.GetAuthorTextBox().Text;
            Section section = (Section)_organizatorGUI.GetPresentationFormSectionComboBox().SelectedItem;
            bool anyEmpty = new[] { Id, Title, Description, Data, Time, Author }.Any(string.IsNullOrEmpty);
            if (anyEmpty)
            {
                return null;
            }
            return new PresentationDTO(int.Parse(Id), Title, Description, DateTime.Parse(Data), TimeSpan.Parse(Time), section, 1, int.Parse(Author));
        }
        private ParticipantDTO ValidParticipantData()
        {
            //TODO: Implement this method trow exception
            throw new NotImplementedException();

        }

        public async Task CreatePresentation()
        {
            PresentationDTO presentation = ValidPresentationData();
            if (presentation == null)
                _organizatorGUI.ShowMessage("All fields are mandatory");
            bool result = await _presentationService.CreatePresentation(presentation);
            if (result)
            {
                _organizatorGUI.ClearFormFields();
                loadPresentationTable();
                _organizatorGUI.ShowMessage("Operation Successful");
            }
            else
            {
                _organizatorGUI.ShowMessage("Operation Failed");
            }
        }
        public async Task UpdatePresentation()
        {
            PresentationDTO presentation = ValidPresentationData();
            if (presentation == null)
                _organizatorGUI.ShowMessage("All fields are mandatory");
            bool result = await _presentationService.UpdatePresentation(presentation);
            if (result)
            {
                _organizatorGUI.ClearFormFields();
                loadPresentationTable();
                _organizatorGUI.ShowMessage("Operation Successful");
            }
            else
            {
                _organizatorGUI.ShowMessage("Operation Failed");
            }
        }
        public async Task DeletePresentation()
        {

            PresentationDTO presentation = ValidPresentationData();
            if (presentation == null)
                _organizatorGUI.ShowMessage("All fields are mandatory");
            bool deletePresentationResult = await _presentationService.DeletePresentation(presentation);
            if (deletePresentationResult)
            {
                _organizatorGUI.ClearFormFields();
                loadPresentationTable();
                _organizatorGUI.ShowMessage("Operation Successful");
            }
            else
            {
                _organizatorGUI.ShowMessage("Operation Failed");
            }

        }

        internal async Task FilterPresentations()
        {
            Section section = (Section)_organizatorGUI.GetFilterPresentationsComboBox().SelectedIndex;
            if (section == Section.ALL)
            {
                await loadPresentationTable();
                return;
            }
            else
            {
                var presentations = await _presentationService.GetPresentationsbySection(section);
                _organizatorGUI.GetTabelPrezentari().ItemsSource = presentations;
                return;
            }
        }

        internal async Task DownloadPresentationList()
        {
            String selectedFileFormat = _organizatorGUI.GetSelectFormatComboBox().SelectedItem.ToString();
            if (String.IsNullOrEmpty(selectedFileFormat))
            {
                MessageBox.Show("Va rog alegeti un format de fisier!");
                return;
            }
            if (selectedFileFormat == "Csv")
            {
                List<PresentationDTO> ListaPrezentari = _organizatorGUI.GetTabelPrezentari().ItemsSource.Cast<PresentationDTO>().ToList();
                _fileWriter.SaveCsv(ListaPrezentari);
                MessageBox.Show("Lista prezentarilor salvata cu succes!");
            }
            if (selectedFileFormat == "Json")
            {
                List<PresentationDTO> ListaPrezentari = _organizatorGUI.GetTabelPrezentari().ItemsSource.Cast<PresentationDTO>().ToList();
                _fileWriter.SaveJson(ListaPrezentari);
                MessageBox.Show("Lista prezentarilor salvata cu succes!");
            }
            if (selectedFileFormat == "Xml")
            {
                List<PresentationDTO> ListaPrezentari = _organizatorGUI.GetTabelPrezentari().ItemsSource.Cast<PresentationDTO>().ToList();
                _fileWriter.SaveXml(ListaPrezentari);
                MessageBox.Show("Lista prezentarilor salvata cu succes!");

            }
            if (selectedFileFormat == "Doc")
            {
                List<PresentationDTO> ListaPrezentari = _organizatorGUI.GetTabelPrezentari().ItemsSource.Cast<PresentationDTO>().ToList();
                _fileWriter.SaveDoc(ListaPrezentari);
                MessageBox.Show("Lista prezentarilor salvata cu succes!");
            }
        }


        public async Task CreateParticipant()
        {
            ParticipantDTO participant = ValidParticipantData();
            if (participant == null)
                _organizatorGUI.ShowMessage("All fields are mandatory");
            bool result = await _participantService.CreateParticipant(participant);
            if (result)
            {
                _organizatorGUI.ClearFormFields();
                loadParticipantTable();
                _organizatorGUI.ShowMessage("Operation Successful");
            }
            else
            {
                _organizatorGUI.ShowMessage("Operation Failed");
            }
        }
        public async Task UpdateParticipant()
        {
            ParticipantDTO participant = ValidParticipantData();
            if (participant == null)
                _organizatorGUI.ShowMessage("All fields are mandatory");
            bool result = await _participantService.UpdateParticipant(participant);
            if (result)
            {
                _organizatorGUI.ClearFormFields();
                loadParticipantTable();
                _organizatorGUI.ShowMessage("Operation Successful");
            }
            else
            {
                _organizatorGUI.ShowMessage("Operation Failed");
            }
        }
        public async Task DeleteParticipant()
        {
            ParticipantDTO participant = ValidParticipantData();
            if (participant == null)
                _organizatorGUI.ShowMessage("All fields are mandatory");
            bool result = await _participantService.DeleteParticipant(participant);
            if (result)
            {
                _organizatorGUI.ClearFormFields();
                loadParticipantTable();
                _organizatorGUI.ShowMessage("Operation Successful");
            }
            else
            {
                _organizatorGUI.ShowMessage("Operation Failed");
            }
        }



    }
}
