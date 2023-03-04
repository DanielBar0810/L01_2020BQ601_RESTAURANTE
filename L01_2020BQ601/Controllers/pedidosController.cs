using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using L01_2020BQ601.Models;
using Microsoft.EntityFrameworkCore;

namespace L01_2020BQ601.Controllers
{
    [Route("pedidos")]
    [ApiController]
    public class pedidosController : ControllerBase
        {

        private readonly restauranteContext _restauranteContexto;

        public pedidosController(restauranteContext restauranteContexto)
        {
            _restauranteContexto = restauranteContexto;
        }

        ///LEER TODOS LOS REGISTROS 
            [HttpGet]
            [Route("GetAll")]
            public IActionResult Get()
            {
                List<pedidos> listadoPedidos = (from e in _restauranteContexto.pedidos select e).ToList();

                if (listadoPedidos.Count == 0)
                {
                    return NotFound();
                }

                return Ok(listadoPedidos);
            }


            ///CREAR
            [HttpPost]
            [Route("Add")]

            public IActionResult GuardarPedido([FromBody] pedidos pedido)
            {

                try
                {
                    _restauranteContexto.pedidos.Add(pedido);
                    _restauranteContexto.SaveChanges();
                    return Ok(pedido);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            ///MODIFICAR
            [HttpPut]
            [Route("actualizar/{id}")]

            public IActionResult ActualizarPedido(int id, [FromBody] pedidos pedidosModificar)
            {
                pedidos? pedidoActual = (from e in _restauranteContexto.pedidos
                                         where e.pedidoId == id
                                         select e).FirstOrDefault();

                if (pedidoActual == null)
                {
                    return NotFound();
                }

                pedidoActual.motoristaId = pedidosModificar.motoristaId;
                pedidoActual.clienteId = pedidosModificar.clienteId;
                pedidoActual.platoId = pedidosModificar.platoId;
                pedidoActual.cantidad = pedidosModificar.cantidad;
                pedidoActual.precio = pedidosModificar.precio;

                _restauranteContexto.Entry(pedidoActual).State = EntityState.Modified;
                _restauranteContexto.SaveChanges();

                return Ok(pedidosModificar);

            }

            ///ELIMINAR
            [HttpDelete]
            [Route("eliminar/{id}")]

            public IActionResult EliminarEquipo(int id)
            {
                pedidos? equipo = (from e in _restauranteContexto.pedidos
                                   where e.pedidoId == id
                                   select e).FirstOrDefault();

                if (equipo == null) { return NotFound(); }

                _restauranteContexto.pedidos.Attach(equipo);
                _restauranteContexto.pedidos.Remove(equipo);
                _restauranteContexto.SaveChanges();

                return Ok(equipo);
            }

            /*
            [HttpGet]
            [Route("Find/{filtro}")]

            public IActionResult FindByDescription(string filtro)
            {

                equipos? equipo = (from e in _equiposContexto.equipos
                                   where e.descripcion.Contains(filtro)
                                   select e).FirstOrDefault();

                if (equipo == null)
                {
                    return NotFound();
                }

                return Ok(equipo);
            }
            */
        }
}
