﻿using Microsoft.AspNetCore.Mvc;
using Rompecabezas.Logica.Models;
using Rompecabezas.Logica.Servicios;


namespace Rompecabezas.Web.Controllers
{
    public class SalaController : Controller
    {
        private readonly ISalaService _salaService;
        public SalaController(ISalaService entityFrameworkService)
        {
            _salaService = entityFrameworkService;
        }

        public IActionResult Crear()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Crear(Sala sala)
        {
            if (ModelState.IsValid)
            {
                if (!_salaService.EstaReptidoElNickName(sala.NickName))
                {
                    Sala? guardada = _salaService.AgregarSala(sala);
                    if (guardada != null)
                    {
                        ViewBag.NuevoPartipante = guardada.NickName;
                        return View("Sala", guardada);
                    }
                }
                ViewBag.Message = "El nombre ingresado ya está en uso!";
                ViewBag.Classes = "alert alert-danger text-center row col-lg-4";
                return View();
            }
            return View();
        }
        [HttpPost]
        public IActionResult Ingresar(IFormCollection form)
        {
            if (ModelState.IsValid)
            {
                int nroSala = int.Parse(form["nroSala"]);
                string pinIngresado = form["pin"];
                string nombreUsuario = form["NickName"];
                try
                {
                    Sala sala = _salaService.ObtenerSala(nroSala, pinIngresado, nombreUsuario);
                    ViewBag.NuevoPartipante = nombreUsuario;
                    return View("Sala", sala);
                }
                catch (Exception ex)
                {
                    TempData["ErrorPin"] = ex.Message;
                } 
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
