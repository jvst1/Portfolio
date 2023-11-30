using AutoMapper;
using IdentityServer4.EntityFramework.Entities;
using Portfolio.Domain.Entities.Cad;
using Portfolio.Domain.Entities.Order;
using Portfolio.Domain.Messaging.Cad;
using Portfolio.Domain.Messaging.Identity;
using Portfolio.Domain.Messaging.Order;
using System;

namespace Portfolio.Crosscutting.Mapping
{
    public static class AutoMapperConfig
    {
        public static Action<IMapperConfigurationExpression> GetAllMappings()
        {
            return cfg =>
            {
                #region Usuario 

                cfg.CreateMap<Usuario, UsuarioRequest>(MemberList.None).ReverseMap();
                cfg.CreateMap<Usuario, UsuarioComercianteRequest>(MemberList.None).ReverseMap();
                cfg.CreateMap<Usuario, UsuarioResponse>(MemberList.None).ReverseMap();

                cfg.CreateMap<UsuarioEndereco, UsuarioEnderecoPostRequest>(MemberList.None).ReverseMap();
                cfg.CreateMap<UsuarioEndereco, UsuarioEnderecoPutRequest>(MemberList.None).ReverseMap();
                cfg.CreateMap<UsuarioEndereco, UsuarioEnderecoResponse>(MemberList.None).ReverseMap();

                #endregion

                #region Categoria

                cfg.CreateMap<Categoria, CategoriaPostRequest>(MemberList.None).ReverseMap();
                cfg.CreateMap<Categoria, CategoriaPutRequest>(MemberList.None).ReverseMap();
                cfg.CreateMap<Categoria, CategoriaResponse>(MemberList.None).ReverseMap();

                #endregion

                #region Cardapio

                cfg.CreateMap<Cardapio, CardapioPostRequest>(MemberList.None).ReverseMap();
                cfg.CreateMap<Cardapio, CardapioPutRequest>(MemberList.None).ReverseMap();
                cfg.CreateMap<Cardapio, CardapioResponse>(MemberList.None).ReverseMap();

                #endregion

                #region Pedido

                cfg.CreateMap<Pedido, PedidoResponse>(MemberList.None).ReverseMap();

                #endregion

                #region Identity

                cfg.CreateMap<ClientRequest, Client>(MemberList.None).ReverseMap();
                cfg.CreateMap<Client, ClientResponse>(MemberList.None).ReverseMap();

                cfg.CreateMap<ClientSecretRequest, ClientSecret>(MemberList.None).ReverseMap();
                cfg.CreateMap<ClientSecret, ClientSecretResponse>(MemberList.None).ReverseMap();

                cfg.CreateMap<ClientScopesRequest, ClientScope>(MemberList.None).ReverseMap();
                cfg.CreateMap<ClientScope, ClientScopesResponse>(MemberList.None).ReverseMap();

                cfg.CreateMap<ApiResourceRequest, ApiResource>(MemberList.None).ReverseMap();
                cfg.CreateMap<ApiResource, ApiResourceResponse>(MemberList.None).ReverseMap();

                cfg.CreateMap<ApiScopeRequest, ApiScope>(MemberList.None).ReverseMap();
                cfg.CreateMap<ApiScope, ApiScopeResponse>(MemberList.None).ReverseMap();

                cfg.CreateMap<ApiResourceSecretsRequest, ApiResourceSecret>(MemberList.None).ReverseMap();
                cfg.CreateMap<ApiResourceSecret, ApiResourceSecretsResponse>(MemberList.None).ReverseMap();

                #endregion
            };
        }
    }
}
