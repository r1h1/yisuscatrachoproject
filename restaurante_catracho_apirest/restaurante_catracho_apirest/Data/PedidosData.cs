﻿using restaurante_catracho_apirest.Models;
using System.Data;
using System.Data.SqlClient;

namespace restaurante_catracho_apirest.Data
{
    public class PedidosData
    {
        private readonly string conexion;

        public PedidosData(IConfiguration configuration)
        {
            conexion = configuration.GetConnectionString("CadenaSQL")!;
        }

        public async Task<List<Pedidos>> Lista()
        {
            List<Pedidos> lista = new List<Pedidos>();

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_GetPedidos", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new Pedidos
                        {
                            IdPedido = Convert.ToInt32(reader["id_pedido"]),
                            IdUsuario = Convert.ToInt32(reader["id_usuario"]),
                            Estado = reader["estado"].ToString()!,
                            FechaCreacion = Convert.ToDateTime(reader["fecha_creacion"]),
                            FechaEntregaEstimada = reader["fecha_entrega_estimada"] as DateTime?,
                            MontoTotal = Convert.ToDecimal(reader["monto_total"])
                        });
                    }
                }
            }
            return lista;
        }

        public async Task<Pedidos> ObtenerId(int id_pedido)
        {
            Pedidos objeto = new Pedidos();

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_GetPedidoById", con);
                cmd.Parameters.AddWithValue("@id_pedido", id_pedido);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        objeto = new Pedidos
                        {
                            IdPedido = Convert.ToInt32(reader["id_pedido"]),
                            IdUsuario = Convert.ToInt32(reader["id_usuario"]),
                            Estado = reader["estado"].ToString()!,
                            FechaCreacion = Convert.ToDateTime(reader["fecha_creacion"]),
                            FechaEntregaEstimada = reader["fecha_entrega_estimada"] as DateTime?,
                            MontoTotal = Convert.ToDecimal(reader["monto_total"])
                        };
                    }
                }
            }
            return objeto;
        }

        public async Task<bool> Crear(Pedidos objeto)
        {
            bool respuesta = true;

            using (var con = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_InsertPedido", con);
                cmd.Parameters.AddWithValue("@id_usuario", objeto.IdUsuario);
                cmd.Parameters.AddWithValue("@estado", objeto.Estado);
                cmd.Parameters.AddWithValue("@fecha_entrega_estimada", (object?)objeto.FechaEntregaEstimada ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@monto_total", objeto.MontoTotal);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await con.OpenAsync();
                    int filasAfectadas = await cmd.ExecuteNonQueryAsync();
                    respuesta = filasAfectadas > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error en Crear: {ex.Message}");
                    respuesta = false;
                }
            }
            return respuesta;
        }

        public async Task<bool> Editar(Pedidos objeto)
        {
            bool respuesta = true;

            using (var con = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_UpdatePedido", con);
                cmd.Parameters.AddWithValue("@id_pedido", objeto.IdPedido);
                cmd.Parameters.AddWithValue("@id_usuario", objeto.IdUsuario);
                cmd.Parameters.AddWithValue("@estado", objeto.Estado);
                cmd.Parameters.AddWithValue("@fecha_entrega_estimada", (object?)objeto.FechaEntregaEstimada ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@monto_total", objeto.MontoTotal);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await con.OpenAsync();
                    int filasAfectadas = await cmd.ExecuteNonQueryAsync();
                    respuesta = filasAfectadas > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error en Editar: {ex.Message}");
                    respuesta = false;
                }
            }
            return respuesta;
        }

        public async Task<bool> Eliminar(int id_pedido)
        {
            bool respuesta = false;

            using (var con = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_DeletePedido", con);
                cmd.Parameters.AddWithValue("@id_pedido", id_pedido);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await con.OpenAsync();
                    int filasAfectadas = await cmd.ExecuteNonQueryAsync();
                    respuesta = filasAfectadas > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error en Eliminar: {ex.Message}");
                    respuesta = false;
                }
            }
            return respuesta;
        }
    }
}