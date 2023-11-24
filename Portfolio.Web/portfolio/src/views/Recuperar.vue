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

          <h5 class="text-center text-muted font-weight-normal mb-4">Portfolio - Cadastrar Senha</h5>

          <!-- Form -->
          <form v-show="!msgResetSuccess && !msgResetError && validToken" @submit.prevent="recuperarSenha">
            <v-col label="E-mail">
              <input-text v-model="model.Login" :disabled="true" />
            </v-col>
            <v-col>
              <input-text label="Nova Senha" type="password" v-model="model.NovaSenha" :disabled="!model.Login" />
            </v-col>
            <v-col>
              <input-text label="Confirmar Senha" type="password" v-model="confirmacaoSenha" :disabled="!model.Login" />
            </v-col>
            <p v-show="model.NovaSenha && confirmacaoSenha && (model.NovaSenha != confirmacaoSenha)" style="color:red">
              Senhas não correspondem</p>

            <v-col>
              <div slot="label" class="d-flex justify-content-between align-items-end">
                <div class="d-flex align-content-center">
                  <v-btn type="submit" variant="primary"
                    :disabled="!model.NovaSenha || (model.NovaSenha != confirmacaoSenha)">Confirmar Senha</v-btn>
                </div>
                <router-link to="/" class="d-block small">Voltar ao login</router-link>
              </div>
            </v-col>
          </form>

          <div v-show="msgResetError">
            <div class="alert alert-danger">{{ msgResetError }}</div>
            <br />
            <router-link to="/lembrar" class="d-block small">Recuperação de senha</router-link>
          </div>

          <div v-show="msgResetSuccess">
            <div class="alert alert-success">{{ msgResetSuccess }}</div>
            <br />
            <router-link to="/" class="d-block small">Voltar ao login</router-link>
          </div>
        </div>
      </v-card>
    </div>
  </div>
</template>

<!--<style src="@/vendor/styles/pages/authentication.scss" lang="scss"></style>
<style src="@/vendor/libs/spinkit/spinkit.scss" lang="scss"></style>-->

<script>
export default {
  name: 'recuperar',
  metaInfo: {
    title: 'Portfolio - Recuperação'
  },
  data: () => ({
    model: { NovaSenha: '', Login: '', Token: '', CodigoUsuario: '' },
    confirmacaoSenha: '',
    msgResetSuccess: '',
    msgResetError: '',
    validToken: false
  }),
  methods: {
    recuperarSenha() {
      const th = this
      th.$api.UI.ShowLoading()
      th.$api.Usuario.RecuperarSenha(th.model)
        .then(function (response) {
          th.$api.UI.HideLoading()
          if (!response) {
            th.msgResetSuccess = 'Senha alterada com sucesso.'
          } else th.msgResetError = response.message
        })
        .catch(err => console.log(err))
    },
    validaToken() {
      const th = this

      th.$api.UI.ShowLoading()
      th.$api.Usuario.ValidaTokenSenha(th.model.Token)
        .then(function (response) {
          th.$api.UI.HideLoading()
          if (response.message) {
            th.msgResetError = 'O link para recuperação de senha expirou, é possível solicitar um novo link a partir da recuperação de senha'
          } else th.validToken = true
        })
        .catch(err => console.log(err))
    }
  },
  mounted: function () {
    var params = this.$route.query
    this.model.Login = params.e
    this.model.CodigoUsuario = params.c
    this.model.Token = params.t

    if (params.t) this.validaToken()
    else this.$router.push({ path: '/' })
  }
}
</script>

<style></style>
