using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;

namespace VTeIC.Requerimientos.Web.WebService
{
    public class GisiaClient
    {
        //public const string Url = "http://localhost:8000/api/";
        public const string Url = "http://181.14.196.108:33122/api/";

        public void SendKeys(List<string> searchKeys)
        {
            var request = new RequestObject
            {
                id_proyecto = 1,
                nombre_directorio = "proyecto_1",
                claves = from s in searchKeys select new SearchKeyRequest { id = 1, clave = s }
            };

            var client = new HttpClient { BaseAddress = new Uri(Url) };

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Esta llamada se bloquea hasta obtener la respuesta
            HttpResponseMessage response = client.PostAsJsonAsync("wsrequests/", request).Result;

            if(response.IsSuccessStatusCode)
            {
                //var data = response.Content.ReadAsAsync<ResponseObject>().Result;
                var data = response.Content.ReadAsAsync<WSResponse>().Result;
                Debug.WriteLine("id_proyecto: " + data.id_proyecto);

                foreach (var buscador in data.buscadores)
                {
                    Debug.WriteLine("Buscador: " + buscador.buscador);
                    foreach (var url in buscador.urls)
                    {
                        Debug.WriteLine("URL: " + url.url);
                    }
                }
                //ProcessResponse();
            }
            else
            {
                Debug.WriteLine((int)response.StatusCode, response.ReasonPhrase);
            }
        }

        private List<KeyValuePair<string, double>> ProcessResponse()
        {
            string[][] urlLists = { new[] { "www.geniar.com",
                                        "www.microsoft.com",
                                        "www.facebook.com",
                                        "www.gmail.com",
                                        "www.google.com",
                                        "www.bing.com",
                                        "www.yahoo.com",
                                        "www.frre.utn.edu.ar",
                                        "www.patents.com",
                                        "jqtXMWYMH7",
                                        "Buh9hwNITN",
                                        "FQSvLYiCVG",
                                        "yE1qqH29p2",
                                        "3yGHD0V2FL",
                                        "GWSpxtEcTs",
                                        "KA88wX8djt",
                                        "lLvFE3wwpS",
                                        "eO5ZGZ3EzB",
                                        "oWPsRnYq6E",
                                        "www.geniar.com",
                                        "t1LUvCggs2",
                                        "pE7k2kKUHS",
                                        "bIM3UG9Cbe",
                                        "Yp1Vej76Y8",
                                        "Iwws57zWze",
                                        "1g2DtJNH89",
                                        "www.patents.com",
                                        "iVSYSKMC0G",
                                        "sZvaRXG15I",
                                        "6eZzICC7ye" } , 
                            new[] { "www.whatsapp.com",
                                        "www.msn.com",
                                        "www.stackoverflow.com",
                                        "www.asp.net",
                                        "DFyy2N3NF5",
                                        "Ra9uzL7mVG",
                                        "ZCGn8glJ1z",
                                        "www.patents.com",
                                        "f7qrLLFI9Z",
                                        "NZjKYka6Mz",
                                        "licklVHc2m",
                                        "eIzfvYxtkS",
                                        "5ALmral3fn",
                                        "www.geniar.com",
                                        "h2a1nxXygs",
                                        "eMpPxeHHcp",
                                        "www.microsoft.com",
                                        "SttCCAY4qQ",
                                        "MR3NDiVPr1",
                                        "6xDs9xnn1P",
                                        "www.bing.com",
                                        "3mYxRHYfpu",
                                        "vzAsZ7kGOR",
                                        "RdwGuNqjB0",
                                        "www.google.com",
                                        "4ZO4cV2dKG",
                                        "QFYKBtg1IM",
                                        "aSHzEjowy9",
                                        "QmtHc8WKto",
                                        "6hnBkTRpNx" }, 
                            new[] { "KGD4vQMjhl",
                                        "N5IzpEpwu4",
                                        "gqQJ3THuhq",
                                        "c5XrC8p4hH",
                                        "SAnT6tAiv3",
                                        "ktnWARKrW7",
                                        "FR7iRLOo6t",
                                        "HTJPbVFOIX",
                                        "wmYhr6kMCp",
                                        "www.bing.com",
                                        "iqxJ9AVyof",
                                        "FbdLrPmwls",
                                        "DksIrbowB3",
                                        "SlWhYm7qM5",
                                        "F668UkaukW",
                                        "rhql1v3O9A",
                                        "QpPvAFKWsB",
                                        "BiZzQ0k6NI",
                                        "www.google.com",
                                        "jPTXA9Dl4d",
                                        "VJrL4orCoo",
                                        "f9WGot79vp",
                                        "hi7J7LjxCU",
                                        "im8OQu05uE",
                                        "ZV523L7OdJ",
                                        "X6P1EDzWgl",
                                        "bMPIxFsmOX",
                                        "NDLwoTppsg",
                                        "WZiawC33i3",
                                        "4oq8smKFJp" } };

            var result = UrlMerger.Procesar(urlLists);

            // Imprimir los resultados
            foreach (KeyValuePair<string, double> kvp in result)
            {
                Debug.WriteLine("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            }

            return result;
        }
    }
}