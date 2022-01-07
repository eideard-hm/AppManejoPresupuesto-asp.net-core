using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Interfaces
{
    public interface ICuentasRepository
    {
        Task Create(Cuenta cuenta);
    }
}
