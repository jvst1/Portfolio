using System.ComponentModel;

namespace Portfolio.Infrastructure.Enums
{
    [Flags]
    public enum TipoPerfilUsuario
    {
        [Description("Nenhum")]
        Nenhum = 0, 
		[Description("Administrador")]
        Administrador = 1,
		[Description("Comerciante")]
        Comerciante = 1 << 1,
		[Description("Cliente")]
        Cliente = 1 << 1 << 1,
    }
}
