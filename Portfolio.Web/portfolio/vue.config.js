const path = require("path");

// Não exibir warnings do hot module no console do chrome em desenvolvimento
const webpack = require("webpack");
const compiler = webpack({});
require("webpack-hot-middleware")(compiler, {
  quiet: process.env.NODE_ENV === "development",
  log: process.env.NODE_ENV === "development",
});

module.exports = {
  transpileDependencies: [
    /\bvue-echarts\b/,
    /\bresize-detector\b/,
    /\bvue-c3\b/,
    /\bvue-masonry\b/,
    /\bvue-cropper\b/,
    /\bvuedraggable\b/,
    "vuetify",
  ],
  chainWebpack: (config) => {
    // aspnet uses the other hmr so remove this one (problema do asp.net q não atualiza sozinho)
    // see https://github.com/webpack/webpack/issues/1583
    config.plugins.delete("hmr");

    config.resolve.alias.set(
      "node_modules",
      path.join(__dirname, "./node_modules")
    );

    // Disable "prefetch" plugin since it's not properly working in some browsers
    config.plugins.delete("prefetch");

    // Do not remove whitespaces
    config.module
      .rule("vue")
      .use("vue-loader")
      .loader("vue-loader")
      .tap((options) => {
        options.compilerOptions.preserveWhitespace = true;
        return options;
      });
  },
  configureWebpack: {
    devServer: {
      client: {
        logging: "info",
        overlay: true,
        progress: true,
      },
      static: {
        watch: true,
      },
    },
  },
};
