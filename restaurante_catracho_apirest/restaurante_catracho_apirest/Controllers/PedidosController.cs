﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using restaurante_catracho_apirest.Data;
using restaurante_catracho_apirest.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace restaurante_catracho_apirest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly PedidosData _data;

        public PedidosController(PedidosData data)
        {
            _data = data;
        }

        // GET: api/Pedidos - Obtener todos los pedidos
        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<Pedidos> lista = await _data.Lista();
            return Ok(lista);
        }

        // GET: api/Pedidos/{id_pedido} - Obtener un pedido por ID
        [HttpGet("{id_pedido}")]
        public async Task<IActionResult> Obtener(int id_pedido)
        {
            var pedido = await _data.ObtenerId(id_pedido);
            if (pedido == null)
            {
                return NotFound(new { isSuccess = false, message = "Pedido no encontrado" });
            }
            return Ok(pedido);
        }

        // POST: api/Pedidos - Crear un pedido
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Pedidos pedido)
        {
            if (pedido == null)
            {
                return BadRequest(new { isSuccess = false, message = "Datos inválidos" });
            }

            try
            {
                bool respuesta = await _data.Crear(pedido);
                return respuesta
                    ? StatusCode(StatusCodes.Status201Created, new { isSuccess = true, message = "Pedido creado exitosamente" })
                    : StatusCode(StatusCodes.Status500InternalServerError, new { isSuccess = false, message = "Error al crear el pedido" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { isSuccess = false, message = ex.Message });
            }
        }

        // PUT: api/Pedidos - Editar un pedido
        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] Pedidos pedido)
        {
            if (pedido == null || pedido.IdPedido == 0)
            {
                return BadRequest(new { isSuccess = false, message = "Datos inválidos" });
            }

            try
            {
                bool respuesta = await _data.Editar(pedido);
                return respuesta
                    ? Ok(new { isSuccess = true, message = "Pedido actualizado correctamente" })
                    : NotFound(new { isSuccess = false, message = "Pedido no encontrado" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { isSuccess = false, message = ex.Message });
            }
        }

        // DELETE: api/Pedidos/{id_pedido} - Eliminar un pedido
        [HttpDelete("{id_pedido}")]
        public async Task<IActionResult> Eliminar(int id_pedido)
        {
            try
            {
                bool respuesta = await _data.Eliminar(id_pedido);
                return respuesta
                    ? Ok(new { isSuccess = true, message = "Pedido eliminado correctamente" })
                    : NotFound(new { isSuccess = false, message = "Pedido no encontrado" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { isSuccess = false, message = ex.Message });
            }
        }
    }
}
