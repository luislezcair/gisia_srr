using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Web;
using VTeIC.Requerimientos.Entidades;

namespace VTeIC.Requerimientos.Web.WebService
{
    public class GisiaClient
    {
        public string Url = Properties.Settings.Default.WebServiceURL;
        private string _userName;
        private Project _project;

        public GisiaClient(string user, Project project)
        {
            _userName = user;
            _project = project;
        }

        /// <summary>
        /// Envía una solicitud al web service con las claves de búsqueda generadas.
        /// Espera como respuesta una lista de URLs de cada buscador.
        /// </summary>
        public void SendRequest()
        {
            var request = new WsRequest
            {
                id_proyecto = _project.Id,
                nombre_directorio = _userName + "/" + _project.Directorio,
                claves = from s in _project.SearchKeys select new SearchKeyRequest { id = 1, clave = s.KeyString }
            };

            var client = new HttpClient { BaseAddress = new Uri(Url),
                                          Timeout = new TimeSpan(0, 3, 0) };

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Esta llamada se bloquea hasta obtener la respuesta
            HttpResponseMessage response = client.PostAsJsonAsync("wsrequests/", request).Result;
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsAsync<WsResponse>().Result;
                ProcessResponse(data);
            }
            else
            {
                throw new HttpException((int)response.StatusCode, response.ReasonPhrase);
            }
        }

        /// <summary>
        /// Envía la lista de URLs filtradas al web service. La respuesta es innecesaria
        /// </summary>
        /// <param name="urls">Lista filtrada de URLs</param>
        /// <param name="requestId">Id de request que devuelve el web service en el envío de claves</param>
        public void SendMergedUrls(List<WsFilteredUrl> urls, int requestId)
        {
            var request = new WsFilteredUrlsRequest
            {
                id_proyecto = _project.Id,
                nombre_directorio = _userName + "/" + _project.Directorio,
                request = requestId,
                urls = urls
            };

            var client = new HttpClient { BaseAddress = new Uri(Url),
                                          Timeout = new TimeSpan(0, 3, 0) };

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Esta llamada se bloquea hasta obtener la respuesta
            HttpResponseMessage response = client.PostAsJsonAsync("wsfilteredurlsrequests/", request).Result;

            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsAsync<WsOkResponse>().Result;
                Debug.WriteLine("Status = {0}", data.status, "");
            }
            else
            {
                throw new HttpException((int)response.StatusCode, response.ReasonPhrase);
            }
        }

        /// <summary>
        /// Imprime en la salida de depuración la respuesta del web service con las URLs de los buscadores y la lista
        /// filtrada y ordenada de URLs.
        /// </summary>
        /// <param name="response">Objeto que contiene la respuesta del web service</param>
        private void ProcessResponse(WsResponse response)
        {
            Debug.WriteLine("---------BUSCADORES---------: {0}", response.buscadores.Count());
            foreach (var searchEngineResult in response.buscadores)
            {
                Debug.WriteLine("BUSCADOR: {0}, URLS: {1}", searchEngineResult.buscador, searchEngineResult.urls.Count());
                foreach (var url in searchEngineResult.urls)
                {
                    Debug.WriteLine("URL = {0}", url.url, "");
                }
            }

            var result = UrlMerger.Procesar(response.buscadores);

            // Imprimir los resultados
            foreach (var url in result)
            {
                Debug.WriteLine("URL = {0}, Value = {1}", url.Url, url.Weight);
            }

            // Pasar la lista ordenada de URLs por peso a la lista en formato esperado por el WS
            var wsFilteredUrlList = new List<WsFilteredUrl>();
            for (int i = 0; i < result.Count(); i++)
            {
                wsFilteredUrlList.Add(new WsFilteredUrl {orden = i + 1, url = result.ElementAt(i).Url});
            }

            Debug.WriteLine("------------- ORDENADAS ---------------");
            foreach (var wsFilteredUrl in wsFilteredUrlList)
            {
                Debug.WriteLine("Orden = {0}, URL = {1}", wsFilteredUrl.orden, wsFilteredUrl.url);
            }

            if(wsFilteredUrlList.Any())
            {
                SendMergedUrls(wsFilteredUrlList, response.id_request);
                Debug.WriteLine("Se enviaron {0} URLs", wsFilteredUrlList.Count);
            }
            else
            {
                Debug.WriteLine("El WS no devolvió ninguna URLs");
            }
        }

        /// <summary>
        /// Envía una consulta al web service para obtener el estado real y una descripción del estado
        /// para este proyecto.
        /// </summary>
        public WSProjectState GetProjectStatus()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(Url),
                Timeout = new TimeSpan(0, 3, 0)
            };

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Esta llamada se bloquea hasta obtener la respuesta
            HttpResponseMessage response = client.GetAsync("wsrequeststate/project_status/?id=" + _project.Id).Result;

            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsAsync<List<WSProjectState>>().Result;

                // Por razones históricas, el web service puede devolver varios objetos de "estado", así que tomamos
                // solamente el primero de ellos.
                return data.FirstOrDefault();
            }
            else
            {
                throw new HttpException((int)response.StatusCode, response.ReasonPhrase);
            }
        }

        /// <summary>
        /// Envía un objeto WSProjectState con la propiedad 'stop' en true para detener el proyecto
        /// </summary>
        public void SendStopSignal()
        {
            var stopState = new WSProjectState
            {
                stop = true
            };

            var client = new HttpClient
            {
                BaseAddress = new Uri(Url),
                Timeout = new TimeSpan(0, 3, 0)
            };

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Esta llamada se bloquea hasta obtener la respuesta
            HttpResponseMessage response = client.PostAsJsonAsync("wsrequeststate/project_stop/?id=" + _project.Id, stopState).Result;

            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                Debug.Print("WS stop response: {0}", data);
            }
            else
            {
                throw new HttpException((int)response.StatusCode, response.ReasonPhrase);
            }
        }
    }
}