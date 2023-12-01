<template>
    <v-dialog v-model="show" :max-width="width" persistent>
        <v-card>
            <v-card-title class="headline">
                {{ title }}
                <span class="caption pt-1 ml-3">{{ caption }}</span>
            </v-card-title>
            <v-card-text>
                <v-simple-table>
                    <template v-slot:default>
                        <thead>
                            <tr>
                                <th class="text-left">Nome</th>
                                <th class="text-left">Quantidade</th>
                                <th class="text-left">Preço (un)</th>
                                <th class="text-left">Preço (Total)</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="item in items" :key="item.product.id">
                                <td>{{ item.product.nome }}</td>
                                <td>{{ item.quantidade }}</td>
                                <td>{{ formatCurrency(item.product.aplicarDesconto ? item.product.valorDescontoFixo :
                                    item.product.valorProduto) }}</td>
                                <td>{{ formatCurrency(item.product.aplicarDesconto ? item.product.valorDescontoFixo *
                                    item.quantidade : item.product.valorProduto * item.quantidade) }}</td>
                            </tr>
                        </tbody>
                    </template>
                </v-simple-table>

                <v-select label="Endereço de entrega" :items="enderecos" item-text="name" item-value="value"
                    v-model="selectedEndereco" class="my-4">
                    <template v-slot:item="data">
                        <div>
                            <div class="primary--text">{{ data.item.title }}</div>
                            <div class="grey--text text--darken-1">{{ data.item.subtitle }}</div>
                        </div>
                    </template>
                </v-select>

                <div class="mb-2">Selecione o método de pagamento:</div>
                <v-btn-toggle v-model="selectedMetodoPagamento" mandatory class="mb-4">
                    <v-btn value="CartaoCredito">Cartão de crédito</v-btn>
                    <v-btn value="CartaoDebito">Cartão de débito</v-btn>
                    <v-btn value="ValeRefeicao">Vale Refeição</v-btn>
                </v-btn-toggle>
            </v-card-text>

            <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="green darken-1" text @click="closeModal">Fechar</v-btn>
                <v-btn color="blue darken-1" text @click="submit" :disabled="isSubmitDisabled">Enviar pedido</v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script>
export default {
    props: {
        title: { type: String, default: "Finalizar pedido" },
        caption: { type: String, default: "" },
        size: { default: "md" },
        codigoComerciante: { type: String },
    },
    data() {
        return {
            show: false,
            items: [],
            selectedEndereco: null,
            selectedMetodoPagamento: null,
            valorTotal: 0,
            enderecos: [],
            schema: [
                { text: 'Nome', value: 'product.nome' },
                { text: 'Quantidade', value: 'quantidade' },
                { text: 'Preço (un)', template: p => p.product.aplicarDesconto ? p.product.valorDescontoFixo : p.product.valorProduto },
                { text: 'Preço (Total)', template: p => p.product.aplicarDesconto ? p.product.valorDescontoFixo * p.quantidade : p.product.valorProduto * p.quantidade },
            ],
        };
    },
    computed: {
        width: {
            get() {
                switch (this.size) {
                    case "sm":
                        return "500px";
                    case "md":
                        return "700px";
                    case "lg":
                        return "900px";
                    case "xl":
                        return "97%";
                    default:
                        return "500px";
                }
            },
            set() { },
        },
        heigth: {
            get() {
                return "90%"
            },
            set() { },
        },
        isSubmitDisabled() {
            return !this.selectedEndereco || !this.selectedMetodoPagamento;
        },
    },
    methods: {
        openModal() {
            this.show = true;
            this.initUsuarioEndereco();
            this.initProdutosCarrinho()
        },
        async initProdutosCarrinho() {
            const carrinho = this.$store.getters.carrinho;
            let promises = carrinho.items.map(item => {
                return this.$api.Cardapio.Get(carrinho.codigoComerciante, item.codigo).then(response => {
                    return { product: { ...response }, quantidade: item.quantidade, comentario: item.comentario };
                });
            });

            let resolvedItems = await Promise.all(promises);
            this.items = resolvedItems;

            this.valorTotal = 0;
            this.items.forEach(item => {
                if (item.product.aplicarDesconto)
                    this.valorTotal += item.product.valorDescontoFixo * item.quantidade;
                else
                    this.valorTotal += item.product.valorProduto * item.quantidade;
            });
        },
        closeModal() {
            this.show = false;
        },
        submit() {
            let user = this.$store.getters.user;
            const dto = {
                codigoCliente: user.codigoUsuario,
                codigoClienteEndereco: this.selectedEndereco.codigo,
                codigoComerciante: this.codigoComerciante,
                valor: this.valorTotal,
                formaPagamento: this.selectedMetodoPagamento,
                produtos: []
            };
            this.items.forEach(item => {
                dto.produtos.push({ codigoProduto: item.product.codigo, nome: item.product.nome, quantidade: item.quantidade, comentario: item.comentario });
            });

            this.$api.Pedido.Post(this.codigoComerciante, dto).then((response) => {
                if (!response.error)
                    this.$api.UI.ShowSuccess("Novo pedido incluido para o estabelecimento comercial");
                else
                    this.$api.UI.ShowError(response)
            });
        },
        formatCurrency(value) {
            return this.$api.UI.Format.Currency(value);
        },
        initUsuarioEndereco() {
            this.enderecos = [];
            const dto = { parameters: [{ name: 'CodigoUsuario', value: this.$store.getters.user.codigoUsuario }] }

            this.$api.UsuarioEndereco.GetAll(dto).then((response) => {
                response.map(item => {
                    this.enderecos.push({
                        value: item,
                        name: `${item.logradouro}, ${item.nroLogradouro}, ${item.bairro}, ${item.cidade}, ${item.uf}`,
                        title: `${item.logradouro}, ${item.nroLogradouro}, ${item.bairro}, ${item.cidade}, ${item.uf}`,
                        subtitle: `${this.$api.UI.Format.Cep(item.cep)} - Endereço principal: ${item.enderecoPrincipal ? 'Sim' : 'Não'}`
                    })
                })
            })
        },
    },
};
</script>
  