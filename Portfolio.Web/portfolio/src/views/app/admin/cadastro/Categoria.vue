<template>
    <div>
        <core-grid ref="grid" :onRead="onRead" :schema="schema"></core-grid>
        <CategoriaAddEdit ref="editar" @close="response"></CategoriaAddEdit>
    </div>
</template>
  
<script>
import CategoriaAddEdit from './CategoriaAddEdit'

export default {
    data() {
        return {
            schema: [
                { value: 'nome', text: 'Nome' },
                { value: 'imageUrl', text: 'URL Imagem' },
                { value: 'dtInclusao', text: 'Data de inclusão', format: 'datetime' },
                { value: 'dtAlteracao', text: 'Data de alteração', format: 'datetime' },
            ]
        }
    },
    components: {
        CategoriaAddEdit
    },
    methods: {
        onRead: function (options, search) {
            const dto = { parameters: [{ name: 'search', value: search }] }
            this.$api.Categoria.GetAll(dto).then((response) => {
                options.success(response)
            })
        },
        excluir(current) {
            const th = this
            if (current) {
                th.$api.Categoria.Delete(current.codigo).then((data) => {
                    if (th.$api.UI.PostAction(data, `Categoria "${current.nome}" removida com sucesso.`)) {
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
  