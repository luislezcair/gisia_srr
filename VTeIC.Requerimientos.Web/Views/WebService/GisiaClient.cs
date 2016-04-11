using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Diagnostics;

namespace VTeIC.Requerimientos.Web.Views.WebService
{
    public class GisiaClient
    {
        public const string URL = "http://localhost:8000/api/";

        public void SendKeys(List<string> searchKeys)
        {
            RequestObject request = new RequestObject
            {
                id_proyecto = 1,
                nombre_directorio = "proyecto_1",
                claves = from s in searchKeys select new SearchKeyRequest { id = 1, clave = s }
            };

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Esta llamada se bloquea hasta obtener la respuesta
            HttpResponseMessage response = client.PostAsJsonAsync("wsrequests/", request).Result;

            if(response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsAsync<ResponseObject>().Result;
                Debug.WriteLine("status: " + data.status);
            }
            else
            {
                Debug.WriteLine((int)response.StatusCode, response.ReasonPhrase);
            }
        }
    }
}