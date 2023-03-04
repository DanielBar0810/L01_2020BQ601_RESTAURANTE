using L01_2020BQ601.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace L01_2020BQ601.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class clientesController : ControllerBase
    {
        private readonly restauranteContext _restauranteContexto;

        public clientesController(restauranteContext restauranteContexto)
        {
            _restauranteContexto = restauranteContexto;
        }

        ///LEER TODOS LOS REGISTROS 
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<clientes> listadoClientes = (from e in _restauranteContexto.clientes select e).ToList();

            if (listadoClientes.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoClientes);
        }


        ///CREAR
        [HttpPost]
        [Route("Add")]

        public IActionResult GuardarCliente([FromBody] clientes cliente)
        {

            try
            {
                _restauranteContexto.clientes.Add(cliente);
                _restauranteContexto.SaveChanges();
                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        ///MODIFICAR
        [HttpPut]
        [Route("actualizar/{id}")]

        public IActionResult ActualizarCliente(int id, [FromBody] clientes clientesModificar)
        {
            clientes? clienteActual = (from e in _restauranteContexto.clientes
                                     where e.clienteId == id
                                     select e).FirstOrDefault();

            if (clienteActual == null)
            {
                return NotFound();
            }

            clienteActual.clienteId = clientesModificar.clienteId;
            clienteActual.nombreCliente = clientesModificar.nombreCliente;
            clienteActual.direccion = clientesModificar.direccion;

            _restauranteContexto.Entry(clienteActual).State = EntityState.Modified;
            _restauranteContexto.SaveChanges();

            return Ok(clientesModificar);

        }

        ///ELIMINAR
        [HttpDelete]
        [Route("eliminar/{id}")]

        public IActionResult EliminarCliente(int id)
        {
            clientes? cliente = (from e in _restauranteContexto.clientes
                               where e.clienteId == id
                               select e).FirstOrDefault();

            if (cliente == null) { return NotFound(); }

            _restauranteContexto.clientes.Attach(cliente);
            _restauranteContexto.clientes.Remove(cliente);
            _restauranteContexto.SaveChanges();

            return Ok(cliente);
        }
    }
}
