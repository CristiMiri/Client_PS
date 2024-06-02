using Client.DTO;
using Client.Services.Interfaces;
using Newtonsoft.Json;

namespace Client.Services
{
    internal class PresentationService : IPresentationService
    {
        private ClientHost _clientHost;

        public PresentationService()
        {
            _clientHost = ClientHost.Instance;
        }

        public async Task<bool> CreatePresentation(PresentationDTO presentationDTO)
        {
            try
            {
                var createUserResult = await _clientHost.RequestPresentationService("CreatePresentation", presentationDTO);
                return Convert.ToBoolean(createUserResult.Result);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> UpdatePresentation(PresentationDTO presentationDTO)
        {
            try
            {
                var updateUserResult = await _clientHost.RequestPresentationService("UpdatePresentation", presentationDTO);
                return Convert.ToBoolean(updateUserResult.Result);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> DeletePresentation(PresentationDTO presentationDTO)
        {
            try
            {
                var deleteUserResult = await _clientHost.RequestPresentationService("DeletePresentation", presentationDTO); 
                return Convert.ToBoolean(deleteUserResult.Result);
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public async Task<PresentationDTO> GetPresentation(int id)
        {
            try
            {
                var user = await _clientHost.RequestUserService("GetUser", new { id });
                return JsonConvert.DeserializeObject<PresentationDTO>(user.Data.ToString());
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<PresentationDTO>> GetAllPresentation()
        {
            try
            {
                var presentations = await _clientHost.RequestPresentationService("GetAllPresentation", new { });
                return JsonConvert.DeserializeObject<List<PresentationDTO>>(presentations.Data.ToString());

            }
            catch (Exception ex)
            {
                return new List<PresentationDTO>();
            }
        }
        public async Task<List<PresentationDTO>> GetPresentationsbySection(Section section)
        {
            try
            {
                var presentations = await _clientHost.RequestPresentationService("GetPresentationsbySection", new { section });
                return JsonConvert.DeserializeObject<List<PresentationDTO>>(presentations.Data.ToString());

            }
            catch (Exception ex)
            {
                return new List<PresentationDTO>();
            }
        }
    }
}
