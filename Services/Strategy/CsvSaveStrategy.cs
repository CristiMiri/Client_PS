using Client.DTO;
using System.IO;
using System.Text;

namespace Client.Services.Strategy
{
    class CsvSaveStrategy : ISaveStrategy
    {
        public void Save(List<PresentationDTO> presentations, string filePath)
        {
            StringBuilder csvBuilder = new StringBuilder();
            // Add the header
            csvBuilder.AppendLine("ID;Titlu;Descriere;Data;Ora;Sectiune;IdAuthor;Author;Participanti");

            foreach (var prezentare in presentations)
            {
                // Convert enum to string
                string sectiuneName = Enum.GetName(typeof(Section), prezentare.Section);
                string author = prezentare.Author.Count > 0 ? prezentare.Author[0].Name : "";

                // Escape commas in strings
                string descriere = prezentare.Description.Contains(",") ? $"\"{prezentare.Description}\"" : prezentare.Description;
                string titlu = prezentare.Title.Contains(",") ? $"\"{prezentare.Title}\"" : prezentare.Title;

                string formattedDate = prezentare.Date.ToString("D", new System.Globalization.CultureInfo("ro-Ro")); // "d" is the short date format

                List<string> participants = prezentare.Participants.ConvertAll(p => p.Name);
                StringBuilder participantsBuilder = new StringBuilder();
                foreach (var participant in participants)
                {
                    participantsBuilder.Append(participant);
                    participantsBuilder.Append(", ");
                }
                // Append each property to the CSV line
                var line = string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8}",
                                         prezentare.Id,
                                         titlu,
                                         descriere,
                                         formattedDate,
                                         prezentare.Hour,
                                         sectiuneName,
                                         prezentare.IdAuthor,
                                         author,
                                         participantsBuilder.ToString());

                csvBuilder.AppendLine(line);
            }

            // Write the CSV content to a file
            //File.WriteAllText(filePath, csvBuilder.ToString());
            File.WriteAllText(filePath, csvBuilder.ToString(), Encoding.UTF8);

        }

    }
}
