﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ISO810_ComprasProject.Models
{
    public class Users : IdentityUser
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Telefono")]
        public override string? PhoneNumber { get; set; } 

        [Display(Name = "Imagen de Perfil")]
        public string? ImagenPerfil { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime? FechaNacimiento { get; set; }

        [Required]
        [EmailAddress]
        public override string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;
    }
}
