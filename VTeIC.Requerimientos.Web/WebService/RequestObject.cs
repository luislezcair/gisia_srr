using System.Collections.Generic;

namespace VTeIC.Requerimientos.Web.WebService
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

    public class WSResponse
    {
        public int id_proyecto { get; set; }
        public IEnumerable<SearchEngineResult> buscadores { get; set; }
    }

    public class SearchEngineResult
    {
        public string buscador { get; set; }
        public IEnumerable<Url> urls { get; set; }

    }

    public class Url
    {
        public string url { get; set; }
    }
}