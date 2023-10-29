namespace Portfolio.Domain.Base.Interfaces.Domain
{
    public interface IEntity : IEntityBase
    {
        Guid Codigo { get; set; }
    }
}
