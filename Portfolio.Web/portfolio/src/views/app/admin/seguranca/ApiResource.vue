<template>
  <div>
    <core-grid ref="grid" :onRead="onRead" :schema="schema" :deleteOption="false"></core-grid>
    <ApiResourceEdit ref="editar" @close="response"></ApiResourceEdit>
    <ApiScope ref="scopes"></ApiScope>
    <ApiSecret ref="secrets"></ApiSecret>
  </div>
</template>

<script>
import ApiResourceEdit from './ApiResourceEdit'
import ApiScope from './ApiScope'
import ApiSecret from './ApiSecret'

export default {
  components: {
    ApiResourceEdit,
    ApiScope,
    ApiSecret
  },
  data() {
    return {
      schema: [
        { value: 'name', text: 'Nome' },
        { value: 'displayName', text: 'Nome Exibição' },
        { value: 'enabled', text: 'Ativo', format: 'boolean' },
        { value: 'created', text: 'Dt. Inclusão', format: 'datetime' },
        { title: 'Scopes', action: this.openScopes, format: 'button' },
        { title: 'Secrets', action: this.openSecrets, format: 'button' }
      ]
    }
  },
  methods: {
    onRead: function (options, search) {
      const dto = { parameters: [{ name: 'search', value: search }] }
      this.$api.ApiResources.GetAll(dto).then(function (response) {
        options.success(response)
      })
    },
    openScopes(e, curr) {
      e.preventDefault()
      const th = this
      if (curr) { th.$refs.scopes.openModal(curr) }
    },
    openSecrets(e, curr) {
      e.preventDefault()
      const th = this
      if (curr) { th.$refs.secrets.openModal(curr) }
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
