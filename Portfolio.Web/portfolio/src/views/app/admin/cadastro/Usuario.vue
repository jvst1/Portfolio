<template>
  <div>
    <core-grid ref="grid" :onRead="onRead" :schema="schema"></core-grid>
    <UsuarioAddEdit ref="editar" @close="response"></UsuarioAddEdit>
  </div>
</template>

<script>
import UsuarioAddEdit from './UsuarioAddEdit'

export default {
  components: {
    UsuarioAddEdit
  },
  data() {
    return {
      schema: [
        { value: 'nome', text: 'Nome' },
        { value: 'email', text: 'E-mail' },
        { value: 'identificador', text: 'Identificador', },
        { value: 'telefoneCelular', text: 'Telefone', format: 'phone' },
        { value: 'documentoFederal', text: 'Documento Federal', format: 'documento' }
      ]
    }
  },
  methods: {
    onRead: function (options, search) {
      const dto = { parameters: [{ name: 'search', value: search }] }
      this.$api.Usuario.GetAll(dto).then(function (response) {
        options.success(response)
      })
    },
    excluir(current) {
      const th = this
      if (current) {
        th.$api.Usuario.Delete(current.codigo).then(function (data) {
          if (th.$api.UI.PostAction(data, `Usuário "${current.identificador}" removido com sucesso.`)) {
            th.refresh()
          }
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
