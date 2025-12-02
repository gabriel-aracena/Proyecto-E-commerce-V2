using Microsoft.AspNetCore.Mvc;
using OnlineSpot.Data.Application.ViewModels;

namespace OnlineSpot.Web.Controllers
{
    public class ContactController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View(new ContactViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual IActionResult Index(ContactViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Por favor, completa todos los campos correctamente.";
                return View(model);
            }

            try
            {
                // Simulación del envío del mensaje (por ejemplo, un correo)
                // En un caso real: servicio de email, registro en BD, etc.
                TempData["Success"] = "Tu mensaje ha sido enviado exitosamente. ¡Gracias por contactarnos!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Ocurrió un error al enviar el mensaje: {ex.Message}";
                return View(model);
            }
        }
    }
}
