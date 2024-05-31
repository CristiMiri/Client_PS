using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.DTO
{
    internal class Response
    {
        public bool Result {  get; set; }
        public string Message {  get; set; }
        public object Data {  get; set; }

        public Response() { }
    }
}
