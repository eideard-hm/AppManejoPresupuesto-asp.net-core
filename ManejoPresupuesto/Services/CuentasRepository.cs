using Dapper;
using ManejoPresupuesto.Interfaces;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Services
{
    public class CuentasRepository : ICuentasRepository
    {
        private readonly string connectionString;
        public CuentasRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Create(Cuenta cuenta)
        {
            using var con = new SqlConnection(connectionString);
            var id = await con.QuerySingleAsync<int>(
                @"INSERT INTO Cuentas(Nombre, TipoCuentaId, Descripcion, Balance)
                VALUES(@Nombre, @TipoCuentaId, @Descripcion, @Balance);
                SELECT SCOPE_IDENTITY();", cuenta
            );

            cuenta.Id = id;
        }

        public async Task<IEnumerable<Cuenta>> SearchById(int usuarioId)
        {
            var conn = new SqlConnection(connectionString);
            return await conn.QueryAsync<Cuenta>(@"
                SELECT c.Id, c.Nombre, c.Balance, tc.Nombre AS TipoCuenta
                FROM Cuentas c
                INNER JOIN TiposCuentas tc
                ON tc.Id = c.TipoCuentaId
                WHERE tc.UsuarioId = @UsuarioId
                ORDER BY tc.Orden;
             ", new {usuarioId});
        }
    }
}
