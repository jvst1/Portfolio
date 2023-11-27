import Vue from "vue";
import Vuex from "vuex";
import axios from "axios";

Vue.use(Vuex);

export const store = new Vuex.Store({
  state: {
    layout: "index-layout",
    isBlocked: false,
    isMobile: false,
    activeRequests: 0,
    drawer: false,
    mensagem: {},
    confirmacao: {},
    carrinho: { codigoComerciante: '', items: [], count: 0 },
    loading: false,
  },
  mutations: {
    isMobile(state) {
      state.isMobile = window.innerWidth < 960;
    },
    drawer(state) {
      state.drawer = !state.drawer;
    },
    addRequest(state) {
      state.activeRequests += 1;
    },
    removeRequest(state) {
      state.activeRequests -= 1;
      if (state.activeRequests < 0) {
        state.activeRequests = 0;
      }
    },
    user(state, user) {
      state.user = user
      localStorage.setItem('user', JSON.stringify(user))
    },
    SET_USER_DATA(state, data) {
      if (!data.user) { data.user = '' }

      state.user = data.user;
      localStorage.setItem("user", JSON.stringify(data.user));
      localStorage.setItem("status", "loggedIn");
      localStorage.setItem("theme", JSON.stringify(data.theme));
      axios.defaults.headers.common.Authorization = `Bearer ${data.user.token}`;
    },
    CLEAR_USER_DATA() {
      localStorage.clear()
      localStorage.setItem('status', 'loggedOut')
      location.reload()
    },
    SET_LAYOUT(state, payload) {
      state.layout = payload;
    },
    confirmacao(state, obj) {
      state.confirmacao = obj;
    },
    mensagem(state, obj) {
      state.mensagem = obj;
    },
    close(state) {
      state.confirmacao.show = false;
    },
    closeMessage(state) {
      state.mensagem.show = false;
    },
    loading(state, obj) {
      state.loading = obj;
    },
    ADD_TO_CART(state, { dto }) {
      if (state.carrinho.codigoComerciante && state.carrinho.codigoComerciante != dto.codigoComerciante) {
        this.$api.UI.ShowError("ERRO!", "Houve uma tentativa de adicionar produtos de estabelecimentos comerciais diferentes ao mesmo tempo no carrinho. Por favor escolha entre eles para continuar com o pedido.")
      } else {
        const record = state.carrinho.items.find(item => item.codigo === dto.added.codigo);

        if (!record) {
          state.carrinho.codigoComerciante = dto.codigoComerciante;
          state.carrinho.items.push(dto.added);
        } else {
          record.quantidade = dto.added.quantidade;
          record.comentario = dto.added.comentario;
        }

        state.carrinho.count = state.carrinho.items.length;
      }
    }
  },
  actions: {
    setUserData({ commit }, data) {
      commit("SET_USER_DATA", data);
    },
    logout({ commit }) {
      commit("CLEAR_USER_DATA");
    },
    addToCart({ commit }, dto) {
      commit("ADD_TO_CART", dto)
    }
  },
  getters: {
    isMobile(state) {
      return state.isMobile;
    },
    drawer(state) {
      return state.drawer;
    },
    activeRequests(state) {
      return state.activeRequests;
    },
    layout(state) {
      return state.layout;
    },
    loggedIn(state) {
      return !!state.user;
    },
    confirmacao(state) {
      return state.confirmacao;
    },
    mensagem(state) {
      return state.mensagem;
    },
    loading(state) {
      return state.loading;
    },
    user(state) {
      if (!state.user) {
        if (localStorage.getItem('user')) {
          state.user = JSON.parse(localStorage.getItem('user'))
        }
      }

      if (!state.user) { state.user = {} }

      if (!state.user.profile) { state.user.profile = {} }

      return state.user
    },
    carrinho(state) {
      if (!state.carrinho)
        state.carrinho = { codigoComerciante: '', items: [], count: 0 };

      return state.carrinho;
    },
  },
});
