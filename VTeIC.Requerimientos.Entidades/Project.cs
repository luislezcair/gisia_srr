﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VTeIC.Requerimientos.Entidades
{
    public class Project
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Directorio { get; set; }
        public string UserId { get; set; }
        public bool Activo { get; set; }
    }
}
