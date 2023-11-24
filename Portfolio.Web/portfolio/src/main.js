// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.

import "babel-polyfill";

import Vue from "vue";
import VueTheMask, { TheMask } from "vue-the-mask";

// Components
import "./components";

// Plugins
import "./plugins/vuetify";

import api from "@/api";
import { store } from "./store";
import globals from "@/plugins/globals";
import axios from "axios";
import vuetify from "./plugins/vuetify";
import VueLodash from "vue-lodash";
import lodash from "lodash";
import Notifications from "vue-notification";
import VueFileAgent from "vue-file-agent";
import VueFileAgentStyles from "vue-file-agent/dist/vue-file-agent.css";
import BlockUI from "vue-blockui";

Vue.use(VueTheMask);
Vue.component(TheMask);

Vue.config.productionTip = false;
Vue.prototype.$api = api;
Vue.prototype.$store = store;
Vue.prototype.global = {
  GuidEmpty: "00000000-0000-0000-0000-000000000000",
};

// Padrão
Vue.prototype.$theme = {
  placeholder: " ",
  placeholderSelect: "Selecione",
  placeholderMultiSelect: "Nenhum",
  placeholderCombobox: "Digite ou Selecione",
  placeholderMultiCombobox: "Nenhum",
  outlined: true,
  dense: true,
};

const themeString = localStorage.getItem("theme");
console.log("themeString", themeString, typeof themeString);

if (themeString && themeString !== "undefined" && themeString !== "") {
  try {
    const parsedTheme = JSON.parse(themeString);
    Vue.prototype.$theme = parsedTheme;
  } catch (error) {
    console.error("Error parsing themeString:", error);
  }
} else {
  console.log("No valid themeString found in localStorage");
}

Vue.use(VueLodash, { lodash: lodash });
Vue.use(Notifications);
Vue.use(VueFileAgent);
Vue.use(VueFileAgentStyles);
Vue.use(BlockUI);

Vue.mixin({
  data: globals,
  computed: {
    $user: {
      get() {
        return store.getters.user
      }
    }
  }
});

// Carregar Arrays e Enums
api.Enum.GetAll().then(function (response) {
  var items = Object.keys(response);
  if (!api.Enums)
    api.Enums = {};
  
  items.forEach(function (p) {
    api.Enums[p] = {};
    response[p].forEach(function (o) {
      api.Enums[p][o.name] = o.value;
    });
  });

  // variáveis que dependem do carregamento completo da API
  const router = require("@/router").default;
  const App = () => import("./App");

  new Vue({
    router,
    axios,
    vuetify,
    render: (h) => h(App),
    created() {
      const userString = localStorage.getItem("user");
      if (userString) {
        const userData = JSON.parse(userString);

        let themeData = {};
        if (this.$theme) {
          themeData = this.$theme;
        }

        this.$store.commit("SET_USER_DATA", {
          user: userData,
          theme: themeData,
        });
      }
    },
  }).$mount("#app");
});
