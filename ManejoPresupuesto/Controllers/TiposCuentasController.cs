using Dapper;
using ManejoPresupuesto.Interfaces;
using ManejoPresupuesto.Models;
using Microsoft.AspNetCore.Mvc;

namespace ManejoPresupuesto.Controllers
{
    public class TiposCuentasController : Controller
    {
        private readonly ITiposCuentas tiposCuentasRepository;

        public TiposCuentasController(ITiposCuentas tiposCuentasRepository)
        {
            this.tiposCuentasRepository = tiposCuentasRepository;
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(TipoCuenta tipoCuenta)
        {
            if (!ModelState.IsValid)
            {
                return View(tipoCuenta);
            }

            tipoCuenta.UsuarioId = 1;
            var existsCuenta =await tiposCuentasRepository.Exists(tipoCuenta.Nombre, tipoCuenta.UsuarioId);

            if (existsCuenta)
            {
                ModelState.AddModelError(nameof(tipoCuenta.Nombre), $"El nombre {tipoCuenta.Nombre} ya existe!!.");
                return View(tipoCuenta);
            }

            await tiposCuentasRepository.Crear(tipoCuenta);

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> VerifyExistsTipoCuenta(string nombre)
        {
            var usuarioId = 1;
            var existsTipoCuenta = await tiposCuentasRepository.Exists(nombre, usuarioId);

            if (existsTipoCuenta)
            {
                return Json($"El nombre {nombre} ya existe !!.");
            }
            return Json(true);
        }


    }
}
