using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSpot.Data.Application.ViewModels
{
    public class SaveUserViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Debe colocar su nombre")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Debe colocar su apellido")]
        [DataType(DataType.Text)]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Debe colocar su email")]
        [DataType(DataType.Text)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Debe colocar su telefono")]
        [RegularExpression(@"(809|829|849)\d{8}$|^([0-9]{10})$", ErrorMessage = "EL número no es válido, debe tener el formato de Republica Dominicana")]
        [DataType(DataType.Text)]
        public string Tel { get; set; }

        public int Role { get; set; }

        [Required(ErrorMessage = "Debe colocar su nombre de usuario")]
        [DataType(DataType.Text)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Ingrese la contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Las contraseñas no coinciden")]
        [Required(ErrorMessage = "Ingrese la contraseña")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public bool? IsActive { get; set; }

    }
}
