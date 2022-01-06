using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Interfaces
{
    public interface ITiposCuentas
    {
        Task Crear(TipoCuenta tipoCuenta);
    }
}
