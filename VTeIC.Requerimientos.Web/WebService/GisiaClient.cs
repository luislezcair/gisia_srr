﻿using System;
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

        public void SendRequest(List<string> searchKeys)
        {
            var request = new WsRequest
            {
                id_proyecto = _project.Id,
                nombre_directorio = _userName + "/" + _project.Directorio,
                claves = from s in searchKeys select new SearchKeyRequest { id = 1, clave = s }
            };

            var client = new HttpClient { BaseAddress = new Uri(Url),
                                          Timeout = new TimeSpan(0, 1, 30) };

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            // Esta llamada se bloquea hasta obtener la respuesta
            HttpResponseMessage response = client.PostAsJsonAsync("wsrequests/", request).Result;

            if(response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsAsync<WsResponse>().Result;
                ProcessResponse(data);
            }
            else
            {
                throw new HttpException((int)response.StatusCode, response.ReasonPhrase);
            }
        }

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
                                          Timeout = new TimeSpan(0, 1, 30) };

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Esta llamada se bloquea hasta obtener la respuesta
            HttpResponseMessage response = client.PostAsJsonAsync("wsfilteredurlsrequests/", request).Result;

            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsAsync<WsOkResponse>().Result;
                Debug.WriteLine("Status = {0}", data.status);
            }
            else
            {
                throw new HttpException((int)response.StatusCode, response.ReasonPhrase);
            }
        }

        private void ProcessResponse(WsResponse response)
        {
            Debug.WriteLine("---------BUSCADORES---------: {0}", response.buscadores.Count());
            foreach (var searchEngineResult in response.buscadores)
            {
                Debug.WriteLine("BUSCADOR: {0}, URLS: {1}", searchEngineResult.buscador, searchEngineResult.urls.Count());
                foreach (var url in searchEngineResult.urls)
                {
                    Debug.WriteLine("URL: {0}", url.url);
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

            SendMergedUrls(wsFilteredUrlList, response.id_request);
        }
    }
}