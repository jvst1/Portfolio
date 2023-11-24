<template>
  <div>
    <modal-form v-model="show" :showSubmit="false" textClose="Fechar" size="lg">
      <template v-slot:title>
        Api Scopes
      </template>
      <template v-slot:body>
        <core-grid modal flat ref="grid" :onRead="onRead" :schema="schema" :deleteOption="false"> </core-grid>
      </template>
    </modal-form>
    <ApiScopeEdit ref="editar" @close="response" :idResource="idResource"></ApiScopeEdit>
  </div>
</template>

<script>
import ApiScopeEdit from './ApiScopeEdit'
export default {
  components: {
    ApiScopeEdit
  },
  data() {
    return {
      show: false,
      idResource: null,
      schema: [
        { value: 'id' },
        { value: 'name', text: 'Nome' },
        { value: 'displayName', text: 'Nome Exibição' }
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
        th.$api.ApiScope.GetAll(dto).then(function (data) {
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
