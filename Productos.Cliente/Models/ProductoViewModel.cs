using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Productos.Cliente.Models;

public class ProductoViewModel
{
    public int Id { get; set; }

    [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
    public string Nombre { get; set; } = null!;

    [DataType(DataType.MultilineText)]
    [MaxLength(500, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
    public string Descripcion { get; set; } = null!;

    [Column(TypeName = "decimal(18,2)")]
    [DisplayFormat(DataFormatString = "{0:C2}")]
    public decimal Precio { get; set; }

}
