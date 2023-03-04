using System.ComponentModel.DataAnnotations;

namespace L01_2020BQ601.Models
{
    public class platos
    {
        [Key]

        public int platoId { get; set; }

        public String nombrePlato { get; set; }

        public decimal precio { get;}
    }
}
