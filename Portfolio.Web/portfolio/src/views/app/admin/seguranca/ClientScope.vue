<template>
  <div>
    <modal-form v-model="show" :showSubmit="false" textClose="Fechar" size="lg">
      <template v-slot:title>
        Client Scopes
      </template>
      <template v-slot:body>
        <core-grid modal flat ref="grid" :onRead="onRead" :editOption="false" :schema="schema">
        </core-grid>
      </template>
    </modal-form>
    <ClientScopeEdit ref="editar" @close="response" :clientId="clientId"></ClientScopeEdit>
  </div>
</template>

<script>
import ClientScopeEdit from './ClientScopeEdit'
export default {
  components: {
    ClientScopeEdit
  },
  data() {
    return {
      show: false,
      clientId: null,
      schema: [
        { value: 'scope', text: 'Scope' }
      ]
    }
  },
  methods: {
    openModal: function (clientId) {
      this.show = true
      this.clientId = clientId
    },
    onRead: function (options, search) {
      const th = this
      if (th.clientId) {
        const dto = {
          parameters: [
            { name: 'clientId', value: th.clientId },
            { name: 'search', value: search }
          ]
        }
        th.$api.ClientScopes.GetAll(dto).then(function (data) {
          options.success(data)
        })
      }
    },
    excluir(current) {
      const th = this
      th.$api.ClientScopes.Delete(current.id).then(function () {
        th.refresh()
      })
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
