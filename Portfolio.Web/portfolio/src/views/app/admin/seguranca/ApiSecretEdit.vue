<template>
  <div>
    <modal-form v-model="show" :submit="submit" textClose="Fechar" size="lg">
      <template v-slot:title>
        {{ model.id ? "Editar" : "Novo" }} Api Secret
      </template>
      <template v-slot:body>
        <v-row>
          <v-col class="col">
            <input-text label="Descricao" required v-model="model.description" />
          </v-col>
        </v-row>

        <v-row>
          <v-col class="col">
            <input-text label="Valor" required v-model="model.value" />
          </v-col>
        </v-row>

        <v-row>
          <v-col sm="4">
            <input-date v-model="model.expiration" label="Dt. Expiração"></input-date>
          </v-col>
        </v-row>
      </template>
    </modal-form>
  </div>
</template>

<script>
export default {
  props: {
    idResource: Number
  },
  data() {
    return {
      show: false,
      model: {}
    }
  },
  methods: {
    closeModal() {
      this.model = { expiration: null }
      this.show = false
    },
    openModal(sender) {
      this.show = true
      if (sender) {
        this.model.expiration = new Date(
          sender.nroAno,
          sender.nroMes - 1,
          sender.nroDia
        )
        this.model = sender
      }
      this.model.apiResourceId = this.idResource
    },
    submit() {
      const th = this
      th.$api.ApiResourceSecrets.Persist(th.model, 'id').then(function (response) {
        if (th.$api.UI.PostAction(response)) {
          th.show = false
          th.$emit('close', !response.error)
        }
      })
    }
  }
}
</script>
