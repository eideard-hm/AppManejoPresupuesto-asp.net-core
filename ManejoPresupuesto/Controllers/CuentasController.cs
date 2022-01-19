using ManejoPresupuesto.Interfaces;
using ManejoPresupuesto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManejoPresupuesto.Controllers
{
    public class CuentasController : Controller
    {
        private readonly ITiposCuentas tiposCuentasRepository;
        private readonly IUsuarios usuarios;
        private readonly ICuentasRepository cuentasRepository;

        public CuentasController(
                                ITiposCuentas tiposCuentasRepository,
                                IUsuarios usuarios,
                                ICuentasRepository cuentasRepository)
        {
            this.tiposCuentasRepository = tiposCuentasRepository;
            this.usuarios = usuarios;
            this.cuentasRepository = cuentasRepository;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = usuarios.GetUserById();
            var cuentasConTipoCuenta = await cuentasRepository.SearchById(usuarioId);

            var model = cuentasConTipoCuenta
                        .GroupBy(x => x.TipoCuenta)
                        .Select(grupo =>
                                new IndiceCuentasViewModel
                                {
                                    TipoCuenta = grupo.Key,
                                    Cuentas = grupo.AsEnumerable()
                                }
                        ).ToList();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            var usuarioId = usuarios.GetUserById();
            
            var model = new CuentaCreacionViewModel();
            model.TiposCuentas = await getTiposCuentas(usuarioId);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CuentaCreacionViewModel cuentaCreacionViewModel)
        {
            var usuarioId = usuarios.GetUserById();
            var tipoCuenta = await tiposCuentasRepository.GetUserById(cuentaCreacionViewModel.Id, usuarioId);

            if(tipoCuenta == null)
            {
                RedirectToAction("NotFoundResult", "Home");
            }
            if (!ModelState.IsValid)
            {
                cuentaCreacionViewModel.TiposCuentas = await getTiposCuentas(usuarioId);
                return View(cuentaCreacionViewModel);
            }

            await cuentasRepository.Create(cuentaCreacionViewModel);
            return RedirectToAction("Index");
        }


        private async Task<IEnumerable<SelectListItem>> getTiposCuentas(int usuarioId)
        {
            var tiposCuentas = await tiposCuentasRepository.GetAll(usuarioId);
            return tiposCuentas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        } 
    }
}
