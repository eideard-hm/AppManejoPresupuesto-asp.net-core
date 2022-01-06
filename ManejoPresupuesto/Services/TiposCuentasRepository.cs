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
                "SP_TiposCuentas_Insertar",
                new { nombre = tipoCuenta.Nombre, usuarioId = tipoCuenta.UsuarioId },
                commandType: System.Data.CommandType.StoredProcedure
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

            return exists == 1;
        }

        public async Task<IEnumerable<TipoCuenta>> GetAll(int usuarioId)
        {
            using var con = new SqlConnection(connectionString);
            return await con.QueryAsync<TipoCuenta>(
                 @"SELECT Id, Nombre, Orden
                    FROM TiposCuentas
                    WHERE UsuarioId = @UsuarioId",
                 new { usuarioId }
            );
        }

        public async Task Edit(TipoCuenta tipoCuenta)
        {
            using var con = new SqlConnection(connectionString);
            await con.ExecuteAsync(
                @"UPDATE TiposCuentas
                    SET Nombre = @Nombre
                    WHERE Id = @Id",
                tipoCuenta
            );
        }

        public async Task Delete(int id)
        {
            using var con = new SqlConnection(connectionString);
            await con.ExecuteAsync(
                @"DELETE TiposCuentas
                  WHERE Id = @Id",
                new { id }
            );
        }

        public async Task<TipoCuenta> GetUserById(int id, int usuarioId)
        {
            using var con = new SqlConnection(connectionString);
            return await con.QueryFirstOrDefaultAsync<TipoCuenta>(
                @"SELECT Id, Nombre, Orden
                  FROM TiposCuentas
                  WHERE Id = @Id AND UsuarioId = @UsuarioId",
                new { id, usuarioId }
         );
        }
    }
}
