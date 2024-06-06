using Client.DTO;
using Client.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Facades
{
    class ParticipantManagementFacade
    {
        private readonly ParticipantService _participantService;
        private readonly PresentationService _presentationService;

        public ParticipantManagementFacade()
        {
            _participantService = new ParticipantService();
        }

        public async Task CreateAuthor(ParticipantDTO author, PresentationDTO presentation)
        {
            try
            {
                Dictionary<string, byte[]> photo = new Dictionary<string, byte[]>();
                string photoPath = author.PhotoFilePath;
                string photoFileName = Path.GetFileName(photoPath);
                photo.Add(photoFileName, File.ReadAllBytes(photoPath));
                await _participantService.SaveParticipantPhoto(photo);

                Dictionary<string, byte[]> document = new Dictionary<string, byte[]>();
                string documentPath = author.PdfFilePath;
                string documentFileName = Path.GetFileName(documentPath);
                document.Add(documentFileName, File.ReadAllBytes(documentPath));
                await _participantService.SaveParticipantCV(document);

                await _participantService.UpsertParticipant(author);
                int idAuthor = await _participantService.GetLastParticipantId();
                presentation.IdAuthor = idAuthor;
                await _presentationService.CreatePresentation(presentation);
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public async Task CreateParticipant(ParticipantDTO participant, int presentationId)
        {
            try
            {
                Dictionary<string, byte[]> photo = new Dictionary<string, byte[]>();
                string photoPath = participant.PhotoFilePath;
                string photoFileName = Path.GetFileName(photoPath);
                photo.Add(photoFileName, File.ReadAllBytes(photoPath));
                await _participantService.SaveParticipantPhoto(photo);

                Dictionary<string, byte[]> document = new Dictionary<string, byte[]>();
                string documentPath = participant.PdfFilePath;
                string documentFileName = Path.GetFileName(documentPath);
                document.Add(documentFileName, File.ReadAllBytes(documentPath));
                await _participantService.SaveParticipantCV(document);

                await _participantService.CreateParticipant(participant);
                int idParticipant = await _participantService.GetLastParticipantId();

                await _participantService.CreateParticipantPresentation(Tuple.Create(idParticipant, presentationId));
            }
            catch (Exception ex)
            {
                return;
            }
        }

        internal async Task<Dictionary<string, byte[]>> GetParticipantCV(string pdfFilePath)
        {
            try
            {
                return await _participantService.GetParticipantCV(pdfFilePath);
            }
            catch (Exception ex)
            {
                return new Dictionary<string, byte[]>();
            }
        }

        internal async Task<Dictionary<string, byte[]>> GetParticipantsPhotos()
        {
            try
            {
                Dictionary<string, byte[]> photos = await _participantService.GetParticipantsPhotos();
                return photos;
            }
            catch (Exception ex)
            {
                return new Dictionary<string, byte[]>();
            }
        }
    }
}
