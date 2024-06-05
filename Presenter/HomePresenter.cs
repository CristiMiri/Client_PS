using Client.Services;
using Client.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Client.DTO;
using Microsoft.Win32;
using System.Windows;
using DocumentFormat.OpenXml.Presentation;
using System.IO;
using DocumentFormat.OpenXml.Drawing.Diagrams;

namespace Client.Presenter
{
    internal class HomePresenter
    {

        private HomeGUI _homeGui;
        private ClientHost clientHost;
        private ParticipantService _participantService;
        private PresentationService _presentationService;

        public HomePresenter(HomeGUI homeGui)
        {
            _homeGui = homeGui;
            _participantService = new ParticipantService();
            _presentationService = new PresentationService();
            StartSetup();
        }

        private async void StartSetup()
        {
            await setup();
        }

        private async Task setup()
        {
            clientHost = ClientHost.Instance;
            await LoadConferenceTable();
            await LoadAttendPresentationComboBox();
            EnglishButton_Click();
        }

        private ParticipantDTO ValidParticipantData()
        {
            string name = _homeGui.GetNameTextBox().Text;
            string email = _homeGui.GetEmailTextBox().Text;
            string phone = _homeGui.GetPhoneTextBox().Text;
            string cnp = _homeGui.GetPinTextBox().Text;
            string photoPath = _homeGui.GetPhotoPathTexBox().Text;
            string photoFileName = Path.GetFileName(photoPath);
            string documentPath = _homeGui.GetDocumentPathTexBox().Text;
            string documentFileName = Path.GetFileName(documentPath);

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(photoPath) || string.IsNullOrEmpty(documentPath))
            {
                _homeGui.ShowMessage("All fields are required!");
                return null;
            }
            return new ParticipantDTO(0, name, email, phone, cnp, documentFileName, photoFileName);
        }
        private PresentationDTO ValidPresentationData()
        {
            string title = _homeGui.GetTitleTextBox().Text;
            string description = _homeGui.GetDescriptionTextBox().Text;
            DateTime date = _homeGui.GetDataDatePicker().SelectedDate.Value;
            string hour = _homeGui.GetTimeTextBox().Text;
            Section section = (Section)_homeGui.GetSectionComboBox().SelectedValue;
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(description) || date == null || section == null)
            {
                _homeGui.ShowMessage("All fields are required!");
                return null;
            }
            return new PresentationDTO(0, title, description, date, TimeSpan.Parse(hour), section, 1, 0);
        }
        private void ClearFields()
        {
            _homeGui.GetNameTextBox().Clear();
            _homeGui.GetEmailTextBox().Clear();
            _homeGui.GetPhoneTextBox().Clear();
            _homeGui.GetPhotoPathTexBox().Clear();
            _homeGui.GetDocumentPathTexBox().Clear();
            _homeGui.GetPinTextBox().Clear();
            //author fields
            _homeGui.GetTitleTextBox().Clear();
            _homeGui.GetDescriptionTextBox().Clear();
            _homeGui.GetTimeTextBox().Clear();
            _homeGui.GetDataDatePicker().SelectedDate = null;
            _homeGui.GetSectionComboBox().SelectedIndex = 0;
        }



        public async Task LoadConferenceTable()
        {
            List<PresentationDTO> presentations = await _presentationService.GetAllPresentation();
            Dictionary<string, byte[]> photos = await _participantService.GetParticipantsPhotos();

            foreach (PresentationDTO p in presentations)
            {
                foreach (var participant in p.Participants)
                {
                    if (photos.ContainsKey(participant.PhotoFilePath))
                    {
                        participant.PhotoImage = ByteArrayToImageSource(photos[participant.PhotoFilePath]);

                    }
                }
                p.Author.ForEach(author =>
                {
                    if (photos.ContainsKey(author.PhotoFilePath))
                    {
                        author.PhotoImage = ByteArrayToImageSource(photos[author.PhotoFilePath]);
                    }
                });

            }

            _homeGui.GetConferenceTable().ItemsSource = presentations;
        }
        private async Task LoadFilteredConferenceTable(Section section)
        {
            List<PresentationDTO> presentations = await _presentationService.GetPresentationsbySection(section);
            Dictionary<string, byte[]> photos = await _participantService.GetParticipantsPhotos();

            foreach (PresentationDTO p in presentations)
            {
                foreach (var participant in p.Participants)
                {
                    if (photos.ContainsKey(participant.PhotoFilePath))
                    {
                        participant.PhotoImage = ByteArrayToImageSource(photos[participant.PhotoFilePath]);

                    }
                }
                p.Author.ForEach(author =>
                {
                    if (photos.ContainsKey(author.PhotoFilePath))
                    {
                        author.PhotoImage = ByteArrayToImageSource(photos[author.PhotoFilePath]);
                    }
                });

            }

            _homeGui.GetConferenceTable().ItemsSource = presentations;
        }
        private async Task LoadAttendPresentationComboBox()
        {            
            List<PresentationDTO> presentations = _homeGui.GetConferenceTable().ItemsSource as List<PresentationDTO>;
            var presentationItems = presentations.Select(p => new
            {
                Id = p.Id,
                Title = p.Title
            }).ToList();
            _homeGui.GetAttendPresentationComboBox().ItemsSource = presentationItems;
            _homeGui.GetAttendPresentationComboBox().DisplayMemberPath = "Title";
            _homeGui.GetAttendPresentationComboBox().SelectedValuePath = "Id";

            _homeGui.GetSectionComboBox().ItemsSource = Enum.GetValues(typeof(Section));
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


        internal async void DownloadLink_Click(string pdfFilePath)
        {
            //string fileName = Path.GetFileName(pdf);
            //await clientHost.RequestParticipantService("GetParticipantCV", pdfFilePath);
            Dictionary<string, byte[]> file = await _participantService.GetParticipantCV(pdfFilePath);
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF file (*.pdf)|*.pdf",
                DefaultExt = "pdf",
                AddExtension = true,
                FileName = pdfFilePath
            };

            if (saveFileDialog.ShowDialog() == true)
            {

                System.IO.File.WriteAllBytes(saveFileDialog.FileName, file[pdfFilePath]);


                //string sourceFilePath = Path.Combine(_documentsDirectory, fileName);
                //File.Copy(sourceFilePath, saveFileDialog.FileName, true);
            }
        }

        internal async Task FilterPresentations()
        {
            Section section = (Section)_homeGui.GetFilterPresentationComboBox().SelectedIndex;
            if (section == Section.ALL)
            {
                await LoadConferenceTable();
                return;
            }
            else
            {
                await LoadFilteredConferenceTable(section);
            }
        }

        internal async Task CreateAuthor()
        {
            try
            {
                ParticipantDTO author = ValidParticipantData();
                if (author != null)
                {
                    Dictionary<string, byte[]> photo = new Dictionary<string, byte[]>();
                    string photoPath = _homeGui.GetPhotoPathTexBox().Text;
                    string photoFileName = Path.GetFileName(photoPath);
                    photo.Add(photoFileName, File.ReadAllBytes(photoPath));
                    await _participantService.SaveParticipantPhoto(photo);

                    Dictionary<string, byte[]> document = new Dictionary<string, byte[]>();
                    string documentPath = _homeGui.GetDocumentPathTexBox().Text;
                    string documentFileName = Path.GetFileName(documentPath);
                    document.Add(documentFileName, File.ReadAllBytes(documentPath));
                    await _participantService.SaveParticipantCV(document);
                    _homeGui.ShowMessage("Files have been successfully uploaded!"); 

                    await _participantService.UpsertParticipant(author);
                    _homeGui.ShowMessage("Author created successfully!");
                    PresentationDTO presentation = ValidPresentationData();
                    if (presentation != null)
                    {
                        int idAuthor = await _participantService.GetLastParticipantId();
                        presentation.IdAuthor = idAuthor;
                        await _presentationService.CreatePresentation(presentation);
                        _homeGui.ShowMessage("Presentation created successfully!");
                        ClearFields();                      
                    }
                }
            }
            catch (Exception ex)
            {
                _homeGui.ShowMessage($"Error creating author: {ex.Message}");
            }
        }

        internal async Task CreateParticipant()
        {
            try
            {
                ParticipantDTO participant = ValidParticipantData();

                if (participant != null)
                {
                    Dictionary<string, byte[]> photo = new Dictionary<string, byte[]>();
                    string photoPath = _homeGui.GetPhotoPathTexBox().Text;
                    string photoFileName = Path.GetFileName(photoPath);
                    photo.Add(photoFileName, File.ReadAllBytes(photoPath));
                    await _participantService.SaveParticipantPhoto(photo);

                    Dictionary<string, byte[]> document = new Dictionary<string, byte[]>();
                    string documentPath = _homeGui.GetDocumentPathTexBox().Text;
                    string documentFileName = Path.GetFileName(documentPath);
                    document.Add(documentFileName, File.ReadAllBytes(documentPath));
                    await _participantService.SaveParticipantCV(document);
                    _homeGui.ShowMessage("Files uploaded successfully!");

                    await _participantService.CreateParticipant(participant);
                    int idParticipant = await _participantService.GetLastParticipantId();
                    int idPresentation = (int)_homeGui.GetAttendPresentationComboBox().SelectedValue;
                    
                    await _participantService.CreateParticipantPresentation(Tuple.Create(idParticipant, idPresentation));
                    _homeGui.ShowMessage("Participant created successfully!");
                    ClearFields();
                }

            }
            catch (Exception ex)
            {
                _homeGui.ShowMessage($"Error creating participant: {ex.Message}");
            }
        }



        //Language Buttons
        public void EnglishButton_Click()
        {
            (Application.Current as App).ChangeLanguage("en");
        }
        public void FrenchButton_Click()
        {
            (Application.Current as App).ChangeLanguage("fr");
        }
        public void SpanishButton_Click()
        {
            (Application.Current as App).ChangeLanguage("es");
        }
    }
}
