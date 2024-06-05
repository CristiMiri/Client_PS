using Client.DTO;
using Client.Services.Interfaces;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services
{
    internal class ParticipantService : IParticipantService
    {
        public async Task<bool> CreateParticipant(ParticipantDTO participantDTO)
        {
            try
            {
                var createUserResult = await ClientHost.Instance.RequestParticipantService("CreateParticipant", participantDTO);
                return Convert.ToBoolean(createUserResult.Result);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> UpdateParticipant(ParticipantDTO participantDTO)
        {
            try
            {
                var createUserResult = await ClientHost.Instance.RequestParticipantService("UpdateParticipant", participantDTO);
                return Convert.ToBoolean(createUserResult.Result);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> DeleteParticipant(ParticipantDTO participantDTO)
        {
            try
            {
                var createUserResult = await ClientHost.Instance.RequestParticipantService("DeleteParticipant", participantDTO);
                return Convert.ToBoolean(createUserResult.Result);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> UpsertParticipant(ParticipantDTO author)
        {
            try
            {
                var createUserResult = await ClientHost.Instance.RequestParticipantService("UpsertParticipant", author);
                return Convert.ToBoolean(createUserResult.Result);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<ParticipantDTO> GetParticipant(int id)
        {
            try
            {
                var result = await ClientHost.Instance.RequestParticipantService("GetParticipant", id);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<ParticipantDTO>(result.Data.ToString());
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<ParticipantDTO>> GetAll()
        {
            try
            {
                var result = await ClientHost.Instance.RequestParticipantService("GetAll", new { });
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<ParticipantDTO>>(result.Data.ToString());
            }
            catch (Exception ex)
            {
                return new List<ParticipantDTO>();
            }
        }
        public async Task<List<ParticipantDTO>> GetParticipantsbySection(Section section)
        {
            try
            {
                var result = await ClientHost.Instance.RequestParticipantService("GetParticipantsbySection", new { });
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<ParticipantDTO>>(result.Data.ToString());
            }
            catch (Exception ex)
            {
                return new List<ParticipantDTO>();
            }
        }
        public async Task<int> GetLastParticipantId()
        {
            try
            {
                var result = await ClientHost.Instance.RequestParticipantService("GetLastParticipantId", new { });
                return Convert.ToInt32(result.Data);
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        internal async Task<Dictionary<string, byte[]>> GetParticipantsPhotos()
        {
            try
            {
                var result = await ClientHost.Instance.RequestParticipantService("GetParticipantsPhotos", new { });
                return Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, byte[]>>(result.Data.ToString());
            }
            catch (Exception ex)
            {
                return new Dictionary<string, byte[]>();
            }

        }
        public async Task<Dictionary<string, byte[]>> GetParticipantCV(string pdfFilePath)
        {
            try
            {

                var result = await ClientHost.Instance.RequestParticipantService("GetParticipantCV", pdfFilePath);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, byte[]>>(result.Data.ToString());
            }
            catch (Exception ex)
            { return null; }

        }

        public async Task<bool> SaveParticipantPhoto(Dictionary<string, byte[]> photo)
        {
            try
            {
                var createUserResult = await ClientHost.Instance.RequestParticipantService("SaveParticipantPhoto", photo);
                return Convert.ToBoolean(createUserResult.Result);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> SaveParticipantCV(Dictionary<string, byte[]> file)
        {
            try
            {
                var createUserResult = await ClientHost.Instance.RequestParticipantService("SaveParticipantCV",file);
                return Convert.ToBoolean(createUserResult.Result);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        internal async Task<bool> CreateParticipantPresentation(Tuple<int, int> relation)
        {
            try
            {
                var result = await ClientHost.Instance.RequestParticipantService("CreateParticipantPresentation", new { relation});
                return Convert.ToBoolean(result.Result);
        }
            catch (Exception ex)
            {
                return false;
            }
        }


    }
}
