using Client.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services.Interfaces
{
    internal interface IPresentationService
    {
        Task<bool> CreatePresentation(PresentationDTO presentationDTO);
        Task<bool> UpdatePresentation(PresentationDTO presentationDTO);
        Task<bool> DeletePresentation(PresentationDTO presentationDTO);

        Task<PresentationDTO> GetPresentation(int id);
        Task<List<PresentationDTO>> GetAllPresentation();
        Task<List<PresentationDTO>> GetPresentationsbySection(Section section);
    }
}
