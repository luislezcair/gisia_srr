using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace VTeIC.Requerimientos.Web.WebService
{
    public class UrlMerger
    {
        //public static List<WeightedURL> Procesar(IEnumerable<SearchEngineResult> buscadores)
        public static IOrderedEnumerable<WeightedURL> Procesar(IEnumerable<SearchEngineResult> buscadores)
        {
            var searchEngineList = buscadores.ToList();
            var distinctUrls = (from resultado in searchEngineList from url in resultado.urls select url.url).Distinct().ToList();

            // Estimar valor de la URL basado en la posición
            int m = searchEngineList.Count;
            var weightedUrls = new List<WeightedURL>();

            foreach (string url in distinctUrls)
            {
                double valueEstimation = 0.0;

                foreach (var searchEngineResult in searchEngineList)
                {
                    int position = searchEngineResult.GetUrlsAsStringList().IndexOf(url) + 1;

                    if (position > 0)
                    {
                        valueEstimation += 1.0 / position;
                    }
                }
                valueEstimation /= m;
                weightedUrls.Add(new WeightedURL { Url = url, Weight = valueEstimation });
            }

            // TODO: Falta considerar el contenido de la clave de búsqueda para ponderar las URLs

            // Devolver la lista de URLs y sus pesos ordenados de forma descendente
            //return weightedUrls.OrderByDescending(v => v.Weight).ToList();
            return weightedUrls.OrderByDescending(v => v.Weight);
        }
    }
}