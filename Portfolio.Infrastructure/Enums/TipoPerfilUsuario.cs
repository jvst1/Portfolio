using System.ComponentModel;

namespace Portfolio.Infrastructure.Enums
{
    public enum TipoPerfilUsuario
    {
        [Description("Nenhum")]
        Nenhum = 0, 
		[Description("Administrador")]
        Administrador = 1,
		[Description("Operador")]
        Operador = 1 << 1,
		[Description("Supervisor")]
        Supervisor = 1 << 1 << 1,
		[Description("Atendimento")]
        Atendimento = 1 << 1 << 1 << 1,
    }
}
