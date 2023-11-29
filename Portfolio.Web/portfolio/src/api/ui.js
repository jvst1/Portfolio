/* eslint-disable */
// Components
import Vue from "vue";
import { store } from "../store";
import { format, parseISO, startOfMonth, lastDayOfMonth } from "date-fns";
import pt from "date-fns/locale/pt-BR";
import Msg from '../components/core/mensagem'
import Conf from '../components/core/confirmacao'

const localApp = new Vue({});
localApp.store = store;

const ui = {
  Confirmacao: function (tipo, mensagem, callbackSim, callbackNao, titulo, sim, nao) {
    const vuetify = require('../plugins/vuetify').default
    const componentInstance = new Vue({
      ...Conf,
      propsData: {
        tipo: tipo,
        titulo: titulo,
        mensagem: mensagem,
        sim: sim,
        nao: nao,
        callbackSim: callbackSim,
        callbackNao: callbackNao
      },
      vuetify
    })
    componentInstance.$mount()
  },
  Mensagem: function (tipo, mensagem, callback, titulo, ok) {
    const vuetify = require('../plugins/vuetify').default
    const componentInstance = new Vue({
      ...Msg,
      propsData: {
        tipo: tipo,
        titulo: titulo,
        mensagem: mensagem,
        ok: ok,
        callback: callback
      },
      vuetify
    })
    componentInstance.$mount()
  },
  Close: function (retorno) {
    localApp.store.getters.mensagem.retorno = retorno;
    localApp.store.commit("closeMessage");
  },
  Confirm: function () {
    localApp.store.getters.confirmacao.retorno = true;
    localApp.store.commit("close");
  },
  Recuse: function () {
    localApp.store.getters.confirmacao.retorno = false;
    localApp.store.commit("close");
  },
  ShowError: function (title, message) {
    localApp.$notify({
      group: "notifications-bottom-right",
      type: "error",
      title: title || "Erro",
      text: message.replace(/\n/g, "<br/>"),
    });
  },
  ShowAlert: function (title, message) {
    localApp.$notify({
      group: "notifications-bottom-right",
      type: "warn",
      title: title,
      text: message.replace(/\n/g, "<br/>"),
    });
  },
  ShowSuccess: function (title, message) {
    localApp.$notify({
      group: "notifications-bottom-right",
      type: "success",
      title: title || "Sucesso",
      text: message.replace(/\n/g, "<br/>"),
    });
  },
  FormValidate: function (th) {
    var form = null;
    if (th && th.$refs && th.$refs.form) {
      form = th.$refs.form;
    }

    if (form && form.validate()) {
      return true;
    } else {
      var msg = [];
      const invalidFields = form.$el.querySelectorAll("[data-invalid=true]");
      invalidFields.forEach(function (p) {
        msg.push(p.getAttribute("data-message"));
      });
      ui.ShowError("Formulário Inválido", msg.join("<br/>"));
      return false;
    }
  },
  PostAction: function (response, mensagem) {
    if (response.error) {
      ui.ShowError("Erro", response.error);
      return false;
    } else if (response.errors && response.errors.length > 0) {
      ui.ShowError("Erro", response.errors[0]);
      return false;
    } else if (response.errors && !response.errors.length) {
      ui.ShowError("Erro", JSON.stringify(response.errors));
      return false;
    } else if (!response.successo && response.mensagem) {
      ui.ShowError("Erro", JSON.stringify(response.mensagem));
      return false;
    } else {
      ui.ShowSuccess("Sucesso", mensagem || "Operação Realizada com Sucesso!");
      return true;
    }
  },
  PostLoadData: function (response) {
    if (response.error) {
      return {
        success: false,
        message: response.error.replace(/\n/g, "<br/>"),
      };
    } else if (response.errors && response.errors.length > 0) {
      return {
        success: false,
        message: response.errors[0].replace(/\n/g, "<br/>"),
      };
    } else if (response.errors && !response.errors.length) {
      return {
        success: false,
        message: JSON.stringify(response.errors).replace(/\n/g, "<br/>"),
      };
    } else {
      return { success: true, message: null };
    }
  },
  ShowLoading: function () {
    localApp.store.commit("loading", true);
  },
  HideLoading: function () {
    localApp.store.commit("loading", false);
  },
  Redirect: function (that, path, data) {
    that.$router.push({
      path: path,
      query: { data: JSON.stringify(data) },
    });
  },
  GetRequest: function (that) {
    return JSON.parse(that.$route.query.data);
  },
  Format: {
    Date: function (value) {
      if (value) {
        const date = format(parseISO(value), "dd/MM/yyyy", { locale: pt });
        return date;
      }
      return "";
    },
    DateTime: function (value) {
      if (value) {
        const date = format(parseISO(value), "dd/MM/yyyy HH:mm:ss", {
          locale: pt,
        });
        return date;
      }
      return "";
    },
    Currency: function (value) {
      const val = (value / 1).toFixed(2).replace(".", ",");
      return "R$ " + val.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
    },
    Decimal: function(value) {
      if (value) {
        const formatter = new Intl.NumberFormat('pt-BR', {
          style: 'decimal',
          minimumFractionDigits: 2,
          maximumFractionDigits: 2
        });
        return formatter.format(value);
      }
      return "";
    },
    DocumentoFederal: function (value) {
      if (value) {
        value = value.toString().replace(/[^0-9]/g, "");
        if (value.length === 11) {
          const regex = /([0-9]{3})([0-9]{3})([0-9]{3})([0-9]{2})/;
          const subst = "$1.$2.$3-$4";
          return value.replace(regex, subst);
        } else if (value.length === 14) {
          const regex = /([0-9]{2})([0-9]{3})([0-9]{3})([0-9]{4})([0-9]{2})/;
          const subst = "$1.$2.$3/$4-$5";
          return value.replace(regex, subst);
        }
      }
      return "";
    },
    Celular: function (value) {
      if (value) {
        value = value.toString().replace(/[^0-9]/g, "");
        const regex = /([0-9]{2})([0-9]{4})([0-9]{1})?([0-9]{4})/;
        const subst = '\($1\) $2$3-$4'    // eslint-disable-line

        return value.replace(regex, subst);
      }
      return "";
    },
    Telefone: function (value) {
      if (value) {
        value = value.toString().replace(/[^0-9]/g, "");
        const regex = /([0-9]{2})([0-9]{4})([0-9]{1})?([0-9]{4})/;
        const subst = '\($1\) $2$3-$4'   // eslint-disable-line

        return value.replace(regex, subst);
      }
      return "";
    },
    SomenteNumeros: function (value) {
      if (value) {
        return value.replace(/[^0-9]/g, "");
      }
      return "";
    },
    LinhaDigitavel: function (value) {
      if (value) {
        value = value.toString().replace(/[^0-9]/g, "");
        const regex =
          /([0-9]{5})([0-9]{5})([0-9]{5})([0-9]{6})([0-9]{5})([0-9]{6})([0-9]{1})([0-9]{14})/;
        const subst = "$1.$2.$3.$4.$5.$6.$7.$8";

        return value.replace(regex, subst);
      }
      return "";
      // "'#####.#####.#####.######.#####.######.#.##############'"
    },
    LeftPadNumbers: function (value, totalWidth, paddingChar) {
      var length = totalWidth - value.toString().length + 1;
      return Array(length).join(paddingChar || "0") + value;
    },
    Cep: function (value) {
      if (value) {
        value = value.toString().replace(/[^0-9]/g, "");
        if (value.length === 8) {
          const regex = /([0-9]{2})([0-9]{3})([0-9]{3})/;
          const subst = "$1.$2-$3";
          return value.replace(regex, subst);
        }
      }
      return "";
    },
  },
  Date: {
    DataAtual: function () {
      return ui.Format.Date(new Date());
    },
    PrimeiroDiaMes: function (date) {
      return startOfMonth(date);
    },
    UltimoDiaMes: function (date) {
      return lastDayOfMonth(date);
    },
    AddDay: function (date, dias) {
      if (!dias) {
        dias = 0;
      }
      dias = date.getDate() + dias;
      date.setDate(dias);
      return date;
    },
    AddMonth: function (date, meses) {
      if (!meses) {
        meses = 0;
      }
      meses = date.getMonth() + meses;
      date.setMonth(meses);
      return date;
    },
    AddYear: function (date, anos) {
      if (!anos) {
        anos = 0;
      }
      anos = date.getFullYear() + anos;
      date.setYear(anos);
      return date;
    },
  },
};

export default ui;
