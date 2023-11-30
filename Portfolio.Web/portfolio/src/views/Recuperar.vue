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
                  <h5 class="text--disabled font-weight-light">Cadastrar Senha</h5>
                </v-card-title>

                <v-card-text>
                  <v-form v-show="!msgResetSuccess && !msgResetError && validToken" @submit.prevent="recuperarSenha">
                    <v-col>
                      <input-text label="E-mail" v-model="model.Login" :disabled="true" />
                    </v-col>
                    <v-col>
                      <input-text label="Nova Senha" type="password" v-model="model.NovaSenha" :disabled="!model.Login" />
                    </v-col>
                    <v-col>
                      <input-text label="Confirmar Senha" type="password" v-model="confirmacaoSenha"
                        :disabled="!model.Login" />
                    </v-col>
                    <v-col v-show="model.NovaSenha && confirmacaoSenha && (model.NovaSenha != confirmacaoSenha)">
                      <v-alert type="error" text>
                        Senhas não correspondem
                      </v-alert>
                    </v-col>
                    <v-col class="pt-0 pb-10">
                      <v-btn type="submit" color="primary"
                        :disabled="!model.NovaSenha || (model.NovaSenha != confirmacaoSenha)">Confirmar Senha</v-btn>
                      <router-link to="/" class="float-right font-weight-medium caption text-decoration-none">Voltar ao
                        login</router-link>
                    </v-col>
                  </v-form>

                  <v-alert v-show="msgResetError" type="error" class="mt-3" text icon="error">
                    {{ msgResetError }}
                    <router-link to="/lembrar" class="d-block small">Recuperação de senha</router-link>
                  </v-alert>

                  <v-alert v-show="msgResetSuccess" type="success" class="mt-3" text icon="success">
                    {{ msgResetSuccess }}
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

<script>
export default {
  name: 'recuperar',
  metaInfo: {
    title: 'Portfolio - Recuperação de senha'
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
