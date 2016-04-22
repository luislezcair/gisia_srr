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
        public const string Url = "http://localhost:8000/api/";
        //public const string Url = "http://181.14.196.108:33122/api/";

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
                var data = response.Content.ReadAsAsync<WSResponse>().Result;
                ProcessResponse(data);
            }
            else
            {
                Debug.WriteLine((int)response.StatusCode, response.ReasonPhrase);
            }
        }

        private void ProcessResponse(WSResponse response)
        {
            var result = UrlMerger.Procesar(response.buscadores);

            // Imprimir los resultados
            foreach (var url in result)
            {
                Debug.WriteLine("URL = {0}, Value = {1}", url.Url, url.Weight);
            }
        }
    }
}