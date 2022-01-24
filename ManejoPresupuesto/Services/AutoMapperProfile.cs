using AutoMapper;
using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Services
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Cuenta, CuentaCreacionViewModel>();
        }
    }
}
