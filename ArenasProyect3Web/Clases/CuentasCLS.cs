using System.ComponentModel.DataAnnotations;

namespace ArenasProyect3Web.Clases
{
    public class CuentasCLS
    {
        [Display(Name = "Código")]
        public int? IdCuenta { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "Debe ingresar un modelo")]
        public string? DescripcionCuenta { get; set; }

        [Display(Name = "Abreviatura")]
        [Required(ErrorMessage = "Debe ingresar una abreviatura")]
        public string? Abreviatura { get; set; }

        [Display(Name = "Código SUNAT")]
        [Required(ErrorMessage = "Debe ingresar un código de SUNAT")]
        public string? CodSunat { get; set; }

        [Display(Name = "Estado")]
        public int? Estado { get; set; }
    }
}
