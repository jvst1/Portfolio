<template>
  <div>
    <modal-form v-model="show" :submit="submit">
      <template v-slot:title>
        {{ model.id ? "Editar" : "Novo" }} Client Scope
      </template>
      <template v-slot:body>
        <v-row>
          <v-col class="col">
            <input-multiselect label="Scope" v-model="model.scopes" required :options="scopes" />
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
      model: {},
      scopes: []
    }
  },
  methods: {
    openModal(sender) {
      this.loadScopes()
      this.show = true
      if (sender) { this.model = Object.assign(this.model, sender) }
    },
    submit() {
      const th = this

      th.model.clientId = th.clientId
      th.model.scopes = JSON.stringify(th.model.scopes)
      th.$api.ClientScopes.Post(th.model).then(function (response) {
        if (th.$api.UI.PostAction(response)) {
          th.show = false
          th.$emit('close', !response.error)
        }
      })
    },
    loadScopes() {
      const th = this
      th.scopes = []

      th.$api.ApiScope.GetAllByClientID(th.clientId).then(function (data) {
        if (Array.isArray(data)) {
          data.forEach(function (item) {
            const scope = {
              value: item.name,
              text: item.name
            }
            th.scopes.push(scope)
          })
        }
      })
    }
  },
  props: {
    clientId: Number
  }
}
</script>
