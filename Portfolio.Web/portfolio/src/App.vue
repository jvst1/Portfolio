<template>
  <div id="app">
    <!--Aqui irá o sitemap-->
    <component v-bind:is="layout">
    </component>

    <core-mensagem :tipo="msg.tipo" :titulo="msg.titulo" :mensagem="msg.mensagem" :ok="msg.ok"
      :show="msg.show"></core-mensagem>
    <core-confirmacao :tipo="conf.tipo" :titulo="conf.titulo" :mensagem="conf.mensagem" :sim="conf.sim" :nao="conf.nao"
      :show="conf.show"></core-confirmacao>

    <notifications group="notifications-bottom-right" position="bottom right" classes="custom-notification" />

    <BlockUI v-if="loading" id="ui-loader-1234">
      <v-progress-circular indeterminate size="64"></v-progress-circular>
    </BlockUI>
  </div>
</template>

<script>
// layout em branco da página index
import IndexLayout from './layouts/IndexLayout'

// layout do sistema
import AppLayout from './layouts/AppLayout'

export default {
  metaInfo: {
    title: 'Portfolio',
    titleTemplate: '%s - Tomio'
  },
  name: 'App',
  data() {
    return {
      isLoaded: false
    }
  },
  mounted() {
    this.verificar()
    var vm = this

    vm.$store.commit('isMobile')
    if (!vm.$store.getters.isMobile && !vm.$store.getters.drawer) { vm.$store.commit('drawer') }

    window.addEventListener('resize', function () {
      vm.$store.commit('isMobile')
    })
  },
  methods: {
    verificar: function () {
      var vm = this
      setTimeout(function () {
        if (!vm.loading) { vm.isLoaded = true } else { vm.verificar() }
      }, 10)
    }
  },
  computed: {
    msg() {
      return this.$store.getters.mensagem
    },
    conf() {
      return this.$store.getters.confirmacao
    },
    loading() {
      return this.$store.getters.loading || this.$store.getters.activeRequests > 0
    },
    layout() {
      this.$store.commit('SET_LAYOUT', IndexLayout)

      // verifica se não é a tela de login, se for, mantém o layout em branco
      if (['/', '/lembrar', '/recuperar'].indexOf(this.$route.path) < 0) { this.$store.commit('SET_LAYOUT', AppLayout) }
      return this.$store.getters.layout
    }
  }
}
</script>
<style lang="scss">
@import '@/assets/styles/variables.scss';

:root {
  --primary: #4E60FF;
  --primary-light: #F3F4FF;
  --secondary: #FD6D22;
  --secondary-light: #FFF3ED;
  --white: #fff;
  --dark: #2B2B43;
  --grey: #83859C;
  --grey-light: #C7C8D2;
  --grey-dark: #545563;
  --grey-lightest: #EDEEF2;
  --error: #FF5C60;
  --error-light: #ffdfe0;
  --yellow: #FFDC9F;
  --yellow-light: #ffecca;
  --yellow-dark: #FFA609;
  --green: #155724;
  --green-light: #d4edda;
}
</style>

<style>
html {
  overflow-y: auto;
}

.theme--light.v-text-field--outlined:not(.v-input--is-focused):not(.v-input--has-state)>.v-input__control>.v-input__slot fieldset {
  color: rgba(0, 0, 0, 0.21);
}

.theme--light.v-text-field--outlined:not(.v-input--is-focused):not(.v-input--has-state):not(.v-input--is-disabled)>.v-input__control>.v-input__slot:hover fieldset {
  color: rgba(0, 0, 0, 0.44);
}

.custom-notification {
  padding: 10px;
  margin: 0 5px 5px;
  font-size: 12px;
  color: #ffffff;
  background: #44A4FC;
  border-left: 5px solid #187FE7;
  font-family: "Roboto", sans-serif !important;
}

.custom-notification.warn {
  background: #ffb648;
  border-left-color: #f48a06;
}

.custom-notification.error {
  background: #E54D42;
  border-left-color: #B82E24;
}

.custom-notification.success {
  background: #68CD86;
  border-left-color: #42A85F;
}

.v-dialog__content {
  align-items: start;
}

#ui-loader-1234.loading-container .loading {
  background-color: transparent;
  border-radius: 0px;
  box-shadow: none;
}
</style>
