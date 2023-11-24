<template>
  <div>
    <v-card class="mb-4">
      <core-grid ref="grid" :onRead="onRead" :schema="schema" :deleteOption="false">
      </core-grid>
    </v-card>
    <ClientEdit ref="editar" @close="response"></ClientEdit>
    <ClientScope ref="modalScopes" :clientId="current"></ClientScope>
    <ClientSecret ref="modalSecrets" :clientId="current"></ClientSecret>
  </div>
</template>

<script>
import ClientEdit from './ClientEdit'
import ClientScope from './ClientScope'
import ClientSecret from './ClientSecret'

export default {
  components: {
    ClientEdit,
    ClientScope,
    ClientSecret
  },
  data() {
    return {
      current: null,
      schema: [
        { value: 'id' },
        { value: 'clientId', text: 'Id Cliente' },
        { value: 'clientName', text: 'Nome Cliente' },
        { value: 'description', text: 'Descrição' },
        { value: 'created', text: 'Dt. Inclusão', format: 'datetime' },
        { value: 'enabled', text: 'Ativo', format: 'boolean' },
        { format: 'button', title: 'Scopes', action: this.openClientScopes },
        { format: 'button', title: 'Secrets', action: this.openClientSecrets }
      ]
    }
  },
  methods: {
    onRead: function (options, search) {
      var dto = { parameters: [{ name: 'search', value: search }] }

      this.$api.Clients.GetAll(dto).then(function (response) {
        options.success(response)
      })
    },
    openClientScopes(e, curr) {
      e.preventDefault()
      const th = this
      if (curr) {
        th.$refs.modalScopes.openModal(curr.id)
      }
    },
    openClientSecrets(e, curr) {
      e.preventDefault()
      const th = this
      if (curr) {
        th.$refs.modalSecrets.openModal(curr.id)
      }
    },
    refresh() {
      this.$refs.grid.refresh()
    },
    response(data) {
      if (data) { this.refresh() }
    }
  }
}
</script>
