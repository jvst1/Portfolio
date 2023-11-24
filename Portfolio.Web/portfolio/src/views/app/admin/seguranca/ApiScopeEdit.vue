<template>
  <div>
    <modal-form v-model="show" :submit="submit" textClose="Fechar" size="lg">
      <template v-slot:title>
        {{ model.id ? "Editar" : "Novo" }} Api Scope
      </template>
      <template v-slot:body>
        <!--nome & nome exibição-->
        <v-row>
          <v-col class="col">
            <input-text label="Nome" v-model="model.name" required :maxlength="400" />
          </v-col>
          <v-col class="col">
            <input-text label="Nome Exibição" v-model="model.displayName" required :maxlength="400" />
          </v-col>
        </v-row>

        <!--descricao-->
        <v-row>
          <v-col class="col">
            <input-text label="Descricao" v-model="model.description" />
          </v-col>
        </v-row>

        <v-switch v-if="model.id ? true : false" v-model="model.emphasize" label="Emphasize"></v-switch>

        <v-switch v-if="model.id ? true : false" v-model="model.required" label="Required"></v-switch>

        <v-switch v-if="model.id ? true : false" v-model="model.showInDiscoveryDocument"
          label="ShowInDiscoveryDocument"></v-switch>
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
    openModal(sender) {
      this.show = true
      if (sender) {
        this.model = sender
      }
      this.model.apiResourceId = this.idResource
    },
    submit() {
      const th = this
      const api = th.$api

      api.ApiScope.Persist(th.model, 'id').then(function (response) {
        if (api.UI.PostAction(response)) {
          th.show = false
          th.$emit('close', !response.error)
        }
      })
    }
  }
}
</script>
