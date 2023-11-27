<template>
    <v-container fluid>
        <core-grid ref="grid" :onRead="onRead" :schema="schema"></core-grid>
        <ComercianteCardapioAddEdit ref="editar" @close="response"></ComercianteCardapioAddEdit>
    </v-container>
</template>
  
<script>
export default {
    name: "ComercianteCardapioPage",
    components: {
        ComercianteCardapioAddEdit: () => import('./ComercianteCardapioAddEdit')
    },
    data() {
        return {
            codigoComerciante: this.$store.getters.user.codigoUsuario,
            schema: [
                { value: 'nome', text: 'Nome do Produto' },
                { value: 'descricao', text: 'Descrição' },
                { value: 'valorProduto', text: 'Preço', format: 'currency' },
                { value: 'vendaAtiva', text: 'DisponÍvel para venda?', format: 'boolean' },
                { value: 'aplicarDesconto', text: 'Aplicar Desconto?', format: 'boolean' },
                { value: 'valorDescontoFixo', text: 'Preço com desconto', format: 'currency' },
                { value: 'valorDescontoPercentual', text: '% de desconto', template: p => p.valorDescontoPercentual + '%' },
            ]
        }
    },
    methods: {
        onRead: function (options, search) {
            const dto = { parameters: [{ name: 'CodigoUsuario', value: this.codigoComerciante }, { name: 'search', value: search }] }

            this.$api.Cardapio.GetAll(this.codigoComerciante, dto).then((response) => {
                if (response.error) {
                    this.$api.UI.ShowError("Erro", response.error);
                } else {
                    options.success(response)
                }
            })
        },
        refresh() {
            this.$refs.grid.refresh()
        },
        response(data) {
            if (data) { this.refresh() }
        },
        excluir(current) {
            if (current) {
                this.$api.Cardapio.Delete(this.codigoComerciante, current.codigo).then((data) => {
                    if (this.$api.UI.PostAction(data, `Item do cardápio "${current.nome}" removido com sucesso.`)) {
                        this.refresh()
                    }
                })
            }
        },
    },
};
</script>