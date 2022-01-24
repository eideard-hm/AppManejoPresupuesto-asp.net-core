using AutoMapper;
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
        private readonly IMapper mapper;

        public CuentasController(
                                ITiposCuentas tiposCuentasRepository,
                                IUsuarios usuarios,
                                ICuentasRepository cuentasRepository,
                                IMapper mapper)
        {
            this.tiposCuentasRepository = tiposCuentasRepository;
            this.usuarios = usuarios;
            this.cuentasRepository = cuentasRepository;
            this.mapper = mapper;
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
            model.TiposCuentas = await GetTiposCuentas(usuarioId);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CuentaCreacionViewModel cuentaCreacionViewModel)
        {
            var usuarioId = usuarios.GetUserById();
            var tipoCuenta = await tiposCuentasRepository.GetUserById(cuentaCreacionViewModel.Id, usuarioId);

            if (tipoCuenta == null)
            {
                RedirectToAction("NotFoundResult", "Home");
            }
            if (!ModelState.IsValid)
            {
                cuentaCreacionViewModel.TiposCuentas = await GetTiposCuentas(usuarioId);
                return View(cuentaCreacionViewModel);
            }

            await cuentasRepository.Create(cuentaCreacionViewModel);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var usuarioId = usuarios.GetUserById();
            var cuenta = await cuentasRepository.GetById(id, usuarioId);

            if (cuenta is null)
            {
                return RedirectToAction("NotFoundResult", "Home");
            }

            var model = mapper.Map<CuentaCreacionViewModel>(cuenta);

            model.TiposCuentas = await GetTiposCuentas(usuarioId);
            return View(model);
        }

        private async Task<IEnumerable<SelectListItem>> GetTiposCuentas(int usuarioId)
        {
            var tiposCuentas = await tiposCuentasRepository.GetAll(usuarioId);
            return tiposCuentas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }
    }
}
