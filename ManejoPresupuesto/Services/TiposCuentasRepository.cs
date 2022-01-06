using Dapper;
using ManejoPresupuesto.Interfaces;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Services
{
    public class TiposCuentasRepository : ITiposCuentas
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

        public async Task<bool> Exists(string nombre, int usuarioId)
        {
            using var con = new SqlConnection(connectionString);
            var exists = await con.QueryFirstOrDefaultAsync<int>(
                @"SELECT 1 
                  FROM TiposCuentas
                  WHERE Nombre = @Nombre AND UsuarioId = @UsuarioId;",
                  new { nombre, usuarioId }
            );

            return  exists == 1;
        }
    }
}
