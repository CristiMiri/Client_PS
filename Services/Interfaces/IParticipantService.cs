using Client.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services.Interfaces
{
    internal interface IParticipantService
    {
        Task<bool> CreateParticipant(ParticipantDTO participantDTO);
        Task<bool> UpdateParticipant(ParticipantDTO participantDTO);
        Task<bool> DeleteParticipant(ParticipantDTO participantDTO);

        Task<ParticipantDTO> GetParticipant(int id);
        Task<List<ParticipantDTO>> GetAll();
        Task<List<ParticipantDTO>> GetParticipantsbySection(Section section);
        Task<int> GetLastParticipantId();
    }
}
