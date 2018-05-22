using System;
using System.Web;
using Microsoft.Practices.ServiceLocation;
using SolrNet;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Web.Mvc;
using SolrNet.Commands.Parameters;
using System.Text;
using System.Security.Policy;


namespace busquedaSolrnet.Models
{
    public class LogicaBuscar
    {
        // private ISolrOperations<HtmlContent> solr;
        /*
         public LogicaBuscar()
         {


         }

         public void prueba(string url, string frase){

             string a = url + frase;
         }
         */

        public StringBuilder IndextoSolr(string url, string frase, string identificador, bool html, bool pdf, bool rtf, bool word, bool excel, string absolutiUri)
        {
            //los parametros se pasan de busquedacontroller
            Startup.InitContainer();
            Startup.Init<HtmlContent>("http://localhost:8983/solr/prueba1");   //el htmlcontent tiene la estructura del documento, por ej. texto, id, formato, etc. Aca ván los datos de conexion con solr.
            ISolrOperations<HtmlContent> solr = ServiceLocator.Current.GetInstance<ISolrOperations<HtmlContent>>();



            string content = string.Empty;
            int index = 1;  //es para darle un id a los archivos, empieza del 1

            DirectoryInfo dir = new DirectoryInfo(url);
            FileInfo[] files = dir.GetFiles();
            //foreach (var file in Directory.GetFiles(url, "*.*"))  //saca los ficheros con todas las extensiones que se encuentren en esa dirección url que pasamos como parametro, la que tiene nombre de usuario/proyecto/.. 
            foreach (var file in files)  //saca los ficheros con todas las extensiones que se encuentren en esa dirección url que pasamos como parametro, la que tiene nombre de usuario/proyecto/.. 
            {
                //FileInfo fileInfo = new FileInfo(file);
                using (Stream fileStream = File.OpenRead(file.FullName))  //va a ir analizando file x file
                {
                    fileStream.Seek(0, SeekOrigin.Begin);
                    ExtractResponse extractResponse = solr.Extract(new ExtractParameters(fileStream, file.Name, url + file.Name)  //permite extraer el texto con un metodo de solrnet.
                    {
                        ExtractFormat = ExtractFormat.Text,
                        ExtractOnly = true,

                    });
                    content = extractResponse.Content; //guardamos el texto extraído
                }
                solr.Delete(new HtmlContent() //vacia el solr.
                {
                    DiscoveryID = index,

                });
                index++;
            }
            solr.Commit();

            index = 1;   //para indexar nuevamente


            if (html)
            {   //indexo por html

                FileInfo[] filesHtml = dir.GetFiles("*.htm");

                foreach (var file in filesHtml)
                {
                    //FileInfo fileInfo = new FileInfo(file);
                    using (Stream fileStream = File.OpenRead(file.FullName))
                    {
                        fileStream.Seek(0, SeekOrigin.Begin);
                        ExtractResponse extractResponse = solr.Extract(new ExtractParameters(fileStream, file.Name, url + file.Name)
                        {
                            ExtractFormat = ExtractFormat.Text,
                            ExtractOnly = true//, 
                                              //  StreamType = "text/html"
                        });
                        content = extractResponse.Content;
                    }

                    solr.Add(new HtmlContent()
                    {   //agrego los htmls al solr
                        DiscoveryID = index,
                        AbsoluteUri = absolutiUri + file.Name, //coloco el fileinfo.name para que cuando haga click pueda abrir ese archivo
                        Text = content,
                        Title = file.Name,
                        IdRuta = identificador,
                        Extension = "html"
                    });
                    index++;
                }
            }

            if (pdf)
            {

                foreach (var file in Directory.GetFiles(url, "*.pdf"))
                {
                    FileInfo fileInfo = new FileInfo(file);
                    using (Stream fileStream = File.OpenRead(file))
                    {
                        fileStream.Seek(0, SeekOrigin.Begin);
                        ExtractResponse extractResponse = solr.Extract(new ExtractParameters(fileStream, fileInfo.Name, url + fileInfo.Name)
                        {
                            ExtractFormat = ExtractFormat.Text,
                            ExtractOnly = true,

                        });
                        content = extractResponse.Content;
                    }
                    solr.Add(new HtmlContent()
                    {
                        DiscoveryID = index,
                        AbsoluteUri = absolutiUri + fileInfo.Name,
                        Text = content,
                        Title = fileInfo.Name,
                        IdRuta = identificador,
                        Extension = "pdf"
                    });
                    index++;
                }
            }


            if (rtf)
            {

                foreach (var file in Directory.GetFiles(url, "*.rtf"))
                {
                    FileInfo fileInfo = new FileInfo(file);
                    using (Stream fileStream = File.OpenRead(file))
                    {
                        fileStream.Seek(0, SeekOrigin.Begin);
                        ExtractResponse extractResponse = solr.Extract(new ExtractParameters(fileStream, fileInfo.Name, url + fileInfo.Name)
                        {
                            ExtractFormat = ExtractFormat.Text,
                            ExtractOnly = true,

                        });
                        content = extractResponse.Content;
                    }
                    solr.Add(new HtmlContent()
                    {
                        DiscoveryID = index,
                        AbsoluteUri = absolutiUri + fileInfo.Name,
                        Text = content,
                        Title = fileInfo.Name,
                        IdRuta = identificador,
                        Extension = "rtf"
                    });
                    index++;
                }
            }


            if (word)
            {

                foreach (var file in Directory.GetFiles(url, "*.doc"))
                {
                    FileInfo fileInfo = new FileInfo(file);
                    using (Stream fileStream = File.OpenRead(file))
                    {
                        fileStream.Seek(0, SeekOrigin.Begin);
                        ExtractResponse extractResponse = solr.Extract(new ExtractParameters(fileStream, fileInfo.Name, url + fileInfo.Name)
                        {
                            ExtractFormat = ExtractFormat.Text,
                            ExtractOnly = true,

                        });
                        content = extractResponse.Content;
                    }
                    solr.Add(new HtmlContent()
                    {
                        DiscoveryID = index,
                        AbsoluteUri = absolutiUri + fileInfo.Name,
                        Text = content,
                        Title = fileInfo.Name,
                        IdRuta = identificador,
                        Extension = "doc"
                    });
                    index++;
                }
            }

            if (excel)
            {

                foreach (var file in Directory.GetFiles(url, "*.xls"))
                {
                    FileInfo fileInfo = new FileInfo(file);
                    using (Stream fileStream = File.OpenRead(file))
                    {
                        fileStream.Seek(0, SeekOrigin.Begin);
                        ExtractResponse extractResponse = solr.Extract(new ExtractParameters(fileStream, fileInfo.Name, url + fileInfo.Name)
                        {
                            ExtractFormat = ExtractFormat.Text,
                            ExtractOnly = true,

                        });
                        content = extractResponse.Content;
                    }
                    solr.Add(new HtmlContent()
                    {
                        DiscoveryID = index,
                        AbsoluteUri = absolutiUri + fileInfo.Name,
                        Text = content,
                        Title = fileInfo.Name,
                        IdRuta = identificador,
                        Extension = "excel"
                    });
                    index++;
                }
            }
            solr.Commit();


            List<SolrQuery> solrQueuries = new List<SolrQuery>();
            string extensiones = "";
            int banderaOr = 0;
            //construye el fragmento para la consulta que se hace mas abajo de acuerdo a la extension, arma un string que se usa en el solr
            if (html)
            {
                extensiones += "extension:html";
                banderaOr = 1;
            }

            if (pdf)
            {
                if (banderaOr == 1)
                {
                    extensiones += " OR ";
                }
                else
                    banderaOr = 1;

                extensiones += "extension:pdf";
            }

            if (rtf)
            {
                if (banderaOr == 1)
                {
                    extensiones += " OR ";
                }
                else
                    banderaOr = 1;


                extensiones += "extension:rtf";
            }

            if (word)
            {
                if (banderaOr == 1)
                {
                    extensiones += " OR ";
                }
                else
                    banderaOr = 1;

                extensiones += "extension: doc";
            }

            if (excel)
            {
                if (banderaOr == 1)
                {
                    extensiones += " OR ";
                }
                extensiones += "extension: excel";
            }

            solrQueuries.Add(new SolrQuery("text:" + frase + " AND idRuta:*" + identificador + "*" + " AND ( " + extensiones + " )")); //hacemos la busqueda por los campos de htmlcontent, que eso es lo que se guarda en el solr, se hace por el campo identificador para que los usuarios solo busquen en archivos de su usuario y proyecto (este identificador se pasa como parametro desde busquedacontroller que se pasa desde la vista, y se setea ese valor en cada Solr.add de logicabuscar)
            SolrMultipleCriteriaQuery solrQuery = new SolrMultipleCriteriaQuery(solrQueuries.ToArray(), "AND");  //es para hacer consultas por multiples criterios, solrQueries.toArray toma todo los queries definidos arriba, que en este caso es 1, pero se deja por escalabilidad
            QueryOptions queryOptions = new QueryOptions();
            queryOptions.Highlight = new HighlightingParameters
            {
                Fields = new[] { "text" },
                Snippets = 4,
                MaxAnalyzedChars = 100000,
                Fragsize = 50,
                BeforeTerm = "<b>",
                AfterTerm = "</b>"
            };
            queryOptions.Fields = new[] { "absoluteuri", "title", "score" };
            queryOptions.Start = 0;
            //queryOptions.Rows = 20;
            var result = solr.Query(solrQuery, queryOptions);
            int rIndex = 0;
            long startID = 0;
            //se construye el resultado, lo que va en el string "a"
            StringBuilder searchResults = new StringBuilder();
            foreach (var searchResult in result.Highlights)
            {
                // searchResults.Append("<a href=" + '"' + "file:" + "///" + result[rIndex].AbsoluteUri + '"' + " target=" + '"' + "_blank" + '"' + "> " + result[rIndex].Title + " </a> <br/> ");
                searchResults.Append("<a href='" + result[rIndex].AbsoluteUri + "'> " + result[rIndex].Title + " </a> <br/> ");
                searchResults.Append("<span>" + result[rIndex].AbsoluteUri + "</span> <br/>");
                if (rIndex == 0)
                    startID = result[rIndex].DiscoveryID;
                foreach (var text in searchResult.Value.Values)
                {
                    searchResults.Append(string.Format("{0} : {1}", searchResult.Key, string.Join("... ", text.ToArray())) + "<br/><br/>");
                }
                rIndex++;
            }
            return searchResults;


        }

        /*public StringBuilder buscarSolr(string frase){

            ISolrOperations<HtmlContent> solr = null;
            Startup.InitContainer();
            Startup.Init<HtmlContent>("http://localhost:8983/solr/prueba1");
             solr = ServiceLocator.Current.GetInstance<ISolrOperations<HtmlContent>>();*/


        //}


    }


}