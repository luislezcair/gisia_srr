﻿using System.Collections.Generic;
using System.Linq;

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

    public class WsOkResponse
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

        public List<string> GetUrlsAsStringList()
        {
            return (from u in urls select u.url).ToList();
        }

    }

    public class Url
    {
        public string url { get; set; }
    }

    public class WsFilteredUrl
    {
        public int orden { get; set; }
        public string url { get; set; }
    }

    public class WSFilteredUrlsRequest
    {
        public int id_proyecto { get; set; }
        public string nombre_directorio { get; set; }
        public List<WsFilteredUrl> urls { get; set; }
    }
}