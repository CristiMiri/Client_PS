using Client.DTO;
using Client.Services;
using Client.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Microsoft.Win32;

namespace Client.Presenter
{
    internal class UtilizatorPresenter
    {
        private UtilizatorGUI _utilizatorGUI;
        private PresentationService _presentationService;
        private ParticipantService _participantService;

        public UtilizatorPresenter(UtilizatorGUI utilizatorGUI)
        {
            _utilizatorGUI = utilizatorGUI;
            _presentationService = new PresentationService();
            _participantService = new ParticipantService();
            StartSetup();
        }

        private async void StartSetup()
        {
            await Setup();
        }
        private async Task Setup()
        {
            await LoadConferenceTable();
        }


        //Load the conference table
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

            _utilizatorGUI.GetConferenceTable().ItemsSource = presentations;
        }



        //Auxiliary methods
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
        public async void DownloadLink_Click(string pdfFilePath)
        {
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

            }


        }
    }
}
