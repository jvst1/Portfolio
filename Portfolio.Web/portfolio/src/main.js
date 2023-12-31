// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.

import "babel-polyfill";

import Vue from "vue";
import VueTheMask, { TheMask } from "vue-the-mask";

// Components
import "./components";

import api from "@/api";
import { store } from "./store";
import globals from "@/plugins/globals";
import axios from "axios";
import vuetify from "./plugins/vuetify";
import VueLodash from "vue-lodash";
import lodash from "lodash";
import Notifications from "vue-notification";
import VueFileAgent from "vue-file-agent";
import BlockUI from "vue-blockui";
import { datadogRum } from '@datadog/browser-rum';

datadogRum.init({
    applicationId: '47b705d4-c0c2-447d-b462-6424b7db29a3',
    clientToken: 'pubd881b1e6ae8e789618c1e2046ded7e9b',
    site: 'datadoghq.com',
    service: 'portfolioweb',
    env: 'production',
    version: '1.0.0', 
    sessionSampleRate: 100,
    sessionReplaySampleRate: 100,
    trackUserInteractions: true,
    trackResources: true,
    trackLongTasks: true,
    defaultPrivacyLevel: 'mask-user-input',
});

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

if (themeString && themeString !== "undefined" && themeString !== "") {
  try {
    const parsedTheme = JSON.parse(themeString);
    Vue.prototype.$theme = parsedTheme;
  } catch (error) {
    console.error("Error parsing themeString:", error);
  }
}

Vue.use(VueLodash, { lodash: lodash });
Vue.use(Notifications);
Vue.use(VueFileAgent);
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
