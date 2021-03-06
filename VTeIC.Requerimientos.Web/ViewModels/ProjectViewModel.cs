﻿using System.IO;
using VTeIC.Requerimientos.Entidades;
using System.Web;
using System.Collections.Generic;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System;

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
            StateReason = project.StateReason;
            StateTime = project.StateTime;
            UserId = project.UserId;
            LanguageId = project.Language.Id;
            CreatedAt = project.CreatedAt;
            WSState = project.WSState;
            WSStopped = project.WSStopped;
            Files = new List<FileViewModel>();
            SearchKeys = (from s in project.SearchKeys select s.KeyString).ToList();
        }

        public ProjectViewModel() { }

        public int Id { get; set; }

        [Display(Name = "Nombre del proyecto")]
        public string Nombre { get; set; }

        [Display(Name = "Ruta del repositorio de archivos")]
        public string Directorio { get; set; }

        [Display(Name = "Estado")]
        public ProjectState State { get; set; }

        [Display(Name = "Motivo del error")]
        public string StateReason { get; set; }

        [Display(Name = "Último cambio de estado")]
        public DateTime? StateTime { get; set; }

        [Display(Name = "Fecha y hora de creación")]
        public DateTime? CreatedAt { get; set; }

        [Display(Name = "Estado detallado")]
        public string WSState { get; set; }

        [Display(Name = "¿En ejecución?")]
        public bool WSStopped { get; set; }

        public string UserId { get; set; }

        public int LanguageId { get; set; }
        public IEnumerable<SelectListItem> Langauges { get; set; }

        public List<FileViewModel> Files { get; set; }

        public List<string> SearchKeys { get; set; }

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