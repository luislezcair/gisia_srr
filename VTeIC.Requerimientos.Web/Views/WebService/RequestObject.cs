using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VTeIC.Requerimientos.Web.Views.WebService
{
    public class RequestObject
    {
        public int id_proyecto { get; set; }
        public string nombre_directorio { get; set; }
        public IEnumerable<SearchKeyRequest> claves { get; set; }
    }

    public class SearchKeyRequest
    {
        public int id { get; set; }
        public string clave { get; set; }
    }

    public class ResponseObject
    {
        public string status { get; set; }
    }
}