using Portfolio.Infrastructure.Extensions;

namespace Portfolio.Domain.Base
{
    public abstract class PortfolioResponseBase
    {
        public PortfolioResponseBase()
        {
            Sucesso = true;
            Mensagem = string.Empty;
        }

        public PortfolioResponseBase(Exception ex)
        {
            Sucesso = false;

            BusinessRuleException? validation = ex as BusinessRuleException;

            if (validation != null)
            {
                BrokenRules = validation.BrokenRules;
                Mensagem = ex.Message;
            }
            else
            {
                Mensagem = ex.GetInnermostExceptionMessage();
            }
        }

        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public string CodErro { get; set; }
        public List<BusinessRule> BrokenRules { get; set; }
    }
}
