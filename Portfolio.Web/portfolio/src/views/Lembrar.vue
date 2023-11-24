<template>
  <div class="authentication-wrapper authentication-2 ui-bg-cover ui-bg-overlay-container px-4"
    :style="`background-image: url('${publicUrl}assets/img/loginbg.jpg');`">
    <div class="ui-bg-overlay bg-dark opacity-25"></div>

    <div class="authentication-inner py-5">
      <v-card no-body>
        <div class="p-4 p-sm-5">
          <!-- Logo -->
          <div class="d-flex justify-content-center align-items-center pb-2 mb-4">
            <img src="@/assets/logo.png" width="100" />
          </div>
          <!-- / Logo -->

          <h5 class="text-center text-muted font-weight-normal mb-4">Portfolio - Recuperação de senha</h5>

          <!-- Form -->
          <form v-show="!msgRecuperacaoSuccess" @submit.prevent="recuperarSenha">
            <v-col label="E-mail">
              <input-text v-model="model.Login" @keypress="clearMsgs" />
            </v-col>
            <v-col>
              <div slot="label" class="d-flex justify-content-between align-items-start">
                <div class="d-flex align-content-center">
                  <v-btn type="submit" variant="primary" :disabled="!model.Login">Enviar Recuperação de Senha</v-btn>
                </div>
                <router-link to="/" class="d-block small">Voltar ao login</router-link>
              </div>
            </v-col>
          </form>

          <div v-show="msgRecuperacaoError">
            <div class="alert alert-danger">{{ msgRecuperacaoError }}</div>
            <br />
          </div>

          <div v-show="msgRecuperacaoSuccess">
            <div class="alert alert-success">{{ msgRecuperacaoSuccess }}</div>
            <br />
            <router-link to="/" class="d-block small">Voltar ao login</router-link>
          </div>

          <!-- / Form -->
        </div>
      </v-card>
    </div>
  </div>
</template>

<!--<style src="@/vendor/styles/pages/authentication.scss" lang="scss"></style>
<style src="@/vendor/libs/spinkit/spinkit.scss" lang="scss"></style>-->

<script>
export default {
  name: 'lembrar',
  metaInfo: {
    title: 'Portfolio - Recuperação de senha'
  },
  data: () => ({
    model: { Login: '', PrimeiroAcesso: false },
    msgRecuperacaoError: '',
    msgRecuperacaoSuccess: ''
  }),
  methods: {
    recuperarSenha() {
      const th = this
      th.$api.UI.ShowLoading()
      th.$api.Usuario.SolicitarLinkSenha(th.model)
        .then(function (response) {
          th.$api.UI.HideLoading()

          if (!response) {
            th.msgRecuperacaoSuccess =
              'Email com link para recuperação de senha enviado para: ' +
              th.model.Login
          } else th.msgRecuperacaoError = response.message
        })
        .catch(err => console.log(err))
    },
    clearMsgs() {
      this.msgRecuperacaoError = ''
      this.msgRecuperacaoSuccess = ''
    }
  }
}
</script>

<style></style>
