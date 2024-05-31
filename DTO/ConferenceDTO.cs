using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Client.DTO
{
    internal class ConferenceDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }

        // Default constructor
        public ConferenceDTO() { }
        public ConferenceDTO(int id, string title, string location, DateTime date)
        {
            Id = id;
            Title = title;
            Location = location;
            Date = date;
        }

        public override string ToString()
        {
            return $"ConferenceDTO with id {Id}, title {Title}, location {Location}, and date {Date}.";
        }

        
    }
}
