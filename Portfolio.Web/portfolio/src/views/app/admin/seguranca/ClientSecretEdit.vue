<template>
  <div>
    <modal-form v-model="show" :submit="submit">
      <template v-slot:title>
        {{ model.id ? "Editar" : "Novo" }} Client Secret
      </template>
      <template v-slot:body>
        <!--descricao-->
        <v-row>
          <v-col class="col">
            <input-text label="Descrição" required v-model="model.description" />
          </v-col>
        </v-row>

        <!--value-->
        <v-row>
          <v-col class="col">
            <input-text label="Valor" v-model="model.value" disabled />
          </v-col>
        </v-row>

        <!--PEM-->
        <v-row>
          <v-col class="col">
            <input-file label="Arquivo" required v-model="model.filePEM" accept=".pem" :change="readFile"
              placeholder="Insira um arquivo .pem"></input-file>
          </v-col>
        </v-row>
      </template>
    </modal-form>
  </div>
</template>

<script>
export default {
  data() {
    return {
      show: false,
      model: {}
    }
  },
  methods: {
    openModal(sender) {
      this.show = true
      if (sender) {
        this.model = sender
        this.model.expiration = new Date(
          sender.nroAno,
          sender.nroMes - 1,
          sender.nroDia
        )
      } else { this.model.clientId = this.clientId }
    },
    readFile(file) {
      const th = this
      th.model.chavePEM = null
      if (file) {
        const reader = new FileReader()
        reader.addEventListener('load', (event) => {
          th.model.chavePEM = event.target.result
        })
        reader.readAsText(file)
      }
    },
    submit() {
      const th = this

      th.$api.ClientSecrets.Persist(th.model).then(function (response) {
        if (th.$api.UI.PostAction(response)) {
          th.show = false
          th.$emit('close', !response.error)
        }
      })
    },
    validate() {
      if (this.model.filePEM.type !== 'text/plain' && !this.model.filePEM.name.toLowerCase().includes('.pem')) {
        this.$api.UI.ShowError('Erro', 'Formato do arquivo é inválido')
        return false
      }
      return true
    },
    fileDeleted: function () {
      console.log(this.$refs.vueFileAgent)
    }
  },
  props: {
    clientId: Number
  }
}
</script>
