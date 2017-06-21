using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using busquedaSolrnet.Models;
using System.Text;

namespace busquedaSolrnet.Controllers
{
    public class BusquedaController : Controller
    {
        //
        // GET: /Busqueda/

       
        /* 
         public ActionResult Index( String frase)   //no se usa esto
        {
           LogicaBuscar obj = new LogicaBuscar();
           return View();
        }
        */

         [HttpPost]
        public ActionResult subirArchivo(HttpPostedFileBase file, string frase, String proyecto, string usuario, bool html = false, bool pdf = false, bool rtf=false, bool word = false, bool excel = false)
        {
            //cuando le diste a submit/buscar/etc, llama a este metodo, trae todo.
            StringBuilder a = new StringBuilder(); //es para construir la respuesta, para mandar a la vista de resultado, es un fragmento del resultado (pagina q buscaste, texto, etc)
            //LogicaBuscar obj = new LogicaBuscar(); 
            string url = Server.MapPath("~/ArchivosVTeIC/"+ usuario + "/");    //url que indica donde buscar los archivos para realizar el indexado en el servidor
            string absolutiUri = "http://localhost:1481/ArchivosVTeIC/" + usuario + "/" + proyecto + "/";   //para que sea linkeable, sustituir por ip y puerto de servidor
            url += proyecto;
            url += "/";

           /* if (file != null) {
            //si llego algo por post, para eso no mas se usa el HttpPostedFileBase file, para que no sea nulo
            //No se necesita este IF
            string archivo = file.FileName;  //no
            file.SaveAs(url + archivo);
            } */
 
            if (html || pdf || rtf || word || excel)
            {
                if (!frase.Trim().Equals(""))
                {
                    LogicaBuscar obj1 = new LogicaBuscar(); 
                    a = obj1.IndextoSolr(url, frase.Trim(), usuario+proyecto, html, pdf, rtf, word, excel, absolutiUri); //el frase.trim quita el espacio del final, le pasamos los parametros necesarios al metodo indextosolr, hace la busqueda y el resultado es un string que guarda todas las coincidencias, paginas,texto, etc
                    // }
                    // a = obj.buscarSolr(frase);
                }
                else
                {
                    a.Append("Debe insertar un criterio de búsqueda válido");
                }
            }
            else
            {
                a.Append("Debe seleccionar al menos un formato");
            }


            return View(a);  //el resultado le pasamos a la vista a subirArchivo (se llama igual que el metodo)
        }


    }
}
