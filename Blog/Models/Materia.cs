﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models
{
    public class Materia
    {
        public long? MateriaID { get; set; }
        public string Titulo { get; set; }
        public string Texto { get; set; }
        public string FotoMimeType { get; set; }
        public byte[] Foto { get; set; }
        [NotMapped]
        public IFormFile formFile { get; set; }

    }
}
