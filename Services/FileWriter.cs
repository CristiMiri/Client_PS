using Client.DTO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using System.IO;
using Client.Services.Strategy;

namespace Client.Services
{
    internal class FileWriter
    {
        private ISaveStrategy _saveStrategy;

        public FileWriter(ISaveStrategy saveStrategy)
        {
            _saveStrategy = saveStrategy;
        }

        public void Save(List<PresentationDTO> presentations, string filePath)
        {
            _saveStrategy.Save(presentations, filePath);
        }


        //string filePath = "C:\\Users\\crist\\Desktop\\ListaPrezentari.csv";
        //string filePath = "C:\\Users\\crist\\Desktop\\ListaPrezentari.json";
        //string filePath = "C:\\Users\\crist\\Desktop\\ListaPrezentari.xml";
        //string filePath = "C:\\Users\\crist\\Desktop\\ListaPrezentari.docx";


        
    }
}
