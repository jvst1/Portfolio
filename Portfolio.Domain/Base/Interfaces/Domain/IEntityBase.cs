namespace Portfolio.Domain.Base.Interfaces.Domain
{
    public interface IEntityBase
    {
        void ThrowExceptionIfUpdateInvalid();
        void ThrowExceptionIfInsertInvalid();
    }
}
