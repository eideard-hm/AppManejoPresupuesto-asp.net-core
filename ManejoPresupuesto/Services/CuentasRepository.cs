﻿using Dapper;
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
    }
}
