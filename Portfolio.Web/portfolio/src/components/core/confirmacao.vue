<template>
  <v-layout row justify-center>
    <v-dialog v-model="show" persistent :width="400" @click.stop="recuse">
      <v-card>
        <v-card-title v-bind:class="classe + '--text pa-4'" class="headline title">
          <v-icon :color="classe" class="mr-2">{{ icone }}</v-icon>{{ tituloTexto }}
          <div class="flex-grow-1"></div>
          <v-btn small icon @click="recuse">
            <v-icon small>mdi-close</v-icon>
          </v-btn>
        </v-card-title>
        <v-divider></v-divider>
        <v-card-text class="pa-5" v-html="mensagem"></v-card-text>
        <v-divider></v-divider>
        <v-card-actions>
          <v-spacer></v-spacer>

          <v-btn color="grey-lighten-5" depressed @click="recuse">
            {{ nao }}
          </v-btn>
          <v-btn :color="classe" depressed @click="confirm">
            {{ sim }}
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-layout>
</template>

<script>
export default {
  props: {
    tipo: { default: 'info' },
    titulo: String,
    mensagem: { default: 'Operacao realizada com sucesso!' },
    sim: { default: 'Sim' },
    nao: { default: 'Não' },
    callbackSim: Function,
    callbackNao: Function
  },
  data() {
    return {
      show: true
    }
  },
  methods: {
    confirm() {
      this.show = false
      this.$nextTick(function () {
        if (this.callbackSim) {
          this.callbackSim()
        }
      })
      setTimeout(() => this.$destroy(), 1000)
    },
    recuse() {
      this.show = false
      this.$nextTick(function () {
        if (this.callbackNao) {
          this.callbackNao()
        }
      })
      setTimeout(() => this.$destroy(), 1000)
    }
  },
  computed: {
    classe() {
      switch (this.tipo) {
        case 'info':
          return 'info'
        case 'alerta':
          return 'warning'
        case 'erro':
          return 'danger'
        default:
          return 'info'
      }
    },
    tituloTexto() {
      if (this.titulo) {
        return this.titulo
      } else {
        switch (this.tipo) {
          case 'info':
            return 'Mensagem'
          case 'alerta':
            return 'Atenção!'
          case 'erro':
            return 'Erro!'
          default:
            return 'Mensagem'
        }
      }
    },
    icone() {
      switch (this.tipo) {
        case 'info':
          return 'info'
        case 'alerta':
          return 'warning'
        case 'erro':
          return 'error'
        default:
          return 'info'
      }
    }
  }
}
</script>
