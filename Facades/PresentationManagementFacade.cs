using Client.DTO;
using Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Facades
{
    class PresentationManagementFacade
    {
        private readonly PresentationService _presentationService;

        public PresentationManagementFacade()
        {
            _presentationService = new PresentationService();
        }

        public async Task<List<PresentationDTO>> GetAllPresentations()
        {
            try
            {
                return await _presentationService.GetAllPresentation();
            }
            catch (Exception ex)
            {
                return new List<PresentationDTO>();
            }
        }

        public async Task<List<PresentationDTO>> GetPresentationsBySection(Section section)
        {
            try
            {
                return await _presentationService.GetPresentationsbySection(section);
            }
            catch (Exception ex)
            {
                return new List<PresentationDTO>();
            }
        }
    }
}
