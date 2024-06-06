using Client.DTO;
using System.IO;

namespace Client.Services.Strategy
{
    class XmlSaveStrategy : ISaveStrategy
    {
        public void Save(List<PresentationDTO> presentations, string filePath)
        {            
            var xml = new System.Xml.Serialization.XmlSerializer(typeof(List<PresentationDTO>));
            using (var writer = new StreamWriter(filePath))
            {
                xml.Serialize(writer, presentations);
            }

        }
    }
}
