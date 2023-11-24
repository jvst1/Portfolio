const api = {};
const requireFiles = require.context("@/api", true, /\.js$/);

requireFiles.keys().forEach((fileName) => {

  let libName = fileName.replace(/^\.\//, "").replace(/\.\w+$/, "");
  if (libName !== "index") {
    const lib = requireFiles(fileName).default;
    let parts = libName.split("/");
    libName = parts[parts.length - 1];

    switch (libName) {
      case "util":
        libName = "UTIL";
        break;
      case "ui":
        libName = "UI";
        break;
      case "enums":
        libName = "Enums";
        break;
      case 'cep': libName = 'CEP'
        break
    }

    if (lib.controller) {
      libName = lib.controller;
      delete lib.controller;
    }

    // import libName from fileName
    api[libName] = lib;
  }
});

export default api;
