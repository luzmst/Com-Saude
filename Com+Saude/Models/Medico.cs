using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com_Saude.Models
{
    public class Medico
    {
        [Key]
        public int IdMedico{ get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }

        [Required]
        [MaxLength(200)]
        public string Email { get; set; }

        [MaxLength(50)]
        public string Telefone { get; set; }

        [Required]
        public DateTime DataNascimento { get; set; }

        [Required]
        [MaxLength(14)]
        public string CPF { get; set; }

        [Required]
        [MaxLength(15)]
        public string Sexo { get; set; }

        [MaxLength(255)]
        public string? UrlFoto { get; set; }

        // Relación con Especialidade
      
        [Display(Name = "Especialidade")]
        public int EspecialidadeId { get; set; }
        public virtual Especialidade? Especialidade { get; set; }


    }
}
