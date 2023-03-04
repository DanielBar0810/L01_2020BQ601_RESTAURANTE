using L01_2020BQ601.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace L01_2020BQ601.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class motoristasController : ControllerBase
    {
        private readonly restauranteContext _restauranteContexto;

        public motoristasController(restauranteContext restauranteContexto)
        {
            _restauranteContexto = restauranteContexto;
        }

        ///LEER TODOS LOS REGISTROS 
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<motoristas> listadoMotoristas = (from e in _restauranteContexto.motoristas select e).ToList();

            if (listadoMotoristas.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoMotoristas);
        }


        ///CREAR
        [HttpPost]
        [Route("Add")]

        public IActionResult GuardarMotorista([FromBody] motoristas motorista)
        {

            try
            {
                _restauranteContexto.motoristas.Add(motorista);
                _restauranteContexto.SaveChanges();
                return Ok(motorista);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        ///MODIFICAR
        [HttpPut]
        [Route("actualizar/{id}")]

        public IActionResult ActualizarMotorista(int id, [FromBody] motoristas motoristaModificar)
        {
            motoristas? motoristaActual = (from e in _restauranteContexto.motoristas
                                     where e.motoristaId == id
                                     select e).FirstOrDefault();

            if (motoristaActual == null)
            {
                return NotFound();
            }

            motoristaActual.motoristaId = motoristaModificar.motoristaId;
            motoristaActual.nombreMotorista = motoristaModificar.nombreMotorista;

            _restauranteContexto.Entry(motoristaActual).State = EntityState.Modified;
            _restauranteContexto.SaveChanges();

            return Ok(motoristaModificar);

        }

        ///ELIMINAR
        [HttpDelete]
        [Route("eliminar/{id}")]

        public IActionResult EliminarMotorista(int id)
        {
            motoristas? motorista = (from e in _restauranteContexto.motoristas
                               where e.motoristaId == id
                               select e).FirstOrDefault();

            if (motorista == null) { return NotFound(); }

            _restauranteContexto.motoristas.Attach(motorista);
            _restauranteContexto.motoristas.Remove(motorista);
            _restauranteContexto.SaveChanges();

            return Ok(motorista);
        }
    }
}
