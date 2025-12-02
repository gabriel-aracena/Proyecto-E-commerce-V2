using System.ComponentModel.DataAnnotations;

namespace OnlineSpot.Data.Application.ViewModels
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "Debe ingresar su nombre")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Debe ingresar su correo electrónico")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Debe ingresar un asunto")]
        [DataType(DataType.Text)]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Debe escribir un mensaje")]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }
    }
}