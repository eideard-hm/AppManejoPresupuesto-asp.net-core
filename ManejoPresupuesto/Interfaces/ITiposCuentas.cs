using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Interfaces
{
    public interface ITiposCuentas
    {
        Task Crear(TipoCuenta tipoCuenta);
        Task<bool> Exists(string nombre, int usuarioId);
        Task<IEnumerable<TipoCuenta>> GetAll(int usuarioId);
    }
}
