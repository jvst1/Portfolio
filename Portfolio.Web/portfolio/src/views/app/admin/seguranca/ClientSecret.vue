<template>
  <div>
    <modal-form v-model="show" :showSubmit="false" textClose="Fechar" size="lg">
      <template v-slot:title>
        Client Secrets
      </template>
      <template v-slot:body>
        <core-grid modal flat ref="grid" :onRead="onRead" :schema="schema">
        </core-grid>
      </template>
    </modal-form>
    <ClientSecretEdit ref="editar" @close="response" :clientId="clientId"></ClientSecretEdit>
    <modal-details title="Valor" ref="detalhes"></modal-details>
  </div>
</template>

<script>
/* eslint-disable */
import ClientSecretEdit from './ClientSecretEdit'
export default {
  components: {
    ClientSecretEdit
  },
  data() {
    return {
      show: false,
      clientId: null,
      schema: [
        { value: 'id' },
        { value: 'description', text: 'Descrição' },
        { value: 'created', text: 'Dt. Inclusão', format: 'datetime' },
        { format: 'button', title: 'Valor', action: this.openDetalhes }
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
        th.$api.ClientSecrets.GetAll(dto).then(function (data) {
          options.success(data)
        })
      }
    },
    excluir(current) {
      const th = this
      if (current) {
        th.$api.ClientSecrets.Delete(current.id).then(function () {
          th.refresh()
        })
      }
    },
    openDetalhes(e, curr) {
      e.preventDefault()
      const th = this
      if (curr) {
        var json = { value: curr.value }
        try {
          json = JSON.parse(curr.value)
        } catch { }
        th.$refs.detalhes.openModal(json)
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
