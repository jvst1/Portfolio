<template>
  <div>
    <modal-form v-model="show" :submit="submit">
      <template v-slot:title>
        {{ model.codigo ? "Editar" : "Novo" }} Cliente
      </template>
      <template v-slot:body>
        <!--nome & idCliente-->
        <v-row>
          <v-col class="col">
            <input-text label="Nome Cliente" v-model="model.clientName" :maxlength="400" required />
          </v-col>
          <v-col class="col">
            <input-text label="Id Cliente" v-model="model.clientId" :maxlength="400" required />
          </v-col>
        </v-row>

        <!--descricao-->
        <v-row>
          <v-col class="col">
            <input-text label="Descrição" v-model="model.description" required />
          </v-col>
        </v-row>

        <!--ativo-->
        <v-row v-if="model.codigo ? true : false">
          <v-col>
            <v-switch v-model="model.enabled" label="Ativo"></v-switch>
          </v-col>
        </v-row>

        <!-- Usuarios -->
        <v-row>
          <v-col class="col">
            <input-select label="Usuario" v-model="model.codigoUsuario" single :options="usuarios" required />
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
      usuarios: []
    }
  },
  methods: {
    openModal(sender) {
      this.show = true
      if (sender) {
        this.model = sender
      }
    },
    submit() {
      const th = this
      th.$api.Clients.Persist(th.model).then(function (response) {
        if (th.$api.UI.PostAction(response)) {
          th.show = false
          th.$emit('close', !response.error)
        }
      })
    },
    loadComboUsuarios() {
      const th = this
      th.$api.Usuario.Combo().then(function (data) {
        if (Array.isArray(data)) {
          data.forEach(function (item) {
            th.usuarios.push(item)
          })
        }
      })
    }
  },
  beforeMount: function () {
    this.loadComboUsuarios()
  }
}
</script>
