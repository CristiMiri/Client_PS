using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.DTO
{
    internal class ParticipantDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string CNP { get; set; }
        public string PdfFilePath { get; set; }
        public string PhotoFilePath { get; set; }

        // Default constructor
        public ParticipantDTO() { }

        public ParticipantDTO(int id, string name, string email, string phone, string cNP, string pdfFilePath, string photoFilePath)
        {
            Id = id;
            Name = name;
            Email = email;
            Phone = phone;
            CNP = cNP;
            PdfFilePath = pdfFilePath;
            PhotoFilePath = photoFilePath;
        }

        public override string ToString()
        {
            return $"ParticipantDTO with id {Id}, name {Name}, email {Email}, phone {Phone}, CNP {CNP}, PDF file path {PdfFilePath}, photo file path {PhotoFilePath}.";
        }        
    }
}
