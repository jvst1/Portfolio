<template>
  <div>
    <modal-form v-model="show" :showSubmit="false" textClose="Fechar" size="lg">
      <template v-slot:title>
        Api Secrets
      </template>
      <template v-slot:body>
        <core-grid modal flat ref="grid" :onRead="onRead" :deleteOption="false" :schema="schema">
        </core-grid>
      </template>
    </modal-form>
    <ApiSecretEdit ref="editar" @close="response" :idResource="idResource"></ApiSecretEdit>
  </div>
</template>

<script>
/* eslint-disable */
import ApiSecretEdit from './ApiSecretEdit'
export default {
  components: {
    ApiSecretEdit
  },
  props: {
    idResource: Number
  },
  data() {
    return {
      show: false,
      schema: [
        { value: 'description', text: 'Descrição' },
        { value: 'created', text: 'Dt. Inclusão', format: 'datetime' },
        { value: 'value', text: 'Valor' },
        { value: 'expiration', text: 'Dt. Expiração', format: 'date' }
      ]
    }
  },
  methods: {
    openModal(sender) {
      if (!sender || !sender.id) { this.$api.UI.Mensagem('alerta', 'ApiResource não informado!') } else {
        this.show = true
        this.idResource = sender.id
      }
    },
    onRead: function (options, search) {
      const th = this
      if (th.idResource) {
        const dto = {
          parameters: [
            { name: 'idResource', value: th.idResource },
            { name: 'search', value: search }
          ]
        }
        th.$api.ApiResourceSecrets.GetAll(dto).then(function (data) {
          options.success(data)
        })
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
