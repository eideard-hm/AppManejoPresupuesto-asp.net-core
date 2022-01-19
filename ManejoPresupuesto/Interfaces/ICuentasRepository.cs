using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Interfaces
{
    public interface ICuentasRepository
    {
        Task Create(Cuenta cuenta);
        Task<IEnumerable<Cuenta>> SearchById(int usuarioId);
    }
}
