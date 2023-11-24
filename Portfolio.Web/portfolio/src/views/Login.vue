<template>
  <v-app id="inspire">
    <v-main>
      <v-container class="fill-height pa-0" fluid
        :style="`background-image: url('${publicUrl}assets/img/loginbg.jpg');background-position: center center; background-size: cover;`">
        <v-col offset-sm="1" sm="10">
          <v-row align="center" justify="center">
            <v-col cols="12" sm="8" md="4">

              <v-card class="elevation-12">
                <v-col class="d-flex justify-center">
                  <v-avatar size="100" tile><v-img src="@/assets/logo.png" width="100%" contain /></v-avatar>
                </v-col>

                <v-card-title class="d-flex justify-center pt-0">
                  <h5 class="text--disabled font-weight-light">Autenticação</h5>
                </v-card-title>
                <v-card-text>
                  <v-form @submit.prevent="login">
                    <v-col>
                      <input-text label="E-mail" v-model="credentials.email" autofocus placeholder=" " />
                    </v-col>
                    <v-col class="pb-1 pt-2">
                      <input-text label="Senha" type="password" v-model="credentials.password" placeholder=" " />
                    </v-col>
                    <v-col class="pt-0 pb-10">
                      <router-link to="/lembrar"
                        class="float-right font-weight-medium caption text-decoration-none">Esqueceu a
                        senha?</router-link>
                      <v-btn type="submit" :disabled="!credentials.email || !credentials.password" color="primary">
                        Entrar
                      </v-btn>
                      <v-alert v-show="error" type="error" class="mt-3" text icon="error">
                        {{ error }}
                      </v-alert>
                    </v-col>
                  </v-form>
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
  name: 'login',
  metaInfo: {
    title: 'Portfolio - Login'
  },
  data: () => ({
    credentials: {
      email: '',
      password: ''
    },
    error: ''
  }),
  methods: {
    login() {
      const email = this.credentials.email
      const password = this.credentials.password
      const th = this

      th.$api.Authentication.Authenticate(email, password)
        .then(function (data) {
          const postLoad = th.$api.UI.PostLoadData(data)
          if (postLoad.success) {
            if (data.accessToken) {
              const user = {
                codigoUsuario: data.codigoUsuario,
                email: email,
                token: data.accessToken,
                refreshToken: data.refreshToken,
                roles: th.getUserRole(data.accessToken),
                expiresAt: new Date(
                  new Date().getTime() + data.expires * 60000
                ),
                identificador: data.identificador
              }

              const theme = {
                placeholder: ' ',
                placeholderSelect: 'Selecione',
                placeholderMultiSelect: 'Nenhum',
                outlined: true,
                dense: true
              }

              th.$store
                .dispatch('setUserData', { user, theme })
                .then(() => {
                  th.$router.push({ path: '/home' })
                })
            } else {
              th.error = data.message
            }
          } else {
            th.error = postLoad.message
          }
        })
        .catch(err => console.log(err))
    },
    getUserRole(jwtToken) {
      const jwtData = jwtToken.split('.')[1]
      const decodedJwtJsonData = window.atob(jwtData)
      return JSON.parse(decodedJwtJsonData).role
    }
  }
}
</script>
