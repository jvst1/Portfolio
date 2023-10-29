namespace Portfolio.Infrastructure.Helpers
{
    public class AppSettings
    {
        #region Identity

        public string SecretToken { get; set; }
        public string SecretApi { get; set; }
        public string SecretSenha { get; set; }
        public long ExpiryMinutes { get; set; }
        public long ExpiryMinutesSenha { get; set; }

        public string AudienceApi => "ApiAudience";
        public string AudienceSenha => "AudienceSenha";
        public string Issuer => "MyApi";

        #endregion

        public string WebUrl { get; set; }
    }
}
