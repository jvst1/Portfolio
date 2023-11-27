import api from "@/api";
const TipoPerfilUsuario = api.Enums.TipoPerfilUsuario;

export default [
  {
    title: "Home",
    icon: "fas fa-home",
    path: "/home",
    view: "Home",
    dir: "views/app/",
    menu: true,
    meta: {
      roles: [TipoPerfilUsuario.Nenhum],
      allRolesRequired: false,
    },
  },
  {
    title: "Configurações",
    icon: "fas fa-cog",
    path: "/cliente/settings",
    view: "Cliente",
    dir: "views/app/profile/",
    menu: true,
    meta: {
      roles: [TipoPerfilUsuario.Cliente],
      allRolesRequired: false,
    },
  },
  {
    title: "Configurações",
    icon: "fas fa-cog",
    path: "/comerciante/settings",
    view: "Comerciante",
    dir: "views/app/profile/",
    menu: true,
    meta: {
      roles: [TipoPerfilUsuario.Comerciante],
      allRolesRequired: false,
    },
  },
  {
    title: "Admin",
    icon: "fas fa-user",
    path: "/admin/",
    dir: "views/app/admin/",
    menu: true,
    meta: {
      roles: [TipoPerfilUsuario.Administrador],
      allRolesRequired: true,
    },
    items: [
      {
        title: "Cadastros",
        path: "cadastro/",
        dir: "cadastro/",
        menu: true,
        meta: {
          roles: [TipoPerfilUsuario.Administrador],
          allRolesRequired: true,
        },
        items: [
          {
            path: "categoria",
            view: "Categoria",
            title: "Categoria",
            menu: true,
            meta: {
              roles: [TipoPerfilUsuario.Administrador],
              allRolesRequired: true,
            },
          },
          {
            path: "usuario",
            view: "Usuario",
            title: "Usuario",
            menu: true,
            meta: {
              roles: [TipoPerfilUsuario.Administrador],
              allRolesRequired: true,
            },
          },
        ],
      },
      {
        title: "Segurança",
        path: "seguranca/",
        dir: "seguranca/",
        menu: true,
        meta: {
          roles: [TipoPerfilUsuario.Administrador],
          allRolesRequired: true,
        },
        items: [
          {
            path: "apiresource",
            view: "ApiResource",
            title: "Recursos",
            menu: true,
            meta: {
              roles: [TipoPerfilUsuario.Administrador],
              allRolesRequired: true,
            },
          },
          {
            path: "clients",
            view: "Client",
            title: "Clientes API",
            menu: true,
            meta: {
              roles: [TipoPerfilUsuario.Administrador],
              allRolesRequired: true,
            },
          },
        ],
      },
    ],
  },
  {
    path: "/",
    view: "Login",
    dir: "views/",
    title: "Login",
    menu: false,
    meta: {
      roles: TipoPerfilUsuario.Nenhum,
      allRolesRequired: true,
    },
  },
  {
    path: "/lembrar",
    view: "Lembrar",
    dir: "views/",
    title: "Lembrar",
    menu: false,
    meta: {
      roles: TipoPerfilUsuario.Nenhum,
      allRolesRequired: true,
    },
  },
  {
    path: "/recuperar",
    view: "Recuperar",
    dir: "views/",
    title: "Recuperar",
    params: true,
    menu: false,
    meta: {
      roles: TipoPerfilUsuario.Nenhum,
      allRolesRequired: true,
    },
  },
];
