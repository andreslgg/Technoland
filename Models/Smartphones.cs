using System.ComponentModel.DataAnnotations;

namespace technoland.Models
{
    public class Smartphones
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Nombre del producto requerido")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Seleccione la categoria del producto")]
        public int Category { get; set; }
        [Required(ErrorMessage = "Descricion del producto requerido")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Precio del producto requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public double Price { get; set; }
        [Required(ErrorMessage = "Selecciona la imagen del producto")]
        public string? ImageUrl { get; set; }
    }
}