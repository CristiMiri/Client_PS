using Client.DTO;
using Client.Services;
using Client.View;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.IO;
using Client.Services.Strategy;

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
            Dictionary<string, byte[]> photos = await _participantService.GetParticipantsPhotos();
            foreach (var participant in participants)
            {
                if (photos.ContainsKey(participant.PhotoFilePath))
                {
                    participant.PhotoImage = ByteArrayToImageSource(photos[participant.PhotoFilePath]);

                }
            }
            _organizatorGUI.GetParticipantsTable().ItemsSource = participants;
        }
        public async Task LoadFilteredParticipantTable(Section section)
        {
            var participants = await _participantService.GetParticipantsbySection(section);
            Dictionary<string, byte[]> photos = await _participantService.GetParticipantsPhotos();
            foreach (var participant in participants)
            {
                if (photos.ContainsKey(participant.PhotoFilePath))
                {
                    participant.PhotoImage = ByteArrayToImageSource(photos[participant.PhotoFilePath]);

                }
            }
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
            if(String.IsNullOrEmpty(_organizatorGUI.GetIdParticipantTextBox().Text))
            {
                _organizatorGUI.GetIdParticipantTextBox().Text = "0";
            }
            int id = Convert.ToInt32(_organizatorGUI.GetIdParticipantTextBox().Text);
            string name = _organizatorGUI.GetNameTextBox().Text;
            string email = _organizatorGUI.GetEmailTextBox().Text;
            string phone = _organizatorGUI.GetPhoneTextBox().Text;
            string pin = _organizatorGUI.GetPinTextBox().Text;
            string photoPath = _organizatorGUI.GetPhotoPathTextBox().Text;
            string documentPath = _organizatorGUI.GetDocumentPathTextBox().Text;
            if (String.IsNullOrEmpty(name) || String.IsNullOrEmpty(email) || String.IsNullOrEmpty(phone) || String.IsNullOrEmpty(pin) || String.IsNullOrEmpty(photoPath) || String.IsNullOrEmpty(documentPath))
            {
                MessageBox.Show("Va rog completati toate campurile!");
                return null;
            }
            if (!email.Contains("@") || !email.Contains("."))
            {
                MessageBox.Show("Email invalid!");
                return null;
            }
            if (phone.Length != 10)
            {
                MessageBox.Show("Numar de telefon invalid!");
                return null;
            }
            if (pin.Length != 13)
            {
                MessageBox.Show("CNP invalid!");
                return null;
            }
            return new ParticipantDTO(id, name, email, phone, pin, documentPath, photoPath);


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
            Subject.GetInstance().Notify();
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
            Subject.GetInstance().Notify();
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
            Subject.GetInstance().Notify();

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
                await LoadFilteredParticipantTable(section);
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

            ISaveStrategy saveStrategy;
            switch (selectedFileFormat)
            {
                case "Csv":
                    saveStrategy = new CsvSaveStrategy();
                    break;
                case "Json":
                    saveStrategy = new JsonSaveStrategy();
                    break;
                case "Xml":
                    saveStrategy = new XmlSaveStrategy();
                    break;
                case "Doc":
                    saveStrategy = new DocxSaveStrategy();
                    break;
                default:
                    throw new ArgumentException("Invalid file format");
            }

            List<PresentationDTO> ListaPrezentari = _organizatorGUI.GetTabelPrezentari().ItemsSource.Cast<PresentationDTO>().ToList();
            FileWriter fileWriter = new FileWriter(saveStrategy);
            fileWriter.Save(ListaPrezentari, "C:\\Users\\crist\\Desktop\\ListaPrezentari." + selectedFileFormat.ToLower());
            MessageBox.Show("Lista prezentarilor salvata cu succes!");
        }

        

        public async Task CreateParticipant()
        {
            ParticipantDTO participant = ValidParticipantData();
            if (participant == null)
                _organizatorGUI.ShowMessage("All fields are mandatory");
            else
            {
                Dictionary<string, byte[]> photo = new Dictionary<string, byte[]>();
                string photoPath = _organizatorGUI.GetPhotoPathTextBox().Text;
                string photoFileName = Path.GetFileName(photoPath);
                photo.Add(photoFileName, File.ReadAllBytes(photoPath));
                await _participantService.SaveParticipantPhoto(photo);

                Dictionary<string, byte[]> document = new Dictionary<string, byte[]>();
                string documentPath = _organizatorGUI.GetDocumentPathTextBox().Text;
                string documentFileName = Path.GetFileName(documentPath);
                document.Add(documentFileName, File.ReadAllBytes(documentPath));
                await _participantService.SaveParticipantCV(document);
                _organizatorGUI.ShowMessage("Files uploaded successfully!");
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
            Subject.GetInstance().Notify();
        }
        public async Task UpdateParticipant()
        {
            ParticipantDTO participant = ValidParticipantData();
            if (participant == null)
                _organizatorGUI.ShowMessage("All fields are mandatory");
            else
            {
                Dictionary<string, byte[]> photo = new Dictionary<string, byte[]>();
                string photoPath = _organizatorGUI.GetPhotoPathTextBox().Text;
                string photoFileName = Path.GetFileName(photoPath);
                photo.Add(photoFileName, File.ReadAllBytes(photoPath));
                await _participantService.SaveParticipantPhoto(photo);

                Dictionary<string, byte[]> document = new Dictionary<string, byte[]>();
                string documentPath = _organizatorGUI.GetDocumentPathTextBox().Text;
                string documentFileName = Path.GetFileName(documentPath);
                document.Add(documentFileName, File.ReadAllBytes(documentPath));
                await _participantService.SaveParticipantCV(document);
                _organizatorGUI.ShowMessage("Files uploaded successfully!");
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
            Subject.GetInstance().Notify();
        }
        public async Task DeleteParticipant()
        {
            ParticipantDTO participant = new ParticipantDTO();
            participant.Id = Convert.ToInt32(_organizatorGUI.GetIdParticipantTextBox().Text);

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
            Subject.GetInstance().Notify();
        }
        public async Task FilterParticipants()
        {
            Section section = (Section)_organizatorGUI.GetFilterParticipantsComboBox().SelectedIndex;
            if (section == Section.ALL)
            {
                await loadParticipantTable();
                return;
            }
            else
            {
                await LoadFilteredParticipantTable(section);
                return;
            }
        }
        public async Task AcceptParticipant()
        {
            ParticipantDTO participant = ValidParticipantData();
            bool result = await _participantService.AcceptParticipant(participant);
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
        public async Task RejectParticipant()
        {
            ParticipantDTO participant = ValidParticipantData();
            bool result = await _participantService.RejectParticipant(participant);
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
        public static ImageSource ByteArrayToImageSource(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
                return null;

            var image = new BitmapImage();
            using (var mem = new System.IO.MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }

    }
}
