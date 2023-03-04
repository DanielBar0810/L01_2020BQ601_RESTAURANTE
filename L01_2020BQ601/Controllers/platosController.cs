using L01_2020BQ601.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace L01_2020BQ601.Controllers
{
    [Route("pedidos")]
    [ApiController]
    public class platosController : ControllerBase
    {
        private readonly restauranteContext _restauranteContexto;

        public platosController(restauranteContext restauranteContexto)
        {
            _restauranteContexto = restauranteContexto;
        }

        ///LEER TODOS LOS REGISTROS 
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<platos> listadoPlatos = (from e in _restauranteContexto.platos select e).ToList();

            if (listadoPlatos.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoPlatos);
        }


        ///CREAR
        [HttpPost]
        [Route("Add")]

        public IActionResult GuardarPlato([FromBody] platos plato)
        {

            try
            {
                _restauranteContexto.platos.Add(plato);
                _restauranteContexto.SaveChanges();
                return Ok(plato);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        ///MODIFICAR
        [HttpPut]
        [Route("actualizar/{id}")]

        public IActionResult ActualizarPlato(int id, [FromBody] platos platosModificar)
        {
            platos? platoActual = (from e in _restauranteContexto.platos
                                     where e.platoId == id
                                     select e).FirstOrDefault();

            if (platoActual == null)
            {
                return NotFound();
            }

            platoActual.nombrePlato = platosModificar.nombrePlato;
            platoActual.precio = platosModificar.precio;

            _restauranteContexto.Entry(platoActual).State = EntityState.Modified;
            _restauranteContexto.SaveChanges();

            return Ok(platosModificar);

        }

        ///ELIMINAR
        [HttpDelete]
        [Route("eliminar/{id}")]

        public IActionResult EliminarPlato(int id)
        {
            platos? plato = (from e in _restauranteContexto.platos
                               where e.platoId == id
                               select e).FirstOrDefault();

            if (plato == null) { return NotFound(); }

            _restauranteContexto.platos.Attach(plato);
            _restauranteContexto.platos.Remove(plato);
            _restauranteContexto.SaveChanges();

            return Ok(plato);
        }
    }
}
