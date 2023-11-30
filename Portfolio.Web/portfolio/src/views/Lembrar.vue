<template>
  <v-app id="inspire">
    <v-main>
      <v-container class="fill-height pa-0" fluid>
        <v-col offset-sm="1" sm="10">
          <v-row align="center" justify="center">
            <v-col cols="12" sm="8" md="4">

              <v-card class="elevation-12">
                <v-col class="d-flex justify-center">
                  <v-avatar size="100" tile><v-img src="@/assets/logo.png" width="100%" contain /></v-avatar>
                </v-col>

                <v-card-title class="d-flex justify-center pt-0">
                  <h5 class="text--disabled font-weight-light">Recuperação de Senha</h5>
                </v-card-title>

                <v-card-text>
                  <v-form v-show="!msgRecuperacaoSuccess" @submit.prevent="recuperarSenha">
                    <v-col>
                      <input-text label="E-mail" v-model="model.Login" autofocus placeholder=" " @keypress="clearMsgs"/>
                    </v-col>
                    <v-col class="pt-0 pb-10">
                      <v-btn type="submit" :disabled="!model.Login" color="primary">Enviar Recuperação de Senha</v-btn>
                      <router-link to="/" class="float-right font-weight-medium caption text-decoration-none">Voltar ao login</router-link>
                    </v-col>
                  </v-form>

                  <v-alert v-show="msgRecuperacaoError" type="error" class="mt-3" text icon="error">
                    {{ msgRecuperacaoError }}
                  </v-alert>

                  <v-alert v-show="msgRecuperacaoSuccess" type="success" class="mt-3" text icon="success">
                    {{ msgRecuperacaoSuccess }}
                    <router-link to="/" class="d-block small">Voltar ao login</router-link>
                  </v-alert>
                </v-card-text>
              </v-card>
            </v-col>
          </v-row>
        </v-col>
      </v-container>
    </v-main>
  </v-app>
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
