using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.DTO
{
    public enum Section
    {
        SCIENCE,
        TECHNOLOGY,
        MEDICINE,
        ART,
        SPORT,
        ALL
    }
    public class PresentationDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Hour { get; set; }
        public Section Section { get; set; }
        public int IdConference { get; set; }
        public int IdAuthor { get; set; }
        public List<ParticipantDTO> Participants { get; set; }
        public List<ParticipantDTO> Author { get; set; }

        // Default constructor
        public PresentationDTO() { }
        public PresentationDTO(int id, string title, string description, DateTime date, TimeSpan hour, Section section, int idConference, int idAuthor)
        {
            Id = id;
            Title = title;
            Description = description;
            Date = date;
            Hour = hour;
            Section = section;
            IdConference = idConference;
            IdAuthor = idAuthor;
        }

        public override string ToString()
        {
            return $"PresentationDTO with id {Id} and title {Title}, description: {Description}, date: {Date}, hour: {Hour}, section: {Section}, conference id: {IdConference}, author id: {IdAuthor}";
        }
        
    }
}
