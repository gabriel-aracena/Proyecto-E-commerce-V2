using Microsoft.AspNetCore.Mvc;
using OnlineSpot.Data.Application.ViewModels;
using OnlineSpot.Data.Domain.Entities;
using OnlineSpot.Data.Domain.Interfaces;
using OnlineSpot.Data.Application.Helpers;


namespace OnlineSpot.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserRepository _userRepository;

        public LoginController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        //GET: Login/SignIn
        [HttpGet]
        public IActionResult SignIn()
        {
            return View(new LoginViewModel());
        }


        //POST: Login/SignIn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(LoginViewModel loginVm)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Por favor completa todos los campos correctamente.";
                return View(loginVm);
            }

            var user = await _userRepository.LoginAsync(loginVm);

            
            if (user == null)
            {
                TempData["Error"] = "Credenciales incorrectas. Intenta nuevamente.";
                return View(loginVm);
            }

            //Usuario inactivo
            if (!user.IsActive)
            {
                TempData["Error"] = "Tu cuenta no está activa. Contacta al administrador.";
                return View(loginVm);
            }

            //Guardar sesión (si está configurada)
            if (HttpContext.Session != null)
            {
                HttpContext.Session.SetString("UserId", user.id.ToString());
                HttpContext.Session.SetString("Username", user.userName);
            }

            TempData["Success"] = $"Bienvenido, {user.userName}";

            //Redirige al Home tras login exitoso
            return RedirectToAction("Index", "Home");
        }



        // GET: Login/SignUp
        [HttpGet]
        public IActionResult SignUp()
        {
            return View(new SaveUserViewModel());
        }

      
        // POST: Login/SignUp
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SaveUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Completa todos los campos correctamente.";
                return View(vm);
            }

            if (vm.Password != vm.ConfirmPassword)
            {
                TempData["Error"] = "Las contraseñas no coinciden.";
                return View(vm);
            }

            try
            {
                //Mapeo de SaveUserViewModel a User
                var user = new User
                {
                    id = Guid.NewGuid(),
                    userName = vm.Username?.Trim(),
                    name = vm.Name?.Trim(),
                    lastName = vm.Lastname?.Trim(),
                    email = vm.Email?.Trim(),
                    Tel = vm.Tel?.Trim(),
                    role = vm.Role == 0 ? 1 : vm.Role, // por defecto 1
                    IsActive = vm.IsActive ?? true,
                    passwordHash = PasswordEncryption.ComputeSha256Hash(vm.Password)
                };

                await _userRepository.AddAsync(user);

                TempData["Success"] = "Cuenta creada exitosamente. Ahora puedes iniciar sesión.";
                return RedirectToAction("SignIn");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Ocurrió un error al registrar el usuario: {ex.Message}";
                return View(vm);
            }
        }

      
        // GET:Login/Logout
        public IActionResult Logout()
        {
            HttpContext.Session?.Clear();
            TempData["Success"] = "Sesión cerrada correctamente.";
            return RedirectToAction("SignIn");
        }
    }
}
