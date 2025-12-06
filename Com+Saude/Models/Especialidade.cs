using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Com_Saude.Models
{
    [Table("Especialidade")]
    public class Especialidade
    {
        [Key]
        public int IdEspecialidade { get; set; }

        [Required]
        [MaxLength(100)]
        public string Descricao{ get; set; }

      
    }
}
