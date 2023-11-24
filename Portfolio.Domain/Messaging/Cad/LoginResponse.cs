namespace Portfolio.Domain.Messaging.Cad
{
    public class LoginResponse
    {
        public Guid CodigoUsuario { get; set; }
        public string Email { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Identificador { get; set; }
        public long Expires { get; set; }
    }
}
