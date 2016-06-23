using System.Collections.Generic;
using System.Web.Mvc;  
using System.IO;
using VTeIC.Requerimientos.Entidades;
using System.Collections;
using System;
using System.Linq;

namespace VTeIC.Requerimientos.Web.Controllers
{
    public class DisplayFileController : Controller
    {
        // GET: DisplayViewModel
        public ActionResult Index(string directorio)
        {
            //identify the virtual path
            string filePath = "/Archivos";

            //map the virtual path to a "local" path since GetFiles() can't use URI paths
            DirectoryInfo dir = new DirectoryInfo(Server.MapPath(filePath));

            //Get all files (but not any subdirectories) in the folder specified above
            FileInfo[] files = dir.GetFiles();
            
            DirectoryInfo dir2 = new DirectoryInfo(dir.ToString() + "\\" + User.Identity.Name +"\\" + directorio);
            FileInfo[] files2 = dir2.GetFiles();

            DisplayFile vm = new DisplayFile();
            List<DownloadableFile> listaArchivos = new List<DownloadableFile>();

            //iterate through each file, get its name and set its path, and add to my VM
            foreach (FileInfo file in files2)
            {
                if(file.FullName.EndsWith("json"))
                    continue;

                DownloadableFile newFile = new DownloadableFile();
                newFile.FileName = Path.GetFileNameWithoutExtension(file.FullName);  //remove the file extension for the name
                newFile.Path = filePath + "/" +  User.Identity.Name + "/" + directorio + "/" + file.Name; //set path to virtual directory + file name
                //newFile.Path = file.Name;
                //vm.FileList.Add(newFile);  //add each file to the right list in the Viewmodel
                listaArchivos.Add(newFile);
            }
            vm.FileList = listaArchivos;

            return View(vm);
        }
    }
}