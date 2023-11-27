<template>
    <v-container fluid>
        <core-grid ref="grid" :onRead="onRead" :schema="schema"></core-grid>
        <ClienteEnderecoAddEdit ref="editar" @close="response"></ClienteEnderecoAddEdit>
    </v-container>
</template>
  
<script>
export default {
    name: "AddressPage",
    components: {
        ClienteEnderecoAddEdit: () => import('./ClienteEnderecoAddEdit')
    },
    data() {
        return {
            schema: [
                { value: 'logradouro', text: 'Logradouro' },
                { value: 'nroLogradouro', text: 'Número Logradouro' },
                { value: 'complemento', text: 'Complemento' },
                { value: 'bairro', text: 'Bairro' },
                { value: 'cidade', text: 'Cidade' },
                { value: 'uf', text: 'Estado' },
                { value: 'cep', text: 'CEP' },
                { value: 'enderecoPrincipal', text: 'Endereco Principal', format: 'boolean' },
            ]
        }
    },
    methods: {
        onRead: function (options, search) {
            const dto = { parameters: [{ name: 'CodigoUsuario', value: this.$store.getters.user.codigoUsuario }, { name: 'search', value: search }] }

            this.$api.UsuarioEndereco.GetAll(dto).then((response) => {
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
                this.$api.UsuarioEndereco.Delete(current.codigo).then((data) => {
                    if (this.$api.UI.PostAction(data, `Endereço removido com sucesso.`)) {
                        this.refresh()
                    }
                })
            }
        },
    },
};
</script>