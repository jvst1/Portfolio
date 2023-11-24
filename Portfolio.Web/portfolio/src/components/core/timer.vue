<template>
  <div>
    <v-menu offset-y>
      <template v-slot:activator="{ on, attrs }">
        <v-btn text tile class="text-subtitle-2" color="default" v-bind="attrs" v-on="on">
          {{ tempoRestanteSessao }}
          <v-icon class="pl-2" :color="clockColor">mdi-timer-outline</v-icon>
        </v-btn>
      </template>

      <v-list dense>
        <v-list-item link @click="refreshToken">
          <v-list-item-title><v-icon class="pr-2" color="default">mdi-refresh-circle</v-icon>Renovar
            Sessão</v-list-item-title>
        </v-list-item>
      </v-list>
    </v-menu>
  </div>
</template>

<script>
export default {
  data() {
    return {
      tempoRestanteSessao: "",
      tempo: 0,
      clockColor: "green",
      user: null,
      msg: "Deseja renovar a sessão?",
    };
  },
  methods: {
    startCountdown() {
      const th = this;
      th.setExpirationTime();
      if (th.tempo > 0) {
        let min = parseInt(th.tempo / 60);
        let seg = th.tempo % 60;

        if (min < 10) {
          min = "0" + min;
          min = min.substr(0, 2);
          if (min === 0) th.clockColor = "amber";
          else th.clockColor = "green";
        }
        if (seg <= 9) seg = "0" + seg;

        th.tempoRestanteSessao = min + ":" + seg;

        setTimeout(function () {
          th.startCountdown();
        }, 1000);
      } else {
        th.clockColor = "red";
        th.tempoRestanteSessao = "00:00";
        th.$api.UI.Confirmacao(
          "info",
          "Deseja renovar a sessão?",
          th.refreshToken,
          th.logout,
          "Sessão expirada"
        );
      }
    },
    setExpirationTime() {
      const status = localStorage.getItem("status");
      this.user = this.$store.getters.user;
      if (!this.$store.getters.useOIDCAuth) {
        if (status) {
          if (status === "loggedIn") {
            while (!this.user) {
              this.user = JSON.parse(localStorage.getItem("user"));
            }

            const dataExpiracao = new Date(this.user.expiresAt);
            const dataAtual = new Date();
            this.tempo = Math.trunc(
              (dataExpiracao.getTime() - dataAtual.getTime()) / 1000
            );
          }
        }
      } else {
        const dataExpiracao = new Date(this.user.expires_at * 1000);
        const dataAtual = new Date();
        this.tempo = Math.trunc(
          (dataExpiracao.getTime() - dataAtual.getTime()) / 1000
        );
      }
    },
    logout() {
      this.$api.Authentication.Logout();
    },
    refreshToken() {
      const th = this;
      th.$api.Authentication.RefreshToken(th.user.refreshToken)
        .then(function (data) {
          const postLoad = th.$api.UI.PostLoadData(data);
          if (postLoad.success) {
            if (data.accessToken || data.access_token) {
              if (data.accessToken) {
                th.$store.dispatch("setUserData", {
                  user: {
                    email: data.email,
                    token: data.accessToken,
                    refreshToken: data.refreshToken,
                    roles: th.getUserRole(data.accessToken),
                    expiresAt: new Date(
                      new Date().getTime() + data.expires * 1000
                    ),
                    identificador: data.identificador,
                  },
                });
              } else if (data.access_token) {
                const userAux = th.$store.getters.user;
                th.$store.dispatch("setUserData", {
                  user: {
                    email: userAux.email,
                    token: data.access_token,
                    refreshToken: userAux.refreshToken,
                    roles: userAux.roles,
                    expiresAt: new Date(
                      new Date().getTime() + data.expires_in * 1000
                    ),
                    identificador: data.identificador,
                  },
                });
              }

              th.user = data;

              if (th.tempo === 0) {
                setTimeout(function () {
                  th.startCountdown();
                }, 500);
              }

              const dataExpiracao = new Date(
                new Date().getTime() + data.expires * 60
              );
              const dataAtual = new Date();
              th.tempo = Math.trunc(
                (dataExpiracao.getTime() - dataAtual.getTime()) / 1000
              );
            } else {
              th.$api.UI.ShowError("Erro", "Não foi possível renovar a sessão");
              setTimeout(th.logout(), 3000);
            }
          } else {
            th.$api.UI.ShowError("Erro", "Não foi possível renovar a sessão");
            setTimeout(th.logout(), 3000);
          }
        })
        .catch((err) => console.log(err));
    },
    getUserRole(jwtToken) {
      const jwtData = jwtToken.split(".")[1];
      const decodedJwtJsonData = window.atob(jwtData);
      return JSON.parse(decodedJwtJsonData).role;
    },
  },
  mounted: function () {
    this.setExpirationTime();
    if (this.tempo > 0) this.startCountdown();
  },
};
</script>
