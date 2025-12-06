using System.ComponentModel.DataAnnotations;

namespace Com_Saude.Models
{
    public class Paciente
    {
        [Key]
        public int IdPaciente { get; set; }

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


    }
}
