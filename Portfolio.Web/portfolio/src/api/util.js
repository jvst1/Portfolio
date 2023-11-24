import Axios from "axios";
import { store } from "../store";

const util = {
  Axios: {
    Get: function (controller, id) {
      store.commit("addRequest");

      if (id) {
        return Axios.get(
          process.env.VUE_APP_ROOT_API + controller + "/" + id
        ).then(HandleResponse, HandleError);
      } else {
        return Axios.get(process.env.VUE_APP_ROOT_API + controller).then(
          HandleResponse,
          HandleError
        );
      }
    },
    GetAll: function (controller, dto) {
      store.commit("addRequest");
      return Axios.get(process.env.VUE_APP_ROOT_API + controller, {
        params: {
          request: dto || {},
        },
      }).then(HandleResponse, HandleError);
    },
    GetBlob: function (controller, dto) {
      store.commit("addRequest");
      return Axios.get(process.env.VUE_APP_ROOT_API + controller, {
        params: { request: dto || {} },
        responseType: "blob", // Force to receive data in a Blob Format
      }).then(HandleResponse, HandleError);
    },
    Post: function (controller, dto) {
      store.commit("addRequest");
      return Axios.post(process.env.VUE_APP_ROOT_API + controller, dto).then(
        HandleResponse,
        HandleError
      );
    },
    PostForm: function (controller, dto) {
      store.commit("addRequest");
      return Axios.post(process.env.VUE_APP_ROOT_API + controller, dto, {
        headers: { "Content-Type": "multipart/form-data" },
      }).then(HandleResponse, HandleError);
    },
    Put: function (controller, id, dto) {
      store.commit("addRequest");
      if (!dto) {
        dto = {};
      }
      return Axios.put(process.env.VUE_APP_ROOT_API + controller + "/" + id, dto).then(HandleResponse, HandleError);
    },
    Persist: function (controller, dto, key) {
      if (!dto) {
        dto = {};
      }

      if (!key) {
        key = "codigo";
      }

      if (dto[key]) {
        return util.Axios.Put(controller, dto[key], dto);
      } else {
        return util.Axios.Post(controller, dto);
      }
    },
    Delete: function (controller, id) {
      store.commit("addRequest");
      return Axios.delete(
        process.env.VUE_APP_ROOT_API + controller + "/" + id
      ).then(HandleResponse, HandleError);
    },
    GetParams: function (controller, paramns) {
      store.commit("addRequest");
      if (!paramns) {
        paramns = {};
      }
      return Axios.get(process.env.VUE_APP_ROOT_API + controller, {
        params: paramns,
      }).then(HandleResponse, HandleError);
    },
  },
  Functions: {
    CopyObject: function (sender) {
      return JSON.parse(JSON.stringify(sender));
    },
    ValidaDocumento: function (documento) {
      return util.Axios.Get('Util/ValidaDocumento', documento)
    },
  },
  GetCrud: function (controller, methods) {
    var factory = {};
    if (!methods || (methods && methods.indexOf("get") >= 0)) {
      factory.Get = function (id) {
        return util.Axios.Get(controller, id);
      };
    }

    if (!methods || (methods && methods.indexOf("getall") >= 0)) {
      factory.GetAll = function (dto) {
        return util.Axios.GetAll(controller, dto);
      };
    }

    if (!methods || (methods && methods.indexOf("post") >= 0)) {
      factory.Post = function (dto) {
        return util.Axios.Post(controller, dto);
      };
    }

    if (!methods || (methods && methods.indexOf("put") >= 0)) {
      factory.Put = function (id, dto) {
        return util.Axios.Put(controller, id, dto);
      };
    }

    if (!methods || (methods && methods.indexOf("persist") >= 0)) {
      factory.Persist = function (dto, key) {
        return util.Axios.Persist(controller, dto, key);
      };
    }

    if (!methods || (methods && methods.indexOf("delete") >= 0)) {
      factory.Delete = function (id) {
        return util.Axios.Delete(controller, id);
      };
    }
    // utilizado na montagem da api index.js
    factory.controller = controller;

    return factory;
  },
};

function HandleResponse(response) {
  store.commit("removeRequest");
  return response.data;
}

function HandleError(error) {
  store.commit("removeRequest");
  if (error) {
    if (error.response) {
      return error.response.data;
    }
    return error;
  }
  return { error: "Erro" };
}

export default util;
