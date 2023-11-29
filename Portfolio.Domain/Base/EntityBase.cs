using Portfolio.Domain.Base.Interfaces.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Portfolio.Infrastructure.Extensions;
using Portfolio.Infrastructure.Attributes;

namespace Portfolio.Domain.Base
{
    public abstract class EntityBase<TIdType> : IEntity
    {
        private readonly List<BusinessRule> _brokenRules = new List<BusinessRule>();

        [AutomaticUpdateIgnore]
        [Key]
        public Guid Codigo { get; set; }

        [AutomaticUpdateIgnore]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public TIdType ID { get; set; }

        [NotMapped]
        public virtual bool IsValid => !GetUpdateBrokenRules().Any();

        protected abstract void InsertValidate();
        protected abstract void UpdateValidate();

        protected void AddBrokenRule(BusinessRule businessRule)
        {
            _brokenRules.Add(businessRule);
        }

        protected void AddBrokenRules(IEnumerable<BusinessRule> businessRules)
        {
            _brokenRules.AddRange(businessRules);
        }

        public List<BusinessRule> GetInsertBrokenRules()
        {
            _brokenRules.Clear();
            InsertValidate();
            return _brokenRules;
        }

        public List<BusinessRule> GetUpdateBrokenRules()
        {
            _brokenRules.Clear();
            UpdateValidate();
            return _brokenRules;
        }

        public void ThrowExceptionIfInsertInvalid()
        {
            var brokenRules = GetInsertBrokenRules();

            if (brokenRules.Any())
                throw new BusinessRuleException(brokenRules);
        }

        public void ThrowExceptionIfUpdateInvalid()
        {
            var brokenRules = GetUpdateBrokenRules();

            if (brokenRules.Any())
                throw new BusinessRuleException(brokenRules);
        }

        public override bool Equals(object obj)
        {
            var entityBase = obj as EntityBase<TIdType>;
            return entityBase != null && this == entityBase;
        }

        public override int GetHashCode()
        {
            return Codigo.GetHashCode();
        }

        public static bool operator ==(EntityBase<TIdType> entity1, EntityBase<TIdType> entity2)
        {
            if ((object)entity1 == null && (object)entity2 == null)
            {
                return true;
            }

            if ((object)entity1 == null || (object)entity2 == null)
            {
                return false;
            }

            return entity1.Codigo.ToString() == entity2.Codigo.ToString();
        }

        public static bool operator !=(EntityBase<TIdType> entity1, EntityBase<TIdType> entity2)
        {
            return !(entity1 == entity2);
        }

        public bool IsNullOrEmpty<T>()
        {
            foreach (var p in GetType().GetProperties())
            {
                var value = p.GetValue(this);
                var defaultValue = p.PropertyType.GetDefaultValue();

                if (defaultValue == null)
                {
                    if (value == null)
                        continue;

                    return false;
                }


                if (!defaultValue.Equals(value)) return false;
            }

            return true;
        }
    }
}
