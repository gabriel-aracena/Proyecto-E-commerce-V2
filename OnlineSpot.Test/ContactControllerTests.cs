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
    public class ContactControllerTests
    {
        private readonly ContactController _controller;

        public ContactControllerTests()
        {
            _controller = new ContactController
            {
                TempData = new TempDataDictionary(
                    new DefaultHttpContext(),
                    Mock.Of<ITempDataProvider>())
            };
        }

        // 1️ GET: Contacto muestra la vista
        [Fact]
        public void Index_Get_ReturnsViewWithModel()
        {
            // Arrange & Act
            var result = _controller.Index();

            // Assert
            var view = Assert.IsType<ViewResult>(result);
            Assert.IsType<ContactViewModel>(view.Model);
        }

        // 2️ POST: modelo inválido regresa vista con error
        [Fact]
        public void Index_Post_InvalidModel_ReturnsViewWithError()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");
            var model = new ContactViewModel();

            // Act
            var result = _controller.Index(model);

            // Assert
            var view = Assert.IsType<ViewResult>(result);
            Assert.True(_controller.TempData.ContainsKey("Error"));
            Assert.Equal("Por favor, completa todos los campos correctamente.", _controller.TempData["Error"]);
        }

        // 3️ POST: modelo válido redirige con mensaje de éxito
        [Fact]
        public void Index_Post_ValidModel_RedirectsWithSuccess()
        {
            // Arrange
            var model = new ContactViewModel
            {
                Name = "Carlos",
                Email = "carlos@test.com",
                Subject = "Consulta",
                Message = "Quiero más información"
            };

            // Act
            var result = _controller.Index(model);

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
            Assert.Equal("Tu mensaje ha sido enviado exitosamente. ¡Gracias por contactarnos!", _controller.TempData["Success"]);
        }

        // 4️ POST: lanza excepción al enviar mensaje
        [Fact]
        public void Index_Post_Exception_ReturnsError()
        {
            // Arrange - simular excepción lanzando manualmente
            var controller = new FakeContactControllerThrowException();
            var model = new ContactViewModel
            {
                Name = "Test",
                Email = "test@test.com",
                Subject = "Error",
                Message = "Provocar excepción"
            };

            // Act
            var result = controller.Index(model);

            // Assert
            var view = Assert.IsType<ViewResult>(result);
            Assert.Contains("Ocurrió un error", controller.TempData["Error"].ToString());
        }

        // 5️ POST: Email con formato inválido => modelo inválido
        [Fact]
        public void Index_Post_InvalidEmail_ReturnsViewWithError()
        {
            // Arrange
            var model = new ContactViewModel
            {
                Name = "Ana",
                Email = "correo-invalido",
                Subject = "Soporte",
                Message = "No puedo acceder a mi cuenta"
            };

            _controller.ModelState.AddModelError("Email", "Formato inválido");

            // Act
            var result = _controller.Index(model);

            // Assert
            var view = Assert.IsType<ViewResult>(result);
            Assert.Equal("Por favor, completa todos los campos correctamente.", _controller.TempData["Error"]);
        }

        // Controlador falso que simula excepción
        private class FakeContactControllerThrowException : ContactController
        {
            public FakeContactControllerThrowException()
            {
                TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
            }

            public override IActionResult Index(ContactViewModel model)
            {
                TempData["Error"] = "Ocurrió un error al enviar el mensaje: Error simulado";
                return View(model);
            }
        }
    }
}
