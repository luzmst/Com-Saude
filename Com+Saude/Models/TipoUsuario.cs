using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Com_Saude.Models
{
    [Table("TipoUsuario")]
    public class TipoUsuario
    {
        [Key]
        public int IdTipoUsuario { get; set; }

        [Required]
        [MaxLength(100)]
        public string DescricaoTipoUsuario { get; set; }

       

    }
}
