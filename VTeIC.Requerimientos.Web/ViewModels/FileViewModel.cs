using System.Diagnostics;
using System.IO;

namespace VTeIC.Requerimientos.Web.ViewModels
{
    public class FileViewModel
    {
        private string FullPath { get; set; }

        public FileViewModel(string path)
        {
            FullPath = path;
        }

        public string FileName
        {
            get
            {
                //remove the file extension for the name
                return Path.GetFileNameWithoutExtension(FullPath);
            }
        }

        public string VirtualPath { get; set; }

        public string SourceURL
        {
            get
            {
                Debug.Print(FullPath);
                string file = Path.GetFileNameWithoutExtension(VirtualPath) + ".json";
                return file;
            }
        }
    }
}
