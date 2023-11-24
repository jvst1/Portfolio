import colors from "vuetify/es5/util/colors";
var coresPadrao = {
  primary: colors.blue.base,
  secondary: colors.grey.lighten2,
  tertiary: colors.green.base,
  info: colors.blue,
  edit: colors.green.darken1,
  remove: colors.red.darken2,
  warning: colors.orange.darken3,
  danger: colors.red.darken2,
  success: colors.green.darken1,
  default: colors.grey.darken1,
  dark: colors.grey.darken3,
  white: colors.white,
  "custom-toolbar": "#FFFFFF",
  "custom-footer": "#FFFFFF",
  menu: colors.grey.lighten3,
  breadcrumb: colors.grey.darken3,
  "breadcrumb-disabled": colors.grey.darken1,
  "page-background": colors.grey.lighten5,
  "content-background": "#FFFFFF",
  icon: colors.blue.darken3,
  "grid-btn": "#FFFFFF",
  "grid-add": colors.blue.darken3,
};

export default {
  dark: false,
  options: {
    customProperties: true,
  },
  themes: {
    light: coresPadrao,
    dark: coresPadrao,
  },
};
