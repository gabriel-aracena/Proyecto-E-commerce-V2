using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using OnlineSpot.Web.Controllers;
using OnlineSpot.Data.Domain.Interfaces;
using OnlineSpot.Data.Domain.Entities;
using OnlineSpot.Data.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineSpot.Tests.Controllers
{
    public class LoginControllerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly LoginController _controller;

        public LoginControllerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();

            // Inicializa el controlador con TempData y Session
            _controller = new LoginController(_userRepositoryMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                },
                TempData = new TempDataDictionary(
                    new DefaultHttpContext(),
                    Mock.Of<ITempDataProvider>()
                )
            };

            // Configura una sesión dummy
            _controller.HttpContext.Session = new DummySession();
        }

        // GET: SignIn retorna vista
        [Fact]
        public void SignIn_Get_ReturnsView()
        {
            var result = _controller.SignIn();
            Assert.IsType<ViewResult>(result);
        }

        // POST: SignIn con modelo inválido
        [Fact]
        public async Task SignIn_Post_InvalidModel_ReturnsViewWithError()
        {
            _controller.ModelState.AddModelError("Username", "Required");

            var result = await _controller.SignIn(new LoginViewModel());

            var view = Assert.IsType<ViewResult>(result);
            Assert.True(_controller.TempData.ContainsKey("Error"));
            Assert.Equal("Por favor completa todos los campos correctamente.", _controller.TempData["Error"]);
        }

        // POST: SignIn con credenciales incorrectas
        [Fact]
        public async Task SignIn_Post_WrongCredentials_ReturnsError()
        {
            _userRepositoryMock.Setup(r => r.LoginAsync(It.IsAny<LoginViewModel>()))
                .ReturnsAsync((User)null);

            var result = await _controller.SignIn(new LoginViewModel { Username = "test", Password = "wrong" });

            var view = Assert.IsType<ViewResult>(result);
            Assert.Equal("Credenciales incorrectas. Intenta nuevamente.", _controller.TempData["Error"]);
        }

        // POST: SignIn usuario inactivo
        [Fact]
        public async Task SignIn_Post_InactiveUser_ReturnsError()
        {
            var user = new User
            {
                id = Guid.NewGuid(),
                userName = "InactiveUser",
                name = "Shany",
                lastName = "Gomez",
                email = "shany@mail.com",
                passwordHash = "ErrorUser@2005",
                role = 1,
                IsActive = false
            };

            _userRepositoryMock.Setup(r => r.LoginAsync(It.IsAny<LoginViewModel>())).ReturnsAsync(user);

            var result = await _controller.SignIn(new LoginViewModel());
            var view = Assert.IsType<ViewResult>(result);
            Assert.Equal("Tu cuenta no está activa. Contacta al administrador.", _controller.TempData["Error"]);
        }

        // POST: SignIn exitoso redirige al Home
        [Fact]
        public async Task SignIn_Post_ValidUser_RedirectsToHome()
        {
            var user = new User
            {
                id = Guid.NewGuid(),
                userName = "Shany",
                name = "Shany",
                lastName = "Peña",
                email = "shany@mail.com",
                passwordHash = "ErrorUser@2005",
                role = 1,
                IsActive = true
            };

            _userRepositoryMock.Setup(r => r.LoginAsync(It.IsAny<LoginViewModel>())).ReturnsAsync(user);

            var result = await _controller.SignIn(new LoginViewModel { Username = "Shany", Password = "123" });

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
            Assert.Equal("Home", redirect.ControllerName);
        }

        // GET: SignUp retorna vista
        [Fact]
        public void SignUp_Get_ReturnsView()
        {
            var result = _controller.SignUp();
            Assert.IsType<ViewResult>(result);
        }

        // POST: SignUp modelo inválido
        [Fact]
        public async Task SignUp_Post_InvalidModel_ReturnsView()
        {
            _controller.ModelState.AddModelError("Email", "Required");

            var invalidUser = new SaveUserViewModel
            {
                Id = Guid.NewGuid(),
                Username = "testuser",
                Name = "Nombre",
                Lastname = "Apellido",
                Email = "",
                Tel = "8091234567",
                Password = "ErrorUser@2005",
                ConfirmPassword = "ErrorUser@2005",
                Role = 1,
                IsActive = true
            };

            var result = await _controller.SignUp(invalidUser);

            Assert.IsType<ViewResult>(result);
            Assert.True(_controller.TempData.ContainsKey("Error"));
            Assert.Equal("Completa todos los campos correctamente.", _controller.TempData["Error"]);
        }

        // POST: SignUp exitoso redirige a SignIn
        [Fact]
        public async Task SignUp_Post_ValidUser_RedirectsToSignIn()
        {
            var newUser = new User
            {
                id = Guid.NewGuid(),
                userName = "newUser",
                name = "Nuevo",
                lastName = "Usuario",
                email = "mail@test.com",
                passwordHash = "ErrorUser@2005",
                role = 1,
                IsActive = true
            };

            _userRepositoryMock.Setup(r => r.AddAsync(It.IsAny<User>())).ReturnsAsync(newUser);

            var saveVm = new SaveUserViewModel
            {
                Id = newUser.id,
                Username = newUser.userName,
                Name = newUser.name,
                Lastname = newUser.lastName,
                Email = newUser.email,
                Tel = "8090000000",
                Password = "abcggh34234fd",
                ConfirmPassword = "abcggh34234fd",
                Role = 1,
                IsActive = true
            };

            var result = await _controller.SignUp(saveVm);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("SignIn", redirect.ActionName);
        }

        // POST: SignUp lanza excepción
        [Fact]
        public async Task SignUp_Post_Exception_ReturnsError()
        {
            _userRepositoryMock.Setup(r => r.AddAsync(It.IsAny<User>())).ThrowsAsync(new Exception("DB error"));

            var user = new SaveUserViewModel
            {
                Id = Guid.NewGuid(),
                Username = "ErrorUser",
                Name = "Test",
                Lastname = "Error",
                Email = "error@mail.com",
                Tel = "8090000000",
                Password = "ErrorUser@2005",
                ConfirmPassword = "ErrorUser@2005",
                Role = 1,
                IsActive = true
            };

            var result = await _controller.SignUp(user);

            var view = Assert.IsType<ViewResult>(result);
            Assert.Contains("Ocurrió un error", _controller.TempData["Error"].ToString());
        }

        // Logout limpia la sesión y redirige
        [Fact]
        public void Logout_ClearsSession_AndRedirects()
        {
            _controller.HttpContext.Session = new DummySession();
            var result = _controller.Logout();

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("SignIn", redirect.ActionName);
        }
    }

    // Dummy session para pruebas unitarias
    public class DummySession : ISession
    {
        private readonly Dictionary<string, byte[]> _store = new();
        public bool IsAvailable => true;
        public string Id => "test";
        public IEnumerable<string> Keys => _store.Keys;
        public void Clear() => _store.Clear();
        public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
        public void Remove(string key) => _store.Remove(key);
        public void Set(string key, byte[] value) => _store[key] = value;
        public bool TryGetValue(string key, out byte[] value) => _store.TryGetValue(key, out value);
    }
}
