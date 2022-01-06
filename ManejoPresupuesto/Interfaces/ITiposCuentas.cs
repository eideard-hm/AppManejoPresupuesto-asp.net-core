using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Interfaces
{
    public interface ITiposCuentas
    {
        Task Crear(TipoCuenta tipoCuenta);
        Task Delete(int id);
        Task Edit(TipoCuenta tipoCuenta);
        Task<bool> Exists(string nombre, int usuarioId);
        Task<IEnumerable<TipoCuenta>> GetAll(int usuarioId);
        Task<TipoCuenta> GetUserById(int id, int usuarioId);
    }
}
