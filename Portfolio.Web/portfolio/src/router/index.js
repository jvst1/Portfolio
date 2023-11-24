/**
 * Vue Router
 *
 * @library
 *
 * https://router.vuejs.org/en/
 */

// Lib imports
import Vue from "vue";
import Router from "vue-router";
import Meta from "vue-meta";
import globals from "@/plugins/globals";

// Routes
import paths from "./paths";
import api from "@/api";

const localApp = new Vue({});

function route(paths) {
  var list = [];
  for (var i = 0; i < paths.length; i++) {
    list.push(getPath(paths[i]));
  }
  list.push({ path: "*", redirect: "/" });
  return list;
}

function getPath(path) {
  if (!path.breadcrumbs) {
    path.breadcrumbs = [{ text: path.title, icon: path.icon }];
  }

  var item = {
    name: path.view,
    path: path.path,
    meta: path.meta,
    title: path.title,
    icon: path.icon,
    menu: path.menu,
    children: getChildren(path),
    component: view(path),
  };

  item.meta.title = path.title;
  item.meta.breadcrumbs = path.breadcrumbs;

  return item;
}

function view(path) {
  if (path.view) {
    return function (resolve) {
      require([`@/${path.dir}${path.view}.vue`], resolve);
    };
  } else {
    return function (resolve) {
      require(["@/layouts/IndexLayout.vue"], resolve);
    };
  }
}

function getChildren(path) {
  if (path.items) {
    var list = [];
    for (var i = 0; i < path.items.length; i++) {
      var item = path.items[i];

      if (
        path.items[i].dir &&
        path.items[i].items &&
        path.items[i].items.length > 0
      ) {
        path.items[i].items.forEach(function (p) {
          return (p.parentDir = path.items[i].dir);
        });
      }
      if (path.items[i].parentDir) {
        path.items[i].dir = path.dir + path.items[i].parentDir;
      } else {
        path.items[i].dir = path.dir;
      }

      path.items[i].path = path.path + path.items[i].path;

      item.breadcrumbs = [];
      if (path.breadcrumbs) {
        path.breadcrumbs.forEach((p) => item.breadcrumbs.push(p));
      } else {
        item.breadcrumbs.push({ text: path.title, icon: path.icon });
      }
      item.breadcrumbs.push({ text: item.title, icon: item.icon });

      list.push(getPath(path.items[i]));
    }
    return list;
  }
  return null;
}

function userHasPermission(path) {
  const userRoles = [];
  const jsonRoles = JSON.parse(localStorage.getItem("user")).roles;

  if (!Array.isArray(jsonRoles)) {
    userRoles.push(api.Enums.TipoPerfilUsuario[jsonRoles]);
  } else {
    jsonRoles.forEach((e) => {
      userRoles.push(api.Enums.TipoPerfilUsuario[e]);
    });
  }

  if (path.meta.allRolesRequired) {
    return path.meta.roles.every((r) => userRoles.indexOf(r) >= 0);
  } else {
    return path.meta.roles.some((r) => userRoles.indexOf(r) >= 0);
  }
}

Vue.use(Router);

// Create a new router
const router = new Router({
  mode: "history",
  routes: route(paths),
  scrollBehavior(to, from, savedPosition) {
    if (savedPosition) {
      return savedPosition;
    }
    if (to.hash) {
      return { selector: to.hash };
    }
    return { x: 0, y: 0 };
  },
});

Vue.use(Meta);

// eslint-disable-next-line
router.afterEach((to, from) => {
  // Remove initial splash screen
  const splashScreen = document.querySelector(".app-splash-screen");
  if (splashScreen) {
    splashScreen.style.opacity = 0;
    setTimeout(
      () => splashScreen && splashScreen.parentNode.removeChild(splashScreen),
      300
    );
  }

  // On small screens collapse sidenav
  if (
    window.layoutHelpers &&
    window.layoutHelpers.isSmallScreen() &&
    !window.layoutHelpers.isCollapsed()
  ) {
    setTimeout(() => window.layoutHelpers.setCollapsed(true, true), 10);
  }

  // Scroll to top of the page
  globals().scrollTop(0, 0);
});

router.beforeEach((to, from, next) => {
  // Set loading state
  document.body.classList.add("app-loading");

  if (["/", "/lembrar", "/recuperar"].indexOf(to.path) < 0) {
    const user = localStorage.getItem("user");
    if (!user) {
      localApp.$api.UI.ShowError("Erro", "Não autenticado");
      return next({ path: "/" });
    } else {
      const dataExpiracao = new Date(JSON.parse(user).expiresAt);
      const dataAtual = new Date();

      if (dataAtual > dataExpiracao || !userHasPermission(to)) {
        localApp.$api.UI.ShowError(
          "Erro",
          "Sessão expirada ou acesso não autorizado "
        );
        localApp.$store.commit("CLEAR_USER_DATA");
        return next({ path: "/" });
      }
    }
  }

  // Add tiny timeout to finish page transition
  setTimeout(() => next(), 10);
});

router.userHasPermission = userHasPermission;

export default router;
