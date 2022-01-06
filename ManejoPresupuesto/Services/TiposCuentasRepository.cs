using Dapper;
using ManejoPresupuesto.Interfaces;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Services
{
    public class TiposCuentasRepository: ITiposCuentas
    {
        private readonly string connectionString;

        public TiposCuentasRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(TipoCuenta tipoCuenta)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(
                @"INSERT INTO TiposCuentas(Nombre, UsuarioId, Orden)
                  VALUES(@Nombre, @UsuarioId, 0);
                  SELECT SCOPE_IDENTITY();", tipoCuenta
            );

            tipoCuenta.Id = id;
        }
    }
}
