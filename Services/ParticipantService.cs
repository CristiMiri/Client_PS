using Client.DTO;
using Client.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services
{
    internal class ParticipantService : IParticipantService
    {
        public Task<bool> CreateParticipant(ParticipantDTO participantDTO)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteParticipant(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ParticipantDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<int> GetLastParticipantId()
        {
            throw new NotImplementedException();
        }

        public Task<ParticipantDTO> GetParticipant(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ParticipantDTO>> GetParticipantsbySection(Section section)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateParticipant(ParticipantDTO participantDTO)
        {
            throw new NotImplementedException();
        }
    }
}
