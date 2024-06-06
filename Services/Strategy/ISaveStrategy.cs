using Client.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services.Strategy
{
    interface ISaveStrategy
    {
        void Save(List<PresentationDTO> presentations, string filePath);
    }
}
