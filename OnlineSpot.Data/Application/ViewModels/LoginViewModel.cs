using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSpot.Data.Application.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "ingrese su nombre de usuario")]
        [DataType(DataType.Text)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Ingrese una contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

}
