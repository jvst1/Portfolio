﻿using AutoMapper;
using Portfolio.Domain.Base.Interfaces.Data;

namespace Portfolio.Crosscutting.Mapping
{
    public class AutoMapperBase<TDomain> : IMappingService<TDomain>
    {
        private readonly IMapper _maper;
        public AutoMapperBase(IMapper mapper)
        {
            _maper = mapper;
        }

        public void UpdateDomain<T>(TDomain destination, T source)
        {
            _maper.Map(source, destination);
        }

        public TDomain ConvertToDomain<T>(T type)
        {
            return _maper.Map<T, TDomain>(type);
        }

        public List<TDomain> ConvertToDomain<T>(List<T> types)
        {
            return _maper.Map<List<T>, List<TDomain>>(types);
        }

        public T ConvertFromDomain<T>(TDomain domain)
        {
            return _maper.Map<TDomain, T>(domain);
        }

        public List<T> ConvertFromDomain<T>(List<TDomain> domains)
        {
            return _maper.Map<List<TDomain>, List<T>>(domains);
        }

        public TOther ConvertOther<T, TOther>(T domain)
        {
            return _maper.Map<T, TOther>(domain);
        }

        public List<TOther> ConvertOther<T, TOther>(List<T> domain)
        {
            return _maper.Map<List<T>, List<TOther>>(domain);
        }
    }
}