using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace VTeIC.Requerimientos.Web.WebService
{
    public class UrlMerger
    {
        //public static void Merge()
        //{


        //    //string clavebusqueda = "patents or papers or te or calidad";

        //    ////convertir a un dictionary las urls y sus valores
        //    //Dictionary<string, string[]> urlDic = new Dictionary<string, string[]>();

        //    //urlDic.Add(".edu", new string[] {"academic","research","papers","cite"});
        //    //urlDic.Add("patents", new string[] { "patents", "technical", "production" });
        //    //urlDic.Add(".com", new string[] { "commercial", "market", "comprar", "vender" });

        //    Procesar(urlLists); //, clavebusqueda,urlDic);
        //}

        
        //static string[] eliminateDuplicatesArray(string[] arr)
        //{
        //    string[] withoutDup = arr.Distinct().ToArray();
        //    return withoutDup;
        //}

        private static int FindInArray(string target, string[] arr)
        {
            var results = Array.FindIndex(arr, s => s.Equals(target));
            return results;
        }

        //string MyDictionaryToJson(Dictionary<int, List<int>> dict)
        //{
        //    var entries = dict.Select(d =>
        //        string.Format("\"{0}\": [{1}]", d.Key, string.Join(",", d.Value)));
        //    return "{" + string.Join(",", entries) + "}";
        //}

        public static List<KeyValuePair<string, double>> Procesar(string[][] arrayOfUrlOriginal) //, string claveBusquedaOriginal, Dictionary<string, string[]> referencias)
        {
            //quitar url duplicadas
            var urlListProcesed = new List<string>();
            //int posicionEncontrada = 0;
            //int pos = 0;
            for (int j = 0; j < arrayOfUrlOriginal.Length; j += 1)
            {
                for (int k = 0; k < arrayOfUrlOriginal[j].Length; k += 1)
                {
                    int posicionEncontrada = FindInArray(arrayOfUrlOriginal[j][k], urlListProcesed.ToArray());
                    if (posicionEncontrada < 0)
                    {
                        urlListProcesed.Add(arrayOfUrlOriginal[j][k]);
                        //pos += 1;
                    }
                }
            }

            //estimar valor de la url basado en la posicion
            int m = 0;
            var valorList = new List<double>();
            for (int x = 0; x < urlListProcesed.Count - 1; x++)
            {
                string stringActual = urlListProcesed[x];
                double valueEstimation = 0;
                for (int y = 0; y < arrayOfUrlOriginal.Length; y++)
                {
                    double position = FindInArray(stringActual, arrayOfUrlOriginal[y]) + 1;
                    if (position > 0)
                    {
                        valueEstimation = valueEstimation + (1 / position);
                    }
                    m = arrayOfUrlOriginal.Length;
                }
                valueEstimation = valueEstimation / m;
                valorList.Add(valueEstimation);
            }

            ////aca lo que hay que hacer es buscar de los terminos originales si aparecen algunos de los items
            ////de las referencias una vez encontrados 
            ////ver si aparecer los .edu en cada url por ej
            //for (int i = 0; i < referencias.Count - 1; i++)
            //{
            //    string[] terminos;
            //    terminos = referencias.Values[i];
            //    for (int j = 0; j < referencias[j].Count; j++)
            //    {

            //    }
            //    string url;
            //    url = urlListProcesed[i];

            //}


            //convertir a un dictionary las urls y sus valores
            Dictionary<string, double> urlDic = new Dictionary<string, double>();
            for (int j = 0; j < urlListProcesed.Count - 1; j++)
            {
                urlDic.Add(urlListProcesed[j], valorList[j]);
            }

            //ordenar el dictionary de acuerdo al valor
            var urlDicOrdered = urlDic.ToList();
            urlDicOrdered.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));
            return urlDicOrdered;

            //Console.ReadKey();

            // ver de recibir las claves de busquedas para poder sumar .2 en un case, interpretar comandos? + -
            // tener en cuenta los operadores AND OR en el case
        }
    }
}