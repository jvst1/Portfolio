using System.Text;

namespace Portfolio.Domain.Base
{
    public class BusinessRuleException : Exception
    {
        public List<BusinessRule> BrokenRules { get; set; }

        public BusinessRuleException(string message) : base(message)
        {
        }

        public BusinessRuleException(List<BusinessRule> brokenRules) : base(BuildBrokenRulesMessage(brokenRules))
        {
            BrokenRules = brokenRules;
        }

        private static string BuildBrokenRulesMessage(IEnumerable<BusinessRule> brokenRules)
        {
            var brokenRulesBuilder = new StringBuilder();
            foreach (var businessRule in brokenRules)
            {
                brokenRulesBuilder.AppendLine(businessRule.RuleDescription);
            }
            return brokenRulesBuilder.ToString();
        }
    }
}
