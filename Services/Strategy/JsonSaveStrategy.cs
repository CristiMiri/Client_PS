using Client.DTO;
using System.IO;
using System.Text.Json;

namespace Client.Services.Strategy
{
    class JsonSaveStrategy : ISaveStrategy
    {
        public void Save(List<PresentationDTO> presentations, string filePath)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(presentations, options);
            File.WriteAllText(filePath, json);
        }
    }
}
