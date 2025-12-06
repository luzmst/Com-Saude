using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Com_Saude.Models
{
    [Table("Agendamento")]
    public class Agendamento
    {
        [Key]
        public int IdAgendamento { get; set; }

        [MaxLength(100)]
        public string NomePaciente { get; set; }


        // Relación con Medico
        [Display(Name = "Medico")]
        public int MedicoId { get; set; }
        public virtual Medico? Medico { get; set; }

        public string NomeRecepcionista { get; set; }
        

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DataHora { get; set; }

        [MaxLength(255)]
        public string Motivo { get; set; }

       
    }
}
