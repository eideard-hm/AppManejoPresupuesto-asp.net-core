using Dapper;
using ManejoPresupuesto.Interfaces;
using ManejoPresupuesto.Models;
using Microsoft.AspNetCore.Mvc;

namespace ManejoPresupuesto.Controllers
{
    public class TiposCuentasController : Controller
    {
        private readonly ITiposCuentas tiposCuentasRepository;
        private readonly IUsuarios usuarios;

        public TiposCuentasController(
                                      ITiposCuentas tiposCuentasRepository,
                                      IUsuarios usuarios)
        {
            this.tiposCuentasRepository = tiposCuentasRepository;
            this.usuarios = usuarios;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = usuarios.GetUserById();
            var tiposCuentas = await tiposCuentasRepository.GetAll(usuarioId);

            return View(tiposCuentas);
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

            tipoCuenta.UsuarioId = usuarios.GetUserById();
            var existsCuenta = await tiposCuentasRepository.Exists(tipoCuenta.Nombre, tipoCuenta.UsuarioId);

            if (existsCuenta)
            {
                ModelState.AddModelError(nameof(tipoCuenta.Nombre), $"El nombre {tipoCuenta.Nombre} ya existe!!.");
                return View(tipoCuenta);
            }

            await tiposCuentasRepository.Crear(tipoCuenta);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> VerifyExistsTipoCuenta(string nombre)
        {
            var usuarioId = usuarios.GetUserById();
            var existsTipoCuenta = await tiposCuentasRepository.Exists(nombre, usuarioId);

            if (existsTipoCuenta)
            {
                return Json($"El nombre {nombre} ya existe !!.");
            }
            return Json(true);
        }


    }
}
