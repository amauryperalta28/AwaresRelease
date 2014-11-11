using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AwareswebApp.Models
{
    public class Colaborador
    {
        [Key]
        public int idColaborador { get; set; }
        public string nombreUsuario { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string sector { get; set; }
        public string localidad { get; set; }
        public string tipoUsuario { get; set; }
                
        public DateTime fechaCreacion { get; set; }

        public Colaborador(string usuario, string email, string password)
        {
            nombreUsuario = usuario;
            this.Email = email;
            this.Password = password;
            tipoUsuario = "Colaborador";
            fechaCreacion = DateTime.Now;
            sector = "";
            localidad = "";
        }
        public Colaborador()
        {
            tipoUsuario = "Colaborador";
            fechaCreacion = DateTime.Now;
            sector = "";
            localidad = "";
        }
    }

    
}