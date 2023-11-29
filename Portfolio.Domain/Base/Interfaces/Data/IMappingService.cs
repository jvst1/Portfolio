namespace Portfolio.Domain.Base.Interfaces.Data
{
    public interface IMappingService<TDomain>
    {
        void UpdateDomain<T>(TDomain destination, T source);
        TDomain ConvertToDomain<T>(T type);
        List<TDomain> ConvertToDomain<T>(List<T> types);
        T ConvertFromDomain<T>(TDomain domain);
        List<T> ConvertFromDomain<T>(List<TDomain> domains);
        TOther ConvertOther<T, TOther>(T domain);
        List<TOther> ConvertOther<T, TOther>(List<T> domain);
    }
}
