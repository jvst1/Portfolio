<template>
    <modal-form v-model="show" textClose="Fechar" :size="size" ref="carrinho" :textSubmit="'Enviar pedido'"
        :submit="submit">
        <template v-slot:title>
            {{ title }}
            <br />
            <span class="caption pt-1 ml-3">{{ caption }}</span>
        </template>
        <template v-slot:body>
            <core-grid :onRead="onRead" :schema="schema" :items="items" :pageSize="-1" :addOption="false"
                :editOption="false" :deleteOption="false" :searchDisabled="true" :refreshOption="false"></core-grid>

            <v-select label="Endereço de entrega" :items="enderecos" item-text="name" item-value="value"
                v-model="selectedEndereco">
                <template v-slot:item="data">
                    <div>
                        <div class="primary--text">{{ data.item.title }}</div>
                        <div class="grey--text text--darken-1">{{ data.item.subtitle }}</div>
                    </div>
                </template>
            </v-select>
            <br />
            <div>Selecione o método de pagamento:</div>
            <v-btn-toggle v-model="selectedMetodoPagamento" mandatory>
                <v-btn value="Credito">Cartão de crédito</v-btn>
                <v-btn value="Debito">Cartão de débito</v-btn>
                <v-btn value="Vale Refeicao">Vale Refeição</v-btn>
            </v-btn-toggle>
        </template>
    </modal-form>
</template>
<script>
export default {
    props: {
        title: { type: String, default: "Finalizar pedido" },
        caption: { type: String, default: "" },
        size: { default: "lg" },
    },
    data() {
        return {
            show: false,
            items: [],
            selectedEndereco: null,
            selectedMetodoPagamento: null,
            enderecos: [],
            schema: [
                { text: 'Nome', value: 'product.nome' },
                { text: 'Quantidade', value: 'quantity' },
                { text: 'Preço (un)', value: 'aplicarDesconto' },
                { text: 'Preço (Total)', value: 'totalPrice' },
            ],
        };
    },
    methods: {
        openModal() {
            this.show = true;
            this.initUsuarioEndereco();
        },
        onRead: function (options) {
            const carrinho = this.$store.getters.carrinho;
            this.items = [];
            carrinho.items.map(item => {
                this.$api.Cardapio.Get(carrinho.codigoComerciante, item.codigo).then((response) => {
                    this.items.push({ product: { ...response }, quantity: item.quantidade });
                });
            });
            options.success(this.items);
        },
        closeModal() {
            this.show = false;
        },
        submit() {
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
  