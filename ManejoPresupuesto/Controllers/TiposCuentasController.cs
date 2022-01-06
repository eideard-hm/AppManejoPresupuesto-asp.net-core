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
            await tiposCuentasRepository.Crear(tipoCuenta);

            return View();
        }


    }
}
