<template>
  <div>
    <modal-form v-model="show" :submit="submit" size="md">
      <template v-slot:title>{{ model.codigo ? "Editar" : "Novo" }} Recurso</template>
      <template v-slot:body>

        <v-row>
          <v-col cols="12">
            <input-text label="Nome" v-model="model.name" :maxlength="200" required />
          </v-col>
          <v-col cols="12">
            <input-text label="Nome Exibição" v-model="model.displayName" :maxlength="200" required />
          </v-col>
        </v-row>

        <!--descricao-->
        <v-row>
          <v-col cols="12">
            <input-text label="Descrição" v-model="model.description" />
          </v-col>
        </v-row>

        <!--ativo & nonEditable-->
        <v-row v-if="model.codigo ? true : false">
          <v-col cols="6">
            <v-switch v-if="model.codigo ? true : false" v-model="model.enabled" label="Ativo"></v-switch>
          </v-col>
          <v-col cols="6">
            <v-switch v-if="model.codigo ? true : false" v-model="model.nonEditable" label="Não Editavel"></v-switch>
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
      this.model = {}
      if (sender) {
        this.model = sender
      }
    },
    submit() {
      const th = this
      th.$api.ApiResources.Persist(th.model).then(function (response) {
        if (th.$api.UI.PostAction(response)) {
          th.show = false
          th.$emit('close', !response.error)
        }
      })
    }
  }
}
</script>
