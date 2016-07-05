﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
            LoadMetadata();
        }

        public string FileName
        {
            get
            {
                // Remove the file extension for the name
                return Path.GetFileNameWithoutExtension(FullPath);
            }
        }

        public string VirtualPath { get; set; }

        public string SourceURL { get; private set; }

        private void LoadMetadata()
        {
            string file = Path.ChangeExtension(FullPath, "json");

            if(File.Exists(file))
            {
                using (StreamReader r = new StreamReader(file))
                {
                    string jsonString = r.ReadToEnd();
                    var metadata = JsonConvert.DeserializeObject<MetadataFile>(jsonString);
                    SourceURL = metadata.Metadata.url;
                }
            }
        }
    }

    public class MetadataFile
    {
        public List<MetadataDocument> document { get; set; }

        public MetadataDocument Metadata
        {
            get
            {
                return document[0];
            }
        }
    }

    public class MetadataDocument
    {
      // "url": "https://www.youtube.com/", 
      // "id_request": "12", 
      // "weight": 1.0276595799263404, 
      // "filename": "01_www.youtube.com.html"
        public string url { get; set; }
    }

    public static class StringExt
    {
        // Limita la longitud de la cadena en maxLength y agrega "..." si la cortó
        public static string Truncate(this string value, int maxLength)
        {

            if (string.IsNullOrEmpty(value)) { return value; }
            return value.Length <= maxLength ? value : (value.Substring(0, maxLength) + "..."); 
        }
    }
}