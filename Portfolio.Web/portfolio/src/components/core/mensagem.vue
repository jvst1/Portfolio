<template>
  <v-layout row justify-center>
    <v-dialog v-model="show" persistent :width="400" @click.stop="close">
      <v-card>
        <v-card-title v-bind:class="classe + '--text pa-4'" class="headline title">
          <v-icon :color="classe" class="mr-2">{{ icone }}</v-icon>{{ tituloTexto }}
          <div class="flex-grow-1"></div>
          <v-btn small icon @click="close">
            <v-icon small>mdi-close</v-icon>
          </v-btn>
        </v-card-title>
        <v-divider></v-divider>
        <v-card-text class="pa-5" v-html="mensagemTexto"></v-card-text>
        <v-divider></v-divider>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn depressed :color="classe" @click="close">
            {{ ok }}
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-layout>
</template>

<script>
export default {
  props: {
    tipo: { default: "info" },
    titulo: String,
    mensagem: String,
    ok: { default: "Ok" },
    callback: Function,
  },
  data() {
    return {
      show: true,
    };
  },
  methods: {
    close() {
      this.show = false;
      this.$nextTick(function () {
        if (this.callback) {
          this.callback();
        }
      });
      setTimeout(() => this.$destroy(), 1000);
    },
  },
  computed: {
    classe() {
      switch (this.tipo) {
        case "info":
          return "info";
        case "alerta":
          return "warning";
        case "erro":
          return "danger";
        default:
          return "info";
      }
    },
    tituloTexto() {
      if (this.titulo) {
        return this.titulo;
      } else {
        switch (this.tipo) {
          case "info":
            return "Mensagem";
          case "alerta":
            return "Atenção!";
          case "erro":
            return "Erro!";
          default:
            return "Mensagem";
        }
      }
    },
    mensagemTexto() {
      if (this.mensagem) {
        return this.mensagem;
      } else {
        switch (this.tipo) {
          case "info":
            return "Operacao realizada com sucesso!";
          case "alerta":
            return "Operacao não permitida!";
          case "erro":
            return "Houve um erro! Tente novamente, se persistir contate o administrador.";
          default:
            return "Operacao realizada com sucesso!";
        }
      }
    },
    icone() {
      switch (this.tipo) {
        case "info":
          return "info";
        case "alerta":
          return "warning";
        case "erro":
          return "error";
        default:
          return "info";
      }
    },
  },
};
</script>
