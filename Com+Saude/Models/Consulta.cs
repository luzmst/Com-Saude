using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Com_Saude.Models
{
    [Table("Consulta")]
    public class Consulta
    {
        [Key]
        public int IdConsulta { get; set; }

        // Relación con Agendamento
        [Display(Name = "Agendamento")]
        public int AgendamentoId { get; set; }
        public virtual Agendamento? Agendamento { get; set; }

        [MaxLength(255)]
        public string Diagnostico { get; set; }

        [MaxLength(255)]
        public string Receita { get; set; }

        [MaxLength(255)]
        public string Observacoes { get; set; }
    }
}
