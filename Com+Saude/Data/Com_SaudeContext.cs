using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Com_Saude.Models;

namespace Com_Saude.Data
{
    public class Com_SaudeContext : DbContext
    {
        public Com_SaudeContext (DbContextOptions<Com_SaudeContext> options)
            : base(options)
        {
        }

        public DbSet<Com_Saude.Models.TipoUsuario> TipoUsuario { get; set; } = default!;
        public DbSet<Com_Saude.Models.Usuario> Usuario { get; set; } = default!;
        public DbSet<Com_Saude.Models.Medico> Medico { get; set; } = default!;
        public DbSet<Com_Saude.Models.Recepcionista> Recepcionista { get; set; } = default!;
        public DbSet<Com_Saude.Models.Paciente> Paciente { get; set; } = default!;
        public DbSet<Com_Saude.Models.Especialidade> Especialidade { get; set; } = default!;
        public DbSet<Com_Saude.Models.Agendamento> Agendamento { get; set; } = default!;
        public DbSet<Com_Saude.Models.Consulta> Consulta { get; set; } = default!;
    }
}
