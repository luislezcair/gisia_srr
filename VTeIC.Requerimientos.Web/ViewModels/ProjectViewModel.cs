﻿using System.IO;
using VTeIC.Requerimientos.Entidades;
using System.Web;
using System.Collections.Generic;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VTeIC.Requerimientos.Web.ViewModels
{
    public class ProjectViewModel
    {
        public ProjectViewModel(Project project)
        {
            Id = project.Id;
            Nombre = project.Nombre;
            Directorio = project.Directorio;
            State = project.State;
            UserId = project.UserId;
            LanguageId = project.Language.Id;
        }

        public ProjectViewModel() { }

        public int Id { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [StringLength(100)]
        public string Nombre { get; set; }
        public string Directorio { get; set; }
        public ProjectState State { get; set; }
        public string UserId { get; set; }
        public int LanguageId { get; set; }
        public IEnumerable<SelectListItem> Langauges { get; set; }

        public bool DirectoryExists()
        {
            DirectoryInfo dir = new DirectoryInfo(GetAbsoluteDirectoryPath());
            return dir.Exists;
        }

        public void RemoveProjectDirectory()
        {
            DirectoryInfo dir = new DirectoryInfo(GetAbsoluteDirectoryPath());
            if(dir.Exists)
            {
                dir.Delete(true);
            }
        }

        private string GetAbsoluteDirectoryPath()
        {
            return HttpContext.Current.Server.MapPath("/Archivos") + '\\' + HttpContext.Current.User.Identity.Name + '\\' + Directorio;
        }
    }
}