import Vue from "vue";
import Vuetify from "vuetify/lib";

import "material-design-icons-iconfont/dist/material-design-icons.css"; // Ensure you are using css-loader
import "@mdi/font/css/materialdesignicons.css"; // Ensure you are using css-loader
import "@fortawesome/fontawesome-free/css/all.css"; // Ensure you are using css-loader
import theme from './theme'

import pt from "vuetify/es5/locale/pt";
import en from "vuetify/es5/locale/en";

Vuetify.config.silent = true;
Vue.use(Vuetify);

if (!theme.options.drawer) {
  theme.options.drawer = {}
}
if (!theme.options.compact) {
  theme.options.compact = {}
}
if (!theme.options.grid) {
  theme.options.grid = { pageSize: [5, 10, 15, 20, -1] }
}

export default new Vuetify({
  theme: theme,
  lang: {
    locales: { pt, en },
    current: "pt",
  },
  icons: {
    iconfont: "mdi",
  },
});
